using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class User : CharacterModel
{

	public const int DEBUG_TIMER_CUT = 1;
	
	public enum PetType
	{
		IMP,
		DRAKE,
		GOLEM,
		AQUA
	}
	
	public List<string> availablePets = new List<string> ();
	
	#region Current upgrade fields
	private const string PET_OLD_ID = "OldPetId";
	private const string PET_NEW_ID = "NewPetId";
	private const string PET_BOOST_COST = "bCost";
	private const string PET_UPGRADE_TYPE = "Type";
	private const string PET_UPGRADE_UID = "uid";
	private const string PET_TIME_REMAINING = "TimeRemaining";
	private const string PET_TOTAL_TIME = "TotalTime";
	#endregion
	
	#region Tag that always exist
	public const string TAG_CURRENT_UPGRADE = "CurrentUpgrade";
	public const string TAG_IMP_CURRENT = "IMP_Current";
	public const string TAG_IMP_NEXT = "IMP_Next";
	public const string TAG_IMP_MAX = "IMP_Max";
	
	public const string TAG_GOLEM_CURRENT = "GOLEM_Current";
	public const string TAG_GOLEM_NEXT = "GOLEM_Next";
	public const string TAG_GOLEM_MAX = "GOLEM_Max";
	
	public const string TAG_AQUA_CURRENT = "AQUA_Current";
	public const string TAG_AQUA_NEXT = "AQUA_Next";
	public const string TAG_AQUA_MAX = "AQUA_Max";
	
	public const string TAG_DRAKE_CURRENT = "DRAKE_Current";
	public const string TAG_DRAKE_NEXT = "DRAKE_Next";
	public const string TAG_DRAKE_MAX = "DRAKE_Max";
	
	#endregion
	
	#region Default pet upgrade dictionary
	[Newtonsoft.Json.JsonIgnore]
	private IDictionary
		_emptySpiritDictionary = new Dictionary<string, object> ();
	[Newtonsoft.Json.JsonIgnore]
	public IDictionary emptySpiritDictionary {
		get {	
			DatabankSystem.Databank dataBank = GameManager._dataBank;
			
			_emptySpiritDictionary [TAG_CURRENT_UPGRADE] = null;
			
			_emptySpiritDictionary [TAG_IMP_CURRENT] = null;
			_emptySpiritDictionary [TAG_GOLEM_CURRENT] = null;
			_emptySpiritDictionary [TAG_AQUA_CURRENT] = null;
			_emptySpiritDictionary [TAG_DRAKE_CURRENT] = null;
			
			_emptySpiritDictionary [TAG_IMP_MAX] = GameManager._dataBank.GetMaxLevelImp ();
			_emptySpiritDictionary [TAG_GOLEM_MAX] = GameManager._dataBank.GetMaxLevelGolem ();
			_emptySpiritDictionary [TAG_AQUA_MAX] = GameManager._dataBank.GetMaxLevelAqua ();
			_emptySpiritDictionary [TAG_DRAKE_MAX] = GameManager._dataBank.GetMaxLevelDrake ();
			
			_emptySpiritDictionary [TAG_IMP_NEXT] = GameManager._dataBank.GetImpForLevel (1);
			_emptySpiritDictionary [TAG_GOLEM_NEXT] = GameManager._dataBank.GetGolemForLevel (1);
			_emptySpiritDictionary [TAG_AQUA_NEXT] = GameManager._dataBank.GetAquaForLevel (1);
			_emptySpiritDictionary [TAG_DRAKE_NEXT] = GameManager._dataBank.GetDrakeForLevel (1);
			
			string petId = "";
			string petName = "";
			int petLevel = -1;
			int count = availablePets.Count;
			for (int i = 0; i < count; i++) {
				petId = availablePets [i];
				petName = petId.Substring (0, petId.Length - 1);
				petLevel = System.Int32.Parse (petId.Substring (petId.Length - 1, 1));
				
				switch (petName) {
				case "IMP":
					_emptySpiritDictionary [TAG_IMP_CURRENT] = GameManager._dataBank.GetImpForLevel (petLevel);
					if (petLevel < 5)
						_emptySpiritDictionary [TAG_IMP_NEXT] = GameManager._dataBank.GetImpForLevel (petLevel + 1);
					else
						_emptySpiritDictionary [TAG_IMP_NEXT] = GameManager._dataBank.GetMaxLevelImp ();
					break;
				case "GOLEM":
					_emptySpiritDictionary [TAG_GOLEM_CURRENT] = GameManager._dataBank.GetGolemForLevel (petLevel);
					if (petLevel < 5)
						_emptySpiritDictionary [TAG_GOLEM_NEXT] = GameManager._dataBank.GetGolemForLevel (petLevel + 1);
					else
						_emptySpiritDictionary [TAG_GOLEM_NEXT] = GameManager._dataBank.GetMaxLevelGolem ();
					break;
				case "AQUA":
					_emptySpiritDictionary [TAG_AQUA_CURRENT] = GameManager._dataBank.GetAquaForLevel (petLevel);
					if (petLevel < 5)
						_emptySpiritDictionary [TAG_AQUA_NEXT] = GameManager._dataBank.GetAquaForLevel (petLevel + 1);
					else
						_emptySpiritDictionary [TAG_AQUA_NEXT] = GameManager._dataBank.GetMaxLevelAqua ();
					break;
				case "DRAKE":
					_emptySpiritDictionary [TAG_DRAKE_CURRENT] = GameManager._dataBank.GetDrakeForLevel (petLevel);
					if (petLevel < 5)
						_emptySpiritDictionary [TAG_DRAKE_NEXT] = GameManager._dataBank.GetDrakeForLevel (petLevel + 1);
					else
						_emptySpiritDictionary [TAG_DRAKE_NEXT] = GameManager._dataBank.GetMaxLevelDrake ();
					break;
				}
			}
			
			return _emptySpiritDictionary;
		}
		private set {
			_emptySpiritDictionary = value;
		}
	}
	#endregion
	
	private IDictionary _petUpgradeDictionary = new Dictionary<string, object> ();
	public IDictionary petUpgradeDictionary {
		get {
			if (_petUpgradeDictionary == null || _petUpgradeDictionary [TAG_CURRENT_UPGRADE] == null)
				return emptySpiritDictionary;
			return _petUpgradeDictionary;
		}
		set {
			_petUpgradeDictionary = value;
			IDictionary currentDictionary = _petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
			
			if (currentDictionary != null) {
				if (currentDictionary [PET_NEW_ID].ToString ().Equals (spiritId))
					spiritId = "";
			}
		}
	}
	
	public void AddToAvailablePets (string petId)
	{
		availablePets.Add (petId);
	}
	
	public IDictionary UpgradePet (string petId)
	{
		string petName = petId.Substring (0, petId.Length - 1);
		
		string prevUpgradePetId = PreviousUpgradePetId (petId);
		IDictionary upgradedPet = GameManager._dataBank.GetSpiritDictionaryForID (petId);
		
		string uid = Nonce.GetUniqueID ();
		string boostUid = Nonce.GetUniqueID ();

		PurchaseRequest.PurchaseRequestType responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.PetUpgrade, uid, "", 0, System.Int32.Parse (upgradedPet ["dCost"].ToString ()), System.Int32.Parse (upgradedPet ["SkullLevel"].ToString ()), petId); 
		if (responseType == PurchaseRequest.PurchaseRequestType.Error)
			return null;
		
		GameManager.instance.scaleformCamera.generalSwf.Init ();
		GameManager.instance.SaveGameState (false);

		return new PetUpgradeProgress (prevUpgradePetId, GetPetTypeForString (petName), petId, GameManager._dataBank.GettCostForSkullLevel (System.Int32.Parse (upgradedPet ["SkullLevel"].ToString ())), uid, boostUid, -1).ToDictionary (); // 10 is the boost cost !
	}
	
	private class PetUpgradeProgress
	{
		string oldPetId;
		PetType petType;
		string newPetId;
		int boostCost;
		long timeRemaining;
		string uid;
		string boostUid;
		
		public PetUpgradeProgress (string oldPetId, PetType petType, string newPetId, int boostCost, string uid = null, string boostUid = null, long timeRemaining = -1)
		{
			this.oldPetId = oldPetId;
			this.petType = petType;
			this.newPetId = newPetId;
			this.boostCost = boostCost;
			this.boostUid = boostUid;
			this.timeRemaining = timeRemaining;
			this.uid = uid;
		}
		
		public IDictionary ToDictionary ()
		{
			IDictionary petUpgradeDictionary = new Dictionary<string, object> ();
			petUpgradeDictionary [PET_OLD_ID] = oldPetId;
			petUpgradeDictionary [PET_UPGRADE_TYPE] = petType.ToString ();
			petUpgradeDictionary [PET_NEW_ID] = newPetId;
			
			PetModel upgradedPet = new PetModel (newPetId);
			
			petUpgradeDictionary [PET_BOOST_COST] = GameManager._dataBank.GetbCostForSkullLevel (upgradedPet.skullLevel);
			petUpgradeDictionary [PET_TIME_REMAINING] = timeRemaining; 
			petUpgradeDictionary [PET_TOTAL_TIME] = GameManager._dataBank.GetUTimeForSkullLevel (upgradedPet.skullLevel);
			
			if (this.boostUid != null)
				petUpgradeDictionary ["Boostuid"] = this.boostUid;
			else
				petUpgradeDictionary ["Boostuid"] = Nonce.GetUniqueID ();
				
			if (uid != null)
				petUpgradeDictionary [PET_UPGRADE_UID] = uid;
			
			IDictionary defaultDictionary = GameManager._gameState.User.emptySpiritDictionary;
			defaultDictionary [TAG_CURRENT_UPGRADE] = petUpgradeDictionary;
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("MiniJSON.Json.Serialize(defaultDictionary)" + MiniJSON.Json.Serialize (defaultDictionary));
			
			return defaultDictionary;
		}
	}
	
	public bool IsCurrentlyUpgradingPet ()
	{
		IDictionary currentDictionary = _petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
			
		if (currentDictionary != null)
			return true;
		return false;
	}
	
	public void EquipSpirit (string petId)
	{
		if (petUpgradeDictionary [TAG_CURRENT_UPGRADE] != null) {
			object objPetId = (petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary) [PET_OLD_ID];
			if (objPetId != null && !objPetId.ToString ().Equals (petId))
				spiritId = petId;
		}
	}
	
	public void UnequipSpirit ()
	{
		spiritId = "";
		GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData (this.json, false);
	}
	
	private PetType GetPetTypeForString (string petTypeString)
	{
		switch (petTypeString) {
		case "IMP":
			return PetType.IMP;
		case "DRAKE":
			return PetType.DRAKE;
		case "AQUA":
			return PetType.AQUA;
		default :
			return PetType.GOLEM;
		}
	}
	
	private string NextUpgradePetId (string petId)
	{
		string petName = petId.Substring (0, petId.Length - 1);
		int petLevel = Int32.Parse (petId.Substring (petId.Length - 1, 1));
		
		if (petLevel < 5)
			return petName + (++petLevel);
		return "";
	}
	
	private string PreviousUpgradePetId (string petId)
	{
		string petName = petId.Substring (0, petId.Length - 1);
		int petLevel = Int32.Parse (petId.Substring (petId.Length - 1, 1));
		
		if (petLevel <= 1)
			return null;
		else
			return petName + (--petLevel);
	}
	
	
	
	public bool OnPetStartUpgrade (string newPetId)
	{
		Debug.LogError ("newPetId > " + newPetId);
		int skullLevel = 0;
		string petName = newPetId.Substring (0, newPetId.Length - 1);
		
		switch (petName) {
		case "IMP":
			skullLevel = Int32.Parse ((this.petUpgradeDictionary [TAG_IMP_NEXT] as IDictionary) ["SkullLevel"].ToString ());
			break;
		case "GOLEM":
			skullLevel = Int32.Parse ((this.petUpgradeDictionary [TAG_GOLEM_NEXT] as IDictionary) ["SkullLevel"].ToString ());
			break;
		case "DRAKE":
			skullLevel = Int32.Parse ((this.petUpgradeDictionary [TAG_DRAKE_NEXT] as IDictionary) ["SkullLevel"].ToString ());
			break;
		case "AQUA":
			skullLevel = Int32.Parse ((this.petUpgradeDictionary [TAG_AQUA_NEXT] as IDictionary) ["SkullLevel"].ToString ());
			break;
		}
		
		if (IsCurrentlyUpgradingPet ()) {
			IDictionary upgradeDictionary = petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary; 
			
			string a_uid = upgradeDictionary [PET_UPGRADE_UID].ToString ();
			
			ServerRequestParam requestParam = new ServerRequestParam (a_uid, "", skullLevel, newPetId, 0);
			ServerManager.Instance.ProcessRequest (new SpiritRequest (ServerRequest.ServerRequestType.Spirit, this.ProcessSpiritResponse, requestParam));

			return false;
		}
		
		IDictionary newUpgradeDictionary = UpgradePet (newPetId);
		if (newUpgradeDictionary == null)
			return false;
		
		IDictionary currentUpgrade = newUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
		if (currentUpgrade != null)
			this.petUpgradeDictionary = newUpgradeDictionary;
		else 
			return false;
		
		string uid = currentUpgrade [PET_UPGRADE_UID].ToString ();
		string boostUid = currentUpgrade ["Boostuid"].ToString ();
		if (uid != null) {
			GameManager.instance.scaleformCamera.generalSwf.UpgradeStarted (); 
			
			PurchaseManager.Instance.CommitTransaction (uid, false);
			
			GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData (-1 + "|" + currentUpgrade [PET_BOOST_COST].ToString (), true);
			
			ServerRequestParam requestParam = new ServerRequestParam (uid, "", skullLevel, newPetId, 0);
			ServerManager.Instance.ProcessRequest (new SpiritRequest (ServerRequest.ServerRequestType.Spirit, this.ProcessSpiritResponse, requestParam));
		}
		GameManager.instance.SaveGameState (false);

		return true;
	}
	
	public void BoostSpiritUpgrade ()
	{
		if (!IsCurrentlyUpgradingPet ())
			return; 
		
		string activity = "Boost Spirit Upgrade";

		IDictionary spiritDictionary = this.petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
		
		string uid = spiritDictionary ["uid"].ToString ();
		string boostUid = spiritDictionary ["Boostuid"].ToString ();
		int gems = Int32.Parse (spiritDictionary ["bCost"].ToString ());

		PetModel upgradedPet = new PetModel (spiritDictionary ["NewPetId"].ToString ());
		int skullLevel = upgradedPet.skullLevel;
		string id = upgradedPet.id;
			
		PurchaseRequest.PurchaseRequestType responseType = PurchaseRequest.PurchaseRequestType.Error;
		responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.PetUpgradeBoost, boostUid, uid, gems, 0, skullLevel, id);
		if (responseType == PurchaseRequest.PurchaseRequestType.Success) {
			GameManager.instance.scaleformCamera.generalSwf.Init ();
			//if(PurchaseManager.Instance.IsParentCommited(uid)) //No need to check this anymore...! 
			//{
			ServerRequestParam requestParam = new ServerRequestParam (uid, boostUid, skullLevel, id, 1);
			ServerManager.Instance.ProcessRequest (new SpiritRequest (ServerRequest.ServerRequestType.SpiritCompleted, this.ProcessSpiritResponse, requestParam));
			
			PurchaseManager.Instance.CommitTransaction (boostUid, false);
			PurchaseManager.Instance.CompleteTransaction (boostUid, uid);
			
			PurchaseManager.Instance.MarkCommittedAndCompleteTrue (uid);
			
			
			OnSpiritUpgradeComplete ();
			NotificationManager.CancelLocalNotification(NotificationManager.NotificationType.Spirit);
			SoundManager.instance.PlayMenuOkSound();
			//}
			//else
			//PurchaseManager.Instance.CommitTransaction(boostUid, false);
			
		} else {
			if (gems > 0)
				PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - GameManager._gameState.User._inventory.gems, activity, "Buy Gems");
		}
	}
	
	private PurchaseRequest.PurchaseRequestType PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType purchaseType, string uid, string parentUid, int gCost, int dCost, int skullLevel, string itemId)
	{
		TransactionRequest tRequest = new TransactionRequest (purchaseType, uid, parentUid, gCost, dCost, skullLevel, itemId);
		return PurchaseManager.Instance.PerformTransaction (tRequest);
	}
	
	public void OnSpiritUpgradeComplete ()
	{
		Debug.LogError("???????? ON SPIRIT UPGRADE COMPLETED //////////");
		if (!IsCurrentlyUpgradingPet ())
			return;
		
		IDictionary spiritDictionary = this.petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;

		this.petUpgradeDictionary [TAG_CURRENT_UPGRADE] = null;
		AddToAvailablePets (spiritDictionary ["NewPetId"].ToString ());

		Debug.Log ("OnSpiritUpgradeComplete: " + spiritDictionary ["NewPetId"].ToString ());
		//GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData(this.json, false);
		if (UIManager.instance.generalSwf.spiritsUI != null) {
			UIManager.instance.generalSwf.spiritsUI.SetInventoryData (spiritDictionary ["NewPetId"].ToString ());
			PetModel newPet = new PetModel (spiritDictionary ["NewPetId"].ToString ());
//			Debug.LogError(" new pet - "+newPet.id+" - "+ newPet.getPetName()+" - "+newPet.uLevel);
			UIManager.instance.generalSwf.spiritsUI.ShowSpiritsCompletePopup(newPet.id, newPet.getPetName(), newPet.uLevel.ToString());
		}
	}
	
	
	public void ProcessSpiritResponse (ServerResponse response)
	{
		IDictionary petDictionary = this.petUpgradeDictionary;
		IDictionary currentPetUpgradeDictionary = petDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
		
		switch (response.Request.RequestType) {
		case ServerRequest.ServerRequestType.All:
			
			
			UpdateResponse updateResponse = (UpdateResponse)response;
			if (updateResponse.Spirit != null) { //Case when pet upgrade is in progress
				
				if (currentPetUpgradeDictionary != null) {
					if (currentPetUpgradeDictionary ["uid"].ToString ().Equals (updateResponse.Spirit.TransactionId)) {
						PetModel nextPet = GetNextPetFromUpgrade ();
					
						int petTotalBoostCost = GameManager._dataBank.GetbCostForSkullLevel (nextPet.skullLevel);
						double petTotalTimeRemaining = Convert.ToDouble (GameManager._dataBank.GetUTimeForSkullLevel (nextPet.skullLevel) / DEBUG_TIMER_CUT);
						double petCurrentTimeRemaining = Convert.ToDouble (updateResponse.Spirit.RemainingTime);
						double timeRatio = petCurrentTimeRemaining / petTotalTimeRemaining;

						int newBoostCost = Convert.ToInt32 (timeRatio * petTotalBoostCost);
						if (GameManager.PRINT_LOGS)
							Debug.Log ("newBoostCost = " + newBoostCost);
						currentPetUpgradeDictionary [PET_TIME_REMAINING] = petCurrentTimeRemaining;
						currentPetUpgradeDictionary [PET_BOOST_COST] = newBoostCost;
						GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData (currentPetUpgradeDictionary ["TimeRemaining"].ToString () + "|" + currentPetUpgradeDictionary [PET_BOOST_COST].ToString (), true);
					
					} else {
						if (currentPetUpgradeDictionary != null) {
							OnPetStartUpgrade (currentPetUpgradeDictionary ["NewPetId"].ToString ());
						}
					}
			
				}
				return;
			} else {
				if (currentPetUpgradeDictionary != null) {
					OnPetStartUpgrade (currentPetUpgradeDictionary ["NewPetId"].ToString ());
				}
			}
			break;
			
		case ServerRequest.ServerRequestType.Spirit: 
			SpiritResponse spiritResponse = (SpiritResponse)response;
			if (spiritResponse.Spirit != null) {
				IDictionary upgradeDictionary = petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
				if (upgradeDictionary == null)
					return;
				if (upgradeDictionary ["uid"].ToString ().Equals (spiritResponse.Spirit.TransactionId)) {
					PurchaseManager.Instance.CommitTransaction (spiritResponse.Spirit.TransactionId, spiritResponse.IsSuccess);
					if (spiritResponse.IsSuccess) {
						upgradeDictionary ["TimeRemaining"] = spiritResponse.Spirit.RemainingTime;
						GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData (upgradeDictionary ["TimeRemaining"].ToString () + "|" + upgradeDictionary [PET_BOOST_COST].ToString (), true);
						
						if (spiritResponse.Spirit.IsCompleted) {
							if (spiritResponse.Spirit.Boost == 1) {
								Analytics.logEvent (Analytics.EventName.Upgrade_Boost);
								PurchaseManager.Instance.CompleteTransaction (spiritResponse.Spirit.BoostId, spiritResponse.Spirit.TransactionId);
							} else {
								Analytics.logEvent (Analytics.EventName.Upgrade_Start); //Sure? Upgrade ends here, no? But then, shouldn't it be in SpiritComplete switch case?
								PurchaseManager.Instance.CompleteTransaction (spiritResponse.Spirit.TransactionId, "");
							}
						}
					}
				}
			}

			break;
		case ServerRequest.ServerRequestType.SpiritCompleted:
			string upgradeJson = "-1";
			spiritResponse = (SpiritResponse)response;
			if (spiritResponse.Spirit != null) {
				IDictionary upgradeDictionary = petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
				if (upgradeDictionary == null) // means that before it was completed, next upgrade was started!
					return;
				if (upgradeDictionary ["uid"].ToString ().Equals (spiritResponse.Spirit.TransactionId) && upgradeDictionary ["Boostuid"].ToString ().Equals (spiritResponse.Spirit.BoostId)) {
					//if(spiritResponse.Spirit.Boost == 1)
					//{
					//	PurchaseManager.Instance.CommitTransaction(spiritResponse.Spirit.BoostId, spiritResponse.IsSuccess);
					//}
					if (spiritResponse.IsSuccess && spiritResponse.Spirit.IsCompleted) {
						upgradeJson = GameManager._gameState.User.json;
							
						if (spiritResponse.Spirit.Boost == 1) {
							GameManager.instance.scaleformCamera.generalSwf.BoostResult (true);
							PurchaseManager.Instance.CompleteTransaction (spiritResponse.Spirit.BoostId, spiritResponse.Spirit.TransactionId);
						} else {
							PurchaseManager.Instance.CommitTransaction (spiritResponse.Spirit.TransactionId, false);
							PurchaseManager.Instance.CompleteTransaction (spiritResponse.Spirit.TransactionId, null);
						}
						Analytics.logEvent (Analytics.EventName.Upgrade_Collect);
					}
						
					OnSpiritUpgradeComplete ();
				}
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("ServerRequest.ServerRequestType.SpiritCompleted :::: spiritResponse = (SpiritResponse)response is NULL");
					
			}
				
				
			break;
		}
		UpdateUIButtons ();
	}
	
	public PetModel GetCurrentPetFromUpgrade ()
	{
		if (!IsCurrentlyUpgradingPet ()) 
			return null;
		
		IDictionary upgradeInfo = petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
		string currentPetID = null;
		PetModel currentPet = null;
		if (upgradeInfo [PET_OLD_ID] != null) {
			currentPetID = upgradeInfo [PET_OLD_ID].ToString ();
			currentPet = new PetModel (currentPetID);
		}
		 
		return currentPet;
	}
	
	public PetModel GetNextPetFromUpgrade ()
	{
		if (!IsCurrentlyUpgradingPet ()) 
			return null;
		
		IDictionary upgradeInfo = petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary;
		string nextPetID = upgradeInfo [PET_NEW_ID].ToString ();
		PetModel nextPet = new PetModel (nextPetID);
		return nextPet;
	}
	
	public void UpdateUIButtons ()
	{
		GameManager.instance.scaleformCamera.generalSwf.GetCurrentPet ();
	}
	
	//This is a CB function... don't call it directly. It will get called on response of UpdateUIButtons BY scaleform
	public void UpdateUIButtons (string selectedPetId)
	{
		string selectedPetName = selectedPetId.Substring (0, selectedPetId.Length - 1);
		if (IsCurrentlyUpgradingPet ()) {
			string upgradingPetId = (this.petUpgradeDictionary [TAG_CURRENT_UPGRADE] as IDictionary) [PET_NEW_ID].ToString ();
			string upgradingPetName = upgradingPetId.Substring (0, upgradingPetId.Length - 1);
			
			if (selectedPetName.Equals (upgradingPetName)) {
				GameManager.instance.scaleformCamera.generalSwf.HideUnEquipButton ();
				GameManager.instance.scaleformCamera.generalSwf.HideEquipButton ();
				return;
			}
		}
		
		string spiritName = GetCurrentSpiritName ();
		if (spiritName == null) {
			GameManager.instance.scaleformCamera.generalSwf.HideUnEquipButton ();
			GameManager.instance.scaleformCamera.generalSwf.ShowEquipButton ();
		} else {
			GameManager.instance.scaleformCamera.generalSwf.HideEquipButton ();
			GameManager.instance.scaleformCamera.generalSwf.ShowUnEquipButton ();
		}
	}
	
	public void EquipPet (string petId)
	{
		spiritId = petId;
		GameManager.instance.scaleformCamera.generalSwf.SetPetUpgradeData (this.json, false);
	}
	
	private string GetCurrentSpiritName ()
	{
		string currentName;
		if (string.IsNullOrEmpty (spiritId))
			currentName = null;
		else
			currentName = spiritId.Substring (0, spiritId.Length - 1);
		return currentName;
	}
	
}
