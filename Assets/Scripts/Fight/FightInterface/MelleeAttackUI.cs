using Fight.Grid;
using UnityEngine;

namespace Fight.FightInterface
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