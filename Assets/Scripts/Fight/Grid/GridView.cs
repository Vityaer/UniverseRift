using Cinemachine;
using Models.Grid;
using UnityEngine;
using VContainerUi.Abstraction;

namespace Fight.Common.Grid
{
    public class GridView : UiView
    {
        public BaseGrid Prefab;
        public Transform ParentTemplateObjects;
        public Transform GridParent;
        public CinemachineVirtualCamera CinemachineVirtual;
    }
}
