using Assets.Scripts.GeneralObject;
using System;
using System.Collections.Generic;

namespace IdleGame.AdvancedObservers
{
    public class ObserverActionWithHero
    {
        private List<Observer> observers = new List<Observer>();

        public void Add(Action<BigDigit> del, string ID, int rating)
        {
            Observer work = GetObserver(ID, rating);
            if (work != null)
            {
                work.Add(del);
            }
            else
            {
                observers.Add(new Observer(del, ID, rating));
            }
        }

        public void Remove(Action<BigDigit> del, string ID, int rating)
        {
            Observer work = GetObserver(ID, rating);
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
            Observer work = GetObserver(ID, rating);
            work?.DoAction();
        }

        private Observer GetObserver(string ID, int rating)
        {
            return observers.Find(x => (x.rating == rating) && (x.ID == ID));
        }

        public class Observer
        {
            public Action<BigDigit> del;
            public int rating;
            public string ID;

            public Observer(Action<BigDigit> d, string ID, int rating)
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