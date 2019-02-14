using UnityEngine;
using System;
using System.Collections;

public class NGBossDefeatPopup : MonoBehaviour 
{
	public Action backToCamp;
	
	
	public void OnClick()
	{
		GoBackToCamp();
	}
	
	void GoBackToCamp()
	{
		if(backToCamp != null)
			backToCamp();
		
		GameObject.Destroy(gameObject);
	}
	
	public void SetCallbacks(Action backToCamp)
	{
		this.backToCamp = backToCamp;
		
		this.PerformActionWithDelay(3.0f, ()=> {
			GoBackToCamp();
		}
		);
	}
}
