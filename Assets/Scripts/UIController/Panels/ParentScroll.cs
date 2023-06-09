using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIController.Panels
{
    public class ParentScroll : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            ((IInitializePotentialDragHandler)_scrollRect).OnInitializePotentialDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ((IDragHandler)_scrollRect).OnDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ((IBeginDragHandler)_scrollRect).OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ((IEndDragHandler)_scrollRect).OnEndDrag(eventData);
        }
    }
}