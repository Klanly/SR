using UnityEngine;
using System.Collections.Generic;

public class InApps {
	
	private List<InApp> _inAppList;
	
	public InApps(List<InApp> inAppList)
	{
		_inAppList = inAppList;
	}
	
	public List<InApp> InAppList
	{
		get
		{
			return _inAppList;
		}
	}
	
	public int GetGCost(InApp.InAppTypes type)
	{
		int gCost = 0;
		InApp inApp = _inAppList.Find(temp => temp.Type == type);
		switch(type)
		{
			case InApp.InAppTypes.PORTIONS:
				gCost = ((InAppProtion)inApp).GCost;
			break;
			case InApp.InAppTypes.KEYS:
				gCost = ((InAppKeys)inApp).GCost;
			break;
		}
		return gCost;
	}
}
