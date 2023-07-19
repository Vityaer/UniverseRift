using Models.Common.BigDigits;
using System;
using System.Collections.Generic;

namespace Common.Observers
{
    public class ObserverDoneTask
    {
        private List<Observer> observers = new List<Observer>();

        public void Add(Action<BigDigit> del, int rating)
        {
            Observer work = GetObserver(rating);
            if (work != null)
            {
                work.Add(del);
            }
            else
            {
                observers.Add(new Observer(del, rating));
            }
        }

        public void Remove(Action<BigDigit> del, int rating)
        {
            Observer work = GetObserver(rating);
            if (work != null)
            {
                work.Remove(del);
                if (work.del == null)
                {
                    observers.Remove(work);
                }
            }
        }

        public void OnDoneTask(int rating)
        {
            Observer work = GetObserver(rating);
            if (work != null)
            {
                work.DoAction();
            }
        }

        private Observer GetObserver(int rating)
        {
            return observers.Find(x => x.rating == rating);
        }
    }
}