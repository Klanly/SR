using UnityEngine;
using System;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
	private Action _callback;
	private Action _onShowCB;
	
	public TutorialManager.TutorialsAndCallback _tutorialId;
	
	private CallbackMech _cbMech;
	public enum CallbackMech
	{
		auto,
		manual
	}
	
	public void Show(TutorialManager.TutorialsAndCallback tutorialId, CallbackMech _cbMech, Action callBack, Action onShowCB = null)
	{
		_onShowCB = onShowCB;
		_tutorialId = tutorialId;
		this._cbMech = _cbMech;
		_callback = callBack;
		
		//gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	public void Start()
	{
		if(_onShowCB != null)
		{
			_onShowCB();
			_onShowCB = null;
		}

//		if(_tutorialId.ToString().Contains("BattleEnd", true) && _tutorialId != TutorialManager.TutorialsAndCallback.BattleEndTutorialStart) {
		NGUITools.BringForward(gameObject);
//		}
	}
	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(_cbMech == CallbackMech.auto)
//			Tutorial.Destroy(this, true);
//	}

	public void ngOnClick(UIButton button)
	{
		Debug.Log("button > " + button.gameObject.name);
		SoundManager.instance.PlayMenuCancelSound();

		if(_cbMech == CallbackMech.auto)
			Tutorial.Destroy(this, true);
	}

	public static void Destroy(Tutorial tutorial, bool chainDestroy = false)
	{
		if(tutorial._callback != null)
		{
			tutorial._callback();
			tutorial._callback = null;
		}
		
		if(tutorial != null && chainDestroy)
			GameObject.Destroy(tutorial.gameObject);
	}
	
	public void OnDestroy()
	{
		Tutorial.Destroy(this);
	}
	
}
