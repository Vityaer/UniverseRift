using System.Collections.Generic;
using UnityEngine;

public class TestingRoom : Building, IWorkWithWarTable
{
    [SerializeField] private Mission mission;
    [SerializeField] private List<WarriorPlace> leftTeam = new List<WarriorPlace>();
    [SerializeField] private List<WarriorPlace> rightTeam = new List<WarriorPlace>();

    protected override void OpenPage()
    {
        UnregisterOnOpenCloseWarTable();
    }

    public void ChangeTeamForFill(bool isLeft)
    {
    }

    public void OpenFight()
    {
        WarTableController.Instance.OpenMission(mission, Tavern.Instance.GetListHeroes);
        RegisterOnOpenCloseWarTable();
    }

    public void Change(bool isOpen)
    {
        if (!isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void RegisterOnOpenCloseWarTable()
    {
        WarTableController.Instance.RegisterOnOpenCloseMission(this.Change);
    }

    public void UnregisterOnOpenCloseWarTable()
    {
        WarTableController.Instance.UnregisterOnOpenCloseMission(this.Change);
    }
}
