using UnityEngine;
using System;
using System.Collections;

public class NGMainScreenMenu : MonoBehaviour {
	
	private Action _singleplayerCB;
	private Action _multiplayerCB;

	
	public void Show(Action singleplayerCB, Action multiplayerCB)
	{
		_singleplayerCB = singleplayerCB;
		_multiplayerCB = multiplayerCB;
	}
	
	public void OnSinglePlayerClick()
	{
//		GameManager.instance.fragNetworkingNew.Connect();
//		return;
		_singleplayerCB();
		SoundManager.instance.PlayMenuOpenSound();
		Destroy(gameObject);
	}

	public void OnMultiPlayerClick()
	{
//		if(AMQP.Client.IsConnected)
//			AMQP.Client.Disconnect();
//		else
//			GameManager.instance.fragNetworkingNew.Connect();
//			return;

		_multiplayerCB();
		SoundManager.instance.PlayMenuOpenSound();
		Destroy(gameObject);
	}
}
