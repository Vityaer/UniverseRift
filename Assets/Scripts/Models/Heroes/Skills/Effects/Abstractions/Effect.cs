using Fight.HeroControllers.Generals;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public class Effect
    {
        [Header("Data")]
        [LabelWidth(150)] public TypePerformance Performance = TypePerformance.Select;
        [LabelWidth(150)] public TypeEvent TypeEvent = TypeEvent.OnStartFight;
        [LabelWidth(150)] public int CountExecutions = 0;

        [Header("Conditions")]
        public List<ConditionEffect> Conditions = new();

        [Header("Actions")]
        public List<AbstractAction> Actions = new();

        private List<HeroController> _listTarget = new();
        private HeroController _master;

        //API
        public void CreateEffect(HeroController master)
        {
            this._master = master;
            foreach (var action in Actions)
            {
                action.Owner = master;
            }
            RegisterOnEvent(master);
        }

        public void ExecuteEffect()
        {
            if (Performance == TypePerformance.Random)
            {
                int rand = UnityEngine.Random.Range(0, Actions.Count);
                Actions[rand].SetNewTarget(_listTarget);
                Actions[rand].ExecuteAction();
            }
            else
            {
                foreach (var action in Actions)
                {
                    action.SetNewTarget(_listTarget);
                    action.ExecuteAction();
                }
            }
        }

        public void ExecuteSpell(List<HeroController> listTarget)
        {
            if (Performance == TypePerformance.Random)
            {
                int rand = UnityEngine.Random.Range(0, Actions.Count);
                Actions[rand].SetNewTarget(listTarget);
                Actions[rand].ExecuteAction();
            }
            else
            {
                foreach (var action in Actions)
                {
                    action.SetNewTarget(listTarget);
                    action.ExecuteAction();
                }
            }
        }

        public void RegisterOnEvent(HeroController master)
        {
            this._master = master;
            switch (TypeEvent)
            {
                case TypeEvent.OnStartFight:
                    master.RegisterOnStartFight(ExecuteEffect);
                    break;
                case TypeEvent.OnStrike:
                    master.RegisterOnStrike(ExecuteSpell);
                    break;
                case TypeEvent.OnTakingDamage:
                    master.RegisterOnTakingDamage(ExecuteEffect);
                    break;
                case TypeEvent.OnDeathHero:
                    master.RegisterOnDeathHero(ExecuteEffect);
                    break;
                case TypeEvent.OnHPLess50:
                    master.RegisterOnHPLess50(ExecuteEffect);
                    break;
                case TypeEvent.OnHPLess30:
                    master.RegisterOnHPLess30(ExecuteEffect);
                    break;
                case TypeEvent.OnHeal:
                    master.RegisterOnHeal(ExecuteEffect);
                    break;
                case TypeEvent.OnSpell:
                    master.RegisterOnSpell(ExecuteSpell);
                    break;
                case TypeEvent.OnDeathFriend:
                    break;
                case TypeEvent.OnDeathEnemy:
                    break;
                case TypeEvent.OnEndRound:
                    break;
            }
        }
    }
}