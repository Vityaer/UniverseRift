using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Misc.Communications
{
    [Serializable]
    public class HeartsContainer
    {
        public List<int> SendedHeartIds;
        public List<int> ReceivedHeartIds;
    }
}
