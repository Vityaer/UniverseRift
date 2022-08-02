using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageControllerScript : MonoBehaviour{

	public LanguageObject currentLanguage;
	public List<LanguageObject> languages = new List<LanguageObject>();
    
    public HeroLocalization GetLocalizationHero(int IDHero){
    	return currentLanguage.GetLocalizationHero(IDHero);
    }


    public void ChangeLanguage(int IDLanguage){
    	if(IDLanguage < languages.Count)
	    	currentLanguage = languages[IDLanguage];
    }

    public delegate void Del();
    public Del observerChangeLanguage;
    public void RegisterOnChangeLanguage(Del d){
    	observerChangeLanguage += d; 
    } 
    public void UnRegisterOnChangeLanguage(Del d){
    	observerChangeLanguage -= d;
    }
    public void OnChangeLanguage(){
    	if(observerChangeLanguage != null)
	    	observerChangeLanguage();
    }

    private static LanguageControllerScript instance;
    public static LanguageControllerScript Instance{get => instance;}
    public void Awake(){
    	if(instance == null){ instance = this;} else { Destroy(this); }
    }

    public void Start(){
    	if(currentLanguage == null){
    		currentLanguage = languages[0];
    	}
    }
}
