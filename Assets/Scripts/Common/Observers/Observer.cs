using Models.Common.BigDigits;
using System;

namespace Common.Observers
{
    public class Observer
    {
        public Action<BigDigit> del;
        public int rating;

        public Observer(Action<BigDigit> d, int rating)
        {
            del = d;
            this.rating = rating;
        }
        public void Add(Action<BigDigit> d) { del += d; }
        public void Remove(Action<BigDigit> d) { del -= d; }
        public void DoAction()
        {
            if (del != null) del(new BigDigit(1));
        }
    }
}
