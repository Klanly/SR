using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InventorySystem
{
	[System.Serializable]
	public class Bag : ItemContainer, IComparable
	{
		public const string BAG_DUST_COST = "dCost";
		public const string BAG_SOUL_MAX = "maxSoul";
		
		public List<InventoryItem> bagItems;
		
		public float soulCapacity;

		// To allow sorting by bag level
		int IComparable.CompareTo(object aBag)
		{
			Bag otherBag = (Bag) aBag;
			if(this.level > otherBag.level)
				return 1;
			else if(this.level < otherBag.level)
				return -1;
			else
				return 0;
		}
		
		public Bag(int capacity, int skullLevel, string id, int level, float dCost, float uTime, string bagDescription, float soulCapacity)
		{
			this.id = id;
			this.level = level;
			this.skullLevel = skullLevel;
			this.dCost = dCost;
			this.uTime = uTime;
			this.capacity = capacity;
			this.description=bagDescription;
			this.soulCapacity = soulCapacity;
			bagItems = new List<InventoryItem>(this.capacity);
		}
		
		public Bag Clone()
		{
			return new Bag(this.capacity, this.skullLevel, this.id, this.level, this.dCost, this.uTime,this.description, this.soulCapacity);
		}
		
		public bool Add(InventoryItem anItem)
		{
			if(BagCount() < this.capacity)
			{
				if(anItem.uid == null || anItem.uid.Equals(""))
					anItem.uid = Nonce.GetUniqueID();
				
				anItem.isSuggested = false;
				bagItems.Add(anItem);
				
				return true;
			}
			return false;
		}

		public int BagCount()
		{
			return bagItems.Count;
		}
		
		public bool Remove(InventoryItem anItem)
		{
			bool success = bagItems.Remove(anItem);
			bagItems.TrimExcess();
			return success;
		}
		
		public void ClearBag()
		{
			bagItems.Clear();
		}
		
		public void RemoveItemAtIndex(int index)
		{
			bagItems.RemoveAt(index);
		}
		
		public List<ItemRing> GetAllRings()
		{
			List<ItemRing> ringsList = new List<ItemRing>();

			InventoryItem anItem = null;
			int count = this.bagItems.Count;
			ItemRing isRing = null;
			for(int i = 0;i<count;i++)
			{
				anItem = this.bagItems[i];
				isRing = anItem as ItemRing;
				
				if(isRing != null)
				{
					ringsList.Add(isRing);
				}
			}
			return ringsList;
		}
		
		public List<ItemRune> GetAllRunes()
		{
			List<ItemRune> runesList = new List<ItemRune>();
			
			InventoryItem anItem = null;
			int count = this.bagItems.Count;
			ItemRune isRune = null;
			for(int i = 0;i<count;i++)
			{
				anItem = this.bagItems[i];
				
				isRune = anItem as ItemRune;
				if(isRune != null)
				{
					runesList.Add(isRune);
				}
			}
			
			return runesList;
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary dictionary = base.ToDictionary();
			dictionary[BAG_DUST_COST] = this.dCost;
			dictionary[BAG_SOUL_MAX] = this.soulCapacity;
			return dictionary;
		}
		
		public void ReplaceItemInBag(InventoryItem oldItem, InventoryItem newItem)
		{
			bagItems[bagItems.IndexOf(oldItem)] = newItem;
		}
		
		
		public override string ToString ()
		{
			return string.Format ("[Bag: id={0}, capacity={1}, level={2}, upgradeGemCost={3}, upgradeTime={4}]", id, capacity, level, dCost, uTime);
		}
		
		public bool HasRingWithTag(string ringTag)
		{
			foreach(InventoryItem anItem in this.bagItems)
			{
				if((anItem as ItemRing) == null)
					continue;
				
				switch(ringTag)
				{
				case "W":
					if((anItem as ItemRing).water > 0) return true;
					break;
				case "L":
					if((anItem as ItemRing).lightning > 0) return true;
					break;
				case "F":
					if((anItem as ItemRing).fire > 0) return true;
					break;
				case "E":
					if((anItem as ItemRing).earth > 0) return true;
					break;
					
				}
			}
			return false;
		}
		
		public bool HasRingWithTags(string[] ringTags)
		{
			foreach(string aTag in ringTags)
			{
				if(HasRingWithTag(aTag))
				{
					if(GameManager.PRINT_LOGS) Debug.Log("RETURNED TRUE :(");
					return true;
				}
			}
			return false;
		}
		
		public void MarkRingsAsNotNew()
		{
			MaskAsNotNew<ItemRing>();
		}
		
		public void MarkRunesAsNotNew()
		{
			MaskAsNotNew<ItemRune>();
		}
		
		private void MaskAsNotNew <T> () where T : InventoryItem
		{
			foreach(InventoryItem anItem in this.bagItems)
				if((anItem as T) != null)
					anItem.isNew = false;
		}
		
		public bool HasSuggestedRing()
		{
			return HasSuggestedItem<ItemRing>();
		}
		
		public bool HasSuggestedRune()
		{
			return HasSuggestedItem<ItemRune>();
		}
		
		private bool HasSuggestedItem <T> () where T : InventoryItem
		{
			foreach(InventoryItem anItem in this.bagItems)
			{
				if(anItem as T != null)
				{
					if((anItem as T).isSuggested)
						return true;
				}
			}
			return false;
		}
		
		public bool HasNewRing()
		{
			return HasNewItem<ItemRing>();
		}
		
		public bool HasNewRune()
		{
			return HasNewItem<ItemRune>();
		}
		
		public void SortNewRunesFirst()
		{
			SortNewFirst<ItemRune>();
		}
		
		public void SortNewRingsFirst()
		{
			SortNewFirst<ItemRing>();
		}
		
		private bool HasNewItem <T> () where T : InventoryItem
		{
			foreach(InventoryItem anItem in this.bagItems)
			{
				if(anItem as T != null)
				{
					if((anItem as T).isNew)
					{
						return true;
					}
						
				}
			}
			return false;
		}
		
		private void SortNewFirst <T> () where T : InventoryItem
		{
			List<InventoryItem> newItems = new List<InventoryItem>();
			List<InventoryItem> otherInventoryItems = new List<InventoryItem>();
			
			lock(bagItems)
			{
				foreach(InventoryItem anItem in this.bagItems)
				{
					if(anItem as T != null)
					{
						if((anItem as T).isNew)
							newItems.Add(anItem);
						else
							otherInventoryItems.Add(anItem);
					}
					else
						otherInventoryItems.Add(anItem);
				}
			
			
				foreach(InventoryItem anItem in otherInventoryItems)
					newItems.Add(anItem);
			}
			
			this.bagItems = newItems;
		}
		
		
	}
}