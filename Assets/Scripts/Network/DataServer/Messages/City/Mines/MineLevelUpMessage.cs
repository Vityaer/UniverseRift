using UnityEngine;

namespace Network.DataServer.Messages.City.Mines
{
    public class MineLevelUpMessage : INetworkMessage
    {
        public int PlayerId;
        public int MineId;

        public string Route => "Mines/LevelUp";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("MineId", MineId);
                return form;
            }
        }
    }
}
