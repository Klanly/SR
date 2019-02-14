using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneModel
{
	
	public ZoneModel()
	{
	}
	
	public string zoneId
	{
		get;set;
	}
	
	public List<string> enemyList
	{
		get;set;
	}
	
//	public int skullMod
//	{
//		get;set;
//	}
	
	public int potionsPercentage
	{
		get;set;
	}
	
	public int keysPercentage
	{
		get;set;
	}
	
//	public int ConsumablesPercentage
//	{
//		get;set;
//	}
	
	public int CharmsPercentage
	{
		get;set;
	}
	
	public ZoneModel Clone()
	{
		ZoneModel zoneModelClone=new ZoneModel();
		zoneModelClone.enemyList=new List<string>();
		
		zoneModelClone.zoneId=this.zoneId;
		//zoneModelClone.skullMod=this.skullMod;
		zoneModelClone.keysPercentage=this.keysPercentage;
		zoneModelClone.potionsPercentage=this.potionsPercentage;
	//	zoneModelClone.ConsumablesPercentage=this.ConsumablesPercentage;
		zoneModelClone.CharmsPercentage=this.CharmsPercentage;
		
		for(int i=0;i<this.enemyList.Count;i++)
		{
			zoneModelClone.enemyList.Add(this.enemyList[i]);
		}
		
		return zoneModelClone;
	}
	
}
