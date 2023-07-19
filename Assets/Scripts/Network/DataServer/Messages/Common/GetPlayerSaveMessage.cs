using UnityEngine;

namespace Network.DataServer.Messages.Common
{
    public class GetPlayerSaveMessage : AbstractMessage
    {
        public new string Route => "GameController/GetPlayerSave";

        public new WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                return form;
            }
        }
    }
}
