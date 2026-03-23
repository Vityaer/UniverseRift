using System.Collections.Generic;
using Fight.Common.HeroControllers.Generals;
using UnityEngine;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Fight.Common.Statistics
{
    public class HeroFightStatistics : MonoBehaviour
    {
        public List<HeroController> leftTeam = new List<HeroController>();
        public List<HeroController> rightTeam = new List<HeroController>();

    }
}