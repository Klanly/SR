using UnityEngine;
using System.Collections;

public abstract class ServerRequest 
{
	private ServerRequestType _requestType;
	private ProcessResponse _response = null;
	
	public enum ServerRequestType
	{
		All,
		IsCompleted,
		Transmutation,
		TransmutationBoost,
		TransmutationCompleted,
		Upgrade,
		UpgradeBoost,
		UpgradeCompleted,
		Spirit,
		SpiritBoost,
		SpiritCompleted,
		GetInAppPurchases,
		VerifyInAppPurchase,
		LogTransactions,
		ActivateShrine,
		ActivateFriendsShrine,
		CollectShrineReward,
		ChargeShrine
	}
	
	public delegate void ProcessResponse(ServerResponse response);
	
	public ServerRequest(ServerRequestType requestType, ProcessResponse response)
	{
		_requestType = requestType;
		_response = response;
	}
	
	
	public ServerRequestType RequestType
	{
		get
		{
			return _requestType;
		}
	}
	
	public ProcessResponse Response
	{
		get
		{
			return _response;
		}
	}
}
