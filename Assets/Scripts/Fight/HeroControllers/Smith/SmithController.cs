using Fight.Grid;
using Fight.HeroControllers.Generals;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroControllers.Smith
{
    public class SmithController : HeroController
    {
        protected override void DoSpell()
        {
            statusState.ChangeStamina(-100);
            Animator.Play("Spell");
        }

        private void AroundStun()
        {
            List<NeighbourCell> neighbours = myPlace.GetAvailableNeighbours;
            listTarget.Clear();
            for (int i = 0; i < neighbours.Count; i++)
            {
                Debug.Log(neighbours[i].Cell.gameObject.name);
                if (neighbours[i].GetHero != null)
                {
                    listTarget.Add(neighbours[i].GetHero);
                    // Debug.Log("find hero", neighbours[i].GetHero.gameObject);
                }
            }
            OnSpell(listTarget);
            EndTurn();
        }
    }
}