using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	[System.Serializable]
	public class ItemRune : InventoryItem 
	{
		private IDictionary runeDictionary = new Dictionary<string, object>();
		
		public Buff buff;
		
		public enum Element {Fire, Lightning, Earth, Water, NotAvailable}
		
		public Element element
		{
			set;get;
		}
		
		public int stats;
		
		public string tag;

		public ItemRune()
		{
			_itemType = Type.kRune;
		}

		public ItemRune(string runeName, int skullLevel)
		{
//			Debug.Log("RUNE CONSTRUCTOR!!!");
			this.id = runeName;
			
			this._itemName = runeName;
			
			this.skullLevel = skullLevel;
			
			this.SetItemType(InventoryItem.Type.kRune);
			
			IDictionary runeDictionary = GameManager._dataBank.GetRuneForRuneID(this.id, this.skullLevel);
			
			this.buff = new Buff(runeDictionary["Spell"].ToString(), this.skullLevel);
			
			this.gCost = System.Convert.ToInt32(runeDictionary["gCost"].ToString());
			
			this.dCost = System.Convert.ToInt32(runeDictionary["dCost"].ToString());
			
			this.tag = runeDictionary["Tag"].ToString();
			
			this.stats = System.Convert.ToInt32(runeDictionary["Stats"].ToString());
			
			this.element = GetElementForElementString(runeDictionary["Element"].ToString());

			this.uid = Nonce.GetUniqueID();
		}
	
		public static Element GetElementForElementString(string elementString)
		{
			switch(elementString.ToLower())
			{
			case "fire":
				return Element.Fire;
			case "earth":
				return Element.Earth;
			case "lightning":
				return Element.Lightning;
			case "water":
				return Element.Water;
			default:
				return Element.NotAvailable;
			}
		}
		
		protected override void SetItemType(Type aType)
		{
			base.SetItemType(aType);
		}
//		public override string ToJson()
//		{
//			string jsonString = "{";
//			jsonString += "\"Type\" : " + Helpers.AddQuotes("Rune") + ",";
//			jsonString += "\"Price\" : " + GameManager._dataBank.GetDustRequiredForSkullLevel(this.skullLevel) + ",";
//			jsonString += "\"Rune Name\" : " + Helpers.AddQuotes(this.id) + ",";
//			jsonString += "\"Skull Level\" : " + this.skullLevel;
//			jsonString += "}";
//			
////			Debug.Log("JSON FOR RUNE :::::::::::: "+jsonString);
//			return jsonString;
//		}
		
		public override IDictionary ToDictionary()
		{
			runeDictionary["Type"] = "Rune";
			runeDictionary["dCost"] = this.dCost;
			runeDictionary["RuneName"] = this.id;
			runeDictionary["Index"] = this.IndexInList;
			runeDictionary["uid"] = this.uid;
			runeDictionary["itemId"] = this._itemName + this.skullLevel;
			runeDictionary["Tag"] = this.tag;
			runeDictionary["Stats"] = this.stats;
			runeDictionary["SkullLevel"] = this.skullLevel;
			runeDictionary["Spell Name"] = this.buff.id;
			runeDictionary["Element"] = this.element;
			runeDictionary["gCost"] = this.gCost;
			runeDictionary["IsSuggested"] = this.isSuggested;
			runeDictionary["IsNew"] = this.isNew;
			runeDictionary["SpellDetail"] = GameManager._dataBank.GetBuffForBuffID(this.buff.id, this.skullLevel);
			runeDictionary["sellCost"] = this.sellCost;
			return runeDictionary;
		}
		
		public override string ToString()
		{
			return "name : "+ this.id + ":::::UID::::::" + this.uid + ":::::: TYPE ::::::::::" + this._itemType;
		}
		
		
		public override bool Equals(object anObject)
		{
			if(anObject == null)
				return false;
			
			ItemRune otherRune = anObject as ItemRune;
			if(otherRune != null)
			{
				if(otherRune.id.Equals(this.id))
				{
					if(otherRune.skullLevel == this.skullLevel)
						return true;
				}
			}
			return false;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		
	}
}