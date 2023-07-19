using Models.Common.BigDigits;
using System;

namespace IdleGame.AdvancedObservers
{
    public partial class ObserverActionWithHero
    {
        public class GameObserver
        {
            public Action<BigDigit> del;
            public int rating;
            public string ID;

            public GameObserver(Action<BigDigit> d, string ID, int rating)
            {
                del = d;
                this.ID = ID;
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
}