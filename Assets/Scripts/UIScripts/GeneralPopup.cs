using UnityEngine;
using System;
using System.Collections;

public class GeneralPopup : MonoBehaviour {
	
	public dfPanel popupPanel;
	
	public dfLabel heading;
	public dfLabel message;
	
	private Action popupCB;
	
	public void showPopup(string heading, string message, Action popupCB)
	{
		this.message.Text = message;
		this.heading.Text = heading;
		
		this.popupCB = popupCB;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == popupPanel)
		{
			if(popupCB != null)
				popupCB();
			
			GameObject.Destroy(gameObject);
		}
	}
}

