using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController;
using UIController.Cards;
using UnityEngine;

namespace City.Panels.HeroesHireResultPanels
{
    public class HeroesHireResultPanelView : BaseBuildingView
    {
        [Header("Many heroes")]
        public GameObject ManyHeroesPanel;
        public List<Card> Cards;

        [Header("One hero")]
        public GameObject OneHeroPanel;
        public RatingHero RatingHeroController;
        public float AnimationTime = 1f;
    }
}
