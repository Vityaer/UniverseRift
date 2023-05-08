using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitiaController : HeroController{
	protected override void DoSpell(){
		statusState.ChangeStamina(-100);
		anim.Play("Spell");
	}

}
