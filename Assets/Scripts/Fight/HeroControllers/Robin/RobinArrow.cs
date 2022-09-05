using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinArrow : ArrowScript{
   public void SetTarget(HeroControllerScript target){
		Debug.Log("set target");
		this.target = target;
		Vector3 dir = target.GetPosition - tr.position;
		dir.Normalize();
		rb.velocity = dir * speed;
	}
}
