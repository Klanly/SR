using UnityEngine;
using System;
using System.Collections;

public class TwoButtonPopup : MonoBehaviour {

	public dfButton yesButton;
	public dfButton noButton;
	
	public dfLabel heading;
	public dfLabel message;
	
	private Action _button1CB;
	private Action _button2CB;
	
	public void showGeneralPopup2(string title, string message, string btn1Text, Action button1CB, string btn2Text, Action button2CB )
	{
		heading.Text = title;
		this.message.Text = message;
		yesButton.Text = btn1Text;
		_button1CB = button1CB;
		noButton.Text = btn2Text;
		_button2CB = button2CB;
	}
	
	public void showGeneralPopup2(string title, string message, int numberVal, string btn1Text, Action button1CB, string btn2Text, Action button2CB )
	{
		heading.Text = title;
		this.message.Text = "Need " + numberVal + "more  ::: " + message;
		yesButton.Text = btn1Text;
		_button1CB = button1CB;
		noButton.Text = btn2Text;
		_button2CB = button2CB;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		bool destroy = false;
		
		if(mouseEvent.Source == yesButton)
		{
			if(_button1CB != null)
				_button1CB();
			destroy = true;
		}
		else if(mouseEvent.Source == noButton)
		{
			if(_button2CB != null)
				_button2CB();
			destroy = true;
		}
		
		if(destroy)
			Destroy(gameObject);
	}

}
