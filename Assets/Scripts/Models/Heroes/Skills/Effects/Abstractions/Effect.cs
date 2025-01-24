using Fight.HeroControllers.Generals;
using Models.Heroes.Skills.Effects.TypeEvents;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public class Effect
    {
        [Header("Data")]
        [LabelWidth(150)] public AbstractTypeEvent TypeEvent;
        [LabelWidth(150)] public int CountExecutions = 0;

        [Header("Conditions")]
        public List<ConditionEffect> Conditions = new();

        [Header("Actions")]
        public List<AbstractAction> Actions = new();

        private List<HeroController> _listTarget = new();
        private HeroController _master;

        //API
        public void CreateEffect(HeroController master, CompositeDisposable disposables)
        {
            _master = master;
            foreach (var action in Actions)
                action.Owner = master;

            RegisterOnEvent(master, disposables);
        }

        public void ExecuteEffect()
        {
            int rand = UnityEngine.Random.Range(0, Actions.Count);
            Actions[rand].SetNewTarget(_listTarget);
            Actions[rand].ExecuteAction();
        }

        public void ExecuteSpell(List<HeroController> listTarget)
        {
            foreach (var action in Actions)
            {
                action.SetNewTarget(listTarget);
                action.ExecuteAction();
            }
        }

        public void RegisterOnEvent(HeroController master, CompositeDisposable disposables)
        {
            _master = master;
            TypeEvent.Subscribe(_master, disposables);
        }
    }
}