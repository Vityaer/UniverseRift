using DG.Tweening;
using UnityEngine;

namespace Fight
{
    public class DotObject : MonoBehaviour
    {
        void Start()
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().DOFade(0f, duration: 1f);
            Destroy(gameObject, 1.25f);
        }
    }
}