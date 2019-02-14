using UnityEngine;
using System.Collections;

public class UpdateResponeParam {
	
	private string _transactionId = "";
	private long _startTime = 0;
	private long _endTime = 0;
	private long _remainingTime = 0;
	private bool _isCompleted = true;
	private int _boost = 0;
	private string _boostId = "";
	
	public UpdateResponeParam(string transactionId, long startTime, long endTime, long remainingTime, bool isCompleted, int boost, string boostId)
	{
		_transactionId = transactionId;
		_startTime = startTime;
		_endTime = endTime;
		_remainingTime = remainingTime;
		_isCompleted = isCompleted;
		_boost = boost;
		_boostId = boostId;
	}
	
	public string TransactionId
	{
		get
		{
			return _transactionId;
		}
	}
	
	public long StartTime
	{
		get
		{
			return _startTime;
		}
	}
	
	public long EndTime
	{
		get
		{
			return _endTime;
		}
	}
	
	public long RemainingTime
	{
		get
		{
			return _remainingTime;
		}
	}
	
	public bool IsCompleted
	{
		get
		{
			return _isCompleted;
		}
	}
	
	public int Boost
	{
		get
		{
			return _boost;
		}
	}
	
	public string BoostId
	{
		get
		{
			return _boostId;
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("[UpdateResponeParam: TransactionId={0}, StartTime={1}, EndTime={2}, RemainingTime={3}, IsCompleted={4}, Boost={5}, BoostId={6}]", TransactionId, StartTime, EndTime, RemainingTime, IsCompleted, Boost, BoostId);
	}
}
