using UnityEngine;
using System.Collections;

public class NGRaidsUI : MonoBehaviour {

	public NGRaidBossDetailInfo _bossDetailsInfo;
	public NGRaidBossDetailInfo _bossLockedDetailsInfo;

//	public dfButton _leaderboardButton;
//	public dfButton _beachButton ;                        make references to 



	public UI_Raid.LeaderBoardListener _leaderboardInterface;
	#region Character selection

	public NGCreateAvatarPopup _characterSelectPrefab;


	public RaidPortalNavigator _portalNavigator;

	[HideInInspector]
	public NGCreateAvatarPopup _characterSelect;
	public UIButton _leaderboardButton;


	private NGLeaderboardUI leaderboardUI;

	public void ShowAvatarWindow(UI_Raid.CharacterSelectionHandler listenerInterface)
	{
		if (_characterSelect == null)
		{
			_characterSelect =  NGUITools.AddChild(this.gameObject, _characterSelectPrefab.gameObject).GetComponent<NGCreateAvatarPopup>();
			_characterSelect.SetListenerInterface(listenerInterface);
			_leaderboardButton.gameObject.SetActive(false);
		}
	}

	public void Start()
	{
		_bossDetailsInfo._portalNavigator = _portalNavigator;
		_bossLockedDetailsInfo._portalNavigator = _portalNavigator;
	}

	public void HideAvatarWindow()
	{
		if (_characterSelect != null)
		{
			Destroy(_characterSelect.gameObject);
			_characterSelect = null;
			_leaderboardButton.gameObject.SetActive(true);

		}
	}

	public void ShowInvalidUsernameError(string message = "Invalid Username")
	{
		if (_characterSelect != null)
		{
			_characterSelect.ShowInvalidUsername(message);
		}
		else
			Debug.Log("Character selector null!");
	}

	#endregion

	#region Boss details panel
	public void ShowBossDetails(bool yesNo)
	{
		_bossDetailsInfo.gameObject.SetActive(yesNo);
	}

	public void ShowMonsterUI(IDictionary raidInfo)
	{

//		Debug.Log("[RaidsUI] ShowMonsterUI > " + MiniJSON.Json.Serialize(raidInfo));
		_bossDetailsInfo.ShowMonsterUI(raidInfo);
	}

	public void ShowRaidInfo(bool yesNo)
	{
		//_bossDetailsInfo.GetComponent<dfPanel>().IsVisible = yesNo;
	}

	#endregion


	public void OnLeaderboardClicked()
	{
		Debug.Log("OnLeaderboardClicked");

		_leaderboardInterface.OnLeaderBoardClicked();

		if(leaderboardUI == null) {
			GameObject asset = Resources.Load("UIPrefabs/NGUI/LeaderboardUI") as GameObject;
			leaderboardUI =  NGUITools.AddChild(gameObject, asset).GetComponent<NGLeaderboardUI>();
			leaderboardUI.SetRaidPortalNavigator(_portalNavigator);
//			if(UIManager.instance.guildUI != null) {
//				NGUITools.BringForward(UIManager.instance.guildUI.gameObject);
//			}
//			Debug.LogError("OnLeaderboardClicked called and added");
		} else {
			if(!leaderboardUI.gameObject.activeSelf) {
				leaderboardUI.gameObject.SetActive(true);
			}
		}
	}

	public void multiplayerBackButton()
	{
		Debug.Log("OnMultiplayerClicked");
		GameManager.instance._monoHelpers.MainLightOn();

		GameObject.Find("ArcanumRuhalis(Clone)").GetComponent<RaidPortalNavigator>().DestroyCharacterObjects();

		GameObject.Destroy(GameManager.instance._levelManager.player.gameObject);
		GameManager.instance._levelManager.player = null;

		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);

		UIManager.instance.generalSwf.ToggleTopStats (false);
		UIManager.instance.generalSwf.DisplayCenterButton (false);
		var obj = GameObject.Find("BuyKeysPotionsPopup(Clone)");
		if(obj != null) {
			Destroy(obj);
		}
		obj = GameObject.Find("PotionPopup(Clone)");
		if(obj != null) {
			Destroy(obj);
		}

		#region comment out this code if you were not able to fix :P
		GameManager.instance.isMultiPlayerMode = false;
		GameManager.instance.currentMultiPlayerLevel = "";
		GameObject levelManagerGameObject = GameManager.instance._levelManager.gameObject;
		LevelManager singlePlayerLevelManager = (LevelManager)levelManagerGameObject.GetComponent<LevelManager>();
		/*GameManager.instance._levelManager.player.parent=null;
		singlePlayerLevelManager.player=GameManager.instance._levelManager.player;
		//singlePlayerLevelManager.daySkybox=GameManager.instance._levelManager.daySkybox;
		GameManager.instance._levelManager.player.localScale=new Vector3(1,1,1);
		*/
		GameManager.instance._levelManager.enabled = false;
		GameManager.instance._levelManager = singlePlayerLevelManager;
		GameManager.instance._levelManager.enabled = true;

		Destroy(gameObject);
		#endregion

	}
}
