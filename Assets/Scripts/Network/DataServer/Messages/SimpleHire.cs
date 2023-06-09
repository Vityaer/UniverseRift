using UnityEngine;
namespace Network.DataServer.Messages
{
    public class SimpleHire : AbstractHireMessage
    {
        public new string Route => "Heroes/GetSimpleHeroes";
    }
}
