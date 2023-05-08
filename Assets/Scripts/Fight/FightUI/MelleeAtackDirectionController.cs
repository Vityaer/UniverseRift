using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MelleeAtackDirectionController : MonoBehaviour{
    public GameObject panelDirectionAttack;
	public List<MelleeAttackUI> listDirections = new List<MelleeAttackUI>();
	private Action<NeighbourDirection> actionOnSelectDirection;
	public void AttackDirectionSelect(int numDirection){
		NeighbourDirection direction = (NeighbourDirection) numDirection;
		Debug.Log("Select direction success");
		Debug.Log(direction.ToString());
		Debug.Log(currentNeighbours.Find(x => x.direction == direction)?.Cell.gameObject.name);
		if(actionOnSelectDirection != null){
			actionOnSelectDirection((NeighbourDirection) numDirection);
			actionOnSelectDirection = null;
		}
		Close();	
	}
	List<NeighbourCell> currentNeighbours = new List<NeighbourCell>();
	private void Open(HexagonCell cell, List<NeighbourCell> neighbours){
		panelDirectionAttack.SetActive(true);
		currentNeighbours = neighbours;
		panelDirectionAttack.transform.position = cell.Position; 
		foreach(NeighbourCell neighbour in neighbours){
			if(neighbour.achievableMove && neighbour.Cell.available){
				listDirections.Find(x => x.direction == neighbour.direction).Open();
			}
		}
	}	
	private void Close(){
		foreach(MelleeAttackUI directionUI in listDirections)
			directionUI.Hide();
		panelDirectionAttack.SetActive(false);
	}
	public void RegisterOnSelectDirection(Action<NeighbourDirection> d,HexagonCell cell, List<NeighbourCell> neighbours){Debug.Log("open directions"); actionOnSelectDirection += d; Open(cell, neighbours); }
	public void UnregisterOnSelectDirection(Action<NeighbourDirection> d){ actionOnSelectDirection -= d; Close(); }
}
