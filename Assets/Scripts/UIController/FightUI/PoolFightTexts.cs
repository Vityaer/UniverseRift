using System.Collections.Generic;
using UnityEngine;

namespace UIController.FightUI
{
    public class PoolFightTexts : MonoBehaviour
    {
        [SerializeField] private List<DamageHealText> _listFightTextChangeHP = new List<DamageHealText>();
        [SerializeField] private GameObject _prefabTextFight;

        public static PoolFightTexts Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void ShowDamage(float damage, Vector2 pos)
        {
            GetFightText().PlayDamage(damage, pos);
        }

        public void ShowHeal(float heal, Vector2 pos)
        {
            GetFightText().PlayHeal(heal, pos);
        }

        public void ShowMessage(string message, Vector2 pos)
        {
            // GetFightText()?.PlayMessage(message, pos);
        }

        private DamageHealText GetFightText()
        {
            DamageHealText result = _listFightTextChangeHP.Find(x => x.InWork == false);
            if (result == null) result = Instantiate(_prefabTextFight).GetComponent<DamageHealText>();
            return result;
        }

    }
}