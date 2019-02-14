using UnityEngine;
using System.Collections;

public class SRCharacterStateControllerYetAgain : MonoBehaviour {
	
	private const string TAG = "*** SRCharacterStateController ... ";

	private StateImplementerInterface _stateImplementerInterface;
	
	public void SetStateImplementer(StateImplementerInterface anInterface)
	{
		this._stateImplementerInterface = anInterface;
		
		if(_stateImplementerInterface == null)
			if(GameManager.PRINT_LOGS) Debug.Log("INTERNAL INTERFACE NULL!!!");
	}

	Animator _animator;
	
	private AnimatorStateInfo currentBaseState;
	
	public static int _chargingLoopState = Animator.StringToHash("Base Layer.ChargingLoop");
	public static int _chargeThrowState = Animator.StringToHash("Base Layer.ChargeThrow");
	public static int _castThrowState = Animator.StringToHash("Base Layer.CastThrow");
	public static int _focusToCastState = Animator.StringToHash("Base Layer.FocusToCast");
	public static int _castingLoopState = Animator.StringToHash("Base Layer.CastingLoop");
	public static int _focusLoopState = Animator.StringToHash("Base Layer.FocusLoop");
	public static int _castToChargeState = Animator.StringToHash("Base Layer.CastToCharge");
	public static int _damagedState = Animator.StringToHash("Base Layer.Wizard_08_Damage_Loop");
	public static int _stunnedState = Animator.StringToHash("Base Layer.Wizard_09_Stun");
	public static int _deathState = Animator.StringToHash("Base Layer.DeathState");
	public static int _wardState = Animator.StringToHash("Base Layer.WardState");
	public static int _walkState = Animator.StringToHash("Base Layer.Walk");
	public static int _kneelState = Animator.StringToHash("Base Layer.Kneel");
	public static int _spellLoop = Animator.StringToHash("Base Layer.SpellLoop");
	public static int _powerThrowState = Animator.StringToHash("Base Layer.PowerThrow");
	
	private int _currentState;
	
	
	public void ResetStates()
	{
		Damaged = false;
		Dead = false;
		Revert = false;
		Stunned = false;
		IsCasting = false;
		IsCharging = false;
		CastThrow = false;
		ChargeThrow = false;
	}
	
	public void SetIdle()
	{
		WalkState = false;
		Idle = true;
		TurnLeft = false;
		TurnRight = false;
		isFocusCB = false;
	}

	
	public void SetIdleToFocusState()
	{
		isFocusToCastCB = false;
		Idle = false;
		WalkState = false;
	}
	
	#region Animation Control variables
	
	private bool _turnLeft;
	public bool TurnLeft
	{
		set
		{
			_turnLeft = value;
			_animator.SetBool("TurnLeft",_turnLeft);
		}
		get
		{
			return this._turnLeft;
		}
	}
	
	private bool _burst;
	public bool Burst
	{
		set
		{
			_burst = value;
			_animator.SetBool("SpellBurst",_burst);
		}
		get
		{
			return this._burst;
		}
	}
	
	private bool _powerThrow;
	public bool PowerThrow
	{
		set
		{
			_powerThrow = value;
			isPowerThrowCB = false;
			_animator.SetBool("PowerThrow",_powerThrow);
//			if(value) {
//				Debug.LogError("PowerThrow animation - "+_animator.speed);
//				_animator.GetCurrentAnimatorStateInfo(0).;
//			}
		}
		get
		{
			return this._powerThrow;
		}
	}
	
	private bool _kneel;
	public bool Kneel
	{
		set
		{
			_kneel = value;
			_animator.SetBool("Kneel",_kneel);
		}
		get
		{
			return this._kneel;
		}
	}
	private bool _turnRight;
	public bool TurnRight
	{
		set
		{
			_turnRight = value;
			_animator.SetBool("TurnRight",_turnRight);
		}
		get
		{
			return this._turnRight;
		}
	}
	
	public bool _idle;
	public bool Idle
	{
		set
		{
			_idle = value;
			_animator.SetBool("Idle",_idle);
		}
		get
		{
			return this._idle;
		}
	}
	
	
	private bool _walk;
	public bool WalkState
	{
		set
		{
			_walk = value;
			_animator.SetBool("Walk",_walk);
		}
		get
		{
			return this._walk;
		}
	}
	
	public bool _damaged;
	public bool Damaged
	{
		set
		{
			_damaged = value;
			_animator.SetBool("IsDamaged",_damaged);
		}
		get
		{
			return this._damaged;
		}
	}
	
	private bool _ward;
	public bool Ward
	{
		set
		{
			_ward = value;
			_animator.SetBool("Ward",_ward);
		}
		get
		{
			return this._ward;
		}
	}
	
	private bool _dead;
	public bool Dead
	{
		set
		{
			_dead = value;
			_animator.SetBool("Death",_dead);
		}
		get
		{
			return this._dead;
		}
	}
	
	private bool _revert;
	public bool Revert
	{
		set
		{
			_revert = value;
			_animator.SetBool("Revert",_revert);
		}
		get
		{
			return this._revert;
		}
	}
	
	public bool _stunned;
	public bool Stunned
	{
		set
		{
			_stunned = value;
			_animator.SetBool("Stun",_stunned);
		}
		get
		{
			return this._stunned;
		}
	}
	
	
	public bool _casting;
	public bool IsCasting
	{
		set
		{
			_casting = value;
			_animator.SetBool("Casting",_casting);
		}
		get
		{
			return this._casting;
		}
	}
	
	public bool _charging;
	public bool IsCharging
	{
		set
		{
			_charging = value;
			_animator.SetBool("Charging",_charging);
		}
		get
		{
			return this._charging;
		}
	}
	
	public bool _chargeThrow;
	private bool ChargeThrow
	{
		set
		{
			_chargeThrow = value;
			_animator.SetBool("ChargeThrow", value);
		}
		get
		{
			return this._chargeThrow;
		}
	}
	
	public bool _castThrow;
	public bool CastThrow
	{
		set
		{
			_castThrow = value;
			_animator.SetBool("CastThrow", value);
		}
		get
		{
			return this._castThrow;
		}
	}
	
	public bool _makeTransition;
	public bool MakeTransition
	{
		set
		{
			_makeTransition = value;
			_animator.SetBool("MakeTransition", value);
		}
		get
		{
			return this._makeTransition;
		}
	}
	#endregion
	
	
	public void SetDeathState()
	{
		Damaged = false;
		Stunned = false;
		Dead = true;
	}
			
	
	public void SetSmallThrowState()
	{
		CastThrow = true;
	}

		
	public void SetCastingState()
	{
		IsCasting = true;
	}
		
	public void SetChargeToThrowState()
	{
		ChargeThrow = true;
	}
	
	public void SetCastToThrowState()
	{
		CastThrow = true;
	}

	
	public void SetCastToChargeState()
	{
		IsCharging = true;
	}
	

	public void SetDamagedState()
	{
		Damaged = true;
	}
	
	public void SetWardState()
	{
		Ward = true;
	}

	public void SetStunnedState(float delay)
	{
		Stunned = true;
	}
	
	private bool isFocusCB = false;
	private bool isFocusToCastCB = false;
	private bool isCastThrowCB = false;
	private bool isCastLoopCB = false;
	private bool isDamagedCB = false;
	private bool isWardCB = false;
	private bool isChargingLoopCB = false;
	private bool isCastToChargeCB = false;
	private bool isChargeThrowCB = false;
	private bool isSpellBurstCB = false;
	private bool isPowerThrowCB = false;
	
	void Update()
	{
		_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;

		if(this._stateImplementerInterface != null)
		{
			if(_currentState == _focusLoopState)
			{
				MakeTransition = false;
				PowerThrow = false;
				
				if(!isFocusCB)
				{
					isDamagedCB = false;
					isWardCB = false;
					isCastThrowCB = false;
					isChargeThrowCB = false;
					isSpellBurstCB = false;
					isFocusCB = true;

					_stateImplementerInterface.OnFocusStateReached();
				}
			}
			else if(_currentState == _focusToCastState)
			{
				IsCasting = false;

				MakeTransition = true;
			
				if(!isFocusToCastCB)
				{
					isFocusCB = false;
					isFocusToCastCB = true;

					_stateImplementerInterface.OnFocusToCastStateReached();
				}
			}
			else if(_currentState == _castingLoopState)
			{
				MakeTransition = false;
				if(!isCastLoopCB){
					isFocusToCastCB = false;
					isCastLoopCB = true;
					
					_stateImplementerInterface.OnCastingLoopReached();
				}
			}
			else if(_currentState == _castToChargeState)
			{
				IsCharging = false; 
				MakeTransition = true;
			
				if(!isCastToChargeCB)
				{
					isCastLoopCB = false;
					isCastToChargeCB = true;
					
					_stateImplementerInterface.OnCastToChargeReached();
				}
			}
			else if(_currentState == _powerThrowState)
			{
				if(!isPowerThrowCB)
				{
					isPowerThrowCB = true;
					Burst = false;
					isFocusCB = false;
					_stateImplementerInterface.OnPowerThrowReached();
				}
			}
			else if(_currentState == _deathState)
			{
				Dead = false;
				Damaged = false;
			}
			else if(_currentState == _chargingLoopState)
			{
				MakeTransition = false;
			
				if(!isChargingLoopCB)
				{
					isCastToChargeCB = false;
					isChargingLoopCB = true;
					
					_stateImplementerInterface.OnChargingLoopReached();
				}
			}
			else if(_currentState == _chargeThrowState)
			{
				MakeTransition = true;
				ChargeThrow = false;
			
				if(!isChargeThrowCB)
				{
					isChargingLoopCB = false;
					isChargeThrowCB = true;
					_stateImplementerInterface.OnChargeThrowReached();
				}
			}
			
			else if(_currentState == _castThrowState)
			{
				CastThrow = false;
				MakeTransition = true;
				if(!isCastThrowCB)
				{
					isCastLoopCB = false;
					isCastThrowCB = true;
					_stateImplementerInterface.OnCastThrowReached();
				}
			}
		
			else if(_currentState == _damagedState)
			{
				MakeTransition = true;
				Damaged = false;
				
				CastThrow = false;
				IsCasting = false;
				IsCharging = false;
				ChargeThrow = false;
				Revert = false;
				
				isCastLoopCB = false;
				isFocusToCastCB = false;
				isFocusCB = false;
				isCastThrowCB = false;
				isChargeThrowCB = false;
				isChargingLoopCB = false;
				isCastToChargeCB = false;
				isSpellBurstCB = false;
				if(!isDamagedCB)
				{
					isDamagedCB = true;
	
					_stateImplementerInterface.OnDamageStateReached();
				}
			}
			
			else if(_currentState == _wardState)
			{
				MakeTransition = true;
				Damaged = false;
				Ward = false;
				
				CastThrow = false;
				IsCasting = false;
				IsCharging = false;
				ChargeThrow = false;
				Revert = false;
				
				isCastLoopCB = false;
				isFocusToCastCB = false;
				isFocusCB = false;
				isCastThrowCB = false;
				isChargeThrowCB = false;
				isChargingLoopCB = false;
				isCastToChargeCB = false;
				if(!isWardCB)
				{
					isWardCB = true;
	
					_stateImplementerInterface.OnWardStateReached();
				}
			}
			else if(_currentState == _spellLoop)
			{
				
				if(!isSpellBurstCB){
					isSpellBurstCB = true;
					
					_stateImplementerInterface.OnBurstLoopReached();
				}
			}
		}
	}
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
	
	public void Walk () {
			//Idle=false;
		isFocusCB = false;
		_animator.SetBool("Walk", true);
	}
	public void KneelDown () {
		_animator.SetBool("Kneel", true);
		
	}
	public void KneelSwitchTurnOff() {
		_animator.SetBool("Kneel", false);
	}
	public void Stop () {
		isFocusCB = false;
		_animator.SetBool("Walk", false);
	}
	
	public void TreasureChestOpen() {
		GameObject tChest = GameObject.FindGameObjectWithTag("TreasureChest");
		if(GameManager.PRINT_LOGS) Debug.Log(tChest.name);
		if (tChest) {
			if(GameManager.PRINT_LOGS) Debug.Log(tChest.animation.name);
			tChest.animation["open"].speed=0.5f;
			tChest.animation.Play("open");
			SoundManager.instance.PlayTreasureOpenSound();
		}
	}
	public void TreasureChestClose() {
		GameObject tChest =GameObject.FindGameObjectWithTag("TreasureChest");
		if (tChest) {
			tChest.animation.Play("close");
		}
	}
}
