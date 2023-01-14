using UnityEngine;
using Models.Grid;

namespace Fight.Grid
{
    public class GridFactory : MonoBehaviour
    {
        public GameObject GridPrefab;
        public Transform SpawnPoint;
        private BaseGrid _grid;
        
        public BaseGrid CreateGrid()
	    {
            _grid = Instantiate(GridPrefab, SpawnPoint.position, Quaternion.identity, SpawnPoint).GetComponent<BaseGrid>();
            _grid.FindNeighbours();
            return _grid;
	    }
    }
}