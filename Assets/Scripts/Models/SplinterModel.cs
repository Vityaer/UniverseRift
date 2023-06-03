namespace Models
{
    [System.Serializable]
    public class SplinterModel : BaseModel
    {
        public int Amount;
        public SplinterModel(SplinterController splinterController)
        {
            this.Id = splinterController.splinter.Id;
            this.Amount = splinterController.splinter.Amount;
        }
    }
}
