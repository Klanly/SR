using UnityEngine;
using System;
using System.Collections;

public class SoulBagLootPopup : MonoBehaviour {
	
	public dfPanel popupPanel;
	public dfLabel numberOfSouls;
	
	private Action callback;
	
	public void showLootKeysPopup(int souls, Action callback)
	{
		numberOfSouls.Text = souls + "";
		this.callback = callback;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(callback != null)
			callback();
		
		GameObject.Destroy(gameObject);
	}
}
