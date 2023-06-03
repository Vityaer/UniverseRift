using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    //Int records
    [System.Serializable]
    public class IntRecordsModel : BaseModel
    {
        [SerializeField] private List<IntRecordModel> list = new List<IntRecordModel>();
        public IntRecordModel GetRecord(string key, int defaultNum = 0)
        {
            IntRecordModel result = list.Find(x => x.Key.Equals(key));
            if (result == null)
            {
                result = new IntRecordModel(key, defaultNum);
                list.Add(result);
            }
            return result;
        }
        public void SetRecord(string key, int value) { GetRecord(key).value = value; }
        public bool CheckRecord(string key) { return (list.Find(x => x.Key.Equals(key)) != null); }
    }
}
