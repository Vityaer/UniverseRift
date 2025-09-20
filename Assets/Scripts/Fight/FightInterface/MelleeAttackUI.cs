using Fight.Common.Grid;
using UnityEngine;

namespace Fight.Common.FightInterface
{
    public class MelleeAttackUI : MonoBehaviour
    {
        public GameObject directionObject;
        public CellDirectionType direction;

        public void Open()
        {
            directionObject.SetActive(true);
        }

        public void Hide()
        {
            directionObject.SetActive(false);
        }
    }
}