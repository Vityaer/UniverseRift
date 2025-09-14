using Db.CommonDictionaries;
using Models.City.Mines;
using UIController.Observers;
using UnityEngine;
using VContainer;

namespace UIController.GameObservers.Imps
{
    public class MineEnergyObserver : OtherObserver
    {
        private const string MINE_BUILDING_NAME = "MainMineBuilding";

        private CommonDictionaries m_commonDictionaries;
        private MineBuildingModel m_mineBuildingModel;

        [Inject]
        public void Construct(CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries;
            m_mineBuildingModel = m_commonDictionaries.Buildings[MINE_BUILDING_NAME] as MineBuildingModel;
        }

        protected override void RegisterOnChange()
        {
        }

        protected override void UpdateUI()
        {
            var administrationMine =
                CommonGameData.City.IndustrySave.Mines.Find(mine => mine.MineId == MINE_BUILDING_NAME);
            if (administrationMine == null)
            {
                Debug.LogError("administrationMine is null");
                return;
            }

            if (m_mineBuildingModel == null)
            {
                Debug.LogError("administrationMine is null");
                return;
            }

            if (m_mineBuildingModel.ConfigureContainers.Count == 0)
            {
                Debug.LogError("m_mineBuildingModel.MineEnergyDatas.Count == 0");
                return;
            }

            var index = 0;
            for (var i = 0; i < m_mineBuildingModel.ConfigureContainers.Count; i++)
                if (administrationMine.Level >= m_mineBuildingModel.ConfigureContainers[i].RequireLevel)
                    index = i;
                else
                    break;

            var maxEnergyCount = m_mineBuildingModel.ConfigureContainers[index].MaxEnergyCount;
            textObserver.text = $"{CommonGameData.City.IndustrySave.MineEnergy} / {maxEnergyCount}";
        }
    }
}