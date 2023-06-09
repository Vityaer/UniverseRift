using System;

namespace UIController.Inventory
{
    [Serializable]
    public class SplinerPosibilityObject
    {
        public string ID;
        public float posibility = 1f;
        public SplinerPosibilityObject(string ID, float percent = 1f)
        {
            this.ID = ID;
            posibility = percent;
        }
    }
}