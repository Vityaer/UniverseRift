using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR_WIN

public class NewHeroWindow : EditorWindow{
/*
	string nameHero = "Name...";
	Race race;
	Vocation classHero;
	int ratingHero;
	int idHero;
	int level;
	bool mellee;
	Characteristics characts = new Characteristics();
    int countResouceForUp;
	List<Resource> listResounceForUp = new List<Resource>();
    List<float> incResList = new List<float>();
    IncreaseCharacteristics increseCharats = new IncreaseCharacteristics();
    Resistance resistance = new Resistance();
    int countSkills;
    List<int> countEffect = new List<int>();
    List<int> countActionEffect = new List<int>();
    List<Skill> skills = new List<Skill>();


    Vector2 scrollPos;
    [MenuItem("Window/New Hero")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        NewHeroWindow window = (NewHeroWindow)EditorWindow.GetWindow(typeof(NewHeroWindow));
        window.Show();
    }

    void OnGUI(){
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


        GUILayout.Label("General info", EditorStyles.boldLabel);
        nameHero = EditorGUILayout.TextField("Name hero:", nameHero);
        idHero = EditorGUILayout.IntField("ID Prefab:", idHero);
        race = (Race)EditorGUILayout.EnumPopup("Race:", race);
        ratingHero = EditorGUILayout.IntSlider("Rating:", ratingHero, 1, 18);
        mellee = EditorGUILayout.Toggle("Mellee?", mellee);
        level = EditorGUILayout.IntField("Level:", level);

        //characteristic
        EditorGUILayout.Space();
        GUILayout.Label("Сharacteristics", EditorStyles.boldLabel);
        characts.limitLevel = EditorGUILayout.IntField("Limit level:", characts.limitLevel);
        characts.Attack = EditorGUILayout.IntField("Attack:", characts.Attack);
        characts.HP = EditorGUILayout.IntField("HP:", characts.HP);
        characts.Armor = EditorGUILayout.IntField("Armor:", characts.Armor);
        characts.Initiative = EditorGUILayout.IntField("Initiative:", characts.Initiative);
        characts.ProbabilityCriticalAttack = EditorGUILayout.FloatField("ProbabilityCriticalAttack:", characts.ProbabilityCriticalAttack);
        characts.DamageCriticalAttack = EditorGUILayout.FloatField("DamageCriticalAttack:", characts.DamageCriticalAttack);
        characts.Accuracy = EditorGUILayout.FloatField("Accuracy:", characts.Accuracy);
        characts.CleanDamage = EditorGUILayout.FloatField("CleanDamage:", characts.CleanDamage);
        characts.Dodge = EditorGUILayout.FloatField("Dodge:", characts.Dodge);
        characts.CountTargetForSimpleAttack = EditorGUILayout.IntSlider("Count target for attack:", characts.CountTargetForSimpleAttack, 1, 6);
        characts.isAlive = EditorGUILayout.Toggle("isAlive?", characts.isAlive);
        countResouceForUp = EditorGUILayout.IntField("countResouceForUp:", countResouceForUp);
		while (countResouceForUp < listResounceForUp.Count){
		    listResounceForUp.RemoveAt( listResounceForUp.Count - 1 );
            incResList.RemoveAt(incResList.Count - 1 );
        }
		while (countResouceForUp > listResounceForUp.Count){
		    listResounceForUp.Add(new Resource());
            incResList.Add(new float());

        }
       	for (int i= 0; i < listResounceForUp.Count; i++) {
       		listResounceForUp[i].Name  = (TypeResource)EditorGUILayout.EnumPopup("TypeResource:", listResounceForUp[i].Name);
            listResounceForUp[i].Count = EditorGUILayout.FloatField("Count:", listResounceForUp[i].Count);
	        listResounceForUp[i].E10   = EditorGUILayout.IntField("E10:", listResounceForUp[i].E10);  
            incResList[i]              = EditorGUILayout.FloatField("inc:", incResList[i]);
		}


        //IncreaseCharacteristics
        EditorGUILayout.Space();
        GUILayout.Label("IncreaseCharacteristics", EditorStyles.boldLabel);
        increseCharats.increaseAttack = EditorGUILayout.FloatField("increaseAttack:", increseCharats.increaseAttack);
        increseCharats.increaseHP = EditorGUILayout.FloatField("increaseHP:", increseCharats.increaseHP);
        increseCharats.increaseArmor = EditorGUILayout.FloatField("increaseArmor:", increseCharats.increaseArmor);
        increseCharats.increaseInitiative = EditorGUILayout.FloatField("increaseInitiative:", increseCharats.increaseInitiative);
        increseCharats.increaseProbabilityCriticalAttack = EditorGUILayout.FloatField("increaseProbabilityCriticalAttack:", increseCharats.increaseProbabilityCriticalAttack);
        increseCharats.increaseDamageCriticalAttack = EditorGUILayout.FloatField("increaseDamageCriticalAttack:", increseCharats.increaseDamageCriticalAttack);
        increseCharats.increaseDodge = EditorGUILayout.FloatField("increaseDodge:", increseCharats.increaseDodge);
        increseCharats.increaseAccuracy = EditorGUILayout.FloatField("increaseAccuracy:", increseCharats.increaseAccuracy);
        increseCharats.increaseCleanDamage = EditorGUILayout.FloatField("increaseCleanDamage:", increseCharats.increaseCleanDamage);
        increseCharats.increaseMagicResistance = EditorGUILayout.FloatField("increaseMagicResistance:", increseCharats.increaseMagicResistance);
        increseCharats.increaseCritResistance = EditorGUILayout.FloatField("increaseCritResistance:", increseCharats.increaseCritResistance);
        increseCharats.increasePoisonResistance = EditorGUILayout.FloatField("increasePoisonResistance:", increseCharats.increasePoisonResistance);



        //Resistance
        EditorGUILayout.Space();
        GUILayout.Label("Resistance", EditorStyles.boldLabel);

        resistance.MagicResistance = EditorGUILayout.FloatField("MagicResistance:", resistance.MagicResistance);
        resistance.CritResistance = EditorGUILayout.FloatField("CritResistance:", resistance.CritResistance);
        resistance.PoisonResistance = EditorGUILayout.FloatField("PoisonResistance:", resistance.PoisonResistance);
        resistance.EfficiencyHeal = EditorGUILayout.FloatField("EfficiencyHeal:", resistance.EfficiencyHeal);
        resistance.StunResistance = EditorGUILayout.FloatField("StunResistance:", resistance.StunResistance);
        resistance.PetrificationResistance = EditorGUILayout.FloatField("PetrificationResistance:", resistance.PetrificationResistance);
        resistance.FreezingResistance = EditorGUILayout.FloatField("FreezingResistance:", resistance.FreezingResistance);
        resistance.AstralResistance = EditorGUILayout.FloatField("AstralResistance:", resistance.AstralResistance);
        resistance.DumbResistance = EditorGUILayout.FloatField("DumbResistance:", resistance.DumbResistance);

        /*
        //skills
        EditorGUILayout.Space();
        GUILayout.Label("Skills", EditorStyles.boldLabel);

        countSkills = EditorGUILayout.IntField("countSkills:", countSkills);
        while (countSkills < skills.Count){
            
            skills[skills.Count - 1].effects.RemoveAt(skills[skills.Count - 1].effects.Count -1);
            countEffect.RemoveAt(countEffect.Count - 1);
            
            skills.RemoveAt( skills.Count - 1 );
        }
        while (countSkills > skills.Count){
            skills.Add(new Skill());
            countEffect.Add(new int());
            countActionEffect.Add(new int());

        }
        for(int i=0; i< skills.Count; i++) {
            skills[i].Name  = EditorGUILayout.TextField("Name:", skills[i].Name);
            skills[i].Image = (Sprite) EditorGUILayout.ObjectField ("Sprite:", skills[i].Image, typeof (Sprite), false);
            skills[i].Description  = EditorGUILayout.TextField("Description:", skills[i].Description);
            skills[i].isActive = EditorGUILayout.Toggle("isActive?", skills[i].isActive);
            countEffect[i] = EditorGUILayout.IntField("countEffect:", countEffect[i]);
            for(int j=0; j < countEffect[i]; j++) {
                skills[i].effects[j].typeEvent = (TypeEvent)EditorGUILayout.EnumPopup("Type Event:", skills[i].effects[j].typeEvent);
            }
        }
        */
/*
        if (GUILayout.Button("Create")){
            CreateHero();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

	// public ListResource resForLevelUP;
    }

    public void CreateHero(){
        Debug.Log("create hero");
    } 
*/
}
#endif