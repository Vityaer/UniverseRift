using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    //"String records
    [System.Serializable]
    public class StringRecordsModel : BaseModel
    {
        [SerializeField] private List<StringRecordModel> list = new List<StringRecordModel>();
        public StringRecordModel GetRecord(string key, string defaultValue = "")
        {
            StringRecordModel result = list.Find(x => x.Key.Equals(key));
            if (result == null)
            {
                result = new StringRecordModel(key, defaultValue);
                list.Add(result);
            }
            return result;
        }
        public void SetRecord(string key, string value) { GetRecord(key).value = value; }
        public bool CheckRecord(string key) { return (list.Find(x => x.Key.Equals(key)) != null); }
    }
}
