using UnityEngine;
using System;
using System.Collections;

public class NGCutsceneDialogUI : MonoBehaviour {
	
	public UILabel _actorLabel;
	public UILabel _contentLabel;
	
	private Action _nextButtonCB;
	private Action _skipButtonCB;
	
	public void Show(Action nextButtonCB, Action skipButtonCB)
	{
		_nextButtonCB = nextButtonCB;
		_skipButtonCB = skipButtonCB;
	}
	
	public void UpdateDialog(string actorName, string dialogContent)
	{
		_actorLabel.text = actorName;
		_contentLabel.text = dialogContent;
	}

	#region public NGUI interface callbacks
	public void OnNextButton()
	{
		_nextButtonCB();
	}

	public void OnSkipButton()
	{
		_skipButtonCB();
	}
	#endregion
}
