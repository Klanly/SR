using UnityEngine;
using System.Collections;
using System.IO;
public class NGDebugMenu : MonoBehaviour {


	public UIGrid grid;
	public UIButton button;

	[SerializeField]
	public UIButton addSoulButton;
	[SerializeField]
	public UIButton addGemButton;
	
	[SerializeField]
	public UIButton addArcaneButton;
	
	[SerializeField]
	public UIButton subArcaneButton;
	
	[SerializeField]
	public UIButton addDayCountButton;
	
	[SerializeField]
	public UIButton subDayCountButton;

	[SerializeField]
	public UIButton addSkullLevelButton;
	
	[SerializeField]
	public UIButton subSkullLevelButton;
	[SerializeField]
	public UIButton addShardButton;
	
	[SerializeField]
	public UIButton subShardButton;

	[SerializeField]
	public UIButton debugButton;
	[SerializeField]
	public UIButton resetButton;
	[SerializeField]
	public UIButton unlockMultiplayerButton;
	[SerializeField]
	public UIButton unlinkButton;

	[SerializeField]
	public UILabel soulsLabel;
	[SerializeField]
	public UILabel gemsLabel;
	[SerializeField]
	public UILabel arcaneLabel;
	[SerializeField]
	public UILabel dayCountLabel;
	[SerializeField]
	public UILabel skullLevelLabel;
	[SerializeField]
	public UILabel shardCountLabel;
	[SerializeField]
	public UILabel UserInfoLabel;
	[SerializeField]
	public UILabel networkStatus;
	[SerializeField]
	public UILabel internetStatus;

	[SerializeField]
	public UILabel connectTime;
	[SerializeField]
	public UILabel levelTime;


	// Use this for initialization
	void Start () {

		foreach(Level level in GameManager._dataBank.Levels.Values) {
			if(level.levelID.Equals("LavaScene") || level.levelID.Equals("ArcanumRuhalis")) {
				continue;
			}
			UIButton btn = (UIButton)Instantiate(button);
			btn.gameObject.SetActive(true);
			grid.AddChild(btn.transform);
			btn.transform.localScale = Vector3.one;
			btn.transform.GetChild(0).GetComponent<UILabel>().text = level.levelID;
			btn.name = level.levelID;
		}
		grid.Reposition();

		soulsLabel.text = GameManager._gameState.User._inventory.souls.ToString();
		gemsLabel.text = GameManager._gameState.User._inventory.gems.ToString();
		arcaneLabel.text = GameManager._gameState.User.arcanePoints.ToString();
		dayCountLabel.text = GameManager._gameState.dayCount.ToString();
		skullLevelLabel.text = GameManager._gameState.skullLevel.ToString();
		shardCountLabel.text = GameManager._gameState.ringShards.ToString();
		UserInfoLabel.text = "Username : "+GameManager._gameState.User.username + 
			"\nUserID : "+ GameManager.instance.fragNetworkingNew.GetUserID();

//		networkStatus.text = Application.internetReachability.ToString();
//		internetStatus.text = IsConnected().ToString();
		internetStatus.text = ServerManager.IsInternetAvailable().ToString();
//		internetStatus.text = AMQP.Client.IsConnectedToInternet ? "Connected" : "Disconnected";

		connectTime.text = PlayerPrefs.GetFloat("ConnectTime", 0)+" ms";
		levelTime.text = PlayerPrefs.GetFloat("LevelTime", 0)+" ms";

	}

	// Update is called once per frame
	void Update () {
	}

	public void OnClick(UIButton button) {
		string levelID = button.name;
		if(levelID.Contains("BeachCamp")) {
			Debug.LogError("Loading Beachcamp so refreshing shrines");
			ShrineManager.Instance.RefreshShrines();
		}
		GameManager._gameState.LevelState.levelID = levelID;
		GameManager.instance._levelManager.currentLevel = levelID;
		Debug.LogError(levelID);
		PlayerPrefs.SetInt("DEBUGNEXTLEVEL", 1);
		GameManager.instance.DebugInit();
		Destroy(gameObject);
	}


	public void OnAdd(UIButton button) {
		if(button == addSoulButton) {
//			GameManager._gameState.User._inventory.souls += 1000;
//			soulsLabel.text = GameManager._gameState.User._inventory.souls.ToString();
		} else if(button == addGemButton) {
//			GameManager._gameState.User._inventory.gems += 100;
//			gemsLabel.text = GameManager._gameState.User._inventory.gems.ToString();
		} else if(button == addArcaneButton) {
			GameManager._gameState.User.arcanePoints += 100;
			arcaneLabel.text = GameManager._gameState.User.arcanePoints.ToString();
		} else if(button == subArcaneButton) {
			GameManager._gameState.User.arcanePoints -= 100;
			arcaneLabel.text = GameManager._gameState.User.arcanePoints.ToString();
		} else if(button == addDayCountButton) {
			GameManager._gameState.dayCount++;
			PlayerPrefs.SetInt("isDayCountIncremented", 1);
			dayCountLabel.text = GameManager._gameState.dayCount.ToString();
		} else if(button == subDayCountButton) {
			if(GameManager._gameState.dayCount >= 0) {
				GameManager._gameState.dayCount--;
				dayCountLabel.text = GameManager._gameState.dayCount.ToString();
			}
		} else if(button == addSkullLevelButton) {
			GameManager._gameState.skullLevel++;
			skullLevelLabel.text = GameManager._gameState.skullLevel.ToString();
		} else if(button == subSkullLevelButton) {
			if(GameManager._gameState.skullLevel >= 0) {
				GameManager._gameState.skullLevel--;
				skullLevelLabel.text = GameManager._gameState.skullLevel.ToString();
			}
		} else if(button == addShardButton) {
			if(GameManager._gameState.ringShards < 5) {
				GameManager._gameState.ringShards++;
				shardCountLabel.text = GameManager._gameState.ringShards.ToString();
			}
		} else if(button == subShardButton) {
			if(GameManager._gameState.ringShards >= 0) {
				GameManager._gameState.ringShards--;
				shardCountLabel.text = GameManager._gameState.ringShards.ToString();
			}
		} else if(button == unlinkButton) {
			GameManager.instance.fragNetworkingNew.UnlinkAccount(GameCenter.instance.debugPlayerId, OnUnlink, OnUnlink);
		} else if(button == resetButton) {
			PlayerPrefs.DeleteAll();
			GameManager.instance._monoHelpers.CopyDefaultToCurrentGamestate();
			GameManager.instance._monoHelpers.WriteIntoPersistantDataPath("", PurchaseManager.FILE_NAME);
			GameManager.instance._monoHelpers.WriteIntoPersistantDataPath("", PurchaseManager.RECEIPTS_FILE_NAME);
			Debug.LogError("Deleted All Player Prefs");
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Gamestate Reset", "Re-Launch game");
		} else if(button == unlockMultiplayerButton) {
			if(GameManager._gameState.ringShards > 0) {
				Debug.LogError("Multiplayer already unlocked");
				PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Multiplayer already Unlocked", "Return to beach camp to gain access to Multiplayer",  () => {});
				return;
			}
			GameManager._gameState.dayCount++;
			GameManager._gameState.ringShards = 1;
			if(GameManager._gameState.bossAttemptDictionary.Contains("PRIMUS_NEX")) {
				Debug.LogError("Primus_Nex - "+GameManager._gameState.bossAttemptDictionary["PRIMUS_NEX"]);
				if(int.Parse(GameManager._gameState.bossAttemptDictionary["PRIMUS_NEX"].ToString()) < 1) {
					GameManager._gameState.bossAttemptDictionary["PRIMUS_NEX"] = 1;
				}
			}
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Multiplayer Unlocked", "Return to beach camp to gain access to Multiplayer", () => {});
		}
	}

	public void OnAddSoulPress() {
		StartCoroutine("AddSoul");
	}
	public void OnAddSoulRelease() {
		StopCoroutine("AddSoul");
	}

	IEnumerator AddSoul() {
		while(true) {
			GameManager._gameState.User._inventory.souls += 1000;
			soulsLabel.text = GameManager._gameState.User._inventory.souls.ToString();
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void OnAddGemPress() {
		StartCoroutine("AddGem");
	}
	public void OnAddGemRelease() {
		StopCoroutine("AddGem");
	}

	IEnumerator AddGem() {
		while(true) {
			GameManager._gameState.User._inventory.gems += 100;
			gemsLabel.text = GameManager._gameState.User._inventory.gems.ToString();
			yield return new WaitForSeconds(0.1f);
		}
	}


	public void OnUnlink(object responseParameters, object error, ServerRequest request) {
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			bool responseSuccess = bool.Parse(response ["success"].ToString());
			
			if(responseSuccess) {
				PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Unlinked", "");
			}
		}
	}
}
