using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	public class ItemWardPotion : ItemPotion 
	{
		private static IDictionary wardPotionDictionary = new Dictionary<string, object>();
		public ItemWardPotion()
		{
			this.SetItemType(InventoryItem.Type.kWardPotion);
		}
	
		protected override void SetItemType(Type aType)
		{
			base.SetItemType(aType);
		}
		
		public int healAmount
		{
			set;get;
		}
		
		public string ToJson()
		{
			string jsonString = "{";
			jsonString += "\"Type\" : " + Helpers.AddQuotes("Ward Potion") + ",";
			jsonString += "\"id\" : " + Helpers.AddQuotes(this.id);
			jsonString += "}";
			
			return jsonString;
		}
		
		public override IDictionary ToDictionary()
		{
			wardPotionDictionary["Type"] = "Ward Potion";
			wardPotionDictionary["id"] = this.id;
			wardPotionDictionary["gCost"] = this.gCost;
			wardPotionDictionary["dCost"] = this.dCost;
			return wardPotionDictionary;
		}
		
	}
}
