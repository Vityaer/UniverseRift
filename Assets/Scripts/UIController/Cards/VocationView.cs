using UnityEngine;
using UnityEngine.UI;

public class VocationView : MonoBehaviour
{
    [SerializeField] private Image imageVocation;
    public void SetData(Vocation newVocation) { imageVocation.sprite = SystemSprites.Instance.GetSprite(newVocation); }
}
