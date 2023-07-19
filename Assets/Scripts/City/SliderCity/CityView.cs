using System.Collections.Generic;
using UnityEngine;
using VContainerUi.Abstraction;

namespace City.SliderCity
{
    public class CityView : UiView
    {
        [field: SerializeField] public List<Transform> ListCitySheet { get; private set; }
    }
}
