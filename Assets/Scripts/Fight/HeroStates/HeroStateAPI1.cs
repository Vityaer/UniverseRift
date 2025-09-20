using System.Collections.Generic;
using Fight.Common.Rounds;
using UnityEngine;

namespace Fight.Common.HeroStates
{
    public partial class HeroStatus : MonoBehaviour
    {
        //Debuff
        private Debuff debuff = new Debuff();
        private void SaveDebuff(State state, int round)
        {
            currentState = state;
            debuff.Update(state, round);
            FightEffectController.Instance.CastEffectStateOnHero(gameObject, debuff.state);
        }

        //Dots	
        private List<Dot> dots = new List<Dot>();
        private void SaveDot(DotType typeDot, List<Round> rounds)
        {
            dots.Add(new Dot(typeDot, rounds));
        }



        //General loop
        public void RoundFinish()
        {
            debuff.RoundFinish();
            if (debuff.IsFinish && debuff.state != State.Clear)
            {
                if (FightEffectController.Instance == null) Debug.Log("FightEffectControllerScript нету");
                if (debuff == null) Debug.Log("debuff null");
                if (gameObject == null) Debug.Log("gameObject null");
                FightEffectController.Instance.ClearEffectStateOnHero(gameObject, debuff.state);
                currentState = State.Clear;
            }

            for (int i = dots.Count - 1; i >= 0; i--)
            {
                FightEffectController.Instance.CreateDot(transform, dots[i].type);
                dots[i].RoundFinish(heroController);
                if (dots[i].IsFinish)
                {
                    dots.RemoveAt(i);
                }
            }
            CheckBuffs();
        }
    }
}
