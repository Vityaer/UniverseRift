using UnityEngine;
using Models.Grid;
using Common.Factories;
using VContainer;

namespace Fight.Common.Grid
{
    public class GridFactory : BaseFactory<BaseGrid>
    {
        public GridFactory(IObjectResolver objectResolver) : base(objectResolver)
        {
        }

        public BaseGrid CreateGrid(BaseGrid Prefab, Transform SpawnPoint)
	    {
            var grid = Object.Instantiate(Prefab, SpawnPoint.position, Prefab.transform.rotation, SpawnPoint);
            return grid;
	    }
    }
}