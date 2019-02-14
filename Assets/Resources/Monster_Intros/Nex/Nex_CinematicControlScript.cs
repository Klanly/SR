using UnityEngine;
using System.Collections;

public class Nex_CinematicControlScript : MonoBehaviour {

	Animator _animator;
	private AnimatorStateInfo currentBaseState;
	
	private int _currentState;
	public GameObject mesh1;

	private Material m;
	
	public void ResetStates()
	{
		//Damaged = false;
	}
	void Start() {
		
		m = mesh1.renderer.sharedMaterial;
		m.SetFloat("_Cutoff",0.0f);
	}
	
	void Update()
	{
		//_currentState = _animator.GetCurrentAnimatorStateInfo(0).nameHash;
		
	}
	
	void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
	}
	
	public void startTalking() {
		_animator.SetBool("Listening", false);
		_animator.SetBool("Talking", true);
		
	}
	
	public void startListening() {
		_animator.SetBool("Talking", false);
		_animator.SetBool("Listening", true);
	}
	
	
	public void StopAndFocus () {
		_animator.SetBool("Walk", false);
		_animator.SetBool("Idle", true);
		_animator.SetBool("Focus",true);
	}
	public void StandIdle() {
		_animator.SetBool("Idle", true);
		_animator.SetBool("CrushEnergyBeam", false);
	}

	public void CrushEnergyBeam() {
		//_animator.SetBool("Idle", false);
		_animator.SetBool("CrushEnergyBeam", true);
		//Invoke("StandIdle",1.0f);
	}
	
	public void StopCrushBeam(){
		_animator.SetBool("CrushEnergyBeam", false);
	}
	
	
	public void DoBigHit() {
		_animator.SetBool("Idle", true);
		_animator.SetBool("DieHard", true);
		
	}
	public void DieHard() {
		//_animator.SetBool("Idle", false);
		_animator.SetBool("DieHard", true);
	}
	public void DieHard2() {
		_animator.SetBool("Idle", true);
		_animator.SetBool("DieHard2", true);
		
	}
	void animateVisibilityViaAlpha() {
		StartCoroutine(disAppearNex());
	}
	IEnumerator disAppearNex()
	{
		float temp=0.0f;
		while(temp<=1.0f)
		{
			m.SetFloat("_Cutoff",temp);
			temp+=0.005f;
			yield return new WaitForSeconds(0.015f);
		}
		yield return null;
	}
	void fadeOutScene() {
		PlayMakerFSM.BroadcastEvent("lmFadeOut");
		Debug.LogWarning("fadeOutScene - Nex_Cine");
	}
}
