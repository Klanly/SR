using UnityEngine;
using System.Collections;

public class AnimationCBForwarder : MonoBehaviour {

	public void CallLevelManagerFromAnimator()
	{
		Debug.LogError("CallLevelManagerForAnimationFinished - AnimationCBForwarder "+gameObject.name);
		GameManager.instance._levelManager.ExecutePoiFunctionality();
	}
	
}
