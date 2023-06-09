using System;
using UnityEngine;

namespace Models
{
    //Dates
    [System.Serializable]
    public class TimeManagementModel : BaseModel
    {
        [SerializeField] private DateTimeRecordsModel dateRecords = new DateTimeRecordsModel();
        public void SetRecordDate(string name, DateTime date)
        {
            dateRecords.SetRecord(name, date);
        }
        public DateTime GetRecordDate(string name)
        {
            return FunctionHelp.StringToDateTime(dateRecords.GetRecord(name).value);
        }
        public bool CheckRecordDate(string name) { return dateRecords.CheckRecord(name); }
    }
}
