using Fight.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public class Effect
    {
        [Header("Data")]
        public TypePerformance performance = TypePerformance.Select;
        public TypeEvent typeEvent = TypeEvent.OnStartFight;
        public int countExecutions = 0;

        [Header("Conditions")]
        public List<ConditionEffect> Conditions = new List<ConditionEffect>();

        [Header("Actions")]
        public List<ActionEffect> Actions = new List<ActionEffect>();

        private HeroController _master;
        private List<HeroController> _listTarget = new List<HeroController>();

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
            if (performance == TypePerformance.Random)
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
            if (performance == TypePerformance.Random)
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
            switch (typeEvent)
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