using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils.Observer
{
    public class ObserverDoneTask{
        private List<Observer> observers = new List<Observer>();

        public class Observer
        {
            public Action<BigDigit> del;
            public int rating;
            public Observer(Action<BigDigit> d, int rating){
                del = d;
                this.rating = rating;
            }
            public void Add(Action<BigDigit> d){ del += d; }
            public void Remove(Action<BigDigit> d){ del -= d; }
            public void DoAction(){
                if(del != null) del(new BigDigit(1));
            }
        }

        public void Add(Action<BigDigit> del,int rating)
        {
            Observer work = GetObserver(rating);
            if(work != null){
                work.Add(del);
            }else{
                observers.Add(new Observer(del, rating));
            }
        }

        public void Remove(Action<BigDigit> del, int rating)
        {
            Observer work = GetObserver(rating);
            if(work != null)
            {
                work.Remove(del);
                if(work.del == null)
                {
                    observers.Remove(work);
                }	
            }
        }

        public void OnDoneTask(int rating)
        {
            Observer work = GetObserver(rating); 
            if(work != null){
                work.DoAction();
            }
        }

        private Observer GetObserver(int rating)
        {
            return observers.Find(x => (x.rating == rating));
        }
    }
}