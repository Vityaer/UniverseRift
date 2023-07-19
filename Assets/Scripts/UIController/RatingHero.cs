using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class RatingHero : MonoBehaviour
    {
        private List<GameObject> _stars = new List<GameObject>();
        private List<Image> _imageStars = new List<Image>();
        private Color _simpleStarColor;
        private Color _advancedStarColor;
        private Color _powerfulStarColor;
        private int _rating = 0;

        void Awake()
        {
            _rating = 0;
            foreach (GameObject star in _stars)
            {
                _imageStars.Add(star.GetComponent<Image>());
            }
        }

        public void ShowRating(int rating)
        {
            if (rating > 6) rating = 6;
            if (_stars.Count > 0)
            {
                for (int i = 0; i < _stars.Count; i++)
                    _stars[i].SetActive(false);
                for (int i = 0; i < rating; i++)
                    _stars[i].SetActive(true);
                this._rating = rating;
            }
        }

        public void Hide()
        {
            ShowRating(0);
        }
    }
}