using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InventorySystem{
	public class ItemKey : InventoryItem
	{
		
		public ItemKey()
		{
			this.SetItemType(InventoryItem.Type.kKey);
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary keyDictionary = new Dictionary<string, object>();
			
			return keyDictionary;
		}
	}
}