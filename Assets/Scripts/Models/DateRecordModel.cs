using System;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class DateRecordModel : BaseModel
    {
        public TypeDateRecord type;
        [SerializeField] private string date;
        public DateTime Date { get => FunctionHelp.StringToDateTime(date); set => date = value.ToString(); }
        public DateRecordModel(TypeDateRecord type, DateTime date)
        {
            this.type = type;
            this.date = date.ToString();
        }
    }
}
