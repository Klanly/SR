using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;


public class NGRuneUi : MonoBehaviour {
	
	public RuneUiInterface _runeUiInterface;
	
	public SRDragDropRuneItem CurrentRune;
	
	public SRDragDropRuneContainer[] EquippedPanel;
	
	public SRCenterOnChildRune _runeCarousel;
	public SRDragDropRuneItem baseRune;
	
	public void setInterface(RuneUiInterface runeInterface)
	{
		_runeUiInterface = runeInterface;
	}
	
	public void GoToRuneNumber(int number)	{	_runeCarousel.GoToIndexNumber(number);	}
	
	public void GoToNewRune()				{	_runeCarousel.GoToNewRune();	}

	private InventorySystem.InventoryItem[] suggestedItemsList; 

	public UILabel bagItemCount;
	// Comment it .. when running form On Register swf callback
	void Start()
	{
		if(_runeUiInterface != null)
			_runeUiInterface.OnRegisterSWFChildCallback(this);
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.HideHeartIcon();
//		DragRing._ringSwapOutListener = OnRingSwappedOut;
	
		if(_runeCarousel != null)
			_runeCarousel.onCenter += OnCenterOnChild;

		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);
	}

	
	
	void OnCenterOnChild (GameObject centeredObj)
	{
		SoundManager.instance.PlayScrollSound();
		if(_runeCarousel.centeredObject != null)
		{
			CurrentRune = _runeCarousel.centeredObject.GetComponent<SRDragDropRuneItem>();
			UpdateRuneDescriptionPanel(CurrentRune);
		}

	}


	public void OnEnable()
	{
		this.PerformActionWithDelay(1f, () => {
			_runeCarousel.Recenter();
		});
	}


	public void OnEquippedRuneSelected(SRDragDropRuneContainer rune)
	{
		Debug.Log("OnEquippedRingSelected > " + rune.name);
		OnDragStart();
		rune.ShowGlow();
		_runeCarousel.GoToItem(rune);
	}

	public void OnDragStart()
	{
		Array.ForEach<SRDragDropRuneContainer>(EquippedPanel, (obj) => obj.RemoveGlow());
	}

	public void OnRunesSwapped(ItemRune outRune, ItemRune inRune)
	{
		if(outRune != null)
		{
			_runeCarousel.UnhighlightRune(outRune);
			swapTheRunes(outRune.ItemName() + "|" + outRune.skullLevel, inRune.ItemName() + "|" + inRune.skullLevel);
		}
		else
			swapTheRunes("empty", inRune.ItemName() + "|" + inRune.skullLevel);

		Array.ForEach<SRDragDropRuneItem>(_runeCarousel.GetComponentsInChildren<SRDragDropRuneItem>(), rune => {
				
			if(rune.runeModel.uid == inRune.uid)
			{
				rune.Highlight();
				rune.isEquipped = true;
				rune.isDragable = false;
			}
			else if(outRune != null && rune.runeModel.uid == outRune.uid)
			{
				rune.Unhighlight();
				rune.isEquipped = false;
				rune.isDragable = true;
			}
		});
		int index = 0;
		Transform runeCarouselTransform = _runeCarousel.transform;
		for(int i = 0; i < runeCarouselTransform.childCount; i++) {
			if(runeCarouselTransform.GetChild(i) == _runeCarousel.mCenteredObject.transform) {
				index = i;
				if(index + 1 >= runeCarouselTransform.childCount) {
					index = 0;
				} else {
					index++;
				}
				_runeCarousel.GoToIndexNumber(index);
				break;
			}
		}
	}

	//ngui interface buy/sell button cb
	public void OnBuySellButton(UILabel buttonText)
	{
		if(buttonText.text == "Buy")
		{
			onBuyRune();
		}
		else
			onSellRune();
	}

	private void onSellRune()
	{
		_runeUiInterface.SellRune(CurrentRune.runeModel.uid, onSellRuneComplete);
	}
	
	private void onBuyRune()
	{
		_runeUiInterface.BuyRune(CurrentRune.runeModel.uid, onBuyRuneComplete);
	}
	
	public void onBuyRuneComplete(bool responce)
	{
		Debug.Log("onBuyRingComplete >> " + responce);
		
		CurrentRune.runeModel.market = !responce;
		
		if(responce)
		{
			SRDragDropRuneItem currentRune = Array.Find<SRDragDropRuneItem>(_runeCarousel.GetComponentsInChildren<SRDragDropRuneItem>(), (obj) => obj.runeModel != null && obj.runeModel.uid == CurrentRune.runeModel.uid);
			if(currentRune != null)
			{
				currentRune.isEquipped = false;
				currentRune.runeModel.market = false;
				currentRune.isDragable = true;

				SoundManager.instance.PlayMenuOkSound();
				CurrentRune = _runeCarousel.centeredObject.GetComponent<SRDragDropRuneItem>();
				UpdateRuneDescriptionPanel(CurrentRune);
			}
		}
		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
	}

	public void Update()
	{
//		if(_runeCarousel.centeredObject != null)
//		{
//			CurrentRune = _runeCarousel.centeredObject.GetComponent<SRDragDropRuneItem>();
//			UpdateRuneDescriptionPanel(CurrentRune);
//		}
	}

	public void onSellRuneComplete(bool responceStr)
	{
		Debug.Log("onSellRingComplete >> " + responceStr);

		CurrentRune.runeModel.market = responceStr;

		SRDragDropRuneContainer currentEquippedRing = Array.Find<SRDragDropRuneContainer>(EquippedPanel, (obj) => obj.runeModel != null && obj.runeModel.uid == CurrentRune.runeModel.uid);
		if(currentEquippedRing != null)
		{
			currentEquippedRing.runeModel = null;

			CurrentRune.Unhighlight();
			CurrentRune.isDragable = false;
			CurrentRune.isEquipped = false;
			CurrentRune.runeModel.market = true;
			SoundManager.instance.PlayMenuCancelSound();
		}
		
		CurrentRune = _runeCarousel.centeredObject.GetComponent<SRDragDropRuneItem>();
		UpdateRuneDescriptionPanel(CurrentRune);
		GameManager.instance.suggestedUIRuneList.Add(CurrentRune.runeModel);
		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
	}
	
	Inventory inventory;
	
	public void setInventoryData(Inventory inventory, InventorySystem.InventoryItem[] suggestedItemsList)
	{
		this.inventory = inventory;
		this.suggestedItemsList = suggestedItemsList;
		
		SetSuggestedRunesPanel();
		SetEquippedRunesPanel();

		Debug.LogError("inventory count = "+inventory.bag.bagItems.Count+" suggested count = "+suggestedItemsList.Length);

		for(int i=0;i< inventory.bag.bagItems.Count;i++)
		{
			if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRune)
			{
				SRDragDropRuneItem aRing = NGUITools.AddChild(_runeCarousel.gameObject, baseRune.gameObject).GetComponent<SRDragDropRuneItem>();
				aRing.runeModel = inventory.bag.bagItems[i] as ItemRune;
				aRing.runeModel.market = false;
				aRing.isEquipped = false;
				aRing.isDragable = true;
			}
		}

		setEquippedRune(inventory);

		Destroy(baseRune.gameObject);
		_runeCarousel.Recenter();
	}
	
//	private void ClearCarousel()
//	{
//		while(Carousel.transform.childCount > 0)
//		{
//			Debug.Log("Carousel.transform.childCount >> " + Carousel.transform.childCount);
//			GameObject.DestroyImmediate(Carousel.transform.GetChild(0));
//		}
//	}
	

	#region population of side panel

	public UILabel _ringNameRightPanel;
	public UILabel _ringLevelRightPanel;
	public UISprite _newRightPanel;
	public UISprite _suggestedRightPanel;
	public UILabel _buyButtonLabel;
	public UISprite _buyButtonGemIcon;
	public UISprite _buyButtonDustIcon;
	public UILabel _buySellCostLabel;
	public UISprite _buffIcon;
	public UILabel _runeDescription;


	private void UpdateRuneDescriptionPanel(SRDragDropRuneItem dragDropRune)
	{

		ItemRune runeModel = dragDropRune.runeModel;
		Debug.LogError(runeModel.id);
		if(runeModel.id.Equals("")) {
			Debug.LogError("is empty");
			return;
		}
		_ringNameRightPanel.text = runeModel.id;
		_ringLevelRightPanel.text = "Level " + runeModel.skullLevel;
		_newRightPanel.gameObject.SetActive(runeModel.isNew);
		_suggestedRightPanel.gameObject.SetActive(runeModel.isSuggested);
		_runeDescription.text = new Buff(runeModel.buff.id, runeModel.skullLevel).description;
		_buffIcon.spriteName = runeModel.buff.id;

		if(runeModel.market && runeModel.dCost>0)
			ButtonPanel(runeModel.dCost,true,true);
		else if(runeModel.market && runeModel.gCost>0)
			ButtonPanel(runeModel.gCost,false,true);
		else if(!runeModel.market && runeModel.sellCost>0)
			ButtonPanel(runeModel.sellCost,true,false);

	}

	private void ButtonPanel(float cost,bool dCost,bool buy)
	{
		if(buy)
			_buyButtonLabel.text = "Buy";
		else
			_buyButtonLabel.text = "Sell";


		if(dCost)
		{
			_buyButtonGemIcon.gameObject.SetActive(false);
			_buyButtonDustIcon.gameObject.SetActive(true);
		}
		else
		{
			_buyButtonGemIcon.gameObject.SetActive(true);
			_buyButtonDustIcon.gameObject.SetActive(false);
		}

		_buySellCostLabel.text = ((int)cost) + "";
	}
	#endregion
	
	private void SetSuggestedRunesPanel()
	{
		SRDragDropRuneItem tempRune;
		
		for(int i=0;i<suggestedItemsList.Length;i++)
		{
			if(suggestedItemsList[i].ItemType() == InventoryItem.Type.kRune)
			{
				tempRune = NGUITools.AddChild(_runeCarousel.gameObject, baseRune.gameObject).GetComponent<SRDragDropRuneItem>();
				tempRune.runeModel =  suggestedItemsList[i] as ItemRune;
				tempRune.runeModel.market = true;
				tempRune.isDragable = false;
				tempRune.isEquipped = false;
			}
		}
	}
	
	
	private void SetEquippedRunesPanel()
	{
		SRDragDropRuneItem tempRune;
		
		for(int i = 0;i<inventory.staffRunes.Count;i++)
		{
			tempRune = NGUITools.AddChild(_runeCarousel.gameObject, baseRune.gameObject).GetComponent<SRDragDropRuneItem>();
			tempRune.runeModel =  inventory.staffRunes[i];
			tempRune.isEquipped = true;
			tempRune.isDragable = false;
			tempRune.runeModel.market = false;
			tempRune.Highlight();
		}
	}
	
	private void setEquippedRune(Inventory inventory)
	{
		for(int i = 0;i< inventory.staffRunes.Count;i++)
		{
			EquippedPanel[i].runeModel = inventory.staffRunes[i];
		}
	}

	
	public void swapTheRunes(string ID1,string ID2)
	{
		Debug.Log("Swap Ring" + ID1 + " with" + ID2);
		
		_runeUiInterface.swapItems(ID1,ID2);
	}

	public void OnDestroy()
	{
		GameManager._gameState.User._inventory.MarkRunesAsNotNew();
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.ShowHeartIcon();
	}
}
