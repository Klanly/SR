using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameStateModule;

using DatabankSystem;
using FRAG;
using System;
//using Chartboost;

public class GameManager : Singleton<GameManager>
{
	#region Variables...
	
	public static bool PRINT_LOGS = true;
	
	public const int UNLOCK_RUNE_DAY = /*1; //*/ 3;
	public const int UNLOCK_TRANSMUTATION_DAY =/* 1; //*/ 4;
	public const int UNLOCK_SPIRITS_DAY = /*1; //*/ 5;
	public const int UNLOCK_UPGRADES_DAY = /*1; //*/ 2;
	
	public static bool enableAnalytics = true;
	
	public int runeProgress {
		set {
			_gameState.runeProgress = value;
		}
		get {
			return _gameState.runeProgress;
		}
	}
	
	public int spiritProgress {
		set {
			_gameState.spiritProgress = value;
		}
		get {
			return _gameState.spiritProgress;
		}
	}

	public const int NUMBER_OF_LEVELS = 30; 
	public const string APP_VERSION = "1.0.3";
	
	public const float HEAL_PER_INTERVAL = 0.06f;
	public const float HEAL_INTERVAL = 15.0f;
	
	public List<InventorySystem.ItemRing> suggestedRingList = new List<InventorySystem.ItemRing> ();
	public List<InventorySystem.ItemRune> suggestedRuneList = new List<InventorySystem.ItemRune> ();
	
	#region suggested lists for new UI
	public List<InventorySystem.ItemRing> suggestedUIRingList = new List<InventorySystem.ItemRing> ();
	public List<InventorySystem.ItemRune> suggestedUIRuneList = new List<InventorySystem.ItemRune> ();
	#endregion
	public IDictionary prefabsDic;
	//static System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
	
	private enum GamestateStatus
	{
		kNone,
		kNetwork,
		kLocal,
		kReady}
	;
	GamestateStatus currentStatus = GamestateStatus.kNone;
	
	public NetworkingNEW fragNetworkingNew;
	
	public fpscounter fpsCounter;
	
	public static GameState _gameState = null;
	
	private GameState networkGameState = null;
	private GameState localGameState = null;
	
	private GameStateHandler _gameStateHandler = null;
	
	public delegate void GameStateProcessDelegate (GameState gState,string filename,bool success);
	
	public delegate void GameStateReadDelegate (GameState gState);
	
	public delegate void FileLoadedIntoPersistant (string filename);
	
	public delegate void GeneralSwfLoaded (bool isLoaded);
	
	public const string DATA_BANK_FILE_NAME = "DataBank.txt";
	
	public bool isResetGameToDefault = false;
	//public bool isDayCountIncremented = false;
	public TouchScreenKeyboard keyBoard;
	public delegate void KeyBoardInputListener (string input);
	public KeyBoardInputListener keyBoardInputListener;
	
	string tempKeyBoardText = "";
	
	public GameStateReadDelegate _gStateReadDelegate;
	
	public UIManager scaleformCamera;
	
	
	private DatabankLoader dbLoader;
	
	public string currentMultiPlayerLevel;
	public bool isMultiPlayerMode;
	
	private LevelManager levelManager;
	public LevelManager _levelManager {
		get {
			//			if(isMultiPlayerMode)
			//			{
			//				return (MultiPlayerLevelManager)levelManager;
			//			}
			return levelManager;
		}
		set {
			levelManager = value;
		}
		
	}
	
	public MonoHelpers _monoHelpers;
	//    {
	//        private set;
	//        get;
	//    }
	
	public MotionMaker motionMaker {
		private set;
		get;
		
	}
	
	public static Databank _dataBank;
	
	public bool isGameLoadedForFirstTime;   //variable to check if data is to be loaded from gamestate or databank
	
	#endregion	
	
	public string deviceRatio = "";
	
	public bool isGeneralSwfLoaded = false;
	
//	private static long _serverTime = -1;
//	public static long offset = 0;
//	
//	public long ServerTime
//	{
////		get
////		{
////			DateTime dtNow = DateTime.UtcNow;
////			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
////			var time = posixTime.AddMilliseconds(offset);
////			var diff = dtNow - time;
////			return (long)diff.TotalMilliseconds;
////		}
////		set{
////			DateTime dtNow = DateTime.UtcNow;
////			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
////			var time = posixTime.AddMilliseconds(value);
////			var diff = dtNow - time;
////			offset = (long)diff.TotalMilliseconds;
////			_serverTime = value;
////		}
//		get
//		{
//			return _serverTime;
//		}
//		set{
//			_serverTime = value;
////			Debug.LogWarning("server timer updated by 1.0f");
////			isServerTimeFetched = true;
//			StartServerTimer();
//		}
//	}
//
//	private void StartServerTimer() {
////		Debug.LogWarning("StartServerTimer timer updated by 1.0f");
//
//		StopCoroutine("StartServerTimerRoutine");
//		StartCoroutine("StartServerTimerRoutine");
//	}
//
//	public IEnumerator StartServerTimerRoutine ()
//	{
//		while(true) {
//			yield return new WaitForSeconds (1.0f);
////			Debug.LogWarning("server timer updated by 1.0f");
//			_serverTime+= 1000;
//		}
//	}
		public void OnGeneralSwfLoaded (bool isLoaded)
	{
		//SetLanguageGeneralSwf(sfCamera.languageSet, GameManager.instance.currentLangauge);
		
		scaleformCamera.generalSwf.LoadLoadingScreen ();
		isGeneralSwfLoaded = true;
		if (PRINT_LOGS)
			Debug.LogError ("~~~~~~~~~~~~~~~~~~~SHOWING LOADING SCREEN IN GAME MANAGER~~~~~~~~~~~~~~~~~~~~~");
		scaleformCamera.LoadGuildUI ();
	}

	public void OnGeneralSwfReloaded (bool isLoaded)
	{
		//scaleformCamera.generalSwf.LoadLoadingScreen();


		isGeneralSwfLoaded = true;
		if (PRINT_LOGS)
			Debug.Log ("~~~~~~~~~~~~~~~~~~~GENERAL SWF RELOADED IN GAME MANAGER~~~~~~~~~~~~~~~~~~~~~");
		Time.timeScale = 1.0f;
		scaleformCamera.generalSwf.Init ();
		if (levelManager.isEnemyClicked) {
			UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
			scaleformCamera.generalSwf.ToggleTopStats (false);
		}
		if (levelManager.currentLevel.Equals ("BeachCamp")) {
			scaleformCamera.generalSwf.DisplayCenterButton (false);
			
			UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
			scaleformCamera.generalSwf.ToggleTopStats (false);
			if (scaleformCamera.levelScene != null) {
				scaleformCamera.levelScene.SetDisplayVisible (false);
			}
		}
	}
	
	public IEnumerator LoadGeneralSwf ()
	{
		yield return new WaitForSeconds (3.0f);
		scaleformCamera.LoadGeneralSwf (GameManager.instance.OnGeneralSwfReloaded);
//		System.Diagnostics.Process myprocess = System.Diagnostics.Process.Start("");
//		myprocess.Close();
	}
	
	protected void Awake ()
	{
		fragNetworkingNew = InitGameVersions.instance._fragNetworkingNew;
		_monoHelpers = InitGameVersions.instance._myHelper;
		
		fragNetworkingNew._loginResponse = LoginResponseHandler;
		fragNetworkingNew._noConnectivityListener = OnInternetConnectionFailed;
//		AMQP.Client.onUnableToConnect = OnInternetConnectionFailed;
	}
	
	protected override void Start ()
	{
		ServerManager ins = ServerManager.Instance;
		base.Start ();
		
		//        if (PRINT_LOGS) Debug.Log("GameManager Start");
		
		scaleformCamera = UIManager.instance;
		
		CheckDeviceRatio ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		NotificationManager.RequestPermission();

		//Register Analytics
		//Analytics.registerAnalyticsManager(new FlurryManager("7HT728SJP4MB8DDRPHY9"));
		//		Debug.Log("Before Loading General Swf");
		if (scaleformCamera.generalSwf == null) {
			GameManager.instance.scaleformCamera.LoadGeneralSwf (OnGeneralSwfLoaded);
			if (PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~LoadGeneralSwf~~~~~~~~~~~~~~~~~~~~~~~~~~~");
		} else {
			isGeneralSwfLoaded = true;
		}
		//		Debug.Log("After Loading General Swf");
		
		motionMaker = gameObject.GetComponent<MotionMaker> ();
		_gameStateHandler = gameObject.GetComponent<GameStateHandler> ();
		dbLoader = gameObject.GetComponent<DatabankLoader> ();
		
		//		fragNetworking = gameObject.AddComponent<Networking>();
		
		fpsCounter = gameObject.AddComponent<fpscounter> ();
//		gameObject.AddComponent<Helper> ();
		
		LoginResponseHandler (InitGameVersions.LoginResponseParams.response, InitGameVersions.LoginResponseParams.error, InitGameVersions.LoginResponseParams.request);
		//		fragNetworking.Invoke("Initialize", 1.0f);
	}
	
	
	private void OnInternetConnectionFailed ()
	{
		Debug.Log ("OnInternetConnectionFailed");
		LoginResponseHandler (null, new object (), null);
	}
	
	private void LoadDataBank ()
	{
		if (PRINT_LOGS)
			Debug.LogError ("LoadDataBank - _isFirstRun "+_isFirstRun);
		if (_isFirstRun && !Application.isEditor) {	
			StartCoroutine (_monoHelpers.LoadIntoPersistantDataPathFromLocalPath ("DataBank.txt", this.OnFileWrittenToPersistantDataPath));
			_isFirstRun = false;
		} else {
			LoadDataBank (fragNetworkingNew.IsConnected);
		}
	}
	
	private void LoadDataBank (bool checkVersion)
	{
		Debug.Log ("LoadDataBank WITH VERSION + checkVersion = " + checkVersion);
		if (checkVersion) {
			dbLoader.LoadDatabank (VersionReceived, true);
		} else {
			dbLoader.LoadDatabank (this.OnDatabaseReceived, false);
		}
	}
	
	public void VersionReceived (Databank dbBank)
	{
		Debug.Log ("VersionReceived > " + dbBank.version);
		ServerManager.Instance.LoadMetaData (LoadMetaDataHandler);
	}
	
	private void OnDatabaseReceived (Databank dbBank)
	{
		//		Debug.Log("OnDatabaseReceived");
		GameManager._dataBank = dbBank;
		_gameStateHandler.Subscribe (OnGameStateLoadedFromDisk);
		
		if (_monoHelpers.CheckFileExistance ("CurrentGameState.txt")) {
			this.RequestGameState ();
		} else if (_monoHelpers.CheckFileExistance ("LastGameState.txt")) {
			this.RequestGameState ();
		} else {
			StartCoroutine (_monoHelpers.LoadIntoPersistantDataPathFromLocalPath ("CurrentGameState.txt", this.OnFileWrittenToPersistantDataPath));
		}
		
		LoadPrefabsDictionary ();
	}
	
	
	private void SaveGameStateDelayed ()
	{
		if (PRINT_LOGS)
			Debug.Log ("SAVE delayedddddddddddddd callled ::::::::::");
		fragNetworkingNew.SaveGameState (MiniJSON.Json.Deserialize (GameStateParser.ConvertGameStateToJson (_gameState)) as IDictionary);
	}
	
	
	
	//	private void OnGameStateReceived(GameState gameState)
	//	{	
	//
	//		Debug.LogError("OnGameStateReceived >> gameState >>" + gameState.version);
	//		_gameState = gameState;
	//		
	//		if(isResetGameToDefault)
	//		{
	//			TutorialManager.instance.tutorialStatusDictionary = GameManager._gameState.tutorialStatusDictionary;
	//			ShrineManager.Instance.LoadShrineState(GameManager._gameState.shrineList);
	//			ReturnToCamp();
	//			isResetGameToDefault = false;
	//		}
	//		//scaleformCamera.levelScene.UpdateSoulDust();
	//	}
	
	private bool LoadGameFromNetwork ()
	{
		Debug.Log ("LoadGameFromNetwork");
		//		Networking.RpcResponse response = new Networking.RpcResponse(LoadGameFromNetworkHandler, null);
		fragNetworkingNew.LoadGameState (LoadGameFromNetworkHandler, LoadGameFromNetworkHandler);
		//        if (PRINT_LOGS) Debug.Log("--------------------------!!!!!!!!!!!!!!!!!-----------------!!!!!!!!!!!!!!!!!!!!!!------------------------!!!!!!!!!!!!!!!!!!!!!!!!");
		return true;
	}
	
	public void LoadGameFromNetworkHandler (object response, object error, ServerRequest request)
	{
		IDictionary resDictionary = response as IDictionary;
		
		
		Debug.Log ("BEFORE CURRENT STATUS = " + currentStatus + "< LoadGameFromNetworkHandler res > " + MiniJSON.Json.Serialize (resDictionary));
		if (error == null) {
			GameManager._gameState = new GameState ();
			try {
				//					Debug.Log("~~~~~~~~~~Process Game State Request from network~~~~~~~~~~~~");
				//PlayerPrefs.DeleteAll();
				IDictionary gameStateDictionary = response as IDictionary;
				GameStateParser.ProcessGameState (this.OnGameStateLoadedFromNetwork, MiniJSON.Json.Serialize (gameStateDictionary ["gameState"]), new GameState (), "");
			} catch (System.Exception ex) {
				Debug.Log ("GameStateParser.ProcessGameState error: " + ex.ToString ());
				networkGameState = null;
				switch (currentStatus) {
				case GamestateStatus.kLocal:
					currentStatus = GamestateStatus.kReady;
					break;
				case GamestateStatus.kReady:
					break;
				case GamestateStatus.kNone:
					currentStatus = GamestateStatus.kNetwork;
					break;
				}
			}
		} else {
			Debug.Log ("Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*ZZ*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*Z*ZZ*Z*Z*Z*Z*Z*Z*Z*Z*Z*");
			networkGameState = null;
			switch (currentStatus) {
			case GamestateStatus.kLocal:
				currentStatus = GamestateStatus.kReady;
				break;
			case GamestateStatus.kReady:
				break;
			case GamestateStatus.kNone:
				currentStatus = GamestateStatus.kNetwork;
				break;
			}
		}
	}
	
	private void LoginResponseHandler (object response, object error, ServerRequest request)
	{
		//		fragNetworking._loginResponse = null;
		StopCoroutine("CheckNetworkInactivity");
//		Debug.LogError ("FIRST RUN - _isFirstRun "+_isFirstRun);

		if (_isFirstRun && !Application.isEditor) {	
//			Debug.LogError ("FIRST RUN");


			// TODO Danger!!!
			StartCoroutine (SetGameStateToDefaultRoutine ());

			StartCoroutine (_monoHelpers.LoadIntoPersistantDataPathFromLocalPath ("DataBank.txt", (filenameWritten) => {
				_isFirstRun = false;
				
				if (error == null) {
					if((response as IDictionary).Contains("errorCode")) {
						string errorCode = (response as IDictionary)["errorCode"].ToString();
						if(errorCode.Equals("ADMIN000")) {
							if(UIManager.instance.generalSwf != null && UIManager.instance.generalSwf.generalSwf != null){
								UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Server Offline", "Game is on maintenance, sorry for interruption.", () => {});
							}
						}
						StartCoroutine("CheckNetworkInactivity");
						ContinueLoadingGame (int.MinValue);
					} else {

						ContinueLoadingGame (int.Parse ((response as IDictionary) ["metaDataVersion"].ToString ()));
						if((response as IDictionary).Contains("serverTimeStamp")) {
							Debug.LogError("contains server timestatmp "+(response as IDictionary) ["serverTimeStamp"].ToString ());
						}
						InitGameVersions.instance.ServerTime = long.Parse ((response as IDictionary) ["serverTimeStamp"].ToString ());
					}
				} else {
					StartCoroutine("CheckNetworkInactivity");
					ContinueLoadingGame (int.MinValue);
				}
			}));
		} else {
			//			Debug.Log("NOT FIRST RUN");
			
			if (error == null) {
				if((response as IDictionary).Contains("errorCode")) {
					string errorCode = (response as IDictionary)["errorCode"].ToString();
					if(errorCode.Equals("ADMIN000")) {
						if(UIManager.instance.generalSwf != null && UIManager.instance.generalSwf.generalSwf != null){
							UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Server Offline", "Game is on maintenance, sorry for interruption.", () => {});
						}
					}
					StartCoroutine("CheckNetworkInactivity");
					ContinueLoadingGame (int.MinValue);
				} else {
					ContinueLoadingGame (int.Parse ((response as IDictionary) ["metaDataVersion"].ToString ()));
				}
			} else {
				StartCoroutine("CheckNetworkInactivity");
				ContinueLoadingGame (int.MinValue);
			}
		}
	}

	IEnumerator CheckNetworkInactivity() {
		//			yield break;
		Debug.LogWarning("CheckNetworkInactivity GM");
		while(true) {
			yield return new WaitForSeconds(30.0f);
			Debug.LogWarning("Trying to connect again GM");
			GameManager.instance.fragNetworkingNew.Connect();
			yield break;
		}
	}


	private void ContinueLoadingGame (int serverDbVersion)
	{
		Debug.Log ("ContinueLoadingGame > " + serverDbVersion);
		
		dbLoader.LoadDatabank ((dBank) => {
//			string sample = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ:;<>";
			Debug.Log ("dBank in streaming assets > " + dBank.version+" gversion "+dBank.gamestateVersion);
//			Debug.LogError ("ciphered with shift of 5 "+GameUtils.Caesar(sample, 5));
//			string ciphered = GameUtils.Caesar(sample, 5);
//			Debug.LogError ("deciphered with shift of 5 "+GameUtils.Caesar(ciphered, -5));
//
//			Debug.LogError ("ciphered with encode of 5 "+GameUtils.Base64Encode(sample));
//			ciphered = GameUtils.Base64Encode(sample);
//			Debug.LogError ("deciphered with decode of 5 "+GameUtils.Base64Decode(ciphered));


			if (int.Parse (dBank.version) >= serverDbVersion) {
				Debug.Log ("ContinueLoadingGame > " + serverDbVersion + "  > dBank.version > " + dBank.version);
				OnDatabaseReceived (dBank);
			} else {
				Debug.Log ("Local version smaller than server!");
				ServerManager.Instance.LoadMetaData (LoadMetaDataHandler);
			}
		}, false);
	}
	
	private void OnGameStateLoadedFromDisk (GameState gameState)
	{
		if (_gameState != null)
			return;
		
		Debug.Log ("GAME STATE LOADED FROM DISK > " + gameState.version);
		localGameState = gameState;
		
		//currentStatus = GamestateStatus.kNetwork;
		
		Debug.Log ("BEFORE currentStatus > " + currentStatus);
		
		switch (currentStatus) {
			
		case GamestateStatus.kNetwork:
			currentStatus = GamestateStatus.kReady;
			break;
		default:
			currentStatus = GamestateStatus.kLocal;
			break;
		}
		
		Debug.Log ("AFTER currentStatus > " + currentStatus);
		
		//		_gameStateHandler.Subscribe(OnGameStateReceived);
		Debug.LogError("server = "+InitGameVersions._gamestateVersion +" - local"+localGameState.version);
		if (InitGameVersions._gamestateVersion <= localGameState.version) {
			Debug.Log ("local gamestate is Equal/Greater than server gamestate version");
			currentStatus = GamestateStatus.kReady;
		} else {
			Debug.Log ("local gamestate version is LESS than server gamestate version.");
			LoadGameFromNetwork ();
		}
	}
	
	private void OnGameStateLoadedFromNetwork (GameState gameState, string filename, bool success)
	{
		networkGameState = gameState;
		Debug.Log ("GAME STATE LOADED FROM NETWORK networkGameState > version > " + networkGameState.version + "   currentStatus > " + currentStatus);
		switch (currentStatus) {
		case GamestateStatus.kLocal:
			currentStatus = GamestateStatus.kReady;
			break;
		default:
			currentStatus = GamestateStatus.kNetwork;
			break;
		}
	}
	
	public bool allowTouch = true;
	private bool isServerTimeFetched = false;

	void Update ()
	{	
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (_levelManager != null)
			{
				_levelManager.OnApplicationQuit();
			} else
			{
				Application.Quit();
			}
		}
		#endif		
		if (currentStatus == GamestateStatus.kReady) {
			currentStatus = DecideGamestateAndProceed ();
			Debug.Log ("~~~GameState is Ready~~~ Decided gamestate >> currentStatus >> " + currentStatus);
			SaveGameState (true);
			// soundmanager settings start
			SoundManager.instance.SetMVolume (GameManager._gameState.musicVolume);
			SoundManager.instance.SetSfxVolume (GameManager._gameState.gfxVolume);
			// end
			
			InvokeRepeating ("HealPlayer", 30.0f, Databank.MULTIPLAYER_HEAL_INTERVAL);
			
			enableAnalytics = true;
			
			if (Application.loadedLevelName != "LevelScene")
				Application.LoadLevelAsync ("LevelScene");
		}
		
		if (keyBoard != null) {
			
			if (keyBoard.done) {
				if (tempKeyBoardText != keyBoard.text) {
					if (keyBoardInputListener != null)
						keyBoardInputListener (keyBoard.text);
					
					tempKeyBoardText = keyBoard.text;
					if (PRINT_LOGS)
						Debug.Log ("Skull Level from keyboard: " + keyBoard.text);
					//scaleformCamera.generalSwf.setText(keyBoard.text);
				}
			}
		}
	}
	
	public bool allowHealOverTime = true;
	public System.Action multiplayerHealListener;
	private void HealPlayer ()
	{
		if (!allowHealOverTime)
			return;
		
		_gameState.User.multiplayerLife += Mathf.CeilToInt (Databank.PERCENT_MULTIPLAYER_HEAL_AMOUNT * (float)_gameState.User.totalLife);
		SaveGameState (false);
		if (multiplayerHealListener != null)
			multiplayerHealListener ();
	}
	
	private GamestateStatus DecideGamestateAndProceed ()
	{
		GameState tempGamestate;
		//networkGameState = null;
		if (networkGameState == null) {
			tempGamestate = localGameState;
			currentStatus = GamestateStatus.kLocal;
		} else {
			Debug.Log ("NETWORK version" + networkGameState.version + " LOCAL version" + localGameState.version);
			if (networkGameState.version > localGameState.version) {
				//if(false)
				tempGamestate = networkGameState;
				currentStatus = GamestateStatus.kNetwork;
			} else {
				tempGamestate = localGameState;
				currentStatus = GamestateStatus.kLocal;
			}
		}
		
		GameManager._gameState = tempGamestate;
		_tempGameState = GameManager._gameState;
		TutorialManager.instance.tutorialStatusDictionary = GameManager._gameState.tutorialStatusDictionary;
		ShrineManager.Instance.LoadShrineState (GameManager._gameState.shrineList);
		
		SaveGameState (false);
		
		//		LoadLanguageFromBundle();
		return currentStatus;
	}
	public GameState _tempGameState;
	private void LoadLanguageFromBundle ()
	{
		gameObject.AddComponent<AssetBundleLoader> ();
		AssetBundleLoader thisBundleLoader = gameObject.GetComponent<AssetBundleLoader> ();
		thisBundleLoader.SetDelegate (this.OnLanguageBundleReceived);
		thisBundleLoader.InvokeLoadAssetBundle (scaleformCamera.fileName);
	}
	
	private void OnLanguageBundleReceived (AssetBundle bundle)
	{
		TextAsset textAsset = bundle.mainAsset as TextAsset;
		string languageText = textAsset.text;
		
		Debug.Log ("bundle.mainAsset.name = " + bundle.mainAsset.name);
		
		BundleDownloadManager.instance.myHelper.WriteIntoPersistantDataPath (languageText, bundle.mainAsset.name + "_new.txt");
	}
	
	public void RequestGameState ()
	{
		_gameStateHandler.GetGameState ();
	}
	
	public void SaveGameState (bool isSaveTosServer)
	{
		//		Debug.Log("SaveGameState ... LOCALLY ONLY = " + !isSaveTosServer);
		if (HasInstance)
			StartCoroutine (SaveGameSateRoutine (isSaveTosServer));
	}
	
	public void SetGameStateToDefault ()
	{
		isResetGameToDefault = true;
		StartCoroutine (SetGameStateToDefaultRoutine ());
	}
	
	
	IEnumerator SetGameStateToDefaultRoutine ()
	{
		StartCoroutine (_monoHelpers.CopyDefaultGameStateToCurrentGameState ());
		
		yield return null;
	}
	
	IEnumerator SaveGameSateRoutine (bool isSaveTosServer)
	{
		if (PRINT_LOGS)
			Debug.Log ("~~~~~~~~~Save Game State Called !!! ~~~~~~~~~~ ");
		StartCoroutine (_monoHelpers.CopyIntoAnotherFileOnPersistantDataPath ("CurrentGameState.txt", "LastGameState.txt"));
		//_gameState.timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
		
		_gameState.version++;
		if (isSaveTosServer) {
			Debug.Log ("Save Gamestate to server!!!");
			//			Networking.RpcResponse response = new Networking.RpcResponse(SaveGameStateHandler, null);
			fragNetworkingNew.SaveGameState (MiniJSON.Json.Deserialize (GameStateParser.ConvertGameStateToJson (_gameState)) as IDictionary);
			PurchaseManager.Instance.LogTranactions ();
		}
		
		_gameStateHandler.SaveGameState (_gameState);
		
		yield return null;
	}
	
	public void SaveGameStateHandler (object response, object error, ServerRequest request)
	{
	}
	
	private void OnFileWrittenToPersistantDataPath (string filename)
	{
		if (filename == "CurrentGameState.txt") {
			this.RequestGameState ();
		} else if (filename == "DataBank.txt") {
			if (PRINT_LOGS)
				Debug.Log ("Loading DataBank");
			LoadDataBank (fragNetworkingNew.IsConnected);
		}
	}
	
	#region Suggested items generation
	private string _suggestedItemsJson;
	private static string[] availableElementTags = {"F","L","E","W"};
	private static string[] availableNonelementTags = {"H","D"};
	
	private string GetRandomTagFromAvailable (bool elementOnly)
	{ 
		if (elementOnly)
			return availableElementTags [new System.Random ().Next (0, availableElementTags.Length)];
		return availableNonelementTags [new System.Random ().Next (0, availableNonelementTags.Length)];
	}
	
	public InventorySystem.InventoryItem [] suggestedItems {
		get {
			suggestedUIRingList.Clear ();
			suggestedUIRuneList.Clear ();
			
			//int indexInInventory = InventorySystem.Inventory.startIndexForSuggested;
			int targetLevelOne;
			int targetLevelTwo;
			int targetLevelThree;
			int targetLevelFour;
			int targetLevelFive;
			
			targetLevelOne = _gameState.skullLevel;
			if (targetLevelOne >= NUMBER_OF_LEVELS)
				targetLevelOne = NUMBER_OF_LEVELS;
			
			targetLevelTwo = _gameState.skullLevel + 3;
			if (targetLevelTwo >= NUMBER_OF_LEVELS)
				targetLevelTwo = NUMBER_OF_LEVELS;
			
			targetLevelThree = _gameState.skullLevel + 5;
			if (targetLevelThree >= NUMBER_OF_LEVELS)
				targetLevelThree = NUMBER_OF_LEVELS;
			
			targetLevelFour = _gameState.skullLevel + 7;
			if (targetLevelFour >= NUMBER_OF_LEVELS)
				targetLevelFour = NUMBER_OF_LEVELS;
			
			targetLevelFive = _gameState.skullLevel + 6;
			if (targetLevelFive >= NUMBER_OF_LEVELS)
				targetLevelFive = NUMBER_OF_LEVELS;
			
			InventorySystem.ItemRing levelOneRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, GetRandomTagFromAvailable (true));
			if (levelOneRing != null) {
				levelOneRing.isSuggested = true;
				levelOneRing.uid = Nonce.GetUniqueID ();
				suggestedUIRingList.Add (levelOneRing);
			}
			
			InventorySystem.ItemRing levelTwoRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, "H");
			if (levelTwoRing != null) {
				levelTwoRing.isSuggested = true;
				levelTwoRing.uid = Nonce.GetUniqueID ();
				suggestedUIRingList.Add (levelTwoRing);
			}
			
			
			InventorySystem.ItemRing levelThreeRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, "D");
			if (levelThreeRing != null) {
				levelThreeRing.isSuggested = true;
				levelThreeRing.uid = Nonce.GetUniqueID ();
				suggestedUIRingList.Add (levelThreeRing);
			}
			
			InventorySystem.ItemRing levelFourRing = Databank.GetRandomPremiumRingForSkullLevel (_dataBank, targetLevelFive, GetRandomTagFromAvailable (true));
			if (levelFourRing != null) {
				levelFourRing.isSuggested = true;
				levelFourRing.uid = Nonce.GetUniqueID ();
				suggestedUIRingList.Add (levelFourRing);
			}
			
			InventorySystem.ItemRing levelFiveRing = Databank.GetRandomPremiumRingForSkullLevel (_dataBank, targetLevelFive, "S");
			if (levelFiveRing != null) {
				levelFiveRing.isSuggested = true;
				levelFiveRing.uid = Nonce.GetUniqueID ();
				suggestedUIRingList.Add (levelFiveRing);
			}
			
			InventorySystem.ItemRune levelOneRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "F");
			if (levelOneRune != null) {
				levelOneRune.isSuggested = true;
				levelOneRune.uid = Nonce.GetUniqueID ();
				suggestedUIRuneList.Add (levelOneRune);
			}
			
			InventorySystem.ItemRune levelTwoRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "L");
			if (levelTwoRune != null) {
				levelTwoRune.isSuggested = true;
				levelTwoRune.uid = Nonce.GetUniqueID ();
				suggestedUIRuneList.Add (levelTwoRune);
			}
			
			
			InventorySystem.ItemRune levelThreeRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "W");
			if (levelThreeRune != null) {
				levelThreeRune.isSuggested = true;
				levelThreeRune.uid = Nonce.GetUniqueID ();
				suggestedUIRuneList.Add (levelThreeRune);
			}
			
			
			InventorySystem.ItemRune levelFourRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "E");
			if (levelFourRune != null) {
				levelFourRune.isSuggested = true;
				levelFourRune.uid = Nonce.GetUniqueID ();
				suggestedUIRuneList.Add (levelFourRune);
			}
			
			
			InventorySystem.ItemRune levelFiveRune = Databank.GetRandomPremiumRuneForSkullLevel (_dataBank, targetLevelFive);
			if (levelFiveRune != null) {
				levelFiveRune.isSuggested = true;
				levelFiveRune.uid = Nonce.GetUniqueID ();
				suggestedUIRuneList.Add (levelFiveRune);
			}
			
			
			List<InventorySystem.InventoryItem> suggestedItemsList = new List<InventorySystem.InventoryItem> ();
			
			foreach (InventorySystem.ItemRing aRing in suggestedUIRingList)
				suggestedItemsList.Add (aRing);
			
			foreach (InventorySystem.ItemRune aRune in suggestedUIRuneList)
				suggestedItemsList.Add (aRune);
			
			return suggestedItemsList.ToArray ();
		}
	}
	
	public string suggestedItemsJson {
		get {
			suggestedRingList.Clear ();
			suggestedRuneList.Clear ();
			
			//int indexInInventory = InventorySystem.Inventory.startIndexForSuggested;
			int targetLevelOne;
			int targetLevelTwo;
			int targetLevelThree;
			int targetLevelFour;
			int targetLevelFive;
			
			targetLevelOne = _gameState.skullLevel;
			if (targetLevelOne >= NUMBER_OF_LEVELS)
				targetLevelOne = NUMBER_OF_LEVELS;
			
			targetLevelTwo = _gameState.skullLevel + 3;
			if (targetLevelTwo >= NUMBER_OF_LEVELS)
				targetLevelTwo = NUMBER_OF_LEVELS;
			
			targetLevelThree = _gameState.skullLevel + 5;
			if (targetLevelThree >= NUMBER_OF_LEVELS)
				targetLevelThree = NUMBER_OF_LEVELS;
			
			targetLevelFour = _gameState.skullLevel + 7;
			if (targetLevelFour >= NUMBER_OF_LEVELS)
				targetLevelFour = NUMBER_OF_LEVELS;
			
			targetLevelFive = _gameState.skullLevel + 6;
			if (targetLevelFive >= NUMBER_OF_LEVELS)
				targetLevelFive = NUMBER_OF_LEVELS;
			
			_suggestedItemsJson = ""
				+ "{"
					+ "\"Rings\" : "
					+ "[";
			
			InventorySystem.ItemRing levelOneRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, GetRandomTagFromAvailable (true));
			if (levelOneRing != null) {
				levelOneRing.isSuggested = true;
				suggestedRingList.Add (levelOneRing);
			}
			
			InventorySystem.ItemRing levelTwoRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, "H");
			if (levelTwoRing != null) {
				levelTwoRing.isSuggested = true;
				suggestedRingList.Add (levelTwoRing);
			}
			
			
			InventorySystem.ItemRing levelThreeRing = Databank.GetRandomNonpremiumRingForSkullLevel (_dataBank, targetLevelOne, "D");
			if (levelThreeRing != null) {
				levelThreeRing.isSuggested = true;
				suggestedRingList.Add (levelThreeRing);
			}
			
			InventorySystem.ItemRing levelFourRing = Databank.GetRandomPremiumRingForSkullLevel (_dataBank, targetLevelFive, GetRandomTagFromAvailable (true));
			if (levelFourRing != null) {
				levelFourRing.isSuggested = true;
				suggestedRingList.Add (levelFourRing);
			}
			
			InventorySystem.ItemRing levelFiveRing = Databank.GetRandomPremiumRingForSkullLevel (_dataBank, targetLevelFive, "S");
			if (levelFiveRing != null) {
				levelFiveRing.isSuggested = true;
				suggestedRingList.Add (levelFiveRing);
			}
			
			for (int i = 0; i < suggestedRingList.Count; i++) {
				suggestedRingList [i].uid = Nonce.GetUniqueID ();
				
				_suggestedItemsJson += MiniJSON.Json.Serialize (suggestedRingList [i].ToDictionary ());
				
				if (i != suggestedRingList.Count - 1)
					_suggestedItemsJson += ",";
			}
			
			_suggestedItemsJson += "]";
			_suggestedItemsJson += ",";
			
			_suggestedItemsJson += "\"Runes\" : ";
			_suggestedItemsJson += "[";
			
			InventorySystem.ItemRune levelOneRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "F");
			if (levelOneRune != null) {
				levelOneRune.isSuggested = true;
				suggestedRuneList.Add (levelOneRune);
			}
			
			InventorySystem.ItemRune levelTwoRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "L");
			if (levelTwoRune != null) {
				levelTwoRune.isSuggested = true;
				suggestedRuneList.Add (levelTwoRune);
			}
			
			
			InventorySystem.ItemRune levelThreeRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "W");
			if (levelThreeRune != null) {
				levelThreeRune.isSuggested = true;
				suggestedRuneList.Add (levelThreeRune);
			}
			
			
			InventorySystem.ItemRune levelFourRune = Databank.GetRandomNonpremiumRuneForSkullLevel (_dataBank, targetLevelOne, "E");
			if (levelFourRune != null) {
				levelFourRune.isSuggested = true;
				suggestedRuneList.Add (levelFourRune);
			}
			
			
			InventorySystem.ItemRune levelFiveRune = Databank.GetRandomPremiumRuneForSkullLevel (_dataBank, targetLevelFive);
			if (levelFiveRune != null) {
				levelFiveRune.isSuggested = true;
				suggestedRuneList.Add (levelFiveRune);
			}
			
			
			
			for (int i = 0; i < suggestedRuneList.Count; i++) {
				suggestedRuneList [i].uid = Nonce.GetUniqueID ();
				
				_suggestedItemsJson += MiniJSON.Json.Serialize (suggestedRuneList [i].ToDictionary ());
				
				if (i != suggestedRuneList.Count - 1)
					_suggestedItemsJson += ",";
			}
			
			
			_suggestedItemsJson += "]";
			
			_suggestedItemsJson += "}";
			
			if (PRINT_LOGS)
				Debug.Log (":::::::::::: SUGGESTED ITEMS JSON ::::::::::::::::::::::" + _suggestedItemsJson);
			return _suggestedItemsJson;
		}
	}
	
	#endregion
	
	public void ReturnToCamp ()
	{
		_levelManager.ReturnToCamp ();
	}
	
	public void RestartBattle ()
	{
		_levelManager.RestartBattle ();
	}
	
	public void DebugInit () {
		_levelManager.DebugInit();
	}

	public void StartStoryMode ()
	{
		_levelManager.StartStoryMode ();
	}
	
	public void StartMultiPlayerMode ()
	{
		isMultiPlayerMode = true;
		_levelManager.StartMultiPlayerMode ();
	}
	
	public void LoadMetaDataHandler (object responseParameters, object error, ServerRequest request)
	{
		if (PRINT_LOGS)
			Debug.Log ("LoadMetaDataHandler");
		if (error == null) {
			IDictionary response = responseParameters as IDictionary;
			
			Debug.Log ("SERVER DATABANK > " + MiniJSON.Json.Serialize (response));
			
			bool upToDate = System.Convert.ToBoolean (response ["upToDate"]);
			if (!upToDate) {
				string metaData = MiniJSON.Json.Serialize (response ["metaData"] as IDictionary);
				GameManager.instance._monoHelpers.WriteIntoPersistantDataPath (metaData, GameManager.DATA_BANK_FILE_NAME);
				// load server meta data
				if (PRINT_LOGS)
					Debug.Log ("Loading meta data from network.");
			} else {
				if (PRINT_LOGS)
					Debug.Log ("Same meta data.");
				// Load local meta data
			}	
		} else {
			if (PRINT_LOGS)
				Debug.Log ("Loading meta data from local.");
			// Load local meta data
		}
		LoadDataBank (false);
	}
	
	private void LoadPrefabsDictionary ()
	{
		TextAsset textFile = Resources.Load ("SRPrefabs", typeof(TextAsset)) as TextAsset;
		prefabsDic = MiniJSON.Json.Deserialize (textFile.text) as IDictionary;
	}
	
	public GameObject LoadPrefab (string id)
	{
		return Resources.Load (GameManager.instance.prefabsDic [id].ToString ()) as GameObject;
	}
	
	public string getEnemyFromGameState (string poi_id)
	{
		string enemyName = null;
		List<KeyValuePair<string,string>> tempPoiEnemyKeyVal = GameManager._gameState.LevelState.POIEnemyKeyVal;
		
		for (int i=0; i<tempPoiEnemyKeyVal.Count; i++) {
			if (tempPoiEnemyKeyVal [i].Key.Equals (poi_id)) {
				enemyName = tempPoiEnemyKeyVal [i].Value;
			}
		}
		return enemyName;
	}
	
	public void OnApplicationFocus (bool yesNo)
	{
//		if (!yesNo)
//			Application.Quit ();
	}
	public void OnApplicationPause ()
	{
//		Application.Quit ();
	}
	
	public void CheckPendingTransactions ()
	{
		fragNetworkingNew.InvokeRepeating ("CheckPendingTransactions", 30.0f, 30.0f);
	}
	
	public void EnableGestureEmitter (bool isEnable)
	{
		if (_levelManager != null && _levelManager.battleManager != null && _levelManager.battleManager.gameObject.activeInHierarchy && _levelManager.battleManager._gestureEmitter != null) {
			_levelManager.battleManager._gestureEmitter.enabled = isEnable;
		}
	}
	
	public Dictionary<string, object> PlayerAndEnemyStatsComparision (AIModel aiModel= null)
	{
		Dictionary<string, object> missingRingDictionary = new Dictionary<string, object> ();
		
		string title = "Ring";
		string messageString = "";
		List<string> missingTags = new List<string> ();
		
		if (aiModel == null) {
			string aiName = GameManager.instance.getEnemyFromGameState (GameManager.instance._levelManager.currentLevel + "_DuelA");
			
			if (GameManager._gameState.bossAttemptDictionary.Contains (aiName) && Int32.Parse (GameManager._gameState.bossAttemptDictionary [aiName].ToString ()) == 0)
				GameManager._gameState.bossAttemptDictionary [aiName] = 1;
			
			if (GameManager._gameState.bossAttemptDictionary.Contains (aiName))
				aiModel = GameManager._dataBank.GetModelForMonsterByEncounter (aiName, Int32.Parse (GameManager._gameState.bossAttemptDictionary [aiName].ToString ()));
			else
				aiModel = GameManager._dataBank.GetModelForMonster (aiName, GameManager._gameState.skullLevel);
		}
		
		int availableCounterCount = aiModel.availableSpellCount;
		
		if (aiModel.fire > 0) {
			availableCounterCount--;
			if (!GameManager._gameState.User._hasWater) {
				messageString += "Water Ring";
				missingTags.Add ("W");
				if (availableCounterCount >= 1)
					messageString += ",";
			}
			
		}
		if (aiModel.lightning > 0) {
			availableCounterCount--;
			if (!GameManager._gameState.User._hasEarth) {
				messageString += " Earth Ring";
				missingTags.Add ("E");
				if (availableCounterCount >= 1)
					messageString += ",";
			}
		}
		if (aiModel.water > 0) {
			availableCounterCount--;
			if (!GameManager._gameState.User._hasLightning) {
				messageString += " Lightning Ring";
				missingTags.Add ("L");
				if (availableCounterCount >= 1)
					messageString += ",";
				
			}
		}
		if (aiModel.earth > 0) {
			availableCounterCount--;
			if (!GameManager._gameState.User._hasFire && GameManager._gameState.firstEnemyDefeated) {
				messageString += " Fire Ring";
				missingTags.Add ("F");
				if (availableCounterCount >= 1)
					messageString += ",";
			}
		}
		missingRingDictionary ["title"] = title;
		missingRingDictionary ["messageString"] = messageString;
		if (missingTags.Count == 0)
			missingRingDictionary ["missingTags"] = null;
		else
			missingRingDictionary ["missingTags"] = missingTags;
		
		return missingRingDictionary;
	}
	
	
	public void OnLogout ()
	{
		if (_gameState.User != null) {	
			_gameState.User.username = null;
			_gameState.User.guild = null;
		}
	}
	
	private void DestroyRoot ()
	{
		Destroy (UIManager.instance.transform.parent.gameObject);
	}
	
	private const string FIRST_RUN_PREF = "GameStartedForFirstTime";
	
	private bool _isFirstRun {
		get	{ return PlayerPrefs.GetInt (FIRST_RUN_PREF, 0) < 1;	}
		set	{ if(!value)	PlayerPrefs.SetInt(FIRST_RUN_PREF, 1);	}
	}
	
	private void CheckDeviceRatio() {
		float resRatio = (float)Screen.width/(float)Screen.height;
		if(PRINT_LOGS) Debug.Log("Device Aspect Ratio: " + resRatio + " " + Screen.width + " " + Screen.height);
		if(resRatio < 1.4){
			deviceRatio = "4x3";
		}
		else if(resRatio >=1.4 && resRatio < 1.55){
			deviceRatio = "3x2";
		}
		else if(resRatio >=1.55){
			deviceRatio = "16x9";
		}
	}
	
	public void EnableInput()
	{
		this.PerformActionWithDelay(0.1f, () => {
			InputWrapper._disableTouch = false;
		});
	}
}
