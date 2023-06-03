using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    //DateTime Records	
    [System.Serializable]
    public class DateTimeRecordsModel : BaseModel
    {
        [SerializeField] private List<DateTimeRecordModel> list = new List<DateTimeRecordModel>();
        public DateTimeRecordModel GetRecord(string key)
        {
            DateTimeRecordModel result = list.Find(x => x.Key.Equals(key));
            if (result == null)
            {
                result = new DateTimeRecordModel(key);
                list.Add(result);
            }
            return result;
        }
        public void SetRecord(string key, DateTime value) { GetRecord(key).SetNewData(value); }
        public bool CheckRecord(string key) { return (list.Find(x => x.Key.Equals(key)) != null); }
    }
}
