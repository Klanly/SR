using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using AnimationOrTween;


public class NGUpgradeui : MonoBehaviour
{
	
	public Transform Bag, PotionBelt, KeyRing, TCube;
	public int UpdatedTime;
	public int UpdatedPrice;
	
	#region Left Upgrade panel area, where there are buttons for upgrading/finish and next upgrade sprite etc	
	
	public UIButton upgradeButton, finishButton, maxedFrame;
	public UISprite gemIcon, dustIcon;
	public UILabel nextUpgradeNameLabel, nextUpgradeLevelLabel;
	public UISprite upgradeItemImage;
	public UILabel costLabel, timeLabel;
	public UISprite timeSprite;
	public UILabel progressBarLabel;
	public UISprite progressBarCurrentFill, progressBarNextFill;
	public UISprite glowSprite;
	#endregion
	
	
	#region Right panel area, where details of item currently selected in carousel are shown
	
	public UILabel currentItemName, currentItemLevelLabel;
	public UISprite bagSprite;
	public UISprite potionBeltSprite;
	public UISprite keyRingSprite;
	public UISprite transmutationCubeSprite;
	#endregion
	
//	public CarouselCoverFlowUpgrades coverFlow;
	public UICenterOnChild coverFlow;
	public item currentObject;
	public UpgradeUiInterface _upgradeUiInterface;
	
	
	public void setInterface (UpgradeUiInterface UpgradeInterface)
	{
		_upgradeUiInterface = UpgradeInterface;
	}
	
	void Start ()
	{	
		if (_upgradeUiInterface != null)
			_upgradeUiInterface.OnRegisterSWFChildCallback (this);
		
		UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
//		UIManager.instance.generalSwf.ToggleTopStats(true); //farhan

		coverFlow.onCenter = OnCenteredItem;

		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);
	}
	
	public void OnCenteredItem (GameObject selectedObject)
	{
		item carouselItem = selectedObject.GetComponent<item> ();
		if (!isUpgrading) {	
			Debug.Log ("NOT isUpgrading >> carouselItem >> " + carouselItem.ToString ());
			UpdateCarouselInfoPanel (carouselItem);
			UpdateUpgradeInfoPanel (carouselItem);
			currentObject = carouselItem;
		}

//		if(Bag.GetComponent<item>() == carouselItem) {
//			bagSprite.spriteName = carouselItem.upgradeID;
//		} else if(PotionBelt.GetComponent<item>() == carouselItem) {
//			potionBeltSprite.spriteName = carouselItem.upgradeID;
//		} else if(KeyRing.GetComponent<item>() == carouselItem) {
//			keyRingSprite.spriteName = carouselItem.upgradeID;
//		} else if(TCube.GetComponent<item>() == carouselItem) {
//			transmutationCubeSprite.spriteName = carouselItem.upgradeID;
//		}
		SoundManager.instance.PlayScrollSound();
	}

	
	private User user;
	private IDictionary upgradesDictionary;
	
	
	public void setInventory (User user, bool isUpdate)
	{
		this.user = user;
		upgradesDictionary = user._inventory.upgradesDictionary;

		PopulateSidePanel ();
		
//		Debug.Log ("UpgradeUi setInventory called - current upgrade >> " + CurrentUpgrade);
//		
		if (!isUpgrading) {	
			if (coverFlow.centeredObject != null) {
				item carouselItem = coverFlow.centeredObject.GetComponent<item> ();
				Debug.Log ("NOT isUpgrading >> carouselItem >> " + carouselItem.ToString ());
				UpdateCarouselInfoPanel (carouselItem);
				UpdateUpgradeInfoPanel (carouselItem);
				currentObject = carouselItem;
				NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Upgrade);
			}
		}
		if (isUpgrading) {
			IDictionary currentUpgradeInfo = upgradesDictionary [Inventory.TAG_UPGRADE_INFO] as IDictionary;
			item upgradingItem = Array.Find<item> (coverFlow.transform.GetComponentsInChildren<item> (), (anItem) => anItem.upgradeID == CurrentUpgrade);
			currentObject = upgradingItem;
//			Debug.Log ("isUpgrading TRUE >> carouselItem >> " + upgradingItem.ToString ());
			UpdateUpgradeInfoPanel (upgradingItem);
			UpdateCarouselInfoPanel (upgradingItem);

			StartCountdown (int.Parse (currentUpgradeInfo ["TimeRemaining"].ToString ()));
			costLabel.text = currentUpgradeInfo ["BoostCost"].ToString ();
//			Debug.LogError ("CostLabel: " + costLabel.text);

		}
	}

	public void GoToUpgrade(PurchaseManager.GeneralPopupType upgradeType) {
		this.PerformActionWithDelay(0.5f, () => {
//			Debug.LogWarning("child name - "+coverFlow.transform.GetChild(0).name);
			if(upgradeType == PurchaseManager.GeneralPopupType.BagUpgrade) {
				coverFlow.CenterOn(coverFlow.transform.GetChild(0));
			} else if(upgradeType == PurchaseManager.GeneralPopupType.BagUpgradeOrSell) {
				coverFlow.CenterOn(coverFlow.transform.GetChild(0));
			} else if(upgradeType == PurchaseManager.GeneralPopupType.PotionBeltUpgrade) {
				coverFlow.CenterOn(coverFlow.transform.GetChild(1));
			} else if(upgradeType == PurchaseManager.GeneralPopupType.KeyRingUpgrade) {
				coverFlow.CenterOn(coverFlow.transform.GetChild(2));
			}
		}, null);
	}

	public void OnCarouselItemScrolled ()
	{
//		if(coverFlow.selectedIndex < 0 || coverFlow.selectedIndex > coverFlow.transform.childCount - 1)
//			return;
		
//		if(coverFlow.transform.GetChild(coverFlow.selectedIndex) != null)
//		{

		item selectedItem = coverFlow.centeredObject.GetComponent<item> ();
//			item selectedItem = coverFlow.transform.GetChild(coverFlow.selectedIndex).GetComponent<item>();
		UpdateCarouselInfoPanel (selectedItem);
			
		if (!isUpgrading) {
			currentObject = selectedItem;
				
			Debug.Log ("UpdateUpgradeInfoPanel >> currentObject >> " + currentObject.ToString ());
				
			UpdateUpgradeInfoPanel (selectedItem);
		}
//		}
		
	}
	
	private void UpdateCarouselInfoPanel (item anItem)
	{
		currentItemName.text = NameForItemFromID (anItem.upgradeID);
		currentItemLevelLabel.text = "Level " + anItem.upgradeLevel;
	}
	
	public static string ConvertSecToString (int UTime)
	{
		float hours = 0;
		hours = Mathf.Floor (UTime / 3600);
		float minutes = Mathf.Floor ((UTime - (hours * 3600)) / 60);
		float seconds = Mathf.Floor (UTime - (hours * 3600) - (minutes * 60));
		
		if (hours > 0)
			return hours + "h " + minutes + "m ";
		else
			return minutes + "m " + seconds + "s"; 
	}
	
	private void UpdateUpgradeInfoPanel (item anItem)
	{
//		Debug.Log ("anItem.upgradeLevel > " + anItem.upgradeLevel + "  anItem.MaxupgradeLevel " + anItem.MaxupgradeLevel);
		if(Bag.GetComponent<item>() == anItem) {
			bagSprite.spriteName = anItem.upgradeID;
		} else if(PotionBelt.GetComponent<item>() == anItem) {
			potionBeltSprite.spriteName = anItem.upgradeID;
		} else if(KeyRing.GetComponent<item>() == anItem) {
			keyRingSprite.spriteName = anItem.upgradeID;
		} else if(TCube.GetComponent<item>() == anItem) {
			transmutationCubeSprite.spriteName = anItem.upgradeID;
		}

		if (anItem.upgradeLevel == anItem.MaxupgradeLevel) {
			maxedFrame.gameObject.SetActive (true);
			finishButton.gameObject.SetActive (false);
			upgradeButton.gameObject.SetActive (false);
			costLabel.gameObject.SetActive (false);
			timeSprite.gameObject.SetActive (false);
			timeLabel.gameObject.SetActive (false);
			timeSprite.gameObject.SetActive (false);
			finishButton.gameObject.SetActive (false);
			gemIcon.gameObject.SetActive (false);
			dustIcon.gameObject.SetActive (false);
			progressBarCurrentFill.fillAmount = 1;
			progressBarNextFill.fillAmount = 1;
			progressBarLabel.text = anItem.upgradeCapacity.ToString();
			upgradeItemImage.spriteName = anItem.upgradeID;
			nextUpgradeNameLabel.text = NameForItemFromID (anItem.upgradeID);
			nextUpgradeLevelLabel.text = "Level " + anItem.upgradeLevel;
			TweenAlpha tweenAlpha = glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = false;
			NGUITools.SetActive( glowSprite.gameObject, false);
			return;
		}
		maxedFrame.gameObject.SetActive (false);
		nextUpgradeNameLabel.text = NameForItemFromID (anItem.NextupgradeID);
		nextUpgradeLevelLabel.text = "Level " + anItem.NextupgradeLevel;
		upgradeItemImage.spriteName = anItem.NextupgradeID;
		costLabel.text = anItem.NextupgradeCost + "";
//		Debug.LogError ("CostLabel: " + costLabel.text);

//		Debug.Log ("anItem.NextupgradeID > " + anItem.NextupgradeID + "     anItem.upgradeID >> " + anItem.upgradeID);

		progressBarLabel.text = anItem.upgradeCapacity + " + " + (anItem.NextupgradeCapacity-anItem.upgradeCapacity);
		progressBarCurrentFill.fillAmount = anItem.upgradeCapacity / (float)anItem.MaxupgradeCapacity;
		progressBarNextFill.fillAmount = anItem.NextupgradeCapacity / (float)anItem.MaxupgradeCapacity;
		
		
		costLabel.gameObject.SetActive (true);
		timeSprite.gameObject.SetActive (true);
		timeLabel.gameObject.SetActive (true);
		
		if (isUpgrading) {
			finishButton.gameObject.SetActive (true);
			upgradeButton.gameObject.SetActive (false);
			dustIcon.gameObject.SetActive (false);
			gemIcon.gameObject.SetActive (true);
			NGUITools.SetActive( glowSprite.gameObject, true);
			TweenAlpha tweenAlpha = glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = true;
		} else {
			finishButton.gameObject.SetActive (false);
			upgradeButton.gameObject.SetActive (true);

			TweenAlpha tweenAlpha = glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = false;
			NGUITools.SetActive( glowSprite.gameObject, false);

			Debug.Log (" :::: UpdateUpgradeInfoPanel :::: Setting time val to >> anItem " + anItem.NextupgradeTime + "   ...... anItem >> " + anItem.upgradeID);
			
			timeLabel.text = ConvertSecToString (anItem.NextupgradeTime);
			if (anItem.upgradeID.ToLower ().StartsWith ("t")) { // case for transmutation
				dustIcon.gameObject.SetActive (false);
				gemIcon.gameObject.SetActive (true);
			} else {
				dustIcon.gameObject.SetActive (true);
				gemIcon.gameObject.SetActive (false);
			}
		}
	}
	
	
	private bool isUpgrading	{ get { return !string.IsNullOrEmpty (CurrentUpgrade); } }
	
	private string CurrentUpgrade {
		get {
			if (upgradesDictionary == null || upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kNone + "") {
				Debug.Log ("CurrentUpgrade IS NULL");
				
				return null;
			}
			
			IDictionary currentUpgradeDictionary = null;
			
			if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kBag + "")
				currentUpgradeDictionary = upgradesDictionary [Inventory.TAG_CURRENT_BAG] as IDictionary;
			else if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kPotionBelt + "")
				currentUpgradeDictionary = upgradesDictionary [Inventory.TAG_CURRENT_POTION_BELT] as IDictionary;
			else if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kTCube + "")
				currentUpgradeDictionary = upgradesDictionary [Inventory.TAG_CURRENT_TCUBE] as IDictionary;
			else
				currentUpgradeDictionary = upgradesDictionary [Inventory.TAG_CURRENT_KEY_RING] as IDictionary;
			
			return currentUpgradeDictionary ["id"].ToString ();
		}
	}
	
	private string NextUpgrade {
		get {
			if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kNone + "")
				return null;
			
			IDictionary nextUpgradeDictionary = null;
			
			if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kBag + "")
				nextUpgradeDictionary = upgradesDictionary [Inventory.TAG_NEXT_BAG_UPGRADE] as IDictionary;
			else if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kPotionBelt + "")
				nextUpgradeDictionary = upgradesDictionary [Inventory.TAG_NEXT_KEY_POTION_BELT_UPGRADE] as IDictionary;
			else if (upgradesDictionary [Inventory.TAG_CURRENT_UPGRADE].ToString () == Inventory.UpgradeType.kTCube + "")
				nextUpgradeDictionary = upgradesDictionary [Inventory.TAG_NEXT_TCUBE_UPGRADE] as IDictionary;
			else
				nextUpgradeDictionary = upgradesDictionary [Inventory.TAG_NEXT_KEY_RING_UPGRADE] as IDictionary;
			
			return nextUpgradeDictionary ["id"].ToString ();
		}
	}
	
	private static string NameForItemFromID (string itemID)
	{
		if (itemID.ToLower ().Contains ("cube"))
			return "Transmutation Cube";
		else if (itemID.ToLower ().Contains ("belt"))
			return "Potion Belt";
		else if (itemID.ToLower ().Contains ("bag"))
			return "Bottomless Bag";
		else if (itemID.ToLower ().Contains ("ring"))
			return "KEY RING";
		return null;
	}
	
	private void PopulateSidePanel ()
	{
		item anItem = Bag.GetComponent<item> ();
		anItem.CurrentUpgrade (upgradesDictionary ["CurrentBag"] as IDictionary);
		anItem.NextUpgrade (upgradesDictionary ["NextBagUpgrade"] as IDictionary);
		anItem.MaxUpgrade (upgradesDictionary ["MaxLevelBag"] as IDictionary);
		
//		Debug.Log ("ITEM >> " + anItem.ToString ());
		
		anItem = PotionBelt.GetComponent<item> ();
		anItem.CurrentUpgrade (upgradesDictionary ["CurrentPotionBelt"] as IDictionary);
		anItem.NextUpgrade (upgradesDictionary ["NextPotionBeltUpgrade"] as IDictionary);
		anItem.MaxUpgrade (upgradesDictionary ["MaxLevelPotionBelt"] as IDictionary);
		
//		Debug.Log ("ITEM >> " + anItem.ToString ());
		
		anItem = KeyRing.GetComponent<item> ();
		anItem.CurrentUpgrade (upgradesDictionary ["CurrentKeyRing"] as IDictionary);
		anItem.NextUpgrade (upgradesDictionary ["NextKeyRingUpgrade"] as IDictionary);
		anItem.MaxUpgrade (upgradesDictionary ["MaxLevelKeyRing"] as IDictionary);
		
//		Debug.Log ("ITEM >> " + anItem.ToString ());
		
		anItem = TCube.GetComponent<item> ();
		anItem.CurrentUpgrade (upgradesDictionary ["CurrentTCube"] as IDictionary);
		anItem.NextUpgrade (upgradesDictionary ["NextTCubeUpgrade"] as IDictionary);
		anItem.MaxUpgrade (upgradesDictionary ["MaxLevelTCube"] as IDictionary);
		
//		Debug.Log ("ITEM >> " + anItem.ToString ());
	}
	
	public void updateUpgradeData (string uDs)
	{
		string[] str;// = new string[2];
		str = uDs.Split ('|');
		UpdatedTime = int.Parse (str [0]);
		
		if (str [1] == null)
			UpdatedPrice = 0;
		else
			UpdatedPrice = int.Parse (str [1]);
	}
	
	public void OnClick (UIButton button)
	{
		if (button == upgradeButton) {
			_upgradeUiInterface.onStartUpgrade (currentObject.upgradeID);
			
			if (isUpgrading) {
				setInventory (user, false);
				IDictionary currentUpgradeInfo = upgradesDictionary [Inventory.TAG_UPGRADE_INFO] as IDictionary;
				StartCountdown (int.Parse (currentUpgradeInfo ["TimeRemaining"].ToString ()));
				costLabel.text = currentUpgradeInfo ["BoostCost"].ToString ();
				Debug.LogError ("CostLabel: " + costLabel.text);

			}
		} else if (button == finishButton) {
			_upgradeUiInterface.onBoostButton ();
			if (!isUpgrading) {
				setInventory (user, false);
				NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Upgrade);
			}
		}
	}
	
	private void StartCountdown (int initialTimerValue)
	{
//		Debug.LogError("initialTimer = "+initialTimerValue+" isUpgrading - "+isUpgrading+" keepCOunter = "+keepCounter);
		if (!isUpgrading || !keepCounter)
			return;
		
		if (initialTimerValue < 0) {
			timeLabel.text = "?";
			return;
		}

//		if (initialTimerValue % 5 == 0)
//			Debug.Log ("StartCountdown :::: Changing timeLabel text to >> " + (initialTimerValue - 1));
		
		this.PerformActionWithDelay (1f, () => {
			if (!isUpgrading || !keepCounter)
				return;
//			timeLabel.text = ConvertSecToString (--initialTimerValue);
			timeLabel.text = ConvertSecToString (initialTimerValue);
			if(initialTimerValue < 5) {
				NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Upgrade);
			}
			if (initialTimerValue == 0) {
				Debug.LogError("initialTimer = "+initialTimerValue);
				_upgradeUiInterface.onReset ();
				if(!isUpgrading)
					setInventory (user, false);
			} else if (initialTimerValue > 0)
				StartCountdown (--initialTimerValue);
		});
	}
	

	public void ShowUpgradeCompletePopup (string itemId, string itemName, string itemLevel)
	{
		var coverflowTransform = coverFlow.transform;
		foreach(Transform child in coverflowTransform) {
			item item = child.GetComponent<item>();
			Debug.LogWarning("model name - "+item.GetItemName()+" itemnName - "+itemName);
			if(item.GetItemName().Equals(itemName)) {
				this.PerformActionWithDelay(0.5f, () => {
					coverFlow.CenterOn(child);
				}, null);
				break;
			}
		}
		UnityEngine.Object asset = Resources.Load ("UIPrefabs/NGUI/UpgradeCompletePopup");
		NGUpgradeComplete popup = NGUITools.AddChild (this.gameObject, asset as GameObject).GetComponent<NGUpgradeComplete> ();
		popup.Show (itemId, itemName, itemLevel);
	}

	public void OnEnable ()
	{
//		coverFlow.SelectedIndexChanged += OnCarouselItemScrolled; //farhan
//		coverFlow.onCenter += OnCarouselItemScrolled;
	}
	
	private bool keepCounter = true;
	public void OnDisable ()
	{
		keepCounter = false;
//		coverFlow.SelectedIndexChanged -= OnCarouselItemScrolled;
//		coverFlow.onDragFinished -= OnCarouselItemScrolled;
	}
	
	public void OnDestroy ()
	{
		keepCounter = false;
		InputWrapper.disableTouch = false;
	}
	
	public void SetUpdateData (string stringTokens)
	{
//		Debug.Log ("SetUpdateData > " + stringTokens + "   isUpgrading > " + isUpgrading);
		if (!isUpgrading)
			return;
		
		Debug.Log (" SetUpdateData >> " + stringTokens);
		
		string[] tokens = stringTokens.Split ('|');
		int time = int.Parse (tokens [0]);
		Debug.LogWarning("TIme remaining for upgrade "+ time);
//		IDictionary currentUpgradeInfo = upgradesDictionary [Inventory.TAG_UPGRADE_INFO] as IDictionary;
//		if(time < 1) {
////			int endTime = int.Parse (currentUpgradeInfo ["EndTime"].ToString ());
//			DateTime nowDatetime = DateTime.Now.ToUniversalTime();
//			DateTime endDatetime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(int.Parse(currentUpgradeInfo["TimeRemaining"]));
//			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//			time = (int)((endDatetime - nowDatetime).TotalSeconds);
////			Debug.LogError(time + " end "+endTime+" and end = "+(int)(endDatetime - epoch).TotalSeconds + " now = "+(int)(nowDatetime - endDatetime).TotalSeconds);
////			Debug.LogError(time + " end "+endTime+" and end = "+(int)(endDatetime - nowDatetime).TotalSeconds + " now = "+(int)(nowDatetime - endDatetime).TotalSeconds);
//			if(time < 0) {
//				time = -1;
//			}
//
//		}
//		DateTime x = new DateTime(1472669059);
//		Debug.LogError(x.ToString("HH mm ss"));
//		foreach(string key in currentUpgradeInfo.Keys) {
//			Debug.LogError("keys - "+key+" and value - "+currentUpgradeInfo[key]);
//		}


		Debug.LogError("Time remaining after datetime - "+time);
		StartCountdown (time);
		costLabel.text = tokens [1];
		if(time > 0) {
			DateTime dateTime = DateTime.Now.AddSeconds(time);
			NotificationManager.SendLocalNotification(dateTime, NotificationManager.NotificationType.Upgrade);
		}
		
		if (isUpgrading) {
			finishButton.gameObject.SetActive (true);
			upgradeButton.gameObject.SetActive (false);
			dustIcon.gameObject.SetActive (false);
			gemIcon.gameObject.SetActive (true);

			NGUITools.SetActive( glowSprite.gameObject, true);
			TweenAlpha tweenAlpha = glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = true;

		} else {
			TweenAlpha tweenAlpha = glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = false;
			NGUITools.SetActive( glowSprite.gameObject, false);
		}
	}
	
	private void Update ()
	{
		InputWrapper.disableTouch = true;
	}
	
}
