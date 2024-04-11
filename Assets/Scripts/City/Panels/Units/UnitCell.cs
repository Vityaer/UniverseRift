using Hero;
using Models;
using System;
using TMPro;
using UIController;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace City.Panels.Units
{
    public class UnitCell : UiView
    {
        [SerializeField] private Image HeroAvatar;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private RatingHero _ratingController;

        private GameHero _hero;
        public void SetData(GameHero hero)
        {
            _hero = hero;
            HeroAvatar.enabled = true;
            HeroAvatar.sprite = _hero.Avatar;
            _level.text = $"{_hero.HeroData.Level}";
            _ratingController.ShowRating(_hero.HeroData.Rating);
        }

        public void Clear()
        {
            _hero = null;
            HeroAvatar.enabled = false;
            _level.text = string.Empty;
            _ratingController.ShowRating(0);
        }
    }
}
