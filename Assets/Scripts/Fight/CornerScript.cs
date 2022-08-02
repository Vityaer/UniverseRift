using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CornerScript : MonoBehaviour{
	public enum Anchor{
		BottomRight,
		TopLeft
	}
    public Anchor anchor;
	void Awake(){
		CalculatePosition();
	}
    void CalculatePosition(){
		float camHalfHeight = Camera.main.orthographicSize;
	    float camHalfWidth = Camera.main.aspect * camHalfHeight;
	    Vector3 Position = new Vector3(0, 0, 0);
	    switch(anchor){
	    	case Anchor.TopLeft:
		    	Position = new Vector3(-camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position;
		    	break;
		    case Anchor.BottomRight:
		    	Position = new Vector3(camHalfWidth, -camHalfHeight, 0) + Camera.main.transform.position;
		    	break;
	    } 
	    transform.position = new Vector3(Position.x, Position.y, 0);
	}
}
