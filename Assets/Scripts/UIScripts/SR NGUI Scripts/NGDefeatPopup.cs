using System;
using UnityEngine;
using System.Collections;

public class NGDefeatPopup : MonoBehaviour {
	
//	public dfButton campButton;
//	public dfButton restartButton;
//	
	public Action campButtonCB;
	public Action restartButtonCB;
	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		bool destroy = false;
//		
//		if(mouseEvent.Source == campButton)
//		{
//			if(campButtonCB != null)
//				campButtonCB();
//			
//			destroy = true;
//		}
//		else if(mouseEvent.Source == restartButton)
//		{
//			if(restartButtonCB != null)
//				restartButtonCB();
//			
//			destroy = true;
//		}
//		
//		if(destroy)
//			GameObject.Destroy(gameObject);
//	}
	
	public void SetCallbacks(Action campButtonCB, Action restartButtonCB)
	{
		this.campButtonCB = campButtonCB;
		this.restartButtonCB = restartButtonCB;
	}

	#region Ngui interface buttons callbacks

	public void OnCampButton()
	{
		if(campButtonCB != null)
			campButtonCB();

		GameObject.Destroy(gameObject);
	}

	public void OnRestartButton()
	{
		if(restartButtonCB != null)
			restartButtonCB();

		GameObject.Destroy(gameObject);
	}
	#endregion
}
