using UnityEngine;
using System.Collections;

public class DisableNonUITouches : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		InputWrapper.disableTouch = true;
	}
	
	void OnDestroy()
	{
		InputWrapper.disableTouch = false;
	}
}
