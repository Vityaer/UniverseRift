using Models.City.TrainCamp;
using Models.Heroes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class Card : MonoBehaviour
    {
        public HeroModel Hero;
        public bool Selected = false;

        [SerializeField] private Image _imageUI;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private Image _panelSelect;
        [SerializeField] private VocationView _vocationUI;
        [SerializeField] private RaceView _raceUI;

        private RatingHero _ratingController;
        private ListCardOnWarTable _listCardController;

        public void SetData(RequirementHeroModel requirementHero)
        {
            gameObject.SetActive(true);
            _levelUI.text = string.Empty;
            _ratingController.ShowRating(requirementHero.rating);
            // vocationUI.SetData(requirementHero.);
            // raceUI.SetData(requirementHero.);
            SetImage(requirementHero.GetData);
        }

        public void ChangeInfo(HeroModel hero)
        {
            this.Hero = hero;
            UpdateUI();
        }

        public void ChangeInfo(HeroModel hero, ListCardOnWarTable listCardController)
        {
            this.Hero = hero;
            this._listCardController = listCardController;
            UpdateUI();
        }

        private void UpdateUI()
        {
            _imageUI.sprite = Hero.General.ImageHero;
            _levelUI.text = Hero.General.Level.ToString();
            _ratingController.ShowRating(Hero.General.RatingHero);

        }

        private void SetImage(HeroModel data)
        {
            _imageUI.sprite = data.General.ImageHero;
        }

        //API
        public void ClickOnCard()
        {
            if (Selected == false)
            {
                _listCardController.SelectCard(this);
            }
            else
            {
                _listCardController.UnselectCard(this);
            }
        }

        public void Select()
        {
            Selected = true;
            _panelSelect.enabled = true;
        }

        public void Unselect()
        {
            Selected = false;
            _panelSelect.enabled = false;
        }

        public void Clear()
        {
            _imageUI.sprite = null;
            _levelUI.text = string.Empty;
            _ratingController.Hide();
            gameObject.SetActive(false);
        }

        public void DestroyCard()
        {
            _listCardController.RemoveCardFromList(this);
            Destroy(gameObject);
        }
    }
}