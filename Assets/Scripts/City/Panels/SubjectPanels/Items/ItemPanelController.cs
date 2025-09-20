using System.Collections.Generic;
using City.TrainCamp.HeroPanels;
using Common.Db.CommonDictionaries;
using LocalizationSystems;
using UIController.Inventory;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.SubjectPanels.Items
{
    public class ItemPanelController : UiPanelController<ItemPanelView>, IInitializable
    {
        private readonly ILocalizationSystem m_localizationSystem;
        private readonly CommonDictionaries m_commonDictionaries;
        private readonly HeroPanelController m_heroPanelController;
        
        private bool m_onHero = false;
        private DynamicUiList<ItemBonusView, Bonus> m_bonusesPool;
        private DynamicUiList<ItemBonusView, Bonus> m_setBonusesPool;

        public readonly ReactiveCommand OnAction = new();
        public readonly ReactiveCommand OnSwapAction = new();
        public readonly ReactiveCommand OnClose = new();

        
        public ItemPanelController(ILocalizationSystem localizationSystem,
            CommonDictionaries commonDictionaries,
            HeroPanelController heroPanelController)
        {
            m_localizationSystem = localizationSystem;
            m_commonDictionaries = commonDictionaries;
            m_heroPanelController = heroPanelController;
        }

        public new void Initialize()
        {
            m_bonusesPool = new(View.itemBonusViewPrefab, View.BonusesScroll.content,
                View.BonusesScroll,
                null,
                OnCreateBonusView);
            
            m_setBonusesPool = new(View.itemBonusViewPrefab, View.SetBonusesScroll.content,
                    View.BonusesScroll,
                    null,
                    OnCreateBonusView);
            
            View.ActionButton.OnClickAsObservable().Subscribe(_ => OnClickButtonAction()).AddTo(Disposables);
            View.SwapButton.OnClickAsObservable().Subscribe(_ => StartSwapItems()).AddTo(Disposables);
            base.Initialize();
        }

        public void Open(GameItem item, bool onHero = false)
        {
            m_onHero = onHero;
            View.ActionButton.gameObject.SetActive(true);
            if (onHero == false)
            {
                View.ActionButtonText.StringReference = m_localizationSystem
                    .GetLocalizedContainer("SetItemButtonLabel");
                View.SwapButton.gameObject.SetActive(false);
            }
            else
            {
                View.ActionButtonText.StringReference = m_localizationSystem
                    .GetLocalizedContainer("TakeOffItemButtonLabel");
                View.SwapButton.gameObject.SetActive(true);
            }

            View.ActionButton.interactable = true;
            FillData(item);
        }

        private void OnCreateBonusView(ItemBonusView view)
        {
            view.Init(m_localizationSystem);
        }

        public void ShowData(GameItem item)
        {
            m_onHero = false;
            View.ActionButton.gameObject.SetActive(false);
            View.SwapButton.gameObject.SetActive(false);
            FillData(item);
        }

        private void FillData(GameItem item)
        {
            View.MainImage.SetData(item);
            View.Name.StringReference = m_localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Id}Name");

            View.ItemType.StringReference = m_localizationSystem.LocalizationUiContainer
                .GetLocalizedContainer($"{item.Type}TypeName");

            m_bonusesPool.ShowDatas(item.Model.Bonuses);

            ShowExtraBonuses(item);
            OpenWindow();
        }

        private void ShowExtraBonuses(GameItem item)
        {
            List<Bonus> extraBonuses = new();
            int availableExtraBonusCount = 0;
            if (m_onHero)
            {
                ItemCostumeExtraBonusesUtils.GetExtraBonusesForItemOnHero(m_heroPanelController.Hero,
                    item,
                    m_commonDictionaries,
                    out extraBonuses,
                    out availableExtraBonusCount
                    );
            }
            else
            {
                ItemCostumeExtraBonusesUtils.GetExtraBonusesForItem(item,
                    m_commonDictionaries,
                    out extraBonuses,
                    out availableExtraBonusCount
                );
            }

            if (extraBonuses == null || extraBonuses.Count == 0)
            {
                m_setBonusesPool.ClearList();
                View.SetBonusesPanel.SetActive(false);
                return;
            }

            View.SetBonusesPanel.SetActive(true);
            m_setBonusesPool.ShowDatas(extraBonuses);

            for (var i = 0; i < m_setBonusesPool.Views.Count; i++)
            {
                m_setBonusesPool.Views[i].SetAvailable(i < availableExtraBonusCount);
            }
        }

        private void OpenWindow()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<ItemPanelController>(openType: OpenType.Additive);
        }

        private void OnClickButtonAction()
        {
            OnAction.Execute();
            Close();
        }

        public override void OnHide()
        {
            OnClose.Execute();
            base.OnHide();
        }

        private void StartSwapItems()
        {
            OnSwapAction.Execute();
        }

        public void CloseAfterSwap()
        {
            Close();
        }
    }
}