using System.Collections.Generic;
using City.Panels.Inventories;
using City.Panels.PosibleArtifacts;
using City.Panels.PosibleHeroes;
using ClientServices;
using Common.Db.CommonDictionaries;
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
using UIController.ControllerPanels.SelectCount;
using UIController.Inventory;
using UIController.Rewards;
using UiExtensions.Panels;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.SubjectPanels.Splinters
{
    public class SplinterPanelController : UiPanelController<SplinterPanelView>, IInitializable
    {
        [Inject] private readonly SplinterSelectCountPanelController m_selectCountPanelController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly GameInventory m_gameInventory;
        [Inject] private readonly PosibleHeroesPanelController m_panelPosibleHeroes;
        [Inject] private readonly PosibleArtifactPanelController m_panelPosibleArtifact;

        private GameItem m_selectItem = null;
        private GameSplinter m_splinter;

        public new void Initialize()
        {
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OpenCountPanel()).AddTo(Disposables);
            m_selectCountPanelController.ActionAfterUse.Subscribe(_ => Close()).AddTo(Disposables);
            m_selectCountPanelController.ActionOnSelectedCount
                .Subscribe(count => OnStartUseSplinters(count).Forget())
                .AddTo(Disposables);
        }

        public void ShowData(GameSplinter splinterController, bool withControl = false)
        {
            m_splinter = splinterController;
            UpdateUIInfo();
            View.PosibilityButton.SetActive(splinterController.CountReward > 1);
            View.ActionButton.interactable = splinterController.IsCanUse;
            View.ActionButton.gameObject.SetActive(withControl);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<SplinterPanelController>(openType: OpenType.Additive);
        }

        private void OpenCountPanel()
        {
            m_selectCountPanelController.Open(m_splinter);
        }

        private async UniTaskVoid OnStartUseSplinters(int count)
        {
            var message = new UseSplinterMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                SplinterId = m_splinter.Id,
                Count = count
            };

            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                switch (m_splinter.typeSplinter)
                {
                    case SplinterType.Hero:
                        var newHeroDatas = m_jsonConverter.Deserialize<List<HeroData>>(result);

                        var heroes = new List<GameHero>(newHeroDatas.Count);
                        for (var i = 0; i < newHeroDatas.Count; i++)
                        {
                            var model = m_commonDictionaries.Heroes[newHeroDatas[i].HeroId];
                            var hero = new GameHero(model, newHeroDatas[i]);
                            m_heroesStorageController.AddHero(hero);
                            heroes.Add(hero);
                        }
                        break;
                    case SplinterType.Item:
                        var rewardModel = m_jsonConverter.Deserialize<RewardModel>(result);
                        var reward = new GameReward(rewardModel, m_commonDictionaries);
                        m_clientRewardService.ShowReward(reward);
                        break;
                }

                m_gameInventory.Remove(m_splinter.Id, m_splinter.RequireAmount * count);
            }
        }

        public void OpenPosibleRewards()
        {
            switch (m_splinter.typeSplinter)
            {
                case SplinterType.Hero:
                    m_panelPosibleHeroes.SetData(m_splinter.Reward);
                    break;
                case SplinterType.Item:
                    m_panelPosibleArtifact.SetData(m_splinter.Reward);
                    break;
            }
        }

        void UpdateUIInfo()
        {
            View.MainImage.Init(null, m_commonDictionaries);
            View.MainImage.SetData(m_splinter);
            View.MainLabel.text = m_splinter.Id;
            View.ItemType.text = m_splinter.GetTextType;
            View.GeneralInfo.text = m_splinter.GetTextDescription;
        }

        public void StartSummon()
        {
            m_selectCountPanelController.Open(m_splinter);
        }

    }
}