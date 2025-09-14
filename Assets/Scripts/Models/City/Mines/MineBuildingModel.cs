using System;
using Models.City.AbstactBuildingModels;
using Models.City.Mines.Energies;
using System.Collections.Generic;

namespace Models.City.Mines
{
    public class MineBuildingModel : BuildingModel
    {
        public int SecondRefresh;
        public List<MineConfigureContainer> ConfigureContainers = new();
        
        public MineConfigureContainer GetContainer(int level)
        {
            int index = -1;
            for (int i = 0; i < this.ConfigureContainers.Count;i++)
            {
                if (level >= ConfigureContainers[i].RequireLevel)
                {
                    index = i;
                }
                else
                {
                    break;
                }
            }

            index = Math.Clamp(index, 0, ConfigureContainers.Count);
            return this.ConfigureContainers[index];
        }
    }
}
