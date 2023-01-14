using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif
[System.Serializable]
public class NeighbourCell{
	public NeighbourDirection direction;
	[SerializeField]private HexagonCellScript cell;

	public  HexagonCellScript Cell{get => cell;}
	public bool achievableMove{get => cell.achievableMove;}
	public bool available{get => cell.available;}
	public HeroControllerScript GetHero{get => cell.Hero;}
	public void CheckMove(int step){ cell.CheckMove(step); }
	public NeighbourCell(HexagonCellScript mainCell, HexagonCellScript neighbourCell){
		cell = neighbourCell;
		direction = GetDirection(mainCell, neighbourCell);
		#if UNITY_EDITOR_WIN
			Undo.RecordObject(cell, "fill neighbourCell");
		#endif
	}
	public void FindWay(HexagonCellScript previousCell, HexagonCellScript target, TypeMovement typeMovement = TypeMovement.Ground, int step = 1){cell.FindWay(previousCell, target, typeMovement, step);}
	public static NeighbourDirection GetDirection(HexagonCellScript mainCell, HexagonCellScript neighbourCell){
		NeighbourDirection direction = NeighbourDirection.UpLeft;
		float deltaX = neighbourCell.Position.x - mainCell.Position.x;
		float deltaY = neighbourCell.Position.y - mainCell.Position.y;
		if(deltaX > 0){
			if(deltaY > 0.1f){
				direction = NeighbourDirection.UpRight; 
			}else if(deltaY < -0.1f){
				direction = NeighbourDirection.BottomRight; 
			}else{
				direction = NeighbourDirection.Right; 
			}
		}else{
			if(deltaY > 0.1f){
				direction = NeighbourDirection.UpLeft; 
			}else if(deltaY < -0.1f){
				direction = NeighbourDirection.BottomLeft; 
			}else{
				direction = NeighbourDirection.Left; 
			}
		}
		return direction;
	}
	private static List<NeighbourDirection> sampleDirections = new List<NeighbourDirection>(){NeighbourDirection.UpRight, NeighbourDirection.Right, NeighbourDirection.BottomRight, NeighbourDirection.BottomLeft, NeighbourDirection.Left, NeighbourDirection.UpLeft};
	private static int currentNumDirection = 0, leftNumDirection = 0, rightNumDirection = 0;
	private static NeighbourCell currentNeighbourCell = null;
	public static void GetNeighboursOnLevelNear(List<NeighbourCell> neighbours, NeighbourDirection direction, int levelNear, List<NeighbourCell> result){
		result.Clear();
		currentNumDirection = sampleDirections.IndexOf(direction);
		leftNumDirection = currentNumDirection - levelNear;
		if(leftNumDirection < 0) leftNumDirection = 6 + leftNumDirection;
		rightNumDirection = currentNumDirection + levelNear;
		if(rightNumDirection >= 6) rightNumDirection = rightNumDirection - 6; 
		currentNeighbourCell = neighbours.Find(x => x.direction == sampleDirections[leftNumDirection]);
		if(currentNeighbourCell != null) result.Add(currentNeighbourCell);
		if(rightNumDirection != leftNumDirection){
			currentNeighbourCell = neighbours.Find(x => x.direction == sampleDirections[rightNumDirection]);
			if(currentNeighbourCell != null) result.Add(currentNeighbourCell);
		}
	}
}
public enum NeighbourDirection{
	UpLeft = 0,
	UpRight = 1,
	Left = 2,
	Right = 3,
	BottomLeft = 4,
	BottomRight = 5
}