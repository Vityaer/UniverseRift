namespace Models
{
    [System.Serializable]
    public class IntRecordModel : BaseRecordModel
    {
        public int value = 0;
        public IntRecordModel(string key, int value = 0)
        {
            this.key = key;
            this.value = value;
        }
    }
}
