namespace Models
{
    [System.Serializable]
    public class StringRecordModel : BaseRecordModel
    {
        public string value;
        public StringRecordModel(string key, string value = "")
        {
            this.key = key;
            this.value = value;
        }
    }
}
