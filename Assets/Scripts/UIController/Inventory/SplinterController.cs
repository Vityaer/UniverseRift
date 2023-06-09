using Hero;
using System;
using UnityEngine;

namespace UIController.Inventory
{
    [System.Serializable]
    public class SplinterController : VisualAPI, ICloneable
    {
        [SerializeField] private Splinter _splinter;
        public Splinter splinter { get => _splinter; }
        public void ClickOnItem()
        {
            InventoryController.Instance.OpenInfoItem(this, withControl: true);
        }
        public SplinterController(Splinter splinter, int amount)
        {
            _splinter = (Splinter)splinter.Clone();
            _splinter.SetAmount(amount);
        }
        public SplinterController(Splinter splinter)
        {
            _splinter = (Splinter)splinter.Clone();
        }
        public SplinterController() : base()
        {
            _splinter = null;
        }
        protected ThingUI UI;
        public void SetUI(ThingUI UI)
        {
            this.UI = UI;
            UpdateUI();
        }
        public void UpdateUI()
        {
            UI?.UpdateUI(this);
        }
        public void ClearUI()
        {
            UI = null;
        }
        public VisualAPI GetVisual()
        {
            return this as VisualAPI;
        }
        public void GetReward(int count = 1)
        {
            splinter.GetReward(count);
            if (splinter.Amount == 0)
            {
                InventoryController.Instance.DropSplinter(this);
            }
        }
        public void IncreaseAmount(int count) { splinter.AddAmount(count); }
        public object Clone()
        {
            return new SplinterController { _splinter = _splinter };
        }
        public int CountReward { get => splinter.CountReward; }
        public bool IsCanUse { get => splinter.IsCanUse; }
    }
}