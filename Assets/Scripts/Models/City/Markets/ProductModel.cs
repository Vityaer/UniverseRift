namespace Models
{
    [System.Serializable]
    public class ProductModel : BaseModel
    {
        public int CountSell;

        public ProductModel(){ }

        public ProductModel(string ID, int countSell)
        {
            this.Id = ID;
            this.CountSell = countSell;
        }

        public void UpdateData(int newCountSell)
        {
            this.CountSell = newCountSell;
        }
    }
}
