using City.Buildings.Abstractions;
using City.Panels.RatingUps;
using Common;
using Common.Resourses;
using IdleGame.AdvancedObservers;
using Models.City.TrainCamp;
using Models.Common.BigDigits;
using Models.Heroes;
using System;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class HeroEvolutionPanelController : UiPanelController<HeroEvolutionPanelView>
    {
        [Header("Data")]
        public LevelUpRatingHeroes listCost;

        [Header("UI")]
        public Button buttonLevelUP;
        public ListRequirementHeroesUI listRequirementHeroes;

        [SerializeField] private ResourceObjectCost objectCost;

        private HeroModel currentHero;
        private LevelUpRaitingModel data;
        private bool resourceDone = false;
        private bool requireHeroesDone = false;
        private ObserverActionWithHero observersRatingUp = new ObserverActionWithHero();

        protected void OpenPage()
        {
            //currentHero = TrainCamp.Instance.ReturnSelectHero();
            //data = listCost.GetRequirements(currentHero);
            //listRequirementHeroes.SetData(data.RequirementHeroes);
            //GameController.Instance.RegisterOnChangeResource(CheckResource, ResourceType.ContinuumStone);
            //CheckCanUpdateRating();
        }

        public void RatingUp(int count = 1)
        {
            //GameController.Instance.SubtractResource(data.Cost);
            //currentHero.UpRating();
            //OnRatingUp();
            //listRequirementHeroes.DeleteSelectedHeroes();
            //Close();
        }

        public void CheckCanUpdateRating()
        {
            //resourceDone = GameController.Instance.CheckResource(data.Cost.GetResource(ResourceType.ContinuumStone));
            //requireHeroesDone = listRequirementHeroes.IsAllDone();
            //buttonLevelUP.interactable = resourceDone && requireHeroesDone;
        }

        public void CheckResource(GameResource res)
        {
            CheckCanUpdateRating();
        }

        public void CheckHeroes()
        {
            CheckCanUpdateRating();
        }

        protected void ClosePage()
        {
            //GameController.Instance.UnregisterOnChangeResource(CheckResource, ResourceType.ContinuumStone);
            //listRequirementHeroes.ClearData();
        }

        public void Close()
        {
            //ClosePage();
            //CanvasBuildingsUI.Instance.CloseBuilding(building);
        }

        public void RegisterOnRatingUp(Action<BigDigit> d, int rating, string ID = "")
        {
            observersRatingUp.Add(d, ID, rating);
        }

        public void UnregisterOnRatingUp(Action<BigDigit> d, int rating, string ID)
        {
            observersRatingUp.Remove(d, ID, rating);
        }

        private void OnRatingUp()
        {
            observersRatingUp.OnAction(string.Empty, currentHero.General.Rating);
            observersRatingUp.OnAction(currentHero.General.ViewId, currentHero.General.Rating);
            //TrainCamp.Instance.HeroPanel.UpdateInfoAbountHero();
        }

    }
}