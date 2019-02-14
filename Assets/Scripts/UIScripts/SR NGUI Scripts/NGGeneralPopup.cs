using UnityEngine;
using System;
using System.Collections;

public class NGGeneralPopup : MonoBehaviour {
	
	public UILabel heading;
	public UILabel message;
	
	private Action popupCB;
	
	public void showPopup(string heading, string message, Action popupCB)
	{
		this.message.text = message;
		this.heading.text = heading;
	
		this.popupCB = popupCB;
	}
	
	public void OnClick()
	{
		if(popupCB != null)
			popupCB();
		
		GameObject.Destroy(gameObject);
	}

}

