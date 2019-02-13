
using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using AnimationOrTween;
 

public class Upgradeui : MonoBehaviour
{
	
//	public dfPanel Bag,PotionBelt,KeyRing,TCube;
//	public int UpdatedTime;
//	public int UpdatedPrice;
//	
//	#region Left Upgrade panel area, where there are buttons for upgrading/finish and next upgrade sprite etc	
//	
//	public dfButton upgradeButton, finishButton;
//	public dfSprite gemIcon, dustIcon;
//	public dfLabel nextUpgradeNameLabel, nextUpgradeLevelLabel;
//	public dfSprite upgradeItemImage;
//	public dfLabel costLabel, timeLabel;
//	public dfSprite timeSprite;
//	public dfLabel progressBarLabel;
//	public dfSlicedSprite progressBarCurrentFill, progressBarNextFill;
//#endregion
//	
//	
//	#region Right panel area, where details of item currently selected in carousel are shown
//	
//	public dfLabel currentItemName, currentItemLevelLabel;
//#endregion
//	
//	public CarouselCoverFlowUpgrades coverFlow;
//	public item currentObject;
//	public UpgradeUiInterface _upgradeUiInterface;
//	
//	
//	public void setInterface(UpgradeUiInterface UpgradeInterface)
//	{
//		_upgradeUiInterface = UpgradeInterface;
//	}
//	
//	void Start()
//	{	
//		if(_upgradeUiInterface != null)
//			_upgradeUiInterface.OnRegisterSWFChildCallback(this);
//		
//		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
//		UIManager.instance.generalSwf.ToggleTopStats(true);
//		gameObject.GetComponent<dfPanel>().PerformLayout();
//	}
//	
//	
//	
//	private User user;
//	private IDictionary upgradesDictionary;
//	
//	
//	public void setInventory(User user, bool isUpdate)
//	{
//		this.PerformDFLayout();
//		
//		this.user = user;
//		upgradesDictionary = user._inventory.upgradesDictionary;
//			
//		PopulateSidePanel();
//		
//		Debug.Log("UpgradeUi setInventory called - current upgrade >> " + CurrentUpgrade);
//		
//		if(!isUpgrading)
//		{	
//			item carouselItem = coverFlow.transform.GetChild(coverFlow.selectedIndex).GetComponent<item>();
//			Debug.Log("NOT isUpgrading >> carouselItem >> " + carouselItem.ToString());
//			UpdateCarouselInfoPanel(carouselItem);
//			UpdateUpgradeInfoPanel(carouselItem);
//			currentObject = carouselItem;
//		}
//		else
//		{
//			IDictionary currentUpgradeInfo = upgradesDictionary[Inventory.TAG_UPGRADE_INFO] as IDictionary;
//			item upgradingItem = Array.Find<item>(coverFlow.transform.GetComponentsInChildren<item>(), (anItem) => anItem.upgradeID == CurrentUpgrade);
//			currentObject = upgradingItem;
//			Debug.Log("isUpgrading TRUE >> carouselItem >> " + upgradingItem.ToString());
//			UpdateUpgradeInfoPanel(upgradingItem);
//			
//			StartCountdown(int.Parse(currentUpgradeInfo["TimeRemaining"].ToString()));
//			costLabel.Text = currentUpgradeInfo["BoostCost"].ToString();
//		}
//		
//	}
//
//	public void OnCarouselItemScrolled(object sender, int valueIndex)
//	{
//		if(coverFlow.selectedIndex < 0 || coverFlow.selectedIndex > coverFlow.transform.childCount - 1)
//			return;
//		
//		if(coverFlow.transform.GetChild(coverFlow.selectedIndex) != null)
//		{
//			item selectedItem = coverFlow.transform.GetChild(coverFlow.selectedIndex).GetComponent<item>();
//			UpdateCarouselInfoPanel(selectedItem);
//			
//			if(!isUpgrading)
//			{
//				currentObject = selectedItem;
//				
//				Debug.Log("UpdateUpgradeInfoPanel >> currentObject >> " + currentObject.ToString());
//				
//				UpdateUpgradeInfoPanel(selectedItem);
//			}
//		}
//		
//	}
//	
//	private void UpdateCarouselInfoPanel(item anItem)
//	{
//		currentItemName.Text =  NameForItemFromID(anItem.upgradeID);
//		currentItemLevelLabel.Text = "Level " + anItem.upgradeLevel;
//	}
	
	#region ConvertTime
		public static string ConvertSecToString (float UTime)
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

	
		public static string ConvertTime (int Seconds)
		{
				string ToReturn = string.Empty;
				TimeSpan t = TimeSpan.FromSeconds (Seconds);
		
				bool Days = false;
				if (t.Days > 0) {
						if (t.Days < 10) {
								ToReturn = string.Format ("{0}d ", t.Days);
						} else {
								ToReturn = string.Format ("{0:D2}d ", t.Days);
						}
						Days = true;
				}
		
				if (t.Hours > 0) {
						if (t.Hours < 10) {
								ToReturn += string.Format ("{0}h ", t.Hours);
						} else {
								ToReturn += string.Format ("{0:D2}h ", t.Hours);
						}
				}
		
		
				if (t.Minutes > 0) {
						if (t.Minutes < 10) {
								ToReturn += string.Format ("{0}m ", t.Minutes);
						} else {
								ToReturn += string.Format ("{0:D2}m ", t.Minutes);
						}
				}
		
				if (t.Seconds > 0 && !Days) {
						if (t.Seconds < 10) {
								ToReturn += string.Format ("{0}s", t.Seconds);
						} else {
								ToReturn += string.Format ("{0:D2}s", t.Seconds);
						}
				}
				return ToReturn = ToReturn == string.Empty ? "0s" : ToReturn;
		}
	#endregion ConvertTime
	
//	private void UpdateUpgradeInfoPanel(item anItem)
//	{
//		if(anItem.upgradeLevel == anItem.MaxupgradeLevel)
//		{
//			finishButton.IsVisible = false;
//			upgradeButton.IsVisible = false;
//			costLabel.IsVisible = false;
//			timeSprite.IsVisible = false;
//			timeLabel.IsVisible = false;
//			timeSprite.IsVisible = false;
//			finishButton.IsVisible = false;
//			gemIcon.IsVisible = false;
//			dustIcon.IsVisible = false;
//			progressBarCurrentFill.FillAmount = 1;
//			progressBarNextFill.FillAmount = 1;
//			upgradeItemImage.SpriteName = anItem.upgradeID;
//			return;
//		}
//		
//		nextUpgradeNameLabel.Text = NameForItemFromID(anItem.NextupgradeID);
//		nextUpgradeLevelLabel.Text = "Level " + anItem.NextupgradeLevel;
//		upgradeItemImage.SpriteName = anItem.NextupgradeID;
//		costLabel.Text = anItem.NextupgradeCost + "";
//		
//		progressBarLabel.Text = anItem.upgradeCapacity + " + " + anItem.NextupgradeCapacity;
//		progressBarCurrentFill.FillAmount = anItem.upgradeCapacity / (float) anItem.MaxupgradeCapacity;
//		progressBarNextFill.FillAmount = anItem.NextupgradeCapacity / (float) anItem.MaxupgradeCapacity;
//		
//		
//		costLabel.IsVisible = true;
//		timeSprite.IsVisible = true;
//		timeLabel.IsVisible = true;
//		
//		if(isUpgrading)
//		{
//			finishButton.IsVisible = true;
//			upgradeButton.IsVisible = false;
//			dustIcon.IsVisible = false;
//			gemIcon.IsVisible = true;
//		}
//		else
//		{
//			finishButton.IsVisible = false;
//			upgradeButton.IsVisible = true;
//			
//			Debug.Log(" :::: UpdateUpgradeInfoPanel :::: Setting time val to >> anItem " + anItem.NextupgradeTime + "   ...... anItem >> " + anItem.upgradeID);
//			
//			timeLabel.Text = ConvertSecToString(anItem.NextupgradeTime);
//			if(anItem.upgradeID.ToLower().StartsWith("t")) // case for transmutation
//			{
//				dustIcon.IsVisible = false;
//				gemIcon.IsVisible = true;
//			}
//			else
//			{
//				dustIcon.IsVisible = true;
//				gemIcon.IsVisible = false;
//			}
//		}
//	}
//	
//	
//	private bool isUpgrading	{	get	{	return !string.IsNullOrEmpty(CurrentUpgrade);	}	}
//	
//	private string CurrentUpgrade
//	{
//		get
//		{
//			if(upgradesDictionary == null || upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kNone + "")
//			{
//				Debug.Log("CurrentUpgrade IS NULL");
//				
//				return null;
//			}
//			
//			IDictionary currentUpgradeDictionary = null;
//			
//			if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kBag + "")
//				currentUpgradeDictionary = upgradesDictionary[Inventory.TAG_CURRENT_BAG] as IDictionary;
//			else if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kPotionBelt + "")
//				currentUpgradeDictionary = upgradesDictionary[Inventory.TAG_CURRENT_POTION_BELT] as IDictionary;
//			else if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kTCube + "")
//				currentUpgradeDictionary = upgradesDictionary[Inventory.TAG_CURRENT_TCUBE] as IDictionary;
//			else
//				currentUpgradeDictionary = upgradesDictionary[Inventory.TAG_CURRENT_KEY_RING] as IDictionary;
//			
//			return currentUpgradeDictionary["id"].ToString();
//		}
//	}
//	
//	private string NextUpgrade
//	{
//		get
//		{
//			if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kNone + "")
//				return null;
//			
//			IDictionary nextUpgradeDictionary = null;
//			
//			if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kBag + "")
//				nextUpgradeDictionary = upgradesDictionary[Inventory.TAG_NEXT_BAG_UPGRADE] as IDictionary;
//			else if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kPotionBelt + "")
//				nextUpgradeDictionary = upgradesDictionary[Inventory.TAG_NEXT_KEY_POTION_BELT_UPGRADE] as IDictionary;
//			else if(upgradesDictionary[Inventory.TAG_CURRENT_UPGRADE].ToString() == Inventory.UpgradeType.kTCube + "")
//				nextUpgradeDictionary = upgradesDictionary[Inventory.TAG_NEXT_TCUBE_UPGRADE] as IDictionary;
//			else
//				nextUpgradeDictionary = upgradesDictionary[Inventory.TAG_NEXT_KEY_RING_UPGRADE] as IDictionary;
//			
//			return nextUpgradeDictionary["id"].ToString();
//		}
//	}
//	
//	private static string NameForItemFromID(string itemID)
//	{
//		if(itemID.ToLower().Contains("cube"))
//			return "Transmutation Cube";
//		else if(itemID.ToLower().Contains("belt"))
//			return "Potion Belt";
//		else if(itemID.ToLower().Contains("bag"))
//			return "Bottomless Bag";
//		else if(itemID.ToLower().Contains("ring"))
//			return "KEY RING";
//		return null;
//	}
//	
//	private void PopulateSidePanel()
//	{
//		item anItem = Bag.GetComponent<item>();
//		anItem.CurrentUpgrade(upgradesDictionary["CurrentBag"] as IDictionary);
//		anItem.NextUpgrade(upgradesDictionary["NextBagUpgrade"] as IDictionary);
//		anItem.MaxUpgrade(upgradesDictionary["MaxLevelBag"] as IDictionary);
//		
//		Debug.Log("ITEM >> " + anItem.ToString());
//		
//		anItem = PotionBelt.GetComponent<item>();
//		anItem.CurrentUpgrade(upgradesDictionary["CurrentPotionBelt"] as IDictionary);
//		anItem.NextUpgrade(upgradesDictionary["NextPotionBeltUpgrade"] as IDictionary);
//		anItem.MaxUpgrade(upgradesDictionary["MaxLevelPotionBelt"] as IDictionary);
//		
//		Debug.Log("ITEM >> " + anItem.ToString());
//		
//		anItem = KeyRing.GetComponent<item>();
//		anItem.CurrentUpgrade(upgradesDictionary["CurrentKeyRing"] as IDictionary);
//		anItem.NextUpgrade(upgradesDictionary["NextKeyRingUpgrade"] as IDictionary);
//		anItem.MaxUpgrade(upgradesDictionary["MaxLevelKeyRing"] as IDictionary);
//		
//		Debug.Log("ITEM >> " + anItem.ToString());
//		
//		anItem = TCube.GetComponent<item>();
//		anItem.CurrentUpgrade(upgradesDictionary["CurrentTCube"] as IDictionary);
//		anItem.NextUpgrade(upgradesDictionary["NextTCubeUpgrade"] as IDictionary);
//		anItem.MaxUpgrade(upgradesDictionary["MaxLevelTCube"] as IDictionary);
//		
//		Debug.Log("ITEM >> " + anItem.ToString());
//	}
//	
//	public void updateUpgradeData(string uDs)
//	{
//		string[] str;// = new string[2];
//		str = uDs.Split('|');
//		UpdatedTime = int.Parse(str[0]);
//		
//		if(str[1]==null)
//			UpdatedPrice = 0;
//		else
//			UpdatedPrice = int.Parse(str[1]);
//	}
//	
//	public void OnClick( dfControl sender, dfMouseEventArgs args )
//	{
//		if(args.Source == upgradeButton)
//		{
//			_upgradeUiInterface.onStartUpgrade(currentObject.upgradeID);
//			
//			if(isUpgrading)
//			{
//				setInventory(user, false);
//				IDictionary currentUpgradeInfo = upgradesDictionary[Inventory.TAG_UPGRADE_INFO] as IDictionary;
//				StartCountdown(int.Parse(currentUpgradeInfo["TimeRemaining"].ToString()));
//				costLabel.Text = currentUpgradeInfo["BoostCost"].ToString();
//			}
//		}
//		else if(args.Source == finishButton)
//		{
//			_upgradeUiInterface.onBoostButton();
//			if(!isUpgrading)
//				setInventory(user, false);
//		}
//	}
//	
//	private void StartCountdown(int initialTimerValue)
//	{
//		if(!isUpgrading || !keepCounter)
//			return;
//		
//		if(initialTimerValue < 0)
//		{
//			timeLabel.Text = "?";
//			return;
//		}
//		
//		Debug.Log("StartCountdown :::: Changing timeLabel text to >> " + (initialTimerValue -1 ));
//		
//		this.PerformActionWithDelay(1f, () => {
//			if(!isUpgrading || !keepCounter)
//				return;
//			timeLabel.Text = ConvertSecToString(--initialTimerValue);
//			if(initialTimerValue == 0)
//				_upgradeUiInterface.onReset();
//			else if(initialTimerValue > 0)
//				StartCountdown(initialTimerValue);
//		});
//	}
//	
//	
//	public void OnEnable()
//	{
//		coverFlow.SelectedIndexChanged += OnCarouselItemScrolled;
//	}
//	
//	private bool keepCounter = true;
//	public void OnDisable()
//	{
//		keepCounter = false;
//		coverFlow.SelectedIndexChanged -= OnCarouselItemScrolled;
//	}
//	
//	public void OnDestroy()
//	{
//		keepCounter = false;
//		InputWrapper.disableTouch = false;
//	}
//	
//	public void SetUpdateData(string stringTokens)
//	{
//		if(!isUpgrading)
//			return;
//		
//		Debug.Log(" SetUpdateData >> "  + stringTokens);
//			
//		string[] tokens = stringTokens.Split('|');
//		StartCountdown(int.Parse(tokens[0]));
//		costLabel.Text = tokens[1];
//	}
//	
//	private void Update()
//	{
//		InputWrapper.disableTouch = true;
//	}
	
}
