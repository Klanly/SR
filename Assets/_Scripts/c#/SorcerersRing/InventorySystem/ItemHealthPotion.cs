using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	public class ItemHealthPotion : ItemPotion 
	{
		private static IDictionary healthPotionDictionary = new Dictionary<string, object>();
		public ItemHealthPotion()
		{
			this.SetItemType(InventoryItem.Type.kHealthPotion);
		}
	
		protected override void SetItemType(Type aType)
		{
			base.SetItemType(aType);
		}
		
		public string name
		{
			set;get;
		}
		
		public int healAmount
		{
			set;get;
		}
		
		public string ToJson()
		{
			string jsonString = "{";
			jsonString += "\"Type\" : " + Helpers.AddQuotes("Health Potion") + ",";
			jsonString += "\"id\" : " + Helpers.AddQuotes(this.id);
			jsonString += "}";
			
			return jsonString;
		}
		
		public override IDictionary ToDictionary()
		{
			healthPotionDictionary["Type"] = "Health Potion";
			healthPotionDictionary["id"] = this.id;
			healthPotionDictionary["dCost"] = this.dCost;
			healthPotionDictionary["gCost"] = this.gCost;
			return healthPotionDictionary;
		}
		
	}
}
