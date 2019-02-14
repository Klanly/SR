using UnityEngine;
using System;
using System.Collections;

public class NGPotionPopup : MonoBehaviour
{
	
    private int _count;
    private int _cost;
    private Action _onUsePotionCB;
    private Action _onCloseCB;
    private Action _onBuyButtonCB;

    public UILabel _currentAmountLabel;
    public UILabel _costAmountLabel;

	public UIButton _buyButton;
	
    public void Show(int count, int cost, Action onUsePotionCB, Action onBuyButtonCB, Action onCloseCB)
    {
        _count = count;
        _cost = cost;
        _onUsePotionCB = onUsePotionCB;
        _onBuyButtonCB = onBuyButtonCB;
        _onCloseCB = onCloseCB;
		
        _currentAmountLabel.text = "x " + _count;
        _costAmountLabel.text = _cost.ToString();

		if(GameManager._gameState.User._inventory.potionBelt.level >= GameManager._dataBank.GetMaxLevelPotionBelt ().level
		   && count >= GameManager._dataBank.GetMaxLevelPotionBelt ().capacity) {
			NGUITools.SetActive(_buyButton.gameObject, false);
		}
    }


	#region NGUI interface callbacks

    public void OnUsePotion()
    {
        if (GameManager._gameState.User.life != GameManager._gameState.User.totalLife) {
            _onUsePotionCB();
			SoundManager.instance.PlayMenuOkSound();
		}
        Destroy(gameObject);
        InputWrapper.disableTouch = false;
    }

    public void OnBuyButton()
    {
        _onBuyButtonCB();

        _currentAmountLabel.text = "x " + GameManager._gameState.User._inventory.potionBelt.Count();
		if(GameManager._gameState.User._inventory.potionBelt.Count() >= GameManager._dataBank.GetMaxLevelPotionBelt ().capacity) {
			NGUITools.SetActive(_buyButton.gameObject, false);
		}
	}

    public void OnBackPanel()
    {
//		Debug.LogError("current tutorial - "+TutorialManager.instance.currentTutorial.ToString());

		if(TutorialManager.instance.currentTutorial != TutorialManager.TutorialsAndCallback.None)
			return;

        _onCloseCB();
        Destroy(gameObject);
    }
	#endregion

//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(mouseEvent.Source == _mainSprite)
//		{
//			Debug.Log("MAIN SPRITE!!! :D");
//			_onUsePotionCB();
//			mouseEvent.Use();
//			Destroy(gameObject);
//		}
//		else if(mouseEvent.Source == _buyButton)
//		{
//			_onBuyButtonCB();
//			mouseEvent.Use();
//		}
//		else if(mouseEvent.Source == _backgroundSprite)
//		{
//			if(!mouseEvent.Used)
//			{
//				_onCloseCB();
//				Destroy(gameObject);
//			}
//		}
//	}
}
