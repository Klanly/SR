using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;


public class RuneUi : MonoBehaviour {

	public RuneUiInterface _RuneUiInterface;
	
	public DragRune CurrentRune;

	public dfPanel[] EquippedPanel;
	
	public dfPanel Carousel;
	public dfPanel NewRune;
//	public dfPanel NewRing;
	
	public RuneCrousel _runeCarousel;
	
	private ItemRune runestats;
	
	public bool isRune;
	public bool isTransmute;

	
	public void setInterface(RuneUiInterface RuneInterface)
	{
		_RuneUiInterface = RuneInterface;
	}
		
	public void gotoRune(int number)
	{
		/// Create Function for Goto Ring in Carousel Cover Flow Class
		
		//gameObject.transform.FindChild("Ring-Carousel").GetComponent<CarouselCoverFlow>().
	}
	
	private InventorySystem.InventoryItem[] suggestedItemsList; 

	
	// Comment it .. when running form On Register swf callback
	void Start()
	{
		DragRune._tapListener = this.OnEquippedRuneSelected;
		DragRune._runeSwapOutListener = OnRuneSwappedOut;
		
		if(_RuneUiInterface != null)
			_RuneUiInterface.OnRegisterSWFChildCallback(this);

		gameObject.GetComponent<dfPanel>().PerformLayout();
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
	}
	
	private void OnEquippedRuneSelected(DragRune rune)
	{
		_runeCarousel.GoToRune(rune);
	}
	
	private void OnRuneSwappedOut(DragRune rune)
	{
		_runeCarousel.Unhighlight(rune);
	}
	
	public void updateTotalStats()
	{
//		MaxHealth.Text = myHealth.ToString();
//		MaxDamage.Text = myDamage.ToString();
//		MaxWards.Text = myWards.ToString();
	}
	
	public void onSellRune()
	{
		_RuneUiInterface.SellRune(CurrentRune._runeModel.uid, onSellRuneComplete);
	}
	
	public void onBuyRune()
	{
		_RuneUiInterface.BuyRune(CurrentRune._runeModel.uid, onBuyRuneComplete);
	}
	
	public void onBuyRuneComplete(bool responce)
	{
		Debug.Log("onBuyRuneComplete >> " + responce);
		
		CurrentRune._runeModel.market = !responce;
		
		if(responce)
		{
			DragRune currentRune = Array.Find<DragRune>(Carousel.GetComponentsInChildren<DragRune>(), (obj) => obj._runeModel != null && obj._runeModel.uid == CurrentRune._runeModel.uid);
			if(currentRune != null)
			{
				DragRune drag = currentRune.GetComponent<DragRune>();
				drag.Icon = CurrentRune._runeModel.id;
				drag._runeModel = CurrentRune._runeModel;
				drag.isDragAble = true;
				drag.isEquipped = false;
			}
		}
	}
	
	public void onSellRuneComplete(bool responceStr)
	{
		Debug.Log("onSellRuneComplete >> " + responceStr);
		
		// Animate Rune
		CurrentRune._runeModel.market = responceStr;
		
		dfPanel currentRune = Array.Find<dfPanel>(EquippedPanel, (obj) => obj.GetComponent<DragRune>()._runeModel != null && obj.GetComponent<DragRune>()._runeModel.uid == CurrentRune._runeModel.uid);
		if(currentRune != null)
		{
			DragRune drag = currentRune.GetComponent<DragRune>();
			drag.Icon = string.Empty;
			drag._runeModel = null;
			drag.isDragAble = false;
			drag.isEquipped = false;
		}
		
		GameManager.instance.suggestedUIRuneList.Add(CurrentRune._runeModel);
	}
	
	Inventory inventory;
	
	public void setInventoryData(Inventory inventory, InventorySystem.InventoryItem[] suggestedItemsList)
	{
		this.inventory = inventory;
		this.suggestedItemsList = suggestedItemsList;
		
		dfControl tempRune;
		
		int runeCount = 0;
		
		Debug.Log(inventory.bag.bagItems.Count+" count of bagitems");

		for(int i=0;i< inventory.bag.bagItems.Count;i++)
		{
			if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRune)
			{
				tempRune = Carousel.AddPrefab(NewRune.gameObject);
				tempRune.gameObject.name = runeCount + "";
				runeCount++;
				
				tempRune.GetComponent<DragRune>()._runeModel = inventory.bag.bagItems[i] as ItemRune;
				runestats = tempRune.GetComponent<DragRune>()._runeModel;
				
				//runestats.SetItemFromInventory(inventory.bag.bagItems[i]);
				tempRune.GetComponent<DragRune>().Icon = runestats.id;// loadRuneImage();
					
			}
		}
		
		SetEquippedRunePanel(runeCount);
		
		SetSuggestedRunePanel(runeCount + inventory.staffRunes.Count);
		
		setEquippedRune(inventory);
		
		for(int i = 0; i < Carousel.transform.childCount; i ++)
		{
			Carousel.transform.GetChild(i).name = (Mathf.Abs(int.Parse(Carousel.transform.GetChild(i).name) - Carousel.transform.childCount) - 1) + "";
		}	
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
	
	private void SetSuggestedRunePanel(int startingIndex)
	{
		dfControl tempRune;
		
		for(int i=0;i<suggestedItemsList.Length;i++)
		{
			if(suggestedItemsList[i].ItemType() == InventoryItem.Type.kRune)
			{
				tempRune = Carousel.AddPrefab(NewRune.gameObject);
				tempRune.gameObject.name = (startingIndex + i) + "";
				tempRune.GetComponent<DragRune>()._runeModel =  suggestedItemsList[i] as ItemRune;
				
				runestats = tempRune.GetComponent<DragRune>()._runeModel;
				runestats.market = true;
				tempRune.GetComponent<DragRune>().Icon = runestats.id;
			}
		}
	}
		
	
	private void SetEquippedRunePanel(int startingIndex)
	{
		dfControl tempRune;
		
		for(int i = 0;i<inventory.staffRunes.Count;i++)
			{
				tempRune = Carousel.AddPrefab(NewRune.gameObject);
				tempRune.gameObject.name = (startingIndex + i) +"";
				tempRune.GetComponent<DragRune>()._runeModel =  inventory.staffRunes[i];
							
				runestats = tempRune.GetComponent<DragRune>()._runeModel;
				tempRune.GetComponent<DragRune>().Icon = runestats.id;
				(tempRune.Find("Equipped") as dfSprite).IsVisible = true;
				tempRune.GetComponent<DragRune>().isEquipped = true;
			}
	}
	
	private void setEquippedRune(Inventory inventory)
	{
		for(int i = 0;i< inventory.staffRunes.Count;i++)
		{
			NewRune.GetComponent<DragRune>()._runeModel = inventory.staffRunes[i];
			runestats = NewRune.GetComponent<DragRune>()._runeModel;
			
			EquippedPanel[i].GetComponent<DragRune>()._runeModel = runestats;
			
			EquippedPanel[i].GetComponent<DragRune>().Icon = runestats.id;
		}
	}
	
	public void Update()
	{
		InputWrapper.disableTouch = true;
	}
	
	public void swapTheRune(string ID1,string ID2)
	{
		Debug.Log("Swap Rune" + ID1 + " with" + ID2);
		
		_RuneUiInterface.swapItems(ID1,ID2);
	}
	
	public void OnDestroy()
	{
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
	}
}
