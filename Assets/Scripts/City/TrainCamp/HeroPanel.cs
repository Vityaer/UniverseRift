using Models.Heroes;
using City.Buildings.General;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Models.Heroes.Characteristics;
using UIController.Inventory;
using UIController.ItemVisual;

namespace City.TrainCamp
{
    public class HeroPanel : Building
    {
        [Header("Controller")]
        public Button btnToLeft;
        public Button btnToRight;
        public Button btnLevelUP;

        [Header("Information")]
        public Image imageHero;
        public TextMeshProUGUI textLevel;
        public TextMeshProUGUI textNameHero;
        public TextMeshProUGUI textHP;
        public TextMeshProUGUI textAttack;
        public TextMeshProUGUI textArmor;
        public TextMeshProUGUI textInitiative;
        public TextMeshProUGUI textStrengthHero;

        [Header("Items")]
        public List<HeroItemCell> CellsForItem = new List<HeroItemCell>();
        [Header("Skills")]
        public SkillUIController skillController;
        [Header("Costs")]
        public CostUIList costController;
        public CostLevelUp costLevelObject;

        [Header("Details")]
        [SerializeField] private HeroDetailsPanel _heroDetailsPanel;
        public Button btnOpenHeroDetails;
        public Button btnCloseHeroDetails;

        public Action LeftButtonClick;
        public Action RightButtonClick;

        private HeroModel _hero;

        protected override void OnStart()
        {
            btnToLeft.onClick.AddListener(() => LeftButtonClick());
            btnToRight.onClick.AddListener(() => RightButtonClick());
            btnLevelUP.onClick.AddListener(() => LevelUp());
            btnOpenHeroDetails.onClick.AddListener(() => _heroDetailsPanel.Open());
            btnCloseHeroDetails.onClick.AddListener(() => _heroDetailsPanel.Close());
        }

        public void ShowHero(HeroModel hero)
        {
            _hero = hero;
            UpdateInfoAbountHero();
        }

        public void UpdateInfoAbountHero()
        {
            imageHero.sprite = _hero.General.ImageHero;
            textNameHero.text = _hero.General.Name;
            UpdateTextAboutHero();
            foreach (HeroItemCell cell in CellsForItem)
            {
                cell.Clear();
                cell.SetItem(_hero.CostumeHero.GetItem(cell.typeCell));
            }
            CheckResourceForLevelUP();
        }

        public void UpdateTextAboutHero()
        {
            textLevel.text = _hero.General.Level.ToString();
            textHP.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.HP)).ToString();
            textAttack.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Damage)).ToString();
            textArmor.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Defense)).ToString();
            textInitiative.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
            textStrengthHero.text = _hero.GetStrength.ToString();
            _hero.PrepareSkillLocalization();
            skillController.ShowSkills(_hero.skills);
            costController.ShowCosts(costLevelObject.GetCostForLevelUp(_hero.General.Level));
            // _heroDetailsPanel.ShowDetails(_hero);
        }

        private void CheckResourceForLevelUP()
        {
            btnLevelUP.interactable = GameController.Instance.CheckResource(costLevelObject.GetCostForLevelUp(_hero.General.Level));
        }

        public void TakeOff(Item item)
        {
            _hero.CostumeHero.TakeOff(item);
            UpdateTextAboutHero();
        }

        public void LevelUp()
        {
            GameController.Instance.SubtractResource(costLevelObject.GetCostForLevelUp(_hero.General.Level));
            _hero.LevelUP();
            UpdateInfoAbountHero();
        }
    }
}