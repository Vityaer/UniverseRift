using City.Buildings.General;
using System.Collections.Generic;
using UIController.Campaign;

namespace City.Buildings
{
    public class WorldCampaign : Building
    {
        public List<CampaignChapterControllerScript> chapters = new List<CampaignChapterControllerScript>();

        protected override void OpenPage()
        {

        }
    }
}