using Fight;
using Fight.HeroControllers.Generals;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Models.Heroes.Actions
{
    [Serializable]
    public class Effect
    {
        [Header("Data")]
        public TypePerformance performance = TypePerformance.Select;
        public TypeEvent typeEvent = TypeEvent.OnStartFight;
        public int countExecutions = -1;
        private HeroController master;

        [Header("Conditions")]
        public List<ConditionEffect> conditions = new List<ConditionEffect>();

        [Header("Actions")]
        public List<ActionEffect> listAction = new List<ActionEffect>();
        private List<HeroController> listTarget = new List<HeroController>();


        //API
        public void CreateEffect(HeroController master)
        {
            this.master = master;
            foreach (var action in listAction)
            {
                action.Master = master;
            }
            RegisterOnEvent(master);
        }

        public void ExecuteEffect()
        {
            if (performance == TypePerformance.Random)
            {
                int rand = UnityEngine.Random.Range(0, listAction.Count);
                listAction[rand].SetNewTarget(listTarget);
                listAction[rand].ExecuteAction();
            }
            else
            {
                foreach (var action in listAction)
                {
                    action.SetNewTarget(listTarget);
                    action.ExecuteAction();
                }
            }
        }

        public void ExecuteSpell(List<HeroController> listTarget)
        {
            if (performance == TypePerformance.Random)
            {
                int rand = UnityEngine.Random.Range(0, listAction.Count);
                listAction[rand].SetNewTarget(listTarget);
                listAction[rand].ExecuteAction();
            }
            else
            {
                foreach (var action in listAction)
                {
                    action.SetNewTarget(listTarget);
                    action.ExecuteAction();
                }
            }
        }

        public void RegisterOnEvent(HeroController master)
        {
            this.master = master;
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