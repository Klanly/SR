using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;


public class RingUi : MonoBehaviour {

	public RingUiInterface _RingUiInterface;
	
	public dfLabel MaxHealth;
	public dfLabel MaxWards;
	public dfLabel MaxDamage;
	
	public float myHealth;
	public float myDamage;
	public float myWards;
	
	public DragRing CurrentRing;

	public dfPanel[] EquippedPanel;
	
	public dfPanel Carousel;
	public dfPanel NewRunes;
	public dfPanel NewRing;
	
	public RingCrousel _ringCarousel;
	
	private ItemRing ringstats;
	
	public bool isRune;
	public bool isTransmute;
	
	public dfSprite earthSprite;
	public dfSprite fireSprite;
	public dfSprite waterSprite;
	public dfSprite lightningSprite;
	
	public void setInterface(RingUiInterface RingInterface)
	{
		_RingUiInterface = RingInterface;
	}
		
	public void GoToRingNumber(int number)
	{
		Debug.Log("GoToRingNumber > " + number);
		/// Create Function for Goto Ring in Carousel Cover Flow Class
		
		//gameObject.transform.FindChild("Ring-Carousel").GetComponent<CarouselCoverFlow>().
		
		_ringCarousel.GoToRingNumber(number);
	}
	
	public void GoToNewRing()
	{
		Debug.Log("GoToNewRing");
		//this.PerformActionWithDelay(0.5f, () => {
		//int count = _ringCarousel.transform.childCount;
		_ringCarousel.GoToNewRing();
		//GoToRingNumber(count - 1);
		//});
		
	}
	
	
	private InventorySystem.InventoryItem[] suggestedItemsList; 

	public void updateHealth(float h)
	{
		myHealth = h;
		updateTotalStats();
	}
	
	public void updateDamage(float h)
	{
		myDamage = h;
		updateTotalStats();
	}
	
	public void updateWards(float h)
	{
		myWards = h;
		updateTotalStats();
	}
	
	
	// Comment it .. when running form On Register swf callback
	void Start()
	{
		if(_RingUiInterface != null)
			_RingUiInterface.OnRegisterSWFChildCallback(this);
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.HideHeartIcon();
		gameObject.GetComponent<dfPanel>().PerformLayout();

		DragRing._ringSwapOutListener = OnRingSwappedOut;
	}
	
	private void OnEquippedRingSelected(DragRing ring)
	{
		Debug.Log("OnEquippedRingSelected > " + ring.name);
		_ringCarousel.GoToRing(ring);
	}
	
	private void OnRingSwappedOut(DragRing ring)
	{
		_ringCarousel.UnhighlightRing(ring);
	}
	
	public void updateTotalStats()
	{
		MaxHealth.Text = myHealth.ToString();
		MaxDamage.Text = myDamage.ToString();
		MaxWards.Text = myWards.ToString();
	}
	
	public void onSellRing()
	{
		_RingUiInterface.SellRing(CurrentRing._ringModel.uid, onSellRingComplete);
		RefreshSpells();
	}
	
	public void onBuyRing()
	{
		_RingUiInterface.BuyRing(CurrentRing._ringModel.uid, onBuyRingComplete);
		RefreshSpells();
	}
	
	public void onBuyRingComplete(bool responce)
	{
		Debug.Log("onBuyRingComplete >> " + responce);
		
		CurrentRing._ringModel.market = !responce;
		
		if(responce)
		{
			DragRing currentRing = Array.Find<DragRing>(Carousel.GetComponentsInChildren<DragRing>(), (obj) => obj._ringModel != null && obj._ringModel.uid == CurrentRing._ringModel.uid);
			if(currentRing != null)
			{
				DragRing drag = currentRing.GetComponent<DragRing>();
				drag.Icon = CurrentRing._ringModel.id;
				drag._ringModel = CurrentRing._ringModel;
				drag.isDragAble = true;
				drag.isEquipped = false;
			}
		}
		RefreshSpells();
	}
	
	public void onSellRingComplete(bool responceStr)
	{
		Debug.Log("onSellRingComplete >> " + responceStr);
		
		// Animate Ring
		CurrentRing._ringModel.market = responceStr;
		
		dfPanel currentRing = Array.Find<dfPanel>(EquippedPanel, (obj) => obj.GetComponent<DragRing>()._ringModel != null && obj.GetComponent<DragRing>()._ringModel.uid == CurrentRing._ringModel.uid);
		if(currentRing != null)
		{
			DragRing drag = currentRing.GetComponent<DragRing>();
			drag.Icon = string.Empty;
			drag._ringModel = null;
			drag.isDragAble = false;
			drag.isEquipped = false;
		}
		
		GameManager.instance.suggestedUIRingList.Add(CurrentRing._ringModel);
		
		RefreshSpells();
	}
	
	Inventory inventory;
	
	public void setInventoryData(Inventory inventory, InventorySystem.InventoryItem[] suggestedItemsList)
	{
		this.inventory = inventory;
		this.suggestedItemsList = suggestedItemsList;
		
		dfControl tempRing;
		
		int ringCount = 0;
		if(!isRune || isTransmute)
		{
			for(int i=0;i< inventory.bag.bagItems.Count;i++)
			{
				if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRing)
				{
					tempRing = Carousel.AddPrefab(NewRing.gameObject);
					tempRing.gameObject.name = ringCount + "";
					ringCount++;
					DragRing ringDragComponent = tempRing.GetComponent<DragRing>();
					ringDragComponent._ringModel = inventory.bag.bagItems[i] as ItemRing;
					ringstats = ringDragComponent._ringModel;
					
					//ringstats.SetItemFromInventory(inventory.bag.bagItems[i]);
					ringDragComponent.Icon = ringstats.id;// loadRingImage();
					ringDragComponent.isDragAble = true;
					
						
					if(isTransmute)
						tempRing.GetComponent<DragRing>().isTransmute = true;
				}
			}
			
			SetEquippedRingsPanel(ringCount);
			
			SetSuggestedRingsPanel(ringCount + inventory.equippedRings.Count);
			
			setEquippedRing(inventory);
			
			for(int i = 0; i < Carousel.transform.childCount; i ++)
			{
				Carousel.transform.GetChild(i).name = (Mathf.Abs(int.Parse(Carousel.transform.GetChild(i).name) - Carousel.transform.childCount) - 1) + "";
			}
		}
		
		RefreshSpells();
	}
	
	private void ClearCarousel()
	{
		Debug.Log("Carousel.transform.childCount >> " + Carousel.transform.childCount);
		while(Carousel.transform.childCount > 0)
		{
			Debug.Log("Carousel.transform.childCount >> " + Carousel.transform.childCount);
			GameObject.DestroyImmediate(Carousel.transform.GetChild(0));
		}
	}
	
	private void RefreshSpells()
	{
		User user = GameManager._gameState.User;
		user.SetRingsAndPetStats();
		
		if(user._hasFire)
			fireSprite.IsVisible = true;
		else
			fireSprite.IsVisible = false;
		
		if(user._hasEarth)
			earthSprite.IsVisible = true;
		else
			earthSprite.IsVisible = false;
		
		if(user._hasLightning)
			lightningSprite.IsVisible = true;
		else
			lightningSprite.IsVisible = false;
		
		if(user._hasWater)
			waterSprite.IsVisible = true;
		else
			waterSprite.IsVisible = false;
	}
	
	
	private void SetSuggestedRingsPanel(int startingIndex)
	{
		dfControl tempRing;
		
		for(int i=0;i<suggestedItemsList.Length;i++)
		{
			if(suggestedItemsList[i].ItemType() == InventoryItem.Type.kRing)
			{
				tempRing = Carousel.AddPrefab(NewRing.gameObject);
				tempRing.gameObject.name = (startingIndex + i) + "";
				tempRing.GetComponent<DragRing>()._ringModel =  suggestedItemsList[i] as ItemRing;
				
				ringstats = tempRing.GetComponent<DragRing>()._ringModel;
				ringstats.market = true;
				tempRing.GetComponent<DragRing>().Icon = ringstats.id;
			}
		}
	}
		
	
	private void SetEquippedRingsPanel(int startingIndex)
	{
		dfControl tempRing;
		
		for(int i = 0;i<inventory.equippedRings.Count;i++)
			{
				tempRing = Carousel.AddPrefab(NewRing.gameObject);
				tempRing.gameObject.name = (startingIndex + i) +"";
				tempRing.GetComponent<DragRing>()._ringModel =  inventory.equippedRings[i];
				
				ringstats = tempRing.GetComponent<DragRing>()._ringModel;
				tempRing.GetComponent<DragRing>().Icon = ringstats.id;
				(tempRing.Find("Equipped") as dfSprite).IsVisible = true;
				tempRing.GetComponent<DragRing>().isEquipped = true;
			}
	}
	
	private void setEquippedRing(Inventory inventory)
	{
		if(!isTransmute)
		{
			for(int i = 0;i< inventory.equippedRings.Count;i++)
			{
				NewRing.GetComponent<DragRing>()._ringModel = inventory.equippedRings[i];
				ringstats = NewRing.GetComponent<DragRing>()._ringModel;
				
				EquippedPanel[i].GetComponent<DragRing>()._ringModel = ringstats;
				
				EquippedPanel[i].GetComponent<DragRing>().Icon = ringstats.id;
			}
		}
	}
	
	public void Update()
	{
		InputWrapper.disableTouch = true;
	}
	
	public void swapTheRings(string ID1,string ID2)
	{
		Debug.Log("Swap Ring" + ID1 + " with" + ID2);
		
		_RingUiInterface.swapItems(ID1,ID2);
		
		RefreshSpells();
	}
	
	public void OnDestroy()
	{
		GameManager._gameState.User._inventory.MarkRingsAsNotNew();
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.ShowHeartIcon();
	}
}
