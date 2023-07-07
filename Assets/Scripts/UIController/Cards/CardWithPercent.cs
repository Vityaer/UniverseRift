using Hero;
using Models;
using Models.Heroes;
using TMPro;
using UnityEngine;

namespace UIController.Cards
{
    public class CardWithPercent : MonoBehaviour
    {
        public Card cardInfo;
        public TextMeshProUGUI textPercent;

        public void SetData(HeroModel model, float percent)
        {
            var hero = new GameHero(model, new HeroData());
            cardInfo.SetData(hero);
            textPercent.text = $"{percent}%";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}