using UnityEngine;
using System;
using System.Collections;

public class NGRunesLootPopup : MonoBehaviour {
	
	public UISprite runeSprite;
	public UISprite runeBuffSprite;
	
	public UILabel runeName;
	
	public UILabel runeBuffName;
	public UILabel runeBuffLevel;
	public UILabel runeBuffDescription;
	
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

	
	//===============================================================================================================
	//
	public void showLootRunePopup(InventorySystem.ItemRune rune, bool isTransmute, Action callback = null, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		if(isTransmute) 
		{
			//TO-DO ... Logic for transmutation ring needs to be added!
			showLootRunePopup(rune, callback);
		}
		else
		{
			showLootRunePopup(rune, callback);
		}
	}
	//===============================================================================================================
	
	
	
	//===============================================================================================================
	//
	public void showLootRunePopup(InventorySystem.ItemRune rune, Action callback = null, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		runeSprite.spriteName = rune.id;
		runeName.text = rune.id;
		
		runeBuffSprite.spriteName = rune.buff.id;
		runeBuffName.text = rune.buff.id;
		runeBuffLevel.text = rune.buff.skullLevel.ToString();
		runeBuffDescription.text = rune.buff.description;
		
		this.callback = callback;
		SoundManager.instance.PlayTreasureLootSound();

	}
	//===============================================================================================================
	
	

	#region ngui interface method cbs
	//===============================================================================================================
	//
	public void OnClick()
	{
		if(callback != null)
			callback();
		
		GameObject.Destroy(gameObject);

	}
	//===============================================================================================================
	#endregion
}
