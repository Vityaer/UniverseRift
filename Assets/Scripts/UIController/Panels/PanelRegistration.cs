using City.Buildings;
using Common;
using TMPro;
using UIController.ControllerPanels;
using UnityEngine.UI;

public class PanelRegistration : BasePanelScript
{
    public TMP_InputField inputFieldNewNamePlayer;
    public FormRegisterController registrationController;
    public Button buttonRegistration;
    private string currentName;

    public void SaveNewName()
    {
        currentName = inputFieldNewNamePlayer.text;
        registrationController.CreateAccount(currentName);
        Close();
    }

    public void OnChangeNewName()
    {
        if (inputFieldNewNamePlayer.text.Equals(currentName) == false)
        {
            var enoughLength = inputFieldNewNamePlayer.text.Length > 3;
            buttonRegistration.interactable = enoughLength;
        }
        else
        {
            buttonRegistration.interactable = false;
        }
    }

    protected override void OnOpen()
    {
        currentName = GameController.Instance.player.GetPlayerInfo.Name;
        inputFieldNewNamePlayer.text = currentName;
        inputFieldNewNamePlayer.Select();
    }
}