using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Utils
{
    public static bool needLogs = true;

   public static async void DoActionAfterSecondsAsync(Action action, float seconds)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        action?.Invoke();
    }

    public static void ColorLog(string color, string message)
    {
        if (needLogs)
        {
            Debug.Log("<color=" + color + ">" + message + "</color>");
        }
    }

    public static void CustomLog(string message)
    {
        if(needLogs)
        {
            Debug.Log(message);
        }
    }
}
