using System;

namespace Models
{
    [System.Serializable]
    public class DateTimeRecordModel : BaseRecordModel
    {

        public string value;
        public DateTimeRecordModel(string key)
        {
            this.key = key;
            this.value = FunctionHelp.GetDateTimeNow().ToString();
        }
        public DateTimeRecordModel(string key, DateTime value)
        {
            this.key = key;
            this.value = value.ToString();
        }
        public void SetNewData(DateTime newDateTime)
        {
            value = newDateTime.ToString();
        }
    }
}
