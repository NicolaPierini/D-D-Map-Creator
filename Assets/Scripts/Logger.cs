using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    /// <summary>
    /// Method <c>LogMessage</c> log a message in console 
    /// </summary>
    public void LogMessage(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    /// Method <c>LogMessage</c> log a defined type message in console 
    /// </summary>
    public void LogMessage(string message, LogType type)
    {
        switch (type)
        {
            case LogType.Generic:
                Debug.Log(message);
                break;

            case LogType.Warning:
                Debug.LogWarning(message);
                break;

            case LogType.Error:
                Debug.LogError(message);
                break;
        }
    }
}


public enum LogType
{
    Generic,
    Warning,
    Error
}
