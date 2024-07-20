using Models.Common.BigDigits;
using Network.DataServer.Models.Guilds;
using System.Collections.Generic;

namespace City.Buildings.Guild.Utils
{
    public class RecruitDonateComparer : IComparer<RecruitData>
    {
        public int Compare(RecruitData x, RecruitData y)
        {
            var xDamageAmount = new BigDigit(x.DonateMantissa, x.DonateE10);
            var yDamageAmount = new BigDigit(y.DonateMantissa, y.DonateE10);

            if (xDamageAmount > yDamageAmount)
            {
                return 1;
            }
            else if (xDamageAmount < yDamageAmount)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
