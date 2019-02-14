using UnityEngine;
using System;
using System.Collections;

public class NGKeysLootPopup : MonoBehaviour {

	public UILabel numberOfKeys;
	
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
		NGUITools.BringForward(gameObject);
	}

	public void showLootKeysPopup(int keys, Action callback, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		numberOfKeys.text = "x "+keys;
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
