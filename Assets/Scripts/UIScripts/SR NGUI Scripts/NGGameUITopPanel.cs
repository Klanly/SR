using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
public class NGGameUITopPanel : MonoBehaviour 
{
	public UISprite _gemsIcon;
	public UILabel _gemsNumber;
	
	public UISprite _healthIcon;
	public UILabel _healthNumber;
	
	public UISprite _soulsIcon;
	public UILabel _soulsNumber;
	
	public UISprite _pauseMenuRing;

	public UISprite pauseMenuHighlight;
	
	private GameUIInterface _listenerInterface;

	public void SetListener(GameUIInterface listenerInterface)
	{
		_listenerInterface = listenerInterface;
	}


	void Update() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			Debug.LogError("Input. touched "+touchDeltaPosition);
		}
	}

	public void UpdateHealth(int health)
	{
		User TempUser =GameManager._gameState.User;
		if(TempUser.life < TempUser.totalLife*0.3f)
		{
			UpdateHealth(health, true);
			SoundManager.instance.PlayHealthLowSound(true);
		}
		else
		{
			UpdateHealth(health, false);
			SoundManager.instance.PlayHealthLowSound(false);
		}
	}

	private Tweener playerHealthAlphaTween;

	public void UpdateHealth(int health, bool playHeartAnimation)
	{
		_healthNumber.text = health.ToString();
		
		if(playHeartAnimation)
		{
//			_healthIcon.GetComponent<dfTweenVector3>().Play();
			Debug.LogError("Playing health Icon animation");
			playerHealthAlphaTween = HOTween.To(_healthIcon, 1.0f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo).Id("heartGlow"));
			SoundManager.instance.PlayHealthLowSound(true);
		}
		else
		{
			SoundManager.instance.PlayHealthLowSound(false);
			if(playerHealthAlphaTween != null) {
				playerHealthAlphaTween.Kill();
			}
			_healthIcon.alpha = 1.0f;
			List<IHOTweenComponent> list = HOTween.GetTweensById("heartGlow", false);
			foreach (IHOTweenComponent ihot in list) {
				ihot.Kill();
			}

		}
	}

	bool IsSoulGemsUpdating = false;
	float tempSoulGemsAmount = 0;
	IEnumerator UpdateSoulGemsEnumerator;
	Tweener gemsTween = null;

	public void UpdateSoulGems(int soulGems)
	{
//		Debug.LogError("Update souls gems to "+soulGems);
		if(IsSoulGemsUpdating) {
			StopCoroutine(UpdateSoulGemsEnumerator);
			IsSoulGemsUpdating = false;
			_gemsNumber.text = tempSoulGemsAmount.ToString();
		}
		UpdateSoulGemsEnumerator = UpdateGemsCountAnimation(_gemsNumber, soulGems);
		StartCoroutine(UpdateSoulGemsEnumerator);
	}

	bool IsSoulDustUpdating = false;
	float tempSoulDustAmount = 0;
	IEnumerator UpdateSoulDustEnumerator;
	Tweener dustTween = null;
	
	public void UpdateSoulDust(float soulDust)
	{
//		Debug.LogError("Update souls dust to "+soulDust);
		if(IsSoulDustUpdating) {
			StopCoroutine(UpdateSoulDustEnumerator);
			IsSoulDustUpdating = false;
			_soulsNumber.text = tempSoulDustAmount.ToString();
		}
		UpdateSoulDustEnumerator = UpdateDustCountAnimation(_soulsNumber, soulDust);
		StartCoroutine(UpdateSoulDustEnumerator);
	}

	IEnumerator UpdateDustCountAnimation(UILabel label, float value) {
		IsSoulDustUpdating = true;
		tempSoulDustAmount = value;

		float waitDuration = 0.5f;
		int difference =  Mathf.Abs( int.Parse(label.text) - (int)value);
		if(difference == 0) {
			label.text = value.ToString();
			yield break;
		}
		int repeats = (difference);
		float interval = waitDuration / repeats;
		float tempInterval = interval;
		int increment = 1;
		
		while(tempInterval < 0.04f) {
			tempInterval = interval * Mathf.Pow(2, increment);
			increment++;
		}
		interval = tempInterval;
		if(increment > 0) {
			repeats = repeats/increment;
			while((repeats * interval) > 1.5f) {
				repeats /= 2;
				increment *= 2;
			}
			repeats++;
		}
		bool add = false;
		int count = int.Parse(label.text); 
		if(value > count) {
			add = true;
		}
//		Debug.LogError("repeats - "+repeats+" - interval - "+interval+" - count - "+count + " - increments - "+increment +" add "+add);
		while(repeats -- != 0) {
			if(add) {
				if(count + increment > value) {
					label.text = value.ToString();
					yield break;
				} else {
					count += increment;
					label.text = count.ToString();
				}
			} else {
				if(count - increment < value) {
					label.text = value.ToString();
					yield break;
				} else {
					count -= increment;
					label.text = count.ToString();
				}
			}

			_soulsIcon.transform.localScale = Vector3.one;
			dustTween = HOTween.To(_soulsIcon.transform, interval, new TweenParms().Prop("localScale", new Vector3(1.2f, 1.2f, 1.2f)).Loops(0, LoopType.Yoyo).Id("dustScale"));
			yield return new WaitForSeconds(interval);
		}
	}

	IEnumerator UpdateGemsCountAnimation(UILabel label, float value) {
		IsSoulGemsUpdating = true;
		tempSoulGemsAmount = value;
		
		float waitDuration = 0.5f;
		int difference =  Mathf.Abs( int.Parse(label.text) - (int)value);
		if(difference == 0) {
			label.text = value.ToString();
			yield break;
		}
		int repeats = (difference);
		float interval = waitDuration / repeats;
		float tempInterval = interval;
		int increment = 1;
		
		while(tempInterval < 0.04f) {
			tempInterval = interval * Mathf.Pow(2, increment);
			increment++;
		}
		interval = tempInterval;
		if(increment > 0) {
			repeats = repeats/increment;
			while((repeats * interval) > 1.5f) {
				repeats /= 2;
				increment *= 2;
			}
			repeats++;
		}
		bool add = false;
		int count = int.Parse(label.text); 
		if(value > count) {
			add = true;
		}
//		Debug.LogError("repeats - "+repeats+" - interval - "+interval+" - count - "+count + " - increments - "+increment +" add "+add);
		while(repeats -- != 0) {
			if(add) {
				if(count + increment > value) {
					label.text = value.ToString();
					yield break;
				} else {
					count += increment;
					label.text = count.ToString();
				}
			} else {
				if(count - increment < value) {
					label.text = value.ToString();
					yield break;
				} else {
					count -= increment;
					label.text = count.ToString();
				}
			}
			_gemsIcon.transform.localScale = Vector3.one;
			gemsTween = HOTween.To(_gemsIcon.transform, interval, new TweenParms().Prop("localScale", new Vector3(1.2f, 1.2f, 1.2f)).Loops(0, LoopType.Yoyo).Id("dustScale"));
			yield return new WaitForSeconds(interval);
		}
	}

	public void HideTopStats()
	{
		_gemsIcon.gameObject.SetActive(false);
		_gemsNumber.gameObject.SetActive(false);
		
		_healthIcon.gameObject.SetActive(false);
		_healthNumber.gameObject.SetActive(false);
		
		_soulsIcon.gameObject.SetActive(false);
		_soulsNumber.gameObject.SetActive(false);
	}
	
	public void ShowTopStats()
	{
		_gemsIcon.gameObject.SetActive(true);
		_gemsNumber.gameObject.SetActive(true);
		
		_healthIcon.gameObject.SetActive(true);
		_healthNumber.gameObject.SetActive(true);
		
		_soulsIcon.gameObject.SetActive(true);
		_soulsNumber.gameObject.SetActive(true);
	}

	#region UI Button clicks handling

	public void OnGemsClicked()
	{
		if(_listenerInterface != null)
		{
			_listenerInterface.onMarketButton();
		}
	}
	
	public void OnSoulsClicked()
	{
		if(_listenerInterface != null)
		{
			_listenerInterface.onMarketButton();
		}
	}
	
	public void OnHeartClicked()
	{
		if(_listenerInterface != null)
		{
			_listenerInterface.onHealthIcon();
		}
	}
	
	public void OnMenuRingClicked()
	{
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.RingTutorialComplete)
			TutorialManager.instance.DestroyRingTutorial();

		if(UIManager.instance.guildUI != null) {
			NGUITools.SetActive(UIManager.instance.guildUI.gameObject, false);
			UIManager.instance.guildUI.CloseChatSlider();
		}
		if(_listenerInterface != null)
			_listenerInterface.MenuButtonClick();

		SoundManager.instance.PlayMenuOpenSound();
		// Turn off the levelScene panel for Fast-forward tap and hold feature
		// Causes issues with Z index of some screens - e.g. Transumtation UI
		GameManager.instance.scaleformCamera.levelScene.levelScene.SetVisible(false);
	}

	#endregion
	/*
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == _gemsIcon || mouseEvent.Source == _gemsNumber)
		{
			Debug.Log("Gems clicked!");
			
			if(_listenerInterface != null)
			{
				_listenerInterface.onMarketButton();
				//_listenerInterface.MenuButtonClick();
			}
		}
		else if(mouseEvent.Source == _soulsIcon || mouseEvent.Source == _soulsNumber)
		{
			Debug.Log("Souls clicked!");
			
			if(_listenerInterface != null)
			{
				_listenerInterface.onMarketButton();
				//_listenerInterface.MenuButtonClick();
			}
		}
		else if(mouseEvent.Source == _healthIcon || mouseEvent.Source == _healthNumber)
		{
			Debug.Log("Health clicked!");
			
			if(_listenerInterface != null)
			{
				_listenerInterface.onHealthIcon();
				//_listenerInterface.MenuButtonClick();
			}
		}
		else if(mouseEvent.Source == _pauseMenuRing)
		{
			Debug.Log("Menu ring clicked!");
			
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.RingTutorialComplete)
				TutorialManager.instance.DestroyRingTutorial();
			
			if(_listenerInterface != null)
				_listenerInterface.MenuButtonClick();
		}
	}
	*/
	public void GlowMenuIcon()
	{
		pauseMenuHighlight.gameObject.SetActive(true);
	}
	
	public void RemoveGlowMenuIcon()
	{
		pauseMenuHighlight.gameObject.SetActive(false);
	}
	
	public void DisplayCenterButton(bool yesno)
	{
		_pauseMenuRing.gameObject.SetActive(yesno);
	}
	
	public void GlowRingIconInMenu()
	{
		
	}
	
	public void RemoveGlowRingIconInMenu()
	{
		
	}
	
	public void SetOnTop()
	{
		NGUITools.BringForward(gameObject);
	}

	public void ShowHeartIcon()
	{
		_healthIcon.gameObject.SetActive(true);
		_healthNumber.gameObject.SetActive(true);
	}

	public void HideHeartIcon()
	{
		_healthIcon.gameObject.SetActive(false);
		_healthNumber.gameObject.SetActive(false);
	}
}
