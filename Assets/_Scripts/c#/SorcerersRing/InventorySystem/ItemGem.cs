using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem{
	public class ItemGem : InventoryItem
	{
		public int gemValue
		{
			get;set;
		}
		
		public ItemGem(int val)
		{
			this.gemValue=val;
			this.SetItemType(InventoryItem.Type.kSoulGem);
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary keyDictionary = new Dictionary<string, object>();
			
			return keyDictionary;
		}
	}
}