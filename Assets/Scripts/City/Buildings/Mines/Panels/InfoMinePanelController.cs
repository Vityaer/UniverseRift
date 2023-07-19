using City.Buildings.Mines.Panels;
using City.TrainCamp;
using Common.Resourses;
using System.Collections.Generic;
using TMPro;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mines
{
    public class InfoMinePanelController : UiPanelController<InfoMinePanelView>
    {
        public GameObject panel, panelController;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textNameMine, textLevelMine, textIncome, textStore;
        public ItemSliderController sliderAmount;
        // public SubjectCellControllerScript product;
        public CostUIList costController;
        [SerializeField] private Button buttonLevelUp;
        private Mine _mine;
        private MineController _mineController;
        private List<GameResource> _cost = new List<GameResource>();

        public void SetData(MineController mineController)
        {
            this._mineController = mineController;
            _mine = mineController.GetMine;
            UpdateUI();
        }

        public void LevelUP()
        {
            _mineController.UpdateLevel();
            UpdateUI();
        }

        private void UpdateUI()
        {
            // product.SetItem(mine.income);
            textNameMine.text = _mine.income.Name;
            textLevelMine.text = $"Level {_mine.level}";
            textIncome.text = _mine.income.GetTextAmount();
            textStore.text = _mine.GetMaxStore.GetTextAmount();
            sliderAmount.SetAmount(_mine.GetStore.Amount, _mine.GetMaxStore.Amount);
            _cost = _mine.GetCostLevelUp();
            costController.ShowCosts(_cost);
            //buttonLevelUp.interactable = GameController.Instance.CheckResource(_cost);
        }
    }
}