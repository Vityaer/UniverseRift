using Models.Common.BigDigits;
using System;
using System.Xml.Linq;

namespace Models.Data
{
    [System.Serializable]
    public class AchievmentData : BaseDataModel
    {
        public string ModelId;
        public int CurrentStage;
        public BigDigit Progress;
        public bool IsComplete;

        public void Clear()
        {
            CurrentStage = 0;
            Progress.Clear();
            IsComplete = false;
        }
    }
}
