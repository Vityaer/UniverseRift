using City.Panels.SelectHeroes;
using Hero;
using System.Collections.Generic;
using UIController.Cards;
using UniRx;
using UnityEngine;

namespace City.Buildings.Abstractions
{
    public class BuildingWithHeroesList<T> : BaseBuilding<T> where T : BaseBuildingView
    {

        protected List<GameHero> ListHeroes = new List<GameHero>();

        public virtual void SelectHero(Card cardHero) { }

        public virtual void UnselectHero(Card cardHero) { }

        protected virtual void FilterHeroes(List<GameHero> heroes) { }

        protected void LoadListHeroes()
        {
            //FilterHeroes(ListHeroes);
            //ListHeroesController.ShowCards(ListHeroes);
            //ListHeroesController.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
        }
    }
}