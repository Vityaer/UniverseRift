using TMPro;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.Registrations
{
    public class RegistrationPanelView : BasePanel
    {
        public TMP_Text Label;
        public TMP_InputField InputFieldNewNamePlayer;
        public Button StartRegistrationButton;

        [Header("Test")]
        public bool OverrideId;
        public int PlayerId;
    }
}
