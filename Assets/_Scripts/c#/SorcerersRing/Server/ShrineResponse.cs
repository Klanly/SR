using UnityEngine;
using System.Collections;

public class ShrineResponse : ServerResponse {
	
	private int _currentShrinePoints;
	private bool _isLocked;
	public ShrineResponse(ServerRequest request, bool isSuccess, long serverCurrentTime = 0,int points = 0,bool lockedState =false)
	: base(request, isSuccess, serverCurrentTime)
	{
		this._currentShrinePoints = points;
		this._isLocked=lockedState;
	}
	
	public int CurrentShrinePoints
	{
		get
		{
			return _currentShrinePoints;
		}
	}
	
	public bool IsLocked
	{
		get
		{
			return _isLocked;
		}
	}
}
