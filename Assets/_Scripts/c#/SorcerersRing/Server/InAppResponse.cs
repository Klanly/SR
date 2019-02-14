using UnityEngine;
using System.Collections;

public class InAppResponse : ServerResponse {
	
	private int _itemQuantity = 0;
	private string _itemIdentifier = "";
	private string _transactionId = "";
	
	public InAppResponse(ServerRequest request, bool isSuccess, long serverCurrentTime, string itemIdentifier, int itemQuantity, string transactionId)
	: base(request, isSuccess, serverCurrentTime)
	{
		_itemIdentifier = itemIdentifier;
		_itemQuantity = itemQuantity;
		_transactionId = transactionId;
	}
	
	public string ItemIdentifier
	{
		get
		{
			return _itemIdentifier;
		}
	}
	
	public int ItemQuantity
	{
		get
		{
			return _itemQuantity;
		}
	}
	
	public string TransactionId
	{
		get
		{
			return _transactionId;
		}
	}
}
