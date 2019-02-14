using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelState {
	
	public string levelID
	{
		get;set;
	}
	
	public string zoneID
	{
		get;set;
	}
	
	public List<KeyValuePair<string,int>> poiIsClearList
	{
		get;set;
	}
	
	// Loot List: Name of all the looted pois for a skull level. It is updated when skull level is updated.
	public List<string> lootList
	{
		get;set;
	}
	
	public List<KeyValuePair<string,string>> POIEnemyKeyVal
	{
		get;set;
	}
	
	public LevelState()
	{
		this.lootList=new List<string>();
	}
	
	
	public bool ValidateLevelState(LevelState levelState)
	{	
		bool isLevelStateValid=true;
		if(!ValidateLevelID(levelState.levelID))
		{
			isLevelStateValid=false;
		}

		if(!ValidateZoneID(levelState.zoneID))
		{
			isLevelStateValid=false;
		}
		
		if(ValidateEnemyList(levelState.POIEnemyKeyVal))
		{
			isLevelStateValid=false;
		}
		
		return isLevelStateValid;
	}
	
	private bool ValidateEnemyList(List<KeyValuePair<string,string>> enemyList)
	{
		bool isValid=false;
		if(enemyList.Count>0)
		{
			isValid=true;
		}
		return isValid;
	}
	
	private bool ValidateZoneID(string zoneID)
	{
		bool isValid=false;
		if(!zoneID.Equals(""))
		{
			isValid=true;
		}

		return isValid;
	}
	
	private bool ValidateLevelID(string levelID)
	{
		bool isValid=false;
		if(!levelID.Equals(""))
		{
			isValid=true;
		}
		return isValid;
	}
	
	public void UpdateEnemyList(string poiID,string enemyName)
	{
		if(!GameManager.instance.isMultiPlayerMode)
		{
			bool enemyExists=false;
			for(int i=0;i<POIEnemyKeyVal.Count;i++)
			{
				if(POIEnemyKeyVal[i].Key.Equals(poiID))
				{
					enemyExists=true;
					break;
				}
			}
			
			if(!enemyExists)
			GameManager._gameState.LevelState.POIEnemyKeyVal.Add(new KeyValuePair<string,string>(poiID,enemyName));
		}
	}
	
	public void UpdateLevelState(string levelId,string zoneId,List<POIModel> writePoiState)
	{
		if(!GameManager.instance.isMultiPlayerMode)
		{
			this.levelID=levelId;
			this.zoneID=zoneId;
			List<KeyValuePair<string,int>> newPoiIsClearList= new List<KeyValuePair<string, int>>();
			//List<KeyValuePair<string,string>> OldEnemies=new List<KeyValuePair<string,string>>();
			
			for(int i=0;i<writePoiState.Count;i++)
			{		
				newPoiIsClearList.Add(new KeyValuePair<string,int>(writePoiState[i].poiId,Convert.ToInt32(writePoiState[i].isCleared)));
			}
			
			//lootList.Clear();
			this.poiIsClearList=newPoiIsClearList;
		}
	}
	
	
	public IDictionary ToDictionary()
	{
		IDictionary LevelDictionary=new Dictionary<string,object>();
		
		List<Dictionary<string,object>> poiClearList=new List<Dictionary<string,object>>();
		List<Dictionary<string,object>> poiEnemyKeyValuePair=new List<Dictionary<string, object>>();

		LevelDictionary.Add("LevelID",this.levelID);
		
		LevelDictionary.Add("ZoneID",this.zoneID);
		
		
		for(int i=0;i<poiIsClearList.Count;i++)
		{
			Dictionary<string,object> aPoiClearObject=new Dictionary<string,object>();
			aPoiClearObject.Add("poi_id",poiIsClearList[i].Key);
			aPoiClearObject.Add("isCleared",poiIsClearList[i].Value);
			poiClearList.Add(aPoiClearObject);
		}
		
		for(int i=0;i<POIEnemyKeyVal.Count;i++)
		{
			Dictionary<string,object> aEnemyKeyVal=new Dictionary<string, object>();
			aEnemyKeyVal.Add("poi_id",POIEnemyKeyVal[i].Key);
			aEnemyKeyVal.Add("enemyName",POIEnemyKeyVal[i].Value);
			poiEnemyKeyValuePair.Add(aEnemyKeyVal);
		}
		
		LevelDictionary.Add("PoiList",poiClearList);
		
		LevelDictionary.Add("LootList",this.lootList);
		
		LevelDictionary.Add("EnemyList",poiEnemyKeyValuePair);
		
		return LevelDictionary;
	}
	
}


