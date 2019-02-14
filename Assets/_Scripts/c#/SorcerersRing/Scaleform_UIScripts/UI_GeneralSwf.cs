using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using GameStateModule;
using MiniJSON;
using InventorySystem;
using Game.UI;

public class UI_GeneralSwf : MonoBehaviour, GameUIInterface, RingUiInterface, UpgradeUiInterface, TransmuteInterface, RuneUiInterface, OptionsUIInterface, MarketUIInterface, SpiritsUIInterface
{
	public NGGameUI generalSwf = null;
	protected MonoBehaviour tempSwf = null;
	UIManager sfCamera = null;
	//UI_Scene_Inventory _inventoryRef = null;
	//UI_CheatSheet _cheatSheetRef = null;
	public string tempMenuName = "";
	public bool tempPopUpOpened = false; //temprary logic for multiple pop ups
	public GameManager.GeneralSwfLoaded gSwfDelegate = null;
	bool isLoading = false;
	private string username = "";
	// Required to implement this constructor.
	private string eventClass = "UIGeneralSwf";
/*	public UI_GeneralSwf(ScaleformCamera parent,GameManager.GeneralSwfLoaded gSwfDel,UI_Scene_Inventory inventory,SFManager sfmgr, SFMovieCreationParams cp):
	base(sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "GameUI.swf" ),  cp)
	{
		AdvanceWhenGamePaused=true;
		//_inventoryRef=inventory;
		//_cheatSheetRef=cSheet;
		gSwfDelegate=gSwfDel;
		sfCamera=parent;
		
	}
	
	public UI_GeneralSwf(ScaleformCamera parent,SFManager sfmgr, SFMovieCreationParams cp):
	base(sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "GameUI.swf" ),  cp)
	{
		AdvanceWhenGamePaused=true;
	
		sfCamera=parent;	
	}
*/	
	
	public NGRingUi ringUI;
	public NGRuneUi runeUI;
	public NGTransmuteUi transmuteUI;
	public NGUpgradeui upgradeUI;
	public NGMarketUI marketUI;
	public NGSpiritsUI spiritsUI;
	
	public void Start ()
	{
		sfCamera = UIManager.instance;
	}
	
	
	public void loginToFacebook ()
	{
		/*
		return;
		
		Analytics.logEvent(Analytics.EventName.Options_FacebookLogin);	
		
		if(GameManager.PRINT_LOGS) Debug.Log("loginToFacebook==="+FB.IsLoggedIn);
		if(!FB.IsLoggedIn)
		{
			FB.Init(OnInitComplete,OnHideUnity);
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("FB already Logged In==="+FB.IsLoggedIn+"fbloginToFacebook:userId = "+FB.UserId+"fbloginToFacebook:access_token"+FB.AccessToken);
			SocialMediaManager.Instance.fbAccessToken=FB.AccessToken;
			SocialMediaManager.Instance.fbUserId=FB.UserId;
			userLoginState(true,false,false);
		}
		*/
	}
	
	
	/*
	private void OnInitComplete()
    {
		if(!FB.IsLoggedIn)
		{
        	if(GameManager.PRINT_LOGS) Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
			FB.Login("email,publish_actions", LoginCallback);
		}
    	
    }
	
	private	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
	    if(GameManager.PRINT_LOGS) FbDebug.Log("APICallback");
	    if (result.Error != null)                                                                                                  
	    {                                                                                                                          
	        FbDebug.Error(result.Error);                                                                                           
	        FB.API("/me?fields=id,first_name,friends.limit(20).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);     
	        return;                                                                                                                
	    }                                                                                                                          
	
	 		IDictionary profile = Json.Deserialize(result.Text) as IDictionary;
			//setUsername
			username= profile["first_name"].ToString();
			GameUserAndVersion(username,GameManager.APP_VERSION);
	                                                                        
	} 

    private void OnHideUnity(bool isGameShown)
    {
        if(GameManager.PRINT_LOGS) Debug.Log("Is game showing? " + isGameShown);
    }
	
	void LoginCallback(FBResult result) {
	if(GameManager.PRINT_LOGS) Debug.Log("fbLoginCallback:result.Text= "+result.Text+"fbLoginCallback:result.Error ="+result.Error);
		
    if(GameManager.PRINT_LOGS) Debug.Log("USER ID ="+FB.UserId+"access_token = "+FB.AccessToken); // Use this to sync progress, etc.
	SocialMediaManager.Instance.fbAccessToken=FB.AccessToken;
	SocialMediaManager.Instance.fbUserId=FB.UserId;
	FB.API("/me?fields=id,first_name,friends.limit(20).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);
	userLoginState(true,false,false);
	ServerManager.Instance.RegisterWithFacebook(SocialMediaManager.Instance.RegisterWithFacebookHandler,FB.AccessToken);
	}

	
	void InviteFriendsCallback(FBResult result)
    {
		HideUILoadingScreen(false);
      	if (result.Error != null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("Error Response:\n" + result.Error);
		}
        else
        {
			Analytics.logEvent(Analytics.EventName.Shrine_Invite);
             if(GameManager.PRINT_LOGS) Debug.Log("Success Response:\n"+result.Text);
        }
    }
	
	#region FB.AppRequest() Friend Selector

    public string FriendSelectorTitle = "Charge My Shrine!!!";
    public string FriendSelectorMessage = "I need to charge this mystical shrine! Can you help me?";
    public string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
    public string FriendSelectorData = "{}";
    public string FriendSelectorExcludeIds = "";
    public string FriendSelectorMax = "";

    private void CallAppRequestAsFriendSelector()
    {
        // If there's a Max Recipients specified, include it
        int? maxRecipients = null;
        if (FriendSelectorMax != "")
        {
            try
            {
                maxRecipients = Int32.Parse(FriendSelectorMax);
            }
            catch (Exception e)
            {
                if(GameManager.PRINT_LOGS) Debug.Log("ERROR ON INVITE FRIEND : "+e.Message);
				HideUILoadingScreen(false);
            }
        }

        // include the exclude ids
        string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');

        FB.AppRequest(
            message: FriendSelectorMessage,
            filters: FriendSelectorFilters,
            excludeIds: excludeIds,
            maxRecipients: maxRecipients,
            data: FriendSelectorData,
            title: FriendSelectorTitle,
            callback: InviteFriendsCallback
        );
    }
    #endregion
	*/
		#region ShrineCalls
	public void ShowShrinePopup (string element, int shrineLevel, int chargePoints, int maxChargePoints, long timmer, bool isCharged, bool isLocked)
	{
		Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string> ();
		analParams.Add (Analytics.ParamName.ShrineElement, ShrineManager.Instance.element);
		
		Analytics.logEvent (Analytics.EventName.Shrine_Open, analParams);
		
/*		Value val1 = new Value(element,MovieID);
		Value val2 = new Value(shrineLevel,MovieID);
		Value val3 = new Value(chargePoints,MovieID);
		Value val4 = new Value(maxChargePoints,MovieID);
		Value val5 = new Value(timmer,MovieID);
		Value val6 = new Value(chargeButtonState,MovieID);
		Value[] args = {val1,val2,val3,val4,val5,val6};
		
		generalSwf.Invoke("showShrinePopup",args);
*/		
		Debug.Log ("generalSwf.showShrinePopup(element, shrineLevel, chargePoints, maxChargePoints, timmer, chargeButtonState) for element >> " + element);
		
		generalSwf.showShrinePopup (element, shrineLevel, chargePoints, maxChargePoints, timmer, isCharged, isLocked);
	}
	
	void OnShrinePopUpOpened ()
	{
		
	}
	
	public void ShrineConnect ()
	{
		Analytics.logEvent (Analytics.EventName.Shrine_Connect);
//		loginToFacebook();
	}
	
	public void SetShrineBar (int currentVal, int maxVal)
	{
		/*
		Value val1 = new Value(currentVal,MovieID);
		Value val2 = new Value(maxVal,MovieID);
		Value[] args = {val1,val2};
		generalSwf.Invoke("setBarFill",args);
		*/
		
		generalSwf.setBarFill (currentVal, maxVal);
	}
	
	public void showLoginPopup ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("ShowSignInPopup", args);
		*/
		
		generalSwf.ShowSignInPopup ();
	}
	/// <summary>
	/// Play the battle tutorial. -- code added by mufti
	/// </summary>
	public void playBattleTutorial ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("PlayBattleTutorial", args);
		*/
		generalSwf.PlayBattleTutorial ();
	}
	
	public void battleEndTutorialCompleted ()
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted)) {
			TutorialManager.instance.AddTutorialIdWithStatus (TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted, true);
		}
	}
	
	public void SetShrineTimmer (long timmer)
	{
		/*
		Value val1 = new Value(timmer,MovieID);
		Value[] args = {val1};
		generalSwf.Invoke("setShrineTimmer",args);
		*/
		generalSwf.setShrineTimmer (timmer);
	}
	
	public void ShrineInviteFriend ()
	{
		/*if(GameManager.PRINT_LOGS) Debug.Log("UI_GENERALSWF~~~~~~~~~~~ShrineInviteFriend~~~~~~~~~~~~");
		 try
            {
				if(FB.IsLoggedIn)
				{
                	CallAppRequestAsFriendSelector();
					ShowUILoadingScreen(false);
				}
				else
				{
					loginToFacebook();
				}
                
            }
            catch (Exception e)
            {
               if(GameManager.PRINT_LOGS) Debug.Log("ERROR message on invite friend : " + e.Message);
            }
            */
	}

	//Callback from pressing the collect button on shrine UI
	public void CollectShrineReward ()
	{
		//GameManager.instance.scaleformCamera.NewTutorialSwf.ShrineTutorial5();
		Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string> ();
		analParams.Add (Analytics.ParamName.ShrineElement, ShrineManager.Instance.element);
		
		Analytics.logEvent (Analytics.EventName.Shrine_Collect, analParams);
		ShrineManager.Instance.CollectShrineReward ();
	}
	
	public void SetShrineLevel (int shrineLevel)
	{
		/*
		Value val = new Value(shrineLevel,MovieID);
		Value[] args = {val};
		generalSwf.Invoke("setShrineLevel",args);
		*/
		generalSwf.setShrineLevel (shrineLevel);
	}

	//Callback from pressing the charge button on shrine UI
	public void ChargeShrine ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UI_GENERALSWF~~~~~~~~~~~~~~Charge Shrinee~~~~~~~~~");
		Shrine tempShrine = ShrineManager.Instance.GetShrineForLevel (GameManager.instance._levelManager.currentLevel + "_Shrine");
		if (tempShrine != null && !tempShrine.isCharged) {
			Debug.LogWarning("Show shrine charging "+tempShrine.isCharged);
			ShrineManager.Instance.ActivateFriendShrine (GameManager.instance._levelManager.currentLevel + "_Shrine");
		} else {
			Debug.LogWarning("closing shrine "+tempShrine.isCharged);
			CloseShrinePopUp ();
			//ShowGeneralPopup("SHRINE ALREADY CHARGED","COME AGAIN AFTER A WHILE");
		}
	}
	
	public void CloseShrinePopUp ()
	{
		/*
		Value[] args = {};
		generalSwf.Invoke("CloseShrine",args);
		*/
		generalSwf.CloseShrine ();
		Time.timeScale = 1.0f;
		GameManager.instance.scaleformCamera.isPaused = false;
		InputWrapper.disableTouch = false;
		if(GameManager.instance._levelManager != null) {
			GameManager.instance._levelManager.ResetPlayerToNeutralPosition();
		}
	}
	
	public void ToggleChargeShrine (bool isCharged)
	{
		/*
		Value val= new Value(isCharged,MovieID);
		Value[] args={val};
		generalSwf.Invoke("toggleChargeShrine", args);
		*/
		generalSwf.toggleChargeShrine (isCharged);
	}
	#endregion
	
	public void LoadLoadingScreen ()
	{
		isLoading = true;
		/*
		Value[] args={};
		//if(GameManager.PRINT_LOGS) Debug.Log("load laoding screen");
		generalSwf.Invoke("onLoadingScreen", args);
		*/
		generalSwf.onLoadingScreen ();
	

	}
	
	public void UnLoadLoadingScreen ()
	{
		isLoading = false;
		/*
		Value[] args={};
//		if(GameManager.PRINT_LOGS) Debug.Log("unloading laoding screen");
		generalSwf.Invoke("onUnLoadingScreen", args);
		*/
		generalSwf.onUnLoadingScreen ();
	}
	
	public void LoadingScreenShowBar (string title)
	{
		/*
		Value tutorialIdVal = new Value(title, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		generalSwf.Invoke("LoadingScreenShowBar", tutorialIdValArr);
		*/
		generalSwf.LoadingScreenShowBar (title);
	}
	
	public void LoadingScreenHideBar ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("LoadingScreenHideBar", args);
		*/
		generalSwf.LoadingScreenHideBar ();
	}
	
	public void LoadingPercentage (int perc)
	{
		/*
		Value tutorialIdVal = new Value(perc, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		generalSwf.Invoke("LoadingPercentage", tutorialIdValArr);
		*/
		generalSwf.LoadingPercentage (perc);
	}
	
	public void ShowNoConnectivityPopup (string msg)
	{

		if (GameManager.PRINT_LOGS) 
			Debug.Log ("ShowNoConnectivityPopup");
		/*
		Value tutorialIdVal = new Value(msg, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		generalSwf.Invoke("showNoConnectivityPopup", tutorialIdValArr);
		*/
		generalSwf.showNoConnectivityPopup (msg);
	}
	
	public void reTryConnection ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("RetryConnection!!");
		/*
		IList<AssetBundleLoader> loaderList = GameManager.instance._levelManager.GetComponentsInChildren<AssetBundleLoader>();
		foreach(AssetBundleLoader tempLoader in loaderList){
			if(tempLoader.isDownloading){
				tempLoader.StartCoroutine(tempLoader.DownloadAB());
			}
		}
		*/
		BundleDownloadManager.instance.Invoke ("Start", 1.0f);
	}
	
	public void DisplayCenterButton (bool isVisible)
	{
		generalSwf.DisplayCenterButton (isVisible);
	}
	public void onStaffButton ()
	{
		/*
		 * Temporarily removed - for testing upgrades functionality
		if(GameManager.PRINT_LOGS) Debug.Log("TESTTINGGGG=================");
		tempMenuName="Staff";
		LoadSwf("staff1.swf");
		*/
		tempMenuName = "Staff";
		//LoadSwf("staff.swf");
		generalSwf.OpenStaffUi ().setInterface (this);

		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RuneTutorialCompleted)) {
			TutorialManager.instance.LoadUIStaffTutorial ();
			Analytics.logEvent (Analytics.EventName.RuneUI_Tutorial_Start);
		}
		Analytics.logEvent (Analytics.EventName.PauseMenu_Rune);
	}
	
	public void onRingsButton ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onRingsButton");

		tempMenuName = "Inventory";
		ringUI = generalSwf.OpenRingsUi ();
		ringUI.setInterface (this);
		if(!GameManager._gameState.User._inventory.HasNewRingInBag ()) {
			ringUI.GoToRingNumber (0, true);
		}

		Resources.UnloadUnusedAssets ();

		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Show ring tutorial");

			TutorialManager.instance.LoadUIRingTutorial ();
			Analytics.logEvent (Analytics.EventName.RingUI_Tutorial_Start);
		}
		
		Analytics.logEvent (Analytics.EventName.PauseMenu_Ring);
	}
	
	public void onUpgradeButton (PurchaseManager.GeneralPopupType popupType)
	{
		tempMenuName = "Upgrades";
		Debug.LogError("onUpgradeButton 1");
		ServerRequest tRequest = new UpdateRequest (ServerRequest.ServerRequestType.All, GameManager._gameState.User._inventory.ProcessResponse);
		ServerManager.Instance.ProcessRequest (tRequest);
		//LoadSwf("upgrades.swf");
		
		if (ringUI != null) {
			
			Destroy (ringUI.gameObject);
			ringUI = null;
		}
		if (runeUI != null) {
			Destroy (runeUI.gameObject);
			runeUI = null;
		}
		if (transmuteUI != null) {
			Destroy (transmuteUI.gameObject);
			transmuteUI = null;
		}
		if (spiritsUI != null) {
			Destroy (spiritsUI.gameObject);
			spiritsUI = null;
		}
		
//		dfMaterialCache.Clear();
		Resources.UnloadUnusedAssets ();
		NGUpgradeui upgradeUI = generalSwf.OpenUpgradeUi ();
		upgradeUI.setInterface (this);
//		upgradeUI.GoToUpgrade(
//		if(popupType != null) {
			upgradeUI.GoToUpgrade(popupType);
//		}
		//generalSwf._topPanel.GetComponent<dfPanel>().BringToFront();
		
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.UpgradeTutorialCompleted)) {
			TutorialManager.instance.loadUIUpgradeTutorial ();
			Analytics.logEvent (Analytics.EventName.UpgradesUI_Tutorial_Start);
		}
		Analytics.logEvent (Analytics.EventName.PauseMenu_Upgrade);
	}
	
	public void onPetButton () //temp cb from GUI
	{
		onSpiritsButton ();
	}
	
	private List<InventorySystem.InventoryItem> itemsToTransmute;

	public void ShowTopPanel (bool show)
	{
		generalSwf._topPanel.gameObject.SetActive (show);
	}

	public void onTransmutationButton ()
	{
		if (GameManager._gameState.User.canTransmute || IsTransmuting ()) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("GameManager._gameState.User.canTransmute is TTTTRRRRUUUUUUEEEEEEE");
			
			tempMenuName = "Transmutation";
			
			ServerRequest tRequest = new UpdateRequest (ServerRequest.ServerRequestType.All, ProcessResponse);
			ServerManager.Instance.ProcessRequest (tRequest);
	
			//LoadSwf("transmute.swf");
			generalSwf.OpenTransmuteUi ().setInterface (this);
			//generalSwf._topPanel.GetComponent<dfPanel>().BringToFront();

			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted)) {
				GameManager.instance.scaleformCamera.loadUITransmutationTutorial ();
				Analytics.logEvent (Analytics.EventName.TransmuteUI_Tutorial_Start);
			}
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("GameManager._gameState.User.canTransmute is FFFFAAAALLLLSSSSSEEEE");
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			ShowGeneralPopup ("Transmutation unavailable", "You need more items in your bag!");
		}
		Analytics.logEvent (Analytics.EventName.PauseMenu_Transmute);
	}
	
	private bool IsTransmuting ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log (" IsTransmuting() :::::::::::::::::::::: " + GameManager._gameState.User.transmutationDictionary.Contains ("Type"));
		return GameManager._gameState.User.transmutationDictionary.Contains ("Type");
	}
	
	public void SetTransmuteData (string jsonString)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("JSON SENT TO TRANSMUTE::::" + jsonString);
		/*
		Value val=new Value(jsonString,MovieID);
		Value[] args1={val};
		generalSwf.Invoke("setInventoryData",args1);
		*/
		generalSwf.setInventoryData (jsonString);
		
	}
	
	public bool onStartTransmute (string uidsString)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("uidsString : " + uidsString);
		// Transmute tutorial - start
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf != null) {
			
			if (GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount == 2) {
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.closeUIScreen ("TransmutationTutorial4");
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount++;
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.ShowTransmutationTutorialEnd ();
			}
		}
		// Transmute tutorial - end
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("ON START TRANSMUTE CALLED FROM SCALEFORM!!!");
		List<InventorySystem.InventoryItem> items = new List<InventorySystem.InventoryItem> ();
		
		string[] uids = null;
		string s = null;
		int count = 0;
		if (uidsString != null) {
			uids = uidsString.Split (',');
			count = uids.Length;
			for (int i = 0; i<count; i++) {
				s = uids [i];
				InventorySystem.InventoryItem item = GameManager._gameState.User._inventory.GetBagItemForUID (s);
				items.Add (item);
			}
		}
		
		itemsToTransmute = items;
		
		if (!IsTransmuting () && uidsString != null) {	
			IDictionary transmuteDictionary = GameManager._gameState.User.Transmute (items);
			string transactionId = transmuteDictionary ["uid"].ToString ();
			int skullLevel = Int32.Parse ((transmuteDictionary ["FinalItem"] as IDictionary) ["SkullLevel"].ToString ());
			string itemId = (transmuteDictionary ["FinalItem"] as IDictionary) ["itemId"].ToString ();
			int gems = System.Convert.ToInt32 (transmuteDictionary ["gCost"]);
			PurchaseManager.Instance.CommitTransaction (transactionId, false);
			
			PurchaseRequest.PurchaseRequestType requestType = PurchaseManager.Instance.PerformTransaction (new TransactionRequest (PurchaseRequest.PurchaseRequestType.Transmutation, transactionId, "", gems, 0, skullLevel, itemId));
			
			if (requestType == PurchaseRequest.PurchaseRequestType.Success) {
				InventorySystem.InventoryItem item = null;
				int a_count = itemsToTransmute.Count;
				for (int i = 0; i<a_count; i++) {
					item = itemsToTransmute [i] as InventorySystem.InventoryItem;
					GameManager._gameState.User._inventory.bag.Remove (item);
				}
				
				Init ();
				TransmuteStarted ();
				GameManager._gameState.User.transmutationDictionary = transmuteDictionary;
				GameManager._gameState.User.transmutationDictionary ["TimeRemaining"] = -1;
				UpdateTransmuteData (transmuteDictionary);
				
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Transaction to transmute: " + transactionId);
				ServerRequestParam requestParam = new ServerRequestParam (transactionId, "", skullLevel, itemId, 0);
				TransmutationRequest tRequest = new TransmutationRequest (ServerRequest.ServerRequestType.Transmutation, ProcessResponse, requestParam);
				ServerManager.Instance.ProcessRequest (tRequest);
//				GameManager.instance.SaveGameState(true);
				return true;
			} else {
				PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - GameManager._gameState.User._inventory.gems, "Complete Transmutation", "Buy Gems");
				return false;
			}
		} else {
			/*itemsToTransmute = new List<InventoryItem>();
			if(GameManager._gameState.User.transmutationDictionary.Contains("Type").Equals("Ring"))
			{
				IList baseItemsList = GameManager._gameState.User.transmutationDictionary["BaseItems"] as IList;
				for(int i = 0;i< baseItemsList;i++)
				{
					itemsToTransmute.Add(ItemRing.GetRingFromDictionary(baseItemsList[i] as IDictionary));
				}
			}*/
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("ALREADY TRANSMUTING!!!");
			
			IDictionary transmuteDictionary = GameManager._gameState.User.transmutationDictionary;
			string transactionId = transmuteDictionary ["uid"].ToString ();
			int skullLevel = Int32.Parse ((transmuteDictionary ["FinalItem"] as IDictionary) ["SkullLevel"].ToString ());
			string itemId = (transmuteDictionary ["FinalItem"] as IDictionary) ["itemId"].ToString ();
			
			ServerRequestParam requestParam = new ServerRequestParam (transactionId, "", skullLevel, itemId, 0);
			TransmutationRequest tRequest = new TransmutationRequest (ServerRequest.ServerRequestType.Transmutation, ProcessResponse, requestParam);
			ServerManager.Instance.ProcessRequest (tRequest);
			return false;
		}
	}
	
	public void ProcessResponse (ServerResponse response)
	{
		//GameManager._gameState.User._inventory.ProcessResponse(response);
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("TRANSMUTATION >> ProcessResponse(ServerResponse response) ::: " + MiniJSON.Json.Serialize (response) + "    TOSTR >> " + response.ToString ());
		TransmuteRespons tResponse = null;
		IDictionary transmutationDictionary = null;
		string transactionId = "";
		switch (response.Request.RequestType) {
		case ServerRequest.ServerRequestType.All:
			UpdateResponse uResponse = (UpdateResponse)response;
			if (IsTransmuting ()) { 		//Locally transmutation started, if Type exists!
				if (GameManager.PRINT_LOGS)
					Debug.Log ("ServerRequest.ServerRequestType.All::::::::::::: IsTransmuting() IS TRUE");
				GameManager._gameState.User.transmutationDictionary ["TimeRemaining"] = -1;
				if (uResponse.Transmutation != null) { //No information from server
					if (GameManager.PRINT_LOGS)
						Debug.Log ("got Response from SERVER!! YEEHAAA!!!!!!!!!!!!");
					transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
					transactionId = transmutationDictionary ["uid"].ToString ();
					if (uResponse.Transmutation.TransactionId.Equals (transactionId)) {
						GameManager._gameState.User.transmutationDictionary ["EndTime"] = uResponse.Transmutation.EndTime;
						GameManager._gameState.User.transmutationDictionary ["CurrentTime"] = uResponse.ServerCurrentTime;
						GameManager._gameState.User.transmutationDictionary ["TimeRemaining"] = uResponse.Transmutation.RemainingTime;
						if (uResponse.Transmutation.IsCompleted) {
							if (uResponse.Transmutation.Boost == 1) {
								PurchaseManager.Instance.CompleteTransaction (uResponse.Transmutation.BoostId, uResponse.Transmutation.TransactionId);
							} else {
								PurchaseManager.Instance.CompleteTransaction (uResponse.Transmutation.TransactionId, "");
							}
						}
					} else
						onStartTransmute (null);
				} else {
					onStartTransmute (null);
				}
				
				UpdateTransmuteData (GameManager._gameState.User.transmutationDictionary);
				if (transmuteUI != null)
					transmuteUI.SetUpdateData (GameManager._gameState.User.transmutationDictionary);
				
			}
			/* else
			{
				GameManager._gameState.User._inventory.ProcessResponse(response);
			} */
			//SetTransmuteData(GameManager._gameState.User.json);
			break;
			
		case ServerRequest.ServerRequestType.Transmutation: 
			if (!IsTransmuting ())
				return;
			
			tResponse = (TransmuteRespons)response;

			if (GameManager.PRINT_LOGS)
				Debug.Log (" :::::::::: ServerRequest.ServerRequestType.Transmutation ::::::::: ");
			transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
			if (transmutationDictionary != null)
				transactionId = transmutationDictionary ["uid"].ToString ();
				
			transmutationDictionary ["TimeRemaining"] = -1;
			UpdateTransmuteData (GameManager._gameState.User.transmutationDictionary);
				
			if (transmutationDictionary != null && tResponse.Transmutation.TransactionId.Equals (transactionId)) {
				if (GameManager.PRINT_LOGS)
					Debug.Log (" :::::::::: ServerRequest.ServerRequestType.Transmutation ::::::::: HERE ------------------------------     1");
				
				PurchaseManager.Instance.CommitTransaction (tResponse.Transmutation.TransactionId, tResponse.IsSuccess);
				
				if (tResponse.IsSuccess) {	
					GameManager._gameState.User.transmutationDictionary ["TimeRemaining"] = tResponse.Transmutation.RemainingTime;
					if (GameManager.PRINT_LOGS)
						Debug.Log (" :::::::::: ServerRequest.ServerRequestType.Transmutation ::::::::: HERE ------------------------------     2");
					//GameManager._gameState.User.transmutationDictionary = GameManager._gameState.User.Transmute(itemsToTransmute, tResponse.Transmutation.EndTime, tResponse.ServerCurrentTime, tResponse.Transmutation.RemainingTime);
					if (tResponse.Transmutation.IsCompleted) {
						if (tResponse.Transmutation.Boost == 1) {
							Analytics.logEvent (Analytics.EventName.Transmute_Boost);
							PurchaseManager.Instance.CompleteTransaction (tResponse.Transmutation.BoostId, tResponse.Transmutation.TransactionId);
						} else {
							Analytics.logEvent (Analytics.EventName.Transmute_Start);
							PurchaseManager.Instance.CompleteTransaction (tResponse.Transmutation.TransactionId, "");
						}
						ProcessCollectTransmute ();
					}
						
					UpdateTransmuteData (GameManager._gameState.User.transmutationDictionary);
					transmuteUI.SetUpdateData (GameManager._gameState.User.transmutationDictionary);	
				} else {
					if (GameManager.PRINT_LOGS)
						Debug.Log ("Can't proceed - you need to connect with server!");
				}
			}
			break;
		case ServerRequest.ServerRequestType.TransmutationCompleted:
			tResponse = (TransmuteRespons)response;
			
			Debug.Log ("tResponse>>" + tResponse.ToString ());
			transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
			if (IsTransmuting ()) {
				transactionId = transmutationDictionary ["uid"].ToString ();
				Debug.Log ("transmutationDictionary[\"uid\"].ToString() >> " + transactionId);
				Debug.Log ("transmutationDictionary[\"uid\"].ToString() >> " + transmutationDictionary ["Boostuid"].ToString ());
			}
			if (IsTransmuting () && transmutationDictionary != null && tResponse != null && tResponse.Transmutation != null && tResponse.Transmutation.TransactionId.Equals (transactionId)) {
				if (tResponse.Transmutation.Boost == 1) {
					PurchaseManager.Instance.CommitTransaction (tResponse.Transmutation.BoostId, tResponse.IsSuccess);
				}
				if (tResponse.IsSuccess && tResponse.Transmutation.IsCompleted) {
					if (tResponse.Transmutation.Boost == 1) {
						PurchaseManager.Instance.CompleteTransaction (tResponse.Transmutation.BoostId, tResponse.Transmutation.TransactionId);
					} else {
						PurchaseManager.Instance.CompleteTransaction (tResponse.Transmutation.TransactionId, "");
					}
				}
			}
			break;
		}
	}
	
	private void UpdateTransmuteData (IDictionary userDictionary)
	{
/*		Value val=new Value(MiniJSON.Json.Serialize(userDictionary),MovieID);
		Value[] args1={val};
		if(tempSwf != null)
		{
			tempSwf.Invoke("updateTransmuteData",args1);
		}
		*/
	}
	public void ProcessCollectTransmute ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("PROCESS COLLECTION OF TRANSMUTED ITEM!!!");
		/*		
 		Value[] args = {};
		if(tempSwf != null)
		{
			tempSwf.Invoke("onCollectRing", args);
		}
		*/
		string transmutationType = GameManager._gameState.User.transmutationDictionary ["Type"].ToString ();
		string ringTransmutationString = InventorySystem.InventoryItem.Type.kRing.ToString ();
		if (transmutationType.Equals (ringTransmutationString)) {
			InventorySystem.ItemRing itemRing = InventorySystem.InventoryLoader.GetRingObjectFromJsonData (GameManager._gameState.User.transmutationDictionary ["FinalItem"] as IDictionary);
			itemRing.isNew = true;
			ShowLootRingPopup (itemRing, true);
			GameManager._gameState.User._inventory.bag.Add (itemRing);
		} else {
			InventorySystem.ItemRune itemRune = InventorySystem.InventoryLoader.GetRuneObjectFromJsonData (GameManager._gameState.User.transmutationDictionary ["FinalItem"] as IDictionary);
			ShowLootRunePopup (itemRune, true);
			GameManager._gameState.User._inventory.bag.Add (itemRune);
		}
		
		GameManager._gameState.User.transmutationDictionary = GameManager._gameState.User.emptyTransmutationDictionary; //check
		GameManager.instance.SaveGameState (false);
		Analytics.logEvent (Analytics.EventName.Transmute_Collect);
		
		transmuteUI.onTransmuteComplete ();
	}
	
	
	public bool onFinishButton () //Callback when finish button on transmute is hit
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf != null) {

			GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.closeUIScreen ("TransmutationTutorialEnd");
			GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.onTransmutationTutorialEnd ();

		}
		if (GameManager.PRINT_LOGS)
			Debug.Log ("On TRANSMUTE FINISH BUTT-on!!!");
		IDictionary currentTransmutationDictionary = GameManager._gameState.User.LoadTransmutationProgress (GameManager._gameState.User.transmutationDictionary);
		string transactionId = currentTransmutationDictionary ["uid"].ToString ();
		string boostId = currentTransmutationDictionary ["Boostuid"].ToString ();
		int skullLevel = Int32.Parse ((currentTransmutationDictionary ["FinalItem"] as IDictionary) ["SkullLevel"].ToString ());
		string itemId = (currentTransmutationDictionary ["FinalItem"] as IDictionary) ["itemId"].ToString ();
		int gems = Int32.Parse (currentTransmutationDictionary ["BoostCost"].ToString ());
		if (TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.TransmutationTutorialStart && GameManager._gameState.User._inventory.gems < gems)
			GameManager._gameState.User._inventory.gems = gems;
		PurchaseRequest.PurchaseRequestType requestType = PurchaseManager.Instance.PerformTransaction (new TransactionRequest (PurchaseRequest.PurchaseRequestType.TransmutationBoost, boostId, transactionId, gems, 0, skullLevel, itemId));
		if (requestType == PurchaseRequest.PurchaseRequestType.Success) {
			Init ();
			//if(PurchaseManager.Instance.IsParentCommited(transactionId))
			//{	
			TransmutateCompleted (ServerRequest.ServerRequestType.TransmutationBoost);
			//}
			//else
			//{
			PurchaseManager.Instance.CommitTransaction (boostId, false);
			// Show error cann't boost transmutation because transmutation timer dosen't exist on server.
			//}
			return true;
		} else {
			PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - GameManager._gameState.User._inventory.gems, "Boost Transmutation", "Buy Gems");
			return false;
		}
	}
	
	public void onPlaceItem ()
	{
		// Transmute Tutorial - start
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf != null) {
			
			if (GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount == 0) {
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.closeUIScreen ("TransmutationTutorial2");
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount++;
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.ShowTransmutationTutorial3 ();
			} else if (GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount == 1) {
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.closeUIScreen ("TransmutationTutorial3");
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.swapCount++;
				GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf.ShowTransmutationTutorial4 ();
			}
		}
		// Transmute Tutorial - end
	}
	/// <summary>
	/// Called When an item is dragged to a slot...
	/// </param>
	public void onUpdateTransmute (string uidsString)
	{	
		/*
		if(GameManager.PRINT_LOGS) Debug.Log("ON UPDATE TRANSMUTE ::::::::: CALLED FROM SCALEFORM!!!");
		string[] uids = uidsString.Split(',');
		
		List<InventorySystem.InventoryItem> itemsList = new List<InventorySystem.InventoryItem>();
		string uid = "";
		int length = uids.Length;
		for(int i = 0; i < length; i++)
		{
			uid = uids[i];
			itemsList.Add(GameManager._gameState.User._inventory.GetBagItemForUID(uid));
		}
		if(length > 1)
		{
			//if(GameManager.PRINT_LOGS) Debug.Log("UPDATE TRANSMUTE :::::::: "+MiniJSON.Json.Serialize(GameManager._gameState.User.Transmute(itemsList)));
			Value val=new Value(MiniJSON.Json.Serialize(GameManager._gameState.User.Transmute(itemsList)),MovieID);
			Value[] args1={val};
			
			tempSwf.Invoke("updateTransmuteData",args1);
		}
		*/
	}
	
	private void TransmutateCompleted (ServerRequest.ServerRequestType requestType)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log (":::::::: TRANSMUTE COMPLETE ::::::::: ");
		
		if (GameManager._gameState.User.transmutationDictionary ["Type"] == null)
			return;
		
		IDictionary transmutationDictionary = GameManager._gameState.User.transmutationDictionary;
		string transactionId = transmutationDictionary ["uid"].ToString ();
		int skullLevel = Int32.Parse ((transmutationDictionary ["FinalItem"] as IDictionary) ["SkullLevel"].ToString ());
		string itemId = (transmutationDictionary ["FinalItem"] as IDictionary) ["itemId"].ToString ();
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Transaction to collect: " + transactionId);
		int boost = 0;
		string boostUid = "";
		
		if (requestType == ServerRequest.ServerRequestType.TransmutationBoost) {
			boost = 1;
			boostUid = transmutationDictionary ["Boostuid"].ToString ();
			
			PurchaseManager.Instance.CommitTransaction (boostUid, false);
			PurchaseManager.Instance.CompleteTransaction (boostUid, transactionId);
			
			PurchaseManager.Instance.MarkCommittedAndCompleteTrue (transactionId);
			//PurchaseManager._instance.MarkCommittedAndCompleteTrue(boostUid);
			//PurchaseManager.Instance.CommitTransaction(boostUid, true); //No need to confirm its commit with server...
		} else { //In case of transmutation timer end...
			PurchaseManager.Instance.CommitTransaction (transactionId, false);
			PurchaseManager.Instance.CompleteTransaction (transactionId, null);
		}
		
		ProcessCollectTransmute ();
		
		ServerRequestParam requestParam = new ServerRequestParam (transactionId, boostUid, skullLevel, itemId, boost);
		ServerRequest tRequest = new TransmutationRequest (ServerRequest.ServerRequestType.TransmutationCompleted, ProcessResponse, requestParam);
		ServerManager.Instance.ProcessRequest (tRequest);
	}
	
	public void OnTransmuteComplete ()
	{
		TransmutateCompleted (ServerRequest.ServerRequestType.TransmutationCompleted);
		//generalSwf.Invoke("setInventoryData", GameManager._gameState.User.json);
	}
	
	public void onOptionButton ()
	{// options swf loaded callback
		SetOptionsConfig ();
		generalSwf.OpenOptionsUI ().setInterface (this);
		Analytics.logEvent (Analytics.EventName.PauseMenu_Options);
	}
	
	public void GameUserAndVersion (string name, string version)
	{
		/*
		Value val1 = new Value(name,MovieID);
		Value val2 = new Value(version,MovieID);
		Value[] args = {val1,val2};
		generalSwf.Invoke("gameUserAndVersion",args);
		*/

		generalSwf.gameUserAndVersion (name, version);
	}
	
	public void SetOptionsConfig ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("options config set!");
		setMVolume ((double)SoundManager.instance.musicVolume);
		setSfxVolume ((double)SoundManager.instance.sfxVolume);
		/*
		if(FB.IsLoggedIn)
			GameUserAndVersion(username,GameManager.APP_VERSION);
		else
			GameUserAndVersion(PlayerPrefs.GetString("networking-userID"),GameManager.APP_VERSION);
		*/
	}
	
	public void onBoostButton ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Boost button pressed CB!!!");
		if (tempMenuName.Equals ("Spirits"))
			GameManager._gameState.User.BoostSpiritUpgrade ();
		else
			GameManager._gameState.User._inventory.BoostUpgrade ();
	}
	
	public void BoostResult (bool boostSuccess)
	{
/*		if(tempSwf != null)
		{
			Value val1=new Value(boostSuccess,MovieID);		
			Value[] args={val1};
			
			tempSwf.Invoke("BoostResult",args);
		}
		*/
	}
	
	public void SetUpdateData (string jsonString, bool isUpdate)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("JSON SENT TO UPGRADES::::" + jsonString + " ISuPDATE ::: " + isUpdate);
/*		Value val=new Value(jsonString,MovieID);
		
		Value[] args1={val};
		if(tempSwf != null)
		{
			if(isUpdate)
			{
				tempSwf.Invoke("updateUpgradeData",args1);
			}
			else
			{
				tempSwf.Invoke("setInventoryData",args1);
			}
		}
		*/
		
		if (isUpdate && tempMenuName == "Upgrades") {
			if (upgradeUI != null) {
				upgradeUI.SetUpdateData (jsonString);
			}
		}
	}
	
	private string petUpgradedId;
	public bool onPetStartUpgrade (string newPetId)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UIGeneralSWF - onPetStartUpgrade");
		Debug.LogError ("UIGeneralSWF - onPetStartUpgrade id = "+newPetId);
		petUpgradedId = newPetId;
		return GameManager._gameState.User.OnPetStartUpgrade (newPetId);
	}

	//CB when start upgrade button is pressed on upgrades ui
	public void onStartUpgrade (string upgradeItemId)
	{
		Debug.Log ("onStartUpgrade - upgradeItemId" + upgradeItemId);
		if (tempMenuName.Equals ("Spirits")) {
			onPetStartUpgrade (upgradeItemId);
		} else {
			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.UpgradeTutorialCompleted)
				&& GameManager.instance.scaleformCamera.uiUpgradeTutorialSwf != null) {
				TutorialManager.instance.AddTutorialIdWithStatus (TutorialManager.TutorialsAndCallback.UpgradeTutorialCompleted, true);
				GameManager.instance.SaveGameState (false);
				Analytics.logEvent (Analytics.EventName.UpgradesUI_Tutorial_End);
			}
			
			GameManager._gameState.User._inventory.OnStartUpgrade (upgradeItemId);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("ITEM ID FROM UPGRADE......" + upgradeItemId);
		}
	}
	
	public void onReset () //Called when an upgrade ends...
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onReset - Called + ------------- tempMenuName = " + tempMenuName);
		
		if (tempMenuName.Equals ("Spirits"))
			GameManager._gameState.User.OnSpiritUpgradeComplete ();
		else {
			GameManager._gameState.User._inventory.onUpgradeComplete (ServerRequest.ServerRequestType.Upgrade);
		}
	}
	
	public void closeLoadedUI ()
	{
		
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)
			&& tempMenuName == "Inventory") {
			if (GameManager.instance.scaleformCamera.uiRingTutorialSwf.ringTutorialatEnd) {
				TutorialManager.instance.AddTutorialIdWithStatus (TutorialManager.TutorialsAndCallback.RingTutorialCompleted, true);
				Analytics.logEvent (Analytics.EventName.RingUI_Tutorial_End);
			}
			TutorialManager.instance.DestroyRingTutorial ();
		}
		
		
		//generalSwf.GetDisplayInfo().Visible=false;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("CLOSE UI CALLED for --- >>> " + tempMenuName);
		this.UnloadSwf ();
		if (GameManager.PRINT_LOGS)
			Debug.Log ("USER INVENTORY BEFORE SAVE --->>> " + GameManager._gameState.User._inventory.json);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SAVE REQUEST SENT! _ CLOSE UI");
		
		GameManager._gameState.musicVolume = SoundManager.instance.musicVolume;
		GameManager._gameState.gfxVolume = SoundManager.instance.sfxVolume;
		
		generalSwf.CloseLoadedUIs ();
		
//		GameManager.instance.SaveGameState(true);
		Init ();
	}
	
	
	public void onUseHealthPotion () //farhan
	{
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("TIME TO USE HEALTH POTION!!! SCENE_GENERALLLLLLLLLLLLLLLL...");
		
		if (GameManager._gameState.User.UsePotion (InventorySystem.InventoryItem.Type.kHealthPotion)) {
			SoundManager.instance.PlayMenuOkSound ();
			GameManager._gameState.User.SetLifeToFull ();
			this.UpdateHealth (GameManager._gameState.User.life);
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("SAVE REQUEST SENT! _ POTION");
			SRCharacterController sorcerrerController = GameManager.instance._levelManager.player.GetComponent<SRCharacterController> ();
			//sorcerrerController.StopHeartBeatSound();
			sorcerrerController.StopBreathingSound ();
			GameManager.instance.SaveGameState (false);	
			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.PotionTutorialCompleted)) {
				if (TutorialManager.instance.currentTutorial.ToString ().ToLower ().Contains ("potion"))
					TutorialManager.instance.PotionTutorialEnd ();
			} 

		} else {
			SoundManager.instance.PlayMenuCancelSound ();
		}

	}
	
	
	
	public void UpdateHealth ()
	{
		/* OLD CODE
		GameManager.instance.scaleformCamera.levelScene.UpdateHealth(GameManager._gameState.User.life);
		*/
		
		UpdateHealth (GameManager._gameState.User.life);
		
		
	}
	
	public void UpdateHealth (int health)
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("USER TOTAL HEALTH >>>>>" + GameManager._gameState.User.totalLife + "Heath sent ::::::::" + health);
//		Value val=new Value(health,MovieID);
//		Value val2;

		bool playHeartAnimation;
		if (health <= GameManager._gameState.User.totalLife * 0.3f) {
			playHeartAnimation = true;
//			val2 = new Value(true,MovieID); // passing true to animate heart beat
			//	if(GameManager.instance._levelManager.isPlayerNeutral){
//			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.PotionTutorialCompleted)){
//				GameManager.instance.scaleformCamera.loadUIPotionTutorial();
//			}
			//		GameManager.instance.scaleformCamera.isPaused = true;
			//	}
		} else {
			playHeartAnimation = false;
//			val2 = new Value(false,MovieID); // passing false to stop animating heart beat
		}
//		Value[] args1={val,val2};
//		generalSwf.Invoke("updateHealth",args1);
		//	Value[] args2={};

		generalSwf.updateHealth (health, playHeartAnimation);
		GameManager._gameState.User.SetRingsAndPetStats ();
	}
	
	public void UpdateSoulGems (int soulGems)
	{
		/*
		Value val=new Value(soulGems,MovieID);
		Value[] args1={val};
		generalSwf.Invoke("updateSoulGems",args1);
		*/
		generalSwf.updateSoulGems (soulGems);
	}
	
	public void UpdateSoulDust (float soulDust)
	{
		soulDust = Mathf.Floor (soulDust);
		/*
		Value val=new Value(soulDust,MovieID);
		Value[] args1={val};
		generalSwf.Invoke("updateSoulDust",args1);
		*/
		generalSwf.updateSoulDust (soulDust);

	}
	
	public void ShowLootPopup (int itemDamage, int itemWards)
	{
		/*
		Value val1=new Value(itemDamage,MovieID);
		Value val2=new Value(itemWards,MovieID);
		Value[] args={val1,val2};
		generalSwf.Invoke("showLootPopup",args);
		*/
		generalSwf.showLootPopup (itemDamage, itemWards);
	}
	
	public void lootPopupClosed ()
	{
		InputWrapper.disableTouch = false;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("POPUP CLOSED --->>> " + tempMenuName);
		if (tempMenuName.Equals ("BattleLootPopUp")) {
			GameManager.instance._levelManager.battleManager.scaleformBattleEnded = true;
			Time.timeScale = 1.0f;
			sfCamera.isPaused = false;
		} else {
			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted) && GameManager.instance._levelManager.currentLevel.Equals ("StatuePath")) {
				TutorialManager.instance.ArcaneKeysEnd ();
//					GameManager.instance.SaveGameState(false);
			}
			Time.timeScale = 1.0f;
			sfCamera.isPaused = false;
		}
		tempMenuName = "";
		InputWrapper.disableTouch = false;
		//SoundManager.instance.PlayMenuCancelSound();
	}
	
	public void ShowNeedKeysPopUp (int availableKeys, float price, int requiredKeys)
	{
		generalSwf.showKeysPopup (availableKeys, price, requiredKeys);
		SoundManager.instance.PlayMenuOpenSound ();
	}
	
	public void onKeyPopUp (int availableKeys, int price)
	{
		generalSwf.showKeysPopup (availableKeys, price);
		SoundManager.instance.PlayMenuOpenSound ();
		InputWrapper.disableTouch = true;
	}
	
	public void ShowUiGeneralPopup (string heading, string message)
	{
		Debug.Log ("ShowUiGeneralPopup --- tempPopUpOpened >> " + tempPopUpOpened);
		
		if (!tempPopUpOpened) {
			tempPopUpOpened = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log (":::::::::: SHOW POP-UP CALLED ::::::::::: + " + heading + message);
			//tempMenuName="GeneralPopUp";
			generalSwf.showUiGeneralPopup (heading, message);
			SoundManager.instance.PlayMenuOpenSound ();
		}
	}
	
	public void ShowGeneralPopup2 (string title, string message, string btn1Text, string btn2Text)
	{
		Debug.Log ("ShowGeneralPopup2 --- tempPopUpOpened >> " + tempPopUpOpened);
		
		if (!tempPopUpOpened) {
			tempPopUpOpened = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log (":::::::::: SHOW POP-UP CALLED ::::::::::: " + title + " " + message);
			//tempMenuName="GeneralPopUp";
			generalSwf.showGeneralPopup2 (title, message, btn1Text, btn2Text);
			SoundManager.instance.PlayMenuOpenSound ();
		}
	}
	
	public void ShowGeneralPopup2 (string title, int numberVal, string message, string btn1Text, string btn2Text)
	{
		Debug.Log ("ShowGeneralPopup2 --- tempPopUpOpened 5 param >> " + tempPopUpOpened);
		
		if (!tempPopUpOpened) {
			tempPopUpOpened = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log (":::::::::: SHOW POP-UP CALLED ::::::::::: " + title + " " + message);
			//tempMenuName="GeneralPopUp";
			generalSwf.showGeneralPopup2 (title, numberVal, message, btn1Text, btn2Text);
			SoundManager.instance.PlayMenuOpenSound ();
		}
	}
	
	
	public void GeneralPopupButton1Pressed ()
	{
		Debug.Log ("GeneralPopupButton1Pressed... PurchaseManager.Instance.currentType > " + PurchaseManager.Instance.currentType);
		ClearBattleEndLoot ();
			
		tempPopUpOpened = false;
		switch (PurchaseManager.Instance.currentType) {
		case PurchaseManager.GeneralPopupType.InsufficientSouls:
			if (PurchaseManager.Instance.LastPurcahseRequest == null)
				return;
			switch (PurchaseManager.Instance.LastPurcahseRequest.RequestType) {
			case PurchaseRequest.PurchaseRequestType.Ring:
			case PurchaseRequest.PurchaseRequestType.Rune:
				PurchaseManager.Instance.LastPurcahseRequest.IsAutoBuy = true;
				PurchaseManager.Instance.PerformPurchase (PurchaseManager.Instance.LastPurcahseRequest);
				break;
			case PurchaseRequest.PurchaseRequestType.BagUpgrade:
				Debug.Log ("case PurchaseRequest.PurchaseRequestType.BagUpgrade:");
				onStartUpgrade (GameManager._gameState.User._inventory.bag.id);
				break;
			case PurchaseRequest.PurchaseRequestType.KeyRingUpgrade:
				Debug.Log ("case PurchaseRequest.PurchaseRequestType.KeyRingUpgrade:");
				onStartUpgrade (GameManager._gameState.User._inventory.keyRing.id);
				break;
			case PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade:
				Debug.Log ("case PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade:");
				onStartUpgrade (GameManager._gameState.User._inventory.transmutationCube.id);
				break;
			case PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade:
				Debug.Log ("case PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade:");
				onStartUpgrade (GameManager._gameState.User._inventory.potionBelt.id);
				break;
			case PurchaseRequest.PurchaseRequestType.PetUpgrade:
				onStartUpgrade (petUpgradedId);
				break;
			}
			break;
		case PurchaseManager.GeneralPopupType.BagUpgrade:
		case PurchaseManager.GeneralPopupType.BagUpgradeOrSell:
		case PurchaseManager.GeneralPopupType.KeyRingUpgrade:
		case PurchaseManager.GeneralPopupType.PotionBeltUpgrade:
//			Debug.Log ("UI_GeneralSWF >> Here... :/");
			if(GameManager._gameState.dayCount >= GameManager.UNLOCK_UPGRADES_DAY) {
//				Debug.LogError("Opening upgrade UI");
				onUpgradeButton (PurchaseManager.Instance.currentType);
			} else {
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Locked!" , string.Format("Upgrades unlock on day {0}!", GameManager.UNLOCK_UPGRADES_DAY),() =>{});
			}
			break;
		case PurchaseManager.GeneralPopupType.InsufficientGems:
			onMarketButton ();
			break;
		case PurchaseManager.GeneralPopupType.InsufficientSoulsForGuild:
//			sfCamera.guildUI.GuildCreateFromTemp ();
			ClanUIHandler.instance.OnCreateButtonClicked();
			break;
		case PurchaseManager.GeneralPopupType.InsufficientSoulsForJoiningGuild:
//			sfCamera.guildUI.JoinGuildFromTemp ();
			break;
		}
		petUpgradedId = null;
		InputWrapper.disableTouch = false;
	}
	
	private List<string> missingRingTags = new List<string> ();
	public void SetMissingRingTags (List<string> missingRingTags)
	{
		this.missingRingTags = missingRingTags;
	}
	
	public void ShowGeneralPopup3 (string title, string message, string btn1Text, string btn2Text)
	{
		if (!tempPopUpOpened) {
			tempPopUpOpened = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log (":::::::::: SHOW POP-UP CALLED ::::::::::: " + title + " " + message);
			//tempMenuName="GeneralPopUp";
			generalSwf.showGeneralPopup3 (title, message, btn1Text, btn2Text);
			SoundManager.instance.PlayMenuOpenSound ();
			//pauseGame();
		}
	}
	
	public void userLoginState (bool facebook, bool gamecenter, bool googleplus)
	{
		/*
		Value val1 = new Value(facebook,MovieID);
		Value val2 = new Value(gamecenter,MovieID);
		Value val3 = new Value(googleplus,MovieID);
		Value[] args = {val1, val2, val3};
		generalSwf.Invoke("userloginState", args);
		*/
		generalSwf.userloginState (facebook, gamecenter, googleplus);
	}
	
	
	public void GeneralPopup3Button1Pressed ()
	{
		Debug.Log ("GeneralPopup3Button1Pressed");
		
		tempPopUpOpened = false;
		if (SocialMediaManager.Instance.retrieveOldGameState) {
			
		} else {
			//OpenRingsUi();
			onRingsButton ();
		}
		//closeLoadedUI();
		
	}
	public void GeneralPopup3Button2Pressed ()
	{
		Debug.Log ("GeneralPopup3Button2Pressed");
		
		if (SocialMediaManager.Instance.retrieveOldGameState) {
			SocialMediaManager.Instance.retrieveOldGameState = false;
		} else {
			tempPopUpOpened = false;
			GameManager.instance._levelManager.TransitionToBattle (Convert.ToInt32 (GameManager.instance._levelManager.lastCollidedTag));
			resumeGame ();
		}
	}
	
	public void OpenMarketUi ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("OpenMarketUi",args);
		*/
		generalSwf.OpenMarketUi ();
	}
	
	public void OpenUpgradeUi ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("OpenUpgradeUi",args);
		*/
		onUpgradeButton (PurchaseManager.GeneralPopupType.BagUpgrade);
	}
	public void OpenRingsUi ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("OpenRingsUi",args);
		*/
		//generalSwf.OpenRingsUi();
		
		onRingsButton ();
		
		//GameManager._gameState.User._inventory.MarkRingsAsNotNew();
	}
	
	public void OpenPetUi ()
	{
		if (generalSwf != null) {
			/*
			Value[] args={};
			generalSwf.Invoke("OpenPetUi",args);
			*/
			generalSwf.OpenPetUi ();
		}
	}
		
	public void GeneralPopupButton2Pressed ()
	{
		UI_Guild.TempGuildParams.Clear ();
		UI_Guild.TempJoinGuild.Clear ();
		
		tempPopUpOpened = false;
		
		ClearBattleEndLoot ();
		
		/*if(tempMenuName.Equals("BattleLootPopUp") || tempMenuName.Equals("VictoryPopUp"))
		resumeGame();*/
		PurchaseManager.Instance.LastPurcahseRequest = null;
		resumeGame ();
		
	}
	
	private void ClearBattleEndLoot () //farhan
	{
		if (tempMenuName.Equals ("BattleLootPopUp")) {
			GameManager.instance._levelManager.battleManager.scaleformBattleEnded = true;
			Time.timeScale = 1.0f;
			sfCamera.isPaused = false;
			tempMenuName = "";
			resumeGame ();
			GameManager.instance.scaleformCamera.DestroyBattleHUD ();
			//GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(true);
			//ToggleTopStats(true);
		}
	}
	
	public void ShowGeneralPopup (string heading, string message)
	{
		if (!tempPopUpOpened) {
			tempPopUpOpened = true;
			InputWrapper.disableTouch = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log (":::::::::: SHOW POP-UP CALLED ::::::::::: + " + heading + message);
			//tempMenuName="GeneralPopUp";
			generalSwf.showGeneralPopup (heading, message);
			SoundManager.instance.PlayMenuOpenSound ();
		}
	}
	
	System.Action popupCBAction;
	public void ShowGeneralPopup (string heading, string message, System.Action cbAction)
	{
		popupCBAction = cbAction;
		ShowGeneralPopup (heading, message);
	}
	
	public void onClosePopup ()
	{
		InputWrapper.disableTouch = false;
		tempPopUpOpened = false;
		if (GameManager.PRINT_LOGS)
			Debug.Log (":::::::::: GENERAL POP-UP CLOSED :::::::::::");
		if (tempMenuName.Equals ("BattleLootPopUp")) {
			GameManager.instance._levelManager.battleManager.scaleformBattleEnded = true;
			Time.timeScale = 1.0f;
			sfCamera.isPaused = false;
			tempMenuName = "";
		} else {
			Time.timeScale = 1.0f;
			sfCamera.isPaused = false;
			if (GameManager.instance.isMultiPlayerMode) {
				//MultiPlayerLevelManager levelManager = (MultiPlayerLevelManager)GameManager.instance._levelManager;
				if (popupCBAction != null) {
					popupCBAction ();
					popupCBAction = null;
				} else
					GameManager.instance._levelManager.GoBackToMainMenu ();
			}
		}
		//SoundManager.instance.PlayMenuCancelSound();
	}
	
	public void ShowVictoryPopup (int soulsWon, int healthLost, int battleTime, int topDamage, string grade)
	{
		tempMenuName = "VictoryPopUp";
		
		generalSwf.showVictoryPopup (soulsWon, healthLost, battleTime, topDamage, grade);
	}
	
	public void LoadLoot (InventorySystem.InventoryItem currentLoot, bool isAnchored = false)
	{
		Debug.Log ("(tempMenuName >> " + tempMenuName + " +currentLoot >> " + currentLoot.ToString ());
		string errorMessage = "";
		
		if (LootManager.processLoot (currentLoot, out errorMessage)) {
			if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kHealthPotion) {
				
				generalSwf.showLootPotionPopup (1);
				
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kHealthPotion");

				SoundManager.instance.PlayMenuOpenSound ();
			} else if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kRing) {
				InventorySystem.ItemRing lootRing = currentLoot as InventorySystem.ItemRing;
				
				if (currentLoot == null)
					throw new System.Exception ("Item loot type ring is invalid");
				
				string tempJson = MiniJSON.Json.Serialize (lootRing.ToDictionary ());
				if (GameManager.PRINT_LOGS)
					Debug.Log ("RingJSONNN===" + tempJson);
				ShowLootRingPopup (lootRing, false);
				
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kRing");
				SoundManager.instance.PlayMenuOpenSound ();
			} else if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kSoulGem) {
				if (currentLoot == null)
					throw new System.Exception ("Item loot type key is invalid");
				InventorySystem.ItemGem tempGemBag = (InventorySystem.ItemGem)currentLoot;
				ShowLootGemsPopUp (tempGemBag.gemValue);
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kSoulGem");
				SoundManager.instance.PlayMenuOpenSound ();
			} else if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kRune) {
				InventorySystem.ItemRune lootRune = currentLoot as InventorySystem.ItemRune;
				
				if (currentLoot == null)
					throw new System.Exception ("Item loot type rune is invalid");
				string tempJson = MiniJSON.Json.Serialize (lootRune.ToDictionary ());
				if (GameManager.PRINT_LOGS)
					Debug.Log ("RuneJSONNN===" + tempJson);
				ShowLootRunePopup (lootRune, false);
				
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kRune");
				SoundManager.instance.PlayMenuOpenSound ();
			} else if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kKey) {
				if (currentLoot == null)
					throw new System.Exception ("Item loot type key is invalid");
				
				ShowLootKeysPopup (1, isAnchored);
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kKey");
				SoundManager.instance.PlayMenuOpenSound ();
			} else if (currentLoot.ItemType () == InventorySystem.InventoryItem.Type.kSoulBag) {
				if (currentLoot == null)
					throw new System.Exception ("Item loot type soulBag is invalid");
			
			
				InventorySystem.ItemSoulBag tempBag = (InventorySystem.ItemSoulBag)currentLoot;
				ShowLootSoulBagPopUp (tempBag.soulValue);
				
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "kSoulBag");
				SoundManager.instance.PlayMenuOpenSound ();
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log (tempMenuName + "Error");
				//ShowGeneralPopup("Error Victory Continue","Error While Showing Loot Pop Up");
			}
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log (tempMenuName + "Message");
			//ShowGeneralPopup("Message","You Just Missed the Loot,Please Make Space For New Items In Your Bag/PotionBelt");
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.BagUpgrade;
			ShowGeneralPopup2 ("Upgrade Required", errorMessage, "Upgrade", "Later");
		}
	}
	
	
	
	public void ShowLootRingPopup (InventorySystem.ItemRing ring, bool isTransmute)
	{
		if (isTransmute) {
			generalSwf.showLootRingPopup (ring, isTransmute);
		} else {
			/*
			Value[] args={val1,val2,val4,val3};
			generalSwf.Invoke("showLootRingPopup",args);
			*/
			generalSwf.showLootRingPopup (ring);
		}		
	}
	
	
	public void ShowLootRunePopup (InventorySystem.ItemRune rune, bool isTransmute)
	{

		if (isTransmute) {
			generalSwf.showLootRunePopup (rune, isTransmute);
		} else {
			/*
			Value[] args={val1};
			generalSwf.Invoke("showLootRunePopup",args)
			*/
			generalSwf.showLootRunePopup (rune);
		}
		
	}
	
	public void ShowLootKeysPopup (int keys, bool isAnchored = false)
	{
		generalSwf.showLootKeysPopup (keys, isAnchored);
	}
	
	public void ShowLootGemsPopup (int gems)
	{
		generalSwf.showLootGemsPopup (gems);
	}
	
	public void ShowLootSoulBagPopUp (int soulValue)
	{
		generalSwf.showSoulPopup (soulValue);
	}
	
	public void ShowLootGemsPopUp (int gemValue)
	{
		generalSwf.showLootGemsPopup (gemValue);
	}
	
	
	public void showShardPopup (int shardCount)
	{
		generalSwf.showShardPopup (shardCount);
		
	}
	public void onVictoryContinue () //on victory popup closed
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onVictoryContinue");
		int currentSkullLevelDustValue = 0;
		int dustskullLevel = GameManager._gameState.skullLevel;
		if (!GameManager.instance.isMultiPlayerMode) {
			string aiName = GameManager.instance.getEnemyFromGameState (GameManager.instance._levelManager.currentLevel + "_DuelA");
			if (GameManager._gameState.bossAttemptDictionary.Contains (aiName)) {
				dustskullLevel = GameManager._dataBank.GetModelForMonsterByEncounter (aiName, Int32.Parse (GameManager._gameState.bossAttemptDictionary [aiName].ToString ())).skullLevel;
			}
		}
		
		Buff greedBuff = GameManager._gameState.User.GetBuffForBuffName ("greed");

		int petGreedBuffDustValue = 0;
//		PetModel petModel = new PetModel(GameManager._gameState.User.spiritId);
//		if(petModel.abilityStat == "GREED") {
//			petGreedBuffDustValue = Mathf.CeilToInt ((((float)petModel.buffTime) / 100 * LootManager.getSoulDust(dustskullLevel)));
//		}

		if (greedBuff == null) //earth has greed
			currentSkullLevelDustValue = LootManager.getSoulDust (dustskullLevel);
		else {
			currentSkullLevelDustValue = LootManager.getSoulDust (dustskullLevel);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("currentSkullLevelDustValue" + currentSkullLevelDustValue);
//			currentSkullLevelDustValue += (int)greedBuff.modifierValue;
			currentSkullLevelDustValue += Mathf.CeilToInt(greedBuff.modifierValue / 100 * currentSkullLevelDustValue);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("currentSkullLevelDustValue += (int)greedBuff.modifierValue" + currentSkullLevelDustValue);
		}
		currentSkullLevelDustValue += petGreedBuffDustValue;

		GameManager._gameState.User._inventory.souls += currentSkullLevelDustValue;
		
		if (sfCamera.showRingShredsPopUp) {
			showShardPopup (GameManager._gameState.ringShards);
			tempMenuName = "BattleLootPopUp";
		} else {
			if (LootManager.isLoot ("Duel", GameManager._gameState.skullLevel)) {
				tempMenuName = "BattleLootPopUp";
				InventorySystem.InventoryItem currentLoot = LootManager.DecideLoot ("Duel", GameManager.instance._levelManager._zone, "");
				if (currentLoot as InventorySystem.ItemRing != null)
					((InventorySystem.ItemRing)currentLoot).isNew = true;
				LoadLoot (currentLoot, true);
				
				battleEndLoot = true;
				//GameManager.instance._levelManager.battleManager.scaleformBattleEnded=true;
			} else {
				tempMenuName = "";
				if (!GameManager.instance.isMultiPlayerMode) {
					GameManager.instance._levelManager.battleManager.scaleformBattleEnded = true;
				}
				Time.timeScale = 1.0f;
				sfCamera.isPaused = false;
				
			}
		}
		GameManager.instance.scaleformCamera.generalSwf.UpdateSoulDust (GameManager._gameState.User._inventory.souls);
//		if(UIManager.instance.guildUI != null) {
//			NGUITools.SetActive(UIManager.instance.guildUI.gameObject, true);
//		}
		UIManager.instance.LoadGuildUI();
	}
	
	private bool battleEndLoot = false;
	
	public void bossDefeatPopupClosed ()
	{
		onDefeatReturn ();
	}
	public void ShowDefeatPopup ()
	{
		tempMenuName = "DefeatPopUp";
		/*
		Value[] args = {};
		generalSwf.Invoke("showDefeatPopup",args);
		*/
		generalSwf.showDefeatPopup ();
	}
	
	
	public void onDefeatReturn ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onDefeatReturnCalled");
		Time.timeScale = 1.0f;
		InvokeSomeFunction ("ReturnToCamp");
		sfCamera.isPaused = false;
		GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);

		//GameManager.instance._levelManager.battleManager.gameObject.SetActive(false);
	}
	
	public void onDefeatRestart ()
	{
		PlayMakerFSM.BroadcastEvent ("FadeOutInstantEvent");
		Debug.LogWarning("FadeOut - UIGeneralSwf - OnDefeatRestart");
		//		if(GameManager.PRINT_LOGS) Debug.Log("onDefeatRestartCalled");
		Time.timeScale = 1.0f;
		sfCamera.isPaused = false;
		InvokeSomeFunction ("RestartBattle");
		GameManager.instance._levelManager.battleManager.gameObject.SetActive (false);
	}
	
	void InvokeSomeFunction (string functionName)
	{
		GameManager.instance.Invoke (functionName, 1.0f);
	}
	
	public void OnRegisterSWFCallback (NGGameUI movieRef)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UI_GeneralSwf::OnRegisterSWFCallback()");
		
		generalSwf = movieRef;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Language :::: " + Application.systemLanguage);
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UNITY SIDE === sfCamera.currentLangauge = " + sfCamera.currentLangauge);
		SetLanguageGeneralSwf (sfCamera.languageSet, sfCamera.currentLangauge);
		
//		Console.WriteLine("UI_GeneralSwf type = " + generalSwf.Type);
		
		if (Application.loadedLevelName.Equals ("LevelScene") && GameManager.instance._levelManager.isEnemyClicked) {
			UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
			ToggleTopStats (false);
		}
		
		if (gSwfDelegate != null)
			gSwfDelegate (true);
		
		//GameManager.instance.Invoke("Init",0.5f);
	}
	
	public void ValidateNames (string nameString)
	{
		/*Value val = new Value(nameString,MovieID);
		Value[] args = {val};
		tempSwf.Invoke("checkNames",args);
		*/
	}
	
	public void UpdateRingSwfHealth (int totalHealth)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("RING SWF ::::: UPDATE HEALTH CALLED!");
		
		ringUI.updateHealth (totalHealth);
//		generalSwf.updateHealth (totalHealth);
	}
	
	public void UpdateRingSwfDamage (int totalDamage)
	{
		ringUI.updateDamage (totalDamage);
	}
	
	public void UpdateRingSwfSoulGems (int soulGems)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RING SWF ::::: UPDATE GEMS CALLED!");
		generalSwf.updateSoulGems (soulGems);
	}
	
	public void UpdateRingSwfSoulDust (int soulGems)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RING SWF ::::: UPDATE DUST CALLED!");
		
		generalSwf.updateSoulDust (soulGems);
	}
	
	public void UpdateRuneSwfHealth (int totalHealth)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RUNES/STAFF SWF ::::: UPDATE HEALTH CALLED!");
		
		generalSwf.updateHealth (totalHealth);
	}
	
	public void UpdateRuneSwfDamage (int totalDamage)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RUNES/STAFF SWF ::::: UPDATE DAMAGE CALLED!");
		generalSwf.updateDamage (totalDamage);
	}
	
	public void UpdateRuneSwfSoulGems (int soulGems)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RUNES/STAFF SWF ::::: UPDATE GEMS CALLED!");
		generalSwf.updateSoulGems (soulGems);
	}
	
	public void UpdateRuneSwfSoulDust (float soulGems)
	{
		soulGems = (int)Mathf.Floor (soulGems);
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("RUNES/STAFF SWF ::::: UPDATE DUST CALLED!");
		generalSwf.updateSoulDust (soulGems);
	}
	
	public void onHealthIcon ()
	{
		if (GameManager.instance._levelManager.isEnemyClicked) {
			return;
		}
		InputWrapper.disableTouch = true;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("HEALTH ICON TAPPED!!");
		SoundManager.instance.PlayMenuOpenSound ();
		this.LoadPotionPopup (GameManager._gameState.User._inventory.potionBelt.Count (), GameManager._dataBank.inApps.GetGCost (InApp.InAppTypes.PORTIONS));
		if (TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.PotionTutorialStart) {
			TutorialManager.instance.PotionTutorial2 ();
		}
	}
	
	public void onCloseHealthPopup ()
	{
		InputWrapper.disableTouch = false;
		tempPopUpOpened = false;
//		GameManager.instance.SaveGameState(true);
	}
	
	
	public void ShowBossDefeatPopup (string heading, string text)
	{
		Time.timeScale = 0.0f;
		sfCamera.isPaused = false;
		/*
		Value val1 = new Value(heading,MovieID);
		Value val2 = new Value(text,MovieID);
		Value[] args = {val1,val2};
		generalSwf.Invoke("showBossDefeatPopup",args);
		*/
		generalSwf.showBossDefeatPopup (heading, text);
	}
	
	public void ShowUnLockMainMenuPopup (int dayCount)
	{
		/*
		Value val1 = new Value(dayCount,MovieID);
		Value[] args = {val1};
		generalSwf.Invoke("showUnLockPopup",args);
		*/
		//generalSwf.showUnLockPopup(dayCount);
		
		if (GameManager._gameState.dayCount >= GameManager.UNLOCK_UPGRADES_DAY && !GameManager._gameState._unlockUpgradesShown) {
			generalSwf.showUnLockPopup (NGUnlockedUpgradesPopup.UnlockedUpgradesType.Upgrades);
			GameManager._gameState._unlockUpgradesShown = true;
//			GameManager.instance.SaveGameState(false);
		}
		if (GameManager._gameState.dayCount >= GameManager.UNLOCK_RUNE_DAY && !GameManager._gameState._unlockRunesShown) {
			generalSwf.showUnLockPopup (NGUnlockedUpgradesPopup.UnlockedUpgradesType.Runes);
			GameManager._gameState._unlockRunesShown = true;
//			GameManager.instance.SaveGameState(false);
		}
		if (GameManager._gameState.dayCount >= GameManager.UNLOCK_SPIRITS_DAY && !GameManager._gameState._unlockSpiritsShown) {
			generalSwf.showUnLockPopup (NGUnlockedUpgradesPopup.UnlockedUpgradesType.Spirits);
			GameManager._gameState._unlockSpiritsShown = true;
//			GameManager.instance.SaveGameState(false);
		}
		if (GameManager._gameState.dayCount >= GameManager.UNLOCK_TRANSMUTATION_DAY && !GameManager._gameState._unlockTransmutationShown) {
			generalSwf.showUnLockPopup (NGUnlockedUpgradesPopup.UnlockedUpgradesType.Transmutation);
			GameManager._gameState._unlockTransmutationShown = true;
//			GameManager.instance.SaveGameState(false);
		}
		
	}
	
	public void GotoRingNo (int index)
	{
		ringUI.GoToRingNumber (index);
	}
	
	
	public void SetLanguageGeneralSwf (string languageString, string charSetID)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("SetLanguage GeneralSwf:::::::::::::::::::::::::::::::::::::::::::: " + languageString);
		/*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		Value[] args1={val, val2};
		generalSwf.Invoke("setLanguage",args1);
		*/
		generalSwf.setLanguage (languageString, charSetID);
	}
	
	private void SetLanguage (string languageString, string charSetID)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SetLanguage Temp Swf:::::::::::::::::::::::::::::::::::::::::: " + languageString);
/*		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		tempSwf.Invoke("setLanguage",args1);
		*/
	}
	
	private void SetRingUIInventory ()
	{
		if (missingRingTags == null || missingRingTags.Count == 0)
			ringUI.setInventoryData (GameManager._gameState.User._inventory, GameManager.instance.suggestedItems);
		else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("missingRingTags is not null - so we missing some ring!");
			
			InventoryItem [] arrayItems = GameManager.instance.suggestedItems;
		
			List<ItemRing> currentSuggestedRingsList = GameManager.instance.suggestedUIRingList;
			
			/*
			IDictionary suggestedDictionary = MiniJSON.Json.Deserialize(GameManager.instance.suggestedItemsJson) as IDictionary;
			IList ringsList = suggestedDictionary["Rings"] as IList;
			
			IDictionary oldRingDictionary = ringsList[0] as IDictionary;
			InventoryItem oldRing = currentSuggestedRingsList.Find(x => (x.uid.Equals(oldRingDictionary["uid"].ToString())));
			if(GameManager.PRINT_LOGS) Debug.Log("oldRing >>>" + oldRing.ToString());
			*/
			
			// new ring
			string aMissingTag = missingRingTags.ToArray () [0];
			int skullLevelForNewItem = Mathf.Max (2, GameManager._gameState.skullLevel);
			ItemRing newRing = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel (GameManager._dataBank, skullLevelForNewItem, aMissingTag);
			newRing.isSuggested = true;
			if (GameManager.PRINT_LOGS)
				Debug.Log ("newRing >>>" + newRing.ToString ());
			
			currentSuggestedRingsList.RemoveAt (0);
			currentSuggestedRingsList.Insert (0, newRing);
			
			//ringsList[0] = newRing.ToDictionary();
			
			missingRingTags.Clear ();
			if (GameManager.PRINT_LOGS)
				Debug.Log ("GameManager._gameState.User._inventory.json = " + GameManager._gameState.User._inventory.json);
			//if(GameManager.PRINT_LOGS) Debug.Log("son.Serialize(suggestedDictionary) = " + Json.Serialize(suggestedDictionary));
			
			if (GameManager._gameState.User._inventory.HasNewRingInBag ()) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("HasNewRingInBag is TRUE~~~~");
				GameManager._gameState.User._inventory.SortNewRingsFirst ();
			}
			
			ringUI.setInventoryData (GameManager._gameState.User._inventory, currentSuggestedRingsList.ToArray ());
			//SetInventoryData(GameManager._gameState.User._inventory.json, Json.Serialize(suggestedDictionary));
						
		}
	}
	
	public MonoBehaviour _loadedMenuUI;
	public void OnRegisterSWFChildCallback (MonoBehaviour movieRef)
	{
		if (_loadedMenuUI != null)
			Destroy (_loadedMenuUI.gameObject);

		if (GameManager.PRINT_LOGS)
			Debug.Log ("testing tempMenuName >>> " + tempMenuName);
		tempSwf = movieRef;
		SetLanguage (sfCamera.languageSet, sfCamera.currentLangauge);
		switch (tempMenuName) {
		case "Inventory":
			ringUI = movieRef as NGRingUi;
			_loadedMenuUI = ringUI;
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Gems" + GameManager._gameState.User._inventory.gems);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Souls" + GameManager._gameState.User._inventory.souls);

			UpdateRingSwfHealth (GameManager._gameState.User.totalLife);
			UpdateRingSwfDamage (GameManager._gameState.User.damage);
			ringUI.updateWards (GameManager._gameState.User._wards);
			
			if (GameManager.PRINT_LOGS) 
				Debug.Log ("GameManager._gameState.User.defaultLife" + GameManager._gameState.User.defaultLife + "GameManager._gameState.User.defaultDamage" + GameManager._gameState.User.defaultDamage);
			
			SetRingUIInventory ();
			
			if (GameManager._gameState.User._inventory.HasNewRingInBag ()) {
				Debug.Log ("GO TO RING GoToNewRing ~~~~");
				
				ringUI.GoToNewRing ();
			}
			
			//GameManager._gameState.User._inventory.MarkRingsAsNotNew();	
			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("GO TO RING 3 ~~~~ TUTORIAL TIMEEEE!!! ~~~!!!! ");
				ringUI.GoToRingNumber (1);
				//ringUI.gotoRing(3);
			}
		
			break;
		case "CheatSheet":
			//_cheatSheetRef.cheatSheet=tempSwf;
			break;
			
		case "Staff": 
			runeUI = movieRef as NGRuneUi;
			_loadedMenuUI = runeUI;
			UpdateRuneSwfSoulGems (GameManager._gameState.User._inventory.gems);
			UpdateRuneSwfSoulDust (GameManager._gameState.User._inventory.souls);
			UpdateRuneSwfHealth (GameManager._gameState.User.life);
			
			runeUI.setInventoryData (GameManager._gameState.User._inventory, GameManager.instance.suggestedItems);
			break;
		case "Transmutation":
			transmuteUI = movieRef as NGTransmuteUi;
			_loadedMenuUI = transmuteUI;
			UpdateRuneSwfSoulGems (GameManager._gameState.User._inventory.gems);
			UpdateRuneSwfSoulDust (GameManager._gameState.User._inventory.souls);
			UpdateRuneSwfHealth (GameManager._gameState.User.life);
			
			transmuteUI.setInventoryData (GameManager._gameState.User._inventory, GameManager._gameState.User.transmutationDictionary);
//			SetTransmuteData(GameManager._gameState.User.json);
			break;
		case "Upgrades":
			
			upgradeUI = movieRef as NGUpgradeui;
			_loadedMenuUI = upgradeUI;
			UpdateRuneSwfSoulGems (GameManager._gameState.User._inventory.gems);
			UpdateRuneSwfSoulDust (GameManager._gameState.User._inventory.souls);
			UpdateRuneSwfHealth (GameManager._gameState.User.life);
			upgradeUI.setInventory (GameManager._gameState.User, false);
			
			break;
		case "Spirits":

			spiritsUI = movieRef as NGSpiritsUI;
			_loadedMenuUI = spiritsUI;
			UpdateRuneSwfSoulGems (GameManager._gameState.User._inventory.gems);
			UpdateRuneSwfSoulDust (GameManager._gameState.User._inventory.souls);
			UpdateRuneSwfHealth (GameManager._gameState.User.life);
			SetPetUpgradeData (GameManager._gameState.User.json, false);

			Debug.LogError("current pet name = "+GameManager._gameState.User.spiritId);
			spiritsUI.SetInventoryData (GameManager._gameState.User.spiritId);
			// Tutorial for spirits...
			
			GameManager.instance.scaleformCamera.loadNewTutorial ();
			
			break;
		case "Market":

			marketUI = movieRef as NGMarketUI;
			_loadedMenuUI = marketUI;
			//UpdateRuneSwfSoulGems(GameManager._gameState.User._inventory.gems);
			//UpdateRuneSwfSoulDust(GameManager._gameState.User._inventory.souls);
			//UpdateRuneSwfHealth(GameManager._gameState.User.life);
			SetMarketData (MiniJSON.Json.Serialize (PurchaseManager.Instance.MarketProducts));
			break;
		}
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UI_tempSwf::OnRegisterSWFChildCallback() --- " + tempMenuName);
		
		Resources.UnloadUnusedAssets ();
		//UpdateHealth();
	}
	public string words;
	public void missingString (string missingThings)
	{
		/*	if(Debug.isDebugBuild)
		{
			words +=missingThings+",\n";
			if(GameManager.PRINT_LOGS) Debug.Log("Word Recieved :::: "+missingThings+" Current Words :::"+words);
		} */
	}
	
//	public void onCloseButton()
//	{
//		_cheatSheetRef.onCloseButton();
//	}
	
	public void showJSON (string jsonOne, string jsonTwo)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("JSON FIRST ::::::::::::: " + jsonOne);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("JSON SECOND ::::::::::::: " + jsonTwo);
	}
	
	
	public void SetInventoryData (string jsonString, string suggestedJsonString)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("JSON SENT TO SCALEFORM :::::: " + jsonString);
		
		if (GameManager.PRINT_LOGS) 
			Debug.Log (":::::::::SUGGESTED ITEMS:::::::::::: " + suggestedJsonString);		
		
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("SUGGESTED RINGS SIZE ::::::" + GameManager.instance.suggestedRingList.Count);
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("SUGGESTED RUNES SIZE ::::::" + GameManager.instance.suggestedRuneList.Count);

		generalSwf.setInventtoryData (jsonString, suggestedJsonString);
	}
	public void Init ()
	{
		UpdateHealth (GameManager._gameState.User.life);
		UpdateSoulDust (GameManager._gameState.User._inventory.souls);
		UpdateSoulGems (GameManager._gameState.User._inventory.gems);
		if (sfCamera.levelScene != null)
			sfCamera.levelScene.UpdateArcanePoints (((int)GameManager._gameState.User.arcanePoints));

	}
	
	public void LoadSwf (string filename)
	{
		generalSwf.loadSwf (filename);
	}
	
	public void LoadPotionPopup (int count, int price)
	{
		generalSwf.showPotionPopup (count, price);
	}
	
	public void UnloadSwf ()
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("~~~~~~~~~~~~UNLOAD SWF CALLED~~~~~~~~~~~~~~");
		/*
		Value[] args1={};
		generalSwf.Invoke("unloadSwf",args1);
		*/
		generalSwf.unloadSwf ();

		if (GameManager.PRINT_LOGS) 
			Debug.Log ("~~~~~~~~~~~~UNLOAD SWF CALLED END~~~~~~~~~~~~~~");
	}

	
	public void pauseGame ()
	{
		InputWrapper.disableTouch = true;
//		if(GameManager.PRINT_LOGS) Debug.Log("pauseCalled");
//		if(GameManager.PRINT_LOGS) Debug.Log("BattleManagerActive =="+GameManager.instance._levelManager.battleManager.gameObject.activeInHierarchy);
		if (GameManager.instance._levelManager.battleManager.gameObject.activeInHierarchy) {
//			if(GameManager.PRINT_LOGS) Debug.Log("Hide Battle HUD");
			//sfCamera.DestroyBattleHUD();
			
			UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
			sfCamera.hud.SetDisplayVisible (false);
		} else {
//			if(GameManager.PRINT_LOGS) Debug.Log("Hide Level HUD");
			//sfCamera.DestroyLevelHUD();
			if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.LevelTutorialCompleted)
				&& GameManager.instance.scaleformCamera.uiLevelTutorialSwf != null) {
				//GameManager.instance.scaleformCamera.generalSwf.resumeGame();
				return;
				/*
					TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted, true);
					GameManager.instance.SaveGameState(false);
					GameManager.instance.scaleformCamera.DestroyLevelTutorial();
					*/
			}
			sfCamera.levelScene.SetDisplayVisible (false);
			
		}
		
		Time.timeScale = 0.0f;
		sfCamera.isPaused = true;
		sfCamera.currentMainCamera = Camera.main;
		if (!sfCamera.currentMainCamera.GetComponent<BoxCollider> ()) {
			sfCamera.currentMainCamera.gameObject.AddComponent<BoxCollider> ();
			sfCamera.currentMainCamera.gameObject.GetComponent<BoxCollider> ().size = new Vector3 (10, 10, 10);
			sfCamera.currentMainCamera.gameObject.GetComponent<BoxCollider> ().center = new Vector3 (0, 0, 1);
		}
		
		if (!string.IsNullOrEmpty (words) && Debug.isDebugBuild) {
			//GameManager.instance.WriteMissingWords(words);
		}
		GameManager.instance.EnableGestureEmitter (false);
	}
	
	public void resumeGame ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("ResumeCalled");
		InputWrapper.disableTouch = false;
		if (GameManager.instance._levelManager.isEnemyClicked) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Show Battle Scene!!!");
			//sfCamera.OnLoadBattleStart();	
			
			if (sfCamera.hud != null && !GameManager.instance._levelManager.battleManager.IsBattleEnded) {
				UnityEngine.Debug.Log (">>> SetDisplayVisible > T");
				sfCamera.hud.SetDisplayVisible (true);
			}
			
			if (generalSwf != null) {
				if (!GameManager.instance._levelManager.battleManager.IsBattleEnded) {
					UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
					ToggleTopStats (false);
						
					if (UIManager.instance.levelScene != null)
						UIManager.instance.levelScene.SetDisplayVisible (false);
				} else
					ToggleTopStats (true);
			}
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Show Level Scene!!!");
			//sfCamera.OnLoadLevelStart();
			
			UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > T");
			ToggleTopStats (true);

			if (UIManager.instance.levelScene != null) {
				UIManager.instance.levelScene.SetDisplayVisible (true);
			}

			if (UIManager.instance.hud != null) {
				UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
				UIManager.instance.hud.SetDisplayVisible (false);
			}
//			if(TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.RingTutorialCompleted))
//			{
//				GameManager.instance._levelManager.ActiveUnProtectedPOI();
//			}
		}
		sfCamera.isPaused = false;
		Time.timeScale = 1.0f;
		GameManager.instance.EnableGestureEmitter (true);
	}
	
	
	
	public void swapItems (string equippedItem, string bagItem)
	{
		Debug.Log (string.Format ("Currently Loaded = {0}, equippedItem = {1}, bagItem = {2}", tempMenuName, equippedItem, bagItem));
		//ring tutorial - start
		
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
			TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.RingTutorialComplete);
		}
		//ring tutorial - end
		
		bool isEmptySlot = false;
		
		string [] firstTokens;
		string [] secondTokens;
		
		string firstItemName = "";
		int firstItemSkullLevel = 0;
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("TURN FOR       ::::::::-------------------------------------------->>>>>>>>>>>>>>>>>>>>" + tempMenuName);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("EQUIPPED TO REPLACE -------------------------------------------->>>>>>>>>>>>>>>>>>>>" + equippedItem);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("BAG ITEM TO REPLACE -------------------------------------------->>>>>>>>>>>>>>>>>>>>" + bagItem);
		
		if (!equippedItem.Equals ("empty")) {
			firstTokens = equippedItem.Split ('|');
			
			firstItemName = firstTokens [0];
			firstItemSkullLevel = System.Convert.ToInt32 (firstTokens [1]);
		} else {
			isEmptySlot = true;
		}
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("1 ISEMPTY       ::::::::-------------------------------------------->>>>>>>>>>>>>>>>>>>>" + isEmptySlot);
		
		secondTokens = bagItem.Split ('|');
		
		string secondItemName = secondTokens [0];
		int secondItemSkullLevel = System.Convert.ToInt32 (secondTokens [1]);
		int count = 0;
		switch (tempMenuName) {
//		case "Transmutation":
//			transmuteUI.OnTransmutationItemsPlaced(firstItemName, firstItemSkullLevel, secondItemName, secondItemSkullLevel);
//			break;
		case "Inventory":
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("2 INVENTORY       ::::::::-------------------------------------------->>>>>>>>>>>>>>>>>>>>");
			
			InventorySystem.ItemRing targetEquippedRing = null;
			InventorySystem.ItemRing targetBagRing = null;
			InventorySystem.ItemRing ring = null;
			List<InventorySystem.ItemRing> ringsList = null;
			
			if (!isEmptySlot) {
				ringsList = GameManager._gameState.User._inventory.equippedRings;
				count = ringsList.Count;
				for (int i = 0; i<count; i++) {
					ring = ringsList [i];
					if (ring.id.Equals (firstItemName) && ring.skullLevel == firstItemSkullLevel) { // Equals(new InventorySystem.ItemRing(firstItemName, firstItemSkullLevel)))
						targetEquippedRing = ring;
						break;
					}
				}
			}			

			ringsList = GameManager._gameState.User._inventory.bag.GetAllRings ();
			count = ringsList.Count;
			for (int i = 0; i<count; i++) {
				ring = ringsList [i];
				if (ring.id.Equals (secondItemName) && ring.skullLevel == secondItemSkullLevel) { //if(ring.Equals(new InventorySystem.ItemRing(secondItemName, secondItemSkullLevel)))
					targetBagRing = ring;
					break;
				}	
			}
			
			
			GameManager._gameState.User._inventory.bag.Remove (targetBagRing);
			if (!isEmptySlot) {
				GameManager._gameState.User._inventory.RemoveFromRingBag (targetEquippedRing);
				GameManager._gameState.User._inventory.bag.Add (targetEquippedRing);
			}
			GameManager._gameState.User._inventory.AddToRingBag (targetBagRing);
			
			//SetRingUIInventory();
			
			//if(GameManager.PRINT_LOGS) Debug.Log("7 BEFORE SETTING STATS! !!!     ::::::::-------------------------------------------->>>>>>>>>>>>>>>>>>>>");
			GameManager._gameState.User.SetRingsAndPetStats ();
			UpdateRingSwfHealth (GameManager._gameState.User.totalLife);
			UpdateRingSwfDamage (GameManager._gameState.User.damage);
			ringUI.updateWards (GameManager._gameState.User._wards);
			break;
			
		case "Staff":
		
			InventorySystem.ItemRune targetEquippedRune = null;
			InventorySystem.ItemRune targetBagRune = null;
			InventorySystem.ItemRune rune = null;
			List<InventorySystem.ItemRune> runeList = null;
			if (!isEmptySlot) {
				runeList = GameManager._gameState.User._inventory.staffRunes;
				count = runeList.Count;
				for (int i = 0; i < count; i++) {
					rune = runeList [i];
					if (rune.Equals (new InventorySystem.ItemRune (firstItemName, firstItemSkullLevel))) {
						targetEquippedRune = rune;
						break;
					}
				}
			}
			runeList = GameManager._gameState.User._inventory.bag.GetAllRunes ();
			count = runeList.Count;
			for (int i = 0; i < count; i++) {
				rune = runeList [i];
				if (rune.Equals (new InventorySystem.ItemRune (secondItemName, secondItemSkullLevel))) {
					targetBagRune = rune;
					break;
				}	
			}
			
			GameManager._gameState.User._inventory.bag.Remove (targetBagRune);
			
			if (targetEquippedRune != null) {
				GameManager._gameState.User._inventory.RemoveRuneFromStaff (targetEquippedRune);
				GameManager._gameState.User._inventory.bag.Add (targetEquippedRune);	
			}
			GameManager._gameState.User._inventory.AddRuneToStaff (targetBagRune);
			
			break;
		}
		GameManager._gameState.User.SetRingsAndPetStats ();
		if (sfCamera.levelScene != null)
			sfCamera.levelScene.Init ();
	}
	
//	public void onOptionsButton()
//	{
//		LoadSwf("cheatsheet.swf");
//		tempMenuName="CheatSheet";
//		//sfCamera.LoadCheatSheet();
//	}
	
	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
//	public void RegisterMovies(Value movieRef)
//	{
//		generalSwf = movieRef;
//	}
	
	
	

	// Callback from the content to launch the game when the "close" animation has finished playing.
	public void OnExitGameCallback ()
	{
//		Console.WriteLine("In OnExitGameCallback");
//		//sfMgr.DestroyMovie(this);
//		// Application.Quit() is Ignored in the editor!
//		Application.Quit();
//		// Application.LoadLevelAsync("Level");
//		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
	}
	
	public void PlayOk ()
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("PlayOk");
		SoundManager.instance.PlayMenuOkSound ();
	}
	
	public void PlayCancel ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("PlayCancel");
		SoundManager.instance.PlayMenuCancelSound ();
	}
	
	public void PlayOpen ()
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("PLay Open");
		SoundManager.instance.PlayMenuOpenSound ();
	}
	
	public void onSetSfxVolume (double volume)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SFX Volume " + volume);
		//setMVolume(volume);
		SoundManager.instance.SetSfxVolume ((float)volume);
	}
	
	public void onSetMVolume (double volume)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Music Volume " + volume);
		SoundManager.instance.SetMVolume ((float)volume);
	}
	
	public void setSfxVolume (double volume)
	{
		/*
		Value tutorialIdVal = new Value(volume, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		generalSwf.Invoke("SfxVolume", tutorialIdValArr);
		*/
		generalSwf.SfxVolume (volume);
		//tempSwf.Invoke("setSfxVolume", tutorialIdValArr);
	}
	
	public void setMVolume (double volume)
	{
		/*
		Value tutorialIdVal = new Value(volume, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		generalSwf.Invoke("MVolume", tutorialIdValArr);	
		*/
		generalSwf.setMVolume (volume);
		//tempSwf.Invoke("setMVolume", tutorialIdValArr);	
	}
	
	
	private Action<bool> actionBuySellCB;
	public void BuyRing (string uid, Action<bool> actionBuySellCB)
	{
		this.actionBuySellCB = actionBuySellCB;
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("BuyRing " + uid);
		InventoryPurchaseRequest request = new InventoryPurchaseRequest (PurchaseRequest.PurchaseRequestType.Ring, uid);
		PurchaseManager.Instance.PerformPurchase (request);
	}
	
	public void SellRing (string uid, Action<bool> actionBuySellCB)
	{
		this.actionBuySellCB = actionBuySellCB;
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SellRing " + uid);
		InventoryPurchaseRequest request = new InventoryPurchaseRequest (PurchaseRequest.PurchaseRequestType.Ring, uid);
		PurchaseManager.Instance.PerformSell (request);
	}
	
	public void OnBuyRingComplete (string response)
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.RingTutorialCompleted)) {
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			Debug.Log("CameraAspect ==> " + cameraAspect);
			if (UIManager.instance.isResolution16by9) {
				TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.RingTutorial3_16x9);
			} else if (UIManager.instance.isResolution4by3) {
				TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.RingTutorial3_4x3);
			} else {
				TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.RingTutorial3_3x2);
			}

			/*
			else if(cameraAspect > 1.3f && cameraAspect < 1.4f)
			{
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.RingTutorial3_4x3);
			}
			
			else if(cameraAspect > 1.45f && cameraAspect < 1.55f)
			{
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.RingTutorial3_3x2);
			}
			*/
//			GameManager.instance.scaleformCamera.uiRingTutorialSwf.ShowRingTutorial3();
		}
		
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("OnBuyRingComplete " + response);
		
		if (actionBuySellCB != null)
			actionBuySellCB (response == "success");
				
		generalSwf.onBuyRingComplete (response);
	}
	
	public void OnSellRingComplete ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("OnSellRingComplete");
		/*
		Value[] valArr = {};
		generalSwf.Invoke("onSellRingComplete", valArr);
		*/
		
		if (actionBuySellCB != null)
			actionBuySellCB (true);
		
		generalSwf.onSellRingComplete ();
	}
	
	public void BuyRune (string uid, Action<bool> actionBuySellCB)
	{
		this.actionBuySellCB = actionBuySellCB;
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("BuyRune " + uid);
		InventoryPurchaseRequest request = new InventoryPurchaseRequest (PurchaseRequest.PurchaseRequestType.Rune, uid);
		PurchaseManager.Instance.PerformPurchase (request);
	}
	
	public void SellRune (string uid, Action<bool> actionBuySellCB)
	{
		this.actionBuySellCB = actionBuySellCB;
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SellRune " + uid);
		InventoryPurchaseRequest request = new InventoryPurchaseRequest (PurchaseRequest.PurchaseRequestType.Rune, uid);
		PurchaseManager.Instance.PerformSell (request);
	}
	
	public void OnBuyRuneComplete (string response)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("OnBuyRuneComplete " + response);
		
		if (actionBuySellCB != null)
			actionBuySellCB (response == "success");
		
		generalSwf.onBuyRuneComplete (response);
	}
	
	public void OnSellRuneComplete ()
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("OnSellRuneComplete");
		/*
		Value[] valArr = {};
		generalSwf.Invoke("onSellRuneComplete", valArr);
		*/
		
		if (actionBuySellCB != null)
			actionBuySellCB (true);
		
		generalSwf.onSellRuneComplete ();
	}
	
	public void onMarketButton ()
	{
		if (GameManager.instance._levelManager.isEnemyClicked) {
			return;
		}

		tempMenuName = "Market";
		
		if (GameObject.FindObjectsOfType (typeof(MarketUI)).Length == 0) {
			if (ringUI != null) {
				Destroy (ringUI.gameObject);
				ringUI = null;
			}
			if (runeUI != null) {
				Destroy (runeUI.gameObject);
				runeUI = null;
			}
			if (transmuteUI != null) {
				Destroy (transmuteUI.gameObject);
				transmuteUI = null;
			}
			if (upgradeUI != null) {
				Destroy (upgradeUI.gameObject);
				upgradeUI = null;
			}
			if (spiritsUI != null) {
				Destroy (spiritsUI.gameObject);
				spiritsUI = null;
			}
			if (marketUI != null)
				return;

			generalSwf.OpenMarketUI ().setInterface (this);
			//generalSwf._topPanel.GetComponent<dfPanel>().BringToFront();
			Analytics.logEvent (Analytics.EventName.PauseMenu_Market);
		}
		
//		Debug.Log("Open Market here!!!!");
//		return;

//		tempMenuName = "Market";
//		LoadSwf("Market.swf");
//		Analytics.logEvent(Analytics.EventName.PauseMenu_Market);
		
	}
	
	public void SetMarketData (string jsonString)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("JSON SENT TO Market::::" + jsonString);
/*		Value val = new Value(jsonString, MovieID);
		
		Value[] args1 = {val};
		
		tempSwf.Invoke("setInventoryData", args1);
		*/
	}
	
	public void CardClick (string itemId)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("~~~~~~~~~~~~~~~Card Clicke~~~~~~~~~~~~~" + itemId);
		PurchaseManager.Instance.BuyItem (itemId);
		Analytics.logEvent (Analytics.EventName.Market_BuyCard);
	}
	
	private void showPauseMenu (bool isBattlePopup, int dayCount)
	{
		generalSwf.showPauseMenu (isBattlePopup, dayCount);
	}
	
	public void MenuButtonClick ()
	{
		TutorialManager.instance.DestroyManualTutorial (TutorialManager.TutorialsAndCallback.BattleEndTutorial2);
		
		if (tempSwf != null) {
			Debug.LogError("Menu button - "+tempSwf.gameObject.name);
			Destroy (tempSwf.gameObject);
			tempSwf = null;
			tempMenuName = string.Empty;
		}
		
		if (GameManager.instance._levelManager != null && GameManager._gameState != null)
			showPauseMenu (GameManager.instance._levelManager.isEnemyClicked, GameManager._gameState.dayCount);
//		if (GameManager.instance._levelManager != null && GameManager._gameState != null)
//			showPauseMenu (!GameManager.instance._levelManager.battleManager.IsBattleEnded, GameManager._gameState.dayCount);
		//unlock spirit on condition...
	}
	
	public void ShowUILoadingScreen (bool isDisableMarket)
	{
		if (isDisableMarket) {
			EnableMarket (!isDisableMarket);
		}

		//Value[] args={};

		if (GameManager.PRINT_LOGS) 
			Debug.Log ("load laoding screen");

		isLoading = true;
		//generalSwf.Invoke("showUiLoadingPopup", args);
		generalSwf.showUiLoadingPopup ();
	}
	
	public void HideUILoadingScreen (bool isEnableMarket)
	{
		if (isEnableMarket) {
			EnableMarket (isEnableMarket);
		}
		//Value[] args={};
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("unloading laoding screen");
		isLoading = false;
		//generalSwf.Invoke("hideUiLoadingPopup", args);
		generalSwf.hideUiLoadingPopup ();
	}
	
	public void onTickSound ()
	{
		SoundManager.instance.PlayMenuTickSound ();
	}
	
	public void onVictorySound ()
	{
		SoundManager.instance.PlayMenuVictorySound ();
	}
	
	public void onDefeatSound ()
	{
		SoundManager.instance.PlayMenuDefeatSound ();
	}
	
	public void onTreasureSound ()
	{
		//	SoundManager.instance.PlayTreasureOpenSound();
	}
	
	public void onSpiralPopupSound ()
	{
		SoundManager.instance.PlayTreasureLootSound ();
	}
	
	public void onRatingSound ()
	{
		SoundManager.instance.PlayRatingSound ();
	}
	
	public void playVideo ()
	{
		Time.timeScale = 1;
		SoundManager.instance.MuteSound (true);
		PlayMakerFSM.BroadcastEvent ("FadeOutInstantEvent");
		Debug.LogWarning("FadeOut - UIGeneralSwf - PlayVideo");

		//Invoke("PlayCinematicMovie",1.0f);
		GameObject.Instantiate (Resources.Load ("CinematicCamera"));
	}
	
	public void PlayCinematicMovie ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Play Moviee!!!!!");
		GameObject.Instantiate (Resources.Load ("CinematicCamera"));
	}
	
	public void EnableMarket (bool isEnable)
	{
		/*if(tempSwf != null && tempMenuName.Equals("Market"))
		{
			Value val = new Value(isEnable, MovieID);
			Value[] args = {val};
			tempSwf.Invoke("mouseClick", args);
		}
		*/
		
	}
	
	public bool onKeyBuyButton ()
	{
		InventorySystem.Inventory inventory = GameManager._gameState.User._inventory;
		InventorySystem.KeyRing keyRing = inventory.keyRing;
		
		Analytics.logEvent (Analytics.EventName.KeysPopUp_Buy);
		
		if (keyRing.keyCount == keyRing.capacity) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("NEED KEYRING UPGRADE!!!");
			// show error
			//ShowGeneralPopup("Keys Maxed out", "Your Arcane KeyRing is maxed out.");
			PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.KeyRingUpgrade, 0, "Upgrade Now", "Not Now");
		} else {
			int gems = GameManager._dataBank.inApps.GetGCost (InApp.InAppTypes.KEYS);
			TransactionRequest request = new TransactionRequest (PurchaseRequest.PurchaseRequestType.Keys, Nonce.GetUniqueID (), "", gems, 0, 0, "", true);
			PurchaseRequest.PurchaseRequestType response = PurchaseManager.Instance.PerformTransaction (request);
			if (response == PurchaseRequest.PurchaseRequestType.Success) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("RESPONSE SUCCESS >>> NO GEMS NEEDED!!!");
				
				keyRing.keyCount = keyRing.capacity;
				sfCamera.levelScene.UpdateTotalKeys (keyRing.keyCount);
				this.UpdateSoulGems (inventory.gems);
//				GameManager.instance.SaveGameState(false);
				return true;
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("RESPONSE FAIL >>> NEED GEMS !!!");
				
				PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - inventory.gems, "Buy Keys", "Buy Gems");
			}
		}
		return false;
	}
	
	public void onPotionBuyButton ()
	{
		InventorySystem.Inventory inventory = GameManager._gameState.User._inventory;
		InventorySystem.PotionBelt potionBelt = inventory.potionBelt;
		if (potionBelt.potions.Count == potionBelt.capacity) {
			// show error
			//ShowGeneralPopup("Potions Maxed out", "Your Potion Belt is maxed out.");
			PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.PotionBeltUpgrade, 0, "", "");
		} else {
			int gems = GameManager._dataBank.inApps.GetGCost (InApp.InAppTypes.PORTIONS);
			TransactionRequest request = new TransactionRequest (PurchaseRequest.PurchaseRequestType.Portions, Nonce.GetUniqueID (), "", gems, 0, 0, "HEALTH_POTION", true);
			PurchaseRequest.PurchaseRequestType response = PurchaseManager.Instance.PerformTransaction (request);
			if (response == PurchaseRequest.PurchaseRequestType.Success) {
				int count = potionBelt.capacity - potionBelt.potions.Count;
				for (int i = 0; i < count; i++) {
					potionBelt.AddPotion (GameManager._dataBank.GetPotionForPotionID ("HEALTH_POTION"));
				}
				this.UpdateSoulGems (inventory.gems);
				SoundManager.instance.PlayMenuOkSound();
//				GameManager.instance.SaveGameState(false);
			} else {
				PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - inventory.gems, "Buy Potions", "Buy Gems");
			}
		}
		Analytics.logEvent (Analytics.EventName.PotionPopUp_Buy);
	}
	
	public void fpsSet (bool isOn)
	{
		GameManager.instance._levelManager.allowDebug = !GameManager.instance._levelManager.allowDebug;
		//GameManager.instance.fpsCounter.isCalculateFPS = isOn;
	}
	
	public void ShowUserID (bool isOn)
	{
//		ShowText(false, (isOn ? ("Id: " + PlayerPrefs.GetString(FRAG.Networking.USERID_PREF)) : ""));
	}
	
	public void resetGame ()
	{
		//resumeGame();
		LoadLoadingScreen ();
		sfCamera.levelScene.SetDisplayVisible (false);
		PlayerPrefs.DeleteAll ();
		GameManager.instance.SetGameStateToDefault ();
	}
	
	public void ShowText (bool isFPS, string text)
	{
		/*
		Value val = new Value(text, MovieID);
		Value[] args = {val};
		*/
		if (isFPS) {
			//generalSwf.Invoke("fpsText", args);
			generalSwf.fpsText (text);
		} else {
			//generalSwf.Invoke("idText", args);
			generalSwf.idText (text);
		}
	}
	
	public void setSkullLevel ()
	{
		getText ();
	}
	
	public void onSecretButton ()
	{
		Analytics.logEvent (Analytics.EventName.Options_Credits);
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("Debug.isDebugBuild" + Debug.isDebugBuild);
		/*
		Value val1=new Value(Debug.isDebugBuild,MovieID);
		Value[] valArr = {val1};
		*/
		//setCurrentSkullLevel();	
		//generalSwf.Invoke("openCredits",valArr);
		generalSwf.openCredits (Debug.isDebugBuild);
	}
	
	public void onDebugEnabled ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onDebugEnabled~~~~~~~~~~~~~");
		setCurrentSkullLevel ();
	}
	
	public void setCurrentSkullLevel ()
	{
		generalSwf.skullLevelValue (GameManager._gameState.skullLevel);
	}
	
	
	public void onSkullLevelTextFocus ()
	{
		GameManager.instance.keyBoard = TouchScreenKeyboard.Open ("", TouchScreenKeyboardType.Default);
	}
	
	
	public void getText ()
	{
		/*
		Value[] args = {};
		generalSwf.Invoke("getSkullLevelText", args);
		*/
		generalSwf.getSkullLevelText ();
	}
	
	
	public void onGetSkullLevelText (string skullLevelStr)
	{
		if (!skullLevelStr.Equals ("")) {
			int skullLevel = System.Convert.ToInt32 (skullLevelStr);
			if (skullLevel > 35) {
				skullLevel = 35;				
			}
			GameManager._gameState.skullLevel = skullLevel;
			setCurrentSkullLevel ();
			setText ("");
//			GameManager.instance.SaveGameState(false);
		}
	}
	
	public void setText (string input)
	{
		/*
		Value val = new Value(input, MovieID);
		Value[] args = {val};
		generalSwf.Invoke("setSkullLevelText", args);
		*/
		generalSwf.setSkullLevelText (input);
	}

	
	public void ToggleTopStats (bool isVisible)
	{
//		Debug.Log("[UI_GeneralSwf] ToggleTopStats = " + isVisible);
		if (generalSwf == null)
			return;
		if (isVisible)
			generalSwf.showTopStats ();
		else
			generalSwf.hideTopStats ();
	}
	
	public void UpgradeStarted ()
	{
		Init ();
		
		/*
		
		if(tempSwf != null)
		{
			Value[] args = {};
			tempSwf.Invoke("upgradeStarted", args);
		}
		
			*/	
	}
	
	
	public void onShrineBoost ()
	{
		Analytics.logEvent (Analytics.EventName.Shrine_Boost);
		//if(FB.IsLoggedIn)
		//{
		Shrine tempShrine = ShrineManager.Instance.GetShrineForLevel (GameManager.instance._levelManager.currentLevel + "_Shrine");
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Friend List :::: " + GameManager.instance._monoHelpers.formatCommaSeperatedString (tempShrine.FriendsList));

		ShrineInvitePopUp (tempShrine.currentShrinePoints, tempShrine.FriendsList.Count, GameManager.instance._monoHelpers.formatCommaSeperatedString (tempShrine.FriendsList));
		//}
		//else
		//{
		//	ShrineConnectPopUp();
		//}
	}
	
	public void ShrineConnectPopUp ()
	{
		/*
		Value[] args ={};
		generalSwf.Invoke("ShrineConnect",args);
		*/
		generalSwf.ShrineConnect ();
	}
	
	public void ShrineInvitePopUp (int points, int friendCount, string friendsList)
	{
		generalSwf.ShrineInvite (points, friendCount, friendsList);
	}
	
	
	public void TransmuteStarted ()
	{
		/*
		if(tempSwf != null)
		{
			Value[] args = {};
			tempSwf.Invoke("transmuteStarted", args);
		}
		*/
	}
	
	public void onCloseUiGeneralPopup ()
	{
		InputWrapper.disableTouch = false;
		tempPopUpOpened = false;
	}
	
	// tutorial commands added by mufti ---- for shrine
	public void ShrineRingChargeTutorial ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("shrineRingChargeTutorial", args);
		*/
		generalSwf.shrineRingChargeTutorial ();
	}
	public void ChargeArrowOffAndBoostDisable ()
	{
		/*
		Value[] args={};
		generalSwf.Invoke("shrineRingChargeTutorialOff", args);
		*/
		generalSwf.shrineRingChargeTutorialOff ();
	}
	
	public void supportFaceBook ()
	{
		Application.OpenURL ("https://www.facebook.com/4rockngames");
	}
	
	public void supportMailUs ()
	{
//		string emailBody ="\nDevice Model: "+SystemInfo.deviceModel+"\nDevice OS: "+SystemInfo.operatingSystem+"\nUser Id: "+PlayerPrefs.GetString(FRAG.Networking.USERID_PREF)+"\nBuild Version: "+GameManager.APP_VERSION;
//		if(GameManager.PRINT_LOGS) Debug.Log("EMAIL BODYYYY === "+emailBody);
//		GameManager.instance._monoHelpers.SendEmail("sorcerersring@frag-games.com","[SR] Customer Support",emailBody);
	}
	
#region Spirits	
	
	public void onSpiritsButton ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("onSpiritsButton");
		tempMenuName = "Spirits";

		Debug.LogError("onSpiritsButton 1");

		ServerRequest tRequest = new UpdateRequest (ServerRequest.ServerRequestType.All, ProcessSpiritResponse);
		ServerManager.Instance.ProcessRequest (tRequest);

		generalSwf.OpenSpiritsUI ().setInterface (this);
//		generalSwf._topPanel.GetComponent<dfPanel>().BringToFront();
//		LoadSwf("familier.swf");
	}
	
	public void ProcessSpiritResponse (ServerResponse response)
	{
		GameManager._gameState.User.ProcessSpiritResponse (response);
	}
	
	public void SetPetUpgradeData (string jsonString, bool isUpdate)
	{
		Debug.Log ("SetPetUpgradeData::::" + jsonString + "        isUpdate =" + isUpdate);
/*		Value val=new Value(jsonString,MovieID);
		
		Value[] args1={val};
		if(isUpdate)
		{
			tempSwf.Invoke("updateUpgradeData",args1);
		}
		else
		{
			tempSwf.Invoke("setInventoryData",args1);
			GameManager._gameState.User.UpdateUIButtons();
		}
		*/
		
		if (isUpdate && spiritsUI != null) {
			string[] tokens = jsonString.Split ('|');
			spiritsUI.SetUpdateData (int.Parse (tokens [0]), int.Parse (tokens [1]));
		}
	}
	
	public void SetPetEquipped (bool yesNo)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("SetPetEquipped   yesNo" + yesNo);
/*		
 		Value val1 = new Value(yesNo, MovieID);
		Value[] args ={val1};
		tempSwf.Invoke("petEquipped",args);
		*/
	}
	
	public void UnEquipPet ()
	{
		GameManager._gameState.User.UnequipSpirit ();
	}
	
	public void UnEquipPet (string petId) //not working
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("UnEquipPet(string petId)" + petId);
	}
	
	public void EquipPet (string petId)
	{
		GameManager._gameState.User.EquipPet (petId);
	}
	
	public void ShowEquipButton ()
	{
		/*
		Value[] args ={};
		tempSwf.Invoke("showEquipButton",args);
		*/
	}
	
	public void HideEquipButton ()
	{
		/*
		Value[] args ={};
		tempSwf.Invoke("hideEquipButton",args);
		*/
	}
	
	public void ShowUnEquipButton ()
	{
		/*
		Value[] args ={};
		tempSwf.Invoke("showUnEquipButton",args);
		*/
	}
	
	public void HideUnEquipButton ()
	{
		/*
		Value[] args ={};
		tempSwf.Invoke("hideUnEquipButton",args);
		*/
	}
	
	public void DisableEquipButton ()
	{
		/*
		Value[] args ={};
		tempSwf.Invoke("disableEquipButton",args);
		*/
	}
	
	public void GetCurrentPet ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("GetCurrentPet CALLLEEEEEDDD");
		
		/*
		Value[] args ={};
		tempSwf.Invoke("getCurrentPet",args);
		*/
	}
	
	//CB from GetCurrentPet ...
	public void getCurrentPet (string petId)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("getCurrentPet(string petId)" + petId);
		GameManager._gameState.User.UpdateUIButtons (petId);
	}
	
	public void onUnlockGame ()
	{
		if (!GameManager.instance._levelManager.currentLevel.Equals ("ThreeGods")) {
			GameState.UnlockGame (GameManager._gameState);
			GameManager.instance._monoHelpers.WriteIntoPersistantDataPath ("", PurchaseManager.FILE_NAME);
			GameManager.instance.SaveGameState (true);
	
			Init ();
		}
	}

	// Cutscene Dialog
	// -------------------------------------

	public Action OnNextClicked;

	public void ShowCutsceneDialog (float x, float y)
	{
		generalSwf.showDilogBox (x, y, () => NextButton (), () => skipDilog ());
	}

	public void HideCutsceneDialog ()
	{
		if(generalSwf != null)
			generalSwf.hideDilogBox ();
	}

	public void ChangeCutsceneDialog (string title, string text, float x, float y)
	{
		generalSwf.changeDilogBox (title, text, x, y);
	}



	public void NextButton ()
	{
		OnNextClicked ();
	}

	//--------------------
#endregion	
	
	public void ShowRaidsVictoryPopup (int position, int arcana, int deltaArcane, int topDamage, int bossHp, int raidsSize, long timeLeft)
	{
		
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("ShowRaidsVictoryPopup " + position + " = " + arcana + " = " + topDamage + " = " + bossHp + " = " + raidsSize + " = " + timeLeft + " USERNAME >> " + GameManager._gameState.User.username);

		/*
		Value val1=new Value(position,MovieID);
		Value val2=new Value(arcana,MovieID);
		Value val3=new Value(topDamage,MovieID);
		Value val4=new Value(bossHp,MovieID);
		Value val5=new Value(raidsSize,MovieID);
		Value val6=new Value(timeLeft,MovieID);
		Value[] args={val1,val2,val3,val4, val5, val6};
		generalSwf.Invoke("showRaidsVictoryPopup",args);
		*/
		generalSwf.showRaidsVictoryPopup (position, arcana, deltaArcane, topDamage, bossHp, raidsSize, timeLeft);
	}
	
	public void ShowRaidsDefeatPopup (int position, int arcana, int deltaAracne, int topDamage, int bossHp, int raidsSize, long timeLeft)
	{
		if (GameManager.PRINT_LOGS) 
			Debug.Log ("ShowRaidsDefeatPopup" + position + " = " + arcana + " = " + topDamage + " = " + bossHp + " = " + raidsSize + " = " + timeLeft);

		generalSwf.showRaidsDefeatPopup (position, arcana, deltaAracne, topDamage, bossHp, raidsSize, timeLeft);
	}
	
	public void onRaidsVictoryPopupClose ()
	{
		InputWrapper.disableTouch = false;
		CloseRaidPopup ();
	}
	
	public void onRaidsDefeatPopupClose ()
	{
		InputWrapper.disableTouch = false;
		CloseRaidPopup ();
	}
	
	private void CloseRaidPopup ()
	{
		InputWrapper.disableTouch = false;
		GameManager.instance._levelManager.battleManager.scaleformBattleEnded = true;
		Time.timeScale = 1.0f;
		GameManager.instance.scaleformCamera.isPaused = false;
	}
	
	public void skipDilog ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("skip Dialog called from scaleform");
		
		GameObject cutsceneObject = GameObject.Find ("BeachCutscene(Clone)");
		if (cutsceneObject != null) {
			cutsceneObject.GetComponent<CutsceneHandler> ().EndCutscene ();
			return;
		}
		
		cutsceneObject = GameObject.Find ("BeachCutsceneNex(Clone)");
		if (cutsceneObject != null)
			cutsceneObject.GetComponent<NexCutsceneHandler> ().EndCutscene ();
	}

	public void playHeartBeat ()
	{

	}

	public void showPausePopup ()
	{

	}

	public void onOpenChest ()
	{

	}

	public void RaidsVictoryPopup ()
	{

	}

	public void RaidsDefeatPopup ()
	{

	}

	public void showLoadingPopup ()
	{

	}

	public void ConfermButtonYes (string msgCopy)
	{

	}

	public void backButtonClicked (string getInventoryData) //useless!!!
	{
		
	}
	
	public void onShrineTutorialEnd ()
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted)) {
			Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string> ();
			analParams.Add (Analytics.ParamName.ShrineLevelName, GameManager.instance._levelManager.currentLevel);
			Analytics.logEvent (Analytics.EventName.Shrine_Tutorial_End, analParams);
			
			TutorialManager.instance.AddTutorialIdWithStatus (TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted, true);
			
			UIManager.instance.generalSwf.resumeGame ();
			GameManager.instance.SaveGameState (false);
		}
	}
	
	public void SetOnTop ()
	{
		generalSwf.SetOnTop ();
	}
	
	public void ShowHeartIcon ()
	{
		generalSwf.ShowHeartIcon ();
	}
	
	public void HideHeartIcon ()
	{
		generalSwf.HideHeartIcon ();
	}
	
	public void onArcaneKeysEnd ()
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted) && GameManager.instance._levelManager.currentLevel.Equals ("StatuePath")) {
			TutorialManager.instance.AddTutorialIdWithStatus (TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted, true);
			
			//GameManager.instance.scaleformCamera.Invoke("DestroyGeneralSWF",1.0f); //bugfix
			//GameManager.instance.StartCoroutine(GameManager.instance.LoadGeneralSwf()); 
			
			GameManager.instance.SaveGameState (false);
			Analytics.logEvent (Analytics.EventName.ArcaneKeys_Tutorial_End);
		}
	}
	
}
