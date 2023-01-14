using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{
	bool isFacingRight = true; 
	private Transform body;
	
	protected void PrepareOnStartTurn()
	{
		if(this.side == Side.Left){
			FightUI.Instance.OpenControllers(this);
		}else{
			FightUI.Instance.CloseControllers();
		}
		listTarget.Clear();
		FindAvailableCells();
		WaitingSelectTarget();
		OnStartAction();
		outlineController.SwitchOn();
	}
	
	private bool NeedFlip(HeroControllerScript enemy)
	{
		bool result = false;
		NeighbourDirection direction = NeighbourCell.GetDirection(myPlace, enemy.Cell);
		switch(direction){
			case NeighbourDirection.UpLeft:
			case NeighbourDirection.Left:
			case NeighbourDirection.BottomLeft:
				result = (side != Side.Right);
				break;
			case NeighbourDirection.UpRight:
			case NeighbourDirection.Right:
			case NeighbourDirection.BottomRight:
				result = (side != Side.Left);
				break;	
		} 
		return result;
	}

	protected void FlipX()
	{
		GetSpriteRenderer.flipX = true;
		isFacingRight = !isFacingRight;
		Vector3 locScale = body.localScale;
		locScale.x *= -1;
		body.localScale = locScale;
	} 
//Fight helps
	protected bool CanAttackHero(HeroControllerScript otherHero) => (this.side != otherHero.side);

	protected void RefreshOnEndRound(){
		currentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
	}

 	protected void ShakeCamera()
	{
		CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY);
	}

	protected void IsSide(Side side){
		this.side = side;
		delta = (side == Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
		if(side == Side.Right) FlipX();
	}
	protected bool CanCounterAttack(HeroControllerScript heroForCounterAttack, HeroControllerScript heroWasAttack){
		bool result = false;
		if((currentCountCounterAttack > 0) &&(statusState.PermissionAction() == true) && !isDeath){
			result = true;
		}
		return result;
	}

	private void CreateSpell(){
		listTarget.Clear();
		OnSpell(listTarget);
		EndTurn();
	}
	public bool SpellExist => CheckExistAnimation(ANIMATION_SPELL);
	public Sprite GetSprite => GetSpriteRenderer.sprite;
	private SpriteRenderer _spriteRenderer;	
	public SpriteRenderer GetSpriteRenderer
	{
		get
		{
			if(_spriteRenderer == null)
				_spriteRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
			return _spriteRenderer;
		}
	}

	public OutlineController outlineController;

	[ContextMenu("Add 100 stamina")]
	private void AddBonus100Stamina(){
		statusState.ChangeStamina(100);
	}
}