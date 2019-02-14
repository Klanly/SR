using UnityEngine;
using System;
using System.Collections;

public class NGShrinePopup : MonoBehaviour
{
    public UILabel _elementLabel;
    public UILabel _shrineLevelLabel;
    public UILabel _chargePointsLabel;
    public UILabel _timerLabel;
	public UILabel _lockedLabel;
	public UIButton _CollectButton;
    public UIProgressBar _BarFill;

    public UIButton _backPanel;
	
    private Action _chargeCallback;
    private Action _collectCallback;
	
    private int shrineChargePoints = 0;
	private int shrineMaxChargePoints = 0;
	private int shrineLevel = 0;

	private bool isLocked = false;
	private bool chargeClicked = false;

	public void Init(string element, int shrineLevel, int chargePoints, int maxChargePoints, long timmer, bool isCharged, bool isLocked, Action chargeCallback, Action collectCallback)
    {
        _elementLabel.text = element;
        _chargePointsLabel.text = chargePoints+"/"+maxChargePoints;
        SetShrineTimer(timmer);
        SetShrineLevel(shrineLevel);
        shrineChargePoints = chargePoints;
		shrineMaxChargePoints = maxChargePoints;
		this.shrineLevel = shrineLevel;
		SetBarFill(shrineChargePoints, shrineMaxChargePoints);
        _chargeCallback = chargeCallback;
        _collectCallback = collectCallback;
		this.isLocked = isLocked;
		_lockedLabel.gameObject.SetActive(isLocked);
		chargeClicked = isCharged;
		if(isLocked) {
			_CollectButton.gameObject.SetActive(false);
			chargeClicked = isLocked;
		} else if(shrineLevel > 1) {
			_CollectButton.gameObject.SetActive(true);
//			chargeClicked = true;
		} else if(shrineChargePoints >= shrineMaxChargePoints) {
			_CollectButton.gameObject.SetActive(true);
//			chargeClicked = true;
		}
//		_CollectButton.gameObject.SetActive(shrineChargePoints >= shrineMaxChargePoints || (!isLocked && shrineLevel > 1));
//		if(isLocked) {
//			chargeClicked = isLocked;
//		}
		GameManager.instance.scaleformCamera.generalSwf.HideUILoadingScreen(true);
	}

	#region UI clicks CB
    public void OnChargeShrineButton()
    {

		if(isLocked) {
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Shrine Locked", "", () => {});
			return;
		}
        
		if (chargeClicked) {
			TutorialManager.instance.ShrineChargeErrorTutorial();
			SoundManager.instance.PlayMenuCancelSound();
			return;
		}

        if (shrineChargePoints < shrineMaxChargePoints)
        {
			shrineChargePoints++;
            _chargePointsLabel.text = shrineChargePoints.ToString();
			SetBarFill(shrineChargePoints, shrineMaxChargePoints);
			if (_chargeCallback != null) {
				_chargeCallback();
			}
			SoundManager.instance.PlayMenuOkSound();

            StartCoroutine("UpdateBarFill");
        }

        if (shrineChargePoints >= shrineMaxChargePoints)
        {
			if (!_CollectButton.gameObject.activeInHierarchy)
            {
				_CollectButton.gameObject.SetActive(true);
            }
        }
		chargeClicked = true;
    }

    public void OnCollectButton()
    {
		UIManager.instance.generalSwf.generalSwf.showGeneralPopup4("Shrine Loot", "Would you like to collect your reward", "Yes", "No", () => {
			if (_collectCallback != null)
				_collectCallback();
			
			SoundManager.instance.PlayTreasureLootSound();
			Destroy(gameObject);

		}, () => {});

//		UnityEngine.Object asset = Resources.Load(NGGameUI._TwoButtonPopupPath);
//		NGTwoButtonPopup popup = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<NGTwoButtonPopup>();
//		popup.showGeneralPopup2("Shrine Loot", "Would you like to collect your reward", "Yes", () => {
//			if (_collectCallback != null)
//				_collectCallback();
//			
//			SoundManager.instance.PlayTreasureLootSound();
//			Destroy(gameObject);
//		}, "No", () => {});
    }

    public void OnBackgroundPanelClicked()
    {

		if(TutorialManager.instance.currentTutorial != null)
			Debug.LogWarning("Current Tutorial - "+TutorialManager.instance.currentTutorial.ToString());
    
		if (TutorialManager.instance.currentTutorial.ToString().ToLower().Contains("shrine"))
        {
			Debug.LogError("Shrine tutorial is on");
            return;
        }

		if(!chargeClicked) {
			return;
		}
		
//		if(_CollectButton.isActiveAndEnabled) {
//			return;
//		}

		if(!_CollectButton.isActiveAndEnabled) {
			UIManager.instance.generalSwf.CloseShrinePopUp();
			Destroy(gameObject);
			return;
		}


		UIManager.instance.generalSwf.CloseShrinePopUp();
        Destroy(gameObject);
    }

    public void OnQuestionButtonClicked()
    {
		TutorialManager.instance.ShrineHelpTutorial();
		SoundManager.instance.PlayMenuOpenSound();
    }
	#endregion
	
    public void SetShrineTimer(long timerValue)
    {
		long time = Helpers.GetTimeDifferenceInSeconds(timerValue, 0);
//		long time = timerValue;
		int timeInt = (int)time;
		_timerLabel.text = Upgradeui.ConvertTime (timeInt);
		StopCoroutine("StartCountdown");
		StartCoroutine("StartCountdown", (timeInt));
    }


	IEnumerator StartCountdown(int time) {

		if(time <= 0) {
			Debug.LogError("Most likely shrine tutorial");
			yield break;
		}

		float deltaTime = 0;
		while(time >= 0) {
			if(_timerLabel != null) {
				while(deltaTime <= 1.0f) {
					deltaTime += Time.unscaledDeltaTime;
					yield return new WaitForEndOfFrame();
				}
				_timerLabel.text = Upgradeui.ConvertTime (time--);
				deltaTime = 0.0f;
			}
			yield return null;
		}
		Time.timeScale = 1.0f;
//		ServerManager.Instance.GetShrineInformation(ShrineManager.Instance.GetShrineInformationHandler);
//		GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);

		UIManager.instance.generalSwf.CloseShrinePopUp();
		Destroy(gameObject);

	}

	public void SetShrineLevel(int level)
    {
        if (level != 0)
        {
            _shrineLevelLabel.text = "Level " + level.ToString();
        }
    }
	
    public void SetBarFill(int chargePoints, int maxChargePoints)
    {
        float fillAmount = Mathf.Min(chargePoints, maxChargePoints) / (float)maxChargePoints;
        _BarFill.value = fillAmount;
		_chargePointsLabel.text = chargePoints+"/"+maxChargePoints;

//        if (chargePoints == maxChargePoints)
		Debug.LogError("points - "+shrineLevel+" charge "+chargePoints+" max - "+maxChargePoints);
		if(chargePoints >= maxChargePoints)
            _CollectButton.gameObject.SetActive(true);
    }
	
    IEnumerator UpdateBarFill()
    {
        float delay = 0.0001f;
        for (int i=0; i < 100; i++)
        {
            _BarFill.value += 0.001f;
            yield return new WaitForSeconds(delay);
        }
	}
	
	public void OnDestroy()
	{
		UIManager.instance.generalSwf.CloseShrinePopUp();
    }
}
