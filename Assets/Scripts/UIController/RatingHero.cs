using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class RatingHero : MonoBehaviour
    {
        public List<GameObject> stars = new List<GameObject>();
        private List<Image> imageStars = new List<Image>();
        public Color colorSimpleStar, colorAdvancedStar, colorPowerfulStar;
        public int rating = 0;
        void Awake()
        {
            rating = 0;
            foreach (GameObject star in stars)
            {
                imageStars.Add(star.GetComponent<Image>());
            }
        }
        public void ShowRating(int rating)
        {
            if (rating > 6) rating = 6;
            if (stars.Count > 0)
            {
                for (int i = 0; i < stars.Count; i++)
                    stars[i].SetActive(false);
                for (int i = 0; i < rating; i++)
                    stars[i].SetActive(true);
                this.rating = rating;
            }
            else
            {
                Debug.Log(gameObject.name);
            }
        }
        public void Hide()
        {
            ShowRating(0);
        }
    }
}