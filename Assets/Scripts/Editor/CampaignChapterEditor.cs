using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #if UNITY_EDITOR_WIN
// using UnityEditor;
// [CustomEditor(typeof(CampaignChapter))]
// public class CampaignChapterEditor : Editor {
// 	CampaignChapter chapterScript;
//   //   public override void OnInspectorGUI(){

// 		// if(chapterScript == null) chapterScript = (CampaignChapter)target;
// 		// EditorGUILayout.LabelField("CampaignChapter");
//   //       chapterScript.Name          = EditorGUILayout.TextField("Name:", chapterScript.Name);
//   //       chapterScript.numChapter    = EditorGUILayout.IntField("Num:", chapterScript.numChapter);
//   //       EditorGUILayout.Space();
// 		// ShowMissions(serializedObject.FindProperty("missions"));
//   //       serializedObject.ApplyModifiedProperties();
		
//   //       // DrawDefaultInspector ();
//   //   }
//     private void ShowMissions(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
// 		EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
// 			for (int i = 0; i < list.arraySize; i++) {
// 				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
// 				if (list.GetArrayElementAtIndex(i).isExpanded) {
// 					EditorGUI.indentLevel += 1;
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("location"));

// 					ShowLevelEnemy(list.GetArrayElementAtIndex(i).FindPropertyRelative("_listEnemy"));
// 					EditorGUILayout.Space();
					
// 					ShowReward(list.GetArrayElementAtIndex(i).FindPropertyRelative("winReward"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("isRrouletteWinReward"));

// 					ShowReward(list.GetArrayElementAtIndex(i).FindPropertyRelative("autoFightReward"));
// 					ShowReward(list.GetArrayElementAtIndex(i).FindPropertyRelative("defeatReward"));

// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("isRrouletteDefeatReward"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("saveResult"));

// 					EditorGUI.indentLevel -= 1;
// 				}
// 			}
// 		}
// 		EditorGUI.indentLevel -= 1;
//     }
//     private void ShowLevelEnemy(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
// 		EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
// 			for (int i = 0; i < list.arraySize; i++) {
// 				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
// 				if (list.GetArrayElementAtIndex(i).isExpanded) {
// 					EditorGUI.indentLevel += 1;
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("enemyPrefab"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("level"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("rating"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("position"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("HP"));
// 					EditorGUI.indentLevel -= 1;
// 				}
// 			}
// 		}
// 		EditorGUI.indentLevel -= 1;
//     }
//     private void ShowReward(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
//     	EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			ShowResourceList(list.FindPropertyRelative("listRewardResource"));
// 			ShowItemList(list.FindPropertyRelative("listRewardItem"));
// 			ShowSplinterList(list.FindPropertyRelative("listRewardSplinter"));
//     	}
// 		EditorGUI.indentLevel -= 1;
//     }
//     private void ShowResourceList(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
// 		EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
// 			for (int i = 0; i < list.arraySize; i++) {
// 				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
// 				if (list.GetArrayElementAtIndex(i).isExpanded) {
// 					EditorGUI.indentLevel += 1;
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("res"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue"));

// 					switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue").enumValueIndex){
// 						case 1:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("posibility"));
// 							break;
// 						case 2:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("min"));
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("max"));
// 							break;
// 					}
// 					EditorGUI.indentLevel -= 1;
// 				}
// 			}
// 		}
// 		EditorGUI.indentLevel -= 1;
//     }
//     private void ShowItemList(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
// 		EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
// 			for (int i = 0; i < list.arraySize; i++) {
// 				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
// 				if (list.GetArrayElementAtIndex(i).isExpanded) {
// 					EditorGUI.indentLevel += 1;
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("ID"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue"));

// 					switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue").enumValueIndex){
// 						case 0:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("count"));
// 							break;
// 						case 1:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("count"));
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("posibility"));
// 							break;
// 						case 2:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("min"));
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("max"));
// 							break;
// 					}
// 					EditorGUI.indentLevel -= 1;
// 				}
// 			}
// 		}
// 		EditorGUI.indentLevel -= 1;
//     }
//     private void ShowSplinterList(SerializedProperty list){
//     	EditorGUILayout.PropertyField(list, false);
// 		EditorGUI.indentLevel += 1;
// 		if (list.isExpanded) {
// 			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
// 			for (int i = 0; i < list.arraySize; i++) {
// 				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
// 				if (list.GetArrayElementAtIndex(i).isExpanded) {
// 					EditorGUI.indentLevel += 1;
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("ID"));
// 					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue"));

// 					switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeIssue").enumValueIndex){
// 						case 0:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("count"));
// 							break;
// 						case 1:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("count"));
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("posibility"));
// 							break;
// 						case 2:
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("min"));
// 							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("max"));
// 							break;
// 					}
// 					EditorGUI.indentLevel -= 1;
// 				}
// 			}
// 		}
// 		EditorGUI.indentLevel -= 1;
//     }
// }
// #endif
