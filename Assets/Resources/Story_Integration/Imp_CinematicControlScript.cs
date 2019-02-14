using UnityEngine;
using System.Collections;

public class Imp_CinematicControlScript : MonoBehaviour
{
	Animator _animator;
	private AnimatorStateInfo currentBaseState;
	
	private int _currentState;
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
	
	void Start()
	{

	}
	
	public void StartTalking() {
		_animator.SetBool("listening", false);
		_animator.SetBool("talking", true);
		
	}
	
	public void StartListening() {
		_animator.SetBool("talking", false);
		_animator.SetBool("listening", true);
	}
	
	


}
