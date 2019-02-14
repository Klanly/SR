using UnityEngine;
using System.Collections;

public interface StateImplementerInterface
{
	void OnFocusStateReached();
	
	void OnWardStateReached();
	
	void OnFocusToCastStateReached();
	
	void OnCastingLoopReached();
	
	void OnCastToChargeReached();
	
	void OnChargingLoopReached();
	
	void OnChargeThrowReached();
	
	void OnDamageStateReached();
	
	void OnCastThrowReached();
	
	void OnBurstLoopReached();
	
	void OnPowerThrowReached();
}
