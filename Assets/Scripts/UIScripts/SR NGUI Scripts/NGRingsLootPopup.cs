using UnityEngine;
using System;
using System.Collections;

public class NGRingsLootPopup : MonoBehaviour {
	
	public UISprite ringSprite;
	
	public UILabel ringName;
	
	public UILabel wardsLabel;
	public UILabel damageLabel;
	public UILabel lifeLabel;

	public UISprite fireActiveSprite;
	public UISprite waterActiveSprite;
	public UISprite lightningActiveSprite;
	public UISprite earthActiveSprite;
	
	public UISprite fireInactiveSprite;
	public UISprite waterInactiveSprite;
	public UISprite lightningInactiveSprite;
	public UISprite earthInactiveSprite;
	
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
	public void showLootRingPopup(InventorySystem.ItemRing ring, bool isTransmute, Action callback = null, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		if(isTransmute) 
		{
			//TO-DO ... Logic for transmutation ring needs to be added!
			showLootRingPopup(ring, callback);
		}
		else
		{
			showLootRingPopup(ring, callback);
		}
	}
	//===============================================================================================================
	
	
	
	//===============================================================================================================
	//
	public void showLootRingPopup(InventorySystem.ItemRing ring, Action callback = null, bool anchored = false)
	{
		if( !anchored )
			repositionToCenter();

		ringName.text = ring.ItemName();
		wardsLabel.text = ring.wards.ToString();
		damageLabel.text = ring.damage.ToString();
		lifeLabel.text = ring.life.ToString();
		ringSprite.spriteName = ring.ItemName(); //Add proper atlas for the rings and set it up in the UI in Unity editor first!
		
		if(ring.water > 0)
		{
			waterActiveSprite.gameObject.SetActive(true);
			waterInactiveSprite.gameObject.SetActive(false);
		}
		else
		{
			waterActiveSprite.gameObject.SetActive(false);
			waterInactiveSprite.gameObject.SetActive(true);
		}
		
		if(ring.fire > 0)
		{
			fireActiveSprite.gameObject.SetActive(true);
			fireInactiveSprite.gameObject.SetActive(false);
		}
		else
		{
			fireActiveSprite.gameObject.SetActive(false);
			fireInactiveSprite.gameObject.SetActive(true);
		}
		
		if(ring.earth > 0)
		{
			earthActiveSprite.gameObject.SetActive(true);
			earthInactiveSprite.gameObject.SetActive(false);
		}
		else
		{
			earthActiveSprite.gameObject.SetActive(false);
			earthInactiveSprite.gameObject.SetActive(true);
		}
		
		if(ring.lightning > 0)
		{
			lightningActiveSprite.gameObject.SetActive(true);
			lightningInactiveSprite.gameObject.SetActive(false);
		}
		else
		{
			lightningActiveSprite.gameObject.SetActive(false);
			lightningInactiveSprite.gameObject.SetActive(true);
		}
		
		this.callback = callback;
		SoundManager.instance.PlayTreasureLootSound();

	}
	//===============================================================================================================
	
	

	#region Ui interface clicks callbacks
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
