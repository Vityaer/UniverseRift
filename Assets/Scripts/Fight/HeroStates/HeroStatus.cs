using System.Collections.Generic;
using UnityEngine;

namespace Fight.Common.HeroStates
{
    public partial class HeroStatus : MonoBehaviour
    {
        private List<BuffModel> _listBuff = new List<BuffModel>();

        public void SetBuff(BuffModel buff)
        {
            _listBuff.Add(buff);
        }

        public int GetAllBuffArmor()
        {
            int armor = 0;
            var armorBuffs = _listBuff.FindAll(x => x.Type == BuffType.Armor);
            for (int i = 0; i < armorBuffs.Count; i++)
            {
                armor += (int)Mathf.Round(armorBuffs[i].GetCurrentAmount);
            }
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
