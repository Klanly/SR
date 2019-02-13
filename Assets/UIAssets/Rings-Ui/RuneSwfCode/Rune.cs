using UnityEngine;
using System.Collections;

public class Rune : MonoBehaviour
{
	public string runeID = null;
	public string runeType = null;
	public string runeName = null;
	public int runeSkullLevel = 0;
	public float runeStats = 0;
	public int runeDamage = 0;
	public float runeLifeDamage = 0;
	public float runeLife =0;
	public int runeIndex =0;
	public string uid = "";
	public bool IsNew = false;
	public bool IsSuggested = false;
	
	public int sellPrice = 0;
	public int runeWards = 0;
	
	public int fire = 0;
	public int lightning =0;
	public int water = 0;
	public int earth =0;
	
	public int daze = 0;
	public int drain = 0;
	public int entangle= 0;
	public int greed= 0;
	public int ignite = 0;
	
	public int damag = 0;
	public int rounds = 0;

	public float x;
	public float y;
	
	public string runeImageURL = null;
	
	public GameObject ownedMarker;
	public bool ownedMarkerOn = false;
	
	public int gPrice = -1;
	public int dPrice = -1;
	public bool market = false;
	
	public string specialRuneValue = "none";
	public int listPosition ;
	
	public string runeSpell = "";
	public string RuneImage;
	public bool isDragAble;
	
	public string loadRuneImage() 
	{
		// Load Rune Image
		if(runeName == null)
		{
			return "image 59";
		}
		else
		{
			return runeName;
		}
	}
	
	public void addOwnedMarker()
	{
		if(ownedMarker == null)
			ownedMarker = new GameObject();
		//ownedMarker.transform.position.x ;
		ownedMarker.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		ownedMarkerOn = true;
	}
	
	public void removeOwnedMarker() 
	{
		ownedMarker = null;
		ownedMarkerOn = false;
		
	}
	
	public void SetItemFromInventory(InventorySystem.InventoryItem runeObject)
	{
		// Save data from json to Rune value;
		if(runeObject.ItemType() == InventorySystem.InventoryItem.Type.kRune)
			this.runeName = runeObject.ItemName();
//		else if (runeObject.ItemType() == InventorySystem.InventoryItem.Type.kRune)
//			this.runeName = runeObject.ItemName();
		
		if(runeObject.skullLevel!=null)
			this.runeSkullLevel = runeObject.skullLevel;
		

		// For Rune Item
		if((runeObject as InventorySystem.ItemRune)!=null)
		{
		if( (runeObject as InventorySystem.ItemRune).buff.description == "GREED")
			this.greed = 1;
		if( (runeObject as InventorySystem.ItemRune).buff.description == "DAZE")
			this.daze = 1;
		if( (runeObject as InventorySystem.ItemRune).buff.description == "LEECH SEED")
			this.entangle = 1;
		if( (runeObject as InventorySystem.ItemRune).buff.description == "DRAIN")
			this.drain = 1;
		if( (runeObject as InventorySystem.ItemRune).buff.description == "IGNITE")
			this.ignite = 1;
			
		if( (runeObject as InventorySystem.ItemRune).buff.duration != null)
			this.rounds = (runeObject as InventorySystem.ItemRune).buff.duration;
		
		if( (runeObject as InventorySystem.ItemRune).buff.duration != null)
			this.damag = (runeObject as InventorySystem.ItemRune).buff.duration;
			
	
			
		if( (runeObject as InventorySystem.ItemRune).dCost > 0)
			this.dPrice = (runeObject as InventorySystem.ItemRune).dCost;
		if( (runeObject as InventorySystem.ItemRune).gCost > 0)
			this.gPrice = (runeObject as InventorySystem.ItemRune).gCost;
		if( (runeObject as InventorySystem.ItemRune).sellCost > 0)
			this.sellPrice = (runeObject as InventorySystem.ItemRune).sellCost;
		if( (runeObject as InventorySystem.ItemRune).IndexInList > 0)
			this.runeIndex = (runeObject as InventorySystem.ItemRune).IndexInList;
		if( (runeObject as InventorySystem.ItemRune).uid!=null)
			this.uid = (runeObject as InventorySystem.ItemRune).uid;
		
		if( (runeObject as InventorySystem.ItemRune).isNew)
			this.IsNew = (runeObject as InventorySystem.ItemRune).isNew;
		
		if( (runeObject as InventorySystem.ItemRune).isSuggested )
			this.IsSuggested = (runeObject as InventorySystem.ItemRune).isSuggested;
		}
	}
//	public Ring duplicate()
//	{
//		Ring tempRing = new Ring();
//		
//		tempRing.ringType = this.ringType;
//		tempRing.ringName = this.ringName;
//		tempRing.ringSkullLevel = this.ringSkullLevel;
//		tempRing.ringStats = this.ringStats;
//		
//		tempRing.fire = this.fire;
//		tempRing.lightning = this.lightning;
//		tempRing.water = this.water;
//		tempRing.earth = this.earth;
//		tempRing.ringWards = this.ringWards;
//		tempRing.IsNew = this.IsNew;
//		tempRing.IsSuggested = this.IsSuggested;
//		//tempRing.ringLifeDamage = this.ringLifeDamage;
//		tempRing.ringLife = this.ringLife;
//		
//		tempRing.ringDamage = this.ringDamage;
//		tempRing.dPrice = this.dPrice;
//		tempRing.gPrice = this.gPrice;
//		tempRing.sellPrice = this.sellPrice;
//		tempRing.ringIndex = this.ringIndex;
//		tempRing.uid = this.uid;
//		
//		tempRing.loadRingImage();
//		
//		return tempRing;
//	}
}
