using UnityEngine;

namespace Network.DataServer.Messages.Guilds
{
    public class CreateNewGuildMessage : INetworkMessage
    {
        public int PlayerId;
        public string GuildName;
        public string IconPath;

        public string Route => "Guild/CreateNewGuild";

        public WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("GuildName", GuildName);
                form.AddField("IconPath", IconPath);
                return form;
            }
        }
    }
}
