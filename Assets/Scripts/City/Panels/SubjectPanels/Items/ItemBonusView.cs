using LocalizationSystems;
using System;
using UIController.Inventory;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace City.Panels.SubjectPanels.Items
{
    public class ItemBonusView : ScrollableUiView<Bonus>
    {
        public LocalizeStringEvent _bonusLocalizeText;
        
        private ILocalizationSystem _localizationSystem;

        public void Init(ILocalizationSystem localizationSystem)
        {
            _localizationSystem = localizationSystem;
        }

        public override void SetData(Bonus data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect);
            UpdateUi();
        }

        private void UpdateUi()
        {
            _bonusLocalizeText.StringReference = _localizationSystem.GetLocalizedContainer($"{Data.Name}BonusName");
            if (_bonusLocalizeText.StringReference.TryGetValue("Bonus", out var variable))
            {
                var stringVariable = variable as StringVariable;
                stringVariable.Value = GetBonusAmountText(); 
            }
            else
            {
                var stringVariable = new StringVariable();
                stringVariable.Value = GetBonusAmountText();
                _bonusLocalizeText.StringReference.Add("Bonus", stringVariable);
            }
            _bonusLocalizeText.StringReference.RefreshString();
        }

        private string GetBonusAmountText()
        {
            var sign = Data.Count > 0 ? "+ " : "- ";
            var color = Data.Count > 0 ? "green" : "red";
            var bonusText = Mathf.Abs(Data.Count).ToString("N0");
            return $"<color={color}>{sign}{bonusText}</color>";
        }
    }
}
