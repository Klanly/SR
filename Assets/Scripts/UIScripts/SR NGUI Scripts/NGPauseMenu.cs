using UnityEngine;
using System;
using System.Collections;

public class NGPauseMenu : MonoBehaviour 
{
	public UIButton resumeButton;
	public UIButton ringsButton;
	public UIButton staffButton;
	public UIButton spiritsButton;
	public UIButton transmutationButton;
	public UIButton upgradesButton;
	public UIButton marketButton;
	public UIButton optionsButton;
	
	private bool staffUnlocked = false;
	private bool transmutationUnlocked = false;
	private bool spiritUnlocked = false;
	private bool upgradesUnlocked = false;

	void Start()
	{
		Debug.Log("TutorialManager.instance.state >> " + TutorialManager.instance.state);
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.BattleEndTutorial2)
		{
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.BattleEndTutorialComplete);
			GlowRingIconInMenu();
		}
	}

	public void Update()
	{
		int dayCount = GameManager._gameState.dayCount ;

		if(dayCount < GameManager.UNLOCK_UPGRADES_DAY)
			upgradesButton.SetState(UIButtonColor.State.Hover, true);
		else
			upgradesUnlocked = true;
		
		if(dayCount < GameManager.UNLOCK_RUNE_DAY)
			staffButton.SetState(UIButtonColor.State.Hover, true);
		else
			staffUnlocked = true;
		
		if(dayCount < GameManager.UNLOCK_TRANSMUTATION_DAY)
			transmutationButton.SetState(UIButtonColor.State.Hover, true);
		else
			transmutationUnlocked = true;
		
		if(dayCount < GameManager.UNLOCK_SPIRITS_DAY)
			spiritsButton.SetState(UIButtonColor.State.Hover, true);
		else
			spiritUnlocked = true;
	}

	private GameUIInterface _listenerInterface;
	public void SetListener(GameUIInterface listenerInterface)
	{
		_listenerInterface = listenerInterface;
	}
	
	public void OnClick(UIButton button)
	{
		bool inTutorial = false;
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.BattleEndTutorialComplete)
			inTutorial = true;
		
		bool close = false;

		SoundManager.instance.PlayMenuOpenSound();

		if(button == resumeButton)
		{
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.resumeGame();
			
			close = true;
			if(UIManager.instance.guildUI != null) {
				NGUITools.SetActive(UIManager.instance.guildUI.gameObject, true);
			}
			// Turn on the levelScene panel for Fast-forward tap and hold feature
			GameManager.instance.scaleformCamera.levelScene.levelScene.SetVisible(true);

		}
		else if(button == ringsButton)
		{
			if(inTutorial)
			{
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted);
				RemoveGlowRingIconInMenu();
			}
			
			if(_listenerInterface != null)
				_listenerInterface.onRingsButton();
			
			close = true;
		}
		else if(button == staffButton)
		{
			if(inTutorial) return;

			if(!staffUnlocked)
			{
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Locked!" , string.Format("Staff and runes unlock on day {0} !", GameManager.UNLOCK_RUNE_DAY), () => {});
				return;
			}

			if(_listenerInterface != null)
				_listenerInterface.onStaffButton();
			
			close = true;
		}
		else if(button == spiritsButton)
		{
			if(inTutorial) return;

			if(!spiritUnlocked)
			{
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Locked!" , string.Format("Spirits unlock on day {0} !", GameManager.UNLOCK_SPIRITS_DAY), () => {});
				return;
			}

			if(_listenerInterface != null)
				_listenerInterface.onSpiritsButton();
			
			close = true;
		}
		else if(button == transmutationButton)
		{
			if(inTutorial) return;

			if(!transmutationUnlocked)
			{
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Locked!" , string.Format("Transmutation unlocks on day {0} !", GameManager.UNLOCK_TRANSMUTATION_DAY), () => {});
				return;
			}

			if(_listenerInterface != null)
				_listenerInterface.onTransmutationButton();
			
			close = true;
		}
		else if(button == upgradesButton)
		{
			if(inTutorial) return;

			if(!upgradesUnlocked)
			{
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Locked!" , string.Format("Upgrades unlock on day {0} !", GameManager.UNLOCK_UPGRADES_DAY), () => {});
				return;
			}
			if(_listenerInterface != null)
				_listenerInterface.onUpgradeButton(PurchaseManager.GeneralPopupType.BagUpgrade);
			
			close = true;
		}
		else if(button == marketButton)
		{
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onMarketButton();
			
			close = true;
		}
		else if(button == optionsButton)
		{
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onOptionButton();
			
			close = true;
		}
		
		if(_listenerInterface == null)
			Debug.Log("Interface is NULL idiot!");
		
		if(close)
			GameObject.Destroy(gameObject);

	}
	
	
	public void GlowRingIconInMenu()
	{
		Debug.Log("GlowRingIconInMenu");
	}
	
	public void RemoveGlowRingIconInMenu()
	{
		Debug.Log("RemoveGlowRingIconInMenu");
	}
}
