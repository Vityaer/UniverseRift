using UnityEngine;

public class TravelSelectScript : MonoBehaviour
{
    [SerializeField] private GameObject _selectBorder;
    public string Race;
    public RaceView RaceUI;
    private static TravelSelectScript selectedRace = null;
    void Start()
    {
        RaceUI.SetData(Race);
    }
    public void Open()
    {
        TravelCircleScript.Instance.ChangeTravel(Race);
    }
    public void Select()
    {
        if (selectedRace != null) selectedRace.Diselect();
        selectedRace = this;
        this._selectBorder.SetActive(true);
    }
    public void Diselect()
    {
        _selectBorder.SetActive(false);
    }
}