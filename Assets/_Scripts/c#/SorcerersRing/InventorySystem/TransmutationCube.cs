using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InventorySystem
{
	public class TransmutationCube : ItemContainer, IComparable
	{
		public int transmutationLoss;
		
		List<ItemRune> runes;
		
		int IComparable.CompareTo(object aTCube) //For Sorting of transmutation cubes according to their level. Lower level first...
		{
			TransmutationCube otherCube = (TransmutationCube) aTCube;
			if(this.level > otherCube.level)
				return 1;
			else if(this.level < otherCube.level)
				return -1;
			else
				return 0;
		}
		
		public TransmutationCube(int capacity, int skullLevel, string id, int level, int transmutationLoss, float gCost, float uTime,string transmutationDescription)
		{
			this.id = id;
			this.skullLevel = skullLevel;
			this.level = level;
			this.gCost = gCost;
			this.uTime = uTime;
			this.capacity = capacity;
			this.transmutationLoss = transmutationLoss;
			this.description=transmutationDescription;
			runes = new List<ItemRune>(this.capacity);
		}
		
		public TransmutationCube Clone()
		{
			return new TransmutationCube(this.capacity, this.skullLevel, this.id, this.level, this.transmutationLoss, this.gCost, this.uTime,this.description);
		}
		
		public bool AddRune(ItemRune aRune)
		{
			if(runes.Count < this.capacity)
			{
				runes.Add(aRune);
				return true;
			}
			return false;
		}
		
		public bool RemoveRune(ItemRune aRune)
		{
			return runes.Remove(aRune);
		}
	
		public override IDictionary ToDictionary()
		{
			IDictionary dictionary = base.ToDictionary();
			dictionary["gCost"] = this.gCost;
			dictionary["tLoss"] = this.transmutationLoss;
			return dictionary;
		}
		
		public string ToJson()
		{
			string jsonString = "[";
			
			for(int i = 0;i<runes.Count;i++)
			{
				jsonString += MiniJSON.Json.Serialize(runes[i].ToDictionary());
				
				if(i != runes.Count -1)
				{
					jsonString += ",";
				}
			}
			
			jsonString += "]";
			
			return jsonString;
		}
	}
}
