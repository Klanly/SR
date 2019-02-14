using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemContainer
{
	public int level;
	public int skullLevel;
	public string id;
	public float gCost;
	public float dCost;
	public float uTime;
	public int capacity;
	public string description;
	
	public virtual IDictionary ToDictionary()
	{
		IDictionary dictionary = new Dictionary<string, object>();
		dictionary["id"] = this.id;
		dictionary["SkullLevel"] = this.skullLevel;
		dictionary["uLevel"] = this.level;
		dictionary["Capacity"] = this.capacity;
		dictionary["uTime"] = this.uTime;
		dictionary["Description"]=this.description;
		return dictionary;
	}
}
