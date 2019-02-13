using UnityEngine;
using System.Collections;

public interface TransmuteInterface  {
	
	void OnRegisterSWFChildCallback(MonoBehaviour mono);
	void onUpdateTransmute(string UidArray);
	void onPlaceItem();
	bool onFinishButton();
	bool onStartTransmute(string UidArray);
	void OnTransmuteComplete();
	void swapItems(string ID1,string ID2);
}
