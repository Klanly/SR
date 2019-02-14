using UnityEngine;
using System;
using System.Collections;

public class NGSoulBagLootPopup : MonoBehaviour {

	public UILabel numberOfSouls;
	
	private Action callback;

	public void repositionToCenter()
	{
		Transform child = transform.FindChild ("Panel");
		
		if(child == null)
			return;
		
		UIPanel panel = child.GetComponent<UIPanel>();
		//UIPanel panel = transform.GetComponentInChildren<UIPanel>();
		
		panel.leftAnchor.target = null;
		panel.rightAnchor.target = null;
		panel.topAnchor.target = null;
		panel.bottomAnchor.target = null;
		
		panel.ResetAndUpdateAnchors();
		
		panel.transform.localPosition = Vector3.zero;
	}

	public void showLootKeysPopup(int souls, Action callback, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		numberOfSouls.text = "x "+ souls;
		this.callback = callback;
		SoundManager.instance.PlayTreasureLootSound();
	}
	
	public void OnClick()
	{
		if(callback != null)
			callback();
		
		GameObject.Destroy(gameObject);
	}
}
