// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.Linq;

// [CustomEditor (typeof(Item))]
// public class ItemGUI : Editor{
//     string newNameBonus;
// 	Item itemController;
//     public override void OnInspectorGUI () {
//     	base.OnInspectorGUI();
//     	itemController = (Item)target;
// 		EditorGUILayout.LabelField("Bonus");
		
// 		if(itemController.bonuses.Count > 0){
// 			for (int index = 0; index < itemController.bonuses.Count; index++) {
// 				KeyValuePair<string, int> item = itemController.bonuses.ElementAt(index);
// 				itemController.bonuses[item.Key] = EditorGUILayout.IntField(item.Key, item.Value);
// 			}
// 		}
// 		// pathCore.showGizmos = EditorGUILayout.Toggle("Show Gizmos", pathCore.showGizmos);
// 		// pathCore.deltaX = EditorGUILayout.Vector2Field("Delta (Xmin, Xmax):", pathCore.deltaX);
// 		// pathCore.deltaY = EditorGUILayout.Vector2Field("Delta (Ymin, Ymax):", pathCore.deltaY);
// 		// pathCore.twoNode = EditorGUILayout.ObjectField("Second node", pathCore.twoNode, typeof(Transform), true) as Transform;
// 		GUILayout.BeginHorizontal("box");
//        		if (GUILayout.Button("Add bonus")){
// 	            NewBonus(newNameBonus);
//         	}
//         	if (GUILayout.Button("Remove last bonus")){
// 	            RemoveBonus();
//         	}
//     	GUILayout.EndHorizontal();
//        		newNameBonus = EditorGUILayout.TextField("Name new bonus:", newNameBonus);

    	
// 	}
// 	private void NewBonus(string newNameBonus){
// 		if(!itemController.bonuses.ContainsKey(newNameBonus)){
// 			itemController.bonuses.Add(newNameBonus, 0);
// 		}
// 	}
// 	private void RemoveBonus(){
// 		if(itemController.bonuses.Count > 0){
// 			KeyValuePair<string, int> item = itemController.bonuses.ElementAt(itemController.bonuses.Count - 1);
// 			itemController.bonuses.Remove(item.Key);
// 		}
// 	}
// }
