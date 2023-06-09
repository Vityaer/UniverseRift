using UnityEngine;

namespace Models
{
    //BaseRecord
    [System.Serializable]
    public class BaseRecordModel
    {
        [SerializeField] protected string key;
        public string Key { get => key; }
    }
}
