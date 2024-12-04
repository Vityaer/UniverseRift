using UnityEngine;
namespace Network.DataServer.Messages
{
    public class SimpleHire : AbstractHireMessage
    {
        public override string Route => "Heroes/GetSimpleHeroes";
    }
}
