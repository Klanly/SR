using UnityEngine;
using System;
using System.Collections;

public class NGOneButtonPopup : MonoBehaviour {
	
	public UILabel yesButtonLabel;
	public UILabel message;
	
	private Action button1CB;
	
	public void showPopup(string message, string yesButtonText, Action button1CB)
	{
		this.message.text = message;
		yesButtonLabel.text = yesButtonText;
		this.button1CB = button1CB;
	}
	
	public void OnClick()
	{
		if(button1CB != null)
			button1CB();

		SoundManager.instance.PlayMenuOkSound();

		GameObject.Destroy(gameObject);
	}
}

