namespace Network.DataServer.Messages
{
    public class FriendHire : AbstractHireMessage
    {
        public override string Route => "Heroes/GetFriendHeroes";
    }
}
