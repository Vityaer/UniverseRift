using System;
using System.Collections.Generic;
using Fight.Common.Strikes;
using Fight.Grid;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        //Register
        private readonly ReactiveCommand<HeroController> m_onStartFight = new();
        private readonly ReactiveCommand<Strike> m_onStrikeFinish = new();
        private readonly ReactiveCommand<HeroController> m_onTakeDamage = new();
        private readonly ReactiveCommand<HeroController> m_onDeath = new();
        private readonly ReactiveCommand<(HeroController, float)> m_onHealthChangePercent = new();
        private readonly ReactiveCommand<(HeroController, float)> m_onHeal = new();
        private readonly ReactiveCommand<List<HeroController>> m_onStrike = new();
        private readonly ReactiveCommand<List<HeroController>> m_onSpell = new();
        private readonly ReactiveCommand<List<HeroController>> m_onListSpell = new();

        public IObservable<HeroController> OnStartFight => m_onStartFight;
        public IObservable<HeroController> OnTakeDamage => m_onTakeDamage;
        public IObservable<HeroController> OnDeath => m_onDeath;
        public IObservable<(HeroController, float)> OnHealthChangePercent => m_onHealthChangePercent;
        public IObservable<(HeroController, float)> OnHeal => m_onHeal;
        public IObservable<List<HeroController>> OnStrike => m_onStrike;
        public IObservable<Strike> OnStrikeFinish => m_onStrikeFinish;
        public IObservable<List<HeroController>> OnSpell => m_onSpell;
        public IObservable<List<HeroController>> OnChangeListSpell => m_onListSpell;


        private static Action<HeroController> _observerStartAction;
        public static void RegisterOnStartAction(Action<HeroController> d) { _observerStartAction += d; }
        public static void UnregisterOnStartAction(Action<HeroController> d) { _observerStartAction -= d; }
        protected void OnStartAction() { if (_observerStartAction != null) _observerStartAction(this); }

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