using UnityEngine;
using System;
using System.Collections;

public class NGTwoButtonPopup : MonoBehaviour {
	
	public UILabel yesButtonLabel;
	public UILabel noButtonLabel;
	
	public UILabel heading;
	public UILabel message;
	
	private Action _button1CB;
	private Action _button2CB;
	
	public void showGeneralPopup2(string title, string message, string btn1Text, Action button1CB, string btn2Text, Action button2CB )
	{
		heading.text = title;
		this.message.text = message;
		yesButtonLabel.text = btn1Text;
		_button1CB = button1CB;
		noButtonLabel.text = btn2Text;
		_button2CB = button2CB;
	}
	
	public void showGeneralPopup2(string title, string message, int numberVal, string btn1Text, Action button1CB, string btn2Text, Action button2CB )
	{
		heading.text = title;
		this.message.text = "Need " + numberVal + " more " + message;
		yesButtonLabel.text = btn1Text;
		_button1CB = button1CB;
		noButtonLabel.text = btn2Text;
		_button2CB = button2CB;
	}
	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		bool destroy = false;
//		
//		if(mouseEvent.Source == yesButton)
//		{
//			if(_button1CB != null)
//				_button1CB();
//			destroy = true;
//		}
//		else if(mouseEvent.Source == noButton)
//		{
//			if(_button2CB != null)
//				_button2CB();
//			destroy = true;
//		}
//		
//		if(destroy)
//			Destroy(gameObject);
//	}

	#region UI Interface click method
	public void OnYesButton()
	{
		if(_button1CB != null)
			_button1CB();
		SoundManager.instance.PlayMenuOkSound();

		Destroy(gameObject);
	}

	public void OnNoButton()
	{
		if(_button2CB != null)
			_button2CB();
		SoundManager.instance.PlayMenuCancelSound();

		Destroy(gameObject);
	}
	#endregion
	
}
