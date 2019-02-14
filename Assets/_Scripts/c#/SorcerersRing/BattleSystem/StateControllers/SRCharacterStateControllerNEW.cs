using UnityEngine;
using System.Collections;

public class SRCharacterStateControllerNEW : MonoBehaviour {
	
	private const string TAG = "*** SRCharacterStateController ... ";

	
	public delegate void OnFocusToCastStateReachedDelegate();
	
	public OnFocusToCastStateReachedDelegate _onFocusToCastStateReachedDelegate
	{
		set;get;
	}
	
	public delegate void OnWalkStateReachedDelegate();
	
	public OnWalkStateReachedDelegate _onWalkStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnCastingLoopStateReachedDelegate();
	
	public OnCastingLoopStateReachedDelegate _onCastingLoopStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnCastThrowStateReachedDelegate();
	
	public OnCastThrowStateReachedDelegate _onCastThrowStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnCastChargeStateReachedDelegate();
	
	public OnCastChargeStateReachedDelegate _onCastChargeStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnChargeLoopStateReachedDelegate();
	
	public OnChargeLoopStateReachedDelegate _onChargeLoopStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnChargeThrowStateReachedDelegate();
	
	public OnChargeThrowStateReachedDelegate _onChargeThrowStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnFocusStateReachedDelegate();
	
	public OnFocusStateReachedDelegate _onFocusStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnDamageStateReachedDelegate();
	
	public OnDamageStateReachedDelegate _onDamageStateReachedDelegate
	{
		set;get;
	}
	
	
	public delegate void OnDelayTimerEndDelegate();
	
	public OnDelayTimerEndDelegate _onDelayTimerEndDelegate
	{
		set;get;
	}
	
	
	public delegate void OnStunStateReachedDelegate();
	
	public OnStunStateReachedDelegate _onStunStateReachedDelegate
	{
		set;get;
	}
	
	public delegate void OnStunStateEndDelegate();
	
	public OnStunStateEndDelegate _onStunStateEndDelegate
	{
		set;get;
	}
	
	private float _delayTimer = 0f;
	
	
		
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
	public static int _walkState = Animator.StringToHash("Base Layer.Walk");
	public static int _deathState = Animator.StringToHash("Base Layer.DeathState");
	
	public void SetIdleState()
	{
		Idle = true;
		Dead = false;
	}
	
	public void SetFocusState()
	{
		Idle = false;
	}
	
	#region Animation Control variables
	
	private bool _idle;
	
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
	
	
	private bool _damaged;
	
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
	
	
	private bool _walk;
	
	public bool Walk
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
	
	private bool _stunned;
	
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
	
	
	private bool _casting;
	
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
	
	public bool ChargeThrow
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
	
	private float _delay = 0f;
	
	public int _currentState;

		
	public void SetCastingState(OnFocusToCastStateReachedDelegate cb)
	{
		_onFocusToCastStateReachedDelegate = cb;

		IsCasting = true;
	}
	
	public void SetCastingStateForDirectCharging(OnCastingLoopStateReachedDelegate cb)
	{
		_onCastingLoopStateReachedDelegate = cb;

		IsCasting = true;
	}
	
	public void SetCastingLoopState(float delay, OnCastingLoopStateReachedDelegate firstCb, OnDelayTimerEndDelegate cb)
	{	
		_onCastingLoopStateReachedDelegate = firstCb;
		
		_onDelayTimerEndDelegate = cb;
		
		_delay = delay;
		
		MakeTransition = true;
	}
	
	public void SetChargeLoopState(float delay, OnDelayTimerEndDelegate cbDelayed)
	{
		_onDelayTimerEndDelegate = cbDelayed;
		
		
		if(_onDelayTimerEndDelegate == null)
		{
		
		}
		_delay = delay;
		
		MakeTransition = true;
	}
	
	
	public void SetChargeToThrowState(OnChargeThrowStateReachedDelegate cb)
	{
		_onChargeThrowStateReachedDelegate = cb;
		
		_delay = 0f;
		
		IsCharging = false;
		
		IsCasting = false;
		
		MakeTransition = false;
		
		ChargeThrow = true;
	}

	
	public void SetCastToThrowState(OnCastThrowStateReachedDelegate cb)
	{
		_onCastThrowStateReachedDelegate = cb;
	
		_delay = 0f;
		
		IsCharging = false;
		
		IsCasting = false;
		
		MakeTransition = false;
		
		ChargeThrow = false;
		
		Damaged = false;
		
		CastThrow = true;
	}

	
	public void SetStateToFocus(OnFocusStateReachedDelegate cb)
	{	
		_onFocusStateReachedDelegate = cb;
		
		_delay = 0f;
		
		_delayTimer = 0f;
		
		_onFocusToCastStateReachedDelegate = null;
		
		//_onCastingLoopStateReachedDelegate = null;
		
		_onCastChargeStateReachedDelegate = null;
		
		_onChargeLoopStateReachedDelegate = null;
		
		_onChargeThrowStateReachedDelegate = null;

		_onDelayTimerEndDelegate = null;
		
		MakeTransition = true;
	}
	
	
	public void SetCastToChargeState(float delay, OnDelayTimerEndDelegate cb)
	{
		_onDelayTimerEndDelegate = cb;
		
		_delay = delay;
		
		IsCharging = true;
		
		MakeTransition = false;
	}
	
	public void SetDeathState()
	{
		Damaged = false;
		
		Stunned = false;
		
		Dead = true;
	}
	

	public void SetDamagedState(OnDamageStateReachedDelegate cb)
	{
		_onDamageStateReachedDelegate = cb;

		_delay = 0f;
	}

	public void SetStunnedState(float delay, OnStunStateReachedDelegate delCb, OnStunStateEndDelegate timerDel)
	{
		Stunned = true;
		
		_onStunStateReachedDelegate = delCb;
		
		_onStunStateEndDelegate = timerDel;
		
		_delay = delay;
		
		_delayTimer = 0f;
	}
	
	public void SetWalkState(OnWalkStateReachedDelegate delCb)
	{
		_onWalkStateReachedDelegate = delCb;
		
		Walk = true;
	}
	

	void Update()
	{
		if(_animator.runtimeAnimatorController != null){
			_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
		}
		else{
			_animator = gameObject.GetComponent<Animator>();
			_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
		}
		
		if(_currentState == _focusLoopState)
		{
			MakeTransition = false;
			
			ChargeThrow = false;
			
			CastThrow = false;
			
			IsCharging = false;
			
			Walk = false;
			
			Stunned = false;
			
			if(_onFocusStateReachedDelegate != null)
			{
				_onFocusStateReachedDelegate();
				
				_onFocusStateReachedDelegate = null;
			}
		}
		
		
		if(_currentState == _focusToCastState)
		{
			IsCharging = false;

			_delayTimer = 0f;
			
			MakeTransition = true;
			
			if(_onFocusToCastStateReachedDelegate != null)
			{
				_onFocusToCastStateReachedDelegate();
				
				_onFocusToCastStateReachedDelegate = null;
			}
		}
		
		if(_currentState == _deathState)
		{
			Dead = false; 
		}
		
		if(_currentState == _walkState)
		{
			Walk = false;
			
			if(_onWalkStateReachedDelegate != null)
			{
				_onWalkStateReachedDelegate();
				
				_onWalkStateReachedDelegate = null;
			}
		}
		
		
		if(_currentState == _castingLoopState)
		{
			if(_onCastingLoopStateReachedDelegate != null)
			{
				_onCastingLoopStateReachedDelegate();
//				_onFocusToCastStateReachedDelegate();
				_onCastingLoopStateReachedDelegate = null;
			}
		}
		
		
		if(_currentState == _castThrowState)
		{
			if(_onCastThrowStateReachedDelegate != null)
			{
				_onCastThrowStateReachedDelegate();
				
				_onCastThrowStateReachedDelegate = null;
			}
		}
		
		if(_currentState == _castToChargeState)
		{
			IsCharging = false;
			
			_delayTimer = 0f;
			
			if(_onCastChargeStateReachedDelegate != null)
			{
				_onCastChargeStateReachedDelegate();
				
				_onCastChargeStateReachedDelegate = null;
			}
		}
		
		
		if(_currentState == _chargingLoopState)
		{
			MakeTransition = false;
			
			if(_onChargeLoopStateReachedDelegate != null)
			{
				_onChargeLoopStateReachedDelegate();
				
				_onChargeLoopStateReachedDelegate = null;
			}
		}

		
		if(_currentState == _chargeThrowState)
		{
			ChargeThrow = false;
			
			if(_onChargeThrowStateReachedDelegate != null)
			{
				_onChargeThrowStateReachedDelegate();
				
				_onChargeThrowStateReachedDelegate = null;
			}
		}
		
		
		if(_currentState == _focusLoopState)
		{
			MakeTransition = false;
			
			if(_onFocusStateReachedDelegate != null)
			{
				_onFocusStateReachedDelegate();
				
				_onFocusStateReachedDelegate = null;
			}
		}
		
		if(_currentState == _stunnedState)
		{
			if(_onStunStateReachedDelegate != null)
			{
				_onStunStateReachedDelegate();
				
				_onStunStateReachedDelegate = null;
			}
		}
		
		
		
		if(_currentState == _damagedState)
		{	
			Damaged = false;
			
			CastThrow = false;
			
			ChargeThrow = false;
			
			IsCasting = false;
			
			IsCharging = false;
			

			
				
			_delayTimer = 0f;
			
			if(!Stunned)
			{
				_delay = 0f;
			}
			
			if(_onDamageStateReachedDelegate != null)
			{
				MakeTransition = false;
				
				_onFocusToCastStateReachedDelegate = null;
			
				_onCastingLoopStateReachedDelegate = null;
				
				_onCastChargeStateReachedDelegate = null;
				
				_onChargeLoopStateReachedDelegate = null;
				
				_onChargeThrowStateReachedDelegate = null;
				
				_onFocusStateReachedDelegate = null;
				
				_onDelayTimerEndDelegate = null;
				
				_onDamageStateReachedDelegate();
				
				_onDamageStateReachedDelegate = null;
			}
			
			
		}
		
		
		if(_onDamageStateReachedDelegate != null)
		{
			Damaged = true;
		}
		
		if(_delay > 0f)
		{
			if(_delayTimer > _delay)
			{
				_delayTimer = 0f;
				
				_delay = 0f;
				
				if(_onDelayTimerEndDelegate != null)
				{
					StartCoroutine(DelayedDelegateExecutor(_onDelayTimerEndDelegate));
					
					_onDelayTimerEndDelegate = null;
				}
				
				if(_onStunStateEndDelegate != null)
				{
					StartCoroutine(DelayedDelegateExecutor(_onStunStateEndDelegate));
					
					_onStunStateEndDelegate = null;
				}
			}
			else
			{
				_delayTimer += Time.deltaTime;
			}
		}
	}
	
	private IEnumerator DelayedDelegateExecutor(OnDelayTimerEndDelegate del)
	{
		yield return null;

		del();
	}
	
	
	private IEnumerator DelayedDelegateExecutor(OnStunStateEndDelegate del)
	{
		yield return null;
		
		del();
	}
	
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
}
