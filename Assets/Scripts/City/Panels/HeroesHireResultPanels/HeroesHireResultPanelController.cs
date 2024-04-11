using City.Buildings.Abstractions;
using City.TrainCamp.HeroInstances;
using DG.Tweening;
using Hero;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.HeroesHireResultPanels
{
    public class HeroesHireResultPanelController : BaseBuilding<HeroesHireResultPanelView>
    {
        [Inject] private readonly HeroInstancesController _heroInstancesController;

        private GameObject _currentPanel;
        private Tween _tween;

        protected override void OnStart()
        {
            foreach (var card in View.Cards)
                Resolver.Inject(card);
        }

        public void ShowHeroes(List<GameHero> heroes)
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroesHireResultPanelController>(openType: OpenType.Exclusive);
            if (heroes.Count == 1)
            {
                ShowOneHero(heroes[0]);
            }
            else
            {
                ShowManyHeroes(heroes);
            }
        }

        public void ShowSplinterHeroes(List<GameHero> heroes)
        {
            ShowManyHeroes(heroes);
        }

        private void ShowOneHero(GameHero hero)
        {
            View.ButtonCloseBuilding.interactable = false;
            _currentPanel = View.OneHeroPanel;
            _currentPanel.SetActive(true);
            _heroInstancesController.ShowHero(hero);
            _currentPanel.transform.localScale = Vector3.one * 2;
            View.RatingHeroController.ShowRating(hero.HeroData.Rating);
            _tween.Kill();
            _tween = _currentPanel.transform.DOScale(Vector3.one, View.AnimationTime).OnComplete(() => SwitchOnButtonClose());
        }

        private void ShowManyHeroes(List<GameHero> heroes)
        {
            View.ButtonCloseBuilding.interactable = true;
            _currentPanel = View.ManyHeroesPanel;
            _currentPanel.SetActive(true);
            for (var i = 0; i < heroes.Count; i++)
            {
                View.Cards[i].SetData(heroes[i]);
            }
        }

        private void SwitchOnButtonClose()
        {
            View.ButtonCloseBuilding.interactable = true;
        }

        protected override void ClosePage()
        {
            _currentPanel.SetActive(false);
        }

        public override void Dispose()
        {
            _tween.Kill();
            base.Dispose();
        }
    }
}
