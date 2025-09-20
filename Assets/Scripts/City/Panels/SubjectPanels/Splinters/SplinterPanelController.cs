using City.Panels.PosibleArtifacts;
using City.Panels.PosibleHeroes;
using City.Panels.SubjectPanels.Splinters;
using ClientServices;
using Common.Heroes;
using Common.Inventories.Splinters;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Hero;
using Misc.Json;
using Models;
using Models.Inventory.Splinters;
using Network.DataServer;
using Network.DataServer.Messages.Inventories.Splinters;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UIController.ControllerPanels.SelectCount;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace UIController.Inventory
{
    public class SplinterPanelController : UiPanelController<SplinterPanelView>, IInitializable
    {
        [Inject] private readonly SplinterSelectCountPanelController _selectCountPanelController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly GameInventory _gameInventory;

        private GameItem _selectItem = null;

        [Header("Other Panel")]
        public PosibleHeroesPanelController panelPosibleHeroes;
        public PosibleArtifactPanelController PanelPosibleArtifact;
        GameSplinter _splinter;

        public new void Initialize()
        {
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OpenCountPanel()).AddTo(Disposables);
            _selectCountPanelController.ActionAfterUse.Subscribe(_ => Close()).AddTo(Disposables);
            _selectCountPanelController.ActionOnSelectedCount.Subscribe(count => OnStartUseSplinters(count).Forget()).AddTo(Disposables);
        }

        public void ShowData(GameSplinter splinterController, bool withControl = false)
        {
            _splinter = splinterController;
            UpdateUIInfo();
            View.PosibilityButton.SetActive(splinterController.CountReward > 1);
            View.ActionButton.interactable = splinterController.IsCanUse;
            View.ActionButton.gameObject.SetActive(withControl);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<SplinterPanelController>(openType: OpenType.Additive);
        }

        private void OpenCountPanel()
        {
            _selectCountPanelController.Open(_splinter);
        }

        private async UniTaskVoid OnStartUseSplinters(int count)
        {
            var message = new UseSplinterMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                SplinterId = _splinter.Id,
                Count = count
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                switch (_splinter.typeSplinter)
                {
                    case SplinterType.Hero:
                        var newHeroDatas = _jsonConverter.Deserialize<List<HeroData>>(result);

                        var heroes = new List<GameHero>(newHeroDatas.Count);
                        for (var i = 0; i < newHeroDatas.Count; i++)
                        {
                            var model = _commonDictionaries.Heroes[newHeroDatas[i].HeroId];
                            var hero = new GameHero(model, newHeroDatas[i]);
                            _heroesStorageController.AddHero(hero);
                            heroes.Add(hero);
                        }
                        break;
                    case SplinterType.Item:
                        var rewardModel = _jsonConverter.Deserialize<RewardModel>(result);
                        var reward = new GameReward(rewardModel, _commonDictionaries);
                        _clientRewardService.ShowReward(reward);
                        break;
                }

                _gameInventory.Remove(_splinter.Id, _splinter.RequireAmount * count);
            }
        }

        public void OpenPosibleRewards()
        {
            switch (_splinter.typeSplinter)
            {
                case SplinterType.Hero:
                    panelPosibleHeroes.SetData(_splinter.reward);
                    break;
                case SplinterType.Item:
                    PanelPosibleArtifact.SetData(_splinter.reward);
                    break;
            }
        }

        void UpdateUIInfo()
        {
            View.MainImage.SetData(_splinter);
            View.MainLabel.text = _splinter.Id;
            View.ItemType.text = _splinter.GetTextType;
            View.GeneralInfo.text = _splinter.GetTextDescription;
        }

        public void StartSummon()
        {
            _selectCountPanelController.Open(_splinter);
        }

    }
}