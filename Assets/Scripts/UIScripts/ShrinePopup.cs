using UnityEngine;
using System;
using System.Collections;

public class ShrinePopup : MonoBehaviour 
{
//	public dfLabel _elementLabel;
//	public dfLabel _shrineLevelLabel;
//	public dfLabel _chargePointsLabel;
//	public dfLabel _timerLabel;
//	public dfButton _CollectButton;
//	public dfSlicedSprite _BarFill;
//	
//	public dfPanel _backPanel;
//	
//	private Action _chargeCallback;
//	private Action _collectCallback;
//	
//	private int shrineChargePoints = 0;
//	private int shrineMaxChargePoints = 0;
//
//	public void Init(string element,int shrineLevel,int chargePoints,int maxChargePoints,long timmer,bool chargeButtonState, Action chargeCallback, Action collectCallback)
//	{
//		_elementLabel.Text = element;
//		_chargePointsLabel.Text = chargePoints.ToString();
//		SetShrineTimer(timmer);
//		SetShrineLevel(shrineLevel);
//		shrineChargePoints = chargePoints;
//		shrineMaxChargePoints = maxChargePoints;
//		SetBarFill(shrineChargePoints,shrineMaxChargePoints);
//		_chargeCallback = chargeCallback;
//		_collectCallback = collectCallback;
//		
//		this.PerformDFLayout();
//	}
//	
//	
//	private bool chargeClicked = false;
//	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(mouseEvent.Source.name.Equals("ChargeShrineButton"))
//		{
//			if(chargeClicked)
//				return;
//			if(shrineChargePoints < shrineMaxChargePoints)
//			{
//				shrineChargePoints++;
//				_chargePointsLabel.Text = shrineChargePoints.ToString();
//				if(_chargeCallback != null)
//					_chargeCallback();
//				StartCoroutine("UpdateBarFill");
//			}
//			
//			if(shrineChargePoints >= shrineMaxChargePoints)
//			{
//				if(!_CollectButton.gameObject.activeInHierarchy)
//				{
//					_CollectButton.gameObject.SetActive(true);
//				}
//			}
//			
//			chargeClicked = true;
//		}
//		else if(mouseEvent.Source.name.Equals("CollectButton"))
//		{
//			if(_collectCallback != null)
//				_collectCallback();
//			
//			Destroy(gameObject);
//		}
//		else if(mouseEvent.Source == _backPanel)
//		{
//			if(TutorialManager.instance.currentTutorial != TutorialManager.TutorialsAndCallback.None && TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted))
//			{
//				UIManager.instance.generalSwf.CloseShrinePopUp();
//				Destroy(gameObject);
//			}
//			else
//				Destroy(gameObject);
//		}
//	}
//
//	public void SetShrineTimer(long timerValue)
//	{
//		float timeVal = (float)timerValue;
//		int remainingHours = Mathf.FloorToInt(timeVal/3600);
//		int remainingMinutes = Mathf.FloorToInt(( timeVal - (remainingHours*3600) ) / 60);
//		int remainingSeconds = Mathf.FloorToInt(timeVal - (remainingHours*3600) - (remainingMinutes*60) );
//
//		if(remainingHours >= 1)
//			_timerLabel.Text = remainingHours + "h " + (remainingMinutes > 9 ? "": "0") +(remainingMinutes) + "m";
//		else
//			_timerLabel.Text = (remainingMinutes > 9 ? "": "0") +(remainingMinutes) + "m " + (remainingSeconds > 9 ? "": "0") +(remainingSeconds) + "s";
//
//	}
//
//	public void SetShrineLevel(int level)
//	{
//		if(level!=0)
//		{
//			_shrineLevelLabel.Text = "Level " + level.ToString();
//		}
//	}
//
//	public void SetBarFill(int chargePoints,int maxChargePoints)
//	{
//		float fillAmount =  chargePoints/(float) maxChargePoints;
//		_BarFill.FillAmount = fillAmount;
//		
//		if(chargePoints == maxChargePoints)
//			_CollectButton.gameObject.SetActive(true);
//	}
//
//	IEnumerator UpdateBarFill()
//	{
//		float delay = 0.0001f;
//		for(int i=0; i < 100 ; i++)
//		{
//			_BarFill.FillAmount += 0.001f;
//			yield return new WaitForSeconds(delay);
//		}
//	}
//	
//	public void OnDestroy()
//	{
//		UIManager.instance.generalSwf.CloseShrinePopUp();
//	}
}
