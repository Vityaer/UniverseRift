using City.Panels.SelectHeroes;
using Hero;
using Models.City.TrainCamp;
using Models.Heroes;
using System;
using System.Collections.Generic;
using TMPro;
using UIController.Animations;
using UIController.Cards;
using UniRx;
using UnityEngine;

namespace UIController
{
    public class RequireCard : MonoBehaviour, IDisposable
    {
        [SerializeField] private Card card;
        [SerializeField] private TextMeshProUGUI textCountRequirement;

        [Header("Panel select heroes")]
        public OpenClosePanel panelListHeroes;
        public HeroCardsContainerController listCard;
        public RequireCard requireCardInfo;

        private int requireSelectCount = 0;
        private RequirementHeroModel requirementHero;
        private List<GameHero> selectedHeroes = new List<GameHero>();

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public void SetData(RequirementHeroModel requirementHero)
        {
            ClearData();
            this.requirementHero = requirementHero;
            requireSelectCount = requirementHero.count;
            card.SetData(requirementHero);
            UpdateUI();
        }

        public void AddHero(Card card)
        {
            if (selectedHeroes.Count < requireSelectCount)
            {
                card.Select();
                selectedHeroes.Add(card.Hero);
                UpdateUI();
                requireCardInfo.UpdateUI();
            }
        }

        public void RemoveHero(Card card)
        {
            if (selectedHeroes.Count > 0)
            {
                card.Unselect();
                selectedHeroes.Remove(card.Hero);
                UpdateUI();
                requireCardInfo.UpdateUI();
            }
        }

        public void UpdateUI()
        {
            textCountRequirement.text = $"{selectedHeroes.Count}/{requireSelectCount}";
        }

        public bool CheckHeroes()
        {
            bool result = false;
            return result;
        }

        public void OpenListCard()
        {
            //listCard.OnSelect.Subscribe(AddHero).AddTo(_disposables);
            //listCard.OnDiselect.Subscribe(RemoveHero).AddTo(_disposables);
            //List<HeroModel> currentHeroes = GameController.Instance.ListHeroes;
            //currentHeroes = currentHeroes.FindAll(x => x.CheckСonformity(requirementHero));
            //currentHeroes.Remove(TrainCamp.Instance.ReturnSelectHero());
            //listCard.SetList(currentHeroes);
            //listCard.SelectCards(selectedHeroes);
            //panelListHeroes.Open();
            //requireCardInfo.ShowData(requirementHero, selectedHeroes);
            //panelListHeroes.RegisterOnClose(OnClosePanelHeroes);
        }

        void OnClosePanelHeroes()
        {
            panelListHeroes.UnregisterOnClose(OnClosePanelHeroes);
            //listCard.UnRegisterOnSelect(AddHero);
            //listCard.UnRegisterOnUnSelect(RemoveHero);
        }

        public void ClearData()
        {
            selectedHeroes.Clear();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void DeleteSelectedHeroes()
        {
            for (int i = 0; i < selectedHeroes.Count; i++)
            {
                //GameController.Instance.RemoveHero(selectedHeroes[i]);
            }
            ClearData();
        }

        public void ShowData(RequirementHeroModel requirementHero, List<HeroModel> selectedHeroes)
        {
            //this.selectedHeroes = selectedHeroes;
            this.requirementHero = requirementHero;
            requireSelectCount = requirementHero.count;
            card.SetData(requirementHero);
            UpdateUI();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}