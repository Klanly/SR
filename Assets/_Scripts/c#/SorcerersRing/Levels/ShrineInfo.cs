using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShrineInfo {
	
	private const string SHRINE_LEVEL = "ShrineLevels";
	private const string SHRINE_MAXPOINTS = "ShrinePoints";
	private const string SHRINE_SKULLMOD = "SkullMod";
	private const string SHRINE_POTIONS = "Potions";
	private const string SHRINE_KEYS = "Keys";
	private const string SHRINE_CHARMS = "Charms";
	private const string SHRINE_RING = "Ring";
	private const string SHRINE_RUNE = "Rune";
	private const string SHRINE_GEMS = "Gems";
	
	
	public int shrineLevel
	{
		get;private set;
	}
	public int maxShrinePoints
	{
		get;private set;
	}
	public int skullMode
	{
		get;private set;
	}
	public int potionsPercentage
	{
		get; private set;
	}
	public int keysPercentage
	{
		get;private set;
	}
	public int charmsPercentage
	{
		get;private set;
	}
	public int ringsPercentage
	{
		get;private set;
	}
	public int runesPercentage
	{
		get;private set;
	}
	public int gemsPercentage
	{
		get;private set;
	}
	
	public static Dictionary<int,ShrineInfo> LoadShrineInfo(IList shrineData)
	{
		Dictionary<int,ShrineInfo> shrineDictionary = new Dictionary<int, ShrineInfo>();
		IDictionary ShrineKeyValPair;
		for(int i = 0; i<shrineData.Count;i++)
		{
			ShrineInfo aShrineInfo = new ShrineInfo();
			ShrineKeyValPair = shrineData[i] as IDictionary;
			aShrineInfo.shrineLevel = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_LEVEL]);
			aShrineInfo.maxShrinePoints = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_MAXPOINTS]);
			aShrineInfo.skullMode = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_SKULLMOD]);
			aShrineInfo.potionsPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_POTIONS]);
			aShrineInfo.keysPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_KEYS]);
			aShrineInfo.charmsPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_CHARMS]);
			aShrineInfo.ringsPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_RING]);
			aShrineInfo.runesPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_RUNE]);
			aShrineInfo.gemsPercentage = System.Convert.ToInt32(ShrineKeyValPair[SHRINE_GEMS]);
			
			shrineDictionary.Add(aShrineInfo.shrineLevel,aShrineInfo);
		}

//		Debug.Log(MiniJSON.Json.Serialize(shrineDictionary));
		return shrineDictionary;
	}

}
