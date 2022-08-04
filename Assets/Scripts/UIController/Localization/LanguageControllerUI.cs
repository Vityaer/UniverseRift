using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageControllerUI : MonoBehaviour{
	Coroutine coroutineChangeLocale = null; 
	public void ChangeLanguage(int newIDLocale){
		if(coroutineChangeLocale == null)
			coroutineChangeLocale = StartCoroutine(SetLocale(newIDLocale));
	}
	IEnumerator SetLocale(int newIDLocale){
		yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[newIDLocale];
		coroutineChangeLocale = null;
	} 
}