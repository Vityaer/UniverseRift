using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour{

	private Rigidbody2D rb;
	private Transform tr;
	void Awake(){
		tr = GetComponent<Transform>();
		rb = GetComponent<Rigidbody2D>();
	}
   	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent<HeroControllerScript>() == target){
			CollisionTarget();
		}	
	}

	private void CollisionTarget(){
		target.GetDamage(strike);
		if(delsCollision != null)
			delsCollision();
		OffArrow();		
	}
	public void OffArrow(){
		tr.position = new Vector3(1000f, 1000f, 0f);
		rb.velocity = new Vector2();
		delsCollision = null;
		Destroy(gameObject);
	}
	private HeroControllerScript target;
	private Strike strike;
	public float speed = 10f;

	public void SetTarget(HeroControllerScript target, Strike strike){
		this.target = target;
		this.strike = strike;
		Vector3 dir = target.GetPosition() - tr.position;
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
