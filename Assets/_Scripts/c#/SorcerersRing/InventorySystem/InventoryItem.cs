using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem
{
	[System.Serializable]
	public class InventoryItem { 
		
		public enum Category{kCommon, kUncommon, kRare, kPremium}
		
		public enum Type{kRing, kSoulDust, kSoulGem, kWeapon, kHealthPotion, kCharm, kPet, kRune, kWardPotion,kKey,kSoulBag}
		
		public const int SOULS_PER_GEM = 250;
		
		private const int SOULS_SELL_FACTOR = 4;
		
		public bool isSuggested = false;
		
		public bool isNew = false;
		
		public int gCost;
		
		public bool market = false; // whether it is in market or in inventory!
		
		public int sellCost
		{
			get
			{
				int totalDust = 0;
				
				if(this.gCost > 0)
					totalDust = this.gCost * SOULS_PER_GEM;
				else
					totalDust = this.dCost;
				
				return totalDust / SOULS_SELL_FACTOR;
			}
			private set{}
		}
		
		public string uid;
		
		public int dCost;
		////////////////////////////////////////////////////////////////// 
		public string id;
	
		public int skullLevel;
		////////////////////////////////////////////////////////////////// 
		public int IndexInList = -1;
		//////////////////////////////////////////////////////////////////
		public string _itemName;
		
		public string ItemName()
		{
			return this._itemName;
		}
		
		public void SetItemName(string itemName)
		{
			this._itemName = itemName;
		}
		//////////////////////////////////////////////////////////////////
		
		//////////////////////////////////////////////////////////////////
		protected Category _itemCategory;
		
		public Category ItemCategory()
		{
			return this._itemCategory;
		}
		
		public void SetItemCategory(Category aCategory)
		{
			this._itemCategory = aCategory;
		}
		//////////////////////////////////////////////////////////////////
		
		//////////////////////////////////////////////////////////////////
		public Type _itemType;
		
		public Type ItemType()
		{
			return this._itemType;
		}
		
		protected virtual void SetItemType(Type aType)
		{
			this._itemType = aType;
		}
		//////////////////////////////////////////////////////////////////
		
		//////////////////////////////////////////////////////////////////
		protected float _marketValue;
		
		public void SetMarketValue(float mVal)
		{
			this._marketValue = mVal;
		}
		
		public float MarketValue()
		{
			return this._marketValue;
		}
		//////////////////////////////////////////////////////////////////
		
		//////////////////////////////////////////////////////////////////
		protected string _prefabName;
		
		public string PrefabName()
		{
			return this._prefabName;
		}

		public void SetPrefabName(string pName)
		{
			this._prefabName = pName;
		}
		//////////////////////////////////////////////////////////////////
		
		public override string ToString()
		{
			return "-=- PARENT -=- :::::::::::::: " + 
				"ITEM TYPE = " + this._itemType;
		}
		
		public virtual IDictionary ToDictionary()
		{
			IDictionary d = new Dictionary<string, object>();
			
			return d;
		}
	}
}