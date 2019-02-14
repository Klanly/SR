using UnityEngine;
using System;
using System.Collections;

public class PotionPopup : MonoBehaviour {
	
	private int _count;
	private int _cost;
	private Action _onUsePotionCB;
	private Action _onCloseCB;
	private Action _onBuyButtonCB;
	
	public dfSprite _mainSprite;
	public dfButton _buyButton;
	public dfLabel _currentAmountLabel;
	public dfLabel _costAmountLabel;
	public dfSprite _backgroundSprite;
	
	public void Show(int count, int cost, Action onUsePotionCB, Action onBuyButtonCB, Action onCloseCB)
	{
		_count = count;
		_cost = cost;
		_onUsePotionCB = onUsePotionCB;
		_onBuyButtonCB = onBuyButtonCB;
		_onCloseCB = onCloseCB;
		
		_currentAmountLabel.Text = _count + "";
		_costAmountLabel.Text = _cost + "";
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == _mainSprite)
		{
			Debug.Log("MAIN SPRITE!!! :D");
			_onUsePotionCB();
			mouseEvent.Use();
			Destroy(gameObject);
		}
		else if(mouseEvent.Source == _buyButton)
		{
			_onBuyButtonCB();
			mouseEvent.Use();
		}
		else if(mouseEvent.Source == _backgroundSprite)
		{
			if(!mouseEvent.Used)
			{
				_onCloseCB();
				Destroy(gameObject);
			}
		}
	}
}
