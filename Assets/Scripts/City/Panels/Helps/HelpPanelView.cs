using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace City.Panels.Helps
{
    public class HelpPanelView : BasePanel
    {
        public LocalizeStringEvent MainMessage;

        public RectTransform HelpWrapper;
        
        public GameObject SimpleHelpPanel;
        public GameObject AdvancedHelpPanel;
    }
}