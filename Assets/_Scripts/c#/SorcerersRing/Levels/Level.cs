using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level {
	
	public Level()
	{
	}
	
	public Level(string levelId, string zoneId)
	{
		this.levelID=levelId;
		this.zoneID=zoneId;
	}

	public string levelID
	{
		get;set;
	}
	
	public string zoneID
	{
		get;set;
	}
	
	public List<string> poiNameList
	{
		get;set;
	}
	
	public Level Clone()
	{
		Level levelClone = new Level();
		levelClone.poiNameList=new List<string>();
		
		levelClone.levelID=this.levelID;
		levelClone.zoneID=this.zoneID;
		
		for(int i=0;i<this.poiNameList.Count;i++)
		{
			levelClone.poiNameList.Add(this.poiNameList[i]);
		}
		
		return levelClone;
	}

}
