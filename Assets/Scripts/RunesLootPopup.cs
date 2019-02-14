using UnityEngine;
using System;
using System.Collections;

public class RunesLootPopup : MonoBehaviour {
//
//	public dfPanel lootPanel;
//	
//	public dfSprite runeSprite;
//	public dfSprite runeBuffSprite;
//	
//	public dfLabel runeName;
//	
//	public dfLabel runeBuffName;
//	public dfLabel runeBuffLevel;
//	public dfLabel runeBuffDescription;
//	
//	private Action callback;
//	
//	
//	
//	//===============================================================================================================
//	//
//	public void showLootRunePopup(InventorySystem.ItemRune rune, bool isTransmute, Action callback = null)
//	{
//		if(isTransmute) 
//		{
//			//TO-DO ... Logic for transmutation ring needs to be added!
//			showLootRunePopup(rune, callback);
//		}
//		else
//		{
//			showLootRunePopup(rune, callback);
//		}
//		
//		this.PerformDFLayout();
//	}
//	//===============================================================================================================
//	
//	
//	
//	//===============================================================================================================
//	//
//	public void showLootRunePopup(InventorySystem.ItemRune rune, Action callback = null)
//	{
//		runeSprite.name = rune.id;
//		runeName.Text = rune.id;
//		
//		runeBuffSprite.name = rune.buff.id;
//		runeBuffName.Text = rune.buff.id;
//		runeBuffLevel.Text = rune.buff.skullLevel + "";
//		runeBuffDescription.Text = rune.buff.description;
//		
//		this.callback = callback;
//	}
//	//===============================================================================================================
//	
//	
//	
//	//===============================================================================================================
//	//
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(mouseEvent.Source == lootPanel)
//		{
//			if(callback != null)
//				callback();
//			
//			GameObject.Destroy(gameObject);
//		}
//	}
//	//===============================================================================================================
}
