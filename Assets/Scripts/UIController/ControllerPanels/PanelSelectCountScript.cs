using System;
using UIController.Inventory;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ControllerPanels
{
    public class PanelSelectCountScript : MonoBehaviour
    {
        public SubjectCellController cellProduct;
        public CountController countController;
        public ItemSliderController slider;
        public Button btnAction;
        public GameObject panel;

        private int _requireCount = 0;
        private int _storeCount = 0;
        private int _selectedCount = 0;
        private SplinterController _splinterController;
        private Action<int> _actionOnSelectedCount;
        private Action _actionAfterUse;

        public static PanelSelectCountScript Instance { get; private set; }
        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            countController.RegisterOnChangeCount(ChangeCount);
            btnAction.onClick.AddListener(() => SelectedCountDone());
        }

        public void ChangeCount(int count)
        {
            _selectedCount = count;
            slider.SetAmount(count * _requireCount, _requireCount);
        }

        public void Open(SplinterController splinterController, int requireCount, int storeCount)
        {
            RegisterOnSelectedCount(splinterController.GetReward);
            cellProduct.SetItem(splinterController);
            countController.SetMax(storeCount / requireCount);
            this._requireCount = requireCount;
            this._storeCount = storeCount;
            ChangeCount(count: storeCount / requireCount);
            panel.SetActive(true);
        }


        public void RegisterOnActionAfterUse(Action d) { _actionAfterUse += d; }
        private void RegisterOnSelectedCount(Action<int> d) { _actionOnSelectedCount += d; }
        void SelectedCountDone()
        {
            if (_actionOnSelectedCount != null)
            {
                _actionOnSelectedCount(_selectedCount);
                _actionOnSelectedCount = null;
            }
            if (_actionAfterUse != null)
            {
                _actionAfterUse();
                _actionAfterUse = null;
            }
            Close();
        }

        void OnDestroy()
        {
            countController.UnregisterOnChangeCount(ChangeCount);
        }

        public void Close()
        {
            panel.SetActive(false);
        }

    }
}