using System.Collections.Generic;
using UnityEngine;

namespace Fight.HeroStates
{
    public partial class HeroState : MonoBehaviour
    {
        private List<BuffModel> _listBuff = new List<BuffModel>();

        public void SetBuff(BuffModel buff)
        {
            _listBuff.Add(buff);
        }

        public int GetAllBuffArmor()
        {
            int armor = 0;
            List<BuffModel> armorBuffs = _listBuff.FindAll(x => x.Type == BuffType.Armor);
            for (int i = 0; i < armorBuffs.Count; i++)
            {
                armor += (int)Mathf.Round(armorBuffs[i].GetCurrentAmount);
            }
            Debug.Log("bonus armor: " + armor.ToString());
            return armor;
        }

        private void CheckBuffs()
        {
            for (int i = 0; i < _listBuff.Count; i++)
                _listBuff[i].FinishRound();
            _listBuff = _listBuff.FindAll(x => x.Rounds.Count > 0);
        }
    }

    

}
