using Assets.Scripts.City.TrainCamp;
using Common;
using Common.Resourses;
using TMPro;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Mines
{
    public class PanelInfoMine : MonoBehaviour
    {
        public GameObject panel, panelController;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI textNameMine, textLevelMine, textIncome, textStore;
        public ItemSliderController sliderAmount;
        // public SubjectCellControllerScript product;
        public CostUIList costController;
        [SerializeField] private Button buttonLevelUp;
        private Mine mine;
        MineController mineController;
        public void SetData(MineController mineController)
        {
            this.mineController = mineController;
            mine = mineController.GetMine;
            UpdateUI();
            Open();
        }
        public void LevelUP()
        {
            mineController.UpdateLevel();
            UpdateUI();
        }
        ListResource cost = new ListResource();
        private void UpdateUI()
        {
            // product.SetItem(mine.income);
            textNameMine.text = mine.income.GetName();
            textLevelMine.text = FunctionHelp.TextLevel(mine.level);
            textIncome.text = mine.income.GetTextAmount();
            textStore.text = mine.GetMaxStore.GetTextAmount();
            sliderAmount.SetAmount(mine.GetStore.Amount, mine.GetMaxStore.Amount);
            cost = mine.GetCostLevelUp();
            costController.ShowCosts(cost);
            buttonLevelUp.interactable = GameController.Instance.CheckResource(cost);
        }
        public void Open()
        {
            panel.SetActive(true);
        }
        public void Close()
        {
            panel.SetActive(false);
        }
    }
}