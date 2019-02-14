using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface AnalyticsManager 
{

	void logEvent(string eventname);

//	void logEvent <T> (string eventname, T parameters);
	void logEvent  (string eventname, Dictionary<string, object> parameters);

	void logEvent(Analytics.CurrencyTrackType trackWhat, int amount);
	
	void logInappEvent(string productID, string transactionID, string transactionReceipt, string signature, Analytics.TransactionTransitionState transactionState);
}
