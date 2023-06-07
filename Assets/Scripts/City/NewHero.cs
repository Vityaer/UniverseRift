using Common;
using Common.Resourses;
using Models.Heroes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace City
{

    //TODO: возможно больше не нужен
    public class NewHero : MonoBehaviour
    {
        public List<HeroModel> listNewHero = new List<HeroModel>();

        public Color colorNotInteractable;
        public Color colorInteractable;
        public Resource DiamondCost;

        private Button btnBuy;
        private Image sprite;

        void Awake()
        {
            btnBuy = GetComponent<Button>();
            sprite = GetComponent<Image>();
        }

        void Start()
        {
            CheckResourceForBuyHero();
        }

        public void GetNewHero()
        {
            GameController.Instance.SubtractResource(DiamondCost);

            HeroModel hero = (HeroModel)listNewHero[Random.Range(0, listNewHero.Count)].Clone();
            hero.General.Name = hero.General.Name + " №" + Random.Range(0, 1000).ToString();
            GetNewHero(hero);
            MessageController.Instance.AddMessage("Новый герой! Это - " + hero.General.Name);
            CheckResourceForBuyHero();
        }
        public void GetNewHero(HeroModel newHero)
        {
            GameController.Instance.AddHero(newHero);
        }

        private void CheckResourceForBuyHero()
        {
            bool result = GameController.Instance.CheckResource(DiamondCost);
            btnBuy.interactable = result;
            sprite.color = result ? colorInteractable : colorNotInteractable;
        }
    }
}