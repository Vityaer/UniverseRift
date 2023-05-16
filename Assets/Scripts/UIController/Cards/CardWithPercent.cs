using TMPro;
using UnityEngine;

public class CardWithPercent : MonoBehaviour
{
    public Card cardInfo;
    public TextMeshProUGUI textPercent;

    public void SetData(string ID, float percent)
    {
        cardInfo.ChangeInfo(Tavern.Instance.GetInfoHero(ID));
        textPercent.text = $"{percent}%";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
