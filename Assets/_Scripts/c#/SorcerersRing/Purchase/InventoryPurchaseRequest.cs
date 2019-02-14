using UnityEngine;
using System;
using System.Collections;

public class InventoryPurchaseRequest : PurchaseRequest {
	
	public InventoryPurchaseRequest(PurchaseRequestType requestType, string uid) 
	: base(requestType, uid)
	{
	}
}
