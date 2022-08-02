using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR_WIN
[CustomEditor(typeof(InfoHero))]
[CanEditMultipleObjects]
public class InfoHeroEditor:Editor{
	InfoHero InfoHeroScript;

    public override void OnInspectorGUI(){
        // serializedObject.Update();

		if(InfoHeroScript == null) InfoHeroScript = (InfoHero)target;
//Main
		Sprite image = null; 
		GUILayout.Label("General info", EditorStyles.boldLabel);
		InfoHeroScript.generalInfo.Name = EditorGUILayout.TextField("Name hero:", InfoHeroScript.generalInfo.Name);
        InfoHeroScript.generalInfo.race = (Race)EditorGUILayout.EnumPopup("Rare:", InfoHeroScript.generalInfo.race);
        InfoHeroScript.generalInfo.rare = (Rare)EditorGUILayout.EnumPopup("Race:", InfoHeroScript.generalInfo.rare);
        InfoHeroScript.generalInfo.ClassHero = (Vocation)EditorGUILayout.EnumPopup("ClassHero:", InfoHeroScript.generalInfo.ClassHero);
        InfoHeroScript.generalInfo.idHero = EditorGUILayout.IntField("ID Prefab:", InfoHeroScript.generalInfo.idHero);
		if(InfoHeroScript.generalInfo.idHero > 0) {image = InfoHeroScript.generalInfo.ImageHero; }else{image = null;}
		EditorGUILayout.ObjectField("Sprite", image ,typeof(Sprite));
        InfoHeroScript.generalInfo.Level = EditorGUILayout.IntField("Level:", InfoHeroScript.generalInfo.Level);
        InfoHeroScript.generalInfo.ratingHero = EditorGUILayout.IntSlider("Rating:", InfoHeroScript.generalInfo.ratingHero, 1, 18);
        InfoHeroScript.characts.baseCharacteristic.Mellee = EditorGUILayout.Toggle("Mellee ?", InfoHeroScript.characts.baseCharacteristic.Mellee);
        if(InfoHeroScript.characts.baseCharacteristic.Mellee == false){
	        InfoHeroScript.PrefabArrow = (GameObject) EditorGUILayout.ObjectField("PrefabArrow:",InfoHeroScript.PrefabArrow, typeof(GameObject), true);
	    }else{InfoHeroScript.PrefabArrow = null;}
        InfoHeroScript.generalInfo.isAlive = EditorGUILayout.Toggle("isAlive ?", InfoHeroScript.generalInfo.isAlive);
    	

    	EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("characts"));

        //Resistance
        EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("resistances"));

		//IncreaseCharacteristics
        EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("incCharacts"));
        
        //skills
        EditorGUILayout.Space();
		ShowSkills(serializedObject.FindProperty("skills"));
        
        EditorGUILayout.Space();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Evolutions"));
        serializedObject.ApplyModifiedProperties();
        // DrawDefaultInspector ();
    }
   public static void ShowSkills (SerializedProperty list) {
   	GUILayout.Label("skill", EditorStyles.boldLabel);
	   	EditorGUILayout.PropertyField(list, false);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
				if (list.GetArrayElementAtIndex(i).isExpanded) {
					EditorGUI.indentLevel += 1;
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("name"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("ID"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("image"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("isActive"));
					ShowLevelSkill(list.GetArrayElementAtIndex(i).FindPropertyRelative("Skill_Level"));
					EditorGUI.indentLevel -= 1;
				}
			}
		}
		EditorGUI.indentLevel -= 1;
	}
	public static void ShowLevelSkill(SerializedProperty list){
		EditorGUILayout.PropertyField(list, false);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
				if (list.GetArrayElementAtIndex(i).isExpanded) {
					EditorGUI.indentLevel += 1;
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("requireNumBreakthrough"));
					ShowEffectLevelSkill(list.GetArrayElementAtIndex(i).FindPropertyRelative("effects"));
					EditorGUI.indentLevel -= 1;
				}	
			}
		}
		EditorGUI.indentLevel -= 1;
	}
	public static void ShowEffectLevelSkill(SerializedProperty list){
		EditorGUILayout.PropertyField(list, false);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
				if (list.GetArrayElementAtIndex(i).isExpanded) {
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("performance"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeEvent"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("countExecutions"));
					ShowActionEffectLevelSkill(list.GetArrayElementAtIndex(i).FindPropertyRelative("listAction"));
				}
			}
		}
		EditorGUI.indentLevel -= 1;
	} 
	public static void ShowActionEffectLevelSkill(SerializedProperty list){
		EditorGUILayout.PropertyField(list, false);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
				if (list.GetArrayElementAtIndex(i).isExpanded) {
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeAction"));

					switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeAction").enumValueIndex){
						case 0:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("simpleAction"));
						 	break;
						case 1:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectBuff"));
						 	break;
						case 2:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectDots"));
						 	break;
						case 3:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectMark"));
						 	break;
						case 4:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectChangeCharacteristic"));
						 	break;
						case 5:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectStatus"));
						 	break;
						case 6:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectOther"));
						 	break;
						case 7:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("effectSpecial"));
						 	break;
					}
					
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("sideTarget"));
					switch(list.GetArrayElementAtIndex(i).FindPropertyRelative("sideTarget").enumValueIndex){
						case 0:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeSelect"));
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("recalculateTarget"));
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("countTarget"));
							break;
						case 1:
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeSelect"));
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("recalculateTarget"));
							EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("countTarget"));
							break;
					}
					if(list.GetArrayElementAtIndex(i).FindPropertyRelative("rounds").arraySize == 0){
						EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("amount"));
						EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeNumber"));
					}
					ShowRoundActionEffectLevelSkill(list.GetArrayElementAtIndex(i).FindPropertyRelative("rounds"));
					ShowRoundActionEffectLevelSkill(list.GetArrayElementAtIndex(i).FindPropertyRelative("RepeatCall"));
				}
			}
		}
		EditorGUI.indentLevel -= 1;
	}


	public static void ShowRoundActionEffectLevelSkill(SerializedProperty list){
		EditorGUILayout.PropertyField(list, false);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
				if (list.GetArrayElementAtIndex(i).isExpanded) {
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("typeNumber"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("amount"));
				}
			}
		}
		EditorGUI.indentLevel -= 1;
	}
}
#endif