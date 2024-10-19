using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Hero;
using Models.Heroes.Skills.Actions.Effects;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public partial class AbstractAction
    {
        private void ExecuteSimpleAction()
        {
            Debug.Log("simple action: " + SimpleAction.ToString());

        }


       

        private void ExecuteSpecial()
        {

        }
        private void ExecuteOther()
        {

        }
    }
}
