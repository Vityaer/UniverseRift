using TMPro;
using UIController.ControllerPanels;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Inventory
{
    public class PanelInfoSplinterController : MonoBehaviour
    {
        [Header("PanelInfo Item")]
        public GameObject panelInfoItem;
        public SubjectCellController imageInfoItem;
        public TextMeshProUGUI textNameItem, textTypeItem, textGeneralInfo;
        private Item selectItem = null;
        [Header("Controllers")]
        public Button btnAction;
        public GameObject btnPosibility, btnHelp, btnClose;
        public TextMeshProUGUI textButtonAction;
        [Header("Other Panel")]
        public PanelPosibleHeroes panelPosibleHeroes;
        public PanelPosibleArtifact PanelPosibleArtifact;
        SplinterController splinterController;

        public void OpenInfoAboutSplinter(SplinterController splinterController, bool withControl = false)
        {
            this.splinterController = splinterController;
            UpdateUIInfo();
            btnPosibility.SetActive(splinterController.CountReward > 1);
            btnAction.interactable = splinterController.IsCanUse;
            btnAction.gameObject.SetActive(withControl);
            OpenPanel();
        }

        public void OpenPosibleRewards()
        {
            switch (splinterController.splinter.typeSplinter)
            {
                case TypeSplinter.Hero:
                    panelPosibleHeroes.Open(splinterController.splinter.reward);
                    break;
                case TypeSplinter.Artifact:
                    PanelPosibleArtifact.Open(splinterController.splinter.reward);
                    break;
            }
        }

        void UpdateUIInfo()
        {
            imageInfoItem.SetItem(splinterController);
            textNameItem.text = splinterController.splinter.Id;
            textTypeItem.text = splinterController.splinter.GetTextType;
            textGeneralInfo.text = splinterController.splinter.GetTextDescription;
        }

        public void StartSummon()
        {
            PanelSelectCountScript.Instance.Open(splinterController, splinterController.splinter.RequireAmount, splinterController.splinter.Amount);
            PanelSelectCountScript.Instance.RegisterOnActionAfterUse(Close);
        }

        void OpenPanel()
        {
            panelInfoItem.SetActive(true);
            btnClose.SetActive(true);
        }

        public void Close()
        {
            panelInfoItem.SetActive(false);
            btnClose.SetActive(false);
            ClearInfo();
        }

        void ClearInfo()
        {
            textNameItem.text = string.Empty;
            textTypeItem.text = string.Empty;
            textGeneralInfo.text = string.Empty;
        }
    }
}