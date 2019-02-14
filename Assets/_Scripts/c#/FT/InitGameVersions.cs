using UnityEngine;
using System;
using System.Collections;
using FRAG;

public class InitGameVersions : Singleton<InitGameVersions>
{
	[HideInInspector]
	public NetworkingNEW
		_fragNetworkingNew;

	[HideInInspector]
	public MonoHelpers
		_myHelper;

	public BundleDownloadManager _bundleDownloadManager;

	public static int _metadataVersion = int.MinValue;
	public static int _gamestateVersion = int.MinValue;
	public static int _bundleVersion = int.MinValue;
	public static IDictionary _bundleDictionary = null;

	private static long _serverTime = -1;
	public static long offset = 0;
	
	public long ServerTime
	{
		get
		{
			return _serverTime;
		}
		set{
			_serverTime = value;
			StartServerTimer();
		}
	}
	
	private void StartServerTimer() {
		//		Debug.LogWarning("StartServerTimer timer updated by 1.0f");
		
		StopCoroutine("StartServerTimerRoutine");
		StartCoroutine("StartServerTimerRoutine");
	}
	
	public IEnumerator StartServerTimerRoutine ()
	{
		while(true) {
			yield return new WaitForSeconds (1.0f);
//						Debug.LogWarning("server timer updated by 1.0f");
			_serverTime+= 1000;
		}
	}


	public static class LoginResponseParams
	{
		public static object response;
		public static object error;
		public static ServerRequest request;
	}

	public static bool _offlineMode = false;

	public const string LOCAL_BUNDLE_FILE = "SRBundlesConfigLocal.txt";

	private string _debugStr = "DoingNothing";

	protected void Awake ()
	{
		_myHelper = gameObject.AddComponent<MonoHelpers> ();
	}

	protected void Start ()
	{
		StartCoroutine (ShowLoadingScreen ());
	}

	protected IEnumerator ShowLoadingScreen ()
	{
		while (UIManager.instance.generalSwf == null) {
			UIManager.instance.LoadGeneralSwf ();
			yield return new WaitForSeconds (0.1f);
		}
		UIManager.instance.generalSwf.LoadLoadingScreen ();
		UIManager.instance.generalSwf.ShowTopPanel (true);
		_fragNetworkingNew = gameObject.AddComponent<NetworkingNEW> ();
		_fragNetworkingNew.gameObject.AddComponent<WebSocket_Client.Client> ();
		_fragNetworkingNew._loginResponse = LoginResponseHandler;
		_fragNetworkingNew._noConnectivityListener = OnInternetConnectionFailed;
//		AMQP.Client.onUnableToConnect = OnInternetConnectionFailed;
	}

	private void LoginResponseHandler (object response, object error, ServerRequest request)
	{
		LoginResponseParams.response = response;
		LoginResponseParams.error = error;
		LoginResponseParams.request = request;

		_debugStr = "Response Received";
		WriteDebug ();
		StopCoroutine("CheckNetworkInactivity");
		if(response != null)
			Debug.LogError("LoginResponseHandler "+response.ToString());
		if (error == null) {
//			Debug.LogError("GameState version - before");
//			if((response as IDictionary).Contains("gameStateVersion")) {
//				Debug.LogError("GameState version");
//				Debug.LogError("server gamestatet version = "+(response as IDictionary)["gameStateVersion"]);
//				Debug.LogError(int.Parse((response as IDictionary)["gameStateVersion"].ToString()));
//			}
			if((response as IDictionary).Contains("errorCode")) {
				string errorCode = (response as IDictionary)["errorCode"].ToString();
				if(errorCode.Equals("ADMIN000")) {
					if(UIManager.instance.generalSwf != null && UIManager.instance.generalSwf.generalSwf != null){
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Server Offline", "Game is on maintenance, sorry for interruption.", () => {});
					}
				}
				WriteDebug ();
				_offlineMode = true;
				StartCoroutine("CheckNetworkInactivity");
			} else {

				Debug.LogError("LoginResponseHandler ::::::::: " + MiniJSON.Json.Serialize(response as IDictionary));
				_metadataVersion = 1; //*/int.Parse((response as IDictionary)["metaDataVersion"].ToString());
				_gamestateVersion = /*1; //*/int.Parse((response as IDictionary)["gameStateVersion"].ToString());
				_bundleVersion = 1; //*/int.Parse((response as IDictionary)["assetsVersion"].ToString());
				_debugStr = string.Format ("LoginResponse : metaVersion = {0}, gamestateVersion = {1}, bundleVersion = {2}", _metadataVersion, _gamestateVersion, _bundleVersion);
				if((response as IDictionary).Contains("serverTimeStamp")) {
					Debug.LogError("contains server timestatmp "+(response as IDictionary) ["serverTimeStamp"].ToString ());
				}
				InitGameVersions.instance.ServerTime = long.Parse ((response as IDictionary) ["serverTimeStamp"].ToString ());
				WriteDebug ();
			}
		} else {
			_debugStr = "Offline mode!";
			WriteDebug ();
			_offlineMode = true;
			StartCoroutine("CheckNetworkInactivity");

		}
		var diff = DateTime.Now - NetworkingNEW.dt;
		string totalTime = diff.TotalMilliseconds.ToString();
		NetworkingNEW.dt = DateTime.Now;
		Debug.LogError("Total time taken for network call - "+totalTime);
		PlayerPrefs.SetFloat("ConnectTime", (float)diff.TotalMilliseconds);
		DecideBundleDownload ();
	}


	IEnumerator CheckNetworkInactivity() {
		//			yield break;
		Debug.LogWarning("CheckNetworkInactivity");
		while(true) {
			yield return new WaitForSeconds(10.0f);
			Debug.LogWarning("Trying to connect again");
			GameManager.instance.fragNetworkingNew.Connect();
			yield break;
		}
	}


	private void DecideBundleDownload ()
	{
		if (_myHelper.CheckFileExistance (LOCAL_BUNDLE_FILE)) {
			StartCoroutine (_myHelper.GetTextFromWriteablePathFile (LOCAL_BUNDLE_FILE, OnBundlesFileReceived));
			Debug.LogError("File exists in writable path");
		} else {
			StartCoroutine (_myHelper.LoadIntoPersistantDataPathFromLocalPath (LOCAL_BUNDLE_FILE, filename => {
				StartCoroutine (_myHelper.GetTextFromWriteablePathFile (LOCAL_BUNDLE_FILE, OnBundlesFileReceived));
			}));
			Debug.LogError("File exists in persistent path");
		}
	}


	private void OnBundlesFileReceived (string fileText)
	{

		Debug.LogError("OnBundlesFileReceived");

		_bundleDictionary = MiniJSON.Json.Deserialize (fileText) as IDictionary;
		int localVersion = int.Parse (_bundleDictionary ["Version"].ToString ());

		_debugStr = "OnBundlesFileReceived. Local Version = " + localVersion;
		WriteDebug ();

		_bundleDownloadManager.useLocalPath = localVersion >= _metadataVersion;
		_bundleDownloadManager.gameObject.SetActive (true);
		_bundleDownloadManager.isVideoPlaying = false;
		_bundleDownloadManager.InvokeInit (0);
		_bundleDownloadManager.ContinueLoadingGame ();
	}


	private void OnInternetConnectionFailed ()
	{
		Debug.Log ("OnInternetConnectionFailed");
		LoginResponseHandler (null, new object (), null);
	}

	private void WriteDebug ()
	{
		Debug.Log ("[DEBUG] " + _debugStr);
	}
}
