using Fight.HeroStates;
using UnityEngine;

namespace Fight.HeroControllers.Generals
{
    public partial class HeroController : MonoBehaviour
    {
        public void StartWait()
        {
            EndTurn();
        }

        public void StartDefend()
        {
            var buffDefend = new BuffModel(BuffType.Armor, 1f);
            _statusState.SetBuff(buffDefend);
            EndTurn();
        }

        public void UseSpecialSpell()
        {
            if (_statusState.Stamina >= 100f)
            {
                DoSpell();
            }
        }

        public void SetDebuff(State debuff, int rounds)
        {
            Debug.Log("set debuff", gameObject);
            this._statusState.SetDebuff(debuff, rounds);
        }
    }
}