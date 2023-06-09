using System;
using UnityEngine;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardObject
    {
        [SerializeField] protected float posibility = 100f;
        public float Posibility { get => posibility; }
    }
}