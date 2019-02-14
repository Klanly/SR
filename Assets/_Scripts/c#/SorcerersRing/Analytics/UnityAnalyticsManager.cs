using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsManager : MonoBehaviour, AnalyticsManager
{

	void Start() {

		if(Debug.isDebugBuild)
			UnityAnalytics.StartSDK("f089705e-f8f6-43cd-83e1-64867be568a0");
		else
			UnityAnalytics.StartSDK("a3ffc9ea-f566-418d-8ed0-dcd27478cac6");

		Analytics.registerAnalyticsManager(this);

		//TODO Remove
//		StartCoroutine("test");
		UnityAnalytics.CustomEvent("Test", new Dictionary<string, object>
		                           {
			{ "Test", "test" },
		});
	}

	public void logEvent(string eventname)
	{
		UnityAnalytics.CustomEvent("Single", new Dictionary<string, object>
		                      {
			{ "event", eventname },
		});
	}
	 
	public void logEvent (string eventname, Dictionary<string, object> parameters) {
		UnityAnalytics.CustomEvent(eventname, parameters);
	}
	
	public void logEvent(Analytics.CurrencyTrackType trackWhat, int amount) {
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(trackWhat.ToString(), amount);
	}
	
	public void logInappEvent(string productID, string transactionID, string transactionReceipt, string signature, Analytics.TransactionTransitionState transactionState) {
		Debug.LogError("product id - "+productID+" transaction id - "+transactionID+" receipt - "+transactionReceipt+" signature - "+signature);
		UnityAnalytics.Transaction(productID, Convert.ToDecimal(0.99), "USD", transactionReceipt, signature);
	}
}