using System;
using UnityEngine;

public class GameAnalyticsManager //: AnalyticsManager
{
	public void logEvent(string eventname)
	{
		if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
		//	GA.API.Design.NewEvent(eventname);
		}
	}
	 
	public void logEventWihParams(string eventname, string[] parms)
	{
		if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			string paramStr = "";
			if(parms.Length > 0)
			{
				for (int i=0; i<parms.Length - 2; i++)
					paramStr += parms[i] + ", ";
				paramStr = parms[parms.Length-1];
			}
			//GA.API.Quality.NewEvent(eventname, paramStr);
		}
	}
}