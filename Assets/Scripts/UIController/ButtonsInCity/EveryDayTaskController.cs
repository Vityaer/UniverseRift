public class EveryDayTaskController : RequirementMenu
{
    protected override void OnLoadGame()
    {
        LoadData(GameController.GetPlayerGame.saveEveryTimeTasks);
    }

    protected override void SaveData()
    {
        GameController.GetPlayerGame.SaveEveryTimeTask(listRequirement);
        SaveGame();
    }
}
