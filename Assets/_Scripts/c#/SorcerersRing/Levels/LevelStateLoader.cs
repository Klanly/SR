using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelStateLoader {
	
	public static List<KeyValuePair<string,int>> LoadIsClearPoiList(IList poiIsClearList)
	{
		List<KeyValuePair<string,int>> poiKeyVal= new List<KeyValuePair<string, int>>();
		
		for(int i=0;i<poiIsClearList.Count;i++)
		{
			IDictionary aGameStatePoiItem=poiIsClearList[i] as IDictionary;
//			Debug.Log("aGameStatePoiItem"+aGameStatePoiItem["poi_id"].ToString());
			poiKeyVal.Add(new KeyValuePair<string,int>(aGameStatePoiItem["poi_id"].ToString(),Convert.ToInt32(aGameStatePoiItem["isCleared"])));
		}
		
		return poiKeyVal;
	}
	
	public static List<string> LoadLootList(IList tempLootList)
	{
		List<string> lootList=new List<string>();
		
		for(int i=0;i<tempLootList.Count;i++)
		{
		  lootList.Add(tempLootList[i].ToString());	
		}
		
		return lootList;	
	}
	
	public static List<KeyValuePair<string,string>> LoadPreviousInstantiatedEnemyList(IList POIEnemyKeyVal)
	{
		List<KeyValuePair<string,string>> enemyList= new List<KeyValuePair<string, string>>();
		
//		Debug.Log("gStateEnemyCount==="+POIEnemyKeyVal.Count);
		for(int i=0;i<POIEnemyKeyVal.Count;i++)
		{
			IDictionary aEnemy=POIEnemyKeyVal[i] as IDictionary;
			enemyList.Add(new KeyValuePair<string,string>(aEnemy["poi_id"].ToString(),aEnemy["enemyName"].ToString()));
		}
		
		return enemyList;
		
	}
	
	public static Level GenerateLevelData() //POIModelNew poiList
	{
		Level _tempLevel= new Level();
		_tempLevel=GameManager._dataBank.GetLevelDetails(GameManager._gameState.LevelState.levelID);
		//List<KeyValuePair<string,int>> poiKeyVal=GameManager._gameState.LevelState.poiIsClearList;
		
		//List<string>
//		for(int i=0;i<_tempLevel.poiNameList.Count;i++)
//		{
//			for(int j=0;j<poiKeyVal.Count;j++)
//			{
//				if(_tempLevel.poiNameList[i].Equals(poiKeyVal[j].Key))
//				{
//					poiList[i].isCleared=Convert.ToBoolean(poiKeyVal[j].Value);
//				}
//			}
//		}
		
		return _tempLevel;
	}
	
}
