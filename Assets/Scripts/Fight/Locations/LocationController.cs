using Models.Fights.Misc;
using System.Collections.Generic;
using UIController;
using UnityEngine;

namespace Fight
{
    public class LocationController : MonoBehaviour
    {
        public List<LocationModel> Locations = new List<LocationModel>();

        private LocationModel _curLocation;

        public static LocationController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void OpenLocation(string typeLocation)
        {
            _curLocation = Locations.Find(x => x.Id == typeLocation);
            //BackgroundController.Instance.OpenBackground(_curLocation.BackgroundForFight);
        }

        // public void CloseLocation(){
        // 	curLocation.backgroundForFight.SetActive(false);
        // }

        //public Sprite GetBackgroundForMission(string typeLocation)
        //{
        //    //return Locations.Find(x => x.Id == typeLocation).BackgroundForMission;
        //}

        public void Close()
        {
            if (_curLocation != null)
                BackgroundController.Instance.OpenCityBackground();
        }

    }
}