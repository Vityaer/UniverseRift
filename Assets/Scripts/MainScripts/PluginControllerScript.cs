using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidPlugin{
	public class PluginControllerScript : MonoBehaviour{
		public class ToastPlugin{
			private const string className = "com.example.toastplugin.ToastPlugin";
			
			#if UNITY_ANDROID && !UNITY_EDITOR	
				private static AndroidJavaClass javaClass = new AndroidJavaClass(className);
			#endif

			public static void Show(string text, bool isLong){
			#if UNITY_ANDROID && !UNITY_EDITOR	
				if(javaClass != null){
					javaClass.CallStatic("Show", text, isLong);
				}
			#endif	
			}
		}
	}
}
