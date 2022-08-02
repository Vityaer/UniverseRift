using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MyScrollRect : MonoBehaviour{
	public ScrollRect scrollRect;
	public void OnInitializePotentialDrag(PointerEventData eventData){
        ((IInitializePotentialDragHandler)scrollRect).OnInitializePotentialDrag(eventData);
    }
    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData){
        ((IDragHandler)scrollRect).OnDrag(eventData);
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData){
        ((IBeginDragHandler)scrollRect).OnBeginDrag(eventData);
    }
    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData){
        ((IEndDragHandler)scrollRect).OnEndDrag(eventData);
    }
}
