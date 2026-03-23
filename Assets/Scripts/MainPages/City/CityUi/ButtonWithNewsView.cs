using UnityEngine;
using UnityEngine.UI;

namespace MainPages.City.CityUi
{
    public class ButtonWithNewsView : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private GameObject m_news;

        public void SetNewsEnabled(bool flag)
        {
            Debug.Assert(m_news != null);
            Debug.Log($"SetNewsEnabled: {flag}", gameObject);
            m_news.SetActive(flag);
        }
    }
}