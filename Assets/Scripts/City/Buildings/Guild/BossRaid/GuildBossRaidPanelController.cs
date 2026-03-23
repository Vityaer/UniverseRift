using System;
using System.Collections.Generic;
using City.Buildings.Guild.BossRaidRewardPanels;
using City.Buildings.Guild.RecruitViews;
using City.Buildings.Guild.Utils;
using City.TrainCamp.HeroInstances;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Fight.Windows.WarTable;
using Hero;
using Misc.Json;
using Models;
using Models.Arenas;
using Models.Common.BigDigits;
using Models.Fights.Campaign;
using Models.Guilds;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages.Guilds;
using Network.DataServer.Messages.Teams;
using Network.DataServer.Models;
using Network.DataServer.Models.Guilds;
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

        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly IObjectResolver m_diContainer;
        [Inject] private readonly WarTableController m_warTableController;
        [Inject] private readonly HeroInstancesController m_heroInstancesController;
        [Inject] private readonly BossRaidRewardPanelController m_bossRaidRewardPanelController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private CompositeDisposable m_raidDisposables;
        private GuildBossMission m_currentBossMission;

        private HeroRenderContainer m_heroRenderContainer;
        private float m_createDamageSum;

        private RecruitData m_myRecruitData;
        private readonly Dictionary<int, RecruitProgressView> m_recruitsView = new();

        private GuildData GuildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            View.BossRaidButton.OnClick.Subscribe(_ => OpenRaidMission()).AddTo(Disposables);
            View.OpenRewardPanelButton.OnClickAsObservable()
                .Subscribe(_ => OpenRewardPanel())
                .AddTo(Disposables);

            base.Start();
        }

        protected override void Hide()
        {
            if (m_heroRenderContainer == null)
            {
                m_heroInstancesController.ReleaseRender(m_heroRenderContainer);
                m_heroRenderContainer = null;
            }

            base.Hide();
        }

        private void OpenRewardPanel()
        {
            m_bossRaidRewardPanelController.Open(m_currentBossMission);
        }

        protected override void OnLoadGame()
        {
            if (CommonGameData.Teams.TryGetValue(GetType().Name, out string teamContainerJson))
                m_jsonConverter.Deserialize<TeamContainer>(teamContainerJson);

            base.OnLoadGame();
        }

        private void OpenRaidMission()
        {
            MissionModel missionModel = new();
            foreach (BossModel unit in m_currentBossMission.BossModels) missionModel.Units.Add(new HeroData(unit));

            OpenMission(missionModel);
        }

        protected override void OnStartMission()
        {
            m_raidDisposables = new();
            FightController.AfterCreateFight.Subscribe(_ => { ChangeUnitsData(); }).AddTo(m_raidDisposables);

            base.OnStartMission();
        }

        private void ChangeUnitsData()
        {
            m_raidDisposables.Dispose();
            m_raidDisposables = new CompositeDisposable();
            m_createDamageSum = 0f;
            GuildBossContainer container = CommonDictionaries.GuildBossContainers["MainBosses"];

            for (int i = 0; i < FightController.GetRightTeam.Count; i++)
            {
                BossModel bossData = container.Missions[GuildData.CurrentBoss].BossModels[i];
                float maxHealth = bossData.Health.Mantissa * Mathf.Pow(10, bossData.Health.E10);
                float currentHealth = GuildData.BossHealthMantissa * Mathf.Pow(10, GuildData.BossHealthE10);
                FightController.GetRightTeam[i].heroController.Hero.ChangeHealth(maxHealth, currentHealth);

                FightController.GetRightTeam[i].heroController.Hero.OnGetDamage
                    .Subscribe(AddDamage)
                    .AddTo(m_raidDisposables);
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
            Debug.Log($"m_createDamageSum: {m_createDamageSum}");
            RaidBossMessage message = new RaidBossMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Damage = (int)m_createDamageSum
            };

            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                GuildPlayerSaveContainer newGuildData = m_jsonConverter.Deserialize<GuildPlayerSaveContainer>(result);
                if (newGuildData != null)
                {
                    m_resourceStorageController.SubtractResource(GetCostRaid());
                    CommonGameData.City.GuildPlayerSaveContainer = newGuildData;

                    RecruitData data = GetMyRecruitData();
                    data.CountRaidBoss += 1;
                    m_createDamageSum = 0f;
                    // UpdateUi();
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

        // TODO Нужно оптимизировать обновление
        private void UpdateUi()
        {
            if (GuildData == null)
                return;

            View.BossLevel.text = $"{GuildData.CurrentBoss + 1}";

            GuildBossContainer container = CommonDictionaries.GuildBossContainers["MainBosses"];
            m_currentBossMission = container.Missions[GuildData.CurrentBoss].Clone();
            BossModel bossData = m_currentBossMission.BossModels[0];
            HeroModel bossModel = CommonDictionaries.Heroes[bossData.HeroId];
            HeroData bossHeroData = new HeroData { Level = bossData.Level, Rating = bossData.Rating };
            GameHero gameBoss = new GameHero(bossModel, bossHeroData);

            if (m_heroRenderContainer != null)
                m_heroInstancesController.ReleaseRender(m_heroRenderContainer);

            m_heroRenderContainer = m_heroInstancesController.ShowHero(gameBoss);
            View.BossRawImage.texture = m_heroRenderContainer.RenderTexture;

            BigDigit currentHealth = new BigDigit(GuildData.BossHealthMantissa, GuildData.BossHealthE10);
            BigDigit maxHealth = bossData.Health;

            BigDigit delta = currentHealth / maxHealth;
            float currentProgress = delta.Mantissa * Mathf.Pow(10, delta.E10);
            currentProgress = Mathf.Clamp01(currentProgress);

            View.BossHealthSlider.value = currentProgress;
            View.BossRaidButton.SetCost(GetCostRaid());

            RecruitData myRecruitData = GetMyRecruitData();
            if (myRecruitData != null && myRecruitData.CountRaidBoss > 0) ShowRefreshTime();

            List<RecruitData> recruits = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits;
            recruits.Sort(new RecruitDamageComparer());
            int index = 0;
            foreach (RecruitData recruit in recruits)
            {
                if (recruit.ResultMantissa < 1f && recruit.ResultE10 == 0) continue;

                RecruitProgressView prefab;
                if (m_recruitsView.TryGetValue(recruit.PlayerId, out prefab))
                {
                    prefab.SetData(recruit, View.Scroll, m_commonDictionaries);
                }
                else
                {
                    prefab = UnityEngine.Object.Instantiate(View.RecruitProgressViewPrefab, View.Content);
                    m_recruitsView.Add(recruit.PlayerId, prefab);
                    m_diContainer.Inject(prefab);
                    prefab.SetData(recruit, View.Scroll, m_commonDictionaries);
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
            RecruitData recruit = GetMyRecruitData();
            if (recruit == null)
                return;

            if (!string.IsNullOrEmpty(recruit.DateTimeFirstRaidBoss))
            {
                DateTime lastDateTime = TimeUtils.ParseTime(recruit.DateTimeFirstRaidBoss);

                TimeSpan deltaTime = DateTime.UtcNow - lastDateTime;
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
            RecruitData recruit = GetMyRecruitData();
            if (recruit == null)
                return new GameResource();

            if (recruit.CountRaidBoss >= BOSS_FREE_RAID_COUNT)
            {
                int count = recruit.CountRaidBoss - BOSS_FREE_RAID_COUNT + 1;
                GameResource result = new GameResource(ResourceType.Diamond, new BigDigit(count * BOSS_RAID_COST_STEP));
                return result;
            }
            else
            {
                return new GameResource(ResourceType.Diamond);
            }
        }

        private async UniTaskVoid SetDefenders(TeamContainer heroesIdsContainer)
        {
            ChangeTeamDefendersMessage message = new ChangeTeamDefendersMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroesIdsContainer = m_jsonConverter.Serialize(heroesIdsContainer),
                TeamsContainerName = GetType().Name
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }
        }

        private RecruitData GetMyRecruitData()
        {
            if (m_myRecruitData == null)
                m_myRecruitData = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits
                    .Find(recruit => recruit.PlayerId == CommonGameData.PlayerInfoData.Id);

            return m_myRecruitData;
        }

        public override void Dispose()
        {
            m_raidDisposables?.Dispose();
            base.Dispose();
        }
    }
}