using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	[System.Serializable]
	public class ItemRing : InventoryItem {
		
		private const string KEY_TYPE = "Type";
		
		private IDictionary ringDictionary = new Dictionary<string, object>();
		
		public enum RingType{kDamage, kLife, kElement, kWard}

		public ItemRing()
		{
//			this.SetItemType(InventoryItem.Type.kRing);
			_itemType = Type.kRing;
		}

		public string tag;
		
		public ItemRing(string id, int skullLevel)
		{
			this.SetItemType(InventoryItem.Type.kRing);
			this.id = id;
			this.skullLevel = skullLevel;
		}
		
		public string itemId;
		public static ItemRing GetRingFromDictionary(IDictionary ringDictionary)
		{
			//this.SetItemType(InventoryItem.Type.kRing);
			bool isPremium = false;
			
			int dCost = System.Int32.Parse(ringDictionary["dCost"].ToString());
			if(dCost == 0)
				isPremium = true;
			string id = ringDictionary["RingName"].ToString();
			int skullLevel = System.Int32.Parse(ringDictionary["SkullLevel"].ToString());
			
			ItemRing targetRing = null;
			if(isPremium)
				targetRing = GameManager._dataBank.premiumRingsList.Find(aRing => (aRing.id.Equals(id) && aRing.skullLevel == skullLevel));
			else
				targetRing = GameManager._dataBank.nonpremiumRingsList.Find(aRing => (aRing.id.Equals(id) && aRing.skullLevel == skullLevel));
			
			if(ringDictionary.Contains("uid"))
				targetRing.uid = ringDictionary["uid"].ToString();
			
			return targetRing;
		}
		
		public ItemRing Clone()
		{
			ItemRing ring = new ItemRing(this.id, this.skullLevel);
			ring.ringType = this.ringType;
			ring.stats = this.stats;
			ring.life = this.life;
			ring.uid = Nonce.GetUniqueID();
			ring._itemName = this._itemName;
			ring.damage = this.damage;
			ring.fire = this.fire;
			ring.water = this.water;
			ring.wards = this.wards;
			ring.earth = this.earth;
			ring.lightning = this.lightning;
			ring.dCost = this.dCost;
			ring.gCost = this.gCost;
			ring.isNew = this.isNew;
			ring.isSuggested = this.isSuggested;
			ring.tag = this.tag;
			return ring;
		}
		
		protected override void SetItemType(Type aType)
		{
			base.SetItemType(InventoryItem.Type.kRing);
		}
		
		public RingType ringType;
		
		public int stats;
		public int life;
		public int damage;
		public int fire;
		public int water;
		public int earth;
		public int lightning;
		public int wards;
		
		public override string ToString()
		{
			return  "TYPE -->" + _itemType.ToString() + " |||||| " + 
					"TAG -->" + this.tag + " |||||| " + 
					"ID -->" + this.id + " |||||| " + 
					"SKULL LVL -->" + this.skullLevel + " |||||| " + 
					"NAME -->" + _itemName + " |||||| " +
					"LIFE -->" + life + " |||||| " +
					"DAMAGE -->" + damage + " |||||| " +
					"MARKET VAL -->" + _marketValue + " |||||| " +
					"WARDS -->" + wards + " |||||| ";
		}
		
		public string ToJson()
		{
			//Have omitted the life-damage field AND Item type (Ring in this case).
			string jsonString = "{";
			jsonString += "\"Type\" : " + Helpers.AddQuotes("Ring") + ",";
			jsonString += "\"Price\" : " + GameManager._dataBank.GetDustRequiredForSkullLevel(this.skullLevel) + ",";
//			jsonString += "\"Ring Type\" : " + Helpers.AddQuotes(this._ringType.ToString()) + ",";
			jsonString += "\"RingName\" : " + Helpers.AddQuotes(this._itemName) + ",";
			jsonString += "\"SkullLevel\" : " + this.skullLevel + ",";
			jsonString += "\"IsNew\" : " + this.isNew + ",";
			jsonString += "\"IsSuggested\" : " + this.isSuggested + ",";
			jsonString += "\"Stats\" : " + this.stats + ",";
			jsonString += "\"Fire\" : " + Helpers.AddQuotes(this.fire+"") + ",";
			jsonString += "\"Lightning\" : " + Helpers.AddQuotes(this.lightning+"") + ",";
			jsonString += "\"Water\" : " + Helpers.AddQuotes(this.water+"") + ",";
			jsonString += "\"Earth\" : " + Helpers.AddQuotes(this.earth+"") + ",";
			jsonString += "\"Wards\" : " + Helpers.AddQuotes(this.wards+"") + ",";
			jsonString += "\"Life\" : " + Helpers.AddQuotes(this.life+"") + ",";
			jsonString += "\"Damage\" : " + this.damage;
			jsonString += "}";
			
			return jsonString;
		}
		
		public override IDictionary ToDictionary()
		{
			ringDictionary[KEY_TYPE] = InventoryLoader.TYPE_OPTION_RING;
			ringDictionary["dCost"] = this.dCost;
			ringDictionary["uid"] = this.uid;
			ringDictionary["itemId"] = this._itemName + this.skullLevel;
			ringDictionary["RingName"] = this._itemName;
			ringDictionary["SkullLevel"] = this.skullLevel;
			ringDictionary["Stats"] = this.stats;
			ringDictionary["IsNew"] = this.isNew;
			ringDictionary["IsSuggested"] = this.isSuggested;
			ringDictionary["Tag"] = this.tag;
			ringDictionary["Fire"] = this.fire+"";
			ringDictionary["Lightning"] = this.lightning+"";
			ringDictionary["Water"] = this.water+"";
			ringDictionary["Earth"] = this.earth+"";
			ringDictionary["Wards"] = this.wards+"";
			ringDictionary["Life"] = this.life+""; 
			ringDictionary["gCost"] = this.gCost;
			ringDictionary["Index"] = this.IndexInList;
			ringDictionary["Damage"] = this.damage;
			ringDictionary["sellCost"] = this.sellCost;

			return ringDictionary;
		}
		
		public override bool Equals(object anObject)
		{
			if(anObject == null)
				return false;
			
			ItemRing otherRing = anObject as ItemRing;
			if(otherRing != null)
			{
				if(otherRing.id.Equals(this.id))
				{
					if(otherRing.skullLevel == this.skullLevel)
					{
						return true;
					}
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