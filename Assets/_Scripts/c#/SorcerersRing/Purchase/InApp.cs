using UnityEngine;
using System.Collections;

public abstract class InApp {
	
	public enum InAppTypes
	{
		PORTIONS,
		KEYS
	}

	public enum InAppClass 
	{
		NONPREMIUM = 0,
		PREMIUM
	}
	
	private InAppTypes _type;
	
	public InApp(InAppTypes type)
	{
		_type = type;
	}
	
	public InAppTypes Type
	{
		get
		{
			return _type;
		}
	}
}
