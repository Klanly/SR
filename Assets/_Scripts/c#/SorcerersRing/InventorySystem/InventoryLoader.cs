using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

namespace InventorySystem
{
	public class InventoryLoader
		{
		
		public const string TAG_BAG_INVENTORY = "Bag Items";
		
		public const string TAG_EQUIP_INVENTORY = "Equipped Rings";
		
		public const string KEY_INVENTORY_TYPE = "Type";
		
		public const string KEY_RING_NAME = "RingName";
		
		public const string KEY_RING_TYPE = "RingType";
		
		public const string KEY_INVENTORY_STATS = "Stats";
		
		public const string KEY_INVENTORY_SKULL_LEVEL = "SkullLevel";
		
		public const string KEY_MINIMUM_SKULL_LEVEL = "Min Skull Level";
		
		public const string KEY_INVENTORY_FIRE = "Fire";
		
		public const string KEY_INVENTORY_WATER = "Water";
		
		public const string KEY_INVENTORY_UID = "uid";
		
		public const string KEY_INVENTORY_EARTH = "Earth";
		
		public const string KEY_INVENTORY_LIGHTNING = "Lightning";
		
		public const string KEY_INVENTORY_WARDS = "Wards";
		
		public const string KEY_INVENTORY_LIFE = "Life";
		
		public const string KEY_INVENTORY_DAMAGE = "Damage";
		
		public const string KEY_RING_TYPE_DAMAGE = "DAMAGE";
		
		public const string KEY_RING_TYPE_LIFE = "LIFE";
		
		public const string KEY_RING_TYPE_ELEMENT = "ELEMENT";
		
		public const string KEY_RING_TYPE_WARD = "WARD";
		
		public const string KEY_INVENTORY_MARKER = "Marker";
		
		public const string TYPE_OPTION_RING = "Ring";
		
		public const string TYPE_OPTION_RUNE = "Rune";
		
		public const string TAG_DUST_COST = "dCost";
		
		public const string TAG_TAG = "Tag";
		
		public const string TAG_GEMS_COST = "gCost";
		
		public const string TAG_GEMS = "Gems";
		
		public const string TAG_SOULS = "Souls";
		
		public const string TAG_BAG = "BAG";
		
		public const string TAG_POTION_BELT = "Potion Belt";
		
		public const string TAG_TRANSMUTATION_CUBE = "T_CUBE";
		
		public const string TAG_KEY_RING = "KEY_RING";
		
		public const string TAG_POTIONS = "Potions";
		
		public const string	TAG_RUNES = "Staff";
		
		public const string	TAG_KEY_COUNT = "key_count";
			
		public static Inventory LoadInventory(IDictionary jsonData)
		{
			Inventory inventory = new Inventory();
			
			inventory.bag = GameManager._dataBank.GetBagForID(jsonData[TAG_BAG].ToString());
			inventory.gems = Convert.ToInt32(jsonData[TAG_GEMS].ToString());
			inventory.souls = Convert.ToInt32(jsonData[TAG_SOULS].ToString());
			inventory.potionBelt = GameManager._dataBank.GetBeltForID(jsonData[TAG_POTION_BELT].ToString());
			inventory.transmutationCube = GameManager._dataBank.GetTransmutationCubeForID(jsonData[TAG_TRANSMUTATION_CUBE].ToString());
			inventory.keyRing = GameManager._dataBank.GetKeyRingForID(jsonData[TAG_KEY_RING].ToString());
			inventory.keyRing.keyCount = Convert.ToInt32(jsonData[TAG_KEY_COUNT].ToString());
			
			IList runesList = jsonData[TAG_RUNES] as IList; 
			for(int i = 0;i<runesList.Count;i++)
			{
				IDictionary runeInfoDictionary = runesList[i] as IDictionary;
				inventory.AddRuneToStaff(GetRuneObjectFromJsonData(runeInfoDictionary)); 
			}
				
			IList potionsList = jsonData[TAG_POTIONS] as IList;
			for(int i = 0;i<potionsList.Count;i++)
			{
				inventory.potionBelt.AddPotion(GameManager._dataBank.GetPotionForPotionID(potionsList[i] as string));
			}
			
			IList bagInventory = jsonData[TAG_BAG_INVENTORY] as IList;
			
			IDictionary anInventoryItem;
			
			for(int i = 0; i<bagInventory.Count; i++)
			{
				anInventoryItem = bagInventory[i] as IDictionary;
				
				if(anInventoryItem[KEY_INVENTORY_TYPE].ToString().Equals(TYPE_OPTION_RING))
				{
					inventory.bag.Add(GetRingObjectFromJsonData(anInventoryItem));
				}
				else if(anInventoryItem[KEY_INVENTORY_TYPE].ToString().Equals(TYPE_OPTION_RUNE))
				{
					ItemRune runeToAdd = new ItemRune(anInventoryItem["RuneName"].ToString(), Convert.ToInt32(anInventoryItem["SkullLevel"].ToString()));
					runeToAdd.uid = anInventoryItem["uid"].ToString();
					inventory.bag.Add(runeToAdd);
				}
			}
			
			IList equippedInventory = jsonData[TAG_EQUIP_INVENTORY] as IList;
			for(int i = 0; i<equippedInventory.Count; i++)
			{
				anInventoryItem = equippedInventory[i] as IDictionary;
				inventory.AddToRingBag(GetRingObjectFromJsonData(anInventoryItem));
			}
			
			inventory.upgradesDictionary = jsonData["Upgrades"] as IDictionary;

			return inventory;
		}
		
		public static ItemRune GetRuneObjectFromJsonData(IDictionary jsonData)
		{
			ItemRune runeToAdd = new ItemRune(jsonData["RuneName"].ToString(), System.Convert.ToInt32(jsonData["SkullLevel"].ToString()));
			if(jsonData.Contains(KEY_INVENTORY_UID))
				runeToAdd.uid = jsonData["uid"].ToString();
			
			return runeToAdd;
		}
		
		
		public static ItemRing GetRingObjectFromJsonData(IDictionary jsonData)
		{
			ItemRing ring = new ItemRing();
			
			string ringName = jsonData[KEY_RING_NAME].ToString();
			
			ring.SetItemName(ringName);
			
			ring.id = ringName;
			
			ring.skullLevel = Convert.ToInt32(jsonData[KEY_INVENTORY_SKULL_LEVEL].ToString());
			
			ring.fire = Convert.ToInt32(jsonData[KEY_INVENTORY_FIRE].ToString());
			
			ring.lightning = Convert.ToInt32(jsonData[KEY_INVENTORY_LIGHTNING].ToString());
			
			ring.dCost = Convert.ToInt32(jsonData[TAG_DUST_COST].ToString());
			
			ring.gCost = Convert.ToInt32(jsonData[TAG_GEMS_COST].ToString());
			
			ring.tag = jsonData[TAG_TAG].ToString();
			
			ring.water = Convert.ToInt32(jsonData[KEY_INVENTORY_WATER].ToString());
			
			if(jsonData.Contains(KEY_INVENTORY_UID))
				ring.uid = jsonData[KEY_INVENTORY_UID].ToString();
			
			ring.earth = Convert.ToInt32(jsonData[KEY_INVENTORY_EARTH].ToString());
			
			ring.wards = Convert.ToInt32(jsonData[KEY_INVENTORY_WARDS].ToString());
			
			ring.life = Convert.ToInt32(jsonData[KEY_INVENTORY_LIFE].ToString());
			
			ring.damage = Convert.ToInt32(jsonData[KEY_INVENTORY_DAMAGE].ToString());
		
			ring.stats = Convert.ToInt32(jsonData[KEY_INVENTORY_STATS].ToString());
			
			return ring;
		}
	}
}