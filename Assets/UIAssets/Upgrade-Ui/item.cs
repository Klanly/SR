using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {

	public string upgradeDescription;
	public string upgradeID;
	public int upgradeLevel=0;
	public int upgradeCost=0;
	public int upgradeTime=0;
	public int upgradeCapacity=0;
	public int upgradetloss=0;
	public int upgradeMaximumSoul=0;
	public int upgradeHealAmount=0;
	
	
	public string NextupgradeID;
	public int NextupgradeLevel=0;
	public int NextupgradeCost=0;
	public int NextupgradeTime;
	public int NextupgradeCapacity;
	public int Nextupgradetloss;
	public int NextupgradeMaximumSoul;
	public int NextupgradeHealAmount;
	
	public string MaxupgradeID;
	public int MaxupgradeLevel=0;
	public int MaxupgradeCost=0;
	public int MaxupgradeTime;
	public int MaxupgradeCapacity;
	public int Maxupgradetloss;
	public int MaxupgradeMaximumSoul;
	public int MaxupgradeHealAmount;

	/*
	public dfSprite UpgradeAreaItem;
	public dfLabel UpgradeTimeLable;
	public dfLabel UpgradeCostLable;
	public dfLabel ItemName;
	public dfSlicedSprite RedBar;
	public dfSlicedSprite BrownBar;
	
	public dfSprite itemIcon;
	*/
	public int boostCost;
	public string boostUid;
	
	public bool UpgradeInProgress;
	
	public void CurrentUpgrade(IDictionary Iitem)
	{
		this.upgradeID = Iitem["id"].ToString();
		
//		if(itemIcon != null)
//			itemIcon.SpriteName = this.upgradeID;
//		
		this.upgradeDescription = Iitem["Description"].ToString();
		this.upgradeLevel =  int.Parse(Iitem["uLevel"].ToString());
		this.upgradeTime =  int.Parse(Iitem["uTime"].ToString());
		this.upgradeCapacity =  int.Parse(Iitem["Capacity"].ToString());
		
		if(Iitem["tLoss"] !=null)
			this.upgradetloss = int.Parse(Iitem["tLoss"].ToString());
		if(Iitem["maxSoul"] != null)
			this.upgradeMaximumSoul = int.Parse(Iitem["maxSoul"].ToString());
		
		if(Iitem["gCost"] != null )
			this.upgradeCost = int.Parse(Iitem["gCost"].ToString());
		else
			this.upgradeCost = int.Parse(Iitem["dCost"].ToString());
	}
	
	public void NextUpgrade(IDictionary Iitem)
	{
		this.NextupgradeID = Iitem["id"].ToString();
		this.NextupgradeLevel =  int.Parse(Iitem["uLevel"].ToString());
		this.NextupgradeTime =  int.Parse(Iitem["uTime"].ToString());
		this.NextupgradeCapacity =  int.Parse(Iitem["Capacity"].ToString());
		
		if(Iitem["tLoss"] !=null)
			this.Nextupgradetloss = int.Parse(Iitem["tLoss"].ToString());
		if(Iitem["maxSoul"] != null)
			this.NextupgradeMaximumSoul = int.Parse(Iitem["maxSoul"].ToString());
		
		if(Iitem["gCost"] != null )
			this.NextupgradeCost = int.Parse(Iitem["gCost"].ToString());
		else
			this.NextupgradeCost = int.Parse(Iitem["dCost"].ToString());
		
	}
	
	public void MaxUpgrade(IDictionary Iitem)
	{
		this.MaxupgradeID = Iitem["id"].ToString();
		this.MaxupgradeLevel =  int.Parse(Iitem["uLevel"].ToString());
		this.MaxupgradeTime =  int.Parse(Iitem["uTime"].ToString());
		this.MaxupgradeCapacity =  int.Parse(Iitem["Capacity"].ToString());
		
		if(Iitem["tLoss"] !=null)
			this.Maxupgradetloss = int.Parse(Iitem["tLoss"].ToString());
		if(Iitem["maxSoul"] != null)
			this.MaxupgradeMaximumSoul = int.Parse(Iitem["maxSoul"].ToString());
		
		if(Iitem["gCost"] != null )
			this.MaxupgradeCost = int.Parse(Iitem["gCost"].ToString());
		else
			this.MaxupgradeCost = int.Parse(Iitem["dCost"].ToString());
	}
	
	public override string ToString ()
	{
		return string.Format ("[item] upgradeID >> {0}  upgradeTime >> {1}   NEXTupgradeTime >> {2}]", upgradeID, upgradeTime, NextupgradeTime);
	}

	public string GetItemName() {
		// Hardcoded - assuming upgradeID is in form - XXXX#
		return upgradeID;
//		return upgradeID.Substring(0, upgradeID.Length - 1);
	}
}
