using System;
using UnityEngine;

public class UI_Raid : MonoBehaviour
{
	//protected Value raidUI = null;
	System.Action ack;
	
	/*
	private UI_Raid(SFManager sfmgr, SFMovieCreationParams cp ):
		base( sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "Raidsinterface.swf" ),  cp )
	{
		AdvanceWhenGamePaused = true;
	}
	
	public UI_Raid(SFManager sfmgr, SFMovieCreationParams cp , System.Action ack):
		base( sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "Raidsinterface.swf" ),  cp )
	{
		this.ack = ack;
		AdvanceWhenGamePaused = true;
	}
	
	public UI_Raid(SFManager sfmgr, SFMovieCreationParams cp , System.Action ack, CharacterSelectionHandler _characterSelectionHandler):
		base( sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "Raidsinterface.swf" ),  cp )
	{
		this._characterSelectionHandler = _characterSelectionHandler;
		this.ack = ack;
		AdvanceWhenGamePaused = true;
	}
	*/
	
	/*
	public void OnRegisterSWFCallback( Value movieRef )
	{
		if(GameManager.PRINT_LOGS) Debug.Log("UI_RAID::OnRegisterSWFCallback()");
		
		raidUI = movieRef;
		if(GameManager.instance != null)
			SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);
		this.ack();
	}
	*/
	
	public void ShowMonsterUI(string monsterJson)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("monsterJson" + monsterJson);
		/*
		Value val = new Value(monsterJson, MovieID);
		Value[] args = {val};
		raidUI.Invoke("raidsInterfaceData", args);
		*/
	}

	public void ShowRaidInfo(bool yesNo)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("ShowRaidInfo yesNo" + yesNo);
		/*
		Value val = new Value(yesNo, MovieID);
		Value[] args = {val};
		raidUI.Invoke("showRaids", args);
		*/
	}
	
	public void leaderboardShow() //CB for leaderboard button click
	{
		if(_leaderBoardListener != null)
			_leaderBoardListener.OnLeaderBoardClicked();
	}
	
	public void leaderboardClose() //CB for leaderboard screen disappear
	{
		if(_leaderBoardListener != null)
			_leaderBoardListener.OnLeaderBoardClosed();
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
		/*GameManager.instance._levelManager.player.parent=null;
		singlePlayerLevelManager.player=GameManager.instance._levelManager.player;
		//singlePlayerLevelManager.daySkybox=GameManager.instance._levelManager.daySkybox;
		GameManager.instance._levelManager.player.localScale=new Vector3(1,1,1);
		*/
		GameManager.instance._levelManager.enabled=false;
		GameManager.instance._levelManager = singlePlayerLevelManager;
		GameManager.instance._levelManager.enabled=true;
		
				#endregion
				
	}
	
	public LeaderBoardListener _leaderBoardListener;
	public interface LeaderBoardListener
	{
		void OnLeaderBoardClicked();
		void OnLeaderBoardClosed();
	}
	
#region Multiplayer Character Select UI
	
	CharacterSelectionHandler _characterSelectionHandler;
	public interface CharacterSelectionHandler
	{
		void OnMaleCharacterSelected();
		void OnFemaleCharacterSelected();
		void OnContinueClick(string nameText, bool isMale);
		void OnCollectButton(string portalID);
	}
	
	public enum Gender {male, female}
	
	public void CreateCharacter(Gender gender)
	{
		/*
		Value val = new Value(gender.ToString(), MovieID);
		Value[] args = {val};
		raidUI.Invoke("createCharacter", args);
		*/
	}
	
	public void onMaleClick() //CB for when male is clicked
	{
		if(_characterSelectionHandler != null)
			_characterSelectionHandler.OnMaleCharacterSelected();
	}
	
	public void onFeMaleClick() //CB for when female is clicked
	{
		if(_characterSelectionHandler != null)
			_characterSelectionHandler.OnFemaleCharacterSelected();
	}
	
	public void onContinueClick(string nameText, bool isMale)
	{
		nameText = Nonce.GetUniqueID();
		if(GameManager.PRINT_LOGS) Debug.Log("onContinueClick ::::::::::::::::::: nameText " + nameText + " ::::::::::::::::::::::::::" + isMale);
		if(_characterSelectionHandler != null)
			_characterSelectionHandler.OnContinueClick(nameText, isMale);
	}
	
	public void onCreateCharacterTextFocus()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("onCreateCharacterTextFocus");
		GameManager.instance.keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
		GameManager.instance.keyBoardInputListener = onGetCreateCharacterText;
	}
	
	public void SetCreateChracterText(string characterName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("SetCreateChracterText string text" + characterName);
		/*
		Value val = new Value(characterName, MovieID);
		Value[] args = {val};
		raidUI.Invoke("setCreateChracterText", args);
		*/
	}
	
	public void GetCreateChracterText() 
	{ 
		if(GameManager.PRINT_LOGS) Debug.Log("getCreateChracterText");
		/*
		Value[] args = {};
		raidUI.Invoke("getCreateChracterText", args);
		*/
	}
	
	public void onGetCreateCharacterText(string characterName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("onGetCreateCharacterText string nameText" + characterName);
		if(!characterName.Equals(""))
			SetCreateChracterText(characterName);
	}
	
	public void HideCharacterSelect()
	{
		/*
		Value[] args ={};
		raidUI.Invoke("removeCharacter",args);
		*/
	}
	
	public void ShowErrorForCharacterCreation(string errorMessage)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("ShowErrorForCharacterCreation errorMessage text" + errorMessage);
		/*
		Value val = new Value(errorMessage, MovieID);
		Value[] args = {val};
		raidUI.Invoke("showError", args);
		*/
	}
	
	public void onRaidCollectButton(string portalId)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("onRaidCollectButton = " + portalId);
		_characterSelectionHandler.OnCollectButton(portalId);
	}
	
	private void SetLanguage(string languageString, string charSetID)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("RAID UI --- private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
		/*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		raidUI.Invoke("setLanguage",args1);
		*/
	}
	
	/*public void missingString(string missingThings)
	{
		if(Debug.isDebugBuild)
		{
			GameManager.instance.scaleformCamera.generalSwf.words +=missingThings+"\n";
			if(GameManager.PRINT_LOGS) Debug.Log("Word Recieved :::: "+missingThings+" Current Words :::"+GameManager.instance.scaleformCamera.generalSwf.words);
		}
	}*/
	
	
#endregion
}
