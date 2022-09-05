using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleGame.AdvancedObservers{
	public class ObserverActionWithHero{
		private List<Observer> observers = new List<Observer>();
		public void Add(Action<BigDigit> del, int ID, int rating){
			Observer work = GetObserver(ID, rating);
			if(work != null){
				work.Add(del);
			}else{
				observers.Add(new Observer(del, ID, rating));
			}
		}
		public void Remove(Action<BigDigit> del, int ID, int rating){
			Observer work = GetObserver(ID, rating);
			if(work != null){
				work.Remove(del);
				if(work.del == null){
					observers.Remove(work);
				}	
			}
		}
		public void OnAction(int ID, int rating){
			Observer work = GetObserver(ID, rating); 
			if(work != null){
				work.DoAction();
			}
		}
		private Observer GetObserver(int ID, int rating){
			return observers.Find(x => (x.rating == rating) && (x.ID == ID));
		}

		public class Observer{
			public Action<BigDigit> del;
			public int rating;
			public int ID;
			public Observer(Action<BigDigit> d, int ID, int rating){
				del = d;
				this.ID = ID;
				this.rating = rating;
			}
			public void Add(Action<BigDigit> d){ del += d; }
			public void Remove(Action<BigDigit> d){ del -= d; }
			public void DoAction(){
				if(del != null) del(new BigDigit(1));
			}
		}
	}
}