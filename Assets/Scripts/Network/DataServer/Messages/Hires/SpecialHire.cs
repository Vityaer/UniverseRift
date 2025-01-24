namespace Network.DataServer.Messages
{
    public class SpecialHire : AbstractHireMessage
    {
        public override string Route => "Heroes/GetSpecialHeroes";
    }
}
