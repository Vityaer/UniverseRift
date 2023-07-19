using Models.City.TrainCamp;
using System.Collections.Generic;
using UIController;
using UnityEngine;

namespace City.TrainCamp
{
    public class ListRequirementHeroesUI : MonoBehaviour
    {
        public HeroEvolutionPanelController mainController;

        [SerializeField] private List<RequireCard> requireCards = new List<RequireCard>();
        private List<RequirementHeroModel> requirementHeroes = new List<RequirementHeroModel>();

        public void SetData(List<RequirementHeroModel> requirementHeroes)
        {
            this.requirementHeroes = requirementHeroes;
            for (int i = 0; i < requireCards.Count; i++)
            {
                if (i < requirementHeroes.Count)
                {
                    requireCards[i].SetData(requirementHeroes[i]);
                }
                else
                {
                    requireCards[i].Hide();
                }
            }
        }

        public bool GetCanLevelUpRating()
        {
            bool result = false;
            return result;
        }

        public void HeroSelectDiselect()
        {
            mainController.CheckHeroes();
        }

        public bool IsAllDone()
        {
            bool result = true;
            for (int i = 0; i < requirementHeroes.Count; i++)
            {
                if (requireCards[i].CheckHeroes())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public void ClearData()
        {
            foreach (RequireCard requireCard in requireCards)
            {
                requireCard.ClearData();
            }
        }
        public void DeleteSelectedHeroes()
        {
            for (int i = 0; i < requirementHeroes.Count; i++)
            {
                requireCards[i].DeleteSelectedHeroes();
            }
        }
    }
}