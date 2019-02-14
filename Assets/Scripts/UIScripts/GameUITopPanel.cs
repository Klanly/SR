using UnityEngine;
using System.Collections;

public class GameUITopPanel : MonoBehaviour 
{
	public dfPanel _topPanel;
	
	public dfSprite _gemsIcon;
	public dfLabel _gemsNumber;
	
	public dfSprite _healthIcon;
	public dfLabel _healthNumber;
	
	public dfSprite _soulsIcon;
	public dfLabel _soulsNumber;
	
	public dfSprite _pauseMenuRing;

	public dfPanel pauseMenuPrefab;
	public dfSprite pauseMenuHighlight;
	
	
	private GameUIInterface _listenerInterface;

	void Start()
	{
		gameObject.GetComponent<dfPanel>().PerformLayout();
	}

	public void SetListener(GameUIInterface listenerInterface)
	{
		_listenerInterface = listenerInterface;
	}
	
	public void UpdateHealth(int health)
	{
		User TempUser =GameManager._gameState.User;
		if(TempUser.life < TempUser.totalLife*0.3f)
		{
			UpdateHealth(health, true);
		}
		else
		{
			UpdateHealth(health, false);
		}
	}

	public void UpdateHealth(int health, bool playHeartAnimation)
	{
		_healthNumber.Text = health + "";
		
		if(playHeartAnimation)
		{
			_healthIcon.GetComponent<dfTweenVector3>().Play();
		}
		else
		{
			_healthIcon.GetComponent<dfTweenVector3>().Stop();
		}
	}

	public void UpdateSoulGems(int soulGems)
	{
		_gemsNumber.Text = soulGems + "";
	}

	public void UpdateSoulDust(float soulDust)
	{
		_soulsNumber.Text = soulDust + "";
	}
	
	public void HideTopStats()
	{
		_gemsIcon.IsVisible = false;
		_gemsNumber.IsVisible = false;
	
		_healthIcon.IsVisible = false;
		_healthNumber.IsVisible = false;
		
		_soulsIcon.IsVisible = false;
		_soulsNumber.IsVisible = false;
	}
	
	public void ShowTopStats()
	{
//		Debug.Log(":::: ShowTopStats ::::");
		_gemsIcon.IsVisible = true;
		_gemsNumber.IsVisible = true;
	
		_healthIcon.IsVisible = true;
		_healthNumber.IsVisible = true;
		
		_soulsIcon.IsVisible = true;
		_soulsNumber.IsVisible = true;
	}
	
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
	
	public void GlowMenuIcon()
	{
		pauseMenuHighlight.IsVisible = true;
	}
	
	public void RemoveGlowMenuIcon()
	{
		pauseMenuHighlight.IsVisible = false;
	}
	
	public void DisplayCenterButton(bool yesno)
	{
		_pauseMenuRing.IsVisible = yesno;
	}
	
	public void GlowRingIconInMenu()
	{
		
	}
	
	public void RemoveGlowRingIconInMenu()
	{
		
	}
	
	public void ShowHeartIcon()
	{
		_healthIcon.IsVisible = true;
		_healthNumber.IsVisible = true;
	}
	
	public void HideHeartIcon()
	{
		_healthIcon.IsVisible = false;
		_healthNumber.IsVisible = false;
	}
}
