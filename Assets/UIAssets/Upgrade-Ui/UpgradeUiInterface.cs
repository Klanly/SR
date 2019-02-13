using UnityEngine;
using System;
using System.Collections;

public interface UpgradeUiInterface 
{
	void onStartUpgrade(string ObjectID);
	void onBoostButton();
	void OnRegisterSWFChildCallback(MonoBehaviour mono);
	void onReset();
} 
