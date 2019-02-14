using UnityEngine;
using System;
using System.Collections;

public class MainScreenMenu : MonoBehaviour {

	public dfButton _singleplayerButton;
	public dfButton _multiplayerButton;
	
	private Action _singleplayerCB;
	private Action _multiplayerCB;

	void Start()
	{
		gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	public void Show(Action singleplayerCB, Action multiplayerCB)
	{
		_singleplayerCB = singleplayerCB;
		_multiplayerCB = multiplayerCB;
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == _singleplayerButton)
			_singleplayerCB();
		else if(mouseEvent.Source == _multiplayerButton)
			_multiplayerCB();
		
		Destroy(gameObject);
	}
}
