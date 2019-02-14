using UnityEngine;
using System;
using System.Collections;

public class RingsLootPopup : MonoBehaviour {

	public dfPanel lootPanel;
	
	public dfSprite ringSprite;
	
	public dfLabel ringName;
	
	public dfLabel wardsLabel;
	public dfLabel damageLabel;
	
	public dfSprite fireActiveSprite;
	public dfSprite waterActiveSprite;
	public dfSprite lightningActiveSprite;
	public dfSprite earthActiveSprite;
	
	public dfSprite fireInactiveSprite;
	public dfSprite waterInactiveSprite;
	public dfSprite lightningInactiveSprite;
	public dfSprite earthInactiveSprite;
	
	private Action callback;
	
	
	
	//===============================================================================================================
	//
	public void showLootRingPopup(InventorySystem.ItemRing ring, bool isTransmute, Action callback = null)
	{
		if(isTransmute) 
		{
			//TO-DO ... Logic for transmutation ring needs to be added!
			showLootRingPopup(ring, callback);
		}
		else
		{
			showLootRingPopup(ring, callback);
		}
		
		dfPanel panel = gameObject.GetComponent<dfPanel>();
		if(panel != null)
			panel.PerformLayout();
	}
	//===============================================================================================================
	
	
	
	//===============================================================================================================
	//
	public void showLootRingPopup(InventorySystem.ItemRing ring, Action callback = null)
	{
		ringName.Text = ring.ItemName();
		wardsLabel.Text = ring.wards + "";
		damageLabel.Text = ring.damage + "";
		
		ringSprite.SpriteName = ring.ItemName(); //Add proper atlas for the rings and set it up in the UI in Unity editor first!
		
		if(ring.water > 0)
		{
			waterActiveSprite.IsVisible = true;
			waterInactiveSprite.IsVisible = false;
		}
		else
		{
			waterActiveSprite.IsVisible = false;
			waterInactiveSprite.IsVisible = true;
		}
		
		if(ring.fire > 0)
		{
			fireActiveSprite.IsVisible = true;
			fireInactiveSprite.IsVisible = false;
		}
		else
		{
			fireActiveSprite.IsVisible = false;
			fireInactiveSprite.IsVisible = true;
		}
		
		if(ring.earth > 0)
		{
			earthActiveSprite.IsVisible = true;
			earthInactiveSprite.IsVisible = false;
		}
		else
		{
			earthActiveSprite.IsVisible = false;
			earthInactiveSprite.IsVisible = true;
		}
		
		if(ring.lightning > 0)
		{
			lightningActiveSprite.IsVisible = true;
			lightningInactiveSprite.IsVisible = false;
		}
		else
		{
			lightningActiveSprite.IsVisible = false;
			lightningInactiveSprite.IsVisible = true;
		}
		
		this.callback = callback;
	}
	//===============================================================================================================
	
	
	
	//===============================================================================================================
	//
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == lootPanel)
		{
			if(callback != null)
				callback();
			
			GameObject.Destroy(gameObject);
		}
	}
	//===============================================================================================================
}
