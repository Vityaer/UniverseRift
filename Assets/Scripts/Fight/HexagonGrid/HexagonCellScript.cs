using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif

public class HexagonCellScript : MonoBehaviour{
	private Transform tr;
	public Vector3 Position{get => transform.position;} 
	[SerializeField] private List<NeighbourCell> neighbours = new List<NeighbourCell>();
	public SpriteRenderer spriteCell, spriteAvailable;
	private GameObject subject;
	[SerializeField] HeroControllerScript heroScript;
	public bool available = true;
	public bool availableMove = true;

	public List<NeighbourCell> GetAvailableNeighbours{get => neighbours.FindAll(x => x.available == true);}
	public bool CanStand{get => (availableMove && (heroScript == null));}
	public HeroControllerScript Hero{get => heroScript;}
	void Awake(){
		tr = base.transform;
	}
	public bool achievableMove = false; 
	public int step = 0;
	private bool showAchievable = false;

	public void StartCheckMove(int step, bool playerCanController){
		HexagonGridScript.PlayerCanController = playerCanController;
		this.step = step;
		achievableMove = true;
		HeroControllerScript.RegisterOnEndSelectCell(ClearCanMove);
		for(int i = 0; i < neighbours.Count; i++) neighbours[i].CheckMove(step - 1);
	}

	public void CheckMove(int step){
		if(available && availableMove){
			if((achievableMove == false) || (this.step < step)){
				this.step = step;
				if(achievableMove == false){
					HeroControllerScript.RegisterOnEndSelectCell(ClearCanMove);
					OnAchivableMove();
				}
				achievableMove = true;
				
				if((showAchievable == false)/* && HexagonGridScript.PlayerCanController*/){
					showAchievable = true;
					spriteAvailable.enabled = true;
				}
				if(step > 0) for(int i = 0; i < neighbours.Count; i++) neighbours[i].CheckMove(step - 1);
			}
		}
	}
	public HexagonCellScript GetAchivableNeighbourCell(){
		Debug.Log("achievableMove count: " + neighbours.FindAll(x => x.achievableMove == true).Count);
		foreach(NeighbourCell neighbour in neighbours.FindAll(x => x.achievableMove == true)){
			Debug.Log(neighbour.Cell.gameObject.name + " with achievableMove = " + neighbour.Cell.achievableMove.ToString(), neighbour.Cell.gameObject);
		}
		return neighbours.Find(x => x.achievableMove).Cell;
	}
	public bool GetCanAttackCell{ get => (neighbours.Find(x => (x.achievableMove == true)) != null); }
	public void RegisterOnSelectDirection(Action<HexagonCellScript> selectDirectionForHero, bool showUIDirection = true){
		Debug.Log("select sell name: "+ gameObject.name);
		observerSelectDirection = selectDirectionForHero;
		if(showUIDirection)
			ShowDirectionsAttack();
	}
	private void ShowDirectionsAttack(){
		FightUI.Instance.melleeAttackController.RegisterOnSelectDirection(SelectDirection, this, GetAvailableNeighbours);
	}

	private void SelectDirection(NeighbourDirection direction){
		Debug.Log("in Cell: "+gameObject.name+ ", get cell on direction: " + direction.ToString());
		if(observerSelectDirection != null){
			observerSelectDirection(GetNeighbourCellOnDirection(direction));
			observerSelectDirection = null;
		}
	}
	private HexagonCellScript GetNeighbourCellOnDirection(NeighbourDirection direction){
		HexagonCellScript result = neighbours.Find(x => x.direction == direction).Cell;
		Debug.Log("GetNeighbourCellOnDirection: " + result.name);
		return result;
	}
	private void OnSelectDirection(){if((observerClick != null) && (available == true)) observerClick(this);}

	public void ClearCanMove(){
		showAchievable = false;
		this.step = 0;
		achievableMove = false;
		spriteAvailable.enabled = false;
		dist = 100;
		HeroControllerScript.UnregisterOnEndSelectCell(ClearCanMove);
	}
	public void SetSubject(GameObject newSubject){
		this.subject = newSubject;
	}
	public void SetHero(HeroControllerScript hero){
		heroScript = hero;
		subject = hero.gameObject;
		availableMove = false;
	}
	public void ClearSublject(){
		this.subject = null;
		heroScript = null;
		availableMove = true;
	}

	public void ClickOnMe(){
		if(HexagonGridScript.PlayerCanController){
			if(achievableMove || (heroScript != null)){
				OnClickCell();
			}
		}
	}
	public void AITurn(){ OnClickCell(); }
	private static Action<HexagonCellScript> observerClick, observerSelectDirection, observerAchivableMove;
	public static void RegisterOnClick(Action<HexagonCellScript> d){ observerClick += d;}
	public static void UnregisterOnClick(Action<HexagonCellScript> d){ observerClick -= d;}
	private void OnClickCell(){if((observerClick != null) && (available == true)) observerClick(this);}
	

	public static void RegisterOnAchivableMove(Action<HexagonCellScript> d){observerAchivableMove += d; }
	public static void UnregisterOnAchivableMove(Action<HexagonCellScript> d){observerAchivableMove += d; }
	private void OnAchivableMove(){if(observerAchivableMove != null) observerAchivableMove(this);}

	public void CheckOnNeighbour(HexagonCellScript otherCell){
		#if UNITY_EDITOR_WIN
		Undo.RecordObject(this, "fill cell");
		#endif
		if(Vector3.Distance(this.Position, otherCell.Position) < (1.1f * transform.localScale.x)){
			NeighbourCell newNeighbour = new NeighbourCell(this, otherCell);
			neighbours.Add(newNeighbour);
		}
	}
	public void ClearNeighbours(){ neighbours.Clear(); }
//Find way
	private int dist = 100;
	public int GetDist{get => dist;}
	private HexagonCellScript previousCell = null;
	public HexagonCellScript PreviousCell{get => previousCell;}
	bool checkNext = false;
	List<NeighbourCell> asumptionNeighbourCell = new List<NeighbourCell>();
	NeighbourDirection directionToTarget;
	public void FindWay(HexagonCellScript previousCell, HexagonCellScript target, TypeMovement typeMovement = TypeMovement.Ground, int step = 1){
		if(available && (availableMove || (previousCell == null) ||(this == target))){
			if(step < dist){
				checkNext = true;
				dist = step;
			}else if(step == dist){
				checkNext = (UnityEngine.Random.Range(0f, 1f) > 0.5f);
			}else{checkNext = false;}
			if(checkNext){
				if(this.previousCell == null) HexagonGridScript.Instance.RegisterOnFoundWay(ClearFindWay);
				this.previousCell = previousCell;
				if((target.GetDist > step) && (this != target)){
					directionToTarget = NeighbourCell.GetDirection(this, target);
					for(int level = 0; level <= 3; level++){
						NeighbourCell.GetNeighboursOnLevelNear(neighbours, directionToTarget, level, asumptionNeighbourCell);
						if(asumptionNeighbourCell.Count > 0) CheckNeighbour(previousCell, target, typeMovement, step);

					}
				}
			}
		}
	}
	private void CheckNeighbour(HexagonCellScript previousCell, HexagonCellScript target, TypeMovement typeMovement, int step){
		for(int i = 0; i < asumptionNeighbourCell.Count; i++){
			if(asumptionNeighbourCell[i].Cell != previousCell) asumptionNeighbourCell[i].FindWay(this, target, typeMovement, step: (step + 1));

		}
	}
	public void ClearFindWay(){
		HexagonGridScript.Instance.UnregisterOnFoundWay(ClearFindWay);
		dist = 100;
		previousCell = null;
		checkNext = false;
	}

	
#if UNITY_EDITOR_WIN
//Potision controller	
	private static Vector2 deltaSize = new Vector2(0.826f, 0.715f);
	[ContextMenu("UpLeft")]
	private void MoveUpLeft(){ Move( new Vector3(-deltaSize.x * transform.localScale.x/2, deltaSize.y * transform.localScale.y,0) ); }
	
	[ContextMenu("UpRight")]
	private void MoveUpRight(){ Move( new Vector3(deltaSize.x * transform.localScale.x/2, deltaSize.y * transform.localScale.y,0) ); }

	[ContextMenu("Left")]
	private void MoveLeft(){ Move( new Vector3(- deltaSize.x * transform.localScale.x, 0,0) ); }
	
	[ContextMenu("Right")]
	private void MoveRight(){ Move( new Vector3(deltaSize.x * transform.localScale.x, 0,0) ); }
	
	[ContextMenu("DownLeft")]
	private void MoveDownLeft(){ Move( new Vector3(-deltaSize.x * transform.localScale.x/2, -deltaSize.y * transform.localScale.y,0) ); }

	[ContextMenu("DownRight")]
	private void MoveDownRight(){ Move( new Vector3(deltaSize.x * transform.localScale.x/2, -deltaSize.y * transform.localScale.y,0) ); }


	private void Move(Vector3 dir){
		Undo.RecordObject(transform, "hexagon position");
		tr.position = tr.position + dir;
	}
#endif
}
