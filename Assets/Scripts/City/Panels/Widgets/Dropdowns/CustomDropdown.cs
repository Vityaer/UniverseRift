using System;
using TMPro;
using UnityEngine;

namespace City.Panels.Widgets.Dropdowns
{
    public class CustomDropdown : TMP_Dropdown
    {
        public event Action OnOpenDropdown;
        public event Action OnCloseDropdown;

        protected override GameObject CreateDropdownList(GameObject template)
        {
            OnOpenDropdown?.Invoke();
            return base.CreateDropdownList(template);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            OnCloseDropdown?.Invoke();
            base.DestroyDropdownList(dropdownList);
        }
    }
}
