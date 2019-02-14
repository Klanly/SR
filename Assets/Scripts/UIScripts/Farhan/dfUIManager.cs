//#define NGUI 
//
//using UnityEngine;
//using System;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//
//#if NGUI
//public class dfUIManager : Singleton<dfUIManager> {
//	
//	#region References to other UI 
//	
//	public GameObject gameUIPrefab;
//	public GameObject levelUIPrefab;
//	
//	public const string battleUIPrefabPath = "UIPanels/InGameBattle";
//	
//	//public RaidsUI _raidUIPrefab;
//	public const string _raidUIPrefabPath = "UIPrefabs/RaidsUI";
//	#endregion
//	public bool isPaused = false;
//	public bool	showRingShredsPopUp = false;
//	
//	public GameObject generalSwfGameobject;
//	public GameObject levelSwfGameobject;
//	public GameObject battleSwfGameobject;
//	
//	
//	public UI_Tutorial uiLevelTutorialSwf;
//	
//	// The following swfs need to be replaced... with c# code
//	public UI_HUD hud = null;
//	public UI_Tutorial NewTutorialSwf = null;
//	public UI_Scene_Level levelScene = null;
//	
//	public UI_Scene_MainMenu mainMenu = null;
//	//public UI_Raid 	raidUI	= null;
//	public UI_Guild guildUI	= null;
//	public UI_Tutorial 	uiRingTutorialSwf = null;
//	public UI_Tutorial 	uiRuneTutorialSwf = null;
//	public UI_Tutorial 	uiUpgradeTutorialSwf = null;
//	public UI_Tutorial 	uiTransmutationTutorialSwf = null;
//	public UI_Tutorial 	uiPotionTutorialSwf = null;
//	public UI_Tutorial 	tutorialSwf = null;
//	
//	//========================================================
//	public Camera currentMainCamera;
//	
//	
//	
//	public UI_GeneralSwf generalSwf;
//	
//	public TextAsset languageStringFile;
//	public string _languageSet;
//	public string languageSet
//	{
//		get
//		{
//			return _languageSet;
//		}
//		private set	{	_languageSet = value;}
//	}
//	
//	public string currentLangauge
//	{
//		private set;
//		get;
//	}
//	
//	
//	public dfLabel logLabel;
//
//	public void Start()
//	{
//		base.Start();
//        isPaused		= false;
//		SetCurrentLanguage(Application.systemLanguage.ToString());
//		Debug.Log("Application.systemLanguage.ToString() = " + Application.systemLanguage.ToString());
//		
//	}
//	
//	public string fileName;
//	private void SetCurrentLanguage(string language)
//	{
//		//return;
//		switch(language)
//		{
//		/*case "Spanish":
//			currentLangauge = "es";
//			fileName = "en_es";
//			break;
//		case "Italian":
//			currentLangauge = "it";
//			fileName = "en_it";
//			break;
//		case "French":
//			currentLangauge = "fr";
//			fileName = "en_fr";
//			break;*/
//		default:
//			currentLangauge = "en";
//			fileName = "en_en";
//			break;
//		}
//		
//		if(CheckFileExistance(fileName+"_new.txt"))
//		{
//			/*if(GameManager.PRINT_LOGS)*/ Debug.Log("file exists at path! filename = " + fileName+"_new.txt");
//			languageSet = LoadTextFromWriteablePath(fileName+"_new.txt");
//		}
//		else
//		{
//			/*if(GameManager.PRINT_LOGS)*/ Debug.Log("file doesnt exists at path! filename = " + fileName+"_new.txt");
//			languageSet = (Resources.Load(fileName) as TextAsset).text;
//		}
//	}
//	
//	public void OnUsernameListener(string username) 
//	{
//		/* if(GameManager.PRINT_LOGS) */ Debug.Log("::::::::::::::::: OnUsernameListener username" + username);
//		if(string.IsNullOrEmpty(username))
//		{
//			DestroyGuildUI();
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: OnUsernameListener username NULL" );
//		}
//		/* else if(guildUI == null)
//		{
//			CreateGuildUI(this.OnGuildUILoaded);
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: GUILD UI ==== NULL" );
//		}*/
//	}
//	
//	private void OnGuildUILoaded()
//	{
//		OnUsernameListener(GameManager._gameState.User.username);
//		if(GameManager.PRINT_LOGS) Debug.Log("Guild UI loaded!");
//	}
//
//	private GameUI gameuiPanel;
//	public void LoadGeneralSwf(GameManager.GeneralSwfLoaded del)
//	{
//		if(generalSwf == null)
//		{
//			gameuiPanel = NGUITools.AddChild(gameUIPrefab).GetComponent<GameUI>();
//			generalSwf = generalSwfGameobject.GetComponent<UI_GeneralSwf>();
////			generalSwf.generalSwf = gameuiPanel;			
//		}
//		
//		gameuiPanel.SetListener(generalSwf);
//		generalSwf.gSwfDelegate = del;
//		
//		SetLayerOrder();
//	}
//	
//	public void LoadGeneralSwf()
//	{
//		if(generalSwf == null)
//		{
//			gameuiPanel = NGUITools.AddChild(gameUIPrefab).GetComponent<GameUI>();
//			generalSwf = generalSwfGameobject.GetComponent<UI_GeneralSwf>();
////			generalSwf.generalSwf = gameuiPanel;
//			
//			if(UIManager.instance.hud != null)
//			{
//				UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
//				generalSwf.ToggleTopStats(false);
//			}
//		}
//		
//		gameuiPanel.SetListener(generalSwf);
//		
//		SetLayerOrder();
//	}
//	
//	
//	private BattleUIScript battleUIPanel;
//	public void CreateGameHud()
//	{
////		if(hud == null)
////		{
////			battleUIPanel = gameObject.GetComponent<dfControl>().AddPrefab(Resources.Load(battleUIPrefabPath) as GameObject).gameObject.GetComponent<BattleUIScript>();
////			hud = battleSwfGameobject.GetComponent<UI_HUD>();
////			hud.hud = battleUIPanel;
////		}
//		
////		battleUIPanel.SetInterface(hud);
//		
//		SetLayerOrder();
//		}
//	
//	private LevelUIScript leveluiPanel;
//	public void CreateLevelScene()
//	{
////		if(levelScene == null)
////		{
////			leveluiPanel = gameObject.GetComponent<dfControl>().AddPrefab(levelUIPrefab).gameObject.GetComponent<LevelUIScript>();
////			levelScene = levelSwfGameobject.GetComponent<UI_Scene_Level>();
////			levelScene.levelScene = leveluiPanel;
////		}
////		
////		leveluiPanel.SetInterface(levelScene);
////		
////		SetLayerOrder();
//	}
//
//	
//	private void SetLayerOrder()
//	{
////		gameuiPanel.GetComponent<dfPanel>().BringToFront();
////		TutorialManager.instance.GetComponent<dfPanel>().BringToFront();
//	}
//	
//	
//	private RaidsUI raidUI;
//	
//	public RaidsUI CreateRaidUI(UI_Raid.LeaderBoardListener leaderboardListener)
//	{
//		raidUI = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_raidUIPrefabPath) as GameObject).GetComponent<RaidsUI>();
//		raidUI._leaderboardInterface = leaderboardListener;
//		return raidUI;
//	}
//	
//	public void LoadGuildUI()
//	{
//		/*
//		if(GameManager.PRINT_LOGS) Debug.Log(":::::::::::::::::::::: LoadGuildUI :::::::::::::::::::::::::::::::::::: ");
//		GameManager._gameState.User.usernameChangeEvent += new User.UsernameChangeDelegate(this.OnUsernameListener);
//		OnUsernameListener(GameManager._gameState.User.username);
//		*/
//	}
//	
//	public void CreateGuildUI(System.Action ack)
//	{
//		/*
//		if(guildUI == null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("guildUI NULL ::: CreateGuildUI CALLED");
//			guildUI = new UI_Guild(SFMgr, CreateMovieCreationParams(UI_Guild.GUILD_SWF_NAME, 100), ack);
//		}
//		*/
//	}
//	
//	public void OnLoadBattleStart()
//	{
//		CreateGameHud();
//	}
//	
//	public void LoadVictoryScreen(int healthLost, int battleTime, int topDamage, string grade,bool isBossFight)
//	{
//		showRingShredsPopUp=isBossFight;
//		SoundManager.instance.StopBattleBackgroundSound();
//		int currentSkullLevelDustValue=0;
//		int dustskullLevel=GameManager._gameState.skullLevel;
//		
//		if(!GameManager.instance.isMultiPlayerMode)
//		{
//			string aiName = GameManager.instance.getEnemyFromGameState(GameManager.instance._levelManager.currentLevel+"_DuelA");
//			if(GameManager._gameState.bossAttemptDictionary.Contains(aiName))
//			{
//				dustskullLevel = GameManager._dataBank.GetModelForMonsterByEncounter(aiName,Int32.Parse(GameManager._gameState.bossAttemptDictionary[aiName].ToString())).skullLevel;
//			}
//		}
//		Buff greedBuff = GameManager._gameState.User.GetBuffForBuffName("greed");
//		if(greedBuff == null) //earth has greed
//			currentSkullLevelDustValue = LootManager.getSoulDust(dustskullLevel);
//		else
//		{
//			currentSkullLevelDustValue = LootManager.getSoulDust(dustskullLevel);
//			if(GameManager.PRINT_LOGS) Debug.Log("currentSkullLevelDustValue" + currentSkullLevelDustValue);
//			currentSkullLevelDustValue += (int)greedBuff.modifierValue;
//			if(GameManager.PRINT_LOGS) Debug.Log("currentSkullLevelDustValue += (int)greedBuff.modifierValue" + currentSkullLevelDustValue);
//		}
//		
//		Time.timeScale=0.0f;
//		generalSwf.ShowVictoryPopup(currentSkullLevelDustValue,healthLost,battleTime,topDamage, grade);
//	}
//	
//	public void LoadLootScreen(string keysReq)
//	{
//		Debug.Log("UIMANAGER >> LoadLootScreen >> keysReq = " + keysReq);
//		
//		Time.timeScale=0.0f;
//		InventorySystem.InventoryItem currentLoot=LootManager.DecideLoot("Loot",GameManager.instance._levelManager._zone,keysReq);
//		if(currentLoot as InventorySystem.ItemRing != null)
//			((InventorySystem.ItemRing) currentLoot).isNew = true;
//		GameManager.instance.scaleformCamera.generalSwf.LoadLoot(currentLoot);
//		levelScene.UpdateTotalKeys(GameManager._gameState.User._inventory.keyRing.keyCount);
//		//generalSwf.ShowLootPopup(10,10);
//		
//	}
//	public void lootPopupClosed()
//	{
//		Time.timeScale=1.0f;
//	}
//	
////	public void UnLoadVictoryScreen()
////	{
////		Time.timeScale=1.0f;
////		generalSwf.onVictoryContinue();
////	}
//	
//	public void LoadDefeatScreen()
//	{
//		Time.timeScale=0.0f;
//		generalSwf.ShowDefeatPopup();
//	}
//	
//	public void loadNewTutorial()
//	{
//		if(GameManager.instance._levelManager.currentLevel=="StatuePath")
//		{
//			TutorialManager.instance.decideInLevelTutorial(false);
//		}
//		else
//		{
//			TutorialManager.instance.decideInLevelTutorial(true);
//		}
//	}
//	
//	
//	public void loadUILevelTutorial()
//	{
//		Debug.Log(":::::::::::::::        loadUILevelTutorial :::::::::::::::");
//
//		float cameraAspect = (float)Screen.width/(float)Screen.height;
//		Debug.Log("CameraAspect ==> " + cameraAspect);
//
//		if(cameraAspect > 1.7f && cameraAspect < 1.8f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_16x9);
//			SetLayerOrder();
//		}
//
//		else if(cameraAspect > 1.3f && cameraAspect < 1.4f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_4x3);
//			SetLayerOrder();
//		}
//
//		else if(cameraAspect > 1.45f && cameraAspect < 1.55f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_3x2);
//			SetLayerOrder();
//		}
//	}
//	
//	public bool isResolution3by2
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.45f && cameraAspect < 1.55f)
//				return true;
//			return false;
//		}
//	}
//	
//	public bool isResolution4by3
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.3f && cameraAspect < 1.4f)
//				return true;
//			return false;
//		}
//	}
//	
//	public bool isResolution16by9
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.7f && cameraAspect < 1.8f)
//				return true;
//			return false;
//		}
//	}
//	
//	public void onPotionTutorialEnd()
//	{
//		/*
//		if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.PotionTutorialStart)
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.PotionTutorialCompleted);
//			*/
//	}
//	
//	
//	
//	
//	public void loadUITransmutationTutorial()
//	{
//		/*
//		if(uiTransmutationTutorialSwf == null)
//		{
//			#if (UNITY_IPHONE || UNITY_EDITOR)
//				if(GameManager.PRINT_LOGS) Debug.Log("IPHONE TRANSMUTATION TUTORIAL");
//				uiTransmutationTutorialSwf = new UI_Tutorial(SFManager.GetScaleformContentPath() +"/UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",SFMgr, CreateMovieCreationParams("/UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf"));
//			#elif UNITY_ANDROID
//				if(GameManager.PRINT_LOGS) Debug.Log("ANDROID TRANSMUTATION TUTORIAL");
//			IDictionary aSwfData = SFSwfsUrl["TransmutationTutorial"+GameManager.instance.deviceRatio] as IDictionary;
//			Byte[] transmutationTutorialSwfContent = getSwfInBytes(aSwfData["url"].ToString());
//			uiTransmutationTutorialSwf = new UI_Tutorial(SFManager.GetScaleformContentPath() +"UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",SFMgr, CreateMovieCreationParams("UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",1,transmutationTutorialSwfContent, new Color(255,255,255,0), true));
//			#endif	
//		}
//		*/
//	}
//
//	public void OnLoadLevelStart()
//	{
//		
//		CreateLevelScene();
//	}
//	
//	public void DestroyLevelHUD()
//	{
//		
//		if(levelScene!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyLevelHUD()");
//		
//			Destroy(levelScene.gameObject);
//			levelScene=null;
//		}
//	}
//	
//	public void DestroyRaidUI()
//	{
//		if(raidUI != null)
//		{
//			Destroy(raidUI.gameObject);
//			raidUI = null;
//		}
//		/*
//		if(raidUI!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyRaidUI()");
//			SFMgr.DestroyMovie(raidUI);
//			
//			raidUI=null;
//		}
//		*/
//	}
//	
//	public void DestroyGuildUI()
//	{
//		/*
//		if(guildUI!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyGildUI()");
//			SFMgr.DestroyMovie(guildUI);
//			
//			guildUI=null;
//		}
//		*/
//	}
//		
//	public void DestroyBattleHUD()
//	{
//		if(hud != null)
//		{
//			Destroy(battleUIPanel.gameObject);
//			battleUIPanel = null;
//			hud = null;
//		}
//	}
//
//	
//	public void DestroyGeneralSWF() 
//	{
//		Debug.Log("DestroyGeneralSWF");
//		if(generalSwf != null)
//		{
//			Destroy(gameuiPanel.gameObject);
//			gameuiPanel = null;
//			generalSwf = null;
//			GameManager.instance.isGeneralSwfLoaded=false;
//		}
//	}
//	
//	public bool CheckFileExistance(string filename)
//	{
//		string url;
//		if(Application.isEditor)
//		{
//			url=Helpers.formatLocalUrlToWrite(filename);
//		}
//		else
//		{
//			url=Helpers.formatLocalPersistentUrlToWrite(filename);
//		}
//		
//		bool success=false;
//		
//		if(File.Exists(url))
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Exists : Check File Existance Function~~~~~~~~~"+filename);
//			success=true;
//		}
//		else
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Does Not Exists : In Check File Existance Function~~~~~~~~~"+filename);
//			success=false;
//		}
//		
//		return success;
//		
//	}
//	
//	public string LoadTextFromWriteablePath(string filename)
//	{
//		string _filepath ;
//		if(Application.isEditor)
//			{
//				_filepath=Helpers.formatLocalUrlToWrite(filename);
//			}
//			else
//			{
//				_filepath=Helpers.formatLocalPersistentUrlToRead(filename);
//			}
//		//string _filepath = Helpers.formatLocalPersistentUrlToRead(filename);
//		//WWW www = new WWW(_filepath);
//		if(GameManager.PRINT_LOGS) Debug.Log("_filepath === " + _filepath);
//		return System.IO.File.ReadAllText(Helpers.formatLocalPersistentUrlToReadWithoutFile(filename));
//		
//		/*filePath = _filepath;
//		if (www.error == null)
//        	return www.text;
//		return null;*/
//	}
//	
//	public void ShowMainScreenMenu(Action singleplayerButtonCB, Action multiplayerButtonCB)
//	{
//		dfControl mainScreenMenu = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load("Prefabs/MainScreenMenu") as GameObject);
//		mainScreenMenu.GetComponent<MainScreenMenu>().Show(singleplayerButtonCB, multiplayerButtonCB);
//	}
//	
//	public void Log(string text)
//	{
//		//logLabel.Text += "\n" + text;
//	}
//	
//	public dfScrollPanel scrollPanel;
////	public void OnGUI()
////	{
////		if(GUI.Button(new Rect(20, 20, 80, 26), "ToggleScroll"))
////		{
////			scrollPanel.BringToFront();
////			scrollPanel.IsInteractive = !scrollPanel.IsInteractive;
////		}
////	}
//}
//
//#else
//
//public class UIManager : Singleton<UIManager> {
//	
//	#region References to other UI 
//	
//	public GameObject gameUIPrefab;
//	public GameObject levelUIPrefab;
//	
//	public const string battleUIPrefabPath = "UIPanels/InGameBattle";
//	
//	//public RaidsUI _raidUIPrefab;
//	public const string _raidUIPrefabPath = "UIPrefabs/RaidsUI";
//	#endregion
//	public bool isPaused = false;
//	public bool	showRingShredsPopUp = false;
//	
//	public GameObject generalSwfGameobject;
//	public GameObject levelSwfGameobject;
//	public GameObject battleSwfGameobject;
//	
//	
//	public UI_Tutorial uiLevelTutorialSwf;
//	
//	// The following swfs need to be replaced... with c# code
//	public UI_HUD hud = null;
//	public UI_Tutorial NewTutorialSwf = null;
//	public UI_Scene_Level levelScene = null;
//	
//	public UI_Scene_MainMenu mainMenu = null;
//	//public UI_Raid 	raidUI	= null;
//	public UI_Guild guildUI	= null;
//	public UI_Tutorial 	uiRingTutorialSwf = null;
//	public UI_Tutorial 	uiRuneTutorialSwf = null;
//	public UI_Tutorial 	uiUpgradeTutorialSwf = null;
//	public UI_Tutorial 	uiTransmutationTutorialSwf = null;
//	public UI_Tutorial 	uiPotionTutorialSwf = null;
//	public UI_Tutorial 	tutorialSwf = null;
//	
//	//========================================================
//	public Camera currentMainCamera;
//	
//	
//	
//	public UI_GeneralSwf generalSwf;
//	
//	public TextAsset languageStringFile;
//	public string _languageSet;
//	public string languageSet
//	{
//		get
//		{
//			return _languageSet;
//		}
//		private set	{	_languageSet = value;}
//	}
//	
//	public string currentLangauge
//	{
//		private set;
//		get;
//	}
//	
//	
//	public dfLabel logLabel;
//	
//	public void Start()
//	{
//		base.Start();
//		isPaused		= false;
//		SetCurrentLanguage(Application.systemLanguage.ToString());
//		Debug.Log("Application.systemLanguage.ToString() = " + Application.systemLanguage.ToString());
//		
//	}
//	
//	public string fileName;
//	private void SetCurrentLanguage(string language)
//	{
//		//return;
//		switch(language)
//		{
//			/*case "Spanish":
//			currentLangauge = "es";
//			fileName = "en_es";
//			break;
//		case "Italian":
//			currentLangauge = "it";
//			fileName = "en_it";
//			break;
//		case "French":
//			currentLangauge = "fr";
//			fileName = "en_fr";
//			break;*/
//		default:
//			currentLangauge = "en";
//			fileName = "en_en";
//			break;
//		}
//		
//		if(CheckFileExistance(fileName+"_new.txt"))
//		{
//			/*if(GameManager.PRINT_LOGS)*/ Debug.Log("file exists at path! filename = " + fileName+"_new.txt");
//			languageSet = LoadTextFromWriteablePath(fileName+"_new.txt");
//		}
//		else
//		{
//			/*if(GameManager.PRINT_LOGS)*/ Debug.Log("file doesnt exists at path! filename = " + fileName+"_new.txt");
//			languageSet = (Resources.Load(fileName) as TextAsset).text;
//		}
//	}
//	
//	public void OnUsernameListener(string username) 
//	{
//		/* if(GameManager.PRINT_LOGS) */ Debug.Log("::::::::::::::::: OnUsernameListener username" + username);
//		if(string.IsNullOrEmpty(username))
//		{
//			DestroyGuildUI();
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: OnUsernameListener username NULL" );
//		}
//		/* else if(guildUI == null)
//		{
//			CreateGuildUI(this.OnGuildUILoaded);
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: GUILD UI ==== NULL" );
//		}*/
//	}
//	
//	private void OnGuildUILoaded()
//	{
//		OnUsernameListener(GameManager._gameState.User.username);
//		if(GameManager.PRINT_LOGS) Debug.Log("Guild UI loaded!");
//	}
//	
//	private GameUI gameuiPanel;
//	public void LoadGeneralSwf(GameManager.GeneralSwfLoaded del)
//	{
//		if(generalSwf == null)
//		{
//			gameuiPanel = gameObject.GetComponent<dfControl>().AddPrefab(gameUIPrefab).gameObject.GetComponent<GameUI>();
//			generalSwf = generalSwfGameobject.GetComponent<UI_GeneralSwf>();
//			generalSwf.generalSwf = gameuiPanel;			
//		}
//		
//		gameuiPanel.SetListener(generalSwf);
//		generalSwf.gSwfDelegate = del;
//		
//		SetLayerOrder();
//	}
//	
//	public void LoadGeneralSwf()
//	{
//		if(generalSwf == null)
//		{
//			gameuiPanel = gameObject.GetComponent<dfControl>().AddPrefab(gameUIPrefab).gameObject.GetComponent<GameUI>();
//			generalSwf = generalSwfGameobject.GetComponent<UI_GeneralSwf>();
//			generalSwf.generalSwf = gameuiPanel;
//			
//			if(UIManager.instance.hud != null)
//			{
//				UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
//				generalSwf.ToggleTopStats(false);
//			}
//		}
//		
//		gameuiPanel.SetListener(generalSwf);
//		
//		SetLayerOrder();
//	}
//	
//	
//	private BattleUIScript battleUIPanel;
//	public void CreateGameHud()
//	{
//		if(hud == null)
//		{
//			battleUIPanel = gameObject.GetComponent<dfControl>().AddPrefab(Resources.Load(battleUIPrefabPath) as GameObject).gameObject.GetComponent<BattleUIScript>();
//			hud = battleSwfGameobject.GetComponent<UI_HUD>();
//			hud.hud = battleUIPanel;
//		}
//		
//		battleUIPanel.SetInterface(hud);
//		
//		SetLayerOrder();
//	}
//	
//	private LevelUIScript leveluiPanel;
//	public void CreateLevelScene()
//	{
//		if(levelScene == null)
//		{
//			leveluiPanel = gameObject.GetComponent<dfControl>().AddPrefab(levelUIPrefab).gameObject.GetComponent<LevelUIScript>();
//			levelScene = levelSwfGameobject.GetComponent<UI_Scene_Level>();
//			levelScene.levelScene = leveluiPanel;
//		}
//		
//		leveluiPanel.SetInterface(levelScene);
//		
//		SetLayerOrder();
//	}
//	
//	
//	private void SetLayerOrder()
//	{
//		//		gameuiPanel.GetComponent<dfPanel>().BringToFront();
//		//		TutorialManager.instance.GetComponent<dfPanel>().BringToFront();
//	}
//	
//	
//	private RaidsUI raidUI;
//	
//	public RaidsUI CreateRaidUI(UI_Raid.LeaderBoardListener leaderboardListener)
//	{
//		raidUI = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load(_raidUIPrefabPath) as GameObject).GetComponent<RaidsUI>();
//		raidUI._leaderboardInterface = leaderboardListener;
//		return raidUI;
//	}
//	
//	public void LoadGuildUI()
//	{
//		/*
//		if(GameManager.PRINT_LOGS) Debug.Log(":::::::::::::::::::::: LoadGuildUI :::::::::::::::::::::::::::::::::::: ");
//		GameManager._gameState.User.usernameChangeEvent += new User.UsernameChangeDelegate(this.OnUsernameListener);
//		OnUsernameListener(GameManager._gameState.User.username);
//		*/
//	}
//	
//	public void CreateGuildUI(System.Action ack)
//	{
//		/*
//		if(guildUI == null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("guildUI NULL ::: CreateGuildUI CALLED");
//			guildUI = new UI_Guild(SFMgr, CreateMovieCreationParams(UI_Guild.GUILD_SWF_NAME, 100), ack);
//		}
//		*/
//	}
//	
//	public void OnLoadBattleStart()
//	{
//		CreateGameHud();
//	}
//	
//	public void LoadVictoryScreen(int healthLost, int battleTime, int topDamage, string grade,bool isBossFight)
//	{
//		showRingShredsPopUp=isBossFight;
//		SoundManager.instance.battleBackGroundPlayer.StopSound();
//		int currentSkullLevelDustValue=0;
//		int dustskullLevel=GameManager._gameState.skullLevel;
//		
//		if(!GameManager.instance.isMultiPlayerMode)
//		{
//			string aiName = GameManager.instance.getEnemyFromGameState(GameManager.instance._levelManager.currentLevel+"_DuelA");
//			if(GameManager._gameState.bossAttemptDictionary.Contains(aiName))
//			{
//				dustskullLevel = GameManager._dataBank.GetModelForMonsterByEncounter(aiName,Int32.Parse(GameManager._gameState.bossAttemptDictionary[aiName].ToString())).skullLevel;
//			}
//		}
//		Buff greedBuff = GameManager._gameState.User.GetBuffForBuffName("greed");
//		if(greedBuff == null) //earth has greed
//			currentSkullLevelDustValue = LootManager.getSoulDust(dustskullLevel);
//		else
//		{
//			currentSkullLevelDustValue = LootManager.getSoulDust(dustskullLevel);
//			if(GameManager.PRINT_LOGS) Debug.Log("currentSkullLevelDustValue" + currentSkullLevelDustValue);
//			currentSkullLevelDustValue += (int)greedBuff.modifierValue;
//			if(GameManager.PRINT_LOGS) Debug.Log("currentSkullLevelDustValue += (int)greedBuff.modifierValue" + currentSkullLevelDustValue);
//		}
//		
//		Time.timeScale=0.0f;
//		generalSwf.ShowVictoryPopup(currentSkullLevelDustValue,healthLost,battleTime,topDamage, grade);
//	}
//	
//	public void LoadLootScreen(string keysReq)
//	{
//		Debug.Log("UIMANAGER >> LoadLootScreen >> keysReq = " + keysReq);
//		
//		Time.timeScale=0.0f;
//		InventorySystem.InventoryItem currentLoot=LootManager.DecideLoot("Loot",GameManager.instance._levelManager._zone,keysReq);
//		if(currentLoot as InventorySystem.ItemRing != null)
//			((InventorySystem.ItemRing) currentLoot).isNew = true;
//		GameManager.instance.scaleformCamera.generalSwf.LoadLoot(currentLoot);
//		levelScene.UpdateTotalKeys(GameManager._gameState.User._inventory.keyRing.keyCount);
//		//generalSwf.ShowLootPopup(10,10);
//		
//	}
//	public void lootPopupClosed()
//	{
//		Time.timeScale=1.0f;
//	}
//	
//	//	public void UnLoadVictoryScreen()
//	//	{
//	//		Time.timeScale=1.0f;
//	//		generalSwf.onVictoryContinue();
//	//	}
//	
//	public void LoadDefeatScreen()
//	{
//		Time.timeScale=0.0f;
//		generalSwf.ShowDefeatPopup();
//	}
//	
//	public void loadNewTutorial()
//	{
//		if(GameManager.instance._levelManager.currentLevel=="StatuePath")
//		{
//			TutorialManager.instance.decideInLevelTutorial(false);
//		}
//		else
//		{
//			TutorialManager.instance.decideInLevelTutorial(true);
//		}
//	}
//	
//	
//	public void loadUILevelTutorial()
//	{
//		Debug.Log(":::::::::::::::        loadUILevelTutorial :::::::::::::::");
//		
//		float cameraAspect = (float)Screen.width/(float)Screen.height;
//		Debug.Log("CameraAspect ==> " + cameraAspect);
//		
//		if(cameraAspect > 1.7f && cameraAspect < 1.8f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_16x9);
//			SetLayerOrder();
//		}
//		
//		else if(cameraAspect > 1.3f && cameraAspect < 1.4f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_4x3);
//			SetLayerOrder();
//		}
//		
//		else if(cameraAspect > 1.45f && cameraAspect < 1.55f)
//		{
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.LevelTutorialStart_3x2);
//			SetLayerOrder();
//		}
//	}
//	
//	public bool isResolution3by2
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.45f && cameraAspect < 1.55f)
//				return true;
//			return false;
//		}
//	}
//	
//	public bool isResolution4by3
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.3f && cameraAspect < 1.4f)
//				return true;
//			return false;
//		}
//	}
//	
//	public bool isResolution16by9
//	{
//		get
//		{
//			float cameraAspect = (float)Screen.width/(float)Screen.height;
//			if(cameraAspect > 1.7f && cameraAspect < 1.8f)
//				return true;
//			return false;
//		}
//	}
//	
//	public void onPotionTutorialEnd()
//	{
//		/*
//		if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.PotionTutorialStart)
//			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.PotionTutorialCompleted);
//			*/
//	}
//	
//	
//	
//	
//	public void loadUITransmutationTutorial()
//	{
//		/*
//		if(uiTransmutationTutorialSwf == null)
//		{
//			#if (UNITY_IPHONE || UNITY_EDITOR)
//				if(GameManager.PRINT_LOGS) Debug.Log("IPHONE TRANSMUTATION TUTORIAL");
//				uiTransmutationTutorialSwf = new UI_Tutorial(SFManager.GetScaleformContentPath() +"/UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",SFMgr, CreateMovieCreationParams("/UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf"));
//			#elif UNITY_ANDROID
//				if(GameManager.PRINT_LOGS) Debug.Log("ANDROID TRANSMUTATION TUTORIAL");
//			IDictionary aSwfData = SFSwfsUrl["TransmutationTutorial"+GameManager.instance.deviceRatio] as IDictionary;
//			Byte[] transmutationTutorialSwfContent = getSwfInBytes(aSwfData["url"].ToString());
//			uiTransmutationTutorialSwf = new UI_Tutorial(SFManager.GetScaleformContentPath() +"UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",SFMgr, CreateMovieCreationParams("UITutorial" +GameManager.instance.deviceRatio+"/TransmutationTutorial.swf",1,transmutationTutorialSwfContent, new Color(255,255,255,0), true));
//			#endif	
//		}
//		*/
//	}
//	
//	public void OnLoadLevelStart()
//	{
//		
//		CreateLevelScene();
//	}
//	
//	public void DestroyLevelHUD()
//	{
//		
//		if(levelScene!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyLevelHUD()");
//			
//			Destroy(levelScene.gameObject);
//			levelScene=null;
//		}
//	}
//	
//	public void DestroyRaidUI()
//	{
//		if(raidUI != null)
//		{
//			Destroy(raidUI.gameObject);
//			raidUI = null;
//		}
//		/*
//		if(raidUI!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyRaidUI()");
//			SFMgr.DestroyMovie(raidUI);
//			
//			raidUI=null;
//		}
//		*/
//	}
//	
//	public void DestroyGuildUI()
//	{
//		/*
//		if(guildUI!=null)
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyGildUI()");
//			SFMgr.DestroyMovie(guildUI);
//			
//			guildUI=null;
//		}
//		*/
//	}
//	
//	public void DestroyBattleHUD()
//	{
//		if(hud != null)
//		{
//			Destroy(battleUIPanel.gameObject);
//			battleUIPanel = null;
//			hud = null;
//		}
//	}
//	
//	
//	public void DestroyGeneralSWF() 
//	{
//		Debug.Log("DestroyGeneralSWF");
//		if(generalSwf != null)
//		{
//			Destroy(gameuiPanel.gameObject);
//			gameuiPanel = null;
//			generalSwf = null;
//			GameManager.instance.isGeneralSwfLoaded=false;
//		}
//	}
//	
//	public bool CheckFileExistance(string filename)
//	{
//		string url;
//		if(Application.isEditor)
//		{
//			url=Helpers.formatLocalUrlToWrite(filename);
//		}
//		else
//		{
//			url=Helpers.formatLocalPersistentUrlToWrite(filename);
//		}
//		
//		bool success=false;
//		
//		if(File.Exists(url))
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Exists : Check File Existance Function~~~~~~~~~"+filename);
//			success=true;
//		}
//		else
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Does Not Exists : In Check File Existance Function~~~~~~~~~"+filename);
//			success=false;
//		}
//		
//		return success;
//		
//	}
//	
//	public string LoadTextFromWriteablePath(string filename)
//	{
//		string _filepath ;
//		if(Application.isEditor)
//		{
//			_filepath=Helpers.formatLocalUrlToWrite(filename);
//		}
//		else
//		{
//			_filepath=Helpers.formatLocalPersistentUrlToRead(filename);
//		}
//		//string _filepath = Helpers.formatLocalPersistentUrlToRead(filename);
//		//WWW www = new WWW(_filepath);
//		if(GameManager.PRINT_LOGS) Debug.Log("_filepath === " + _filepath);
//		return System.IO.File.ReadAllText(Helpers.formatLocalPersistentUrlToReadWithoutFile(filename));
//		
//		/*filePath = _filepath;
//		if (www.error == null)
//        	return www.text;
//		return null;*/
//	}
//	
//	public void ShowMainScreenMenu(Action singleplayerButtonCB, Action multiplayerButtonCB)
//	{
//		dfControl mainScreenMenu = gameObject.GetComponent<dfPanel>().AddPrefab(Resources.Load("Prefabs/MainScreenMenu") as GameObject);
//		mainScreenMenu.GetComponent<MainScreenMenu>().Show(singleplayerButtonCB, multiplayerButtonCB);
//	}
//	
//	public void Log(string text)
//	{
//		//logLabel.Text += "\n" + text;
//	}
//	
//	public dfScrollPanel scrollPanel;
//	//	public void OnGUI()
//	//	{
//	//		if(GUI.Button(new Rect(20, 20, 80, 26), "ToggleScroll"))
//	//		{
//	//			scrollPanel.BringToFront();
//	//			scrollPanel.IsInteractive = !scrollPanel.IsInteractive;
//	//		}
//	//	}
//}
//
//#endif
