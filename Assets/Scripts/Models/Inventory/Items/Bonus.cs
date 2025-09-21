using Models.Heroes.HeroCharacteristics;
using System;
using UnityEngine.Serialization;

namespace UIController.Inventory
{
    [Serializable]
    public class Bonus
    {
        public TypeCharacteristic Name;
        [FormerlySerializedAs("Count")] public float Value;
    }
}