using UnityEngine;
using System;
using System.Collections;

public interface RuneUiInterface
{
	void backButtonClicked(string getInventoryData);//getInventoryData());
	void swapItems(string runeID1,string runeID2);
	
	void BuyRune(string RuneUid, Action<bool> successDel);
	void SellRune(string RuneUid, Action<bool> successDel);  
	void OnRegisterSWFChildCallback(MonoBehaviour mono);
}
