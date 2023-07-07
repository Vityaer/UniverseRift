using City.Panels.PosibleArtifacts;
using City.Panels.PosibleHeroes;
using City.Panels.SubjectPanels.Splinters;
using Common.Inventories.Splinters;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UIController.ControllerPanels.SelectCount;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace UIController.Inventory
{
    public class SplinterPanelController : UiPanelController<SplinterPanelView>, IInitializable
    {
        [Inject] private readonly SplinterSelectCountPanelController _selectCountPanelController;

        private GameItem _selectItem = null;

        [Header("Other Panel")]
        public PosibleHeroesPanelController panelPosibleHeroes;
        public PosibleArtifactPanelController PanelPosibleArtifact;
        GameSplinter _splinter;

        public void Initialize()
        {
            _selectCountPanelController.ActionAfterUse.Subscribe(_ => Close()).AddTo(Disposables);
        }

        public void OpenInfoAboutSplinter(GameSplinter splinterController, bool withControl = false)
        {
            _splinter = splinterController;
            UpdateUIInfo();
            View.PosibilityButton.SetActive(splinterController.CountReward > 1);
            View.ActionButton.interactable = splinterController.IsCanUse;
            View.ActionButton.gameObject.SetActive(withControl);
        }

        public void ShowData(GameSplinter spliter)
        {
        }

        public void OpenPosibleRewards()
        {
            switch (_splinter.typeSplinter)
            {
                case TypeSplinter.Hero:
                    panelPosibleHeroes.SetData(_splinter.reward);
                    break;
                case TypeSplinter.Artifact:
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