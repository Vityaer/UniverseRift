using UnityEngine;
namespace Network.DataServer.Messages
{
    public abstract class AbstractHireMessage : AbstractMessage, INetworkMessage
    {
        public int Count;

        //TODO: неправильно работает, дочерние классы не переопределяют свойство
        public new string Route => "Heroes/GetSimpleHeroes";

        public new WWWForm Form
        {
            get
            {
                var form = new WWWForm();
                form.AddField("PlayerId", PlayerId);
                form.AddField("Count", Count);
                return form;
            }
        }
    }
}
