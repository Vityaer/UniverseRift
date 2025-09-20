using LocalizationSystems;
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
        
        private ILocalizationSystem m_localizationSystem;

        private bool m_available = true;
        
        public void Init(ILocalizationSystem localizationSystem)
        {
            m_localizationSystem = localizationSystem;
            m_available = true; 
        }

        public override void SetData(Bonus data, ScrollRect scrollRect)
        {
            m_available = true;
            base.SetData(data, scrollRect);
            UpdateUi();
        }
        
        public void SetAvailable(bool available)
        {
            m_available = available;
            UpdateUi();
        }

        private void UpdateUi()
        {
            _bonusLocalizeText.StringReference = m_localizationSystem.GetLocalizedContainer($"{Data.Name}BonusName");
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

            if (!m_available)
            {
                color = "gray";
            }

            var bonusText = Mathf.Abs(Data.Count).ToString("N0");
            string result = $"<color={color}>{sign}{bonusText}</color>";
            return result;
        }
    }
}
