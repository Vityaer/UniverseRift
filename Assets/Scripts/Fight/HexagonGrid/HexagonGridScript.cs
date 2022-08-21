using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HexagonGridScript : MonoBehaviour{

	public GameObject grid;
	[SerializeField] private List<HexagonCellScript> cells = new List<HexagonCellScript>(); 
	[SerializeField] private List<HexagonCellScript> startCellsLeftTeam, startCellsRightTeam;
	public List<HexagonCellScript> GetLeftTeamPos{get => startCellsLeftTeam;}
	public List<HexagonCellScript> GetRightTeamPos{get => startCellsRightTeam;}
	private void FindAllCell(){
		cells.Clear();
		foreach(Transform child in grid.transform){
			if(child.GetComponent<HexagonCellScript>() != null){
				cells.Add(child.GetComponent<HexagonCellScript>());
			}
		}
	}
	[ContextMenu("FindNeighbours")]
	public void FindNeighbours(){
		FindAllCell();
		for(int i = 0; i < cells.Count; i++) cells[i].ClearNeighbours();
		for(int i = 0; i < cells.Count - 1; i++){
			for(int j = i + 1; j < cells.Count; j++){
				cells[i].CheckOnNeighbour(cells[j]);
				cells[j].CheckOnNeighbour(cells[i]);
			}
		}
	}
	private static HexagonGridScript instance;
	public static HexagonGridScript Instance {get => instance;}
	void Awake(){
		instance = this;
	}
	public void OpenGrid(){
		grid.SetActive(true);
		FightControllerScript.Instance.RegisterOnStartFight(StartFight);
		FightControllerScript.Instance.RegisterOnFinishFight(FinishFight);
	}
	public void CloseGrid(){
		grid.SetActive(false);
		FightControllerScript.Instance.UnregisterOnStartFight(StartFight);
		FightControllerScript.Instance.UnregisterOnFinishFight(FinishFight);
	}
	void Start(){
		FindNeighbours();
	}
	bool fighting = false;
	Coroutine coroutineCheckClick = null;
	void StartFight(){
		fighting = true;
	}
    
    RaycastHit2D hit;
 	void Update(){
		if(fighting){
			if (Input.GetMouseButtonDown(0)){
		        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
		        if(hit != null){
		        	if(hit.transform != null){
		                if (hit.transform.CompareTag("HexagonCell")){
		                    HexagonCellScript HexagonCell = hit.collider.transform.GetComponent<HexagonCellScript>();
		                    HexagonCell.ClickOnMe();
		                }
		                if(hit.transform.CompareTag("Hero")){
		                	 hit.collider.transform.GetComponent<HeroControllerScript>().ClickOnMe();
			        	}
	                }
	            }
	        }
		}
 	}
	void FinishFight(){
		fighting = false;

	}
//Find Way	
	Stack<HexagonCellScript> way = new Stack<HexagonCellScript>();
	HexagonCellScript PreviousCell = null;
	public Stack<HexagonCellScript> FindWay(HexagonCellScript startCell, HexagonCellScript finishCell, TypeMovement typeMovement = TypeMovement.Ground){
		Debug.Log("start: " + startCell.gameObject.name + " to finish: " + finishCell.gameObject.name);
		way.Clear();
		PreviousCell = null;
		startCell.FindWay(previousCell: null, target: finishCell, typeMovement : typeMovement);
		way.Push(finishCell);
		do{
			if(finishCell != null){
				PreviousCell = finishCell.PreviousCell;
				if(PreviousCell != null) way.Push(PreviousCell);
			}
			finishCell = PreviousCell;
		} while((finishCell != startCell) && (finishCell != null));
		OnFoundWay();
		// Debug.Log("way length: " + way.Count.ToString());
		// foreach (HexagonCellScript cell in way){
		// 	Debug.Log(cell.gameObject.name);
		// }
		return way;
	}
	public static bool PlayerCanController = false;
	Action observerFoundWay;
	public void RegisterOnFoundWay(Action d){observerFoundWay += d;}
	public void UnregisterOnFoundWay(Action d){observerFoundWay -= d;}
	void OnFoundWay(){if(observerFoundWay != null) observerFoundWay();} 
}