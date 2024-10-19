using Fight.HeroControllers.Generals;
using Models.Heroes.Actions;
using UnityEngine;

namespace Models.Heroes.Skills.Actions.Imps.SimpleActions
{
    public class HealAction : ContinuousAction
    {
        public override void ExecuteAction()
        {
            foreach (HeroController heroController in ListTarget)
            {
                heroController.GetHeal((int)Mathf.Floor(Amount), TypeNumber);
            }
        }
    }
}
