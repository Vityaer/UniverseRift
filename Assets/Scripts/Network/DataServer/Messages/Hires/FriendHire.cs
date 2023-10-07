namespace Network.DataServer.Messages
{
    public class FriendHire : AbstractHireMessage
    {
        public new string Route => "Heroes/GetFriendHeroes";
    }
}
