using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// #if UNITY_EDITOR_WIN
// using UnityEditor;

//Editor
// [CustomEditor(typeof(SplintersList))]
// [CanEditMultipleObjects]
// public class SplinterEditor : Editor {
//     public override void OnInspectorGUI(){
// 		SplintersList splinterScript = (SplintersList)target;
// 		EditorGUILayout.LabelField("Splinter");
//         ShowListSplinters(serializedObject.FindProperty("list"));
//         serializedObject.ApplyModifiedProperties();
//     }

//     private void ShowListSplinters(SerializedProperty list){
//       EditorGUILayout.PropertyField(list, false);
//     EditorGUI.indentLevel += 1;
//     if (list.isExpanded) {
//       EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
//       for (int i = 0; i < list.arraySize; i++) {
//         EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
//         if (list.GetArrayElementAtIndex(i).isExpanded) {
//           EditorGUI.indentLevel += 1;
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("id"));
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("description"));
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeSplinter"));
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("rating"));
//           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("requireAmount"));

//           EditorGUILayout.Space();
          

//           EditorGUI.indentLevel -= 1;
//         }
//       }
//     }
//     EditorGUI.indentLevel -= 1;
//     }
// }
// #endif