using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #if UNITY_EDITOR_WIN

// using UnityEditor;
// [CustomEditor(typeof(ListRequirements))]
// public class RequirementEditor : Editor {
// 	ListRequirements requirementScript;
//   //   public override void OnInspectorGUI(){
//   //       if(requirementScript == null) requirementScript = (ListRequirements) target;
// 		// EditorGUILayout.LabelField("Requirements");
//   //       ShowListRequirement(serializedObject.FindProperty("listRequirement"));
//   //       serializedObject.ApplyModifiedProperties();
//   //   }

//     public static void ShowListRequirement(SerializedProperty list){
//         EditorGUILayout.PropertyField(list, false);
//         EditorGUI.indentLevel += 1;
//         if (list.isExpanded) {
//             EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
//             for (int i = 0; i < list.arraySize; i++) {
//                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
//                 if (list.GetArrayElementAtIndex(i).isExpanded) {
//                     EditorGUI.indentLevel += 1;
//                         EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("type"));
//                         ShowListStageRequirement(list.GetArrayElementAtIndex(i).FindPropertyRelative("stages"), list.GetArrayElementAtIndex(i).FindPropertyRelative("type").enumValueIndex);
                        
//                     EditorGUI.indentLevel -= 1;
//                 }
//             }
//         }
//         EditorGUI.indentLevel -= 1;
//     }
//     public static void ShowListStageRequirement(SerializedProperty list, int numType){
//         EditorGUILayout.PropertyField(list, false);
//         TypeRequirement typeRequirement = (TypeRequirement) numType;
//         EditorGUI.indentLevel += 1;
//         if (list.isExpanded) {
//             EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
//             for (int i = 0; i < list.arraySize; i++) {
//                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
//                 if (list.GetArrayElementAtIndex(i).isExpanded) {
//                          switch(typeRequirement){
//                             case TypeRequirement.GetLevel:
//                             case TypeRequirement.DoneChapter:
//                             case TypeRequirement.DoneMission:
//                             case TypeRequirement.SimpleSpin:
//                             case TypeRequirement.SpecialSpin:
//                             case TypeRequirement.BuyItemCount:
//                             case TypeRequirement.DestroyHeroCount:
//                             case TypeRequirement.CountWin:
//                             case TypeRequirement.CountDefeat:
//                             case TypeRequirement.CountPointsOnSimpleArena:
//                             case TypeRequirement.CountPointsOnTournament:
//                             case TypeRequirement.CountSpecialHaring:
//                                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("requireCount"));
//                                 break;  
//                             // case TypeRequirement.GetHeroes:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;
//                             // case TypeRequirement.GetHeroesWithLevel:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;
//                             // case TypeRequirement.GetHeroesWithRating:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;
//                             // case TypeRequirement.GetHeroesCount:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;
//                             // case TypeRequirement.SynthesCount:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;
//                             // case TypeRequirement.SynthesItem:
//                             //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
//                             //     break;  
//                             // case TypeRequirement.BuyItem:
//                             //     requirementScript.listRequirement[i].requireObject = (ScriptableObject) EditorGUILayout.ObjectField ("Object:", requirementScript.listRequirement[i].requireObject, typeof (ScriptableObject), false);
//                             //     break;
//                             // case TypeRequirement.SpendResource:
//                             //     requirementScript.requireRes = (Resource) EditorGUILayout.ObjectField ("Resource:", requirementScript.requireRes, typeof(Resource), true);
//                             //  break;  
//                         }
//                         EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeReward"));
//                         switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeReward").enumValueIndex){
//                             case 0:
//                                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("rewardResource"));
//                                 break;
//                             case 1:
//                                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("rewardItemId"));
//                                 break;
//                             case 2:
//                             case 3:
//                                 EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("rewardObject"));
//                                 break;
//                         }
                        
//                 }
//             }
//         }
//         EditorGUI.indentLevel -= 1;
//     }
// }
// #endif