using UnityEngine;
using System.Collections;

public class AutoPlaySequence : MonoBehaviour 
{
	public USSequencer sequence = null;
	public float delay = 1.0f;
	
	private float currentTime = 0.0f;
	private bool hasPlayed = false;
	public bool isAutoPlay = false;
	
	// Use this for initialization
	void Start() 
	{
		if(!sequence)
		{
			Debug.LogError("You have added an AutoPlaySequence, however you haven't assigned it a sequence", gameObject);
			return;
		}
	}
	
	void Update()
	{
		if(!isAutoPlay)
			return;
		
		if(hasPlayed)
			return;
		
		currentTime += Time.deltaTime;
			
		if(currentTime >= delay && sequence)
		{
			sequence.Play();
			hasPlayed = true;
		}
	}
	
	public void PlaySequence(){
		sequence.Play();
		hasPlayed = true;
	}
	
	public void OnAnimationFinishedIntro(){
		Debug.Log("Intro Sequence Finished!!");
		GameManager.instance._levelManager.TransitionToBattleAfterCinematic();
	}
	
	public void OnAnimationFinishedOutro(){
		Debug.Log("Outro Sequence Finished!!");
		StartCoroutine(GameManager.instance._levelManager.battleManager.WaitForEndAnimationAndReturn(3.0f,true));
		//GameManager.instance._levelManager.TransitiionToBattleAfterCinematic();
	}
}
