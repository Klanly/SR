using UnityEngine;
using System.Collections;

public class Sorcerer_CinematicControlScript : MonoBehaviour {
	

	Animator _animator;
	private AnimatorStateInfo currentBaseState;
	

	public static int _focusLoopState = Animator.StringToHash("Base Layer.FocusLoop");
	public static int _wardState = Animator.StringToHash("Base Layer.WardState");
	public static int _walkState = Animator.StringToHash("Base Layer.Walk");
	
	private int _currentState;
	
	
	public void ResetStates()
	{
		//Damaged = false;
	}
	
	
	void Update()
	{
		//_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
		
	}
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
	
	public void Walk () {
		_animator.SetBool("Walk", true);
	}
	public void StopAndIdle () {
		_animator.SetBool("Walk", false);
		_animator.SetBool("Idle", true);
	}
	public void StopAndFocus () {
		_animator.SetBool("Walk", false);
		_animator.SetBool("Idle", true);
		_animator.SetBool("Focus",true);
	}
	public void StandIdle() {
		_animator.SetBool("Walk", false);
		_animator.SetBool("Focus",false);
		_animator.SetBool("BigHit",false);
		_animator.SetBool("Idle", true);
	}
	
	public void DoFocus() {
		_animator.SetBool("Idle", false);
	}
	public void DoBigHit() {
		_animator.SetBool("Idle", true);
		_animator.SetBool("BigHit", true);
		
	}
	public void StartWard() {
		_animator.SetBool("Ward", true);
	}
	public void StopWard() {
		_animator.SetBool("Ward", false);
		
	}
	
	public void StartTalking() {
		_animator.SetBool("listening", false);
		_animator.SetBool("talking", true);
		
	}
	
	public void StartListening() {
		_animator.SetBool("talking", false);
		_animator.SetBool("listening", true);
	}
	
	public void startYogaTalking() {
		_animator.SetBool("yogaListening", false);
		_animator.SetBool("yogaTalking", true); 
		
	}
	
	public void startYogaListening() {
		_animator.SetBool("yogaTalking", false);
		_animator.SetBool("yogaListening", true);
	}
	public void startYoga() {
		_animator.SetBool("yogaListening", false);
		_animator.SetBool("yogaTalking", false);
		_animator.SetBool("yoga", true);
	}

}
