using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour{

	protected Rigidbody2D rb;
	protected Transform tr;
	void Awake(){
		tr = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>();
	}
	bool isDone = false;
   	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent<HeroControllerScript>() == target){
			if(isDone == false){
				isDone = true;
				CollisionTarget();
			}
		}	
	}

	protected void CollisionTarget(){
		if(strike != null)
			target.GetDamage(strike);
		if(delsCollision != null)
			delsCollision();
		OffArrow();		
	}
	public void OffArrow(){
		Destroy(gameObject);
	}
	protected HeroControllerScript target;
	protected Strike strike = null;
	public float speed = 10f;

	public void SetTarget(HeroControllerScript target, Strike strike){
		this.target = target;
		this.strike = strike;
		Vector3 dir = target.GetPosition - tr.position;
		dir.Normalize();
		rb.velocity = dir * speed;
	}

	public delegate void Del();
	private Del delsCollision;
	public void RegisterOnCollision(Del d){
		delsCollision += d;
	}
	public void UnRegisterOnCollision(Del d){
		delsCollision -= d;
	}
}
