using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fight.Grid;

public class BaseAI : MonoBehaviour
{
	public List<Warrior> leftTeam = new List<Warrior>();
	public List<Warrior> rightTeam = new List<Warrior>();
	public List<HexagonCellScript> achievableMoveCells = new List<HexagonCellScript>();

	private Side sideForAI = Side.Right;
	private HeroControllerScript currentHero = null;

	public void StartAI()
	{
		FightControllerScript.Instance.RegisterOnStartFight(StartFight);
		FightControllerScript.Instance.RegisterOnFinishFight(FinishFight);
		HexagonCellScript.RegisterOnAchivableMove(AddAchivableMoveCell);
		HeroControllerScript.RegisterOnStartAction(OnHeroStartAction);
		HeroControllerScript.RegisterOnEndAction(ClearInfo);
		ClearInfo();
	}

	void StartFight()
	{
		rightTeam = FightControllerScript.Instance.GetRightTeam;
		leftTeam = FightControllerScript.Instance.GetLeftTeam;
	}

	void ClearInfo()
	{
		achievableMoveCells.Clear();
	}

	void FinishFight()
	{
		FightControllerScript.Instance.UnregisterOnStartFight(StartFight);
		FightControllerScript.Instance.UnregisterOnFinishFight(FinishFight);
		HeroControllerScript.UnregisterOnStartAction(OnHeroStartAction);
		HeroControllerScript.UnregisterOnEndAction(ClearInfo);
		HexagonCellScript.UnregisterOnAchivableMove(AddAchivableMoveCell);
	}


	void OnHeroStartAction(HeroControllerScript heroConroller)
	{
		if((heroConroller.side == sideForAI) || (sideForAI == Side.All))
		{
			currentHero = heroConroller;
			var workTeam = (heroConroller.side == Side.Right) ? leftTeam : rightTeam;
			var availableEnemies = workTeam.FindAll(x => x.Cell.GetCanAttackCell == true);
			Warrior enemy = null;
			if (availableEnemies.Count > 0)
				enemy = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count)];
			
			if(heroConroller.Mellee == true)
			{
				if(enemy != null)
				{
					heroConroller.SelectDirectionAttack(enemy.Cell.GetAchivableNeighbourCell(), enemy.heroController);
				}
				else
				{
					SelectCellForMove(achievableMoveCells, workTeam).AITurn();				
				}
			}
			else
			{
				if(enemy != null)
					heroConroller.StartDistanceAttackOtherHero(enemy.heroController);
			}
		}
	}

	public bool CheckMeOnSubmission(Side side)
	{
		return ((side == sideForAI) || (sideForAI == Side.All));
	}

	void AddAchivableMoveCell(HexagonCellScript newCell)
	{
		achievableMoveCells.Add(newCell);
	}

	HexagonCellScript SelectCellForMove(List<HexagonCellScript> achievableMoveCells, List<Warrior> enemies)
	{
		HexagonCellScript result = achievableMoveCells[UnityEngine.Random.Range(0, achievableMoveCells.Count)];
		int min = 1000;
		Stack<HexagonCellScript> way = new Stack<HexagonCellScript>(), minWay = new Stack<HexagonCellScript>(0);
		Warrior selectEnemy = null;
		for(int i = 0; i < enemies.Count; i++)
		{
			way = GridController.Instance.FindWay(currentHero.Cell, enemies[i].Cell);
			if(way.Count < min){
				min = way.Count;
				selectEnemy = enemies[i];
				minWay = way;
			}
		}
		HexagonCellScript workCell = null;
		for(int i = 0; i < minWay.Count; i++){
			workCell = minWay.Pop();

			if(achievableMoveCells.Contains(workCell)){
				result = workCell;
			}
		}
		return result;
	}

}