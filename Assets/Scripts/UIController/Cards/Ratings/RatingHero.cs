using System.Collections.Generic;
using UIController.Cards.Ratings;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class RatingHero : MonoBehaviour
    {
        [SerializeField] private List<RatingStar> _stars = new();
        [SerializeField] private Color _simpleStarColor;
        [SerializeField] private Color _advancedStarColor;
        [SerializeField] private Color _powerfulStarColor;

        private int _rating = 0;

        public void ShowRating(int rating)
        {
            if (rating > 5) rating = 5;
            _rating = rating;

            if (_stars.Count > 0)
            {
                for (int i = 0; i < _stars.Count; i++)
                    _stars[i].Hide();
                for (int i = 0; i < _rating; i++)
                    _stars[i].Show();
            }
        }

        public void Hide()
        {
            ShowRating(0);
        }

        public void AddStarWithAnimation()
        {
            _rating += 1;
            _stars[_rating % _stars.Count].ShowWithAnimation();
        }
    }
}