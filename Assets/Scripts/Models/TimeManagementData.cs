using Models.Data;
using Models.Data.Common;
using System;
using UnityEngine;

namespace Models
{
    //Dates
    [System.Serializable]
    public class TimeManagementData : BaseDataModel
    {
        public RecordsData<DateTime> DateRecords = new RecordsData<DateTime>();
    }
}
