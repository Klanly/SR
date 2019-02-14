using UnityEngine;
using System.Collections.Generic;

public class LogTransactionsResponse : ServerResponse
{
	private List<string> _transactionIds = null;
	
	public LogTransactionsResponse(ServerRequest request, bool isSuccess, long serverCurrentTime, List<string> transactionIds)
	: base(request, isSuccess, serverCurrentTime)
	{
		_transactionIds = transactionIds;
	}
	
	public List<string> TransactionIds
	{
		get
		{
			return _transactionIds;
		}
	}
}
