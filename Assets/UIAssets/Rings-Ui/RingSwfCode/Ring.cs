using UnityEngine;
using System.Collections;

public class RingMufti : MonoBehaviour
{
	public string ringID = null;
	public string ringType = null;
	public string ringName = null;
	public int ringSkullLevel = 0;
	public float ringStats = 0;
	public int ringDamage = 0;
	public float ringLifeDamage = 0;
	public float ringLife =0;
	public int ringIndex =0;
	public string uid = "";
	public bool IsNew = false;
	public bool IsSuggested = false;
	
	public int sellPrice = 0;
	public int ringWards = 0;
	
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
	
	public string ringImageURL = null;
	
	public GameObject ownedMarker;
	public bool ownedMarkerOn = false;
	
	public int gPrice = -1;
	public int dPrice = -1;
	public bool market = false;
	
	public string specialRingValue = "none";
	public int listPosition ;
	
	public string ringSpell = "";
	public string RingImage;
	public bool isDragAble;
	
	public string loadRingImage() 
	{
		// Load Ring Image
		if(ringName == null)
		{
			return "image 59";
		}
		else
		{
			return ringName;
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
	
	
	public void ClearAllData()
	{
		ringName = string.Empty;
		RingImage = string.Empty; 
		ringImageURL = string.Empty;
		ringSkullLevel = 0;
		fire = 0;
		water = 0;
		lightning = 0;
		earth = 0;
		ringWards = 0;
		ringDamage = 0;
		dPrice = 0;
		gPrice = 0;
		sellPrice = 0;
		uid = string.Empty;
		greed = 0;
		daze = 0;
	}
		
	public void SetItemFromInventory(InventorySystem.InventoryItem ringObject)
	{
		// Save data from json to Ring value;
		if(ringObject.ItemType() == InventorySystem.InventoryItem.Type.kRing)
			this.ringName = ringObject.ItemName();
		else if (ringObject.ItemType() == InventorySystem.InventoryItem.Type.kRune)
			this.ringName = ringObject.ItemName();
		
		if(ringObject.skullLevel!=null)
			this.ringSkullLevel = ringObject.skullLevel;
		
		// For RIng Item
		if((ringObject as InventorySystem.ItemRing)!=null)
		{
		if( (ringObject as InventorySystem.ItemRing).fire > 0)
			this.fire = (ringObject as InventorySystem.ItemRing).fire;
		if( (ringObject as InventorySystem.ItemRing).lightning > 0)
			this.lightning = (ringObject as InventorySystem.ItemRing).lightning;
		if( (ringObject as InventorySystem.ItemRing).water > 0)
			this.water = (ringObject as InventorySystem.ItemRing).water;
		if( (ringObject as InventorySystem.ItemRing).earth > 0)
			this.earth = (ringObject as InventorySystem.ItemRing).earth;
		if( (ringObject as InventorySystem.ItemRing).wards > 0)
			this.ringWards = (ringObject as InventorySystem.ItemRing).wards;
		if( (ringObject as InventorySystem.ItemRing).damage > 0)
			this.ringDamage = (ringObject as InventorySystem.ItemRing).damage;
		if( (ringObject as InventorySystem.ItemRing).dCost > 0)
			this.dPrice = (ringObject as InventorySystem.ItemRing).dCost;
		if( (ringObject as InventorySystem.ItemRing).gCost > 0)
			this.gPrice = (ringObject as InventorySystem.ItemRing).gCost;
		if( (ringObject as InventorySystem.ItemRing).sellCost > 0)
			this.sellPrice = (ringObject as InventorySystem.ItemRing).sellCost;
		if( (ringObject as InventorySystem.ItemRing).IndexInList > 0)
			this.ringIndex = (ringObject as InventorySystem.ItemRing).IndexInList;
		if( (ringObject as InventorySystem.ItemRing).uid!=null)
			this.uid = (ringObject as InventorySystem.ItemRing).uid;
		
		if( (ringObject as InventorySystem.ItemRing).isNew)
			this.IsNew = (ringObject as InventorySystem.ItemRing).isNew;
		
		if( (ringObject as InventorySystem.ItemRing).isSuggested )
			this.IsSuggested = (ringObject as InventorySystem.ItemRing).isSuggested;
		}
		
		// For Rune Item
		if((ringObject as InventorySystem.ItemRune)!=null)
		{
		if( (ringObject as InventorySystem.ItemRune).buff.description == "GREED")
			this.greed = 1;
		if( (ringObject as InventorySystem.ItemRune).buff.description == "DAZE")
			this.daze = 1;
		if( (ringObject as InventorySystem.ItemRune).buff.description == "LEECH SEED")
			this.entangle = 1;
		if( (ringObject as InventorySystem.ItemRune).buff.description == "DRAIN")
			this.drain = 1;
		if( (ringObject as InventorySystem.ItemRune).buff.description == "IGNITE")
			this.ignite = 1;
			
		if( (ringObject as InventorySystem.ItemRune).buff.duration != null)
			this.rounds = (ringObject as InventorySystem.ItemRune).buff.duration;
		
		if( (ringObject as InventorySystem.ItemRune).buff.duration != null)
			this.damag = (ringObject as InventorySystem.ItemRune).buff.duration;
			
	
			
		if( (ringObject as InventorySystem.ItemRune).dCost > 0)
			this.dPrice = (ringObject as InventorySystem.ItemRune).dCost;
		if( (ringObject as InventorySystem.ItemRune).gCost > 0)
			this.gPrice = (ringObject as InventorySystem.ItemRune).gCost;
		if( (ringObject as InventorySystem.ItemRune).sellCost > 0)
			this.sellPrice = (ringObject as InventorySystem.ItemRune).sellCost;
		if( (ringObject as InventorySystem.ItemRune).IndexInList > 0)
			this.ringIndex = (ringObject as InventorySystem.ItemRune).IndexInList;
		if( (ringObject as InventorySystem.ItemRune).uid!=null)
			this.uid = (ringObject as InventorySystem.ItemRune).uid;
		
		if( (ringObject as InventorySystem.ItemRune).isNew)
			this.IsNew = (ringObject as InventorySystem.ItemRune).isNew;
		
		if( (ringObject as InventorySystem.ItemRune).isSuggested )
			this.IsSuggested = (ringObject as InventorySystem.ItemRune).isSuggested;
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
