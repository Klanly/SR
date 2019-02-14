using UnityEngine;
using System.Collections;

public abstract class PurchaseRequest {

	public enum PurchaseRequestType
	{
		Ring,
		Rune,
		Keys,
		Portions,
		DUST,
		INAPP,
		Transmutation,
		TransmutationBoost,
		BagUpgrade,
		BagUpgradeBoost,
		PetUpgrade,
		PetUpgradeBoost,
		PotionBeltUpgrade,
		PotionBeltUpgradeBoost,
		KeyRingUpgrade,
		KeyRingUpgradeBoost,
		TransmutationCubeUpgrade,
		TransmutationCubeUpgradeBoost,
		Success,
		Error
	};
	
	private PurchaseRequestType _requestType;
	private string _uid;
	private bool _isAutoBuy = false;
	
	public PurchaseRequest(PurchaseRequestType requestType, string uid)
	{
		_requestType = requestType;
		_uid = uid;
	}
	
	public PurchaseRequestType RequestType
	{
		get
		{
			return _requestType;
		}
	}
	
	public string Uid
	{
		get
		{
			return _uid;
		}
	}
	
	public bool IsAutoBuy
	{
		get
		{
			return _isAutoBuy;
		}
		set
		{
			_isAutoBuy = value;
		}
	}
}
