using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour
{
	[SerializeField] bool isFacingRight = true; 

	private Transform BodyParent;
	public OutlineController outlineController;

	private SpriteRenderer _spriteRenderer;	
	public Sprite GetSprite => GetSpriteRenderer.sprite;
	public bool SpellExist => CheckExistAnimation(ANIMATION_SPELL);
	public SpriteRenderer GetSpriteRenderer
	{
		get
		{
			if(_spriteRenderer == null)
				_spriteRenderer = transform.Find("BodyParent/Body").GetComponent<SpriteRenderer>();
			return _spriteRenderer;
		}
	}
	
	protected virtual void PrepareOnStartTurn()
	{
		needFlip = false;
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
				result = (isFacingRight == true);
				break;
			case NeighbourDirection.UpRight:
			case NeighbourDirection.Right:
			case NeighbourDirection.BottomRight:
				result = (isFacingRight == false);
				break;	
		}

		return result;
	}

	protected void FlipX()
	{
		isFacingRight = !isFacingRight;
		Vector3 locScale = BodyParent.localScale;
		locScale.x *= -1;
		BodyParent.localScale = locScale;
	} 

//Fight helps
	protected virtual bool CanAttackHero(HeroControllerScript otherHero) => (this.side != otherHero.side);

	protected void RefreshOnEndRound()
	{
		currentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
	}

 	protected void ShakeCamera()
	{
		CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY);
	}

	protected void IsSide(Side side)
	{
		this.side = side;
		delta = (side == Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
		if(side == Side.Right) FlipX();
	}

	protected bool CanCounterAttack(HeroControllerScript heroForCounterAttack, HeroControllerScript heroWasAttack)
	{
		bool result = false;
		if((currentCountCounterAttack > 0) &&(statusState.PermissionAction() == true) && !isDeath)
		{
			result = true;
		}
		return result;
	}

	private void CreateSpell()
	{
		listTarget.Clear();
		OnSpell(listTarget);
		EndTurn();
	}

	private void ShowHeroesPlaceInteractive()
 	{
 		foreach(var warrior in FightControllerScript.Instance.GetLeftTeam)
 		{
 			Color color;
 			if(warrior.Cell.GetAchivableNeighbourCell() == null || !CanShoot())
 			{
 				color = Costants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR;
 			}
 			else
 			{
				color = Costants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR;
 			}
 			warrior.Cell.SetColor(color);
 		}

 		foreach(var warrior in FightControllerScript.Instance.GetRightTeam)
 		{
 			Color color = Color.red;
 			if(warrior.Cell.GetAchivableNeighbourCell() == null || !CanShoot())
 			{
 				color = Costants.Colors.ACHIEVABLE_ENEMY_CELL_COLOR;
 			}
 			else
 			{
 				color = Costants.Colors.NOT_ACHIEVABLE_ENEMY_CELL_COLOR;
 			}
 			warrior.Cell.SetColor(color);
 		}
 	}

 	private void SetMyPlaceColor()
 	{
 		if(side == Side.Left)
 		{
 			myPlace.SetColor(Costants.Colors.NOT_ACHIEVABLE_FRIEND_CELL_COLOR);
 		}
 		else
 		{
 			myPlace.SetColor(Costants.Colors.NOT_ACHIEVABLE_ENEMY_CELL_COLOR);
 		}
 	}

 	private bool CanShoot()
 	{
 		return (hero.characts.baseCharacteristic.Mellee == true) || (!hero.characts.baseCharacteristic.Mellee && myPlace.MyEnemyNear(this.side) );
 	}

	[ContextMenu("Add 100 stamina")]
	private void AddBonus100Stamina()
	{
		statusState.ChangeStamina(100);
	}
}