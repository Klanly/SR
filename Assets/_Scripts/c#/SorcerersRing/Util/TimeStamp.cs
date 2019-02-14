using UnityEngine;
using System;
	
public class TimeStamp 
{
	public static System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
	
	public static double CurrentTimeInMilliSeconds()
	{
		return (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	
	public static long CurrentTimeInSeconds()
	{
		return (long)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	}

	public static long GetSecondsFromEpoch(System.DateTime dateTime)
	{
		if((dateTime - epochStart).TotalSeconds < 0 )
			throw new System.Exception("Invalid dateTime sent. Make sure it is after epoch time!");
		
		long elapsedTicks = dateTime.Ticks - epochStart.Ticks;
		TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
		
		return (long) elapsedSpan.TotalSeconds;
	}
	
	public static double GetDifferenceInSeconds(System.DateTime startDate, System.DateTime endDate)
	{
		return (endDate - startDate).TotalSeconds;
	}


}
