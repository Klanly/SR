using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShrineManager {
	
	private static ShrineManager _instance = null;
	
	public static ShrineManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new ShrineManager();
			}
			return _instance;
		}
	}
	
	private ShrineManager()
	{
	}
	
	public Shrine aFireShrine=null;
	public Shrine aWaterShrine = null;
	public Shrine aEarthShrine = null;
	public Shrine aLightningShrine = null;

	public bool isShrineInfoRecieved
	{
		get; set;
	}
	
	public bool shrineConnectionProblem
	{
		get;set;
	}
	public string element
	{
		get;set;
	}
	
	public void LoadShrineState(IList shrineList)
	{
//		Debug.LogError("Load Shrine data");
		for(int i=0;i<shrineList.Count;i++)
		{
			IDictionary tempShrine = shrineList[i] as IDictionary;
//			Debug.LogError("type - "+tempShrine["element"].ToString()+" "+System.Convert.ToBoolean(tempShrine["isActivated"]));

			switch(tempShrine["element"].ToString())
			{
				case "fire":
				aFireShrine = new Shrine(GestureEmitter.Gesture.kFire);
				aFireShrine.Tag = "F";
				aFireShrine.Type = GestureEmitter.Gesture.kFire;
				aFireShrine.isCharged =System.Convert.ToBoolean(tempShrine["isCharged"]);
				aFireShrine.isLocked = System.Convert.ToBoolean(tempShrine["isLocked"]);
				aFireShrine.isActivated = System.Convert.ToBoolean(tempShrine["isActivated"]);
//				Debug.LogError("Shrine is "+aFireShrine.isActivated);
				if(!aFireShrine.isActivated)
				{
					aFireShrine.shrineLevel=1;
					foreach(ShrineInfo shrineInfo in GameManager._dataBank.ShrineDataDictionary.Values) {
						if(shrineInfo.shrineLevel == aFireShrine.shrineLevel) {
							aFireShrine.maxShrinePoints = shrineInfo.maxShrinePoints;
							aFireShrine.currentShrinePoints = shrineInfo.maxShrinePoints - 1;
							aFireShrine.guildShrinePoints = shrineInfo.maxShrinePoints - 1;
//							Debug.LogError("fire Shrine values changed "+aFireShrine.currentShrinePoints+" guild - "+aFireShrine.guildShrinePoints);
							break;
						}
					}
				}
				break;
				case "water":
				aWaterShrine = new Shrine(GestureEmitter.Gesture.kWater);
				aWaterShrine.Tag = "W";
				aWaterShrine.Type = GestureEmitter.Gesture.kWater;
				aWaterShrine.isCharged =System.Convert.ToBoolean(tempShrine["isCharged"]);
//				Debug.LogError("ischarge - "+aWaterShrine.isCharged);
				aWaterShrine.isLocked = System.Convert.ToBoolean(tempShrine["isLocked"]);
				aWaterShrine.isActivated = true;
//				Debug.LogError("Shrine is "+aWaterShrine.isActivated);
				if(!aWaterShrine.isActivated)
				{
					aWaterShrine.shrineLevel =1 ;
					foreach(ShrineInfo shrineInfo in GameManager._dataBank.ShrineDataDictionary.Values) {
						if(shrineInfo.shrineLevel == aWaterShrine.shrineLevel) {
							aWaterShrine.maxShrinePoints = shrineInfo.maxShrinePoints;
							aWaterShrine.currentShrinePoints = shrineInfo.maxShrinePoints - 1;
							aWaterShrine.guildShrinePoints = shrineInfo.maxShrinePoints - 1;
//							Debug.LogError("water Shrine values changed "+aWaterShrine.currentShrinePoints+" guild - "+aWaterShrine.guildShrinePoints);
							break;
						}
					}
				}
				break;
				case "earth":
				aEarthShrine = new Shrine(GestureEmitter.Gesture.kEarth);
				aEarthShrine.Tag = "E";
				aEarthShrine.Type = GestureEmitter.Gesture.kEarth;
				aEarthShrine.isCharged =System.Convert.ToBoolean(tempShrine["isCharged"]);
				aEarthShrine.isLocked = System.Convert.ToBoolean(tempShrine["isLocked"]);
				aEarthShrine.isActivated = System.Convert.ToBoolean(tempShrine["isActivated"]);
//				Debug.LogError("Shrine is "+aEarthShrine.isActivated);
				if(!aEarthShrine.isActivated)
				{
					aEarthShrine.shrineLevel = 1;
					foreach(ShrineInfo shrineInfo in GameManager._dataBank.ShrineDataDictionary.Values) {
						if(shrineInfo.shrineLevel == aEarthShrine.shrineLevel) {
							aEarthShrine.maxShrinePoints = shrineInfo.maxShrinePoints;
							aEarthShrine.currentShrinePoints = shrineInfo.maxShrinePoints - 1;
							aEarthShrine.guildShrinePoints = shrineInfo.maxShrinePoints - 1;
//							Debug.LogError("earth Shrine values changed "+aEarthShrine.currentShrinePoints+" guild - "+aEarthShrine.guildShrinePoints);
							break;
						}
					}
				}
				break;
				case "lightning":
				aLightningShrine = new Shrine(GestureEmitter.Gesture.kLightning);
				aLightningShrine.Tag = "L";
				aLightningShrine.Type = GestureEmitter.Gesture.kLightning;
				aLightningShrine.isCharged =System.Convert.ToBoolean(tempShrine["isCharged"]);
				aLightningShrine.isLocked = System.Convert.ToBoolean(tempShrine["isLocked"]);
				aLightningShrine.isActivated = System.Convert.ToBoolean(tempShrine["isActivated"]);
//				Debug.LogError("Shrine is "+aLightningShrine.isActivated);
				if(!aLightningShrine.isActivated)
				{
					aLightningShrine.shrineLevel = 1;
					foreach(ShrineInfo shrineInfo in GameManager._dataBank.ShrineDataDictionary.Values) {
						if(shrineInfo.shrineLevel == aLightningShrine.shrineLevel) {
							aLightningShrine.maxShrinePoints = shrineInfo.maxShrinePoints;
							aLightningShrine.currentShrinePoints = shrineInfo.maxShrinePoints - 1;
							aLightningShrine.guildShrinePoints = shrineInfo.maxShrinePoints - 1;
//							Debug.LogError("lightning Shrine values changed "+aLightningShrine.currentShrinePoints+" guild - "+aLightningShrine.guildShrinePoints);
							break;
						}
					}
				}
				break;
			}
			
		}
	}
	
	public void RefreshShrines()
	{
		if(aFireShrine != null && !aFireShrine.isLocked)
		{
			aFireShrine.isCharged = false;
		}
		
		if(aWaterShrine != null && !aWaterShrine.isLocked)
		{
			aWaterShrine.isCharged = false;
		}
		
		if(aEarthShrine != null && !aEarthShrine.isLocked)
		{
			aEarthShrine.isCharged = false;
		}
		
		if(aLightningShrine != null && !aLightningShrine.isLocked)
		{
			aLightningShrine.isCharged = false;
		}
	}
	public void ProcessResponse(ServerResponse response)
	{
		if(GameManager.PRINT_LOGS) Debug.LogError("ProcessResponse - <<<<<<<<<<<<<<<<<<< "+response.Request.RequestType);
		switch(response.Request.RequestType)
		{
			case ServerRequest.ServerRequestType.ActivateShrine:
			
			ShrineResponse sResponseActivate = (ShrineResponse)response;
			ShrineRequest sRequestActivate=  (ShrineRequest)sResponseActivate.Request;

			UpdateShrine(sRequestActivate);
			break;

			case ServerRequest.ServerRequestType.ActivateFriendsShrine:
			
			ActivateShrine();
			break;

			case ServerRequest.ServerRequestType.CollectShrineReward:
			ShrineResponse sResponseReward = (ShrineResponse)response;
			ShrineRequest sRequestReward=  (ShrineRequest)sResponseReward.Request;
			if(sResponseReward.IsSuccess)
			{
				InventorySystem.InventoryItem shrineLootItem = DecideShrineLoot(GetShrineForLevel(GameManager.instance._levelManager.currentLevel+"_Shrine"));
				GameManager.instance.scaleformCamera.generalSwf.LoadLoot(shrineLootItem);
				UpdateShrine(sRequestReward);
			}
			else
			{
				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("ERROR","Something went wrong while extracting loot from shrine. Please Try Again In A While");
			}
			break;
		}
		GameManager.instance.SaveGameState(false);
	}
	
	public void CollectShrineReward()
	{
		if(GetShrineForLevel(element).isActivated)
		{
			ServerRequest request = new ShrineRequest(ServerRequest.ServerRequestType.CollectShrineReward, ProcessResponse,element);
			ServerManager.Instance.ProcessRequest(request);
		}
		else
		{
			InventorySystem.InventoryItem shrineLootItem = DecideShrineLoot(GetShrineForLevel(element));
			if(shrineLootItem as InventorySystem.ItemRing != null || shrineLootItem as InventorySystem.ItemRune != null)
				shrineLootItem.isNew = true;
			
			GameManager.instance.scaleformCamera.generalSwf.LoadLoot(shrineLootItem);
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted))
			{
				TutorialManager.instance.ShrineTutorial5(() => {
					UIManager.instance.generalSwf.onShrineTutorialEnd();
				});
			}
			GameManager.instance.SaveGameState(false);
		}
	}
	
	
	public void UpdateShrine(ShrineRequest shrineRequestServer)
	{
		Shrine shrineToUpdate = GetShrineForLevel(shrineRequestServer.shrineType);
		if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ "+shrineToUpdate.Type.ToString()+" SHRINE UPDATED ~~~~~~~~~~~~ current - "+shrineToUpdate.currentShrinePoints+" guild - "+shrineToUpdate.guildShrinePoints);
		shrineToUpdate.isCharged=true;
		shrineToUpdate.isActivated=true;
		shrineToUpdate.currentShrinePoints +=1;
		shrineToUpdate.guildShrinePoints +=1;
		//GameManager.instance.scaleformCamera.generalSwf.ToggleChargeShrine(!aFireShrine.isCharged);
		GameManager.instance.scaleformCamera.generalSwf.SetShrineBar(shrineToUpdate.guildShrinePoints,shrineToUpdate.maxShrinePoints);
		Debug.LogError("before SetShrine Level");
		SetShrineLevel(shrineToUpdate.Type);
		Debug.LogError("current charges - "+shrineToUpdate.guildShrinePoints+" "+shrineToUpdate.maxShrinePoints);
		Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
		analParams.Add(Analytics.ParamName.ShinePoints, shrineToUpdate.guildShrinePoints.ToString());
		Analytics.logEvent(Analytics.EventName.Shrine_Charge, analParams);
	}

	public void SetShrineLevel(GestureEmitter.Gesture kType)
	{
		bool isShrineLevelSelected=false;


		for(int i=1;i<GameManager._dataBank.ShrineDataDictionary.Count+1;i++)
		{
			if(!isShrineLevelSelected)
			{
				switch(kType)
				{
					case GestureEmitter.Gesture.kFire:
				
					if(aFireShrine.guildShrinePoints<GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints)
					{
						aFireShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aFireShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aFireShrine.shrineLevel);
						isShrineLevelSelected=true;
					}

					if(aFireShrine.guildShrinePoints>=GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints && GameManager._dataBank.ShrineDataDictionary[i].shrineLevel == GameManager._dataBank.ShrineDataDictionary.Count) {
						aFireShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aFireShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aFireShrine.shrineLevel);
						isShrineLevelSelected=true;
						Debug.LogError("Setting "+kType+" to max because charges are greater - "+aFireShrine.guildShrinePoints);
					}
					break;
				
					case GestureEmitter.Gesture.kWater:
					Debug.LogError("kWater - i = "+i+" count = "+GameManager._dataBank.ShrineDataDictionary.Count);
					if(GameManager._dataBank.ShrineDataDictionary.ContainsKey(i)) {
						Debug.LogError("Contains a - "+i);
						Debug.LogError("Contains b - "+GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints);
						Debug.LogError("Contains c - "+i);
					} else {
						Debug.LogError("Count - "+GameManager._dataBank.ShrineDataDictionary.Count);
						foreach(int keys in GameManager._dataBank.ShrineDataDictionary.Keys) {
							Debug.LogError("In ELse - "+keys+" "+GameManager._dataBank.ShrineDataDictionary[keys]);
						}
					}
					if(GameManager._dataBank.ShrineDataDictionary[i] == null) {
							Debug.LogError("kWater - is null");
					}
					if(aWaterShrine.guildShrinePoints<GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints)
					{
						aWaterShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aWaterShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aWaterShrine.shrineLevel);
						isShrineLevelSelected=true;
					}
					if(aWaterShrine.guildShrinePoints>=GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints && GameManager._dataBank.ShrineDataDictionary[i].shrineLevel == GameManager._dataBank.ShrineDataDictionary.Count) {
						aWaterShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aWaterShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aWaterShrine.shrineLevel);
						isShrineLevelSelected=true;
						Debug.LogError("Setting "+kType+" to max because charges are greater - "+aWaterShrine.guildShrinePoints);
					}
					break;
				
					case GestureEmitter.Gesture.kEarth:
				
					if(aEarthShrine.guildShrinePoints<GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints)
					{
						aEarthShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aEarthShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aEarthShrine.shrineLevel);
						isShrineLevelSelected=true;
					}
					if(aEarthShrine.guildShrinePoints>=GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints && GameManager._dataBank.ShrineDataDictionary[i].shrineLevel == GameManager._dataBank.ShrineDataDictionary.Count) {
						aEarthShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aEarthShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aEarthShrine.shrineLevel);
						isShrineLevelSelected=true;
						Debug.LogError("Setting "+kType+" to max because charges are greater - "+aEarthShrine.guildShrinePoints);
					}
					break;
				
					case GestureEmitter.Gesture.kLightning:
				
					if(aLightningShrine.guildShrinePoints<GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints)
					{
						aLightningShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aLightningShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aLightningShrine.shrineLevel);
						isShrineLevelSelected=true;
					}
					if(aLightningShrine.guildShrinePoints>=GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints && GameManager._dataBank.ShrineDataDictionary[i].shrineLevel == GameManager._dataBank.ShrineDataDictionary.Count) {
						aLightningShrine.shrineLevel= GameManager._dataBank.ShrineDataDictionary[i].shrineLevel;
						aLightningShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[i].maxShrinePoints;
						GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(aLightningShrine.shrineLevel);
						isShrineLevelSelected=true;
						Debug.LogError("Setting "+kType+" to max because charges are greater - "+aLightningShrine.guildShrinePoints);
					}
					break;
				}
			}
		}
	}
	
	public Shrine GetShrineForLevel(string shrineType)
	{
		Shrine levelShrine= null;
		switch (shrineType)
		{
		case "BeachCamp_Shrine":
		case "water":
			levelShrine = aWaterShrine;
			break;
			
		case "HollowTree_Shrine":
		case "earth":
			levelShrine = aEarthShrine;
			break;
			
		case "ForbiddenCave_Shrine":
		case "fire":
			levelShrine = aFireShrine;
			break;
			
		case "ThreeGods_Shrine":
		case "lightning":
			levelShrine = aLightningShrine;
			break;
		}
		
		return levelShrine;
		
	}
	
	
	public InventorySystem.InventoryItem DecideShrineLoot(Shrine aShrine)
	{
		InventorySystem.InventoryItem LootedItem=null;

		Debug.LogError("Decide loot isActivated - "+aShrine.isActivated+" level - "+aShrine.shrineLevel+" getshrinelevel "+GetShrineForLevel(element).isActivated+" level - "+GetShrineForLevel(element).shrineLevel);
		if(!GetShrineForLevel(element).isActivated)
		{
			LootedItem = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel(GameManager._dataBank,2,GetShrineForLevel(element).Tag);
			GetShrineForLevel(element).isActivated=true;
			GetShrineForLevel(element).isCharged=false;
		}
		else
		{
			int randomNumber=Random.Range(1,101);
		
			if(GameManager.PRINT_LOGS) Debug.Log("Shrine Dictionaryyyy skull mode===="+GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].skullMode);
			int shrineLootSkullLevel;
			if(aShrine.shrineLevel==1)
			{
				shrineLootSkullLevel=Helpers.CheckForSkullLevelBounds(GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].skullMode);
			}
			else
			{
				shrineLootSkullLevel=Helpers.CheckForSkullLevelBounds(GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel-1].skullMode);
			}
			
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~randomNUMBERRRRshrine!!!!!!"+randomNumber);
			
			if(randomNumber<=GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].potionsPercentage)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Potion Shrine Loot");
				LootedItem = GameManager._dataBank.GetPotionForPotionID("HEALTH_POTION");
			}
			else if(randomNumber>GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].potionsPercentage && randomNumber<=GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].keysPercentage)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Key Shrine Loot");
				LootedItem = new InventorySystem.ItemKey();
			}
			else if(randomNumber>GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].keysPercentage && randomNumber<=GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].ringsPercentage)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Ring Shrine Loot");
				LootedItem = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel(GameManager._dataBank,shrineLootSkullLevel,aShrine.Tag);
			}
			else if(randomNumber>GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].ringsPercentage && randomNumber<=GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].runesPercentage)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Rune Shrine Loot");
				LootedItem = DatabankSystem.Databank.GetRandomNonpremiumRuneForSkullLevel(GameManager._dataBank,shrineLootSkullLevel,aShrine.Tag);
			}
			else if(randomNumber>GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].runesPercentage && randomNumber>GameManager._dataBank.ShrineDataDictionary[aShrine.shrineLevel].gemsPercentage)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("Gems Shrine Loot");
				LootedItem = new InventorySystem.ItemGem(GameManager._dataBank.GetChestGemsLootForSkullLevel(shrineLootSkullLevel));
			}
		}
		return LootedItem;
	}

	public void ActivateFriendShrine(string levelName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~ActivateShrine Called~~~~~~~~~~~");
		switch(levelName)
		{
		case "ForbiddenCave_Shrine":
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ FIRE SHRINE ACTIVATED ~~~~~~~~~~~~");
			element="fire";
			break;
			
		case "BeachCamp_Shrine":
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ WATER SHRINE ACTIVATED ~~~~~~~~~~~~");
			element="water";
			break;
			
		case "HollowTree_Shrine":
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ EARTH SHRINE ACTIVATED ~~~~~~~~~~~~");
			element="earth";
			
			break;
			
		case "ThreeGods_Shrine":
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ LIGHTNING SHRINE ACTIVATED ~~~~~~~~~~~~");
			element="lightning";
			break;
		}

		Shrine currentShrineState = GetShrineForLevel(levelName);
		if(currentShrineState.isActivated)
		{
			Debug.LogError("Activiate shrine for element = "+element);
//			ServerRequest request = new ShrineRequest(ServerRequest.ServerRequestType.ActivateFriendsShrine, ProcessResponse,element);
			ServerRequest request = new ShrineRequest(ServerRequest.ServerRequestType.ActivateShrine, ProcessResponse,element);
			ServerManager.Instance.ProcessRequest(request);
		}
		else
		{
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted))
			{
				TutorialManager.instance.ShrineTutorial4();
				GameManager.instance.scaleformCamera.generalSwf.ChargeArrowOffAndBoostDisable();
				Debug.LogError("EarttShrine not completed");
			}
			currentShrineState.guildShrinePoints = currentShrineState.maxShrinePoints;
			currentShrineState.isCharged = true;
			//GameManager.instance.scaleformCamera.generalSwf.ToggleChargeShrine(!currentShrineState.isCharged);
			GameManager.instance.scaleformCamera.generalSwf.SetShrineBar(currentShrineState.guildShrinePoints,currentShrineState.maxShrinePoints);
		}
	}

	public void ActivateShrine()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~ActivateShrine Called~~~~~~~~~~~");
//		switch(levelName)
//		{
//			case "ForbiddenCave_Shrine":
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ FIRE SHRINE ACTIVATED ~~~~~~~~~~~~");
//			element="fire";
//			break;
//			
//			case "BeachCamp_Shrine":
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ WATER SHRINE ACTIVATED ~~~~~~~~~~~~");
//			element="water";
//			break;
//			
//			case "HollowTree_Shrine":
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ EARTH SHRINE ACTIVATED ~~~~~~~~~~~~");
//			element="earth";
//			
//			break;
//			
//			case "ThreeGods_Shrine":
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~ LIGHTNING SHRINE ACTIVATED ~~~~~~~~~~~~");
//			element="lightning";
//			break;
//		}
		Shrine currentShrineState = GetShrineForLevel(element);
		if(currentShrineState.isActivated)
		{
			Debug.LogError("ActivateShrine for element = "+element);
			ServerRequest request = new ShrineRequest(ServerRequest.ServerRequestType.ActivateShrine, ProcessResponse,element);
			ServerManager.Instance.ProcessRequest(request);
		}
	}
	
	#region Shrine Server Handlers
	public void GetShrineInformationHandler(object responseParameters, object error, ServerRequest request)
	{
		//if(GameManager.PRINT_LOGS) Debug.Log("GetShrineInformation");
		isShrineInfoRecieved = true;
		shrineConnectionProblem=false;
		if(error == null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("ShrineInformation"+MiniJSON.Json.Serialize(responseParameters));
			IDictionary response = responseParameters as IDictionary;
			
			IDictionary responseShrines = response["shrines"] as IDictionary;
			
			IDictionary responseAShrine = null;
			if(responseShrines.Contains("earth"))
			{
				if(aEarthShrine == null)
				{
					aEarthShrine =new Shrine(GestureEmitter.Gesture.kEarth);
					aEarthShrine.Tag = "E";
					aEarthShrine.Type=GestureEmitter.Gesture.kEarth;
				}
				
				responseAShrine= responseShrines["earth"] as IDictionary;
				if(responseAShrine.Contains("points"))
				{
					aEarthShrine.currentShrinePoints = System.Convert.ToInt32(responseAShrine["points"]);
					aEarthShrine.guildShrinePoints = System.Convert.ToInt32(responseAShrine["GuildPoint"]);
					SetShrineLevel(aEarthShrine.Type);
					aEarthShrine.maxShrinePoints = GameManager._dataBank.ShrineDataDictionary[aEarthShrine.shrineLevel].maxShrinePoints;
				}
				
				if(responseAShrine.Contains("remainingTime"))
					aEarthShrine.remainingTime= (long)responseAShrine["remainingTime"];
				
				if(responseAShrine.Contains("locked"))
					aEarthShrine.isLocked= System.Convert.ToBoolean(responseAShrine["locked"]);

				if(responseAShrine.Contains("resetTime")) {
					long resetTime = (long)responseAShrine["resetTime"];
					if(resetTime > aEarthShrine.resetTime) {
						aEarthShrine.isCharged = false;
					}
					aEarthShrine.resetTime = resetTime;
				}

				if(responseAShrine.Contains("activeFriends"))
				{
					IList fList = responseAShrine["activeFriends"] as IList;
					for(int i=0;i<fList.Count;i++)
					{
						aEarthShrine.FriendsList.Add(fList[i].ToString());
					}
				}
			}			
			
			if(responseShrines.Contains("fire"))
			{
				if(aFireShrine == null)
				{
					aFireShrine =new Shrine(GestureEmitter.Gesture.kFire);
					aFireShrine.Tag = "F";
					aFireShrine.Type = GestureEmitter.Gesture.kFire;
				}
				responseAShrine= responseShrines["fire"] as IDictionary;
				if(responseAShrine.Contains("points"))
				{
					aFireShrine.currentShrinePoints = System.Convert.ToInt32(responseAShrine["points"]);
					aFireShrine.guildShrinePoints = System.Convert.ToInt32(responseAShrine["GuildPoint"]);
					SetShrineLevel(aFireShrine.Type);
					aFireShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[aFireShrine.shrineLevel].maxShrinePoints;
				}
				
				if(responseAShrine.Contains("remainingTime"))
					aFireShrine.remainingTime= (long)responseAShrine["remainingTime"];
				
				if(responseAShrine.Contains("locked"))
				aFireShrine.isLocked= System.Convert.ToBoolean(responseAShrine["locked"]);

				if(responseAShrine.Contains("resetTime")) {
					long resetTime = (long)responseAShrine["resetTime"];
					if(resetTime > aFireShrine.resetTime) {
						aFireShrine.isCharged = false;
					}
					aFireShrine.resetTime = resetTime;
				}

				if(responseAShrine.Contains("activeFriends"))
				{
					IList fList = responseAShrine["activeFriends"] as IList;
					for(int i=0;i<fList.Count;i++)
					{
						aFireShrine.FriendsList.Add(fList[i].ToString());
					}
				}
			}
			
			if(responseShrines.Contains("water"))
			{
				if(aWaterShrine == null)
				{
					aWaterShrine =new Shrine(GestureEmitter.Gesture.kWater);
					aWaterShrine.Tag = "W";
					aWaterShrine.Type = GestureEmitter.Gesture.kWater;
					
				}
				responseAShrine= responseShrines["water"] as IDictionary;
				if(responseAShrine.Contains("points"))
				{
					aWaterShrine.currentShrinePoints = System.Convert.ToInt32(responseAShrine["points"]);
					aWaterShrine.guildShrinePoints = System.Convert.ToInt32(responseAShrine["GuildPoint"]);
					SetShrineLevel(aWaterShrine.Type);
					aWaterShrine.maxShrinePoints = GameManager._dataBank.ShrineDataDictionary[aWaterShrine.shrineLevel].maxShrinePoints;
				}
				
				if(responseAShrine.Contains("remainingTime"))
					aWaterShrine.remainingTime= (long)responseAShrine["remainingTime"];

				if(responseAShrine.Contains("locked"))
				aWaterShrine.isLocked= System.Convert.ToBoolean(responseAShrine["locked"]);

				if(responseAShrine.Contains("resetTime")) {
					long resetTime = (long)responseAShrine["resetTime"];
					if(resetTime > aWaterShrine.resetTime) {
						aWaterShrine.isCharged = false;
					}
					aWaterShrine.resetTime = resetTime;
				}

				if(responseAShrine.Contains("activeFriends"))
				{
					IList fList = responseAShrine["activeFriends"] as IList;
					for(int i=0;i<fList.Count;i++)
					{
						aWaterShrine.FriendsList.Add(fList[i].ToString());
					}
				}
			}
			
			if(responseShrines.Contains("lightning"))
			{
				if(aLightningShrine == null)
				{
					aLightningShrine =new Shrine(GestureEmitter.Gesture.kLightning);
					aLightningShrine.Tag = "L";
					aLightningShrine.Type = GestureEmitter.Gesture.kLightning;
				}
				responseAShrine= responseShrines["lightning"] as IDictionary;
				if(responseAShrine.Contains("points"))
				{
					aLightningShrine.currentShrinePoints = System.Convert.ToInt32(responseAShrine["points"]);
					aLightningShrine.guildShrinePoints = System.Convert.ToInt32(responseAShrine["GuildPoint"]);
					SetShrineLevel(aLightningShrine.Type);
					aLightningShrine.maxShrinePoints= GameManager._dataBank.ShrineDataDictionary[aLightningShrine.shrineLevel].maxShrinePoints;
				}
				
				if(responseAShrine.Contains("remainingTime"))
					aLightningShrine.remainingTime= (long)responseAShrine["remainingTime"];
				
				if(responseAShrine.Contains("locked"))
				aLightningShrine.isLocked= System.Convert.ToBoolean(responseAShrine["locked"]);

				if(responseAShrine.Contains("resetTime")) {
					long resetTime = (long)responseAShrine["resetTime"];
					if(resetTime > aLightningShrine.resetTime) {
						aLightningShrine.isCharged = false;
					}
					aLightningShrine.resetTime = resetTime;
				}

				if(responseAShrine.Contains("activeFriends"))
				{
					IList fList = responseAShrine["activeFriends"] as IList;
					for(int i=0;i<fList.Count;i++)
					{
						aLightningShrine.FriendsList.Add(fList[i].ToString());
					}
				}
			}
		}
		else
		{
			shrineConnectionProblem = true;
			GameManager.instance.scaleformCamera.generalSwf.generalSwf.showUiGeneralPopup("CONNECTION ERROR", "CHECK YOUR INTERNET CONNECTIVITY AND TRY AGAIN !!!", () => {});
		}
	}

	#endregion
	
	public void ShowShrinePopUp()
	{
		Debug.Log("<<<<< ShowShrinePopUp >>>>");

		if(shrineConnectionProblem)
		{
			GameManager.instance.scaleformCamera.isPaused=false;
			Time.timeScale=1.0f;
			Debug.LogError("<<<<< ShowShrinePopUp >>>> shrineConnectionProblem");
		}
		else
		{
			switch (GameManager.instance._levelManager.currentLevel+"_Shrine")
			{
				case "BeachCamp_Shrine":
				GameManager.instance.scaleformCamera.generalSwf.ShowShrinePopup("Water Shrine",aWaterShrine.shrineLevel,aWaterShrine.guildShrinePoints,aWaterShrine.maxShrinePoints,aWaterShrine.remainingTime,aWaterShrine.isCharged, aWaterShrine.isLocked);
				element = "water";
				if(GameManager.PRINT_LOGS) Debug.Log("aWaterShrine.isCharged"+aWaterShrine.isCharged);
				break;
				
				case "HollowTree_Shrine":
				GameManager.instance.scaleformCamera.generalSwf.ShowShrinePopup("Earth Shrine",aEarthShrine.shrineLevel,aEarthShrine.guildShrinePoints,aEarthShrine.maxShrinePoints,aEarthShrine.remainingTime,aEarthShrine.isCharged, aEarthShrine.isLocked);
				element = "earth";
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted))
				{
					//GameManager.instance.scaleformCamera.NewTutorialSwf.ShrineTutorial3();
					TutorialManager.instance.ShrineTutorial3();
					GameManager.instance.scaleformCamera.generalSwf.ShrineRingChargeTutorial();
				}
				break;
				
				case "ForbiddenCave_Shrine":
				GameManager.instance.scaleformCamera.generalSwf.ShowShrinePopup("Fire Shrine",aFireShrine.shrineLevel,aFireShrine.guildShrinePoints,aFireShrine.maxShrinePoints,aFireShrine.remainingTime,aFireShrine.isCharged, aFireShrine.isLocked);
				element = "fire";
				break;
					
				case "ThreeGods_Shrine":
				GameManager.instance.scaleformCamera.generalSwf.ShowShrinePopup("Lightning Shrine",aLightningShrine.shrineLevel,aLightningShrine.guildShrinePoints,aLightningShrine.maxShrinePoints,aLightningShrine.remainingTime,aLightningShrine.isCharged, aLightningShrine.isLocked);
				element = "lightning";
				break;
			}
			
			GameManager.instance.scaleformCamera.isPaused=true;
//			Time.timeScale=0.0f;

		}

	}
	
	public List<Dictionary<string,object>> ToDictionary()
	{	 
		List<Dictionary<string,object>> activatedShrineDictionary = new List<Dictionary<string,object>>();
			
		if(aFireShrine!=null)
		{
			activatedShrineDictionary.Add(aFireShrine.ToDictionary());
		}
		
		if(aWaterShrine!=null)
		{
			activatedShrineDictionary.Add(aWaterShrine.ToDictionary());
		}
		
		if(aEarthShrine!=null)
		{
			activatedShrineDictionary.Add(aEarthShrine.ToDictionary());
		}
		
		if(aLightningShrine!=null)
		{
			activatedShrineDictionary.Add(aLightningShrine.ToDictionary());
		}
		//if(GameManager.PRINT_LOGS) Debug.Log("SHRINE JSON===="+MiniJSON.Json.Serialize(activatedShrineDictionary));
		return activatedShrineDictionary;
	}
	
}
