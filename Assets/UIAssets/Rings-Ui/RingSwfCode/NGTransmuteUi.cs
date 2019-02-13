using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;


public class NGTransmuteUi : MonoBehaviour {
	
	public TransmuteInterface _interface;
	
	public SRDragDropTransmuteItem CurrentRing;
	
	public SRDragDropTransmuteContainer[] EquippedPanel;
	
	public SRCenterOnChildTransmute _ringCarousel;
	public SRDragDropTransmuteItem baseRing;

	public UILabel TransmuteTimeLabel;
	public UILabel TransmutePriceLabel;
	public UILabel TransmuteButtonLabel;

	public UISprite timeIcon;
	public UISprite gemIcon;

	public void setInterface(TransmuteInterface anInterface)
	{
		_interface = anInterface;
	}
	
	private InventorySystem.InventoryItem[] suggestedItemsList; 

	// Comment it .. when running form On Register swf callback
	void Start()
	{
		if(_interface != null)
			_interface.OnRegisterSWFChildCallback(this);

		_ringCarousel.onCenter += OnScrollCenter;

		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.HideHeartIcon();

		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);

		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted))
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialStart);

		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
		gemIcon.gameObject.SetActive(false);
		TransmutePriceLabel.gameObject.SetActive(false);
		Debug.LogError("transmutation capacity - "+ GameManager._gameState.User._inventory.transmutationCube.capacity);

		foreach(SRDragDropTransmuteContainer container in EquippedPanel) {
			container.icon.spriteName = "Locked";
			container.enabled = false;
			container.icon.gameObject.SetActive(true);
		}
		for(int i = 0; i < GameManager._gameState.User._inventory.transmutationCube.capacity; i++)
		{
//			EquippedPanel[i].icon.spriteName = "Locked";
			EquippedPanel[i].enabled = true;
			EquippedPanel[i].icon.gameObject.SetActive(false);

		}

	}
	
	public void OnEquippedItemSelected(SRDragDropTransmuteContainer ring)
	{
		Debug.Log("OnEquippedRingSelected > " + ring.name);
		OnDragStart();
		ring.ShowGlow();
	}

	private void OnScrollCenter(GameObject item) {
		SoundManager.instance.PlayScrollSound();
		if(_ringCarousel.centeredObject != null)
		{
			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropTransmuteItem>();
			UpdateRingDescriptionPanel(CurrentRing);
		}
	}

	public void OnDragStart()
	{
//		Array.ForEach<SRDragDropTransmuteContainer>(EquippedPanel, (obj) => obj.RemoveGlow());
	}

	private List<InventoryItem> itemsList = new List<InventoryItem>();

	public void SwapTransmute(SRDragDropTransmuteContainer container, SRDragDropTransmuteItem inItem)
	{
		Debug.LogError("swapTransmute - "+(container == null ? " null ":"not null"));
		if(container != null && !container.enabled) {
			Debug.LogError("is not enabled");
			return;
		}
		if(container != null)
		{
			itemsList.RemoveAll(target => target.uid == container.ringModel.uid);

			Array.ForEach<SRDragDropTransmuteItem>(_ringCarousel.transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), ring => {
				if(ring.ringModel != null && ring.ringModel.uid == container.ringModel.uid)
					ring.isDragable = true;
			});
		}

		itemsList.Add(inItem.ringModel);

		Array.ForEach<SRDragDropTransmuteItem>(_ringCarousel.transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), ring => {
			if(ring.ringModel != null && ring.ringModel.uid == inItem.ringModel.uid)
				ring.isDragable = false;
		});

		IDictionary transmuteDictionary = null;
		List<InventoryItem> inventoryItems = new List<InventoryItem>();

		if(itemsList.Count > 1)
		{
			itemsList.ForEach(anItem => {
				inventoryItems.Add(GameManager._gameState.User._inventory.GetBagItemForUID(anItem.uid));
			});

			transmuteDictionary = GameManager._gameState.User.Transmute(inventoryItems);

			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial3)
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorial4);
		}
		else
		{
			transmuteDictionary = null;

			Array.ForEach<SRDragDropTransmuteItem>(_ringCarousel.transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), ring => {
				if(ring.isRing != inItem.isRing)
					Destroy(ring.gameObject);
			});

			_ringCarousel.Recenter();
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial2)
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorial3);
//			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial2)
//				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorial3);
		}

		if(transmuteDictionary != null)
		{
			inventoryItems.ForEach(item => Debug.Log("ITEM > "+item.ToString()));
			TransmutePriceLabel.text = transmuteDictionary["gCost"].ToString();
			Debug.Log("transmuteDictionary[\"gCost\"].ToString() > " + transmuteDictionary["gCost"].ToString());
			int gCost = int.Parse(transmuteDictionary["gCost"].ToString());
			if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.TransmutationTutorialStart && GameManager._gameState.User._inventory.gems < gCost)
				GameManager._gameState.User._inventory.gems = gCost;
			TransmuteTimeLabel.text = Upgradeui.ConvertSecToString(int.Parse(transmuteDictionary["Duration"].ToString()));
			gemIcon.gameObject.SetActive(false);
			TransmutePriceLabel.gameObject.SetActive(false);
		}
		else
		{
			TransmutePriceLabel.text = "0";
			TransmuteTimeLabel.text = Upgradeui.ConvertSecToString(0);
		}
		StartCoroutine("GoToNextItem", inItem.ringModel.uid);
	}

	IEnumerator GoToNextItem(string uid) {
		yield return new WaitForSeconds(0.1f);
		int index = 0;
		Transform ringCarouselTransform = _ringCarousel.transform;
		for(int i = 0; i < ringCarouselTransform.childCount; i++) {
			if(ringCarouselTransform.GetChild(i) == _ringCarousel.mCenteredObject.transform) {
//			if(ringCarouselTransform.GetChild(i).GetComponent<SRDragDropTransmuteItem>().ringModel.uid.Equals(uid)) {
				index = i;
				if(index + 1 >= ringCarouselTransform.childCount) {
					// Not 0 because 0 is an inactive Prefab(Gameobject actually)
					index = 1;
				} else {
					index++;
				}
				_ringCarousel.GoToIndexNumber(index);
				break;
			}
		}
	}

	public void OnClick(UILabel buttonLabel)
	{
		if(buttonLabel.text == "Transmute")
		{
			onTransmuteButton();
		}
		else if(buttonLabel.text == "Finish Now")
		{
			onFinishNowButton();
		}
		else if(buttonLabel.text == "Collect") //farhan
		{
			SoundManager.instance.PlayTreasureOpenSound();
			UIManager.instance.generalSwf.ProcessCollectTransmute();
		}
	}

	public void onFinishNowButton()
	{
		Debug.Log("OnFinishButton called");
		if(_interface.onFinishButton())
		{
			TransmuteButtonLabel.text = "Transmute";
			SoundManager.instance.PlayMenuOkSound();
			NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Transmutation);
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorialEnd)
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted);
		
			Destroy(gameObject);
			GameManager.instance.scaleformCamera.levelScene.levelScene.SetVisible(true);

		}
		if(_ringCarousel.centeredObject != null)
		{
			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropTransmuteItem>();
			UpdateRingDescriptionPanel(CurrentRing);
		}
	}

	private void onTransmuteButton()
	{
		List<string> stringList = new List<string>();
		String uidArray = "";
		int count = 0;
		foreach (SRDragDropTransmuteContainer panel in EquippedPanel)
		{
			if(panel.ringModel != null && !string.IsNullOrEmpty( panel.ringModel.uid))
			{
				if(count++ == 0)
				{
					uidArray += panel.ringModel.uid;	
				}
				else 
				{
					uidArray += "," + panel.ringModel.uid;
				}
			}
		}
		
		if(count <= 1)
			return;

		Debug.Log("onTransmute button called");
		if(_interface.onStartTransmute(uidArray))
		{
			isTransmuting = true;
			TransmuteButtonLabel.text = "Finish Now";
			SoundManager.instance.PlayMenuOkSound();

			gemIcon.gameObject.SetActive(true);
			TransmutePriceLabel.gameObject.SetActive(true);

			IDictionary transmuteDictionary = GameManager._gameState.User.transmutationDictionary;
			if(transmuteDictionary != null)
				TransmutePriceLabel.text = transmuteDictionary["BoostCost"].ToString();
		}
		
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial4)
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialEnd);

		if(_ringCarousel.centeredObject != null)
		{
			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropTransmuteItem>();
			UpdateRingDescriptionPanel(CurrentRing);
		}
	}

	public void Update()
	{
//		if(_ringCarousel.centeredObject != null)
//		{
//			CurrentRing = _ringCarousel.centeredObject.GetComponent<SRDragDropTransmuteItem>();
//			UpdateRingDescriptionPanel(CurrentRing);
//		}
	}

	private bool isTransmuting;

	private Inventory inventory;
	private IDictionary transmutationDictionary;
	public void setInventoryData(InventorySystem.Inventory inventory, IDictionary tDictionary)
	{
		this.transmutationDictionary = tDictionary;
		this.inventory = inventory;
		
		SRDragDropTransmuteItem tempTransmute;

		baseRing.gameObject.SetActive(true);

		int ringCount = 0;
		int runeCount = 0;

		for(int i = 0; i < inventory.bag.bagItems.Count; i++)
		{
			if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRing)
				ringCount++;
			else if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRune)
				runeCount++;
		}

		for(int i=0;i< inventory.bag.bagItems.Count;i++)
		{

			if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRing && ringCount >= 2)
			{
				tempTransmute = NGUITools.AddChild(_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropTransmuteItem>();
				
				tempTransmute.ringModel = inventory.bag.bagItems[i] as InventoryItem;

				tempTransmute.isRing = true;
			}
			else if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRune && runeCount >= 2)
			{
				tempTransmute = NGUITools.AddChild(_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropTransmuteItem>();
				
				tempTransmute.ringModel = inventory.bag.bagItems[i] as InventoryItem;

				tempTransmute.isRing = false;
			}
		}
		//		IDictionary transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
		IList itemList = transmutationDictionary["BaseItems"] as IList;
		
		if(itemList.Count == 0)
		{
			isTransmuting = false;

			if(baseRing != null)
				baseRing.gameObject.SetActive(false);

			return;
		}
		else 
		{
			itemList = transmutationDictionary["BaseItems"] as IList;
			isTransmuting = true;
			TransmuteButtonLabel.text = "Finish Now";
			setEquippedTransmute(itemList);
		}	

		if(baseRing != null)
			baseRing.gameObject.SetActive(false);

	}

	private void setEquippedTransmute(IList list)
	{	
		InventoryItem item = null;

		if(transmutationDictionary["Type"].ToString().Equals(InventoryItem.Type.kRing+""))
		{
			for(int i = 0; i <list.Count; i++ )
			{
				IDictionary modelDictionary = list[i] as IDictionary;
				item = InventoryLoader.GetRingObjectFromJsonData(modelDictionary);
				EquippedPanel[i].ringModel = item;
			}
		}
		else
		{
			for(int i = 0; i <list.Count; i++ )
			{
				IDictionary modelDictionary = list[i] as IDictionary;
				item = InventoryLoader.GetRuneObjectFromJsonData(modelDictionary);
				EquippedPanel[i].ringModel = item;
			}
		}
	}

	#region population of side panel

	public UILabel _ringNameRightPanel;
	public UILabel _ringLevelRightPanel;

	public UISprite _fireRightPanelContainer;
	public UISprite _fireRightPanel;
	public UISprite _waterRightPanelContainer;
	public UISprite _waterRightPanel;
	public UISprite _earthRightPanelContainer;
	public UISprite _earthRightPanel;
	public UISprite _lightningRightPanelContainer;
	public UISprite _lightningRightPanel;
	public UISprite _damageWardIconRightPanel;
	public UILabel _damageWardLabelRightPanel;
	public UILabel _runeDescription;

	private void UpdateRingDescriptionPanel(SRDragDropTransmuteItem dragDropRing)
	{
		if(dragDropRing == null || dragDropRing.ringModel == null) 
			return;

		if(!dragDropRing.gameObject.activeInHierarchy) {
			StartCoroutine("GoToNextItem");
			return;
		}
		InventoryItem ringModel = dragDropRing.ringModel;

		_ringNameRightPanel.text = ringModel.id;
		_ringLevelRightPanel.text = "Level " + ringModel.skullLevel;
		if(dragDropRing.isRing) {
			ItemRing itemRing = (ItemRing) ringModel;

			NGUITools.SetActive(_runeDescription.gameObject, false);
			NGUITools.SetActive(_fireRightPanelContainer.gameObject, true);
			NGUITools.SetActive(_waterRightPanelContainer.gameObject, true);
			NGUITools.SetActive(_earthRightPanelContainer.gameObject, true);
			NGUITools.SetActive(_lightningRightPanelContainer.gameObject, true);
			NGUITools.SetActive(_damageWardLabelRightPanel.gameObject, true);

			_fireRightPanel.gameObject.SetActive (itemRing.fire > 0);
			_waterRightPanel.gameObject.SetActive (itemRing.water > 0);
			_earthRightPanel.gameObject.SetActive (itemRing.earth > 0);
			_lightningRightPanel.gameObject.SetActive (itemRing.lightning > 0);

			_damageWardIconRightPanel.gameObject.SetActive(true);
			_damageWardLabelRightPanel.gameObject.SetActive(true);
			if (itemRing.damage > 0) {
				_damageWardIconRightPanel.spriteName = "TopDamageIconSmall";
				_damageWardLabelRightPanel.text = "x" + itemRing.damage;
			} else if (itemRing.wards > 0) {
				_damageWardIconRightPanel.spriteName = "WardIcon";
				_damageWardLabelRightPanel.text = "x" + itemRing.wards;
			} else if (itemRing.life > 0) {
				_damageWardIconRightPanel.spriteName = "HealthIconMain";
				_damageWardLabelRightPanel.text = "x" + itemRing.life;
			} else {
				_damageWardIconRightPanel.gameObject.SetActive(false);
				_damageWardLabelRightPanel.gameObject.SetActive(false);
			}
		} else {
			ItemRune itemRune = (ItemRune) ringModel;

			NGUITools.SetActive(_runeDescription.gameObject, true);
			NGUITools.SetActive(_fireRightPanelContainer.gameObject, false);
			NGUITools.SetActive(_waterRightPanelContainer.gameObject, false);
			NGUITools.SetActive(_earthRightPanelContainer.gameObject, false);
			NGUITools.SetActive(_lightningRightPanel.gameObject, false);
			NGUITools.SetActive(_damageWardLabelRightPanel.gameObject, false);
			_runeDescription.text = new Buff(itemRune.buff.id, itemRune.skullLevel).description;
			_damageWardIconRightPanel.spriteName = itemRune.buff.id;
		}

	}

	#endregion

	private void SetEquippedRingsPanel()
	{
		SRDragDropRingItem tempRing;
		
		for(int i = 0;i<inventory.equippedRings.Count;i++)
		{
			// TODO Refactor - very bad way to disable transmutation of Sorcerer's Ring
			tempRing = NGUITools.AddChild(_ringCarousel.gameObject, baseRing.gameObject).GetComponent<SRDragDropRingItem>();
			tempRing.ringModel =  inventory.equippedRings[i];
			if(tempRing._ringModel.ItemName().Equals("Sorcerer's Ring"))
				continue;
			tempRing.isEquipped = true;
			tempRing.isDragable = false;
			tempRing.ringModel.market = false;
			tempRing.Highlight();
		}
	}
	

	public void SetUpdateData(IDictionary tDictionary)
	{
		int time = Convert.ToInt32(tDictionary["TimeRemaining"]);
		if(time > 0) {
			DateTime dateTime = DateTime.Now.AddSeconds(time);
			NotificationManager.SendLocalNotification(dateTime, NotificationManager.NotificationType.Transmutation);
		}
		StartCountdown(time);
		gemIcon.gameObject.SetActive(true);
		TransmutePriceLabel.gameObject.SetActive(true);
		TransmutePriceLabel.text = Convert.ToInt32(tDictionary["BoostCost"]).ToString();
		Debug.Log("setUpdate called of transmutation - time remaining = "+ tDictionary["TimeRemaining"]);
	}

	public void onTransmuteComplete()
	{
//		isTransmuting = false;
//		removeTransmuteItems();
//		setInventoryData(inventory, transmutationDictionary);

		Destroy(gameObject);
		GameManager.instance.scaleformCamera.levelScene.levelScene.SetVisible(true);
	}

	public void removeTransmuteItems()
	{
		for(int i = 0;i < _ringCarousel.transform.childCount; i++)
		{
			Destroy(_ringCarousel.transform.GetChild(i).gameObject);
		}	
	}

	private void StartCountdown(int initialTimerValue)
	{
//		Debug.Log(" initialTimerValue >> " + initialTimerValue+" isTransmuting => --  "+isTransmuting);
		//		Debug.Log("setUpdate called of transmutation - time remaining = "+ tDictionary["TimeRemaining"]);
		
		if(!GameManager._gameState.User.transmutationDictionary.Contains("Type"))
		{
			TransmutePriceLabel.text = "00";
			TransmuteTimeLabel.text = Upgradeui.ConvertSecToString(0);
			Array.ForEach<SRDragDropTransmuteContainer>(EquippedPanel, aPanel => aPanel.GetComponent<SRDragDropTransmuteContainer>().ringModel = null);
			return;
		}		
		
		if(initialTimerValue < 0)
		{
			TransmuteTimeLabel.text = "?";
			return;
		}
		
		this.PerformActionWithDelay(1f, () => {
			if(initialTimerValue < 5) {
				NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Transmutation);
			}
			TransmuteTimeLabel.text = Upgradeui.ConvertSecToString(initialTimerValue);
			if(initialTimerValue == 0)
			{
				gemIcon.gameObject.SetActive(false);
				timeIcon.gameObject.SetActive(false);
				TransmuteTimeLabel.gameObject.SetActive(false);
				TransmutePriceLabel.gameObject.SetActive(false);
				TransmuteButtonLabel.text = "Collect";
			}
			else if(initialTimerValue > 0)
				StartCountdown(--initialTimerValue);
		});
	}
	
	public void swapTheRings(string ID1,string ID2)
	{
		Debug.Log("Swap Ring" + ID1 + " with" + ID2);
		
		_interface.swapItems(ID1,ID2);
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.ShowHeartIcon();

	}

//	public void OnGUI()
//	{
//		if(GUI.Button(new Rect(10, 10, 100, 30), "Give souls/gems"))
//		{
//			GameManager._gameState.User._inventory.gems = 5000;
//			GameManager._gameState.User._inventory.souls = 50000;
//		}
//		if(GUI.Button(new Rect(10, 50, 100, 30), "Take souls/gems"))
//		{
//			GameManager._gameState.User._inventory.gems = 0;
//			GameManager._gameState.User._inventory.souls = 0;
//		}
//		if(GUI.Button(new Rect(10, 90, 100, 30), "TIMER ZERO"))
//		{
//			StartCountdown(1);
//		}
//	}
}
