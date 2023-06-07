using City.Buildings.Requirement;
using Common;

namespace UIController.ButtonsInCity
{
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
}