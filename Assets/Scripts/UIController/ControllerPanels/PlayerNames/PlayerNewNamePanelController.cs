using UiExtensions.Scroll.Interfaces;
using VContainer.Unity;

namespace UIController.ControllerPanels.PlayerNames
{
    public class PlayerNewNamePanelController : UiPanelController<PlayerNewNamePanelView>, IStartable
    {

        private string currentName;

        public override void Start()
        {
            base.Start();
            //View.PayNewNameButton.RegisterOnBuy(SaveNewName);
        }

        public void OnChangeNewName()
        {
            if (View.NewNamePlayerInputField.text.Equals(currentName) == false)
            {
                if (View.NewNamePlayerInputField.text.Length > 3)
                {
                    View.PayNewNameButton.Enable();
                }
                else
                {
                    View.PayNewNameButton.Disable();
                }
            }
            else
            {
                View.PayNewNameButton.Disable();
            }
        }
        public void SaveNewName(int count)
        {
            currentName = View.NewNamePlayerInputField.text;
            //View.mainPlayerController.SaveNewName(inputFieldNewNamePlayer.text);
            Close();
        }
        protected void OnOpen()
        {
            //currentName = GameController.Instance.player.GetPlayerInfo.Name;
            //inputFieldNewNamePlayer.text = currentName;
            //inputFieldNewNamePlayer.Select();
        }
    }
}