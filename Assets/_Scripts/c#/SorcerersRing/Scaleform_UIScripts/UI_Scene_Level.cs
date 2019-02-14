using System;
using System.Timers;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;


public class UI_Scene_Level : MonoBehaviour, LevelUIInterface
{
	public NGLevelUIScript levelScene = null;
	
	float totalSoulGems;
	float totalHealth;
	float totalSoulDust;
	UIManager sfCamera = null;
	private bool canSkip;

	/*
	public UI_Scene_Level(ScaleformCamera parent,SFManager sfmgr, SFMovieCreationParams cp):
	base(sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "level1.1_v2.swf" ), cp)
	{
		AdvanceWhenGamePaused=true;
		sfCamera=parent;
		//SetDepth(4);
	}
	*/

	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void OnRegisterSWFCallback(NGLevelUIScript movieRef)
	{
		sfCamera = UIManager.instance;

		levelScene = movieRef;

		Init();
		CheckUsername();
//		sfCamera.LoadGuildUI();
	}
	
	private void CheckUsername()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("CheckUsername :::::::::::::::: CheckUsername" + GameManager._gameState.User.username);
//		if(string.IsNullOrEmpty(GameManager._gameState.User.username))
//			ServerManager.Instance.GetUserNameAndEmail(this.OnUserDetailReceived);
//		else
		if(!string.IsNullOrEmpty(GameManager._gameState.User.username))
			sfCamera.LoadGuildUI();
	}
	
	private void OnUserDetailReceived(object responseParameters, object error, ServerRequest request)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("OnUserDetailReceived");
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			if(GameManager.PRINT_LOGS) Debug.Log("OnUserDetailReceived ::::::::::::::::::: response = " + MiniJSON.Json.Serialize(response));
			bool responseSuccess = bool.Parse(response["success"].ToString());
			
			if(response.Contains("userName") && response["userName"] != null && !string.IsNullOrEmpty(response["userName"].ToString()))
			{
				Debug.Log("username equals userid ? >> " + (response["userName"].ToString() == GameManager.instance.fragNetworkingNew.GetUserID()));
				GameManager._gameState.User.username = response["userName"].ToString();
				CheckUsername();
			}
		}
	}
	
	
	public void Init()
	{
		sfCamera.generalSwf.Init();
		UpdateTotalKeys(GameManager._gameState.User._inventory.keyRing.keyCount);
		UpdateArcanePoints(((int)GameManager._gameState.User.arcanePoints));
//		canSkip = true;
	}	
	
	public void UpdateArcanePoints(int arcanePoints)
	{
		/*
		Value val1 = new Value(arcanePoints,MovieID);
		Value[] args = {val1};
		levelScene.Invoke("updateArc",args);
		*/
		levelScene.UpdateArc(arcanePoints);
		
	}
	
#region old code region	
	public void UpdateTotalKeys(int keys)
	{
		/*
		Value val1 = new Value(keys,MovieID);
		
		Value[] args = {val1};
		
		levelScene.Invoke("updateKeys",args);
		*/
		
		levelScene.UpdateKeys(keys);
	}
	
	public void SetDisplayVisible(bool isVisible)
	{
//		Debug.Log("[UI_Scene_level] SetDisplayVisible = " + isVisible);
		//UIManager.instance.generalSwf.ToggleTopStats(isVisible);
		//sfCamera.generalSwf.ToggleTopStats(isVisible);
		if(levelScene != null)
		{
			/*
			bool isAlreadyLoaded = (bool)levelScene.GetMember("visible");
			levelScene.SetMember("visible", new Value(isVisible, levelScene.MovieId));
			*/
			levelScene.SetVisible(isVisible);
		}
	}
	
	
	public void onHealthIcon()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("HEALTH ICON TAPPED!!");
		sfCamera.generalSwf.LoadPotionPopup(GameManager._gameState.User._inventory.potionBelt.Count(), GameManager._dataBank.inApps.GetGCost(InApp.InAppTypes.PORTIONS));
	}

	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void RegisterMovies(NGLevelUIScript movieRef)
	{
		levelScene = movieRef;
	}
	
	/*
	public void UpdateHealth(int health)
	{
	//	if(GameManager.PRINT_LOGS) Debug.Log("USER TOTAL HEALTH >>>>>" + GameManager._gameState.User.totalLife + "Heath sent ::::::::" + health);
		Value val=new Value(health,MovieID);
		Value val2;
		if(health <= GameManager._gameState.User.totalLife*0.3f)
		{
			val2 = new Value(true,MovieID); // passing true to animate heart beat
		}
		else
		{
			val2 = new Value(false,MovieID); // passing false to stop animating heart beat
		}
		Value[] args1={val,val2};
		levelScene.Invoke("updateHealth",args1);
	//	Value[] args2={};
		GameManager._gameState.User.SetRingStats();
	}
	
	public void UpdateSoulGems(int soulGems)
	{
		Value val=new Value(soulGems,MovieID);
		Value[] args1={val};
		levelScene.Invoke("updateSoulGems",args1);
	}
	*/
	List<string> CreateAiModelSpellArray(AIModel aiModel)
	{
		List<string> toSendArray= new List<string>();
		
		if(aiModel.fire>0)
		{
			toSendArray.Add("Fire");
		}
		
		if(aiModel.water>0)
		{
			toSendArray.Add("Water");
		}
		
		if(aiModel.earth>0)
		{
			toSendArray.Add("Earth");
		}
		
		if(aiModel.lightning>0)
		{
			toSendArray.Add("Lightning");
		}
		
		return toSendArray;
	}
	
	string CreateAiModelSpellJson(AIModel aiModel)
	{
		//List<string> toSendArray= new List<string>();
		string json="{\"monsterspell\":{";
		
		if(aiModel.fire>0)
		{
			json += "\"fire\": "+"true,";
		}
		else
		{
			json += "\"water\": "+"false,";
		}
		
		if(aiModel.water>0)
		{
			json += "\"water\": "+"true,";
		}
		else
		{
			json += "\"water\": "+"false,";
		}	
		if(aiModel.earth>0)
		{
			json += "\"earth\": "+"true,";
		}
		else
		{
			json += "\"earth\": "+"false,";
		}
		
		if(aiModel.lightning>0)
		{
			json += "\"lightning\": "+"true";
		}
		else
		{
			json += "\"lightning\": "+"false";
		}
		
		int count = aiModel.availableBuffs.Count;
		string buff = null;
		for(int i = 0;i<count;i++)
		{
			buff = aiModel.availableBuffs[i];
			json += ",\""+buff.ToLower() +"\":true";
		}
		
		json += "}}";
		
		if(GameManager.PRINT_LOGS) Debug.Log("SpellArrayJson==="+json);
		return json;
	}
	
	public void HideMonsterIcon()
	{
		/*
		Value[] args={};
		levelScene.Invoke("HideMonsterIcon",args);
		*/
		
		levelScene.HideMonsterIcon();
	}
	
	public void ShowMonsterIcon()
	{
//		Debug.Log("ShowMonsterIcon");
		/*
		Value[] args={};
		levelScene.Invoke("ShowMonsterIcon",args);
		*/
		
		levelScene.ShowMonsterIcon();
	}
	
	public void ShowMonsterStats(AIModel aiModel)
	{
		/*
		//if(GameManager.PRINT_LOGS) Debug.Log("Showing Monster Stats>>>"+" MonsterName="+MonsterName+" MonsterLevel="+MonsterLevel+" MonsterHealth="+MonsterHealth+" MonsterDamage="+MonsterDamage);
		Value val1= new Value(aiModel.name.Replace('_', ' '),MovieID);	
		Value val2= new Value(aiModel.skullLevel,MovieID);	
		Value val3= new Value(aiModel.life,MovieID);	
		Value val4= new Value(aiModel.damage,MovieID);
		
		//List<string> tempArr = CreateAiModelSpellArray(aiModel);
		*/
		string json = CreateAiModelSpellJson(aiModel);
		/*
		Value val5 = new Value(json,MovieID);

		Value[] args = {val1,val2,val3,val4,val5};
		//if(GameManager.PRINT_LOGS) Debug.Log("ArrayToSenddd ==="+arrayTosend.GetArraySize());
		levelScene.Invoke("MonsterStats",args);
		*/
		levelScene.MonsterStats(aiModel.name.Replace('_', ' '), aiModel.skullLevel, aiModel.life, aiModel.damage, MiniJSON.Json.Deserialize(json) as IDictionary);
	}
	
	public void onMonsterIcon()
	{
		if(GameManager._gameState.LevelState.POIEnemyKeyVal.Count>0)
		{
			KeyValuePair<string,int> poi;
			int count = GameManager._gameState.LevelState.poiIsClearList.Count;
			for(int i = 0;i<count;i++)
			{
				poi = GameManager._gameState.LevelState.poiIsClearList[i];

				if(poi.Key.Equals(GameManager.instance._levelManager.currentLevel+"_DuelA"))
				{
					if(poi.Value==0)
					{
						if(GameManager.PRINT_LOGS) Debug.Log("POI POINT FOR ENEMY INFO=="+GameManager.instance._levelManager.currentLevel+"_DuelA");
						if(GameManager.PRINT_LOGS) Debug.Log("Displaying enmey info"+GameManager.instance.getEnemyFromGameState(GameManager.instance._levelManager.currentLevel+"_DuelA"));
						
						AIModel aiModel = null;
						string aiName = GameManager.instance.getEnemyFromGameState(GameManager.instance._levelManager.currentLevel+"_DuelA");
						
						if(GameManager._gameState.bossAttemptDictionary.Contains(aiName) && Int32.Parse(GameManager._gameState.bossAttemptDictionary[aiName].ToString()) == 0)
							GameManager._gameState.bossAttemptDictionary[aiName] = 1;
		
						if(GameManager._gameState.bossAttemptDictionary.Contains(aiName))
							aiModel = GameManager._dataBank.GetModelForMonsterByEncounter(aiName, Int32.Parse(GameManager._gameState.bossAttemptDictionary[aiName].ToString()));
						else
							aiModel = GameManager._dataBank.GetModelForMonster(aiName, GameManager._gameState.skullLevel);
						
						//string[] tempArr = {"Fire","Water"};
						ShowMonsterStats(aiModel);
					}
					else
					{
						levelScene.MonsterStats("",0,0,0, null);
					}
				}
			}
		}
		else
		{
			if(GameManager.instance._levelManager.currentLevel.Equals("LavaScene")) {
				AIModel aiModel = GameManager.instance._levelManager.enemy.GetComponent<AICharacterControllerNEWAgain>()._aiModel;
				ShowMonsterStats(aiModel);
			} else {
				levelScene.MonsterStats("", 0, 0, 0, null);
			}
		}
	}
	
	public void setDaysText(int daynumber)
	{
		/*
		Value val1=new Value(daynumber,MovieID);
		Value[] args={val1};
		if(GameManager.PRINT_LOGS) Debug.Log("setDaysText CALLED <<<<<<<<<");
		levelScene.Invoke("showDaysText",args);
		*/

		levelScene.ShowDaysText(daynumber);
	}
	
	public void hideDaysText()
	{
		/*
		Value[] args={};
		if(GameManager.PRINT_LOGS) Debug.Log("setDaysText CALLED <<<<<<<<<");
		levelScene.Invoke("hideDaysText",args);
		*/
		
		levelScene.HideDaysText();
	}
	
	
	public void onKeysIcon()
	{
		sfCamera.generalSwf.onKeyPopUp(GameManager._gameState.User._inventory.keyRing.keyCount, GameManager._dataBank.inApps.GetGCost(InApp.InAppTypes.KEYS));
	}
/*
	public override void Update()
	{
		
	}
*/	
	// Callback from the content to launch the game when the "close" animation has finished playing.
//	public void OnStartGameCallback()
//	{
//		Application.LoadLevelAsync("StarshipDown");
//		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
//		
//		sfMgr.DestroyMovie(this);
//		ScaleformCamera sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
//		sfCamera.levelScene = null;
//		sfCamera.OnLevelLoadStart();
//	}

	// Callback from the content to launch the game when the "close" animation has finished playing.
//	public void OnExitGameCallback()
//	{
//		Console.WriteLine("In OnExitGameCallback");
//		//sfMgr.DestroyMovie(this);
//		// Application.Quit() is Ignored in the editor!
//		Application.Quit();
//		// Application.LoadLevelAsync("Level");
//		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
//	}
#endregion	
	
	private void SetLanguage(string languageString, string charSetID)
	{
		/*
		if(GameManager.PRINT_LOGS) Debug.Log("UI INVENTORY SCENE --- private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		levelScene.Invoke("setLanguage",args1);
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

}