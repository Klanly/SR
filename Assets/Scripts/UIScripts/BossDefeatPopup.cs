using UnityEngine;
using System;
using System.Collections;

public class BossDefeatPopup : MonoBehaviour 
{
	public Action backToCamp;
	

	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
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
		this.GetComponent<dfPanel>().PerformLayout();

		this.PerformActionWithDelay(3.0f, ()=> {
			GoBackToCamp();
		}
		);
	}
}
