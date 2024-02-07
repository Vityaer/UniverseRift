using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class RatingHero : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _stars = new List<GameObject>();
        [SerializeField] private Color _simpleStarColor;
        [SerializeField] private Color _advancedStarColor;
        [SerializeField] private Color _powerfulStarColor;

        private List<Image> _imageStars = new List<Image>();
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
            _rating = rating;

            if (_stars.Count > 0)
            {
                for (int i = 0; i < _stars.Count; i++)
                    _stars[i].SetActive(false);
                for (int i = 0; i < _rating; i++)
                    _stars[i].SetActive(true);
            }
        }

        public void Hide()
        {
            ShowRating(0);
        }
    }
}