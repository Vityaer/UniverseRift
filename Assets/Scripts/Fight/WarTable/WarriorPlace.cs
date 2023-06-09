using Campaign;
using Models.Heroes;
using TMPro;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Fight.WarTable
{
    public class WarriorPlace : MonoBehaviour
    {
        public int Id;
        public Card Card;
        public WarTableController WarTable;


        [SerializeField] private Image ImageHero;
        [SerializeField] private TextMeshProUGUI textLevel;

        public bool IsEmpty => Card == null;
        public HeroModel Hero { get; private set; }

        void Start()
        {
            WarTable = WarTableController.Instance;
        }

        public void SetHero(Card card, HeroModel hero)
        {
            if (card != null)
                Card = card;

            this.Hero = hero;
            card.Select();
            UpdateUI();
        }

        public void OnClickPlace()
        {
            if (Card != null)
                WarTable.UnSelectCard(Card);
        }
        public void ClearPlace()
        {
            if (Card != null)
            {
                Card.Unselect();
                Card = null;
            }
            Hero = null;
            ClearUI();
        }

        public void UpdateUI()
        {
            ImageHero.sprite = Hero?.General.ImageHero;
            ImageHero.enabled = true;
            textLevel.text = Hero.General.Level.ToString();
        }

        private void ClearUI()
        {
            ImageHero.enabled = false;
            ImageHero.sprite = null;
            textLevel.text = string.Empty;
        }

        public void SetEnemy(Unit enemy)
        {
            Hero = enemy.Prefab;
            Hero.PrepareHeroWithLevel(enemy.Level);
            UpdateUI();
        }

    }
}