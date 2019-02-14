using UnityEngine;
using System.Collections;

public class SRPetStateController : MonoBehaviour {

	static System.Random random = new System.Random();
		
	public interface PetStateInterface
	{
		void OnReadyStateReached();
		void OnCastingStateReached();
		void OnSummonStateReached();
		void OnThrowStateReached();
	}	
	
	public PetStateInterface _petStateListener;
	
	public static int _summonState = Animator.StringToHash("Base Layer.SummonState");
	public static int _castingState = Animator.StringToHash("Base Layer.CastingState");
	public static int _readyState = Animator.StringToHash("Base Layer.ReadyState");
	public static int _throwState = Animator.StringToHash("Base Layer.ThrowState");
	
	public static int _subCastJunctionState = Animator.StringToHash("CastingState.CastingJunctionState");
	public static int _subCast1State = Animator.StringToHash("CastingState.CastingSub1");
	public static int _subCast2State = Animator.StringToHash("CastingState.CastingSub2");
	public static int _subCast3State = Animator.StringToHash("CastingState.CastingSub3");
	
	Animator _animator;
	
	public int _currentState;
	
	#region Animation Control variables
	
	private bool _cast;
	public bool Cast
	{
		set
		{
			_cast = value;
			_animator.SetBool("cast",_cast);
		}
		get
		{
			return _cast;
		}
	}
	
	private bool _ready;
	public bool Ready
	{
		set
		{
			_ready = value;
			_animator.SetBool("ready",_ready);
		}
		get
		{
			return _ready;
		}
	}
	
	private bool _throw;
	public bool Throw
	{
		set
		{
			_throw = value;
			_animator.SetBool("throw",_throw);
		}
		get
		{
			return _throw;
		}
	}
	
	private int _subCast;
	public int SubCast
	{
		set
		{
			_subCast = value;
			_animator.SetInteger("subCast",_subCast);
		}
		get
		{
			return _subCast;
		}
	}
	
	#endregion
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
	
	public void CastSpell()
	{
		Cast = true;
		SubCast = random.Next(1,4);
	}
	
	private bool isSummonCB = false;
	private bool isCastCB = false;
	private bool isReadyCB = false;
	private bool isThrowCB = false;
	
	void Update()
	{
		_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
		
		if(this._petStateListener != null)
		{
			if(_currentState == _summonState)
			{
				if(!isSummonCB)
				{
					isCastCB = false;
					isReadyCB = false;
					isThrowCB = false;
						
					isSummonCB = true;
					_petStateListener.OnSummonStateReached();
				}
			}
			else if(_currentState == _subCast1State || _currentState == _subCast2State || _currentState == _subCast3State)
			{
				Throw = false;
				
				if(!isCastCB)
				{
					isSummonCB = false;
					isReadyCB = false;
					isThrowCB = false;
					
					isCastCB = true;
					_petStateListener.OnCastingStateReached();
				}
			}
			else if(_currentState == _readyState)
			{
				if(!isReadyCB)
				{
					isSummonCB = false;
					isThrowCB = false;
					isCastCB = false;
					
					 isReadyCB = true;
					_petStateListener.OnReadyStateReached();
				}
			}
			else if(_currentState == _subCastJunctionState)
			{
				CastSpell();
			}
			else if(_currentState == _throwState)
			{
				Ready = false;
				
				if(!isThrowCB)
				{
					isSummonCB = false;
					isReadyCB = false;
					isCastCB = false;
					
					isThrowCB = true;
					_petStateListener.OnThrowStateReached();
				}
			}
		}
	}
}
