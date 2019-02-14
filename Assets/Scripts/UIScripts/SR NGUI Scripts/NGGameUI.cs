using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class NGGameUI : MonoBehaviour
{
	
	
    public GameManager.GeneralSwfLoaded gSwfDelegate = null;
	
    public void Start()
    {
        if (gSwfDelegate != null)
            gSwfDelegate(true);
		
        if (_topPanel != null)
            _topPanel.SetListener(_listenerInterface);
    }
	
    GameUIInterface _listenerInterface;
    public void SetListener(GameUIInterface listenerInterface)
    {
        _listenerInterface = listenerInterface;
        if (_topPanel != null)
            _topPanel.SetListener(_listenerInterface);
    }
	
	
	#region shrine related calls...
	
    private NGShrinePopup _shrinePopupScript;
	
    public void showShrinePopup(string element, int shrineLevel, int chargePoints, int maxChargePoints, long timmer, bool isCharged, bool isLocked)
    {
        if (_shrinePopupScript == null)
        {
            UnityEngine.Object asset = Resources.Load(_ShrinePopupPath);
            _shrinePopupScript = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGShrinePopup>();
        }
		if (_shrinePopupScript != null)
			_shrinePopupScript.Init(element, shrineLevel, chargePoints, maxChargePoints, timmer, isCharged, isLocked, _listenerInterface.ChargeShrine, _listenerInterface.CollectShrineReward);
	}
	
    public void setShrineTimmer(long timmer)
    {
        if (_shrinePopupScript != null)
        {
            _shrinePopupScript.SetShrineTimer(timmer);
        }
    }
	
    public void setShrineLevel(int shrineLevel)
    {
        if (_shrinePopupScript != null)
        {
            _shrinePopupScript.SetShrineLevel(shrineLevel);
        }
    }
	
    public void CloseShrine()
    {
        if (_shrinePopupScript != null)
        {
            Destroy(_shrinePopupScript.gameObject);
            _shrinePopupScript = null;
        }
    }
	
    public void toggleChargeShrine(bool isCharged)
    {
        if (_shrinePopupScript != null)
        {
			
        }
    }
	
    public void ShrineConnect()
    {
        Debug.Log("SHOW :::: ShrineConnect ::::");
    }
	
    public void ShrineInvite(int points, int friendCount, string friendsList)
    {
		
    }
	
    public void shrineRingChargeTutorial()
    {
		
    }
	
    public void shrineRingChargeTutorialOff()
    {
		
    }
	
    public void setBarFill(int currentVal, int maxVal) //Shring bar fill!
    {
        if (_shrinePopupScript != null)
        {
            _shrinePopupScript.SetBarFill(currentVal, maxVal);
        }
    }
	
	#endregion
	
	
    public void ShowSignInPopup()
    {
		
    }
	
	
    public void PlayBattleTutorial()
    {
		
    }
	
    public void showNoConnectivityPopup(string msg)
    {
		
    }
	
    public void setInventoryData(string jsonData)
    {
		
    }
	
    public void gameUserAndVersion(string name, string version)
    {
		
    }
	
	#region Top Panel related ...
	
    public NGGameUITopPanel _topPanel;
	
    public void updateHealth(int health)
    {
        _topPanel.UpdateHealth(health);
    }
	
    public void updateHealth(int health, bool playHeartAnimation)
    {
        _topPanel.UpdateHealth(health, playHeartAnimation);
    }
	
    public void updateSoulGems(int soulGems)
    {
        _topPanel.UpdateSoulGems(soulGems);
    }
	
    public void updateSoulDust(float soulDust)
    {
        _topPanel.UpdateSoulDust(soulDust);
    }
	
	#endregion
	
    public void showLootPopup(int itemDamage, int itemWards)
    {
		
    }
	
	#region Keys Popup....
	
    private NGKeysPopup _keysPopupScript;
	
    public void showKeysPopup(int availableKeys, float price, int requiredKeys)
    {
        Debug.Log("showKeysPopup");
        if (_keysPopupScript == null)
        {
            UnityEngine.Object asset = Resources.Load(_BuyKeysPotionsPopupPath);
            _keysPopupScript = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGKeysPopup>();
            _keysPopupScript.init(availableKeys, price, requiredKeys);
        }
    }
	
    public void showKeysPopup(int availableKeys, float price)
    {
        Debug.Log("showKeysPopup 2 param...");
		
        if (_keysPopupScript == null)
        {
            UnityEngine.Object asset = Resources.Load(_BuyKeysPotionsPopupPath);
            _keysPopupScript = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGKeysPopup>();
            _keysPopupScript.init(availableKeys, price);
        }
    }
	
	#endregion


    public void showUiGeneralPopup(string heading, string message)
    { 
        UnityEngine.Object asset = Resources.Load(_generalPopupPath);
        NGGeneralPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGGeneralPopup>();
        popup.showPopup(heading, message, _listenerInterface.GeneralPopupButton1Pressed);
    }
	
    public void showUiGeneralPopup(string heading, string message, Action cb)
    {
		NGGeneralPopup oldPopup = GameObject.FindObjectOfType<NGGeneralPopup>();
		if(oldPopup != null) {
			Destroy(oldPopup.gameObject);
		}
        UnityEngine.Object asset = Resources.Load(_generalPopupPath);
        NGGeneralPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGGeneralPopup>();
        popup.showPopup(heading, message, cb);
		NGUITools.BringForward(popup.gameObject);
    }
	
    public void showGeneralPopup2(string title, string message, string btn1Text, string btn2Text)
    {
        UnityEngine.Object asset = Resources.Load(_TwoButtonPopupPath);
        NGTwoButtonPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTwoButtonPopup>();
        popup.showGeneralPopup2(title, message, btn1Text, _listenerInterface.GeneralPopupButton1Pressed, btn2Text, _listenerInterface.GeneralPopupButton2Pressed);
    }
	
    public void showGeneralPopup2(string title, int numberVal, string message, string btn1Text, string btn2Text)
    {
        UnityEngine.Object asset = Resources.Load(_TwoButtonPopupPath);
        NGTwoButtonPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTwoButtonPopup>();
        popup.showGeneralPopup2(title, message, numberVal, btn1Text, _listenerInterface.GeneralPopupButton1Pressed, btn2Text, _listenerInterface.GeneralPopupButton2Pressed);
		NGUITools.BringForward(popup.gameObject);
	}
	
    public void showGeneralPopup3(string title, string message, string btn1Text, string btn2Text)
    {
        UnityEngine.Object asset = Resources.Load(_TwoButtonPopupPath);
        NGTwoButtonPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTwoButtonPopup>();
        popup.showGeneralPopup2(title, message, btn1Text, _listenerInterface.GeneralPopup3Button1Pressed, btn2Text, _listenerInterface.GeneralPopup3Button2Pressed);
    }

	public void showGeneralPopup4(string title, string message, string btn1Text, string btn2Text, Action callbackBtn1, Action callbackBtn2) {
		UnityEngine.Object asset = Resources.Load(_TwoButtonPopupPath);
		NGTwoButtonPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTwoButtonPopup>();
		popup.showGeneralPopup2(title, message, btn1Text, callbackBtn1, btn2Text, callbackBtn2);
	}

    public void userloginState(bool facebook, bool gamecenter, bool googleplus)
    {
		
    }
	
    public void OpenMarketUi()
    {
		
    }
	
    public NGTransmuteUi OpenTransmuteUi()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_TransmuteUiPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTransmuteUi>();
    }
	
    public NGUpgradeui OpenUpgradeUi()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_UpgradeUiPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGUpgradeui>();
    }
	
    public NGRingUi OpenRingsUi()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_ringUIPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRingUi>();
    }
	
    public NGRuneUi OpenStaffUi()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_RuneUiPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRuneUi>();
    }
	
	
    public void OpenPetUi()
    {
		
    }
	
    public NGSpiritsUI OpenSpiritsUI()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_SpiritsMenuPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGSpiritsUI>();
    }
	
    public NGOptionsMenu OpenOptionsUI()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_OptionsMenuPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGOptionsMenu>();
    }
	
    public void showGeneralPopup(string heading, string message)
    {
        showUiGeneralPopup(heading, message);
    }
	
    public void showVictoryPopup(int soulsWon, int healthLost, int battleTime, int topDamage, string grade)
    {
        UnityEngine.Object asset = Resources.Load(_VictoryPopupPath);
        NGVictoryPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGVictoryPopup>();
        popup.showPopup(soulsWon, healthLost, battleTime, topDamage, grade, _listenerInterface.onVictoryContinue); //Add callback if any!
    }
	
    public void showLootRingPopup(InventorySystem.ItemRing ring, bool isTransmute)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupRingsPath);
        NGRingsLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRingsLootPopup>();
        popup.showLootRingPopup(ring, isTransmute, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showLootRingPopup(InventorySystem.ItemRing ring)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupRingsPath);
        NGRingsLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRingsLootPopup>();
        popup.showLootRingPopup(ring, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showLootRunePopup(InventorySystem.ItemRune rune, bool isTransmute)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupRunesPath);
        NGRunesLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRunesLootPopup>();
        popup.showLootRunePopup(rune, isTransmute, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showLootRunePopup(InventorySystem.ItemRune rune)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupRunesPath);
        NGRunesLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRunesLootPopup>();
        popup.showLootRunePopup(rune, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showLootKeysPopup(int keys, bool isAnchored = false)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupKeysPath);
        NGKeysLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGKeysLootPopup>();
        popup.showLootKeysPopup(keys, _listenerInterface.lootPopupClosed, isAnchored); //Add callback if any!
    }
	
    public void showLootGemsPopup(int gems)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupGemsPath);
        NGSoulBagLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGSoulBagLootPopup>();
        popup.showLootKeysPopup(gems, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showSoulPopup(int soulValue)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupSoulBagPath);
        NGSoulBagLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGSoulBagLootPopup>();
        popup.showLootKeysPopup(soulValue, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showShardPopup(int shardCount)
    {
        UnityEngine.Object asset = Resources.Load(_shardPopupPath);
        NGRingShardPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRingShardPopup>();
        popup.Show(shardCount, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    public void showDefeatPopup()
    {
        UnityEngine.Object asset = Resources.Load(_defeatPopupPath);
        NGDefeatPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGDefeatPopup>();
        popup.SetCallbacks(_listenerInterface.onDefeatReturn, _listenerInterface.onDefeatRestart);
    }
	
    public void updateSoulDust(int soulGems)
    {
		
    }
	
    public void updateDamage(int totalDamage)
    {
		
    }
	
    public void showBossDefeatPopup(string heading, string text)
    {
        UnityEngine.Object asset = Resources.Load(_bossDefeatPopupPath);
        NGBossDefeatPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGBossDefeatPopup>();
        popup.SetCallbacks(_listenerInterface.bossDefeatPopupClosed);
    }
	
    public void showUnLockPopup(NGUnlockedUpgradesPopup.UnlockedUpgradesType popupType)
    {
        UnityEngine.Object asset = Resources.Load(_UnlockPopupPath);
        NGUnlockedUpgradesPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGUnlockedUpgradesPopup>();
        popup.showUnlockedUpgrade(popupType, null); //farhan
    }
	
    public void setLanguage(string languageString, string charSetID)
    {
		
    }
	
    public void setInventtoryData(string jsonString, string suggestedJsonString)
    {
		
    }
	
    public void loadSwf(string filename)
    {
		
    }
	
    public void showPotionPopup(int count, int price)
    {
//		GameObject potionPopup = ;
		NGPotionPopup popup = GameObject.FindObjectOfType<NGPotionPopup>();
		if(popup == null) {
			UnityEngine.Object asset = Resources.Load(_PotionPopupPath);
			popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGPotionPopup>();
		}

        popup.Show(count, price, _listenerInterface.onUseHealthPotion, _listenerInterface.onPotionBuyButton, _listenerInterface.onCloseHealthPopup);
    }
	
    public void unloadSwf()
    {
		
    }
	
    public void SfxVolume(double volume)
    {
		
    }
	
    public void setMVolume(double volume)
    {
		
    }
	
    public void onBuyRingComplete(string response)
    {
		
    }
	
    public void onSellRingComplete()
    {
		
    }
	
    public void onBuyRuneComplete(string response)
    {
		
    }
	
    public void onSellRuneComplete()
    {
		
    }
	
	public void SetOnTop()
	{
		_topPanel.SetOnTop();
	}

	public void ShowHeartIcon()
	{
		_topPanel.ShowHeartIcon();
	}

	public void HideHeartIcon()
    {
        _topPanel.HideHeartIcon();
    }
	
    public void showLootPotionPopup(int val)
    {
        UnityEngine.Object asset = Resources.Load(_lootPopupPotionsPath);
        NGSoulBagLootPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGSoulBagLootPopup>();
        popup.showLootKeysPopup(val, _listenerInterface.lootPopupClosed); //Add callback if any!
    }
	
    private void showBattleMenu(int dayCount)
    {
		updateSoulGems (GameManager._gameState.User._inventory.gems);
		updateSoulDust (GameManager._gameState.User._inventory.souls);
		updateHealth (GameManager._gameState.User.life);
		updateDamage (GameManager._gameState.User.damage);
		UnityEngine.Object asset = Resources.Load(battlePauseMenuPrefabPath);
        NGUITools.AddChild(this.gameObject, asset as GameObject);
    }
	
    NGPauseMenu menu;
    private void showInGameMenu(int dayCount)
    {
        Debug.Log("[NGGameUI] showInGameMenu");
        if (menu != null)
        {
            DestroyImmediate(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_pauseMenuPrefabPath);
        menu = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGPauseMenu>();
//		NGUITools.BringForward(menu.gameObject);
        menu.SetListener(_listenerInterface);
    }

	#region Loading screen related calls...
	
    private NGLoadingScreen _loadingScreenScript;
	
    public void onLoadingScreen() //Show loading screen... 'wtf' naming conventions :/
    {
        if (_loadingScreenScript == null)
        {
            UnityEngine.Object asset = Resources.Load(_LoadingScreenPrefabPath);
            _loadingScreenScript = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGLoadingScreen>();
        } else
        {
            _loadingScreenScript.SetDefaultValues();
        }
    }
	
    public void onUnLoadingScreen() //Hide loading screen... 'wtf' naming conventions :/
    {
        if (_loadingScreenScript != null)
        {
            Destroy(_loadingScreenScript.gameObject);
            _loadingScreenScript = null;
        }
    }
	
    public void LoadingScreenShowBar(string title) //Set loading bar text
    {
        //		Debug.Log("LoadingScreenShowBar" + title);
        if (_loadingScreenScript == null)
        {
            Debug.Log("_loadingScreenScript NULL at showTextOnBar :(");
            return;
        }
		
        _loadingScreenScript.ShowLoadingScreenBarText(title);
    }
	
    public void LoadingScreenHideBar()
    {
//		Debug.Log("LoadingScreenHideBar");
        if (_loadingScreenScript == null) 
            return;
		
        _loadingScreenScript.HideLoadingBar();
    }
	
    public void LoadingPercentage(int perc)
    {
//		Debug.Log("LoadingPercentage value = " + perc);
        if (_loadingScreenScript == null)
        {
            Debug.Log("_loadingScreenScript NULL");
            return;
        }
		
        _loadingScreenScript.SetLoadingPercentage(perc);
    }
	
    public void OnGUI()
    {
        /*
		if (GUI.Button(new Rect(10, 50, 150, 30), "Show Loading Screen"))
			onLoadingScreen();
		
		if (GUI.Button(new Rect(10, 90, 150, 30), "Hide Loading Screen"))
            onUnLoadingScreen();
		
		if (GUI.Button(new Rect(10, 130, 150, 30), "LoadingScreen Show bar"))
            LoadingScreenShowBar("Sup idiot!");
		
		if (GUI.Button(new Rect(10, 170, 150, 30), "Ring ui"))
            loadRingUi();
		
		if (GUI.Button(new Rect(10, 210, 150, 30), "Ring ui"))
		{
			showGeneralPopup2("My Title", "my message", "OKAY", "CANCEL");
		}
		
		if (GUI.Button(new Rect(10, 240, 150, 30), "InputWrapper enable touch"))
			InputWrapper.disableTouch = false;
		*/
    }
	
	#endregion
    /*
	public void loadRingUi()
	{
		//gameObject.GetComponent<dfPanel>().AddPrefab(_ringUI.gameObject);
	}
	
	public void loadUpgradeUi()
	{
		gameObject.GetComponent<dfPanel>().AddPrefab(_UpgradeUi.gameObject);
	}
	
	public void loadRuneUi()
	{
		gameObject.GetComponent<dfPanel>().AddPrefab(_RuneUi.gameObject);
	}
	
	public void loadTransmuteUi()
	{
		gameObject.GetComponent<dfPanel>().AddPrefab(_TransmuteUi.gameObject);
	}
	*/
	
	
    private GameObject _loadingPopupObject;
    public void showUiLoadingPopup()
    {

        if (_loadingPopupObject == null)
        {
            UnityEngine.Object asset = Resources.Load(_UILoadingPopupPath);
            _loadingPopupObject = NGUITools.AddChild(this.gameObject, asset as GameObject);
        }
    }
	
    public void hideUiLoadingPopup()
    {
        if (_loadingPopupObject != null)
            GameObject.Destroy(_loadingPopupObject);
    }
	
	
    public void fpsText(string text)
    {
		
    }
	
    public void idText(string text)
    {
		
    }
	
    public void openCredits(bool isDebugBuild)
    {
		
    }
	
    public void skullLevelValue(int skullLevel)
    {
		
    }
	
    public void getSkullLevelText()
    {
		
    }
	
    public void setSkullLevelText(string input)
    {
		
    }
	
    public NGMarketUI OpenMarketUI()
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            menu = null;
        }

        UnityEngine.Object asset = Resources.Load(_MarketUIPath);
        return NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGMarketUI>();
    }
	
    public void showTopStats()
    {
        //		Debug.Log("showTopStats");
        _topPanel.ShowTopStats();
    }
	
    public void hideTopStats()
    {
        _topPanel.HideTopStats();
    }
	
    NGCutsceneDialogUI _cutsceneDialogUI;
    public void showDilogBox(float x, float y, Action nextCB, Action skipCB) //add code for cutscenes... //farhan
    {
        UnityEngine.Object asset = Resources.Load(_CutsceneDialogUIPath);
        _cutsceneDialogUI = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGCutsceneDialogUI>();
        _cutsceneDialogUI.Show(nextCB, skipCB);
    }
	
    public void hideDilogBox()
    {
		Debug.LogError("hide dialog box - - - - - - - - ");;
		if(_cutsceneDialogUI != null) {
        	Destroy(_cutsceneDialogUI.gameObject);
        	_cutsceneDialogUI = null;
		}
    }
	
    public void changeDilogBox(string title, string text, float x, float y)
    {
        _cutsceneDialogUI.UpdateDialog(title, text);
    }
	
    public void showRaidsVictoryPopup(int position, int arcana, int deltaArcane, int topDamage, int bossHp, int raidsSize, long timeLeft)
    {
        UnityEngine.Object asset = Resources.Load(_RaidsPopupPath);
        NGRaidsPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRaidsPopup>();
        popup.Show(position, arcana, deltaArcane, topDamage, bossHp, raidsSize, timeLeft, _listenerInterface.onRaidsVictoryPopupClose); //Add callback if any!
    }
	
	public void showRaidsDefeatPopup(int position, int arcana, int deltaArcane, int topDamage, int bossHp, int raidsSize, long timeLeft)
    {
        UnityEngine.Object asset = Resources.Load(_RaidsDefeatPopupPath);
        NGRaidsPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGRaidsPopup>();
		popup.Show(position, arcana, deltaArcane, topDamage, bossHp, raidsSize, timeLeft, _listenerInterface.onRaidsDefeatPopupClose); //Add callback if any!
    }
    public void DisplayCenterButton(bool isVisible)
    {
        //		if(GameManager.PRINT_LOGS) Debug.Log("DisplayCenterButton: " + isVisible);
        //		if(generalSwf!=null)
        //		{
        //			Value menuButton = generalSwf.GetMember("menuButton");
        //			SFDisplayInfo displayInfo = menuButton.GetDisplayInfo();
        //			displayInfo.Visible = isVisible;
        //			menuButton.SetDisplayInfo(displayInfo);
        //		}
        _topPanel.DisplayCenterButton(isVisible);
    }
	
    public void showPauseMenu(bool isBattlePopup, int dayCount)
    {	
        if (isBattlePopup)
        {
            showBattleMenu(dayCount);
        } else
        {
//			GameManager.instance._levelManager
            showInGameMenu(dayCount);
        }
    }
	
	
    public void CloseLoadedUIs()
    {
        List<Transform> transformsToDel = new List<Transform>();
        for (int i = 0; i <transform.childCount; i++)
        {
            if (transform.GetChild(i).name.EndsWith("(Clone)"))
            {
                transformsToDel.Add(transform.GetChild(i));
            }
        }
		
        transformsToDel.ForEach(t => Destroy(t.gameObject));
    }
	
	
    public void GlowMenuIcon()
    {
        _topPanel.GlowMenuIcon();
    }
	
    public void RemoveGlowMenuIcon()
    {
        _topPanel.RemoveGlowMenuIcon();
    }
}
