using UnityEngine;
using System;
using System.Collections;

public static class MonoExtensions
{
	
	//=============================================================================================================
	//	SAMPLE USAGE : (CAN ONLY BE CALLED FROM MONOBEHAVIOR)
	//	this.PerformActionWithDelay(1.3f, () => {	Debug.Log("Executed after 1.3 seconds!");	}
	//
	public static void PerformActionWithDelay(this MonoBehaviour mono, float delay, Action action, Action callback = null)
	{
		mono.StartCoroutine(mono.PerformActionWithDelayRoutine(delay, action, callback));
	}

	private static IEnumerator PerformActionWithDelayRoutine(this MonoBehaviour ienum, float delay, Action action, Action callback)
	{
		yield return new WaitForSeconds(delay);

		action();

		if(callback != null)
			callback();
	}
	//=============================================================================================================
	
	
	
	//=============================================================================================================
	// This is a "Daikon Forge" only extension! Remove this method if you are not using Daikon forge
	//
//	public static void PerformDFLayout(this MonoBehaviour mono)
//	{
//		dfPanel daikonPanel = mono.GetComponent<dfPanel>();
//		if(daikonPanel != null)
//			daikonPanel.PerformLayout();
//	}
	//=============================================================================================================
}
