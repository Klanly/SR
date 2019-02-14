using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	public class PotionBelt : ItemContainer, IComparable
	{
		public List<ItemPotion> potions;
		public int healAmount;
		
		int IComparable.CompareTo(object aBelt) //For Sorting
		{
			PotionBelt otherBelt = (PotionBelt) aBelt;
			if(this.level > otherBelt.level)
				return 1;
			else if(this.level < otherBelt.level)
				return -1;
			else
				return 0;
		}
		
		public PotionBelt(int capacity,int skullLevel, string id, int level, float dCost, float upgradeTime,string potionDescription,int healAmount)
		{
			this.id = id;
			this.level = level;
			this.skullLevel = skullLevel;
			this.dCost = dCost;
			this.uTime = upgradeTime;
			this.capacity = capacity;
			this.description=potionDescription;
			this.healAmount = healAmount;

			potions = new List<ItemPotion>(this.capacity);
		}
		
		public int Count()
		{
			return potions.Count;
		}
		
		public bool AddPotion(ItemPotion aPotion)
		{
			if(potions.Count < this.capacity)
			{
				potions.Add(aPotion);
				return true;
			}
			return false;
		}
		
		public bool RemovePotion(InventorySystem.InventoryItem.Type aPotion)
		{
			if(aPotion == InventoryItem.Type.kHealthPotion)
			{
				return RemoveHealthPotion();
			}
			
			return RemoveWardPotion();
		}
		
		private bool RemoveHealthPotion()
		{
			for(int i = 0;i<potions.Count;i++)
			{
				if(potions[i].ItemType() == InventoryItem.Type.kHealthPotion)
				{
					if(potions.Remove(potions[i]))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		private bool RemoveWardPotion()
		{
			for(int i = 0;i<potions.Count;i++)
			{
				if(potions[i].ItemType() == InventoryItem.Type.kWardPotion)
				{
					if(potions.Remove(potions[i]))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary dictionary = base.ToDictionary();
			dictionary["dCost"] = this.dCost;
			dictionary["HealAmount"] = this.healAmount;

			return dictionary;
		}
		
		public override string ToString()
		{
			return string.Format("[PotionBelt: id={0}, level={1}, upgradeGemCost={2}, upgradeTime={3}, capacity={4}]", id, level, dCost, uTime, capacity);
		}
			
		public string ToJson()
		{
			string jsonString = "[";
			
			for(int i = 0;i<potions.Count;i++)
			{
				jsonString += Helpers.AddQuotes(potions[i].id);
				
				if(i != potions.Count -1)
				{
					jsonString += ",";
				}
			}
			
			jsonString += "]";
			
			return jsonString;
		}
		
		public PotionBelt Clone()
		{
			return new PotionBelt(this.capacity, this.skullLevel, this.id, this.level, this.dCost, this.uTime,this.description,this.healAmount);
		}
	}
}