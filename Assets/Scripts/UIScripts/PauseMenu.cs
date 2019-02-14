using UnityEngine;
using System;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{
	public dfButton resumeButton;
	public dfButton ringsButton;
	public dfButton staffButton;
	public dfButton staffUnavailableButton;
	public dfButton spiritsButton;
	public dfButton spiritsUnavailableButton;
	public dfButton transmutationButton;
	public dfButton transmutationUnavailableButton;
	public dfButton upgradesButton;
	public dfButton upgradesUnavailableButton;
	public dfButton marketButton;
	public dfButton optionsButton;
	
	void Start()
	{
		if(GameManager._gameState.dayCount == 0)
			Destroy(gameObject);
		
		Debug.Log("TutorialManager.instance.state >> " + TutorialManager.instance.state);
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.BattleEndTutorial2)
		{
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.BattleEndTutorialComplete);
			GlowRingIconInMenu();
		}
		
		if(GameManager._gameState.dayCount >= GameManager.UNLOCK_UPGRADES_DAY)
		{
			upgradesButton.gameObject.SetActive(true);
			upgradesUnavailableButton.gameObject.SetActive(false);
		}
		else
		{
			upgradesButton.gameObject.SetActive(false);
			upgradesUnavailableButton.gameObject.SetActive(true);
		}
		
		if(GameManager._gameState.dayCount >= GameManager.UNLOCK_RUNE_DAY)
		{
			staffButton.gameObject.SetActive(true);
			staffUnavailableButton.gameObject.SetActive(false);
		}
		else
		{
			staffButton.gameObject.SetActive(false);
			staffUnavailableButton.gameObject.SetActive(true);
		}
		
		if(GameManager._gameState.dayCount >= GameManager.UNLOCK_TRANSMUTATION_DAY)
		{
			transmutationButton.gameObject.SetActive(true);
			transmutationUnavailableButton.gameObject.SetActive(false);
		}
		else
		{
			transmutationButton.gameObject.SetActive(false);
			transmutationUnavailableButton.gameObject.SetActive(true);
		}
		
		if(GameManager._gameState.dayCount >= GameManager.UNLOCK_SPIRITS_DAY)
		{
			spiritsButton.gameObject.SetActive(true);
			spiritsUnavailableButton.gameObject.SetActive(false);
		}
		else
		{
			spiritsButton.gameObject.SetActive(false);
			spiritsUnavailableButton.gameObject.SetActive(true);
		}
	}
	
	private GameUIInterface _listenerInterface;
	public void SetListener(GameUIInterface listenerInterface)
	{
		_listenerInterface = listenerInterface;
		this.gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		bool inTutorial = false;
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.BattleEndTutorialComplete)
			inTutorial = true;
		
		bool close = false;
		Debug.Log("mouseEvent.Source >> " + mouseEvent.Source.gameObject.name);
		
		if(mouseEvent.Source == resumeButton)
		{
			Debug.Log("resumeButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.resumeGame();
			
			close = true;
		}
		else if(mouseEvent.Source == ringsButton)
		{
			Debug.Log("ringsButton clicked!");
			
			if(inTutorial)
			{
				TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted);
				RemoveGlowRingIconInMenu();
			}
			
			if(_listenerInterface != null)
				_listenerInterface.onRingsButton();
			
			close = true;
		}
		else if(mouseEvent.Source == staffButton)
		{
			Debug.Log("staffButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onStaffButton();
			
			close = true;
		}
		else if(mouseEvent.Source == staffUnavailableButton)
		{
			if(inTutorial) return;

			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.ShowGeneralPopup("Locked!" , string.Format("Staff and runes unlock on day {0} !", GameManager.UNLOCK_RUNE_DAY), () => {});
		}
		else if(mouseEvent.Source == spiritsButton)
		{
			Debug.Log("spiritsButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onSpiritsButton();
			
			close = true;
		}
		else if(mouseEvent.Source == spiritsUnavailableButton)
		{
			if(inTutorial) return;
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.ShowGeneralPopup("Locked!" , string.Format("Spirits unlock on day {0} !", GameManager.UNLOCK_SPIRITS_DAY), () => {});
		}
		else if(mouseEvent.Source == transmutationButton)
		{
			Debug.Log("transmutationButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onTransmutationButton();
			
			close = true;
		}
		else if(mouseEvent.Source == transmutationUnavailableButton)
		{
			if(inTutorial) return;
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.ShowGeneralPopup("Locked!" , string.Format("Transmutation unlocks on day {0} !", GameManager.UNLOCK_TRANSMUTATION_DAY), () => {});
		}
		else if(mouseEvent.Source == upgradesButton)
		{
			Debug.Log("upgradesButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onUpgradeButton(PurchaseManager.GeneralPopupType.BagUpgrade);
			
			close = true;
		}
		else if(mouseEvent.Source == upgradesUnavailableButton)
		{
			if(inTutorial) return;
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.ShowGeneralPopup("Locked!" , string.Format("Upgrades unlock on day {0} !", GameManager.UNLOCK_UPGRADES_DAY), () => {});
		}
		else if(mouseEvent.Source == marketButton)
		{
			Debug.Log("marketButton clicked!");
			
			if(inTutorial) return;
			
			if(_listenerInterface != null)
				_listenerInterface.onMarketButton();
			
			close = true;
		}
		else if(mouseEvent.Source == optionsButton)
		{
			Debug.Log("optionsButton clicked!");
			
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
	
	public void Update()
	{
		InputWrapper.disableTouch = true;
	}
}
