using UnityEngine;
using System.Collections;

public class InAppRequest : ServerRequest {
	private string _transactionId = null;
	private string _receipt = null;
	private string _signature = null;
	private string _itemId = null;

	public InAppRequest(ServerRequest.ServerRequestType requestType, ProcessResponse response, string itemId = null, string transactionId = null, string receipt = null, string signature = null) : base(requestType, response)
	{
		_receipt = receipt;
		_transactionId = transactionId;
		_itemId = itemId;
	}
	
	public string TransactionId
	{
		get
		{
			return _transactionId;
		}
	}
	
	public string Receipt
	{
		get
		{
			return _receipt;
		}
	}
	public string Signature
	{
		get
		{
			return _signature;
		}
	}
	public string ItemId
	{
		get
		{
			return _itemId;
		}
	}
}
