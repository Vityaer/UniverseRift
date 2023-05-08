using UnityEngine;
using UnityEngine.UI;

public class RaceView : MonoBehaviour
{
    [SerializeField] private Image imageRace;
    
    public void SetData(Race newRace)
    {
        imageRace.sprite = SystemSprites.Instance.GetSprite(newRace);
        currentRace = newRace;
    }

    private Race currentRace;
}