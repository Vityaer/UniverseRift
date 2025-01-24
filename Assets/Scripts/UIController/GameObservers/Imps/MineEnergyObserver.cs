using UIController.Observers;
using VContainer;

namespace UIController.GameObservers.Imps
{
    public class MineEnergyObserver : OtherObserver
    {
        protected override void RegisterOnChange()
        {
        }

        protected override void UpdateUI()
        {
            textObserver.text = $"{CommonGameData.City.IndustrySave.MineEnergy} / ";
        }
    }
}
