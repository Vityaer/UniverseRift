namespace Models
{
    [System.Serializable]
    public class ItemModel : BaseModel
    {
        public int Amount;
        public ItemModel(ItemController itemController)
        {
            this.Id = itemController.item.Id;
            this.Amount = itemController.Amount;
        }
    }
}
