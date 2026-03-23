using Models.Inventory.Splinters;
using UnityEngine;
using Utils.DataCheckUtils;

namespace UIController.ItemVisual
{
    public class GameRandomSplinterData
    {
        private readonly RandomSplinterModel m_model;
        private readonly float m_percentage;
        
        public RandomSplinterModel Model => m_model;
        public float Percentage => m_percentage;
        
        public GameRandomSplinterData(RandomSplinterModel model, float percentage)
        {
            m_model = model.ThrowIfArgumentNull();
            m_percentage = Mathf.Clamp01(percentage);
        }
    }
}