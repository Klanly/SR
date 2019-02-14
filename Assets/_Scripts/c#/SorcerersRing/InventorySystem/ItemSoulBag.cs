using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem{
	public class ItemSoulBag : InventoryItem
	{
		public int soulValue
		{
			get;set;
		}
		public ItemSoulBag(int val)
		{
			this.soulValue=val;
			this.SetItemType(InventoryItem.Type.kSoulBag);
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary keyDictionary = new Dictionary<string, object>();
			
			return keyDictionary;
		}
	}
}