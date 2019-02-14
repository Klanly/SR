using UnityEngine;
using System.Collections;

public class PortalHelper : MonoBehaviour {
	
	void Start()
	{
		
	}

	public void CheckBeachCampPortal()
	{
		GameManager.instance._levelManager.MultiPlayerAnimationCompleted();
	}
	
	public void SetAnimationStateToZero()
	{
		 foreach (AnimationState state in animation) {
         state.speed = 0.0f;
		//state.time=state.length;
       }
	}
	
	public void CallLevelManagerForAnimationFinished()
	{
		Debug.LogError("CallLevelManagerForAnimationFinished - PortalHelper "+gameObject.name);
		GameManager.instance._levelManager.ExecutePoiFunctionality();
	}
	
}
