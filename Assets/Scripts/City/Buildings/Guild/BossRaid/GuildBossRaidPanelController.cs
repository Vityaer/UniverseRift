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
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace City.Buildings.Guild.BossRaid
{
    public class GuildBossRaidPanelController : UiPanelController<GuildBossRaidPanelView>
    {
        private const int BOSS_FREE_RAID_COUNT = 2;
        private const int BOSS_RAID_REFRESH_HOURS = 16;
        private const int BOSS_RAID_COST_STEP = 50;

        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IObjectResolver _diContainer;

        private RecruitData _myRecruitData;
        private Dictionary<int, RecruitProgressView> _recruitsView = new();
        private GuildData _guildData => CommonGameData.City.GuildPlayerSaveContainer.GuildData;

        public override void Start()
        {
            View.BossRaidButton.OnClick.Subscribe(_ => StartRaidBoss().Forget()).AddTo(Disposables);
            base.Start();
        }

        private async UniTaskVoid StartRaidBoss()
        {
            var message = new RaidBossMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
            };

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

        private void UpdateUi()
        {
            if (_guildData == null)
                return;

            //View.GuildName.text = _guildData.Name;
            //View.GuildLevel.text = $"Level {_guildData.Level}";
            //View.GuildId.text = $"ID: {_guildData.Id}";
            View.BossLevel.text = $"{_guildData.CurrentBoss + 1}";

            var container = _commonDictionaries.GuildBossContainers["MainBosses"];
            var bossData = container.Missions[_guildData.CurrentBoss].BossModels[0];
            var bossModel = _commonDictionaries.Heroes[bossData.HeroId];
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

        private RecruitData GetMyRecruitData()
        {
            if (_myRecruitData == null)
            {
                _myRecruitData = CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits
                    .Find(recruit => recruit.PlayerId == CommonGameData.PlayerInfoData.Id);
            }

            Debug.Log($"_myRecruitData: {_myRecruitData != null} count: {CommonGameData.City.GuildPlayerSaveContainer.GuildRecruits.Count}");
            return _myRecruitData;
        }
    }
}
