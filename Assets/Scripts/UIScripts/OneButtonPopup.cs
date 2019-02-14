using UnityEngine;
using System;
using System.Collections;

public class OneButtonPopup : MonoBehaviour {

	public dfButton yesButton;
	public dfLabel message;
	
	private Action button1CB;
	
	public void showPopup(string message, string yesButtonText, Action button1CB)
	{
		this.message.Text = message;
		yesButton.Text = yesButtonText;
		this.button1CB = button1CB;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == yesButton)
		{
			if(button1CB != null)
				button1CB();

			SoundManager.instance.PlayMenuOkSound();

			GameObject.Destroy(gameObject);
		}
	}
}

