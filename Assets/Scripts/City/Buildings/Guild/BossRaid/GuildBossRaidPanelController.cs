using City.Buildings.Guild.RecruitViews;
using City.Buildings.Guild.Utils;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
using System;
using System.Collections.Generic;
using System.Threading;
using City.Buildings.Abstractions;
using Fight;
using Fight.WarTable;
using Models.Arenas;
using Models.Fights.Campaign;
using Models.Guilds;
using Network.DataServer.Messages.Teams;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace City.Buildings.Guild.BossRaid
{
    public class GuildBossRaidPanelController : PanelWithFight<GuildBossRaidPanelView>
    {
        private const int BOSS_FREE_RAID_COUNT = 2;
        private const int BOSS_RAID_REFRESH_HOURS = 16;
        private const int BOSS_RAID_COST_STEP = 50;

        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IObjectResolver _diContainer;
        [Inject] private readonly WarTableController _warTableController;

        private CompositeDisposable m_raidDisposables;
        private GuildBossMission m_currentBossMission;

        private float m_createDamageSum;
        
        private TeamContainer _teamContainer;
        private RecruitData _myRecruitData;
        private Dictionary<int, RecruitProgressView> _recruitsView = new();
        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            View.BossRaidButton.OnClick.Subscribe(_ => OpenRaidMission()).AddTo(Disposables);
            base.Start();
        }

        protected override void OnLoadGame()
        {
            if (CommonGameData.Teams.TryGetValue(GetType().Name, out string _teamContainerJSON))
            {
                _teamContainer = _jsonConverter.Deserialize<TeamContainer>(_teamContainerJSON);
            }
            else
            {
                _teamContainer = new(GetType().Name);
            }

            base.OnLoadGame();
        }

        private void OpenRaidMission()
        {
            MissionModel missionModel = new();
            foreach (var unit in m_currentBossMission.BossModels)
            {
                missionModel.Units.Add(new HeroData(unit));
            }
            
            OpenMission(missionModel);
        }

        protected override void OnStartMission()
        {
            m_raidDisposables = new();
            FightController.AfterCreateFight.Subscribe(_ =>
            {
                ChangeUnitsData();
            }).AddTo(m_raidDisposables);
            
            base.OnStartMission();
        }

        private void ChangeUnitsData()
        {
            m_raidDisposables.Dispose();
            m_raidDisposables = new CompositeDisposable();
            m_createDamageSum = 0f;
            var container = CommonDictionaries.GuildBossContainers["MainBosses"];
            
            for (var i = 0; i < FightController.GetRightTeam.Count; i++)
            {
                var bossData = container.Missions[_guildData.CurrentBoss].BossModels[i];
                float maxHelth = bossData.Health.Mantissa * Mathf.Pow(10, bossData.Health.E10);
                float currentHealth = _guildData.BossHealthMantissa * Mathf.Pow(10, _guildData.BossHealthE10);
                FightController.GetRightTeam[i].heroController.Hero.ChangeHealth(maxHelth, currentHealth);
                FightController.GetRightTeam[i].heroController.Hero.OnGetDamage.Subscribe(AddDamage).AddTo(m_raidDisposables);
            }
        }

        private void AddDamage(float damage)
        {
            m_createDamageSum += damage;
        }

        protected override void OnResultFight(FightResultType result)
        {
            StartRaidBoss().Forget();
            base.OnResultFight(result);
        }

        private async UniTaskVoid StartRaidBoss()
        {
            var message = new RaidBossMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Damage = m_createDamageSum
            };

            m_createDamageSum = 0f;
            
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var newGuildData = _jsonConverter.Deserialize<GuildPlayerSaveContainer>(result);
                if (newGuildData != null)
                {
                    _resourceStorageController.SubtractResource(GetCostRaid());
                    CommonGameData.City.GuildPlayerSaveContainer = newGuildData;

                    var data = GetMyRecruitData();
                    data.CountRaidBoss += 1;
                    UpdateUi();
                }
            }
        }

        protected override void OnCloseWarTable()
        {
            m_raidDisposables?.Dispose();
            m_raidDisposables = null;
            base.OnCloseWarTable();
        }

        protected override void Show()
        {
            UpdateUi();
            base.Show();
        }

        private void UpdateUi()
        {
            if (_guildData == null)
                return;

            View.BossLevel.text = $"{_guildData.CurrentBoss + 1}";

            var container = CommonDictionaries.GuildBossContainers["MainBosses"];
            m_currentBossMission = container.Missions[_guildData.CurrentBoss].Clone();
            var bossData = m_currentBossMission.BossModels[0];
            var bossModel = CommonDictionaries.Heroes[bossData.HeroId];
            var bossHeroData = new HeroData { Level = bossData.Level, Rating = bossData.Rating };
            var gameBoss = new GameHero(bossModel, bossHeroData);
            View.BossImage.sprite = gameBoss.Avatar;
            
            var currentHealth = new BigDigit(_guildData.BossHealthMantissa, _guildData.BossHealthE10);
            var maxHealth = bossData.Health;

            var delta = currentHealth / maxHealth;
            var currentProgress = delta.Mantissa * Mathf.Pow(10, delta.E10);
            currentProgress = Mathf.Clamp01(currentProgress);

            View.BossHealthSlider.value = currentProgress;
            View.BossRaidButton.SetCost(GetCostRaid());

            var myRecruitData = GetMyRecruitData();
            if (myRecruitData != null && myRecruitData.CountRaidBoss > 0)
            {
                ShowRefreshTime();
            }
            
            var recruits = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits;
            recruits.Sort(new RecruitDamageComparer());
            var index = 0;
            foreach (var recruit in recruits)
            {
                if (recruit.ResultMantissa < 1f && recruit.ResultE10 == 0)
                {
                    continue;
                }

                RecruitProgressView prefab = null;
                if (_recruitsView.TryGetValue(recruit.PlayerId, out prefab))
                {
                    prefab.SetData(recruit, View.Scroll);
                }
                else
                {
                    prefab = UnityEngine.GameObject.Instantiate(View.RecruitProgressViewPrefab, View.Content);
                    _recruitsView.Add(recruit.PlayerId, prefab);
                    _diContainer.Inject(prefab);
                    prefab.SetData(recruit, View.Scroll);
                }

                if (prefab != null)
                {
                    prefab.transform.SetSiblingIndex(index);
                    index++;
                }
            }
        }

        private void ShowRefreshTime()
        {
            var recruit = GetMyRecruitData();
            if (recruit == null)
                return;

            if (!string.IsNullOrEmpty(recruit.DateTimeFirstRaidBoss))
            {
                var lastDateTime = TimeUtils.ParseTime(recruit.DateTimeFirstRaidBoss);

                var deltaTime = DateTime.UtcNow - lastDateTime;
                View.TimerRaidPanel.SetActive(deltaTime.TotalHours < BOSS_RAID_REFRESH_HOURS);
                View.TimeForRefreshRaid.text = FunctionHelp.TimeSpanConvertToSmallString(deltaTime);
            }
            else
            {
                View.TimerRaidPanel.SetActive(false);
            }
        }

        private GameResource GetCostRaid()
        {
            var recruit = GetMyRecruitData();
            if (recruit == null)
                return new GameResource();

            if (recruit.CountRaidBoss >= BOSS_FREE_RAID_COUNT)
            {
                var count = recruit.CountRaidBoss - BOSS_FREE_RAID_COUNT + 1;
                var result = new GameResource(ResourceType.Diamond, new BigDigit(count * BOSS_RAID_COST_STEP));
                return result;
            }
            else
            {
                return new GameResource(ResourceType.Diamond);
            }
        }

        private async UniTaskVoid SetDefenders(TeamContainer heroesIdsContainer)
        {
            var message = new ChangeTeamDefendersMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroesIdsContainer = _jsonConverter.Serialize(heroesIdsContainer),
                TeamsContainerName = GetType().Name
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }
        }
        
        private RecruitData GetMyRecruitData()
        {
            if (_myRecruitData == null)
            {
                _myRecruitData = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits
                    .Find(recruit => recruit.PlayerId == CommonGameData.PlayerInfoData.Id);
            }

            return _myRecruitData;
        }

        public override void Dispose()
        {
            m_raidDisposables?.Dispose();
            base.Dispose();
        }
    }
}
