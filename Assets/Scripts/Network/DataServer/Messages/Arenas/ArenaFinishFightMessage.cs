using UnityEngine;

namespace Network.DataServer.Messages.Arenas
{
    public class ArenaFinishFightMessage : INetworkMessage
    {
        public int PlayerId;
        public int OpponentId;
        public int Result;

        public string Route => "Arena/FinishFight";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("OpponentId", OpponentId);
                form.AddField("Result", Result);
                return form;
            }
        }
    }
}
