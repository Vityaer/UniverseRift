namespace Models
{
    [System.Serializable]
    public class MarketProductModel : BaseModel
    {
        public int countSell;
        public MarketProductModel(string ID, int countSell)
        {
            this.Id = ID;
            this.countSell = countSell;
        }
        public void UpdateData(int newCountSell)
        {
            this.countSell = newCountSell;
        }
    }
}
