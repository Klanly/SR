using UnityEngine;
using System.Collections;
using SA.Common.Models;

public class GameCenter : Singleton<GameCenter> {

	public string debugPlayerId = "1111";

	void Awake() {

	}

	// Use this for initialization
	void Start () {
//		UnityEngine.ios
//		GameManager.instance.fragNetworkingNew.SetDeviceIdentifier("testId", 2,OnDeciceIdentifierSet, OnDeciceIdentifierSet);
//

//		ISN_RemoteNotificationsController.Instance.RegisterForRemoteNotifications ((ISN_RemoteNotificationsRegistrationResult result) => {
//			if(result.IsSucceeded) {
//				Debug.Log ("DeviceId: " + result.Token.DeviceId);
//				GameManager.instance.fragNetworkingNew.SetDeviceIdentifier(result.Token.DeviceId, 2, OnDeciceIdentifierSet, OnDeciceIdentifierSet);
//			} else {
//				Debug.Log ("Error: " + result.Error.Code + " / " + result.Error.Message);
//			}
//		});
	}
	
	public void OnDeciceIdentifierSet(object responseParameters, object error, ServerRequest request) {
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			bool responseSuccess = bool.Parse(response ["success"].ToString());
			
			if(responseSuccess) {
				Debug.LogError("Success true");
			}
		}
	}


	// Update is called once per frame
	void Update () {
	
	}

	public void Init() {

#if  UNITY_EDITOR_OSX || UNITY_EDITOR
		// TODO
		// Call the ExistPlayerID network call directly
//		GameManager.instance.fragNetworkingNew.FindAccount(debugPlayerId, OnFindAccount, OnFindAccount);
//		Debug.LogError("Calling FInd account");
#elif UNITY_IOS
		GameCenterManager.OnAuthFinished += OnAuthFinished;
		GameCenterManager.Init();
#elif UNITY_ANDROID
		GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
		GooglePlayConnection.Instance.Connect ();;
#endif
	}

	void OnAuthFinished (Result res) {
		if (res.IsSucceeded) {
//			IOSNativePopUpManager.showMessage("Player Authored ", "ID: " + GameCenterManager.Player.Id + "\n" + "Alias: " + GameCenterManager.Player.Alias);
			GameCenterManager.RetrievePlayerSignature();
			GameCenterManager.OnPlayerSignatureRetrieveResult += OnPlayerSignatureRetrieveResult;

			// TODO

			// Call server to check if playerId exists
			// "Yes" - ask to load previous
			// load previous gamestate with retrieved credentials
			// "No" - check with stored playerId: if empty ignore; if different reset gamestate
		} else {
//			IOSNativePopUpManager.showMessage("Game Center ", "Player auth failed");
		}

	}


	void OnPlayerSignatureRetrieveResult (GK_PlayerSignatureResult result) {
		Debug.Log("OnPlayerSignatureRetrieveResult");
		
		if(result.IsSucceeded) {
			
			Debug.LogError("PublicKeyUrl: " + result.PublicKeyUrl);
			Debug.LogError("Signature: " + result.Signature);
			Debug.LogError("Salt: " + result.Salt);
			Debug.LogError("Timestamp: " + result.Timestamp);
			
		} else {
			Debug.LogError("Error code: " + result.Error.Code);
			Debug.LogError("Error description: " + result.Error.Message);
		}
		
		GameCenterManager.OnPlayerSignatureRetrieveResult -= OnPlayerSignatureRetrieveResult;
	}

	private void ActionConnectionResultReceived(GooglePlayConnectionResult result) {
		if(result.IsSuccess) {
			Debug.Log("Connected!");
			if(GooglePlayManager.Instance.player != null) {
				Debug.LogError("id - "+GooglePlayManager.Instance.player.playerId + " name - "+GooglePlayManager.Instance.player.name);
			}
		} else {
			Debug.Log("Cnnection failed with code: " + result.code.ToString());
		}
	}

//	private void OnFindAccount(object responseParameters, object error, ServerRequest request) {
//		if(error == null)
//		{
//			Debug.LogError("OnFindAccount - - - "+MiniJSON.Json.Serialize(responseParameters));
//			IDictionary response = responseParameters as IDictionary;
//			bool responseSuccess = bool.Parse(response ["success"].ToString());
//			
//			if(responseSuccess) {
//				int userId = System.Convert.ToInt32(response ["userId"].ToString());
//				string username = response ["userName"].ToString();
//				string password = response ["user_password"].ToString();
//				bool found = bool.Parse(response ["found"].ToString());
//
//				Debug.LogError("user - "+userId+" pass - "+password+" found - "+found);
//			}
//		}
//	}
}
