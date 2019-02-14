using UnityEngine;
using System.Collections;

public class NGSpiritsUI : MonoBehaviour
{
	public UILabel MaxHealth;
	public UILabel MaxWards;
	public UILabel MaxDamage;

	public UILabel _currentSpiritLevelLabel;
	public UILabel _nextSpiritLevelLabel;
	public UILabel _activeAbilityLabel;
	public UILabel _statPassiveAbilityLabel;
	public UILabel _elementPassiveAbilityLabel;
	public UILabel _spiritNameRightPanel;
	public UILabel _spiritNameLeftPanel;
	public UILabel _upgradeCostLabel;
	public UILabel _upgrageTimeRemainingLabel;
	public UILabel _activeLabelLeft;
	public UILabel _activeCurrentValueLable;
	public UILabel _activeDifferenceValueLabel;
	public UISprite _activeCurrentValueFill;
	public UISprite _activeDifferenceValueFill;
	public UILabel _statLabelLeft;
	public UILabel _statCurrentValueLabel;
	public UILabel _statDifferenceValueLabel;
	public UISprite _statCurrentValueFill;
	public UISprite _statDifferenceValueFill;
	public UILabel _elementLabelLeft;
	public UILabel _elementCurrentValueLabel;
	public UILabel _elementDifferenceValueLabel;
	public UISprite _elementCurrentValueFill;
	public UISprite _elementDifferenceValueFill;
	public UISprite _gemsIcon;
	public UISprite _soulsIcon;
	public UISprite _durationIcon;
	public UISprite _spiritCurrentLevelImage;
	public UITexture _glowSprite;
	public UISprite _spiritNextLevelImage;
	public UIButton _equipButton;
	public UILabel _equipButtonText;
	public UIButton _TrainButton;
	public UILabel _TrainButtonText;


	public UISprite _activeBar;
	public UISprite _statsBar;
	public UISprite _elementBar;
	public UILabel _maxLevelLabel;

	public UISprite _activeImage;
	public UISprite _statImage;
	public UISprite _elementImage;


	public UIGrid spiritListView;

	public Camera renderCamera;

	public SpiritsUIInterface _spiritsUIInterface;

	private User user;

	private SRSpiritScrollViewItem previousCenter = null;
	
	public void setInterface (SpiritsUIInterface spiritsInterface)
	{
		_spiritsUIInterface = spiritsInterface;
	}

//	int count = 0;
//	void OnGUI() {
//		if (GUI.Button(new Rect(10, 70, 50, 30), "Click")) {
//			count++;
//			if(count >= spiritListView.GetChildList().Count)
//				count = 0;
//
//			Transform child = spiritListView.GetChildList()[count];
//			Debug.LogError("child name = "+child.name +" "+count);
//			spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//		}
//		
//	}
	
	void Start ()
	{
		populateSpiritsListView ();

		if (_spiritsUIInterface != null)
			_spiritsUIInterface.OnRegisterSWFChildCallback (this);

		user = GameManager._gameState.User;


		var centerOnChild = spiritListView.gameObject.GetComponent<UICenterOnChild> ();
		if (centerOnChild != null)
			centerOnChild.onFinished = onCenterOnChild;
		RefreshUI ();

		//UIManager.instance.generalSwf.ToggleTopStats(true);
		UIManager.instance.generalSwf.HideHeartIcon ();

		NGUITools.BringForward(gameObject);
		UIManager.instance.generalSwf.SetOnTop();
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);

	}
	
	private PetModel nextPet = null;
	private PetModel currentPet = null;

	public void populateSpiritsListView ()
	{
		var listNodes = spiritListView.GetChildList ();
		if (listNodes.Count < 4) {
			Debug.LogError ("Add there Children in the Grid.");
			return;
		}

		IDictionary petDictionary = GameManager._gameState.User.petUpgradeDictionary;

		IDictionary nextImp = petDictionary [User.TAG_IMP_NEXT] as IDictionary;
		listNodes [0].GetComponent<SRSpiritScrollViewItem> ().petModel = new PetModel (nextImp ["id"].ToString ());

		
		IDictionary nextAqua = petDictionary [User.TAG_AQUA_NEXT] as IDictionary;
		listNodes [1].GetComponent<SRSpiritScrollViewItem> ().petModel = new PetModel (nextAqua ["id"].ToString ());

		
		IDictionary nextDrake = petDictionary [User.TAG_DRAKE_NEXT] as IDictionary;
		listNodes [2].GetComponent<SRSpiritScrollViewItem> ().petModel = new PetModel (nextDrake ["id"].ToString ());

		IDictionary nextGolem = petDictionary [User.TAG_GOLEM_NEXT] as IDictionary;
		listNodes [3].GetComponent<SRSpiritScrollViewItem> ().petModel = new PetModel (nextGolem ["id"].ToString ());
		//listNodes[0].GetComponent<SRSpiritScrollViewItem>().petModel =;


	}

	void onCenterOnChild ()
	{
		if (previousCenter != null)
			previousCenter.Unhighlight ();
//		Debug.LogError("onCenterOnChild");
		SoundManager.instance.PlayScrollSound();
		GameObject centered = spiritListView.gameObject.GetComponent<UICenterOnChild> ().centeredObject;
		if (centered != null) {
			previousCenter = centered.GetComponent<SRSpiritScrollViewItem> ();
			previousCenter.Highlight ();
			PetModel pet = previousCenter.petModel;
			if (pet != null) {
				PopulateRightPanel (pet);

				PetModel maxPet = null;
				IDictionary petDictionary = GameManager._gameState.User.petUpgradeDictionary;

				if (!GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
					IDictionary currentPetDictionary = petDictionary [pet.getPetName () + "_Current"] as IDictionary;

					if (currentPetDictionary != null)
						currentPet = new PetModel (currentPetDictionary ["id"].ToString ());
					else {
						currentPet = null;
//						Debug.LogError("current Dic is null for "+pet.getPetName()+"_Current");
					}

					IDictionary nextPetDictionary = petDictionary [pet.getPetName () + "_Next"] as IDictionary;
					nextPet = new PetModel (nextPetDictionary ["id"].ToString ());
//					Debug.LogError("next pet Name: "+nextPet.getPetName());

					IDictionary maxPetDictionary = petDictionary [pet.getPetName () + "_Max"] as IDictionary;
					maxPet = new PetModel (maxPetDictionary ["id"].ToString ());

//					Debug.LogError("Current Pat: "+nextPet.getPetName());
					SetActiveLevel (currentPet, nextPet, maxPet);
					SetStatPassiveLevel (currentPet, nextPet, maxPet);
					SetElementPassiveLevel (currentPet, nextPet, maxPet);
					PopulateLeftPanel (currentPet);

					if(currentPet != null && currentPet.uLevel == maxPet.uLevel) {
//						NGUITools.SetActive(_activeBar.gameObject, false);
						NGUITools.SetActive(_activeDifferenceValueFill.gameObject, false);
						NGUITools.SetActive(_activeDifferenceValueLabel.gameObject, false);
//						NGUITools.SetActive(_statsBar.gameObject, false);
						NGUITools.SetActive(_statDifferenceValueFill.gameObject, false);
						NGUITools.SetActive(_statDifferenceValueLabel.gameObject, false);
//						NGUITools.SetActive(_elementBar.gameObject, false);
						NGUITools.SetActive(_elementDifferenceValueFill.gameObject, false);
						NGUITools.SetActive(_elementDifferenceValueLabel.gameObject, false);
						NGUITools.SetActive(_maxLevelLabel.gameObject, true);
//						string petName = currentPet.getPetName().Replace ("IMP", "HASS").Replace ("AQUA", "CTHULU").Replace ("GOLEM", "BALBOA");
						string petName = currentPet.getPetName();
						_maxLevelLabel.text = "Max Level "+petName;
					} else {
//						NGUITools.SetActive(_activeBar.gameObject, true);
						NGUITools.SetActive(_activeDifferenceValueFill.gameObject, true);
						NGUITools.SetActive(_activeDifferenceValueLabel.gameObject, true);
//						NGUITools.SetActive(_statsBar.gameObject, true);
						NGUITools.SetActive(_statDifferenceValueFill.gameObject, true);
						NGUITools.SetActive(_statDifferenceValueLabel.gameObject, true);
//						NGUITools.SetActive(_elementBar.gameObject, true);
						NGUITools.SetActive(_elementDifferenceValueFill.gameObject, true);
						NGUITools.SetActive(_elementDifferenceValueLabel.gameObject, true);
						NGUITools.SetActive(_maxLevelLabel.gameObject, false);
					}

					
					PopulateStatsArea (currentPet, nextPet);

					_equipButton.gameObject.SetActive (true);
				}
				
				SetEquippedButton (currentPet);
			} else
				Debug.LogError ("Pet Model is NULL.");
		}
	}

	void RefreshPetModel (PetModel pet)
	{
//		Debug.LogError ("RefreshPetModel: " + pet.id);
		var children = spiritListView.GetChildList ();
		foreach (var child in children) {
			PetModel model = child.GetComponent<SRSpiritScrollViewItem>().petModel;
//			Debug.LogError("list child name = "+model.id+" "+model.name+"current child name centering  = "+pet.id);

			if (child.GetComponent<SRSpiritScrollViewItem> ().petModel.id.Contains (pet.getPetName ())) {
				child.GetComponent<SRSpiritScrollViewItem> ().petModel = pet;
				spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//				child.GetComponent<SRSpiritScrollViewItem>().icon.color = Color.blue;
//				Debug.LogError("child name centering  = "+child.GetComponent<SRSpiritScrollViewItem>().petModel.id);
				break;
			}
		}
	}
    
	public void SetInventoryData (string updatedPetId = "")
	{
		Debug.LogError ("SetInventoryData CALLED -- "+ updatedPetId+" --");
	
		PetModel maxPet = null;
		IDictionary petDictionary = GameManager._gameState.User.petUpgradeDictionary;
	
		if (GameManager._gameState.User.IsCurrentlyUpgradingPet ()) { // Currently upgrading...
			GameManager._gameState.User.UnequipSpirit ();
			currentPet = GameManager._gameState.User.GetCurrentPetFromUpgrade ();
			nextPet = GameManager._gameState.User.GetNextPetFromUpgrade ();

			Debug.LogError("next pet Name: "+nextPet.getPetName());

			IDictionary maxPetDetails = petDictionary [nextPet.getPetName () + "_Max"] as IDictionary;
			maxPet = new PetModel (maxPetDetails ["id"].ToString ());
			
			if (!_isTimerRunning) {
				_timeRemaining = (int)nextPet.uTime;
				StartCountdown ();
			}

			NGUITools.SetActive( _glowSprite.gameObject, true);
			TweenAlpha tweenAlpha = _glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = true;
			Debug.LogError("Enabling the glow on spirit");

			SetActiveLevel (currentPet, nextPet, maxPet);
			SetStatPassiveLevel (currentPet, nextPet, maxPet);
			SetElementPassiveLevel (currentPet, nextPet, maxPet);
			PopulateLeftPanel (currentPet);

			if(currentPet != null && currentPet.uLevel == maxPet.uLevel) {
				NGUITools.SetActive(_activeDifferenceValueFill.gameObject, false);
				NGUITools.SetActive(_activeDifferenceValueLabel.gameObject, false);
				NGUITools.SetActive(_statDifferenceValueFill.gameObject, false);
				NGUITools.SetActive(_statDifferenceValueLabel.gameObject, false);
				NGUITools.SetActive(_elementDifferenceValueFill.gameObject, false);
				NGUITools.SetActive(_elementDifferenceValueLabel.gameObject, false);
				NGUITools.SetActive(_maxLevelLabel.gameObject, true);
				string petName = currentPet.getPetName();
				_maxLevelLabel.text = "Max Level "+petName;
			} else {
				NGUITools.SetActive(_activeDifferenceValueFill.gameObject, true);
				NGUITools.SetActive(_activeDifferenceValueLabel.gameObject, true);
				NGUITools.SetActive(_statDifferenceValueFill.gameObject, true);
				NGUITools.SetActive(_statDifferenceValueLabel.gameObject, true);
				NGUITools.SetActive(_elementDifferenceValueFill.gameObject, true);
				NGUITools.SetActive(_elementDifferenceValueLabel.gameObject, true);
				NGUITools.SetActive(_maxLevelLabel.gameObject, false);
			}
			PopulateStatsArea (currentPet, nextPet);

			_equipButton.gameObject.SetActive (false);

			if (currentPet != null) {
//				var children = spiritListView.GetChildList ();
//				foreach (var child in children) {
//					if (child.GetComponent<SRSpiritScrollViewItem> ().petModel.getPetName ().Equals (currentPet.getPetName ())) {
//						spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//						Debug.LogError("Inside - "+child.GetComponent<SRSpiritScrollViewItem>().petModel.getPetName());
//					}
//				}
			}


		} else {
			TweenAlpha tweenAlpha = _glowSprite.GetComponent<TweenAlpha>();
			tweenAlpha.enabled = false;
			_glowSprite.alpha = 0.4f;
		}

		onCenterOnChild ();
		if(GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
			PetModel p = null;
			if(currentPet != null) {
				p = currentPet;
			}
			if(nextPet != null) {
				p = nextPet;
			}
//			Debug.LogError("id = "+p.id+" name - "+p.name);
			foreach(Transform child in spiritListView.GetChildList()) {
//				Debug.LogError(child.GetComponent<SRSpiritScrollViewItem>().petModel.id+" "+child.GetComponent<SRSpiritScrollViewItem>().petModel.name+" "+p.id+" "+p.name);
				if(child.GetComponent<SRSpiritScrollViewItem>().petModel.getPetName().Equals(p.getPetName())) {
					spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//					Debug.LogError("Inside - "+child.GetComponent<SRSpiritScrollViewItem>().petModel.getPetName());
					break;
				}
			}
//			return;
		} else {
			if(!string.IsNullOrEmpty(updatedPetId)) {
//				Debug.LogError("id = "+updatedPetId);
				PetModel newPet = new PetModel ((petDictionary [updatedPetId.Substring (0, updatedPetId.Length - 1) + "_Next"]as IDictionary) ["id"].ToString ()); 
				RefreshPetModel (newPet);
				foreach(Transform child in spiritListView.GetChildList()) {
//					Debug.LogError(child.GetComponent<SRSpiritScrollViewItem>().petModel.id+" "+child.GetComponent<SRSpiritScrollViewItem>().petModel.name+" "+updatedPetId);
					if(updatedPetId.Contains(child.GetComponent<SRSpiritScrollViewItem>().petModel.getPetName())) {
						spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//						Debug.LogError("Inside - "+child.GetComponent<SRSpiritScrollViewItem>().petModel.getPetName());
						break;
					}
				}
			} else {
				Transform child = spiritListView.GetChildList()[0];
				spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
			}
		}
		return;
		if (!updatedPetId.Equals("")) {
//			Debug.LogError ("Pet Updated: " + updatedPetId);
			PetModel newPet = new PetModel ((petDictionary [updatedPetId.Substring (0, updatedPetId.Length - 1) + "_Next"]as IDictionary) ["id"].ToString ()); 
			RefreshPetModel (newPet);
			foreach(Transform child in spiritListView.GetChildList()) {
				if(child.GetComponent<SRSpiritScrollViewItem>().petModel.id.Equals(newPet.id)) {
					spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
//					Debug.LogError("Inside - "+child.GetComponent<SRSpiritScrollViewItem>().petModel.id);
					break;
				}
			}
		} else {
//			Debug.LogError ("Does not equal empty");

			if (currentPet != null) {
				foreach(Transform child in spiritListView.GetChildList()) {
					if(child.GetComponent<SRSpiritScrollViewItem>().petModel.name.Equals(currentPet.name)) {
						spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
						break;
					}
				}
			} else {
				Transform child = spiritListView.GetChildList()[0];
				spiritListView.gameObject.GetComponent<UICenterOnChild> ().CenterOn (child);
			}
		}
		
	}
	
	private void SetEquippedButton (PetModel pet)
	{
//		string str = "";
//		GameManager._gameState.User.availablePets.ForEach((pt) => str += pt+", ");
//		Debug.LogError("SetEquippedButton Called."+ " Pets: "+str);
		if (GameManager._gameState.User.availablePets.Count > 0 && pet != null && GameManager._gameState.User.availablePets.Contains (pet.id)) {
			//Debug.LogError("Equipped Button: "+pet.id+", "+ GameManager._gameState.User.spiritId);
			if (pet.id != GameManager._gameState.User.spiritId)
				_equipButtonText.text = "Equip";
			else
				_equipButtonText.text = "Unequip";
		} else
			_equipButton.gameObject.SetActive (false);
	}
	
	private void PopulateStatsArea (PetModel current, PetModel next)
	{
		//TODO Check if it is right
		_upgradeCostLabel.gameObject.SetActive (true);
		_upgrageTimeRemainingLabel.gameObject.SetActive (true);
		_TrainButtonText.gameObject.SetActive (true);
		_durationIcon.gameObject.SetActive (true);
		_TrainButton.gameObject.SetActive (true);

		if (current != null && current.uLevel == next.uLevel) {
			Debug.Log ("Current and next at same level! :(");
			_gemsIcon.gameObject.SetActive (false);
			_soulsIcon.gameObject.SetActive (false);
			_upgradeCostLabel.gameObject.SetActive (false);
			_upgrageTimeRemainingLabel.gameObject.SetActive (false);
			_TrainButtonText.gameObject.SetActive (false);
			_durationIcon.gameObject.SetActive (false);
			_TrainButton.gameObject.SetActive (false);
			return;
		}
		
		if (GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
			Debug.Log ("PopulateStatsArea >> CURRENTLY UPGRADING!");
			_gemsIcon.gameObject.SetActive (true);
			_soulsIcon.gameObject.SetActive (false);
			_TrainButtonText.text = "Finish now";
		} else {
			Debug.Log ("PopulateStatsArea >> CURRENTLY NOT UPGRADING!");
			_gemsIcon.gameObject.SetActive (false);
			_soulsIcon.gameObject.SetActive (true);
			
			_upgradeCostLabel.text = next.dCost + "";
			_upgrageTimeRemainingLabel.text = Upgradeui.ConvertSecToString ((int)next.uTime);
			_TrainButtonText.text = "Train";
		}
	}
	
	private void PopulateRightPanel (PetModel pet)
	{
//		_spiritNameRightPanel.text = pacttiveet.getPetName ().Replace ("IMP", "HASS").Replace ("AQUA", "CTHULU").Replace ("GOLEM", "BALBOA");
		_spiritNameRightPanel.text = pet.getPetName ();

		_nextSpiritLevelLabel.text = "Level "+pet.uLevel.ToString ();
//		_activeImage.gameObject.SetActive(false);
//		_statImage.gameObject.SetActive(false);
//		_elementImage.gameObject.SetActive(false);
		if (string.IsNullOrEmpty (pet.activeSpell)) {
//			_activeAbilityLabel.text = "";
		} else {
//			_activeAbilityLabel.text = pet.activeSpell;
//			_activeImage.spriteName = GetSpriteNameFromAbility(pet.activeSpell);
//			_activeImage.gameObject.SetActive(true);
		}
		
		if (string.IsNullOrEmpty (pet.abilityStat)) {
//			_statPassiveAbilityLabel.text = "";
		} else {
//			_statPassiveAbilityLabel.text = pet.abilityStat;
//			_statImage.spriteName = GetSpriteNameFromAbility(pet.abilityStat);
//			_statImage.gameObject.SetActive(true);
		}

		if (string.IsNullOrEmpty (pet.abilityElement)) {
//			_elementPassiveAbilityLabel.text = "";
		} else {
//			_elementPassiveAbilityLabel.text = pet.abilityElement;
//			_elementImage.spriteName = GetSpriteNameFromAbility(pet.abilityElement);
//			_elementImage.gameObject.SetActive(true);
		}
//		Debug.LogError(pet.activeSpell+" "+pet.abilityStat+" "+pet.abilityElement);
	}
	
	private void PopulateLeftPanel (PetModel pet)
	{
		if (pet == null || !GameManager._gameState.User.availablePets.Contains (pet.id)) {
			//_spiritNameLeftPanel.text = "";
			//_spiritCurrentLevelImage.spriteName = "";
			//_currentSpiritLevelLabel.text = "";
		} else {
			//_spiritNameLeftPanel.text = pet.getPetName().Replace("IMP","HASS");//"Hass";
			// TODO: Uncomment the Sprite Name Setter after adding the Pets respective sprites to the Atlas.
			//_spiritCurrentLevelImage.spriteName = "IMP";//pet.id.Substring(0,pet.id.Length-1);
//			_currentSpiritLevelLabel.text = "The " + pet.getPetName ().Replace ("AQUA", "DEEP HORROR") + ": " + pet.uLevel;
			_currentSpiritLevelLabel.text = "THE " + pet.getPetName () + ": " + pet.uLevel;
		}
	}
	private int _timeRemaining = int.MinValue;
	
	public void SetUpdateData (int timeRemaining, int boostCost)
	{
		Debug.LogError("Spirits - time - "+timeRemaining+" boost - "+boostCost);
		_upgrageTimeRemainingLabel.text = Upgradeui.ConvertSecToString (timeRemaining);
		_upgradeCostLabel.text = boostCost.ToString();
		
		_timeRemaining = timeRemaining;
		if (!_isTimerRunning) {
			StartCountdown ();
			if(_timeRemaining > 0) {
				System.DateTime dateTime = System.DateTime.Now.AddSeconds(_timeRemaining);
				NotificationManager.SendLocalNotification(dateTime, NotificationManager.NotificationType.Spirit);
			}
		}
		
		SetInventoryData ();
	}
	
	private bool _isTimerRunning = false;
	
	private void StartCountdown ()
	{
		if (!GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
			_isTimerRunning = false;
			return;
		}
		if (_timeRemaining < 0) {
			_upgrageTimeRemainingLabel.text = "?";
			_isTimerRunning = false;
			return;
		}
		
		_isTimerRunning = true;
		
		this.PerformActionWithDelay (1f, () => {
			if(_timeRemaining < 5) {
				NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Transmutation);
			}
			if (!GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
				_isTimerRunning = false;
				return;
			}
			_upgrageTimeRemainingLabel.text = Upgradeui.ConvertSecToString (_timeRemaining);
			if (_timeRemaining == 0) {
				GameManager._gameState.User.OnSpiritUpgradeComplete ();
				_isTimerRunning = false;
			} else if (_timeRemaining > 0) {
				_timeRemaining--;
				StartCountdown ();
			}
		});
	}

	public void ShowSpiritsCompletePopup (string itemId, string itemName, string itemLevel)
	{
		var children = spiritListView.GetChildList();
		foreach(Transform child in children) {
			PetModel model = child.GetComponent<SRSpiritScrollViewItem>().petModel;
			if(model.getPetName().Equals(itemName)) {
				Debug.LogWarning("model name - "+model.getPetName()+" itemnName - "+itemName);
				this.PerformActionWithDelay(0.5f, () => {
					spiritListView.GetComponent<UICenterOnChild>().CenterOn(child);
				}, null);
				break;
			}
		}
		UnityEngine.Object asset = Resources.Load ("UIPrefabs/NGUI/SpiritCompletePopup");
		NGSpiritComplete popup = NGUITools.AddChild (this.gameObject, asset as GameObject).GetComponent<NGSpiritComplete> ();
		Debug.LogError(" show pet - "+itemId+" - "+ itemName +" - "+itemLevel);
		popup.Show (itemId, itemName, itemLevel);
	}
	
	public void OnClick (UIButton button, UILabel label)
	{
		if (button == _TrainButton) {
			if (label.text == "Train") {
				//start upgrade...
				if (UIManager.instance.generalSwf.onPetStartUpgrade (nextPet.id)) {
					Debug.Log ("Pet upgrade started!");
					SetInventoryData ();
					SoundManager.instance.PlayMenuOkSound();
				}
			} else if (label.text == "Finish now") {//Finish now...
				GameManager._gameState.User.BoostSpiritUpgrade ();
				Debug.Log ("Calling SetInventoryData ::: ");
				SetInventoryData ();
			} else {
				Debug.Log ("Spirit at max level!");
			}
		} else if (button == _equipButton) {
			if (label.text == "Equip") {
				if (GameManager._gameState.User.IsCurrentlyUpgradingPet ()) {
					//show popup saying that you can't equip while upgrading
					UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup ("Sorry!", string.Format ("Can't equip while upgrading."), () => {});
				} else {
					if (GameManager._gameState.User.availablePets.Count <= 0)
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup ("Can't equip!", string.Format ("You don't have a spirit yet."), () => {});
					else {
						Debug.Log ("Spirit Equiped: " + currentPet.id);
						GameManager._gameState.User.spiritId = currentPet.id;
						_equipButtonText.text = "Unequip";
						SoundManager.instance.PlayMenuOkSound();
					}
					//equip current pet...
				}
			} else {
				GameManager._gameState.User.UnequipSpirit ();
				_equipButtonText.text = "Equip";
			}

			GameManager._gameState.User.SetRingsAndPetStats ();
			RefreshUI ();
		}
	}
	
	private void SetStatPassiveLevel (PetModel current, PetModel next, PetModel max)
	{
		foreach (Transform child in renderCamera.transform) 
			GameObject.Destroy (child.gameObject);

		if (max != null) {
			_spiritNameLeftPanel.text = max.name;

			if (current == null)
				_currentSpiritLevelLabel.text = "The " + max.getPetName ();
//			_currentSpiritLevelLabel.text = "The " + max.getPetName ().Replace ("AQUA", "DEEP HORROR");

			var pet = (GameObject)Instantiate (Resources.Load (Constants.SPIRIT_LOOP_SPIRITUI + max.getPetName ()));
//			pet.transform.parent = _spiritCurrentLevelImage.transform;
			pet.transform.parent = renderCamera.transform;
//			pet.transform.localPosition = new Vector3 (0, -120f, -200f);
			pet.transform.localPosition = new Vector3 (8, -96, 418);
//			pet.transform.localPosition = new Vector3 (-798, -106, 330);
			if (max.getPetName ().Equals ("IMP"))
				pet.transform.localScale = new Vector3 (300f, 300f, 300f);
			else if (max.getPetName ().Equals ("AQUA"))
				pet.transform.localScale = new Vector3 (500f, 500f, 500f);
			else if (max.getPetName ().Equals ("DRAKE"))
				pet.transform.localScale = new Vector3 (3600f, 3600f, 3600f);
			else if (max.getPetName ().Equals ("GOLEM"))
				pet.transform.localScale = new Vector3 (900f, 900f, 900f);
								

//			Debug.LogError("SetStatPassiveLevel Init.");
		} else {
			Debug.LogError ("SetStatPassiveLevel Max is NULL.");
			_spiritNameLeftPanel.text = "";
		}

		if (!string.IsNullOrEmpty (max.abilityStat))
			_statLabelLeft.text = max.abilityStat;

		if (current == null || string.IsNullOrEmpty (current.abilityStat)) {
			_statCurrentValueLabel.text = "0%";
			_statCurrentValueFill.fillAmount = 0;
			
			if (string.IsNullOrEmpty (next.abilityStat)) {
				_statDifferenceValueLabel.text = "+ 0%";
				_statDifferenceValueFill.fillAmount = 0;
			} else {
				_statDifferenceValueLabel.text = "+ " + next.buffTime + "%";
				_statDifferenceValueFill.fillAmount = next.buffTime / (float)max.buffTime;
			}
		} else {
			_statCurrentValueLabel.text = current.buffTime + "%";
			_statCurrentValueFill.fillAmount = current.buffTime / (float)max.buffTime;
			_statDifferenceValueLabel.text = "+ " + (next.buffTime - current.buffTime) + "%";
			_statDifferenceValueFill.fillAmount = next.buffTime / (float)max.buffTime;
		}
	}

	private void SetElementPassiveLevel (PetModel current, PetModel next, PetModel max)
	{
		if (!string.IsNullOrEmpty (max.abilityElement))
			_elementLabelLeft.text = max.abilityElement + " Damage";

		if (current == null || string.IsNullOrEmpty (current.abilityElement)) {
			_elementCurrentValueLabel.text = "0%";
			_elementCurrentValueFill.fillAmount = 0;
			
			if (string.IsNullOrEmpty (next.abilityElement)) {
				_elementDifferenceValueLabel.text = "+ 0%";
				_elementDifferenceValueFill.fillAmount = 0;
			} else {
				_elementDifferenceValueLabel.text = "+ " + next.buffTime.ToString()+"%";
				_elementDifferenceValueFill.fillAmount = next.buffTime / (float)max.buffTime;
			}

			//
//			_elementPassiveAbilityLabel.text = "";
			//
		} else {
			_elementCurrentValueLabel.text = current.buffTime + "%";
			_elementCurrentValueFill.fillAmount = current.buffTime / (float)max.buffTime;
			_elementDifferenceValueLabel.text = "+ " + (next.buffTime - current.buffTime) + "%";
			_elementDifferenceValueFill.fillAmount = next.buffTime / (float)max.buffTime;
//			_elementPassiveAbilityLabel.text = current.buffTime+ " -> [FF8C00]"+next.buffTime+"[-]";

		}
	}
	
	private void SetActiveLevel (PetModel current, PetModel next, PetModel max)
	{
		if (!string.IsNullOrEmpty (max.activeSpell))
			_activeLabelLeft.text = max.activeSpell + " Level";

		if (current == null || string.IsNullOrEmpty (current.activeSpell)) {
			_activeCurrentValueFill.fillAmount = 0;
			_activeCurrentValueLable.text = "0%";
			
			_activeDifferenceValueLabel.text = "+ " + next.skullLevel + "%";
			_activeDifferenceValueFill.fillAmount = next.skullLevel / (float)max.skullLevel;

			//
//			_activeAbilityLabel.text = "";
			//
		} else {
			_activeCurrentValueLable.text = current.skullLevel + "%";
			_activeCurrentValueFill.fillAmount = current.skullLevel / (float)max.skullLevel;
			_activeDifferenceValueLabel.text = "+ " + next.skullLevel + "%";
			_activeDifferenceValueFill.fillAmount = next.skullLevel / (float)max.skullLevel;
//			_activeAbilityLabel.text = current.skullLevel+ "+next.skullLevel;
		}
	}
	
	private void Update ()
	{
		InputWrapper.disableTouch = true;
	}

	public void RefreshUI ()
	{
		MaxHealth.text = GameManager._gameState.User.totalLife.ToString ();
		MaxDamage.text = GameManager._gameState.User.damage.ToString ();
		MaxWards.text = GameManager._gameState.User._wards.ToString ();
	}
	
	private void OnDestroy ()
	{
		InputWrapper.disableTouch = false;

		//UIManager.instance.generalSwf.ToggleTopStats (true);
		UIManager.instance.generalSwf.ShowHeartIcon ();
		UIManager.instance.generalSwf.UpdateHealth ();
	}


	private string GetSpriteNameFromAbility(string ability) {
		string spriteName = "";
		switch (ability) {
		case "WARD":
			spriteName = "WardIcon";
			break;
		case "LIFE":
			spriteName = "HealthIconMain";
			break;
		case "DAMAGE":
			spriteName = "TopDamageIconSmall";
			break;
		case "GREED":
			spriteName = "GREED";
			break;
		case "AMPLIFY":
			spriteName = "AMPLIFY";
			break;
		case "SMASH":
			spriteName = "WardIcon";
			break;
		case "DISPEL":
			spriteName = "WardIcon";
			break;
		case "IGNITE":
			spriteName = "IGNITE";
			break;
		case "FIRE":
			spriteName = "FireElement";
			break;
		case "WATER":
			spriteName = "WaterElement";
			break;
		case "EARTH":
			spriteName = "EarthElement";
			break;
		case "LIGHTNING":
			spriteName = "LightningElement";
			break;
		}
		Debug.LogError("ability = "+ability +" spritename = "+spriteName);

		return spriteName;
	}
}
