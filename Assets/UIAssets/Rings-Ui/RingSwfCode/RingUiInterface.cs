using UnityEngine;
using System;
using System.Collections;

public interface RingUiInterface
{
	void backButtonClicked(string getInventoryData);//getInventoryData());
	void swapItems(string ringID1,string ringID2);
	
	void BuyRing(string RingUid, Action<bool> successDel);
	void SellRing(string RingUid, Action<bool> successDel);  
	void OnRegisterSWFChildCallback(MonoBehaviour mono);
}
