using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InventorySystem
{
	public class KeyRing : ItemContainer, IComparable
	{
		private int _keyCount;
		public int keyCount
		{
			get
			{
				return _keyCount;
			}
			set
			{
				_keyCount=value;
				if(keyCount>this.capacity)
				_keyCount=this.capacity;
			}
		}
		
		int IComparable.CompareTo(object aKeyRing)
		{
			KeyRing otherKeyRing = (KeyRing) aKeyRing;
			if(this.level > otherKeyRing.level)
				return 1;
			else if(this.level < otherKeyRing.level)
				return -1;
			else
				return 0;
		}
		
		public KeyRing(int capacity, int skullLevel, string id, int level, float upgradeDustCost, float upgradeTime,string keyDescription)
		{
			this.capacity = capacity;
			this.skullLevel = skullLevel;
			this.id = id;
			this.level = level;
			this.dCost = upgradeDustCost;
			this.uTime = upgradeTime;
			this.keyCount=0;
			this.description=keyDescription;
		}
		
		public bool AddKey()
		{
			if(this.keyCount<this.capacity)
			{
				this.keyCount++;
				return true;
			}
			else
			return false;
		}
		
		public bool RemoveKey()
		{
			if(this.keyCount>0)
			{
				this.keyCount--;
				return true;
			}
			else
			return false;
		}
		
		public int RemoveKeys(int count)
		{
			if(this.keyCount>0)
			return this.keyCount-=count;
			else
			return this.keyCount;
		}
		
		public override IDictionary ToDictionary()
		{
			IDictionary dictionary = base.ToDictionary();
			dictionary["dCost"] = this.dCost;
			
			return dictionary;
		}
		
		public KeyRing Clone()
		{
			return new KeyRing(this.capacity, this.skullLevel, this.id, this.level, this.dCost, this.uTime,this.description);
		}
	}
}