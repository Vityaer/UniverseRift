using City.General;
using Common;
using Models.Heroes;
using System.Collections.Generic;
using UIController;
using UIController.Cards;
using UIController.GameSystems;
using UIController.Inventory;

namespace City.TrainCamp
{
    public class TrainCamp : MainPage
    {
        public HeroPanel HeroPanel;
        public List<HeroModel> listHeroes = new List<HeroModel>();
        public int numSelectHero = 0;
        private HeroModel hero;
        public ListCardOnWarTable listCardPanel;
        public ListMyHeroesController ListCard;

        private static TrainCamp instance;
        public static TrainCamp Instance => instance;

        protected override void Awake()
        {
            instance = this;
            base.Awake();
        }

        private void SelectHero(int num)
        {
            if (num <= 0)
                num = 0;

            if (num >= listHeroes.Count - 1)
                num = listHeroes.Count - 1;

            numSelectHero = num;
            hero = listHeroes[numSelectHero];
            HeroPanel.ShowHero(hero);
        }

        //API
        public void TakeOff(Item item)
        {
            HeroPanel.TakeOff(item);
        }

        public void SelectHero(Card card)
        {
            numSelectHero = listHeroes.FindIndex(x => x == card.hero);
            SelectHero(numSelectHero);
            OpenHeroPanel();
        }

        public HeroModel ReturnSelectHero()
        {
            return listHeroes[numSelectHero];
        }

        public void OpenHeroPanel()
        {
            MainTouchControllerScript.Instance.RegisterOnObserverSwipe(OnSwipe);
            ListCard.Close();
            MenuController.Instance.CloseMainPage();
            HeroPanel.Open();
        }

        public void CloseHeroPanel()
        {
            MainTouchControllerScript.Instance.UnregisterOnObserverSwipe(OnSwipe);
            HeroPanel.Close();
            ListCard.Open();
        }

        public override void Open()
        {
            base.Open();
            ListCard.Open();
        }

        public override void Close()
        {
            ListCard.Close();
        }

        void Start()
        {
            listHeroes = GameController.Instance.GetListHeroes;
            listCardPanel.RegisterOnSelect(SelectHero);
            HeroPanel.LeftButtonClick += () => SelectHero(numSelectHero - 1);
            HeroPanel.RightButtonClick += () => SelectHero(numSelectHero + 1);
        }

        private void OnSwipe(TypeSwipe typeSwipe)
        {
            switch (typeSwipe)
            {
                case TypeSwipe.Left:
                    SelectHero(numSelectHero - 1);
                    break;
                case TypeSwipe.Right:
                    SelectHero(numSelectHero + 1);
                    break;
            }
        }
    }
}