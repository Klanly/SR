using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using System;

public class TransmuteUi : MonoBehaviour {

	public TransmuteInterface _transmuteInterface;
	
	public DragTransmute CurrentTransmute;

	public dfPanel[] EquippedPanel;
	
	public dfPanel Carousel;
	public dfPanel NewRune;
	public dfPanel NewRing;
	public dfPanel NewTransmute;
	
	private ItemRune runestats;
	private ItemRing ringstats;
	
	//
	public List<string> TransmuteItemSlot;
	//
	public dfLabel TransmutePrice;
	public dfLabel TransmuteTime;
	//
	public dfButton TransmuteButton;
	//
	public static bool isTransmuting;
		
	private Inventory inventory;
	
	public InventoryItem transmutestats;
	private IDictionary transmutationDictionary;
	
	public void setInterface(TransmuteInterface tranmuteInterface)
	{
		_transmuteInterface = tranmuteInterface;
		gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	public TransmuteCrousel _transmuteCarousel;
	
	private List<DragTransmute> itemsList;
	
	void Start()
	{
		DragTransmute._itemSwapOutListener = OnItemSwappedOut;
		if(_transmuteInterface != null)
			_transmuteInterface.OnRegisterSWFChildCallback(this);
		
		for(int i = GameManager._gameState.User._inventory.transmutationCube.capacity; i < EquippedPanel.Length; i++)
		{
			(EquippedPanel[i].Find("lock") as dfSlicedSprite).IsVisible = true;
			EquippedPanel[i].IsEnabled = false;
		}
		
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
		
		itemsList = new List<DragTransmute>();
		
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted))
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialStart);
	}
	
	public void OnItemSwappedOut(DragTransmute transItem)
	{
		_transmuteCarousel.Unhighlight(transItem);
	}
	
	public void Update()
	{
		InputWrapper.disableTouch = true;
	}
	
	/// <summary>
	/// Updates the transmute data form Json.
	/// </summary>
	/// <param name='d1'>
	/// D1. where d1 is Json input
	/// </param>
	public void updateTransmuteData(string d1)
	{
		
	}
	
	public void setInventoryData(InventorySystem.Inventory inventory, IDictionary tDictionary)
	{
//		List<InventoryItem> items =  inventory.bag.bagItems;
		this.transmutationDictionary = tDictionary;
		this.inventory = inventory;
		
		dfControl tempTransmute;

		for(int i=0;i< inventory.bag.bagItems.Count;i++)
		{
			tempTransmute = Carousel.AddPrefab(NewTransmute.gameObject);
				
			tempTransmute.GetComponent<DragTransmute>()._model = inventory.bag.bagItems[i] as InventoryItem;
			transmutestats = tempTransmute.GetComponent<DragTransmute>()._model;
					
			//runestats.SetItemFromInventory(inventory.bag.bagItems[i]);
			tempTransmute.GetComponent<DragTransmute>().Icon = transmutestats.id;// loadRuneImage();
			if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRing)
			{
				tempTransmute.GetComponent<DragTransmute>().isRing = true;
			}else if(inventory.bag.bagItems[i].ItemType() == InventoryItem.Type.kRune)
			{
				tempTransmute.GetComponent<DragTransmute>().isRing = false;
			}
		}
//		IDictionary transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
		IList itemList = transmutationDictionary["BaseItems"] as IList;
		
		if(itemList.Count == 0)
		{
			return;
		}
		else 
		{
			if(transmutationDictionary["Type"].ToString().Equals(InventoryItem.Type.kRing.ToString()))
			{
				itemList = transmutationDictionary["BaseItems"] as IList;
				List<InventorySystem.ItemRing> ringList = new List<InventorySystem.ItemRing>();
				for(int i = 0;i<itemList.Count;i++)
				{
					ringList.Add(InventoryLoader.GetRingObjectFromJsonData(itemList[i] as IDictionary)); 
				}				
				Debug.Log("called setEquipped of rings = >> calling setUpdateDictionaty");
				isTransmuting = true;
				TransmuteButton.Text = "Finish Now";
//				SetUpdateData(transmutationDictionary);
				setEquippedTransmute(itemList);
			}
			else 
			{
				itemList = transmutationDictionary["BaseItems"] as IList;
				List<InventorySystem.ItemRune> runesList = new List<InventorySystem.ItemRune>();
				for(int i = 0;i<itemList.Count;i++)
				{
					runesList.Add(InventoryLoader.GetRuneObjectFromJsonData(itemList[i] as IDictionary)); 
				}
				Debug.Log("called setEquipped of runes = >> calling setUpdateDictionaty");
				isTransmuting = true;
				TransmuteButton.Text = "Finish Now";
//				SetUpdateData(transmutationDictionary);
				setEquippedTransmute(itemList);
			}
		}	
	}
	
	
	private void setEquippedTransmute(IList list)
	{
		//Debug.Log("Equipped count > " + list.Count);
		
		InventoryItem model = null;
		
		Array.ForEach<dfPanel>(EquippedPanel, aPanel => aPanel.GetComponent<DragTransmute>().Icon = "");
		
		for(int i = 0; i <list.Count; i++ )
		{
			IDictionary modelDictionary = list[i] as IDictionary;
				
			EquippedPanel[i].GetComponent<DragTransmute>().Icon = modelDictionary["RingName"].ToString();				
		}
		
	}
	
	
	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
//		Debug.Log("mouseEvent = "+mouseEvent.Source.name+" transmuteButton.text = "+TransmuteButton.Text+" condition"+TransmuteButton.Text.Equals("Finish Now"));
		if(!isTransmuting && mouseEvent.Source == TransmuteButton)
		{
			onTransmuteButton();
		}
		else if(TransmuteButton.Text.Equals("Finish Now") && mouseEvent.Source == TransmuteButton)
		{
//			Debug.Log("calling onFinishButton now");
			onFinishNowButton();
		}
	}

	public void OnDoubleClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		OnClick(control,mouseEvent);
	}
	
	private void onTransmuteButton()
	{
		List<string> stringList = new List<string>();
		String uidArray = "";
		int count = 0;
		foreach (dfPanel panel in EquippedPanel)
        {
			if(panel.GetComponent<DragTransmute>()._model != null && panel.GetComponent<DragTransmute>()._model.uid != null)
			{
				if(count++ == 0)
				{
					uidArray += panel.GetComponent<DragTransmute>()._model.uid;	
				}
				else 
				{
					uidArray += "," + panel.GetComponent<DragTransmute>()._model.uid;
				}
			}
		}
		
		if(count <= 1)
			return;
		
		isTransmuting = true;
		TransmuteButton.Text = "Finish Now";
		Debug.Log("onTransmute button called");
		_transmuteInterface.onStartTransmute(uidArray);
		
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial4)
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialEnd);
	}
	
	public void onFinishNowButton()
	{
		Debug.Log("OnFinishButton called");
		_transmuteInterface.onFinishButton();
		TransmuteButton.Text = "";
		
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorialEnd)
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted);
		
		Destroy(gameObject);
	}
	
	public void onTransmuteComplete()
	{
		isTransmuting = false;
		removeTransmuteItems();
		setInventoryData(inventory, transmutationDictionary);
	}
	
	public void setTransmutePrice(int transmutePrice)
	{
		TransmutePrice.Text = transmutePrice.ToString();
	}
	
	public void setTransmuteTime(int setTransmuteTime)
	{
		
	}
	
	public void swapTheTransmute(DragTransmute first, DragTransmute second)
	{
		Debug.Log(string.Format("First = {0}, Second = {1}", first, second.ItemName));
		
		
		if(first != null)
		{
			itemsList.RemoveAll(target => target._model.uid == first._model.uid);
		}

		itemsList.Add(second);
		
		IDictionary transmuteDictionary = null;
		
		List<InventorySystem.InventoryItem> inventoryItems = new List<InventorySystem.InventoryItem>();
		
		if(itemsList.Count > 1)
		{
			itemsList.ForEach(anItem => {
				inventoryItems.Add(GameManager._gameState.User._inventory.GetBagItemForUID(anItem._model.uid));
			});
			
			TransmuteButton.IsEnabled = true;
			transmuteDictionary = GameManager._gameState.User.Transmute(inventoryItems);
			
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial3)
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorial4);
		}
		else
		{
			transmuteDictionary = null;
			TransmuteButton.IsEnabled = false;
			
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.TransmutationTutorial2)
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.TransmutationTutorial3);
		}
		
		if(transmuteDictionary != null)
		{
			inventoryItems.ForEach(item => Debug.Log("ITEM > "+item.ToString()));
			setTransmutePrice(int.Parse(transmuteDictionary["gCost"].ToString()));
			TransmuteTime.Text = Upgradeui.ConvertSecToString(int.Parse(transmuteDictionary["Duration"].ToString()));
		}
		else
		{
			setTransmutePrice(0);
			TransmuteTime.Text = Upgradeui.ConvertSecToString(0);
		}
	}
	
	
		
//	// Added later while working on transmutation...
//	public void OnTransmutationItemsPlaced(string firstName, int firstLevel, string secondName, int secondLevel)
//	{
//		if(!string.IsNullOrEmpty(firstName))
//			itemsList.RemoveAll(pair => pair.Key == firstName && pair.Value == firstLevel);
//			
//		itemsList.Add(new KeyValuePair<string, int> (secondName, secondLevel));
//		
//		if(itemsList.Count > 1)
//		{
//			
//		}
//	}
	
	
	private void StartCountdown(int initialTimerValue)
	{
		Debug.Log(" initialTimerValue >> " + initialTimerValue+" isTransmuting => --  "+isTransmuting);
//		Debug.Log("setUpdate called of transmutation - time remaining = "+ tDictionary["TimeRemaining"]);

		if(!GameManager._gameState.User.transmutationDictionary.Contains("Type"))
		{
			TransmutePrice.Text = "00";
			TransmuteTime.Text = Upgradeui.ConvertSecToString(0);
			Array.ForEach<dfPanel>(EquippedPanel, aPanel => aPanel.GetComponent<DragTransmute>().Icon = "");
			return;
		}		
		
		if(initialTimerValue < 0)
		{
			TransmuteTime.Text = "?";
			return;
		}
		
		this.PerformActionWithDelay(1f, () => {
			TransmuteTime.Text = Upgradeui.ConvertSecToString(--initialTimerValue);
			if(initialTimerValue == 0)
				_transmuteInterface.OnTransmuteComplete();
			else if(initialTimerValue > 0)
				StartCountdown(initialTimerValue);
		});
	}
	
	
	public void SetUpdateData(IDictionary tDictionary)
	{
		StartCountdown(Convert.ToInt32(tDictionary["TimeRemaining"]));
		setTransmutePrice(Convert.ToInt32(tDictionary["BoostCost"]));
		Debug.Log("setUpdate called of transmutation - time remaining = "+ tDictionary["TimeRemaining"]);
	}

	public void transmuteStarted()
	{

	}
	
	private void setEquippedItems(Inventory inventory)
	{
		
	}
	
	public void removeTransmuteItems(bool isRing)
	{
		GameObject panel;
		if(isRing)
		{
			for(int i = 0;i < Carousel.transform.childCount; i++)
			{
				panel = Carousel.transform.GetChild(i).gameObject;
				if(!panel.GetComponent<DragTransmute>().isRing)
				{
					Destroy(panel);
				}
			}	
		}
		else 
		{
			for(int i = 0;i < Carousel.transform.childCount; i++)
			{
				panel = Carousel.transform.GetChild(i).gameObject;
				if(panel.GetComponent<DragTransmute>().isRing)
				{
					Destroy(panel);
				}			
			}
		}
	}

	public void removeTransmuteItems()
	{
		GameObject panel;
		for(int i = 0;i < Carousel.transform.childCount; i++)
		{
			panel = Carousel.transform.GetChild(i).gameObject;
			Destroy(panel);
		}	
	}
	
	public void OnDestroy()
	{
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
	}
}
