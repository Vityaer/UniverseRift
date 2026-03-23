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
        [Inject] private readonly HeroInstancesController m_heroInstancesController;

        private HeroRenderContainer m_heroRenderContainer;
        private GameObject m_currentPanel;
        private Tween m_tween;

        protected override void OnStart()
        {
            foreach (var card in View.Cards)
                Resolver.Inject(card);
        }

        public void ShowHeroes(List<GameHero> heroes)
        {
            MessagesPublisher.OpenWindowPublisher
                .OpenWindow<HeroesHireResultPanelController>(openType: OpenType.Exclusive);
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
            m_currentPanel = View.OneHeroPanel;
            m_currentPanel.SetActive(true);
            
            if (m_heroRenderContainer != null)
            {
                m_heroInstancesController.ReleaseRender(m_heroRenderContainer);
            }

            m_heroRenderContainer = m_heroInstancesController.ShowHero(hero);
            View.OneHeroRawImage.texture = m_heroRenderContainer.RenderTexture;
            
            m_currentPanel.transform.localScale = Vector3.one * 2;
            View.RatingHeroController.ShowRating(hero.HeroData.Rating);
            m_tween.Kill();
            m_tween = m_currentPanel.transform.DOScale(Vector3.one, View.AnimationTime).OnComplete(() => SwitchOnButtonClose());
        }

        private void ShowManyHeroes(List<GameHero> heroes)
        {
            View.ButtonCloseBuilding.interactable = true;
            m_currentPanel = View.ManyHeroesPanel;
            m_currentPanel.SetActive(true);
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
            m_currentPanel.SetActive(false);
        }

        public override void Close()
        {
            if (m_heroRenderContainer != null)
            {
                m_heroInstancesController.ReleaseRender(m_heroRenderContainer);
                m_heroRenderContainer = null;
            }

            base.Close();
        }

        public override void Dispose()
        {
            m_tween.Kill();
            base.Dispose();
        }
    }
}
