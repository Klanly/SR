using UnityEngine;
using System.Collections;

public class RaidsUI : MonoBehaviour 
{
	/*
	public RaidBossDetailInfo _bossDetailsInfo;
	
	public dfButton _leaderboardButton;
	public dfButton _beachButton;
	

	
	public UI_Raid.LeaderBoardListener _leaderboardInterface;
	#region Character selection
	
	public CreateAvatarPopup _characterSelectPrefab;
	
	[HideInInspector]
	public RaidPortalNavigator _portalNavigator;
	
	[HideInInspector]
	public CreateAvatarPopup _characterSelect;
	
	public void ShowAvatarWindow(UI_Raid.CharacterSelectionHandler listenerInterface)
	{
		if(_characterSelect == null)
		{
			_characterSelect = gameObject.GetComponent<dfPanel>().AddPrefab(_characterSelectPrefab.gameObject).GetComponent<CreateAvatarPopup>();
			_characterSelect.SetListenerInterface(listenerInterface);
		}
	}
	
	public void Start()
	{
		_bossDetailsInfo._portalNavigator = _portalNavigator;
	}
	
	public void HideAvatarWindow()
	{
		if(_characterSelect != null)
		{
			Destroy(_characterSelect.gameObject);
			_characterSelect = null;
		}
	}
	
	public void ShowInvalidUsernameError(string message  = "Invalid Username")
	{
		if(_characterSelect != null)
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
		_bossDetailsInfo.GetComponent<dfPanel>().IsVisible = yesNo;
	}

	public void ShowMonsterUI(IDictionary raidInfo)
	{

		Debug.Log("[RaidsUI] ShowMonsterUI > " + MiniJSON.Json.Serialize(raidInfo));
		_bossDetailsInfo.ShowMonsterUI(raidInfo);
	}
	
	public void ShowRaidInfo(bool yesNo)
	{
		//_bossDetailsInfo.GetComponent<dfPanel>().IsVisible = yesNo;
	}
	
	#endregion
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == _leaderboardButton)
		{
			_leaderboardInterface.OnLeaderBoardClicked();
		}
		else if(mouseEvent.Source == _beachButton)
		{
			multiplayerBackButton();
		}
			
	}
	
	public void multiplayerBackButton()
	{
		GameManager.instance._monoHelpers.MainLightOn();
			
		GameObject.Find("ArcanumRuhalis(Clone)").GetComponent<RaidPortalNavigator>().DestroyCharacterObjects();
		
		GameObject.Destroy(GameManager.instance._levelManager.player.gameObject);
		GameManager.instance._levelManager.player = null;
		
		if(GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);

		#region comment out this code if you were not able to fix :P
		GameManager.instance.isMultiPlayerMode=false;
		GameManager.instance.currentMultiPlayerLevel="";
		GameObject levelManagerGameObject = GameManager.instance._levelManager.gameObject;
		LevelManager singlePlayerLevelManager = (LevelManager)levelManagerGameObject.GetComponent<LevelManager>();
//		GameManager.instance._levelManager.player.parent=null;
//		singlePlayerLevelManager.player=GameManager.instance._levelManager.player;
//		//singlePlayerLevelManager.daySkybox=GameManager.instance._levelManager.daySkybox;
//		GameManager.instance._levelManager.player.localScale=new Vector3(1,1,1);

		GameManager.instance._levelManager.enabled=false;
		GameManager.instance._levelManager = singlePlayerLevelManager;
		GameManager.instance._levelManager.enabled=true;
		
		Destroy(gameObject);
				#endregion
				
	}
	*/
}
