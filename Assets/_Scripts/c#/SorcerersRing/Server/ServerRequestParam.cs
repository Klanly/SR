using UnityEngine;
using System.Collections;

public class ServerRequestParam {

	private string _transactionId;
	private string _boostTransactionId;
	private int _skullLevel;
	private string _itemId;
	private int _boost;
	
	public ServerRequestParam(string transactionId, string boostTransactionId, int skullLevel, string itemId, int boost)
	{
		_transactionId = transactionId;
		_boostTransactionId = boostTransactionId;
		_skullLevel = skullLevel;
		_itemId = itemId;
		_boost = boost;
	}
	
	public string TransactionId
	{
		get
		{
			return _transactionId;
		}
	}
	
	public string BoostTransactionId
	{
		get
		{
			return _boostTransactionId;
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
	
	public int Boost
	{
		get
		{
			return _boost;
		}
	}
}
