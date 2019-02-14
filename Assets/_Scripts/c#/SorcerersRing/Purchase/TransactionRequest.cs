using UnityEngine;
using System.Collections;

public class TransactionRequest : PurchaseRequest {
	
	private string _parentUid;
	private int _transactionGem;
	private int _transactionSouls;
	private int _skullLevel;
	private string _itemId;
	private bool _isCommitted;
	
	public TransactionRequest(PurchaseRequestType requestType, string uid, string parentUid, int transactionGem, int transactionSouls, int skullLevel, string itemId, bool isCommitted = false) 
	: base(requestType, uid)
	{
		_parentUid = parentUid;
		_transactionGem = transactionGem;
		_transactionSouls = transactionSouls;
		_skullLevel = skullLevel;
		_itemId = itemId;
		_isCommitted = isCommitted;
	}
	
	public string ParentUid
	{
		get
		{
			return _parentUid;
		}
	}
	
	public int TransactionGem
	{
		get
		{
			return _transactionGem;
		}
	}
	
	public int TransactionSouls
	{
		get
		{
			return _transactionSouls;
		}
	}
	
	public int SkullLevel
	{
		get
		{
			return _skullLevel;
		}
	}
	
	public string ItemId
	{
		get
		{
			return _itemId;
		}
	}

	public bool IsCommitted
	{
		get
		{
			return _isCommitted;
		}
	}
}
