using UnityEngine; 
using System;
using System.Collections;

public class ShrineTextPopup: MonoBehaviour {
	
	public UILabel _message;
	
	private Action button1CB;
	
	public void showPopup(string message, Action button1CB)
	{
		_message.text = message;
		this.button1CB = button1CB;
	}
	

	public void ngOnClick(UIButton button)
	{
		if(button1CB != null)
			button1CB();
		
		GameObject.Destroy(gameObject);
	}
}
