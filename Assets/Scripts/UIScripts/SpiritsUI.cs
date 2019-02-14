using UnityEngine;
using System.Collections;

public class SpiritsUI : MonoBehaviour 
{
	public dfLabel _currentSpiritLevelLabel;
	public dfLabel _nextSpiritLevelLabel;
	public dfLabel _activeAbilityLabel;
	public dfLabel _passiveAbilityLabel;
	public dfLabel _spiritNameRightPanel;
	public dfLabel _spiritNameLeftPanel;
	public dfLabel _upgradeCostLabel;
	public dfLabel _upgrageTimeRemainingLabel;
	public dfLabel _amplifyCurrentValueLable;
	public dfLabel _amplifyDifferenceValueLabel;
	public dfLabel _regenerationCurrentValueLabel;
	public dfLabel _regenerationDifferenceValueLabel;
	public dfSlicedSprite _amplifyCurrentValueFill;
	public dfSlicedSprite _amplifyDifferenceValueFill;
	public dfSlicedSprite _regenerationCurrentValueFill;
	public dfSlicedSprite _regenerationDifferenceValueFill;
	public dfSprite _gemsIcon;
	public dfSprite _soulsIcon;
	public dfSprite _durationIcon;
	public dfSprite _spiritCurrentLevelImage;
	public dfSprite _spiritNextLevelImage;
	public dfButton _equipButton;
	public dfButton _TrainButton;

	public SpiritsUIInterface _spiritsUIInterface;

	private User user;
	
	
	public void setInterface(SpiritsUIInterface spiritsInterface)
	{
		_spiritsUIInterface = spiritsInterface;
		gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	
	void Start () 
	{
		if(_spiritsUIInterface != null)
			_spiritsUIInterface.OnRegisterSWFChildCallback(this);

		user = GameManager._gameState.User;
	}
	
	private PetModel nextPet = null;
	private PetModel currentPet = null;
	
	public void SetInventoryData()
	{
		Debug.Log("SetInventoryData CALLED");
		
		PetModel maxPet = null;
		IDictionary petDictionary = GameManager._gameState.User.petUpgradeDictionary;

		Debug.LogError("Wrong Hole.");

		if(GameManager._gameState.User.IsCurrentlyUpgradingPet()) // Currently upgrading...
		{
			GameManager._gameState.User.UnequipSpirit();
			currentPet = GameManager._gameState.User.GetCurrentPetFromUpgrade();
			nextPet = GameManager._gameState.User.GetNextPetFromUpgrade();

			IDictionary maxImp = petDictionary[User.TAG_IMP_MAX] as IDictionary;
			maxPet = new PetModel(maxImp["id"].ToString());
			
			if(!_isTimerRunning)
			{
				_timeRemaining = (int)nextPet.uTime;
				StartCountdown();
			}
		}
		else
		{
			IDictionary currentPetDictionary = petDictionary[User.TAG_IMP_CURRENT] as IDictionary;
			
			if(currentPetDictionary != null)
				currentPet = new PetModel(currentPetDictionary["id"].ToString());	
			
			IDictionary nextPetDictionary = petDictionary[User.TAG_IMP_NEXT] as IDictionary;
			nextPet = new PetModel(nextPetDictionary["id"].ToString());
			
			IDictionary maxPetDictionary = petDictionary[User.TAG_IMP_MAX] as IDictionary;
			maxPet = new PetModel(maxPetDictionary["id"].ToString());
		}

		SetAmplifyLevel(currentPet, nextPet, maxPet);
		SetRegenLevel(currentPet, nextPet, maxPet);
		PopulateLeftPanel(currentPet);
		PopulateRightPanel(nextPet);
		SetEquippedButton();
		PopulateStatsArea(currentPet, nextPet);
	}
	
	private void SetEquippedButton()
	{
		if(GameManager._gameState.User.availablePets.Count > 0)
		{
			if(string.IsNullOrEmpty(GameManager._gameState.User.spiritId))
				_equipButton.Text = "Equip";
			else
				_equipButton.Text = "Unequip";
		}
	}
	
	private void PopulateStatsArea(PetModel current, PetModel next)
	{
		if(current != null && current.uLevel == next.uLevel)
		{
			Debug.Log("Current and next at same level! :(");
			_gemsIcon.IsVisible = false;
			_soulsIcon.IsVisible = false;
			_upgradeCostLabel.Text = "";
			_upgrageTimeRemainingLabel.Text = "";
			_TrainButton.Text = "";
			_durationIcon.SpriteName = "";
			_TrainButton.IsEnabled = false;
			return;
		}
		
		if(GameManager._gameState.User.IsCurrentlyUpgradingPet())
		{
			Debug.Log("PopulateStatsArea >> CURRENTLY UPGRADING!");
			_gemsIcon.IsVisible = true;
			_soulsIcon.IsVisible = false;
			_TrainButton.Text = "Finish now";
		}
		else
		{
			Debug.Log("PopulateStatsArea >> CURRENTLY NOT UPGRADING!");
			_gemsIcon.IsVisible = false;
			_soulsIcon.IsVisible = true;
			
			_upgradeCostLabel.Text = next.dCost + "";
			_upgrageTimeRemainingLabel.Text = Upgradeui.ConvertSecToString((int)next.uTime);
			_TrainButton.Text = "Train";
		}
	}
	
	private void PopulateRightPanel(PetModel pet)
	{
		_nextSpiritLevelLabel.Text = pet.uLevel + "";
		
		if(string.IsNullOrEmpty(pet.activeSpell))
			_activeAbilityLabel.Text = "";
		else
			_activeAbilityLabel.Text = pet.activeSpell;
		
		if(string.IsNullOrEmpty(pet.abilityStat))
			_passiveAbilityLabel.Text = "";
		else
			_passiveAbilityLabel.Text = pet.abilityStat;
	}
	
	private void PopulateLeftPanel(PetModel pet)
	{
		if(pet == null || !GameManager._gameState.User.availablePets.Contains(pet.id))
		{
			_spiritNameLeftPanel.Text = "";
			_spiritCurrentLevelImage.SpriteName = "";
			_currentSpiritLevelLabel.Text = "";
		}
		else
		{
			_spiritNameLeftPanel.Text = "Hass";
			_spiritCurrentLevelImage.SpriteName = "image 7";
			_currentSpiritLevelLabel.Text = pet.uLevel + "";
		}
	}
	private int _timeRemaining = int.MinValue;
	
	public void SetUpdateData(int timeRemaining, int boostCost)
	{
		_upgrageTimeRemainingLabel.Text = Upgradeui.ConvertSecToString(timeRemaining);
		_upgradeCostLabel.Text = boostCost + "";
		
		_timeRemaining = timeRemaining;
		if(!_isTimerRunning)
			StartCountdown();
		
		SetInventoryData();
	}
	
	private bool _isTimerRunning = false;
	
	private void StartCountdown()
	{
		if(!GameManager._gameState.User.IsCurrentlyUpgradingPet())
		{
			_isTimerRunning = false;
			return;
		}
		if(_timeRemaining < 0)
		{
			_upgrageTimeRemainingLabel.Text = "?";
			_isTimerRunning = false;
			return;
		}
		
		_isTimerRunning = true;
		
		this.PerformActionWithDelay(1f, () => {
			if(!GameManager._gameState.User.IsCurrentlyUpgradingPet())
			{
				_isTimerRunning = false;
				return;
			}
			_upgrageTimeRemainingLabel.Text = Upgradeui.ConvertSecToString(_timeRemaining);
			if(_timeRemaining == 0)
			{
				GameManager._gameState.User.OnSpiritUpgradeComplete();
				_isTimerRunning = false;
			}
			else if(_timeRemaining > 0)
			{
				_timeRemaining--;
				StartCountdown();
			}
		});
	}
	
	public void OnClick( dfControl sender, dfMouseEventArgs args )
	{
		if(args.Source == _TrainButton)
		{
			if(_TrainButton.Text == "Train")
			{
				//start upgrade...
				if(UIManager.instance.generalSwf.onPetStartUpgrade(nextPet.id))
				{
					Debug.Log("Pet upgrade started!");
					SetInventoryData();
				}
			}
			else if(_TrainButton.Text == "Finish now")//Finish now...
			{
				GameManager._gameState.User.BoostSpiritUpgrade();
				Debug.Log("Calling SetInventoryData ::: ");
				SetInventoryData();
			}
			else
			{
				Debug.Log("Spirit at max level!");
			}
		}
		else if(args.Source == _equipButton)
		{
			if(_equipButton.Text == "Equip")
			{
				if(GameManager._gameState.User.IsCurrentlyUpgradingPet())
				{
					//show popup saying that you can't equip while upgrading
					UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Sorry!" , string.Format("Can't equip while upgrading."), () => {});
				}
				else
				{
					if(GameManager._gameState.User.availablePets.Count <= 0)
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Can't equip!" , string.Format("You don't have a spirit yet."), () => {});
					else
					{
						GameManager._gameState.User.spiritId = currentPet.id;
						_equipButton.Text = "Unequip";
					}
					//equip current pet...
				}
			}
			else
			{
				GameManager._gameState.User.UnequipSpirit();
				_equipButton.Text = "Equip";
			}
		}
	}
	
	private void SetRegenLevel(PetModel current, PetModel next, PetModel max)
	{
		if(current == null || string.IsNullOrEmpty(current.abilityStat))
		{
			_regenerationCurrentValueLabel.Text = "00";
			_regenerationCurrentValueFill.FillAmount = 0;
			
			if(string.IsNullOrEmpty(next.abilityStat))
			{
				_regenerationDifferenceValueLabel.Text = "+" + "00";
				_regenerationDifferenceValueFill.FillAmount = 0;
			}
			else
			{
				_regenerationDifferenceValueLabel.Text = "+" + next.skullLevel + "";
				_regenerationDifferenceValueFill.FillAmount = next.skullLevel / (float) max.skullLevel;
			}
		}
		else
		{
			_regenerationCurrentValueLabel.Text = current.skullLevel + "";
			_regenerationCurrentValueFill.FillAmount = current.skullLevel / (float) max.skullLevel;
			_regenerationDifferenceValueLabel.Text = "+" + next.skullLevel + "";
			_regenerationDifferenceValueFill.FillAmount = next.skullLevel / (float) max.skullLevel;
		}
	}
		
	private void SetAmplifyLevel(PetModel current, PetModel next, PetModel max)
	{
		if(current == null || string.IsNullOrEmpty(current.activeSpell))
		{
			_amplifyCurrentValueFill.FillAmount = 0;
			_amplifyCurrentValueLable.Text = "0";
			
			_amplifyDifferenceValueLabel.Text = "+" + next.skullLevel + "";
			_amplifyDifferenceValueFill.FillAmount = next.skullLevel / (float) max.skullLevel;
		}
		else
		{
			_amplifyCurrentValueLable.Text = current.skullLevel + "";
			_amplifyCurrentValueFill.FillAmount = current.skullLevel / (float) max.skullLevel;
			_amplifyDifferenceValueLabel.Text = "+" + next.skullLevel + "";
			_amplifyDifferenceValueFill.FillAmount = next.skullLevel / (float) max.skullLevel;
		}
	}
	
	private void Update()
	{
		InputWrapper.disableTouch = true;
	}
	
	private void OnDestroy()
	{
		InputWrapper.disableTouch = false;
	}
}
