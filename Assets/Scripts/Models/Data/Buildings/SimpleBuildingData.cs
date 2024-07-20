using Models.Data;
using Models.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    //Simple building save
    [System.Serializable]
    public class SimpleBuildingData : BaseDataModel
    {
        public RecordsData<int> IntRecords = new();
        public RecordsData<string> DateRecords = new();
    }
}
