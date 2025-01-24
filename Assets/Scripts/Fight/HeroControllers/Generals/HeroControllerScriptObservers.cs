using Fight.Common.Strikes;
using Fight.Grid;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        //Register
        private ReactiveCommand<HeroController> _onStartFight = new();
        private ReactiveCommand<Strike> _onStrikeFinish = new();
        private ReactiveCommand<HeroController> _onTakeDamage = new();
        private ReactiveCommand<HeroController> _onDeath = new();
        private ReactiveCommand<(HeroController, float)> _onHealthChangePercent = new();
        private ReactiveCommand<(HeroController, float)> _onHeal = new();
        private ReactiveCommand<List<HeroController>> _onStrike = new();
        private ReactiveCommand<List<HeroController>> _onSpell = new();
        private ReactiveCommand<List<HeroController>> _onListSpell = new();

        public IObservable<HeroController> OnStartFight => _onStartFight;
        public IObservable<HeroController> OnTakeDamage => _onTakeDamage;
        public IObservable<HeroController> OnDeath => _onDeath;
        public IObservable<(HeroController, float)> OnHealthChangePercent => _onHealthChangePercent;
        public IObservable<(HeroController, float)> OnHeal => _onHeal;
        public IObservable<List<HeroController>> OnStrike => _onStrike;
        public IObservable<Strike> OnStrikeFinish => _onStrikeFinish;
        public IObservable<List<HeroController>> OnSpell => _onSpell;
        public IObservable<List<HeroController>> OnChangeListSpell => _onListSpell;


        private static Action<HeroController> observerStartAction;
        public static void RegisterOnStartAction(Action<HeroController> d) { observerStartAction += d; }
        public static void UnregisterOnStartAction(Action<HeroController> d) { observerStartAction -= d; }
        protected void OnStartAction() { if (observerStartAction != null) observerStartAction(this); }

        private static Action observerEndAction;
        public static void RegisterOnEndAction(Action d) { observerEndAction += d; }
        public static void UnregisterOnEndAction(Action d) { observerEndAction -= d; }
        protected void OnEndAction() { if (observerEndAction != null) observerEndAction(); }

        private Action observerEndSelectCell;
        public void RegisterOnEndSelectCell(Action d) { observerEndSelectCell += d; }
        public void UnregisterOnEndSelectCell(Action d) { observerEndSelectCell -= d; }

        protected void OnEndSelectCell()
        {
            HexagonCell.UnregisterOnClick(SelectHexagonCell);
            observerEndSelectCell?.Invoke();
        }
    }
}