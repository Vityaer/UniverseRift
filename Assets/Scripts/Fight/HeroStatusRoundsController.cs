using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroStatus : MonoBehaviour{
//Debuff
	private Debuff debuff = new Debuff();
	private void SaveDebuff(State state, int round){
		currentState = state;
		debuff.Update(state, round);
		FightEffectController.Instance.CastEffectStateOnHero(gameObject, debuff.state);
	}

//Dots	
	private List<Dot> dots = new List<Dot>();
	private void SaveDot(TypeDot typeDot, List<Round> rounds){
		dots.Add(new Dot(typeDot, rounds));
	}



//General loop
	public void RoundFinish(){
		debuff.RoundFinish();
		if(debuff.IsFinish && debuff.state != State.Clear){
			if(FightEffectController.Instance == null) Debug.Log("FightEffectControllerScript нету");
			if(debuff == null) Debug.Log("debuff null");
			if(gameObject == null) Debug.Log("gameObject null");
			FightEffectController.Instance.ClearEffectStateOnHero(gameObject, debuff.state);
			currentState = State.Clear;
		}

		for(int i = dots.Count - 1; i >= 0; i--){
			FightEffectController.Instance.CreateDot(transform, dots[i].type);
			dots[i].RoundFinish(heroController);
			if(dots[i].IsFinish){
				dots.RemoveAt(i);
			}
		}
		CheckBuffs();
	}
}

//Debuff
public class Debuff{
	public State state = State.Clear;
	public int countRound;
	private bool virgin = true;
	public void Update(State state, int rounds){
		this.state = state;
		this.countRound = rounds;
		virgin = true;
	}
	public void RoundFinish(){
		if(this.virgin) {this.virgin = false; }
		else            {this.countRound -= 1;}
	}
	public bool IsFinish{get => (countRound == 0);}
}

//Dots
	

	public class Dot{
		public TypeDot type;
		public List<Round> rounds;
		public void Update(List<Round> extraRounds){
			bool find = false;
			List<bool> flags = new List<bool>(this.rounds.Count){false};
			foreach (Round curRound in extraRounds){
				find = false;
				for(int i = 0; i < this.rounds.Count; i++){
					if((this.rounds[i].typeNumber == curRound.typeNumber) &&(flags[i] == false)){
						this.rounds[i].Add(curRound.amount);
						find = true;
						flags[i] = true;
						break;
					}
				}
				if(find == false) this.rounds.Add((Round) curRound.Clone());
			}
		}
		public void RoundFinish(HeroController heroController){
			if(rounds.Count > 0){
				heroController.GetDamage(new Strike(rounds[0].amount, 0, rounds[0].typeNumber, TypeStrike.Clean) );
				rounds.RemoveAt(0);
			}
		}
		public bool IsFinish{get => (rounds.Count == 0);}

		public Dot(TypeDot type, List<Round> newRounds){
			this.type   = type;
			this.rounds = new List<Round>();
			foreach (Round round in newRounds) {
				this.rounds.Add((Round) round.Clone());
			}
		}
	}

