using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

public class BundleDownloadManager : Singleton<BundleDownloadManager>
{
 	
	// server url
	// https://s3-us-west-2.amazonaws.com/staging-streaming-assets/

	public InitGameVersions _initializer;
	public bool isStaging;
	
	private string BundleURL;// = "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/";
	
	private const string BUNDLES_VERSION_FILE = "BundlesVersionLocalCopy.txt";
	private const string BUNDLES_URL_FILE = "BundlesURLLocalCopy.txt";
	private const string SWF_URL_FILE = "SRSwfsNew.txt";
	private string CONFIG_FILE;// = "SRBundlesConfig.txt";
	public string AssetName;
	public string[] LevelsArray;
	public int version = 1;
	public int downloadingLevelNo = 0;
	public bool clearCache = false;
	public bool useLocalPath = false;
	public int totalBundlesCount = 0;
	public MonoHelpers myHelper;
	public UIManager  SFCamera;
	public bool _clearPlayerPrefs;
	
	private enum BundleDownloadProgress
	{
		offline,
		processed,
		online
	}
	private BundleDownloadProgress _bundleDownloadProgress = BundleDownloadProgress.online;
	
	public bool isBundlesCached = false;
	public bool isUpdateRequired = false;
	
	static IDictionary bundlesConfigDictionary = new Dictionary<string,string> ();
	static IDictionary localBundlesConfigDictionary = new Dictionary<string,string> ();
	static IDictionary bundlesURLDictionary = new Dictionary<string,string> ();
	
	private IList<string> bundlesName = new List<string> ();
	// Use this for initialization
	void Start ()
	{
		if (_clearPlayerPrefs)
			PlayerPrefs.DeleteAll ();
		myHelper = _initializer._myHelper;
		
		//StartBundlesDownload();
		//SFCamera.LoadGeneralSwf();
		//PlayerPrefs.DeleteAll();
		DontDestroyOnLoad (this.gameObject);
	}
	
	public void InvokeInit (float delay)
	{
//		#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_IPHONE)
		Invoke ("InvokeLoadingGenerealSwf", delay);
//		#elif UNITY_ANDROID
//		StartCoroutine(myHelper.LoadFromFile(SWF_URL_FILE,true,OnSwfUrlLoaded));
//		#endif
	}
	
	void InvokeLoadingGenerealSwf ()
	{
		OnGeneralSwfLoaded (false);
	}
	
	void OnSwfUrlLoaded (bool isError, string content)
	{
		if (!isError) {
			/*SFCamera.SFSwfsUrl =  MiniJSON.Json.Deserialize(content) as IDictionary;

			foreach(string SFSwfsUrls in SFCamera.SFSwfsUrl.Keys)
			{
				IDictionary aSwfData = SFCamera.SFSwfsUrl[SFSwfsUrls] as IDictionary;
				if(GameManager.PRINT_LOGS) Debug.Log("SCALEFORM ==== SwfPath--"+ aSwfData["url"] +" SwfSize--"+aSwfData["size"]);
				StartCoroutine(myHelper.CopySwfFile(aSwfData["url"].ToString()));
			}*/
		}
	}

	public void OnGeneralSwfLoaded (bool isLoaded)
	{
//		SFCamera.generalSwf.LoadLoadingScreen();
		//isGeneralSwfLoaded = true;
//		if(PlayerPrefs.HasKey("SRingMovieSkipAllowed") && !isVideoPlaying)
//		{
		if (isLoaded) {
			SFCamera.generalSwf.LoadLoadingScreen ();
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~SHOWING LOADING SCREEN IN BUNDLE DOWNLOAD MANAGER~~~~~~~~~~~~~~~~~~~~~");
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~ALTERNATE FLOW FOR BUNDLE DOWNLOAD MANAGER~~~~~~~~~~~~~~~~~~~~~");
		}
		//scaleformCamera.generalSwf.loginToFacebook();
		//scaleformCamera.generalSwf.showLoginPopup();
		if (!useLocalPath) {
			if (isStaging) {
				CONFIG_FILE = "SRBundlesConfigStaging.txt";
				BundleURL = "https://s3.amazonaws.com/sorcerers-ring/staging/streaming-assets/";
			} else {
				CONFIG_FILE = "SRBundlesConfigProduction.txt";
				BundleURL = "https://s3.amazonaws.com/sorcerers-ring/production/streaming-assets/";
			}
			if (clearCache) {
				Caching.CleanCache ();
			}
		
			myHelper = InitGameVersions.instance._myHelper;
			if (SFCamera.generalSwf == null)
				SFCamera.LoadGeneralSwf ();
			
			if (PlayerPrefs.HasKey ("SRingMovieSkipAllowed") && !isVideoPlaying) {
				StartCoroutine (GetConfigFile (BundleURL + CONFIG_FILE));
				Invoke ("showLoadingSWF", 4);
				Invoke ("ToggleBar", 2);
			} else {
				StartCoroutine (GetConfigFile (BundleURL + CONFIG_FILE));
				Invoke ("ToggleBar", 2);
			}
			
			/*
			if(SFCamera.generalSwf == null)
				SFCamera.LoadGeneralSwf();
			
			if(!isVideoPlaying){
				StartCoroutine(GetConfigFile(BundleURL+CONFIG_FILE));
				Invoke("showLoadingSWF",4);
				Invoke("ToggleBar",2);
			}
			else{
				StartCoroutine(GetConfigFile(BundleURL+CONFIG_FILE));
				Invoke("ToggleBar",2);
			}*/
		} else {
			if (SFCamera.generalSwf == null)
				SFCamera.LoadGeneralSwf ();
			if (clearCache) {
				Caching.CleanCache ();
			}
			//isUpdateRequired = false;
			//isBundlesCached = true;
			Invoke ("ContinueLoadingGame", 2.0f);
		}
		//	}
	}
	public void DelayedMovieStart ()
	{
		UIManager.instance.LoadGeneralSwf ();
		if (GameManager.PRINT_LOGS)
			Debug.LogError ("DelayedMovieStart()");
		//StartCoroutine(GetConfigFile(BundleURL+CONFIG_FILE));
		if (SFCamera.generalSwf != null) {
			SFCamera.generalSwf.LoadLoadingScreen ();
			SFCamera.generalSwf.LoadingScreenShowBar ("Checking Content...");
			SoundManager.instance.playDownloadDefaultBackGround ();
		} else {
			OnGeneralSwfLoaded (false);
		}
	}
	
	private void ToggleBar ()
	{
//		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
		SFCamera.generalSwf.ToggleTopStats (false);
		SFCamera.generalSwf.DisplayCenterButton (false);
	}
	
	private void showLoadingSWF ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("showLoadingSWF()");
		if (SFCamera.generalSwf != null) {
			SFCamera.generalSwf.LoadLoadingScreen ();
			SFCamera.generalSwf.LoadingScreenShowBar ("Checking Content...");
			SoundManager.instance.playDownloadDefaultBackGround ();
		}
	}
	
	IEnumerator GetConfigFile (string configUrl)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("configUrl >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>    " + configUrl);
		yield return new WaitForSeconds (0.1f);
		if (!isVideoPlaying) {
			SFCamera.generalSwf.LoadLoadingScreen ();
		}
		
		UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
		SFCamera.generalSwf.ToggleTopStats (false);
		downloadingLevelNo = 0;
		/*
		bundlesConfigDictionary.Clear();
		bundlesURLDictionary.Clear();
		bundlesName.Clear();
		*/
		LoadVerionsDic ();
		LoadURLDic ();
		
		WWW configWWW = new WWW (configUrl);
		yield return configWWW;
		
		if (configWWW.error != null && _bundleDownloadProgress == BundleDownloadProgress.online) {
			
			
			
			//LoadVerionsDic();
			//LoadURLDic();
			yield return new WaitForSeconds (0.1f);
			CheckLocalBundles ();
			if (!isBundlesCached) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("No Internet Connectivity: Bundles Remaining");
				//SFCamera.generalSwf.ShowNoConnectivityPopup("No Internet Connectivity: Bundles Remaining");
				ContinueLoadingGame ();
			} else {
				if (isUpdateRequired) {
					Debug.Log ("No Internet Connectivity: Update Required");
					SFCamera.generalSwf.ShowNoConnectivityPopup ("No Internet Connectivity: Update Required");
				} else {
					Debug.Log ("NO UPDATE REQUIRED");
					_bundleDownloadProgress = BundleDownloadProgress.processed;
					yield return new WaitForSeconds (0.1f);
					ContinueLoadingGame ();
				}
			}
			//Invoke("Start",5.0f);
			Debug.Log ("couldn't download! :(");
			if (_bundleDownloadProgress == BundleDownloadProgress.online) {
				_bundleDownloadProgress = BundleDownloadProgress.offline;
				StartCoroutine (GetConfigFile (""));
			}
//				throw new Exception("WWW config download had an error:" + configWWW.error);
			
		} else {
			//if(GameManager.PRINT_LOGS) Debug.Log("Config file :" + configWWW.text);
			IDictionary tempDic;
			
			if (_bundleDownloadProgress == BundleDownloadProgress.offline) {
				_bundleDownloadProgress = BundleDownloadProgress.processed;
				tempDic = InitGameVersions._bundleDictionary;
			} else
				tempDic = Json.Deserialize (configWWW.text) as IDictionary;	
			
			IList tempList;
			#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
			tempList = tempDic ["Iphone"] as IList; 
			#elif UNITY_ANDROID
						tempList = tempDic["Android"] as IList;
			#elif UNITY_IPHONE
						tempList = tempDic["Iphone"] as IList;
			#endif
			//if(GameManager.PRINT_LOGS) Debug.Log(tempList[0]);
			IDictionary key = null;
			int count = tempList.Count;
			
			
				
			if (bundlesConfigDictionary != null) {
				bundlesConfigDictionary.Clear ();
			}
			if (bundlesURLDictionary != null) {
				bundlesURLDictionary.Clear ();
			}
			if (bundlesName != null) {
				bundlesName.Clear ();
			}
			for (int i = 0; i < count; i++) {
				key = tempList [i] as IDictionary;
				//foreach(IDictionary key in tempList){
				foreach (string levelKey in key.Keys) {
					IDictionary verionDic = key [levelKey] as IDictionary;
					if (GameManager.PRINT_LOGS)
						Debug.Log ("KEY ---- " + levelKey + " Version: " + verionDic ["version"].ToString () + " url: " + verionDic ["url"].ToString ());
					bundlesConfigDictionary.Add (levelKey, verionDic ["version"].ToString ());
					bundlesURLDictionary.Add (levelKey, verionDic ["url"]);
					bundlesName.Add (levelKey);
					
				}
			}
			foreach (string levelKey in bundlesConfigDictionary.Keys) {
//				if(GameManager.PRINT_LOGS) Debug.Log("KEY ---- " + levelKey + " Version: " + bundlesConfigDictionary[levelKey]);
			}
			CheckLocalBundles ();
			CheckUpdateRequired ();
			if (isUpdateRequired || !isBundlesCached) {
				SaveVerionsDic ();
				SaveURLDic ();
				
				totalBundlesCount = bundlesName.Count;
				//	if(GameManager.PRINT_LOGS) Debug.Log("BUNDEL TESTTTT______----- " + bundlesName[0]);
				yield return new WaitForSeconds (0.1f);
				StartBundlesDownload ();
			} else {
				SaveVerionsDic ();
				SaveURLDic ();
				yield return new WaitForSeconds (0.1f);
				ContinueLoadingGame ();
			}
		}
		
	}
	
	public void CheckLocalBundles ()
	{
		foreach (string levelKey in localBundlesConfigDictionary.Keys) {
			if (Caching.IsVersionCached (GetFormatedURL (levelKey), GetVersionForLevelName (levelKey))) {
				isBundlesCached = true;
//				if(GameManager.PRINT_LOGS) Debug.Log("BUNDLES CACHED");
			} else {
//				if(GameManager.PRINT_LOGS) Debug.Log("BUNDLES NOT CACHED");
				isBundlesCached = false;
				return;
			}
		}
	}
	
	public void CheckUpdateRequired ()
	{
		int versionNo;
		int localVersionNo;
		foreach (string levelKey in localBundlesConfigDictionary.Keys) {
			versionNo = System.Convert.ToInt32 (bundlesConfigDictionary [levelKey]);
			localVersionNo = System.Convert.ToInt32 (localBundlesConfigDictionary [levelKey]);
			if (versionNo > localVersionNo) {
				isUpdateRequired = true;
				if (GameManager.PRINT_LOGS)
					Debug.Log ("UPDATE REQUIRED");
				return;
			} else {
//				if(GameManager.PRINT_LOGS) Debug.Log("UPDATE NOT REQUIRED");
				isUpdateRequired = false;
			}
		}
	}
	
	
	public void StartBundlesDownload ()
	{
		
		if (!Caching.IsVersionCached (GetFormatedURL (bundlesName [downloadingLevelNo]), GetVersionForLevelName (bundlesName [downloadingLevelNo]))) {
			StartCoroutine (DownloadAndCache (GetFormatedURL (bundlesName [downloadingLevelNo]), GetVersionForLevelName (bundlesName [downloadingLevelNo])));
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Starting Download for : " + GetFormatedURL (bundlesName [downloadingLevelNo]));
		} else {
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Bundle Already Cached : " + GetFormatedURL (bundlesName [downloadingLevelNo]));
			if (downloadingLevelNo < totalBundlesCount - 1) {
				downloadingLevelNo++;
				StartBundlesDownload ();
			} else {
				isBundlesCached = true;
				isUpdateRequired = false;
				ContinueLoadingGame ();
			}
		}
	}
	
	private string FormatURL (string bundleName)
	{
		string bundleURL;
		#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
		//bundleURL =  bundleName+"_bundle_iphone.unity3d";
		bundleURL = bundleName + ".unity3d";
		#elif UNITY_ANDROID
			bundleURL =  bundleName+".unity3d";
		#elif UNITY_IPHONE
			bundleURL =  bundleName+".unity3d";
		#endif
		
		return bundleURL;
	}
	
	public string GetFormatedURL (string bundleName, bool loadFromLocal = false)
	{
		//yahan public expose kar kuch local ka bool, aur is bundleurl ko apnalocal path day day ju bi dena 
		
		Console.WriteLine ("bundleName = " + bundleName + "    bundlesURLDictionary[bundleName] =" + bundlesURLDictionary [bundleName]);
		string bundleURL;
		if (!useLocalPath && !loadFromLocal) {
			if (bundlesURLDictionary [bundleName].Equals ("LOCAL")) {
				return GetFormatedURL (bundleName, true);
			} else
				bundleURL = bundlesURLDictionary [bundleName] + bundleName + ".unity3d";
		} else {
			
			
			#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
			bundleURL = "file://" + Application.streamingAssetsPath + "/Iphone/" + bundleName + ".unity3d"; 
			#elif UNITY_ANDROID
			bundleURL = Application.streamingAssetsPath+"/Android/"+ bundleName + ".unity3d"; 
			#elif UNITY_IPHONE
			bundleURL = "file://"+Application.streamingAssetsPath+"/Iphone/"+ bundleName + ".unity3d"; 
			#endif

		}
		if (GameManager.PRINT_LOGS)
			Debug.Log ("bundleURL ==" + bundleURL);
		return bundleURL;
	}
	
	WWW www;
	IEnumerator DownloadAndCache (string bundleUrl, int bundleVersion)
	{
		//ClearAllLocalizationFiles();
		
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;
		SFCamera.generalSwf.LoadingScreenShowBar ("Loading files " + (downloadingLevelNo + 1) + " of " + totalBundlesCount);
		// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		using (www = WWW.LoadFromCacheOrDownload (bundleUrl, bundleVersion)) {
			//while(!www.isDone){
			//if(GameManager.PRINT_LOGS) Debug.Log("Download Progress: " +www.progress*100+"%");
			InvokeRepeating ("DisplayProgress", 0, 0.5f);
			yield return www;
			//}
			if (www.error != null) {
				CancelInvoke ("DisplayProgress");
				StartCoroutine (GetConfigFile (BundleURL + CONFIG_FILE));
				throw new Exception ("WWW download had an error:" + www.error);
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log (bundleUrl + " Cached");
				//LoadLanguageIfApplicable(www.assetBundle);
				//www.Dispose();
				CancelInvoke ("DisplayProgress");
				if (downloadingLevelNo < totalBundlesCount - 1) {
					downloadingLevelNo++;
					StartBundlesDownload ();
					SFCamera.generalSwf.LoadingScreenHideBar ();
					SFCamera.generalSwf.LoadingScreenShowBar ("Loading files " + (downloadingLevelNo + 1) + " of " + totalBundlesCount);
				} else {
					isBundlesCached = true;
					isUpdateRequired = false;
					ContinueLoadingGame ();
				}
			}
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
		
		
		
	}
	
	private void LoadLanguageIfApplicable (AssetBundle bundle)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("keyname = bundle name =" + bundle.mainAsset.name);
		if (bundle.mainAsset.name.StartsWith ("en_")) {
			TextAsset textAsset = bundle.mainAsset as TextAsset;
			string languageText = textAsset.text;
			if (GameManager.PRINT_LOGS)
				Debug.Log ("languageText FROM BUNDLE = " + languageText);
			
			myHelper.WriteIntoPersistantDataPath (languageText, bundle.mainAsset.name + "_new.txt");
		}
	}

	
	public void PauseDownloader ()
	{
		CancelInvoke ("Start");
		CancelInvoke ("DisplayProgress");
		if (www != null) {
			www.Dispose ();
		}
	}
	
	public void ResumeDownloader ()
	{
		//Invoke("Start",1.0f);
		if (downloadingLevelNo > 0) {
			//	downloadingLevelNo--;
		}
		//StartBundlesDownload();
	}
	private int DProgress;
	void DisplayProgress ()
	{
		if (www != null) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Download Progress: " + www.progress * 100 + "%");
			if (SFCamera.generalSwf != null) {
				DProgress = (int)(www.progress * 100);
				SFCamera.generalSwf.LoadingPercentage (DProgress);
			}
		} else {
			CancelInvoke ("DisplayProgress");
		}
	}
	
	//GameManager.instance.scaleformCamera.generalSwf.LoadingScreenHideBar();
	//GameManager.instance.scaleformCamera.generalSwf.LoadingScreenShowBar(levelName);
	//GameManager.instance.scaleformCamera.generalSwf.UnLoadLoadingScreen();
	//GameManager.instance.scaleformCamera.generalSwf.LoadLoadingScreen();
	public bool isVideoPlaying = false;
	public void ShowDownloadingProgress ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("ShowDownloadingProgress()");
		if (downloadingLevelNo < totalBundlesCount - 1) {
			if (SFCamera.generalSwf != null) {
				SFCamera.generalSwf.LoadLoadingScreen ();
				//SFCamera.generalSwf.LoadingScreenShowBar("Downloading " +(downloadingLevelNo+1)+" of " + totalBundlesCount);
				SFCamera.generalSwf.LoadingScreenShowBar ("Checking Content...");
			} else {
				SFCamera.LoadGeneralSwf ();
			}
		} else {
			SFCamera.generalSwf.LoadingScreenHideBar ();
			if (Application.loadedLevel == 0 && !isVideoPlaying)
				LoadMainMenu ();
		}
	}
	
	public void ContinueLoadingGame ()
	{
		if (!useLocalPath) {
			if (GameManager.PRINT_LOGS)
				Debug.LogError ("ContinueLoadingGame - 1");
			if (SFCamera.generalSwf != null) {
				if (GameManager.PRINT_LOGS)
					Debug.LogError ("ContinueLoadingGame - 2");
				if (Application.loadedLevel == 0 && !isVideoPlaying && !isUpdateRequired && isBundlesCached) {
					if (GameManager.PRINT_LOGS)
						Debug.LogError ("ContinueLoadingGame - 3");
					SFCamera.generalSwf.LoadLoadingScreen ();
					SFCamera.generalSwf.LoadingScreenHideBar ();
					LoadMainMenu ();
				}
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.LogError ("ContinueLoadingGame - 4");
				SFCamera.LoadGeneralSwf ();
			}
		} else {
			if (SFCamera.generalSwf != null) {
				if (Application.loadedLevel == 0) {
					SFCamera.generalSwf.LoadLoadingScreen ();
					SFCamera.generalSwf.LoadingScreenHideBar ();
					LoadMainMenu ();	
				}
			}
		}
	}
	
	
	public void LoadMainMenu ()
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("LoadMainMenu");
		if (!useLocalPath) {
			if (Application.loadedLevel == 0 && !isVideoPlaying && !isUpdateRequired && isBundlesCached)
				Application.LoadLevel ("MainMenuScene");
		} else {
			Application.LoadLevel ("MainMenuScene");
		}
		
	}
	private void LoadVerionsDic ()
	{
		//transactionList = new List<Dictionary<string, string>>();
		//MonoHelpers monoHelper = GameManager.instance._monoHelpers;
		myHelper.StartCoroutine (myHelper.LoadFromFile (BUNDLES_VERSION_FILE, false, OnFileContentReceived));
	}
	
	private void SaveVerionsDic ()
	{
		string verionsdic = Json.Serialize (bundlesConfigDictionary);
//		if(GameManager.PRINT_LOGS) Debug.Log("STRiiingggggGGGG___" + verionsdic);
		myHelper.WriteIntoPersistantDataPath (verionsdic, BUNDLES_VERSION_FILE);
	}
	
	
	private void OnFileContentReceived (bool isError, string fileContent)
	{
		if (fileContent != null && !fileContent.Equals ("")) {
			//bundlesConfigDictionary = Json.Deserialize(fileContent) as IDictionary;
			if (localBundlesConfigDictionary != null) {
				localBundlesConfigDictionary.Clear ();
			}
			localBundlesConfigDictionary = Json.Deserialize (fileContent) as IDictionary;
			foreach (string bundles in localBundlesConfigDictionary.Keys) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("key--" + bundles + " version no--" + localBundlesConfigDictionary [bundles]);
			}
		}
	}
	
	public int GetVersionForLevelName (string levelName)
	{
		int versionNo;
		if (bundlesConfigDictionary != null && bundlesConfigDictionary.Contains (levelName)) {
			#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
			versionNo = System.Convert.ToInt32 (bundlesConfigDictionary [levelName]);
//			if(GameManager.PRINT_LOGS) Debug.Log("Get Verion---" + bundlesConfigDictionary[levelName] + "Verion--" + versionNo + " Level Name--" + levelName);
			#elif UNITY_ANDROID
			versionNo =  System.Convert.ToInt32(bundlesConfigDictionary[levelName]);
			#elif UNITY_IPHONE
			versionNo =  System.Convert.ToInt32(bundlesConfigDictionary[levelName]);
			#endif
		} else if (localBundlesConfigDictionary != null && localBundlesConfigDictionary.Contains (levelName)) {
			#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
			versionNo = System.Convert.ToInt32 (localBundlesConfigDictionary [levelName]);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Get Verion---" + localBundlesConfigDictionary [levelName] + "Verion--" + versionNo + " Level Name--" + levelName);
			#elif UNITY_ANDROID
			versionNo =  System.Convert.ToInt32(localBundlesConfigDictionary[levelName]);
			#elif UNITY_IPHONE
			versionNo =  System.Convert.ToInt32(localBundlesConfigDictionary[levelName]);
			#endif
		} else {
			versionNo = 1;
		}
		return versionNo;
	}
	
	private void LoadURLDic ()
	{
		//transactionList = new List<Dictionary<string, string>>();
		//MonoHelpers monoHelper = GameManager.instance._monoHelpers;
		myHelper.StartCoroutine (myHelper.LoadFromFile (BUNDLES_URL_FILE, false, OnURLFileContentReceived));
	}
	
	private void SaveURLDic ()
	{
		string verionsdic = Json.Serialize (bundlesURLDictionary);		
		myHelper.WriteIntoPersistantDataPath (verionsdic, BUNDLES_URL_FILE);
	}
	
	
	private void OnURLFileContentReceived (bool isError, string fileContent)
	{
		if (fileContent != null && !fileContent.Equals ("")) {
			bundlesURLDictionary = Json.Deserialize (fileContent) as IDictionary;
//			foreach(string bundles in bundlesURLDictionary.Keys)
//			{
//				if(GameManager.PRINT_LOGS) Debug.Log("key--"+ bundles +" url--"+bundlesURLDictionary[bundles]);
//			}
		}
	}
	
	public void ClearAllLocalizationFiles ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("--- ClearAllLocalizationFiles ---");
		List<string> languageFiles = new List<string> ();
		languageFiles.Add ("en_en_new.txt");
		languageFiles.Add ("en_es_new.txt");
		languageFiles.Add ("en_fr_new.txt");
		languageFiles.Add ("en_de_new.txt");
		languageFiles.Add ("en_it_new.txt");
		
		foreach (string fileName in languageFiles) {
			if (myHelper.CheckFileExistance (fileName))
				myHelper.DeleteFile (fileName);
		}
	}
}
