using Models.Heroes.HeroCharacteristics;
using System;

namespace UIController.Inventory
{
    [Serializable]
    public class Bonus
    {
        public TypeCharacteristic Name;
        public float Count;
    }
}