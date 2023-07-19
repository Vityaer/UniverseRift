using Models.Heroes;
using Models.City.TrainCamp;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    [CreateAssetMenu(fileName = "LevelUPRaitingHeroes", menuName = "Custom ScriptableObject/LevelUpRatingHeroes", order = 61)]
    [System.Serializable]
    public class LevelUpRatingHeroes : ScriptableObject
    {
        [Header("Ratings")]
        public List<LevelUpRaitingModel> ratings = new List<LevelUpRaitingModel>();

        public LevelUpRaitingModel GetRequirements(HeroModel hero)
        {
            int currentRating = hero.General.Rating;
            LevelUpRaitingModel result = ratings.Find(x => x.Level == currentRating + 1);
            if (result != null)
            {
                result.UpdateData(hero);
            }
            else
            {
                Debug.Log("not found LevelUpRaiting");
            }
            return result;
        }
    }






}