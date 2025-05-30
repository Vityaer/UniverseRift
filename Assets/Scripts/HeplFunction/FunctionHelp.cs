﻿using Models.Common.BigDigits;
using System;
using System.IO;
using UnityEngine;

public static class FunctionHelp
{
    public static string BigDigit(float Num, int qENum, bool xFlag = false)
    {
        string result = string.Empty;
        if (xFlag) result = string.Concat(result, "x");
        result = string.Concat(result, ((int)Mathf.Floor(Num)).ToString());
        AddPowerE10(ref result, qENum);
        return result;
    }

    private static void AddPowerE10(ref string result, int qENum)
    {
        string postFix = "";
        switch (qENum)
        {
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

    public static string TimerText(float time)
    {
        int Num1 = (int)Mathf.Floor(time);
        int Num2 = (int)Mathf.Floor((time - Num1) * 10f);
        string result = string.Concat(Num1.ToString(), ".", Num2.ToString(), " sec");
        return result;
    }

    public static void ClearFile(string Name)
    {
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
    public static DateTime StringToDateTime(string strDate)
    {
        DateTime convertedDate = new DateTime();
        try
        {
            convertedDate = Convert.ToDateTime(strDate);
        }
        catch (FormatException)
        {
            convertedDate = DateTime.UtcNow;
        }
        return convertedDate;
    }

    public static string AmountFromRequireCount(int currentAmount, int maxCount)
    {
        return string.Concat(currentAmount.ToString(), "/", maxCount.ToString());
    }

    public static string AmountFromRequireCountWithColorLess(int currentAmount, int maxCount)
    {
        string result = string.Empty;
        if (currentAmount < maxCount)
        {
            result = string.Concat("<color=red>", currentAmount.ToString(), "</color>", "/", maxCount.ToString());
        }
        else
        {
            result = AmountFromRequireCount(currentAmount, maxCount);
        }
        return result;
    }

    public static string AmountFromRequireCount(BigDigit currentAmount, BigDigit maxCount)
    {
        return string.Concat(currentAmount.ToString(), "/", maxCount.ToString());
    }

    public static string AmountFromRequireCountWithColorLess(BigDigit currentAmount, BigDigit maxCount)
    {
        string result = string.Empty;
        if (currentAmount < maxCount)
        {
            result = string.Concat("<color=red>", currentAmount.ToString(), "</color>", "/", maxCount.ToString());
        }
        else
        {
            result = AmountFromRequireCount(currentAmount, maxCount);
        }
        return result;
    }

    public static TimeSpan CalculateTimeHasPassed(DateTime previousDateTime)
    {
        DateTime localDate = DateTime.UtcNow;
        TimeSpan interval = localDate - previousDateTime;
        return interval;
    }

    public static DateTime GetDateTimeNow()
    {
        return DateTime.UtcNow;
    }

    public static string TimeSpanConvertToSmallString(TimeSpan interval)
    {
        string result = string.Empty;
        if (interval.Days > 0)
        {
            result = $"{interval.Days}d {interval.Hours}h";
        }
        else if (interval.Hours > 0)
        {
            result = $"{interval.Hours}h {interval.Minutes}m";
        }
        else if (interval.Minutes > 0)
        {
            result = $"{interval.Minutes}m {interval.Seconds}s";
        }
        else
        {
            result = $"{interval.Seconds}s";
        }
        return result;
    }
    public static TimeSpan DeltaDateTimes(DateTime a, DateTime b)
    {
        return (DateTime.Compare(a, b) > 0) ? a - b : b - a;
    }

    public static TimeSpan GetLeftTimeToEnd(DateTime start, TimeSpan timeUp)
    {
        return (start + timeUp - DateTime.UtcNow);
    }

    public static float GetLeftSecondsToEnd(DateTime start, TimeSpan timeUp)
    {
        return (float)GetLeftTimeToEnd(start, timeUp).TotalSeconds;
    }
}
