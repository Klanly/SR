using UnityEngine;
using System.Collections;

public class NGOptionsMenu : MonoBehaviour {

	public UISlider musicSlider;
	public UISlider sfxSlider;
	private float currentMusicValue;
	private float currentSFXValue;

	[SerializeField]
	private UIButton recoveryButton;

	public const string battleTipsPrefabPath = "UIPrefabs/NGUI/BattleTips";
	public const string creditsPrefabPath = "UIPrefabs/NGUI/Credits";


	public const string gamecenterActiveSpritename = "GameCenterIconActive";
	public const string gamecenterInactiveSpritename = "GameCenterIconInactive";
	public const string googleActiveSpritename = "GoogleOptionsIconActive";
	public const string googleInactiveSpritename = "GoogleOptionsIconInactive";
	public UILabel loginInfoLabel;
	public UILabel versionLabel;

	public OptionsUIInterface _optionsUIInterface;

	public void setInterface(OptionsUIInterface optionsInterface)
	{
		_optionsUIInterface = optionsInterface;
	}

	// Use this for initialization
	void Start ()
	{
		currentMusicValue = 0.0f;
		currentSFXValue = 0.0f;


		if(_optionsUIInterface != null)
			_optionsUIInterface.OnRegisterSWFChildCallback(this);

		musicSlider.value = GameManager._gameState.musicVolume;
		sfxSlider.value = GameManager._gameState.gfxVolume;

		UIManager.instance.generalSwf.ToggleTopStats(false);

		string userId = GameManager.instance.fragNetworkingNew.GetUserID();
		string username = GameManager._gameState.User.username;
//		if(Debug.isDebugBuild) {
		loginInfoLabel.text = string.Format("Logged in with user id = {0}, user name = {1}", userId, (string.IsNullOrEmpty(username) || username.Equals(userId) ? "Unknown" : username));
//		} else {
//			loginInfoLabel.text = string.Format("Logged in with user id = {0}, user name = {1}", GameManager.instance.fragNetworkingNew.GetUserID(), GameManager._gameState.User.username,Equals() );
//			loginInfoLabel.gameObject.SetActive(false);
//		}
		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
		string recoverySpritename = "";

		int recovery = PlayerPrefs.GetInt("recovery", 0);
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(recovery == 0) {
			recoverySpritename = googleInactiveSpritename;
		} else {googleInactiveSpritename
			recoverySpritename = googleActiveSpritename;
//			recoveryButton.enabled = false;
		}
		#elif UNITY_IPHONE && !UNITY_EDITOR
		if(recovery == 0) {
			recoverySpritename = gamecenterInactiveSpritename;
		} else {
			recoverySpritename = gamecenterActiveSpritename;
//			recoveryButton.enabled = false;
		}
		#endif
		recoveryButton.normalSprite = recoverySpritename;
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void onMusicValueChanged()
	{
		currentMusicValue = musicSlider.value;
		SoundManager.instance.SetMVolume(currentMusicValue);
		GameManager._gameState.musicVolume = currentMusicValue;
		Debug.Log("value changed to "+currentMusicValue);
	}

	public void onSfxValueChanged()
	{
		currentSFXValue = sfxSlider.value;
		SoundManager.instance.SetSfxVolume(currentSFXValue);
		GameManager._gameState.gfxVolume = currentSFXValue;
		Debug.Log("value changed to "+currentSFXValue);
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
	}

	public void onBattleTipsButton()
	{
		SoundManager.instance.PlayMenuOpenSound();
		UnityEngine.Object asset = Resources.Load(battleTipsPrefabPath);
		NGUITools.AddChild(this.gameObject, asset as GameObject);

		Analytics.logEvent(Analytics.EventName.Options_BattleTutorial);
	}


	public void onSocialButton()
	{
		Application.OpenURL("https://www.facebook.com/4rockngames/");
		SoundManager.instance.PlayMenuOpenSound();
	}

	public void onEquipmentsButton()
	{
		SoundManager.instance.PlayMenuOpenSound();
	}

	public void onSupportButton()
	{
		SoundManager.instance.PlayMenuOpenSound();
		// Application.OpenURL("mailto:hello@frag-games.com?subject=SR%20Feedback&body=Hi%0A%0AMy%20UserId%20is%3A%20%0A"+WWW.EscapeURL(SystemInfo.operatingSystem)+GameManager.instance.fragNetworkingNew.GetUserID()+"%0A%0AEnter%20message%20here");
		Application.OpenURL("mailto:sorcerersring@frag-games.com?subject=SR%20Feedback&body=Hi%0A%0AMy%20UserId%20is%3A%20%0A"+WWW.EscapeURL(SystemInfo.operatingSystem)+GameManager.instance.fragNetworkingNew.GetUserID()+"%0A%0AEnter%20message%20here");
//		Application.OpenURL("mailto:recipient@example.com?subject=hi!&body=Text%20goes%20here!");
	}

	public void onCreditsButton()
	{
		SoundManager.instance.PlayMenuOpenSound();
		UnityEngine.Object asset = Resources.Load(creditsPrefabPath);
		NGUITools.AddChild(this.gameObject, asset as GameObject);

		Analytics.logEvent(Analytics.EventName.Options_Credits);
	}

	public void onCinematicsButton()
	{
		SoundManager.instance.PlayMenuOpenSound();
		Debug.LogError("Cinematics button called");
//		var obj = GameObject.Find ("CinematicCamera");
//		if(obj != null)
//			Destroy(obj);
//		Screen.SetResolution(1280, 720, true);
		var obj = GameObject.Instantiate (Resources.Load ("FullScreen")) as GameObject;
		TestMobileTexture temp = obj.GetComponent<TestMobileTexture>();
		temp.gameObject.SetActive(true);
		temp.scrMedia.Load("Cinematics_vo_9.mp4");
		temp.scrMedia.m_bFullScreen = true;
		temp.scrMedia.OnReady = OnReady;
		temp.scrMedia.OnEnd = OnEnd;
		temp.scrMedia.UpdateWidthAndHeight();
		Debug.LogError(Screen.width+"  "+Screen.height);
//		Debug.LogError("Size - "+TestMobileTexture.instance.movieCamera.orthographicSize);
//		TestMobileTexture.instance.movieCamera.orthographicSize *= 2;

//		TestMobileTexture.instance.scrMedia.Resize();
//		TestMobileTexture.instance.scrMedia.ResizeTexture();

	}

	private void OnReady() {
		TestMobileTexture.instance.gameObject.SetActive(true);
		TestMobileTexture.instance.scrMedia.Play();
//		TestMobileTexture.instance.scrMedia.Resize();
//		TestMobileTexture.instance.scrMedia.ResizeTexture();
		isVideoPlaying = true;
	}

	private void OnEnd() {
		TestMobileTexture.instance.scrMedia.UnLoad();
		TestMobileTexture.instance.gameObject.SetActive(false);
		isVideoPlaying = false;

		Destroy(TestMobileTexture.instance.gameObject);
	}

	float widthDenom = 14f;
	float heightDenom = 10f;
	private bool isVideoPlaying = false;
	public Texture buttonImage;
//	public void OnGUI ()
//	{
//		if(!isVideoPlaying)
//			return;
//
//		float skipButtonWidth = Screen.width / widthDenom;
//		float skipButtonHeight = Screen.height / heightDenom;
//
//		GUIStyle labelStyle = GUI.skin.label;
//
//		if (GUI.Button (new Rect (Screen.width - skipButtonWidth - 1, Screen.height - skipButtonHeight - 1, skipButtonWidth, skipButtonHeight), buttonImage, labelStyle))
//		{
//			Debug.LogError("GUI button Clicked");
//			OnEnd();
//		}
//	}

	public void onRecoveryButton()
	{
		if(PlayerPrefs.GetInt("recovery") == 1)
		{
			string recoveryId = "";
			string recoverySpritename = "";

			PlayerPrefs.SetInt("recovery", 0);
			PlayerPrefs.SetString("RecoveryId", "");
			#if UNITY_ANDROID && !UNITY_EDITOR
			recoverySpritename = googleInactiveSpritename;
			#elif UNITY_IPHONE && !UNITY_EDITOR
			recoverySpritename = gamecenterInactiveSpritename;
			#endif
			recoveryButton.normalSprite = recoverySpritename;
		} else {

		#if  UNITY_EDITOR_OSX || UNITY_EDITOR
		// TODO
		// Call the ExistPlayerID network call directly
		//		GameManager.instance.fragNetworkingNew.FindAccount(debugPlayerId, OnFindAccount, OnFindAccount);
		//		Debug.LogError("Calling FInd account");
		OnPlayerAuthFinished();
		#elif UNITY_IOS
		GameCenter.instance.Init();
		GameCenterManager.OnAuthFinished += OnAuthFinished;
//		GameCenterManager.Init();
		#elif UNITY_ANDROID
		GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
//		GooglePlayConnection.Instance.Connect ();;
		#endif
		}
	}

	private void ActionConnectionResultReceived(GooglePlayConnectionResult result) {
		if(result.IsSuccess) {
			Debug.Log("Connected!");
			if(GooglePlayManager.Instance.player != null) {
				Debug.LogError("id - "+GooglePlayManager.Instance.player.playerId + " name - "+GooglePlayManager.Instance.player.name);
				OnPlayerAuthFinished();
			}
		} else {
			Debug.Log("Cnnection failed with code: " + result.code.ToString());
		}
	}

	void OnAuthFinished (SA.Common.Models.Result res) {
		if (res.IsSucceeded) {
			GameCenterManager.RetrievePlayerSignature();
//			GameCenterManager.OnPlayerSignatureRetrieveResult += OnPlayerSignatureRetrieveResult;
			OnPlayerAuthFinished();
		} else {
			IOSNativePopUpManager.showMessage("Game Center ", "Player auth failed");
		}

	}

	private void OnPlayerAuthFinished() {
		if(GameCenterManager.IsPlayerAuthenticated || Application.isEditor) {
			string playerId = "";
			#if  UNITY_EDITOR_OSX || UNITY_EDITOR
			playerId = GameCenter.instance.debugPlayerId;
			#elif UNITY_ANDROID && !UNITY_EDITOR
			playerId = GooglePlayManager.Instance.player.playerId;
			#elif UNITY_IPHONE && !UNITY_EDITOR
			playerId =  GameCenterManager.Player.Id;
			#endif

			if(PlayerPrefs.GetString("RecoveryId", "").Equals(playerId)) {
				Debug.LogError("already signed in");
			} else if(string.IsNullOrEmpty(PlayerPrefs.GetString("RecoveryId", ""))) {
				Debug.LogError("RecoveryId = null");
				GameManager.instance.fragNetworkingNew.LinkAccount(playerId, OnRecoveryIDHandler, OnRecoveryIDHandler);
			}
		}

	}

	private void OnRecoveryIDHandler(object responseParameters, object error, ServerRequest request) {
		if(error == null)
		{
			Debug.LogError("OnRecoveryIDHandler - - - "+MiniJSON.Json.Serialize(responseParameters));
			IDictionary response = responseParameters as IDictionary;
			bool responseSuccess = bool.Parse(response ["success"].ToString());

			if(responseSuccess) {
				int userId = System.Convert.ToInt32(response ["userId"].ToString());
				string username = response ["userName"].ToString();
				string password = response ["user_password"].ToString();
				Debug.LogError("user - "+userId+" pass - "+password);

				if(userId.ToString().Equals(GameManager.instance.fragNetworkingNew.GetUserID())) {
					Debug.LogError("Same user - "+userId+" so must be linked or not?");
					PlayerPrefs.SetInt("recovery", 1);
					string recoveryId = "";
					string recoverySpritename = "";

					#if UNITY_ANDROID && !UNITY_EDITOR
						recoverySpritename = googleActiveSpritename;
						recoveryId = GooglePlayManager.Instance.player.playerId;
					#elif UNITY_IPHONE && !UNITY_EDITOR
						recoveryId = GameCenterManager.Player.Id;
						recoverySpritename = gamecenterActiveSpritename;
					#elif  UNITY_EDITOR_OSX || UNITY_EDITOR
						recoveryId = GameCenter.instance.debugPlayerId;
						recoverySpritename = gamecenterActiveSpritename;
					#endif
					PlayerPrefs.SetString("RecoveryId", recoveryId);
					recoveryButton.normalSprite = recoverySpritename;
				} else {
					PlayerPrefs.SetInt("recovery", 0);
					PlayerPrefs.SetString("RecoveryId", "");

					UIManager.instance.generalSwf.generalSwf.showGeneralPopup4("Recovery Error", "This account is linked to user ID - "+userId+". Would you like to load it?", "Yes", "No", () => {
						Debug.LogError("Call disconnect and login");
						if(!userId.ToString().Equals(username)) {
							PlayerPrefs.SetString("userName", username);
						} else {
							PlayerPrefs.SetString("userName", userId.ToString());
						}
						PlayerPrefs.SetString("userID", userId.ToString());
						PlayerPrefs.SetString("password", password);


						GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
						AMQP.Client.Disconnect();
						GameManager.instance.fragNetworkingNew._loginResponse += LoginResponseHandler;
						GameManager.instance.fragNetworkingNew.Connect();
						GameManager._gameState.version = 0;
					}, () => {
					});
				}
			}
		}
	}

	private void LoginResponseHandler (object response, object error, ServerRequest request) {
		if (error == null) {
			GameManager.instance.scaleformCamera.generalSwf.HideUILoadingScreen(true);
			GameManager.instance.fragNetworkingNew._loginResponse -= LoginResponseHandler;
			PlayerPrefs.SetInt("recovery", 1);
			PlayerPrefs.SetString("RecoveryId", GameCenterManager.Player.Id);
		}
	}

}
