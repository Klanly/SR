using UnityEngine;
using System.Collections;
using System;

public class Debugger {
	
	public const int MIN_LEVEL = 1;
	public const int MAX_LEVEL = 4;
	
	private static int _logLevel = 1;
	
	public static int LogLevel
	{
		get
		{
			return _logLevel;
		}
		set
		{
			if(value > MAX_LEVEL)
			{
				_logLevel = MAX_LEVEL;
			}
			else if(value < MIN_LEVEL)
			{
				_logLevel = MIN_LEVEL;
			}
			else
			{
				_logLevel = value;
			}
		}
	}
	
	public static void Log(object message)
	{
		if(Debug.isDebugBuild && _logLevel >= 1)
		{
			Debug.Log(message);
		}
	}
	
	public static void Log(object message, UnityEngine.Object context)
	{
		if(Debug.isDebugBuild && _logLevel >= 1)
		{
			Debug.Log(message, context);
		}
	}
	
	public static void LogWarning(object message)
	{
		if(Debug.isDebugBuild && _logLevel >= 2)
		{
			Debug.LogWarning(message);
		}
	}
	
	public static void LogWarning(object message, UnityEngine.Object context)
	{
		if(Debug.isDebugBuild && _logLevel >= 2)
		{
			Debug.LogWarning(message, context);
		}
	}
	
	public static void LogException(Exception ex)
	{
		if(Debug.isDebugBuild && _logLevel >= 3)
		{
			Debug.LogException(ex);
		}
	}
	
	public static void LogException(Exception ex, UnityEngine.Object context)
	{
		if(Debug.isDebugBuild && _logLevel >= 3)
		{
			Debug.LogException(ex, context);
		}
	}
	
	public static void LogError(object message)
	{
		if(Debug.isDebugBuild && _logLevel >= 4)
		{
			Debug.LogError(message);
		}
	}
	
	public static void LogError(object message, UnityEngine.Object context)
	{
		if(Debug.isDebugBuild && _logLevel >= 4)
		{
			Debug.LogError(message, context);
		}
	}
}
