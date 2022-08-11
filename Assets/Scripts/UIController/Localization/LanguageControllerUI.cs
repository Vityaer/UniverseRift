using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
public class LanguageControllerUI : MonoBehaviour{
	Coroutine coroutineChangeLocale = null; 
	public void ChangeLanguage(TMP_Dropdown laguageDropdown){ 
		if(coroutineChangeLocale == null)
			coroutineChangeLocale = StartCoroutine(SetLocale(laguageDropdown.value));
	}
	IEnumerator SetLocale(int newIDLocale){
		yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[newIDLocale];
		coroutineChangeLocale = null;
	} 
}