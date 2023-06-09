using System;
using UnityEngine;

namespace Models
{
    //Simple building save
    [System.Serializable]
    public class SimpleBuildingModel : BaseModel
    {
        [SerializeField] private IntRecordsModel intRecords = new IntRecordsModel();
        [SerializeField] private DateTimeRecordsModel dateRecords = new DateTimeRecordsModel();
        public void SetRecordInt(string name, int num)
        {
            intRecords.SetRecord(name, num);
        }
        public void SetRecordDate(string name, DateTime date)
        {
            dateRecords.SetRecord(name, date);
        }
        public int GetRecordInt(string name, int defaultNum = 0)
        {
            return intRecords.GetRecord(name, defaultNum).value;
        }
        public DateTime GetRecordDate(string name)
        {
            return FunctionHelp.StringToDateTime(dateRecords.GetRecord(name).value);
        }
        public bool CheckRecordInt(string name) { return intRecords.CheckRecord(name); }
        public bool CheckRecordDate(string name) { return dateRecords.CheckRecord(name); }
    }
}
