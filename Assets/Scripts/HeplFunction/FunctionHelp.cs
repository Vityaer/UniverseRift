using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;

internal class FunctionHelp : MonoBehaviour {

	public static string BigDigit( float Num, int qENum, bool xFlag = false){
		string    result  =  string.Empty;
		if(xFlag) result = string.Concat(result, "x");
		result = string.Concat(result, ((int) Mathf.Floor(Num)).ToString());
		AddPowerE10(ref result, qENum);
		return result;
	}
	private static void AddPowerE10(ref string result, int qENum){
		string postFix = "";
		switch (qENum) {
			case 3:
				postFix = "K";
				break;
			case 6:
				postFix = "M";
				break;
			case 9:
				postFix = "B";
				break;	
		}
		result = string.Concat(result, postFix);
	}  
	public static string TimerText(float time){
		int Num1 = (int) Mathf.Floor(time);
		int Num2 = (int) Mathf.Floor((time - Num1)*10f); 
		string result = string.Concat( Num1.ToString(), ".", Num2.ToString(), " sec");
		return result;
	}
	public static void ClearFile(string Name){
		StreamWriter sw = new StreamWriter(Application.dataPath + Name);
		sw.Close();
	}
	// public static string AmountToString(TypeIssue typeIssue, int min, int  max, int count){
	// 	string result = string.Empty;
	// 	switch(typeIssue){
	// 		case TypeIssue.Necessarily:
	// 		case TypeIssue.Perhaps:
	// 			result = string.Concat(result, count.ToString());
	// 			break;
	// 		case TypeIssue.Range:
	// 			result = string.Concat(min.ToString(), "-", max.ToString());
	// 			break;
	// 	}
	// 	return result;
	// }
	// public static string ResourceAmountToString(TypeIssue typeIssue,Resource res, float min, float  max, float count){
	// 	string result = string.Empty;
	// 	Resource curRes = new Resource(res.Name, 0, res.E10);
	// 	switch(typeIssue){
	// 		case TypeIssue.Necessarily:
	// 			result = string.Concat(res.ToString());
	// 			break;
	// 		case TypeIssue.Perhaps:
	// 			curRes.Count = count;  
 // 				result = string.Concat(curRes.ToString());
	// 			break;
	// 		case TypeIssue.Range:
	// 			curRes.Count = min;
	// 			result = string.Concat(curRes.ToString(), "-");
	// 			curRes.Count = max;
	// 			result = string.Concat(result, curRes.ToString());
	// 			break;
	// 	}
	// 	return result;
	// }
	public static DateTime StringToDateTime(string strDate){
		DateTime convertedDate = new DateTime();
		try{
        	convertedDate = Convert.ToDateTime(strDate);
    	} catch (FormatException) {
    		convertedDate = DateTime.Now; 
    	}
    	return convertedDate;
	}
	public static string AmountFromRequireCount(int currentAmount, int maxCount){
		return string.Concat(currentAmount.ToString(), "/", maxCount.ToString());
	}
	public static string AmountFromRequireCountWithColorLess(int currentAmount, int maxCount){
		string result = string.Empty;
		if(currentAmount < maxCount){
			result = string.Concat("<color=red>",currentAmount.ToString(), "</color>","/", maxCount.ToString());
		}else{
			result = AmountFromRequireCount(currentAmount, maxCount);
		}
		return result;
	}
	public static string AmountFromRequireCount(BigDigit currentAmount, BigDigit maxCount){
		return string.Concat(currentAmount.ToString(), "/", maxCount.ToString());
	}
	public static string AmountFromRequireCountWithColorLess(BigDigit currentAmount, BigDigit maxCount){
		string result = string.Empty;
		if(currentAmount < maxCount){
			result = string.Concat("<color=red>",currentAmount.ToString(), "</color>","/", maxCount.ToString());
		}else{
			result = AmountFromRequireCount(currentAmount, maxCount);
		}
		return result;
	}
	public static int CalculateCountTact(DateTime previousDateTime, int MaxCount = 8640, int lenthTact = 5){
		TimeSpan interval = FunctionHelp.CalculateTimeHasPassed(previousDateTime);
		int tact = (int) (interval.TotalSeconds)/lenthTact;
		tact = Math.Min(tact, MaxCount);
		return tact;
	}
	public static TimeSpan CalculateTimeHasPassed(DateTime previousDateTime){
		DateTime localDate = DateTime.Now;
		TimeSpan interval = localDate - previousDateTime;
		return interval;
	}
	public static string TextLevel(int level){
		return string.Concat("Уровень ", level.ToString());
	}
	public static DateTime GetDateTimeNow(){
		return DateTime.Now;
	}
	public static string TimeSpanConvertToSmallString(TimeSpan interval){
		string result = string.Empty;
    	if(interval.Days > 0){
    		result = String.Concat(interval.Days.ToString(), "d ", interval.Hours.ToString(), "h"); 
    	}else if(interval.Hours > 0){
    		result = String.Concat(interval.Hours.ToString(), "h ", interval.Minutes.ToString(), "m"); 
    	}else if(interval.Minutes > 0){
    		result = String.Concat(interval.Minutes.ToString(), "m ", interval.Seconds.ToString(), "s");
    	}else{
    		result = String.Concat(interval.Seconds.ToString(), "s");
    	}
    	return result;
	}
}
