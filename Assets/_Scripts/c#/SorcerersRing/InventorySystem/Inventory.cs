using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InventorySystem
{
	public partial class Inventory {
		
		public const int startIndexForBag = 200;
		public const int startIndexForEquippedItems = 50;
		public const int startIndexForSuggested = 0;
		/// <summary>
		/// 
		/// Type tags :
		/// 			Ring ->> Ring
		/// 			Rune ->> Rune
		/// 			Health Potion ->> Health Potion
		/// </summary>
		public const int RING_BAG_SIZE = 6;
		
		private const int STAFF_RUNE_SIZE = 3;
		
		public const int SELL_FACTOR = 4;

		[Newtonsoft.Json.JsonIgnore]
		public string json
		{
			set
			{
				
			}
			get
			{
				int indexInInventory = startIndexForBag;
				string jsonString = "" 
					+ "{" 
						+ "\"Bag Items\" : "
							+ "[";
				
				for(int i = 0;i<bag.bagItems.Count;i++)
				{
					bag.bagItems[i].IndexInList = ++indexInInventory;
					if(bag.bagItems[i].ItemType() == InventoryItem.Type.kRing)
					{
						jsonString += MiniJSON.Json.Serialize(((ItemRing)bag.bagItems[i]).ToDictionary());
					}
					else if(bag.bagItems[i].ItemType() == InventoryItem.Type.kRune)
					{
						jsonString += MiniJSON.Json.Serialize(((ItemRune)bag.bagItems[i]).ToDictionary());
					}
					
					if(i != bag.bagItems.Count-1)
					{
						jsonString += ",";
					}
				}
				
				jsonString += "],";
				
				indexInInventory = startIndexForEquippedItems;
				jsonString += "\"Equipped Rings\" : "
							+ "[";
						
				for(int i = 0;i<equippedRings.Count;i++)
				{
					equippedRings[i].IndexInList = ++indexInInventory;
					jsonString += MiniJSON.Json.Serialize(equippedRings[i].ToDictionary());
					if(i != equippedRings.Count-1)
					{
						jsonString += ",";
					}
				}
				
				jsonString += "],";
				
				jsonString += "\"Staff\" : "
							+ "[";
						
				for(int i = 0;i<staffRunes.Count;i++)
				{
					staffRunes[i].IndexInList = ++indexInInventory;
					jsonString += MiniJSON.Json.Serialize(staffRunes[i].ToDictionary());
					
					if(i != staffRunes.Count-1)
					{
						jsonString += ",";
					}
				}
				
				jsonString += "],";
				
				
				jsonString += "\"Souls\" : " + this.souls + ",";
				jsonString += "\"Gems\" : " + this.gems + ",";
				jsonString += "\"BAG\" : " + Helpers.AddQuotes(this.bag.id) + ",";
				jsonString += "\"KEY_RING\" : " + Helpers.AddQuotes(this.keyRing.id) + ",";
				jsonString += "\"Potion Belt\" : " + Helpers.AddQuotes(this.potionBelt.id) + ",";
				jsonString += "\"T_CUBE\" : " + Helpers.AddQuotes(this.transmutationCube.id) + ",";
				jsonString += "\"Upgrades\" : " + MiniJSON.Json.Serialize(this.LoadUpgradesDictionary(this.upgradesDictionary)) + ",";
				jsonString += "\"key_count\" : " +this.keyRing.keyCount + ",";
				jsonString += "\"Potions\" : " + this.potionBelt.ToJson();
				
				jsonString += "}";
	
				return jsonString;
			}
		}
		
		public List<ItemRing> equippedRings;
		
		public List<ItemRune> staffRunes;
		
		public PotionBelt potionBelt;
		
		public Bag bag;
		
		public KeyRing keyRing;
		
		public TransmutationCube transmutationCube;
		
		private int _gems;
		public int gems
		{
			set
			{
				_gems = value;
				
				if(GameManager.enableAnalytics)
					Analytics.logEvent(Analytics.CurrencyTrackType.TrackGems, _gems);
			}
			get
			{
				return _gems;
			}
		}
		
		private float _souls;
		public float souls
		{
			get
			{
				return _souls;
			}
			set
			{
				_souls = value;
				
				if(_souls > this.bag.soulCapacity)
					_souls = this.bag.soulCapacity;
				
				if(GameManager.enableAnalytics)
					Analytics.logEvent(Analytics.CurrencyTrackType.TrackSouls, (int)_souls);
			}
		}

		public Inventory()
		{
			equippedRings = new List<ItemRing>();
			staffRunes = new List<ItemRune>();
		}
		
		
		public int TotalItemCount() {
			Dictionary<string, InventoryItem> dictionary = new Dictionary<string, InventoryItem>();

			equippedRings.ForEach((item) => { if(!dictionary.ContainsKey(item.uid))dictionary.Add(item.uid, item);});
			staffRunes.ForEach((item) => { if(!dictionary.ContainsKey(item.uid))dictionary.Add(item.uid, item);});
			bag.bagItems.ForEach((item) => { if(!dictionary.ContainsKey(item.uid))dictionary.Add(item.uid, item);});
			return dictionary.Count;
		}
		
		public bool AddRuneToStaff(ItemRune aRune)
		{
			if(StaffRunesCount() < STAFF_RUNE_SIZE)
			{
				if(aRune.uid == null || aRune.uid.Equals(""))
					aRune.uid = Nonce.GetUniqueID();
				
				staffRunes.Add(aRune);
				
				return true;
			}
			
			return false;
		}
		
		public bool AddRuneToStaff(ItemRune aRune, bool autopass)
		{
			if(StaffRunesCount() < STAFF_RUNE_SIZE)
			{
				if(aRune.uid == null || aRune.uid.Equals(""))
					aRune.uid = Nonce.GetUniqueID();
				
				staffRunes.Add(aRune);
				
				return true;
			}
			
			if(autopass)
				return this.bag.Add(aRune);
			
			return false;
		}
		
		public void ReplaceRuneInStaff(ItemRune oldRune, ItemRune newRune)
		{
			staffRunes[staffRunes.IndexOf(oldRune)] = newRune;
		}
		
		public bool RemoveRuneFromStaff(ItemRune aRune)
		{
			return staffRunes.Remove(aRune);
		}
		
		public int StaffRunesCount()
		{
			return staffRunes.Count;
		}
		
		public bool AddToRingBag(ItemRing anItem)
		{
			if(EquippedRingsCount() < RING_BAG_SIZE)
			{
				equippedRings.Add(anItem);
				
				return true;
			}
			return false;
		}
		
		public InventoryItem GetBagItemForUID(string uid)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("UID TO GET --->>>" + uid);
			
			int count = bag.bagItems.Count;
			InventoryItem anItem = null;
			
			for(int i = 0;i<count;i++)
			{
				anItem = bag.bagItems[i];
				if(anItem.uid.Equals(uid))
				{
					if(GameManager.PRINT_LOGS) Debug.Log("if(anItem.ItemType() == )" + anItem.ItemType());
					return anItem;
				}
			}
			
			return null;
		}
		
		
		
		/// <summary>
		/// If autopass is true, ring will be added to bag if there was no space in the equipped-rings slots. If false, same as AddToRingBag(ItemRing anItem) method
		/// </summary>
		public bool AddToRingBag(ItemRing anItem, bool autopass)
		{
			if(EquippedRingsCount() < RING_BAG_SIZE)
			{
				equippedRings.Add(anItem);
				
				return true;
			}
			if(autopass)
				return this.bag.Add(anItem);
			
			return false;
		}
		
		/// <summary>
		/// Replaces the first occurence of an inventory item with anItem.
		/// </summary>
		public void ReplaceRingInRingBag(ItemRing oldRing, ItemRing newRing)
		{
			equippedRings[equippedRings.IndexOf(oldRing)] = newRing;
		}
		
		
		/// <summary>
		/// Removes the first occurence of an inventory item with anItem.
		/// </summary>
		
		public bool RemoveFromRingBag(ItemRing aRing)
		{
			bool success = equippedRings.Remove(aRing);
			equippedRings.TrimExcess();
			return success;
			
		}
		
		public bool RemoveRingItemAtIndex(int index)
		{
			if(equippedRings.Count==0)
			{
				return false;
			}
			
			if(index >= equippedRings.Count)
			{
				return false;
			}
			
			equippedRings.RemoveAt(index);
			
			return true;
		}
		
		public int EquippedRingsCount()
		{
			return equippedRings.Count;
		}
		
		/*
		 * public override string ToString()
		{
			string stringVal =  "EQUIPPED RINGS \n";
			
			ItemRing item = null;
			int count = equippedRings.Count;
			for(int i = 0;i<count;i++)
			{
				item = equippedRings[i];
				
				stringVal += item.ToString();
				stringVal += "\n";
			}
			stringVal += "\n";
			
			stringVal += " BAG ITEMS \n";
			
			count = bag.bagItems.Count;
			for(int i = 0;i<count;i++)
			{
				item = bag.bagItems[i];
				stringVal += item.ToString();
				stringVal += "\n";
			}
				
			stringVal += "\n";
			
			return stringVal;
		}
		*/
		
		public bool HasRingInBagWithTag(string ringTag)
		{
			return this.bag.HasRingWithTag(ringTag);
		}
		
		public bool HasRingInBagWithTags(string [] ringTags)
		{
			foreach(string aTag in ringTags)
				Debug.Log("TAG TO LOOK FOR >> " + aTag);
			
			return this.bag.HasRingWithTags(ringTags);
		}
		
		public void MarkRingsAsNotNew(bool bagOnly = false)
		{
			if(!bagOnly)
				equippedRings.ForEach(ring => ring.isNew = false);
			this.bag.MarkRingsAsNotNew();
		}
		
		public void MarkRunesAsNotNew(bool bagOnly = false)
		{
			if(!bagOnly)
				staffRunes.ForEach(rune => rune.isNew = false);
			this.bag.MarkRunesAsNotNew();
		}
		
		public void SortNewRingsFirst()
		{
			this.bag.SortNewRingsFirst();
		}
		
		public void SortNewRunesFirst()
		{
			this.bag.SortNewRunesFirst();
		}
		
		public bool HasNewRingInBag()
		{
			return this.bag.HasNewRing();
		}
		
		public bool HasNewRuneInBag()
		{
			Debug.Log("HasNewRuneInBag >> " + this.bag.HasNewRune());
			return this.bag.HasNewRune();
		}
	}
}