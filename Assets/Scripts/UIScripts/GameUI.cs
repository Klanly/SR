using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
 
public partial class GameUI : MonoBehaviour {
	
//
//	public GameManager.GeneralSwfLoaded gSwfDelegate = null;
//	
//	public void Start()
//	{
//		if(gSwfDelegate!=null)
//			gSwfDelegate(true);
//
//		if(_topPanel != null)
//			_topPanel.SetListener(_listenerInterface);
//
//		gameObject.GetComponent<dfPanel>().PerformLayout();
//	}
//	
//	GameUIInterface _listenerInterface;
//	public void SetListener(GameUIInterface listenerInterface)
//	{
//		_listenerInterface = listenerInterface;
//		if(_topPanel != null)
//			_topPanel.SetListener(_listenerInterface);
//	}
//	
//	
//	#region shrine related calls...
//
//	private ShrinePopup _shrinePopupScript;
//
//	public void showShrinePopup(string element,int shrineLevel,int chargePoints,int maxChargePoints,long timmer,bool chargeButtonState)
//	{
//		if(_shrinePopupScript == null)
//		{
//			_shrinePopupScript = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_ShrinePopupPath) as GameObject).GetComponent<ShrinePopup>();
//			_shrinePopupScript.Init(element,shrineLevel,chargePoints,maxChargePoints,timmer,chargeButtonState, _listenerInterface.ChargeShrine, _listenerInterface.CollectShrineReward);
//		}
//	}
//
//	public void setShrineTimmer(long timmer)
//	{
//		if(_shrinePopupScript != null)
//		{
//			_shrinePopupScript.SetShrineTimer(timmer);
//		}
//	}
//
//	public void setShrineLevel(int shrineLevel)
//	{
//		if(_shrinePopupScript != null)
//		{
//			_shrinePopupScript.SetShrineLevel(shrineLevel);
//		}
//	}
//	
//	public void CloseShrine()
//	{
//		if(_shrinePopupScript != null)
//		{
//			Destroy(_shrinePopupScript.gameObject);
//			_shrinePopupScript = null;
//		}
//	}
//	
//	public void toggleChargeShrine(bool isCharged)
//	{
//		if(_shrinePopupScript != null)
//		{
//			
//		}
//	}
//
//	public void ShrineConnect()
//	{
//		Debug.Log("SHOW :::: ShrineConnect ::::");
//	}
//	
//	public void ShrineInvite(int points, int friendCount, string friendsList)
//	{
//		
//	}
//	
//	public void shrineRingChargeTutorial()
//	{
//		
//	}
//	
//	public void shrineRingChargeTutorialOff()
//	{
//		
//	}
//
//	public void setBarFill(int currentVal, int maxVal) //Shring bar fill!
//	{
//		if(_shrinePopupScript != null)
//		{
//			_shrinePopupScript.SetBarFill(currentVal, maxVal);
//		}
//	}
//
//	#endregion
//	
//	
//	public void ShowSignInPopup()
//	{
//		
//	}
//	
//	
//	public void PlayBattleTutorial()
//	{
//
//	}
//
//	public void showNoConnectivityPopup(string msg)
//	{
//
//	}
//
//	public void setInventoryData(string jsonData)
//	{
//
//	}
//
//	public void gameUserAndVersion(string name, string version)
//	{
//
//	}
//
//	#region Top Panel related ...
//	
//	public GameUITopPanel _topPanel;
//	
//	public void updateHealth(int health)
//	{
//		_topPanel.UpdateHealth(health);
//	}
//
//	public void updateHealth(int health, bool playHeartAnimation)
//	{
//		_topPanel.UpdateHealth(health, playHeartAnimation);
//	}
//
//	public void updateSoulGems(int soulGems)
//	{
//		_topPanel.UpdateSoulGems(soulGems);
//	}
//
//	public void updateSoulDust(float soulDust)
//	{
//		_topPanel.UpdateSoulDust(soulDust);
//	}
//	
//	#endregion
//
//	public void showLootPopup(int itemDamage, int itemWards)
//	{
//
//	}
//
//	#region Keys Popup....
//
//	private KeysPopup _keysPopupScript;
//
//	public void showKeysPopup(int availableKeys, float price, int requiredKeys)
//	{
//		Debug.Log("showKeysPopup");
//		if(_keysPopupScript == null)
//		{
//			_keysPopupScript = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_BuyKeysPotionsPopupPath) as GameObject).GetComponent<KeysPopup>();
//			_keysPopupScript.init(availableKeys,price,requiredKeys);
//		}
//	}
//
//	public void showKeysPopup(int availableKeys, float price)
//	{
//		Debug.Log("showKeysPopup 2 param...");
//		
//		if(_keysPopupScript == null)
//		{
//			_keysPopupScript = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_BuyKeysPotionsPopupPath) as GameObject).GetComponent<KeysPopup>();
//			_keysPopupScript.init(availableKeys,price);
//		}
//	}
//
//	#endregion
//
//	public void showUiGeneralPopup(string heading, string message)
//	{ 
//		GeneralPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_generalPopupPath) as GameObject).GetComponent<GeneralPopup>();
//		popup.showPopup(heading, message, _listenerInterface.GeneralPopupButton1Pressed); //TO-DO... Pass listener to popup tap
//	}
//	
//	public void showUiGeneralPopup(string heading, string message, Action cb)
//	{ 
//		GeneralPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_generalPopupPath) as GameObject).GetComponent<GeneralPopup>();
//		popup.showPopup(heading, message, cb); //TO-DO... Pass listener to popup tap
//	}
//
//	public void showGeneralPopup2(string title, string message, string btn1Text, string btn2Text)
//	{
//		TwoButtonPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_TwoButtonPopupPath) as GameObject).GetComponent<TwoButtonPopup>();
//		popup.showGeneralPopup2(title, message, btn1Text, _listenerInterface.GeneralPopupButton1Pressed,  btn2Text, _listenerInterface.GeneralPopupButton2Pressed);
//	}
//
//	public void showGeneralPopup2(string title, int numberVal, string message, string btn1Text, string btn2Text)
//	{
//		TwoButtonPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_TwoButtonPopupPath) as GameObject).GetComponent<TwoButtonPopup>();
//		popup.showGeneralPopup2(title, message, numberVal, btn1Text, _listenerInterface.GeneralPopupButton1Pressed,  btn2Text, _listenerInterface.GeneralPopupButton2Pressed);
//	}
//
//	public void showGeneralPopup3(string title, string message, string btn1Text, string btn2Text)
//	{
//		TwoButtonPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_TwoButtonPopupPath) as GameObject).GetComponent<TwoButtonPopup>();
//		popup.showGeneralPopup2(title, message, btn1Text, _listenerInterface.GeneralPopup3Button1Pressed,  btn2Text, _listenerInterface.GeneralPopup3Button2Pressed);
//	}
//
//	public void userloginState(bool facebook, bool gamecenter, bool googleplus)
//	{
//
//	}
//
//	public void OpenMarketUi()
//	{
//
//	}
//	
//	public TransmuteUi OpenTransmuteUi()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//		
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_TransmuteUiPath) as GameObject).GetComponent<TransmuteUi>();
//	}
//	
//	public Upgradeui OpenUpgradeUi()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//		
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_UpgradeUiPath) as GameObject).GetComponent<Upgradeui>();
//	}
//
//	public RingUi OpenRingsUi()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//		
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_ringUIPath) as GameObject).GetComponent<RingUi>();
//	}
//	
//	public RuneUi OpenStaffUi()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//		
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_RuneUiPath) as GameObject).GetComponent<RuneUi>();
//	}
//	
//
//	public void OpenPetUi()
//	{
//		
//	}
//
//	public SpiritsUI OpenSpiritsUI()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_SpiritsMenuPath) as GameObject).GetComponent<SpiritsUI>();
//	}
//
//	public OptionsMenu OpenOptionsUI()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_OptionsMenuPath) as GameObject).GetComponent<OptionsMenu>();
//	}
//
//	public void showGeneralPopup(string heading, string message)
//	{
//		showUiGeneralPopup(heading, message);
//	}
//
//	public void showVictoryPopup(int soulsWon, int healthLost, int battleTime, int topDamage, string grade)
//	{
//		VictoryPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_VictoryPopupPath) as GameObject).GetComponent<VictoryPopup>();
//		popup.showPopup(soulsWon, healthLost, battleTime, topDamage, grade, _listenerInterface.onVictoryContinue); //Add callback if any!
//	}
//
//	public void showLootRingPopup(InventorySystem.ItemRing ring, bool isTransmute)
//	{
//		RingsLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupRingsPath) as GameObject).GetComponent<RingsLootPopup>();
//		popup.showLootRingPopup(ring, isTransmute, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showLootRingPopup(InventorySystem.ItemRing ring)
//	{
//		RingsLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupRingsPath) as GameObject).GetComponent<RingsLootPopup>();
//		popup.showLootRingPopup(ring, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showLootRunePopup(InventorySystem.ItemRune rune, bool isTransmute)
//	{
//		RunesLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupRunesPath) as GameObject).GetComponent<RunesLootPopup>();
//		popup.showLootRunePopup(rune, isTransmute, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showLootRunePopup(InventorySystem.ItemRune rune)
//	{
//		RunesLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupRunesPath) as GameObject).GetComponent<RunesLootPopup>();
//		popup.showLootRunePopup(rune, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showLootKeysPopup(int keys)
//	{
//		KeysLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupKeysPath) as GameObject).GetComponent<KeysLootPopup>();
//		popup.showLootKeysPopup(keys, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showLootGemsPopup(int gems)
//	{
//		SoulBagLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupGemsPath) as GameObject).GetComponent<SoulBagLootPopup>();
//		popup.showLootKeysPopup(gems, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showSoulPopup(int soulValue)
//	{
//		SoulBagLootPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_lootPopupSoulBagPath) as GameObject).GetComponent<SoulBagLootPopup>();
//		popup.showLootKeysPopup(soulValue, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showShardPopup(int shardCount)
//	{
//		RingShardPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_shardPopupPath) as GameObject).GetComponent<RingShardPopup>();
//		popup.Show(shardCount, _listenerInterface.lootPopupClosed); //Add callback if any!
//	}
//
//	public void showDefeatPopup()
//	{
//		DefeatPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_defeatPopupPath) as GameObject).GetComponent<DefeatPopup>();
//		popup.SetCallbacks(_listenerInterface.onDefeatReturn, _listenerInterface.onDefeatRestart);
//	}
//
//	public void updateSoulDust(int soulGems)
//	{
//
//	}
//
//	public void updateDamage(int totalDamage)
//	{
//
//	}
//
//	public void showBossDefeatPopup(string heading, string text)
//	{
//		BossDefeatPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_bossDefeatPopupPath) as GameObject).GetComponent<BossDefeatPopup>();
//		popup.SetCallbacks(_listenerInterface.bossDefeatPopupClosed);
//	}
//
//	public void showUnLockPopup(UnlockedUpgradesPopup.UnlockedUpgradesType popupType)
//	{
//		UnlockedUpgradesPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_UnlockPopupPath) as GameObject).GetComponent<UnlockedUpgradesPopup>();
//		popup.showUnlockedUpgrade(popupType, null); //farhan
//	}
//
//	public void setLanguage(string languageString, string charSetID)
//	{
//
//	}
//
//	public void setInventtoryData(string jsonString, string suggestedJsonString)
//	{
//
//	}
//
//	public void loadSwf(string filename)
//	{
//
//	}
//
//	public void showPotionPopup(int count, int price)
//	{
//		PotionPopup popup = GetComponent<dfPanel>().AddPrefab(Resources.Load("PotionPopup") as GameObject).GetComponent<PotionPopup>();
//		popup.Show(count, price, _listenerInterface.onUseHealthPotion, _listenerInterface.onPotionBuyButton, _listenerInterface.onCloseHealthPopup);
//	}
//
//	public void unloadSwf()
//	{
//
//	}
//
//	public void SfxVolume(double volume)
//	{
//
//	}
//
//	public void setMVolume(double volume)
//	{
//
//	}
//
//	public void onBuyRingComplete(string response)
//	{
//
//	}
//
//	public void onSellRingComplete()
//	{
//
//	}
//
//	public void onBuyRuneComplete(string response)
//	{
//
//	}
//
//	public void onSellRuneComplete()
//	{
//
//	}
//	
//	public void ShowHeartIcon()
//	{
//		_topPanel.ShowHeartIcon();
//	}
//	
//	public void HideHeartIcon()
//	{
//		_topPanel.HideHeartIcon();
//	}
//	
//	public void showLootPotionPopup(int val)
//	{
//
//	}
//
//	private void showBattleMenu(int dayCount)
//	{
//		gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(battlePauseMenuPrefabPath) as GameObject);
//	}
//	
//	PauseMenu menu;
//	private void showInGameMenu(int dayCount)
//	{
//		if(menu != null)
//		{
//			DestroyImmediate(menu.gameObject);
//			menu = null;
//		}
//		menu = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_pauseMenuPrefabPath) as GameObject).GetComponent<PauseMenu>();
//		menu.SetListener(_listenerInterface);
//	}
//	
//	#region Loading screen related calls...
//	
//	private LoadingScreen _loadingScreenScript;
//	
//	public void onLoadingScreen() //Show loading screen... 'wtf' naming conventions :/
//	{
//		if(_loadingScreenScript == null)
//		{
//			_loadingScreenScript = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_LoadingScreenPrefabPath) as GameObject).GetComponent<LoadingScreen>();
//		}
//		else
//		{
//			_loadingScreenScript.SetDefaultValues();
//		}
//	}
//
//	public void onUnLoadingScreen() //Hide loading screen... 'wtf' naming conventions :/
//	{
//		if(_loadingScreenScript != null)
//		{
//			Destroy(_loadingScreenScript.gameObject);
//			_loadingScreenScript = null;
//		}
//	}
//
//	public void LoadingScreenShowBar(string title) //Set loading bar text
//	{
////		Debug.Log("LoadingScreenShowBar" + title);
//		if(_loadingScreenScript == null) 
//		{
//			Debug.Log("_loadingScreenScript NULL at showTextOnBar :(");
//			return;
//		}
//		
//		_loadingScreenScript.ShowLoadingScreenBarText(title);
//	}
//
//	public void LoadingScreenHideBar()
//	{
//		Debug.Log("LoadingScreenHideBar");
//		if(_loadingScreenScript == null) 
//			return;
//		
//		_loadingScreenScript.HideLoadingBar();
//	}
//
//	public void LoadingPercentage(int perc)
//	{
//		Debug.Log("LoadingPercentage value = " + perc);
//		if(_loadingScreenScript == null) 
//		{
//			Debug.Log("_loadingScreenScript NULL");
//			return;
//		}
//		
//		_loadingScreenScript.SetLoadingPercentage(perc);
//	}
//	
//	public void OnGUI()
//	{
//		/*
//		if (GUI.Button(new Rect(10, 50, 150, 30), "Show Loading Screen"))
//			onLoadingScreen();
//		
//		if (GUI.Button(new Rect(10, 90, 150, 30), "Hide Loading Screen"))
//            onUnLoadingScreen();
//		
//		if (GUI.Button(new Rect(10, 130, 150, 30), "LoadingScreen Show bar"))
//            LoadingScreenShowBar("Sup idiot!");
//		
//		if (GUI.Button(new Rect(10, 170, 150, 30), "Ring ui"))
//            loadRingUi();
//		
//		if (GUI.Button(new Rect(10, 210, 150, 30), "Ring ui"))
//		{
//			showGeneralPopup2("My Title", "my message", "OKAY", "CANCEL");
//		}
//		
//		if (GUI.Button(new Rect(10, 240, 150, 30), "InputWrapper enable touch"))
//			InputWrapper.disableTouch = false;
//		*/
//	}
//
//	#endregion
//	/*
//	public void loadRingUi()
//	{
//		//gameObject.GetComponent<dfPanel>().AddPrefab(_ringUI.gameObject);
//	}
//	
//	public void loadUpgradeUi()
//	{
//		gameObject.GetComponent<dfPanel>().AddPrefab(_UpgradeUi.gameObject);
//	}
//	
//	public void loadRuneUi()
//	{
//		gameObject.GetComponent<dfPanel>().AddPrefab(_RuneUi.gameObject);
//	}
//	
//	public void loadTransmuteUi()
//	{
//		gameObject.GetComponent<dfPanel>().AddPrefab(_TransmuteUi.gameObject);
//	}
//	*/
//	
//	
//	private GameObject _loadingPopupObject;
//	public void showUiLoadingPopup()
//	{
//		if(_loadingPopupObject == null)
//			 _loadingPopupObject = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_UILoadingPopupPath) as GameObject).gameObject;
//	}
//
//	public void hideUiLoadingPopup()
//	{
//		if(_loadingPopupObject != null)
//			GameObject.Destroy(_loadingPopupObject);
//	}
//	
//	
//	public void fpsText(string text)
//	{
//
//	}
//
//	public void idText(string text)
//	{
//		
//	}
//
//	public void openCredits(bool isDebugBuild)
//	{
//
//	}
//
//	public void skullLevelValue(int skullLevel)
//	{
//
//	}
//
//	public void getSkullLevelText()
//	{
//
//	}
//
//	public void setSkullLevelText(string input)
//	{
//
//	}
//	
//	public MarketUI OpenMarketUI()
//	{
//		if(menu != null)
//		{
//			Destroy(menu.gameObject);
//			menu = null;
//		}
//
//		return gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_MarketUIPath) as GameObject).GetComponent<MarketUI>();
//	}
//	
//	public void showTopStats()
//	{
////		Debug.Log("showTopStats");
//		_topPanel.ShowTopStats();
//	}
//
//	public void hideTopStats()
//	{
//		_topPanel.HideTopStats();
//	}
//	
//	CutsceneDialogUI _cutsceneDialogUI;
//	public void showDilogBox(float x, float y, Action nextCB, Action skipCB) //add code for cutscenes... //farhan
//	{
//		_cutsceneDialogUI = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_CutsceneDialogUIPath) as GameObject).GetComponent<CutsceneDialogUI>();
//		_cutsceneDialogUI.Show(nextCB, skipCB);
//	}
//
//	public void hideDilogBox()
//	{
//		Destroy(_cutsceneDialogUI.gameObject);
//		_cutsceneDialogUI = null;
//	}
//
//	public void changeDilogBox(string title, string text, float x, float y)
//	{
//		_cutsceneDialogUI.UpdateDialog(title, text);
//	}
//
//	public void showRaidsVictoryPopup(int position, int arcana, int topDamage, int bossHp, int raidsSize, long timeLeft)
//	{
//		RaidsPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_RaidsPopupPath) as GameObject).GetComponent<RaidsPopup>();
//		popup.Show(position, arcana, topDamage, bossHp, raidsSize, timeLeft, _listenerInterface.onRaidsVictoryPopupClose); //Add callback if any!
//	}
//
//	public void showRaidsDefeatPopup(int position, int arcana, int topDamage, int bossHp, int raidsSize, long timeLeft)
//	{
//		RaidsPopup popup = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_RaidsDefeatPopupPath) as GameObject).GetComponent<RaidsPopup>();
//		popup.Show(position, arcana, topDamage, bossHp, raidsSize, timeLeft, _listenerInterface.onRaidsDefeatPopupClose); //Add callback if any!
//	}
//	public void DisplayCenterButton(bool isVisible)
//	{
////		if(GameManager.PRINT_LOGS) Debug.Log("DisplayCenterButton: " + isVisible);
////		if(generalSwf!=null)
////		{
////			Value menuButton = generalSwf.GetMember("menuButton");
////			SFDisplayInfo displayInfo = menuButton.GetDisplayInfo();
////			displayInfo.Visible = isVisible;
////			menuButton.SetDisplayInfo(displayInfo);
////		}
//		_topPanel.DisplayCenterButton(isVisible);
//	}
//	
//	public void showPauseMenu(bool isBattlePopup,int dayCount)
//	{	
//		if(isBattlePopup)
//		{
//			showBattleMenu(dayCount);
//		}
//		else
//		{
//			showInGameMenu(dayCount);
//		}
//	}
//	
//	
//	public void CloseLoadedUIs()
//	{
//		List<Transform> transformsToDel = new List<Transform>();
//		for(int i = 0; i <transform.childCount ; i++)
//		{
//			if(transform.GetChild(i).name.EndsWith("(Clone)"))
//			{
//				transformsToDel.Add(transform.GetChild(i));
//			}
//		}
//		
//		transformsToDel.ForEach(t => Destroy(t.gameObject));
//	}
//	
//	
//	public void GlowMenuIcon()
//	{
//		_topPanel.GlowMenuIcon();
//	}
//	
//	public void RemoveGlowMenuIcon()
//	{
//		_topPanel.RemoveGlowMenuIcon();
//	}
//	
//	
//	
	
}
