using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR_WIN
// [CustomEditor(typeof(MarketScript), true)]
// public class MarketScriptEditor : Editor{
// 	 MarketScript marketScript;
//     public override void OnInspectorGUI(){
// 		if(marketScript == null) marketScript = (MarketScript)target;
// 		EditorGUILayout.LabelField("Market");
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("showcase"));
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("resources"));
//         // EditorGUILayout.PropertyField(serializedObject.FindProperty("productsForSale"));
//         // ShowProductsList(serializedObject.FindProperty("productsForSale"));

//     	GUILayout.BeginHorizontal("box");
//        		if (GUILayout.Button("Add Resource")){
// 	            marketScript.AddResource();
//         	}
//         	if (GUILayout.Button("Add Splinter")){
// 	            marketScript.AddSplinter();
//         	}
//         	if (GUILayout.Button("Add Item")){
// 	            marketScript.AddItem();
//         	}
//         GUILayout.EndHorizontal();



//         serializedObject.ApplyModifiedProperties();
//     }
// }
#endif