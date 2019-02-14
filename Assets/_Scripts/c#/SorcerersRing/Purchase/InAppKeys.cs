using UnityEngine;
using System.Collections;

public class InAppKeys : InApp {
	
	private string _name;
	private int _gCost;
	
	public InAppKeys(InApp.InAppTypes type, string name, int gCost) : base(type)
	{
		_name = name;
		_gCost = gCost;
	}
	
	public string Name
	{
		get
		{
			return _name;
		}
	}
	
	public int GCost
	{
		get
		{
			return _gCost;
		}
	}
}
