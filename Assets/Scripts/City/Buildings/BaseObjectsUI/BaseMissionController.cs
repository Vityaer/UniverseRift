using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class BaseMissionController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
	protected virtual MyScrollRect GetScrollParent(){Debug.Log("not override GetScrollParent"); return null;}
    public void OnBeginDrag(PointerEventData eventData){
    	GetScrollParent()?.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData){
    	GetScrollParent()?.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData){
    	GetScrollParent()?.OnEndDrag(eventData);
    }
}