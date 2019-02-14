using UnityEngine;
using System.Collections.Generic;

public class LogTransactionsRequest : ServerRequest {

	private List<Dictionary<string, object>> _transactions = null;
	
	public LogTransactionsRequest(ServerRequestType requestType, ProcessResponse response, List<Dictionary<string, object>> transactions) 
	: base(requestType, response)
	{
		_transactions = transactions;
	}
	
	public List<Dictionary<string, object>> Transactions
	{
		get
		{
			return _transactions;
		}
	}
}
