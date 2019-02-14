using UnityEngine;
using System;
using System.Collections;

public class KeysLootPopup : MonoBehaviour {
	
	public dfPanel popupPanel;
	public dfLabel numberOfKeys;
	
	private Action callback;
	
	public void showLootKeysPopup(int keys, Action callback)
	{
		numberOfKeys.Text = keys + "";
		this.callback = callback;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(callback != null)
			callback();
		
		GameObject.Destroy(gameObject);
	}
}
