using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Touch{
	public class ScreenSize : MonoBehaviour{
		public Corner cornerLeftTop, cornerBottomRight;
		private static Vector2 screenSize = Vector2.zero;
		void Awake(){
			CalculateSize();
		}
		private void CalculateSize(){
			screenSize = cornerBottomRight.Position - cornerLeftTop.Position;
			screenSize.x = Mathf.Abs(screenSize.x);
			screenSize.y = Mathf.Abs(screenSize.y);
		}
		public static float X{ get => screenSize.x;}
		public static float Y{ get => screenSize.y;}
		public static Vector2 Size{ get => screenSize;}

	}
}