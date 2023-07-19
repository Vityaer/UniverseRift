using Models.Common.BigDigits;
using System;
using System.Collections.Generic;

namespace IdleGame.AdvancedObservers
{
    public partial class ObserverActionWithHero
    {
        private List<GameObserver> observers = new List<GameObserver>();

        public void Add(Action<BigDigit> del, string ID, int rating)
        {
            GameObserver work = GetObserver(ID, rating);
            if (work != null)
            {
                work.Add(del);
            }
            else
            {
                observers.Add(new GameObserver(del, ID, rating));
            }
        }

        public void Remove(Action<BigDigit> del, string ID, int rating)
        {
            GameObserver work = GetObserver(ID, rating);
            if (work != null)
            {
                work.Remove(del);
                if (work.del == null)
                {
                    observers.Remove(work);
                }
            }
        }

        public void OnAction(string ID, int rating)
        {
            GameObserver work = GetObserver(ID, rating);
            work?.DoAction();
        }

        private GameObserver GetObserver(string ID, int rating)
        {
            return observers.Find(x => (x.rating == rating) && (x.ID == ID));
        }
    }
}