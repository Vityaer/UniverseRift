using System.Collections.Generic;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    public List<Location> locations = new List<Location>();
    private Location curLocation;
    void Awake()
    {
        instance = this;
    }

    public void OpenLocation(TypeLocation typeLocation)
    {
        curLocation = locations.Find(x => x.type == typeLocation);
        BackgroundController.Instance.OpenBackground(curLocation.backgroundForFight);
    }

    // public void CloseLocation(){
    // 	curLocation.backgroundForFight.SetActive(false);
    // }

    public Sprite GetBackgroundForMission(TypeLocation typeLocation)
    {
        return locations.Find(x => x.type == typeLocation).backgroundForMission;
    }

    public void Close()
    {
        if (curLocation != null) BackgroundController.Instance.OpenCityBackground();
    }

    private static LocationController instance;
    public static LocationController Instance { get => instance; }
}

public enum TypeLocation
{
    Forest,
    NightForest,
    Desert
}
[System.Serializable]
public class Location
{
    public TypeLocation type;
    public GameObject backgroundForFight;
    public Sprite backgroundForMission;
}