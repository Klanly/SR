using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;


public class NGRingUi : MonoBehaviour
{	
	public RingUiInterface _RingUiInterface;
	
	public UILabel MaxHealth;
	public UILabel MaxWards;
	public UILabel MaxDamage;
	
	public float myHealth;
	public float myDamage;
	public float myWards;

	public SRDragDropRingItem CurrentRing;
	
	public SRDragDropRingContainer[] EquippedPanel;
	
	public SRCenterOnChildRing _ringCarousel;
	public SRDragDropRingItem baseRing;
	
	private ItemRing ringstats;

	public UISprite earthSprite;
	public UISprite fireSprite;
	public UISprite waterSprite;
	public UISprite lightningSprite;

	public UILabel bagItemCount;


	void OnEnable() {
		_ringCarousel.SetEnable(true);
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);
	}

	public void setInterface (RingUiInterface RingInterface)
	{
		_RingUiInterface = RingInterface;
	}
	
	public void GoToRingNumber (int number, bool instant = false)
	{
		_ringCarousel.GoToIndexNumber (number, instant);
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
			_ringCarousel.SetEnable(false);
		}
	}
	
	public void GoToNewRing ()
	{
		_ringCarousel.GoToNewRing ();
	}
	
	public void updateHealth (float h)
	{
		myHealth = h;
		updateTotalStats ();
	}
	
	public void updateDamage (float h)
	{
		myDamage = h;
		updateTotalStats ();
	}
	
	public void updateWards (float h)
	{
		myWards = h;
		updateTotalStats ();
	}
	
	private InventorySystem.InventoryItem[] suggestedItemsList; 

	// Comment it .. when running form On Register swf callback
	void Start ()
	{
		if (_RingUiInterface != null)
			_RingUiInterface.OnRegisterSWFChildCallback (this);
		
		UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
		UIManager.instance.generalSwf.ToggleTopStats (true);
		UIManager.instance.generalSwf.HideHeartIcon ();


		if (_ringCarousel != null) {
			_ringCarousel.onCenter = OnCenterOnChild;
		}
//		DragRing._ringSwapOutListener = OnRingSwappedOut;

//		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
	}


	void OnCenterOnChild (GameObject centeredObj)
	{
		SoundManager.instance.PlayScrollSound();
		if (_ringCarousel.centeredObject != null) {
			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropRingItem> ();
			UpdateRingDescriptionPanel (CurrentRing);
			//			Debug.LogError("Updating rinf description panel");
		}
	}

	public void OnEquippedRingSelected (SRDragDropRingContainer ring)
	{
		Debug.Log ("OnEquippedRingSelected > " + ring.name);
		OnDragStart ();
		ring.ShowGlow ();
		_ringCarousel.GoToItem (ring);
	}

	public void OnDragStart ()
	{
		Array.ForEach<SRDragDropRingContainer> (EquippedPanel, (obj) => obj.RemoveGlow ());
	}

	public void OnRingsSwapped (ItemRing outRing, ItemRing inRing)
	{
		if (outRing != null) {
			_ringCarousel.UnhighlightRing (outRing);
			swapTheRings (outRing.ItemName () + "|" + outRing.skullLevel, inRing.ItemName () + "|" + inRing.skullLevel);
		} else
			swapTheRings ("empty", inRing.ItemName () + "|" + inRing.skullLevel);

		Array.ForEach<SRDragDropRingItem> (_ringCarousel.GetComponentsInChildren<SRDragDropRingItem> (), ring => {
				
			if (ring.ringModel.uid == inRing.uid) {
				ring.Highlight ();
				ring.isEquipped = true;
				ring.isDragable = false;
			} else if (outRing != null && ring.ringModel.uid == outRing.uid) {
				ring.Unhighlight ();
				ring.isEquipped = false;
				ring.isDragable = true;
			}
		});
		Debug.LogError("OnRingsSwapped");
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
//			return;
		}
		int index = 0;
		Transform ringCarouselTransform = _ringCarousel.transform;
		for(int i = 0; i < ringCarouselTransform.childCount; i++) {
			if(ringCarouselTransform.GetChild(i) == _ringCarousel.mCenteredObject.transform) {
				index = i;
				if(index + 1 >= ringCarouselTransform.childCount) {
					index = 0;
				} else {
					index++;
				}
				Debug.LogError("OnRingsSwapped - inside - "+index);
//				_ringCarousel.GoToIndexNumber(index);
				GoToRingNumber(index);
				break;
			}
		}
	}

	public void updateTotalStats ()
	{
		MaxHealth.text = myHealth.ToString ();
		MaxDamage.text = myDamage.ToString ();
		MaxWards.text = myWards.ToString ();
		UIManager.instance.generalSwf.UpdateHealth ();
	}

	//ngui interface buy/sell button cb
	public void OnBuySellButton (UILabel buttonText)
	{
		if (buttonText.text == "Buy") {
			onBuyRing ();
		} else
			onSellRing ();
		RefreshUI ();
	}

	private void onSellRing ()
	{
		_RingUiInterface.SellRing (CurrentRing.ringModel.uid, onSellRingComplete);
		RefreshSpells ();

	}
	
	private void onBuyRing ()
	{
		_RingUiInterface.BuyRing (CurrentRing.ringModel.uid, onBuyRingComplete);
		RefreshSpells ();
	}
	
	public void onBuyRingComplete (bool responce)
	{
		Debug.Log ("onBuyRingComplete >> " + responce);
		
		CurrentRing.ringModel.market = !responce;
		
		if (responce) {
			SRDragDropRingItem currentRing = Array.Find<SRDragDropRingItem> (_ringCarousel.GetComponentsInChildren<SRDragDropRingItem> (), (obj) => obj.ringModel != null && obj.ringModel.uid == CurrentRing.ringModel.uid);
			if (currentRing != null) {
				currentRing.isEquipped = false;
				currentRing.ringModel.market = false;
				currentRing.isDragable = true;
			}
		}
		SoundManager.instance.PlayMenuOkSound();
		RefreshSpells ();
		CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropRingItem> ();
		UpdateRingDescriptionPanel (CurrentRing);
//		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
	}

	public void Update ()
	{
//		if (_ringCarousel.centeredObject != null) {
//			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropRingItem> ();
//			UpdateRingDescriptionPanel (CurrentRing);
////			Debug.LogError("Updating rinf description panel");
//		}
	}

	public void onSellRingComplete (bool responceStr)
	{
		Debug.Log ("onSellRingComplete >> " + responceStr);

		CurrentRing.ringModel.market = responceStr;

		SRDragDropRingContainer currentEquippedRing = Array.Find<SRDragDropRingContainer> (EquippedPanel, (obj) => obj.ringModel != null && obj.ringModel.uid == CurrentRing.ringModel.uid);
		if (currentEquippedRing != null) {
			currentEquippedRing.ringModel = null;

			CurrentRing.Unhighlight ();
			CurrentRing.isDragable = false;
			CurrentRing.isEquipped = false;
			CurrentRing.ringModel.market = true;
		}
		SoundManager.instance.PlayMenuCancelSound();

		GameManager.instance.suggestedUIRingList.Add (CurrentRing.ringModel);

		CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropRingItem> ();
		UpdateRingDescriptionPanel (CurrentRing);

		RefreshSpells ();
//		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
//		bagItemCount.text = inventory.TotalItemCount()+"/"+inventory.bag.capacity;
	}
	
	Inventory inventory;
	
	public void setInventoryData (Inventory inventory, InventorySystem.InventoryItem[] suggestedItemsList)
	{
		this.inventory = inventory;
		this.suggestedItemsList = suggestedItemsList;
		
		SetSuggestedRingsPanel ();
		SetEquippedRingsPanel ();

		for (int i=0; i< inventory.bag.bagItems.Count; i++) {
			if (inventory.bag.bagItems [i].ItemType () == InventoryItem.Type.kRing) {
				SRDragDropRingItem aRing = NGUITools.AddChild (_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropRingItem> ();
//				Debug.Log ("inventory.bag.bagItems[i] > " + inventory.bag.bagItems [i].ToString ());
				aRing.ringModel = inventory.bag.bagItems [i] as ItemRing;
				Debug.Log ("inventory.bag.bagItems[i] RING > " + aRing.ringModel.ToString ());
				aRing.ringModel.market = false;
				aRing.isEquipped = false;
				aRing.isDragable = true;
			}
		}
		setEquippedRing (inventory);

		Destroy (baseRing.gameObject);
		_ringCarousel.Recenter ();

		RefreshSpells ();
	}
	
//	private void ClearCarousel()
//	{
//		while(Carousel.transform.childCount > 0)
//		{
//			Debug.Log("Carousel.transform.childCount >> " + Carousel.transform.childCount);
//			GameObject.DestroyImmediate(Carousel.transform.GetChild(0));
//		}
//	}
	
	private void RefreshSpells ()
	{
		User user = GameManager._gameState.User;
		user.SetRingsAndPetStats ();
		
		if (user._hasFire)
			fireSprite.gameObject.SetActive (true);
		else
			fireSprite.gameObject.SetActive (false);
		
		if (user._hasEarth)
			earthSprite.gameObject.SetActive (true);
		else
			earthSprite.gameObject.SetActive (false);
		
		if (user._hasLightning)
			lightningSprite.gameObject.SetActive (true);
		else
			lightningSprite.gameObject.SetActive (false);
		
		if (user._hasWater)
			waterSprite.gameObject.SetActive (true);
		else
			waterSprite.gameObject.SetActive (false);
	}

	#region population of side panel

	public UISprite _fireRightPanel;
	public UISprite _waterRightPanel;
	public UISprite _earthRightPanel;
	public UISprite _lightningRightPanel;
	public UILabel _ringNameRightPanel;
	public UILabel _ringLevelRightPanel;
	public UISprite _newRightPanel;
	public UISprite _suggestedRightPanel;
	public UISprite _damageWardIconRightPanel;
	public UILabel _damageWardLabelRightPanel;
	public UILabel _buyButtonLabel;
	public UISprite _buyButtonGemIcon;
	public UISprite _buyButtonDustIcon;
	public UILabel _buySellCostLabel;

	public UIButton _buySellPanel;

	private void UpdateRingDescriptionPanel (SRDragDropRingItem dragDropRing)
	{
		ItemRing ringModel = dragDropRing.ringModel;

		_fireRightPanel.gameObject.SetActive (ringModel.fire > 0);
		_waterRightPanel.gameObject.SetActive (ringModel.water > 0);
		_earthRightPanel.gameObject.SetActive (ringModel.earth > 0);
		_lightningRightPanel.gameObject.SetActive (ringModel.lightning > 0);
		_ringNameRightPanel.text = ringModel.id;
		_ringLevelRightPanel.text = "Level " + ringModel.skullLevel;
		_newRightPanel.gameObject.SetActive (ringModel.isNew);
		_suggestedRightPanel.gameObject.SetActive (ringModel.isSuggested);

		NGUITools.SetActive(_damageWardIconRightPanel.gameObject, true);
		NGUITools.SetActive(_damageWardLabelRightPanel.gameObject, true);

		if (ringModel.damage > 0) {
			_damageWardIconRightPanel.spriteName = "TopDamageIconSmall";
			_damageWardLabelRightPanel.text = "x" + ringModel.damage;
		} else if (ringModel.wards > 0) {
			_damageWardIconRightPanel.spriteName = "WardIcon";
			_damageWardLabelRightPanel.text = "x" + ringModel.wards;
		} else if (ringModel.life > 0) {
			_damageWardIconRightPanel.spriteName = "HealthIconMain";
			_damageWardLabelRightPanel.text = "x" + ringModel.life;
		} else {
			NGUITools.SetActive(_damageWardIconRightPanel.gameObject, false);
			NGUITools.SetActive(_damageWardLabelRightPanel.gameObject, false);
		}

//		Debug.LogError("damage = "+ringModel.damage+" - wards = "+ringModel.wards+" life = "+ringModel.life);

		if (ringModel.market && ringModel.dCost > 0)
			ButtonPanel (ringModel.dCost, true, true);
		else if (ringModel.market && ringModel.gCost > 0)
			ButtonPanel (ringModel.gCost, false, true);
		else if (!ringModel.market && ringModel.sellCost > 0)
			ButtonPanel (ringModel.sellCost, true, false);

		// TODO Refactor - very bad way to disable buying or selling of Sorcerer's Ring
		if(ringModel.ItemName().Equals("Sorcerer's Ring"))
		{
			_buySellPanel.gameObject.SetActive(false);
		} else {
			_buySellPanel.gameObject.SetActive(true);
		}

		bagItemCount.text = GameManager._gameState.User._inventory.bag.BagCount().ToString()+"/"+GameManager._gameState.User._inventory.bag.capacity.ToString();
	}

	private void ButtonPanel (float cost, bool dCost, bool buy)
	{
		if (buy)
			_buyButtonLabel.text = "Buy";
		else
			_buyButtonLabel.text = "Sell";


		if (dCost) {
			_buyButtonGemIcon.gameObject.SetActive (false);
			_buyButtonDustIcon.gameObject.SetActive (true);
		} else {
			_buyButtonGemIcon.gameObject.SetActive (true);
			_buyButtonDustIcon.gameObject.SetActive (false);
		}

		_buySellCostLabel.text = ((int)cost) + "";
	}
	#endregion
	
	private void SetSuggestedRingsPanel ()
	{
		SRDragDropRingItem tempRing;
		
		for (int i=0; i<suggestedItemsList.Length; i++) {
			if (suggestedItemsList [i].ItemType () == InventoryItem.Type.kRing) {
				tempRing = NGUITools.AddChild (_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropRingItem> ();
				tempRing.ringModel = suggestedItemsList [i] as ItemRing;
				tempRing.ringModel.market = true;
				tempRing.isDragable = false;
				tempRing.isEquipped = false;
			}
		}
	}
	
	
	private void SetEquippedRingsPanel ()
	{
		SRDragDropRingItem tempRing;
		
		for (int i = 0; i<inventory.equippedRings.Count; i++) {
			tempRing = NGUITools.AddChild (_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropRingItem> ();
			tempRing.ringModel = inventory.equippedRings [i];
			tempRing.isEquipped = true;
			tempRing.isDragable = false;
			tempRing.ringModel.market = false;
			tempRing.Highlight ();
		}
	}
	
	private void setEquippedRing (Inventory inventory)
	{
		for (int i = 0; i< inventory.equippedRings.Count; i++) {
			EquippedPanel [i].ringModel = inventory.equippedRings [i];
		}
	}

	
	public void swapTheRings (string ID1, string ID2)
	{
		Debug.Log ("Swap Ring" + ID1 + " with" + ID2);
		
		_RingUiInterface.swapItems (ID1, ID2);
		
		RefreshSpells ();
	}

	public void RefreshUI ()
	{
		updateHealth (GameManager._gameState.User.totalLife);
		updateDamage (GameManager._gameState.User.damage);
		updateWards (GameManager._gameState.User._wards);
	}

	public void OnDestroy ()
	{
		GameManager._gameState.User._inventory.MarkRingsAsNotNew ();
		
		UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats (true);
		UIManager.instance.generalSwf.ShowHeartIcon ();
	}
}
