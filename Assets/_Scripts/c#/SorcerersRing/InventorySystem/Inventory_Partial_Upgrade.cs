using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace InventorySystem
{
	[System.Serializable]
	public partial class Inventory
	{
		public const string TAG_CURRENT_UPGRADE = "CurrentUpgrade";
		public const string TAG_UPGRADE_INFO = "UpgradeInfo";
		
		public const string TAG_CURRENT_BAG = "CurrentBag";
		public const string TAG_NEXT_BAG_UPGRADE = "NextBagUpgrade";
		
		public const string TAG_CURRENT_KEY_RING = "CurrentKeyRing";
		public const string TAG_NEXT_KEY_RING_UPGRADE = "NextKeyRingUpgrade";
		
		public const string TAG_CURRENT_POTION_BELT = "CurrentPotionBelt";
		public const string TAG_NEXT_KEY_POTION_BELT_UPGRADE = "NextPotionBeltUpgrade";
		
		public const string TAG_CURRENT_TCUBE = "CurrentTCube";
		public const string TAG_NEXT_TCUBE_UPGRADE = "NextTCubeUpgrade";
		
		#region upgrades empty
		[Newtonsoft.Json.JsonIgnore]
		private IDictionary
			_emptyUpgradesDictionary = new Dictionary<string, object> ();
		[Newtonsoft.Json.JsonIgnore]
		public IDictionary emptyUpgradesDictionary {
			get {
				_emptyUpgradesDictionary [TAG_CURRENT_UPGRADE] = UpgradeType.kNone;
				for (int i = 0; i< GameManager._dataBank.sortedBagList.Count; i++) {
					if (GameManager._dataBank.sortedBagList [i].level == this.bag.level) {
						if (i < GameManager._dataBank.sortedBagList.Count - 1)
							_emptyUpgradesDictionary [TAG_NEXT_BAG_UPGRADE] = GameManager._dataBank.sortedBagList [i + 1].ToDictionary ();
						else
							_emptyUpgradesDictionary [TAG_NEXT_BAG_UPGRADE] = GameManager._dataBank.GetMaxLevelBag ().ToDictionary ();
					}
				}
				for (int i = 0; i< GameManager._dataBank.sortedKeyRingList.Count; i++) {
					if (GameManager._dataBank.sortedKeyRingList [i].level == this.keyRing.level) {
						if (i < GameManager._dataBank.sortedKeyRingList.Count - 1)
							_emptyUpgradesDictionary [TAG_NEXT_KEY_RING_UPGRADE] = GameManager._dataBank.sortedKeyRingList [i + 1].ToDictionary ();
						else
							_emptyUpgradesDictionary [TAG_NEXT_KEY_RING_UPGRADE] = GameManager._dataBank.GetMaxLevelKeyRing ().ToDictionary ();
					}
				}
				for (int i = 0; i< GameManager._dataBank.sortedTCubeList.Count; i++) {
					if (GameManager._dataBank.sortedTCubeList [i].level == this.transmutationCube.level) {
						if (i < GameManager._dataBank.sortedTCubeList.Count - 1)
							_emptyUpgradesDictionary ["NextTCubeUpgrade"] = GameManager._dataBank.sortedTCubeList [i + 1].ToDictionary ();
						else
							_emptyUpgradesDictionary ["NextTCubeUpgrade"] = GameManager._dataBank.GetMaxLevelTCube ().ToDictionary ();
					}
				}
				for (int i = 0; i< GameManager._dataBank.sortedPotionBeltList.Count; i++) {
					if (GameManager._dataBank.sortedPotionBeltList [i].level == this.potionBelt.level) {
						if (i < GameManager._dataBank.sortedPotionBeltList.Count - 1) {
							_emptyUpgradesDictionary [TAG_NEXT_KEY_POTION_BELT_UPGRADE] = GameManager._dataBank.sortedPotionBeltList [i + 1].ToDictionary ();
						} else {
							_emptyUpgradesDictionary [TAG_NEXT_KEY_POTION_BELT_UPGRADE] = GameManager._dataBank.GetMaxLevelPotionBelt ().ToDictionary ();
						}
					}
				}
				_emptyUpgradesDictionary [TAG_CURRENT_BAG] = GameManager._gameState.User._inventory.bag.ToDictionary ();

				_emptyUpgradesDictionary [TAG_CURRENT_POTION_BELT] = GameManager._gameState.User._inventory.potionBelt.ToDictionary ();
				_emptyUpgradesDictionary [TAG_CURRENT_KEY_RING] = GameManager._gameState.User._inventory.keyRing.ToDictionary ();
				_emptyUpgradesDictionary [TAG_CURRENT_TCUBE] = GameManager._gameState.User._inventory.transmutationCube.ToDictionary ();
				
				_emptyUpgradesDictionary ["MaxLevelBag"] = GameManager._dataBank.GetMaxLevelBag ().ToDictionary ();
				_emptyUpgradesDictionary ["MaxLevelPotionBelt"] = GameManager._dataBank.GetMaxLevelPotionBelt ().ToDictionary ();
				_emptyUpgradesDictionary ["MaxLevelKeyRing"] = GameManager._dataBank.GetMaxLevelKeyRing ().ToDictionary ();
				_emptyUpgradesDictionary ["MaxLevelTCube"] = GameManager._dataBank.GetMaxLevelTCube ().ToDictionary ();
				
				return _emptyUpgradesDictionary;
			}
		}
		#endregion
		
		
		#region Upgrades Container

		public enum UpgradeType
		{
			kNone,
			kBag,
			kTCube,
			kKeyRing,
			kPotionBelt
		}
		;
		
		public IDictionary upgradesDictionary = new Dictionary<string, object> ();
		
		public IDictionary LoadUpgradesDictionary (IDictionary uDictionary)
		{
			string currentUpgrade = uDictionary [TAG_CURRENT_UPGRADE].ToString ();
			uDictionary = emptyUpgradesDictionary;
			uDictionary [TAG_CURRENT_UPGRADE] = currentUpgrade;
			if (uDictionary [TAG_CURRENT_UPGRADE].Equals (UpgradeType.kBag + ""))
				uDictionary [TAG_UPGRADE_INFO] = LoadBagProgress (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) as object;
			else if (uDictionary [TAG_CURRENT_UPGRADE].Equals (UpgradeType.kPotionBelt + ""))
				uDictionary [TAG_UPGRADE_INFO] = LoadPotionBeltProgress (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) as object;
			else if (uDictionary [TAG_CURRENT_UPGRADE].Equals (UpgradeType.kTCube + ""))
				uDictionary [TAG_UPGRADE_INFO] = LoadTCubeProgress (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) as object;
			else if (uDictionary [TAG_CURRENT_UPGRADE].Equals (UpgradeType.kKeyRing + ""))
				uDictionary [TAG_UPGRADE_INFO] = LoadKeyRingProgress (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) as object;
			
			upgradesDictionary = uDictionary;
			
			return uDictionary;
		}
		#endregion
		
		#region Bag Upgrade
		private IDictionary _bagUpgradeProgressDictionary;
		public IDictionary bagUpgradeProgressDictionary {
			get {
				return _bagUpgradeProgressDictionary;
			}
			set {
				_bagUpgradeProgressDictionary = value;
				//upgradesDictionary["BagUpgrade"] = _bagUpgradeProgressDictionary; //Wtf is this line ? Why did i write it ? :|
			}
			
		}
		public class BagUpdateProgress
		{
			//private Bag oldBag;
			private Bag newBag;
			long endTime;
			long currentTime;
			int dCost;
			private string uid;
			private string boostUid;
			
			public BagUpdateProgress (/*Bag oldBag,*/long currentTime, long endTime, Bag newBag, string uid = null, string boostUid = null, long timeRemaining = -1)
			{
				//this.oldBag = oldBag;
				this.newBag = newBag;
				this.currentTime = currentTime;
				this.endTime = endTime;
				this.timeRemaining = timeRemaining;
				this.uid = uid;
				this.boostUid = boostUid;
			}
			
			private double _timeRemaining = -1;
			public double timeRemaining {
				get {
					return _timeRemaining;
				}
				private set{ _timeRemaining = value;}
			}
			
			public IDictionary ToDictionary ()
			{
				IDictionary bagUpgradeDictionary = new Dictionary<string, object> ();
				
				bagUpgradeDictionary ["CurrentTime"] = this.currentTime;
				bagUpgradeDictionary ["EndTime"] = this.endTime;
				bagUpgradeDictionary ["TimeRemaining"] = this.timeRemaining;
				bagUpgradeDictionary ["BoostCost"] = GameManager._dataBank.GetbCostForSkullLevel (newBag.skullLevel);
				if (this.boostUid != null)
					bagUpgradeDictionary ["boostId"] = this.boostUid;
				
				if (this.uid != null)
					bagUpgradeDictionary ["transactionId"] = this.uid;
				
				return bagUpgradeDictionary;
			}
		}
		
		public IDictionary LoadBagProgress (IDictionary upgradesDictionary)
		{
			string uid;
			string boostUid;
			
			if (!upgradesDictionary.Contains ("transactionId"))
				uid = Nonce.GetUniqueID ();
			else
				uid = upgradesDictionary ["transactionId"].ToString ();
			
			if (!upgradesDictionary.Contains ("boostId"))
				boostUid = Nonce.GetUniqueID ();
			else
				boostUid = upgradesDictionary ["boostId"].ToString ();
			
			//IDictionary currentBagDictionary = this.upgradesDictionary[TAG_CURRENT_BAG] as IDictionary;
			IDictionary newBagDictionary = this.upgradesDictionary [TAG_NEXT_BAG_UPGRADE] as IDictionary;
			
			//Bag oldBag = GameManager._dataBank.GetBagForID(currentBagDictionary["id"].ToString());
			Bag newBag = GameManager._dataBank.GetBagForID (newBagDictionary ["id"].ToString ());
			
			long endTime = long.Parse (upgradesDictionary ["EndTime"].ToString ());
			long currentTime = long.Parse (upgradesDictionary ["CurrentTime"].ToString ());
			long timeRemaining = -1;//long.Parse(upgradesDictionary["TimeRemaining"].ToString());
			return new BagUpdateProgress (/*oldBag,*/currentTime, endTime, newBag, uid, boostUid, timeRemaining).ToDictionary ();
		}
		
		public IDictionary UpgradeBag (long endTime = -1, long currentTime = -1)
		{
			Debug.Log ("UpgradeBag Called...");
			if (isCurrentlyUpgrading ())
				return null;
			
			Bag newBag = null;
			for (int i = 0; i<GameManager._dataBank.sortedBagList.Count; i++) {
				if (GameManager._dataBank.sortedBagList [i].level == this.bag.level) {
					if (this.bag.level != GameManager._dataBank.GetMaxLevelBag ().level)
						newBag = GameManager._dataBank.sortedBagList [i + 1].Clone ();
					else
						newBag = GameManager._dataBank.GetMaxLevelBag ();
				}
			}
			
			if (this.bag.BagCount () >= newBag.capacity)
				return null;
			
			string uid = Nonce.GetUniqueID ();
			string boostUid = Nonce.GetUniqueID ();
			PurchaseRequest.PurchaseRequestType responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.BagUpgrade, uid, "", (int)newBag.gCost, (int)newBag.dCost, newBag.skullLevel, newBag.id);
			if (responseType == PurchaseRequest.PurchaseRequestType.Error) {
				Debug.Log ("responseType == PurchaseRequest.PurchaseRequestType.Error");
				if (newBag.gCost > 0) {
					PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, (int)(newBag.gCost - GameManager._gameState.User._inventory.gems), "Upgrade Bag", "Buy Gems");
				}
				return null;
			}
			return new BagUpdateProgress (/*this.bag,*/currentTime, endTime, newBag, uid, boostUid).ToDictionary ();
		}
		
		public void OnBagUpgradeMatured ()
		{
			Bag newBag = null;
			for (int i = 0; i<GameManager._dataBank.sortedBagList.Count; i++) {
				if (GameManager._dataBank.sortedBagList [i].level == this.bag.level) {
					if (this.bag.level != GameManager._dataBank.GetMaxLevelBag ().level)
						newBag = GameManager._dataBank.sortedBagList [i + 1].Clone ();
					else
						newBag = GameManager._dataBank.GetMaxLevelBag ();
				}
			}

			//Show bag upgrade popup...
			UIManager.instance.generalSwf.upgradeUI.ShowUpgradeCompletePopup (newBag.id, newBag.id, newBag.level.ToString ());
			
			this.bag.id = newBag.id;
			this.bag.level = newBag.level;
			this.bag.dCost = newBag.dCost;
			this.bag.level = newBag.level;
			this.bag.skullLevel = newBag.skullLevel;
			this.bag.uTime = newBag.uTime;
			this.bag.capacity = newBag.capacity;
			this.bag.soulCapacity = newBag.soulCapacity;
			
			ResetUpgradeDictionary ();
		}
		#endregion
		
		
		#region TransmutationCube Upgrade
		
		private IDictionary _tCubeUpgradeProgressDictionary;
		public IDictionary tCubeUpgradeProgressDictionary {
			get {
				return _tCubeUpgradeProgressDictionary;
			}
			set {
				_tCubeUpgradeProgressDictionary = value;
				//upgradesDictionary["TCubeUpgrade"] = _tCubeUpgradeProgressDictionary;
			}
		}
		
		public class TCubeUpdateProgress
		{
			//private TransmutationCube oldTCube;
			private TransmutationCube newTCube;
			//private int gCost;
			private long endTime;
			private long currentTime;
			private string uid;
			private string boostUid;
			
			public TCubeUpdateProgress (/*TransmutationCube oldTCube,*/long currentTime, long endTime, TransmutationCube newTCube, string uid = null, string boostUid = null, long timeRemaining = -1)
			{
				//this.oldTCube = oldTCube;
				this.newTCube = newTCube;
				this.currentTime = currentTime;
				this.endTime = endTime;
				//this.gCost = (int)newTCube.gCost;
				this.uid = uid;
				this.timeRemaining = timeRemaining;
				this.boostUid = boostUid;
			}
	
			private double _timeRemaining = -1;
			public double timeRemaining {
				get {
					return _timeRemaining;
				}
				private set{ _timeRemaining = value;}
			}
			
			public IDictionary ToDictionary ()
			{
				IDictionary tCubeUpgradeDictionary = new Dictionary<string, object> ();
				
				tCubeUpgradeDictionary ["CurrentTime"] = this.currentTime;
				tCubeUpgradeDictionary ["EndTime"] = this.endTime;
				tCubeUpgradeDictionary ["TimeRemaining"] = this.timeRemaining;
				tCubeUpgradeDictionary ["BoostCost"] = GameManager._dataBank.GetbCostForSkullLevel (newTCube.skullLevel);
				if (this.uid != null)
					tCubeUpgradeDictionary ["transactionId"] = this.uid;
				
				if (this.boostUid != null)
					tCubeUpgradeDictionary ["boostId"] = this.boostUid;
				
				return tCubeUpgradeDictionary;
			}
		}
	
		public IDictionary LoadTCubeProgress (IDictionary upgradesDictionary)
		{
			string uid;
			string boostUid;
			
			if (!upgradesDictionary.Contains ("transactionId"))
				uid = Nonce.GetUniqueID ();
			else
				uid = upgradesDictionary ["transactionId"].ToString ();
			
			if (!upgradesDictionary.Contains ("boostId"))
				boostUid = Nonce.GetUniqueID ();
			else
				boostUid = upgradesDictionary ["boostId"].ToString ();
			
			//IDictionary currentTCubeDictionary = this.upgradesDictionary[TAG_CURRENT_TCUBE] as IDictionary;
			IDictionary newTCubeDictionary = this.upgradesDictionary ["NextTCubeUpgrade"] as IDictionary;
			
			//TransmutationCube oldTCube = GameManager._dataBank.GetTransmutationCubeForID(currentTCubeDictionary["id"].ToString());
			TransmutationCube newTCube = GameManager._dataBank.GetTransmutationCubeForID (newTCubeDictionary ["id"].ToString ());
			
			long endTime = long.Parse (upgradesDictionary ["EndTime"].ToString ());
			long currentTime = long.Parse (upgradesDictionary ["CurrentTime"].ToString ());
			long timeRemaining = -1;//long.Parse(upgradesDictionary["TimeRemaining"].ToString());
			return new TCubeUpdateProgress (/*oldTCube,*/currentTime, endTime, newTCube, uid, boostUid, timeRemaining).ToDictionary ();
		}
		
		
		public IDictionary UpgradeTCube (long currentTime = -1, long endTime = -1)
		{
			Debug.Log ("UpgradePotionBelt CALLED!");
			TransmutationCube newTCube = null;
			for (int i = 0; i<GameManager._dataBank.sortedTCubeList.Count; i++) {
				if (GameManager._dataBank.sortedTCubeList [i].level == this.transmutationCube.level) {
					if (this.transmutationCube.level != GameManager._dataBank.GetMaxLevelTCube ().level)
						newTCube = GameManager._dataBank.sortedTCubeList [i + 1].Clone ();
					else
						newTCube = GameManager._dataBank.GetMaxLevelTCube ();
				}
			}
			
			if (this.transmutationCube.level >= newTCube.level)
				return null;
			
			string uid = Nonce.GetUniqueID ();
			string boostUid = Nonce.GetUniqueID ();
			PurchaseRequest.PurchaseRequestType responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade, uid, "", (int)newTCube.gCost, (int)newTCube.dCost, newTCube.skullLevel, newTCube.id);
			if (responseType == PurchaseRequest.PurchaseRequestType.Error) {
				if (newTCube.gCost > 0) {
					PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, (int)(newTCube.gCost - GameManager._gameState.User._inventory.gems), "Upgrade Transmutation Cube", "Buy Gems");
				}
				return null;
			}
			
			return new TCubeUpdateProgress (/*this.transmutationCube,*/currentTime, endTime, newTCube, uid, boostUid).ToDictionary ();
		}
		
		public void OnTCubeUpgradeMatured ()
		{
			TransmutationCube newTCube = null;
			for (int i = 0; i<GameManager._dataBank.sortedTCubeList.Count; i++) {
				if (GameManager._dataBank.sortedTCubeList [i].level == this.transmutationCube.level) {
					if (this.transmutationCube.level != GameManager._dataBank.GetMaxLevelTCube ().level)
						newTCube = GameManager._dataBank.sortedTCubeList [i + 1].Clone ();
					else
						newTCube = GameManager._dataBank.GetMaxLevelTCube ();
				}
			}

			UIManager.instance.generalSwf.upgradeUI.ShowUpgradeCompletePopup (newTCube.id, newTCube.id, newTCube.level.ToString ());

			this.transmutationCube.id = newTCube.id;
			this.transmutationCube.level = newTCube.level;
			this.transmutationCube.gCost = newTCube.gCost;
			this.transmutationCube.uTime = newTCube.uTime;
			this.transmutationCube.capacity = newTCube.capacity;
			
			ResetUpgradeDictionary ();
			GameManager.instance.SaveGameState (false);
		}
		#endregion
		
		
		#region Keyring Upgrade
		private IDictionary _keyRingUpgradeProgressDictionary;
		public IDictionary keyRingUpgradeProgressDictionary {
			get {
				return _keyRingUpgradeProgressDictionary;
			}
			set {
				_keyRingUpgradeProgressDictionary = value;
				upgradesDictionary ["KeyRingUpgrade"] = _keyRingUpgradeProgressDictionary;
			}
		}
		
		public class KeyRingUpdateProgress
		{
			//private KeyRing oldKeyRing;
			private KeyRing newKeyRing;
			//private int dCost;
			private long endTime;
			private long currentTime;
			private string uid;
			private string boostUid;
			
			
			public KeyRingUpdateProgress (/*KeyRing oldKeyRing,*/long currentTime, long endTime, KeyRing newKeyRing, string uid = null, string boostUid = null, long timeRemaining = -1)
			{
				//this.oldKeyRing = oldKeyRing;
				this.newKeyRing = newKeyRing;
				this.currentTime = currentTime;
				this.endTime = endTime;
				this.uid = uid;
				this.timeRemaining = timeRemaining;
				//this.dCost = (int)newKeyRing.dCost;
				this.boostUid = boostUid;
			}
			
			private double _timeRemaining = -1;
			public double timeRemaining {
				get {
					return _timeRemaining;//(long)(endTime - currentTime);
				}
				private set{ _timeRemaining = value;}
			}
			
			public IDictionary ToDictionary ()
			{
				IDictionary keyRingUpgradeDictionary = new Dictionary<string, object> ();
				
				keyRingUpgradeDictionary ["CurrentTime"] = this.currentTime;
				keyRingUpgradeDictionary ["EndTime"] = this.endTime;
				keyRingUpgradeDictionary ["TimeRemaining"] = this.timeRemaining;
				keyRingUpgradeDictionary ["BoostCost"] = GameManager._dataBank.GetbCostForSkullLevel (newKeyRing.skullLevel);
				
//				Debug.Log (double.Parse (keyRingUpgradeDictionary ["BoostCost"].ToString ()));
				
				if (this.uid != null)
					keyRingUpgradeDictionary ["transactionId"] = this.uid;
				if (this.boostUid != null)
					keyRingUpgradeDictionary ["boostId"] = this.boostUid;
				
				return keyRingUpgradeDictionary;
			}
		}
		
		public IDictionary LoadKeyRingProgress (IDictionary upgradesDictionary)
		{
			string uid;
			string boostUid;
			
			if (!upgradesDictionary.Contains ("transactionId"))
				uid = Nonce.GetUniqueID ();
			else
				uid = upgradesDictionary ["transactionId"].ToString ();
			
			if (!upgradesDictionary.Contains ("boostId"))
				boostUid = Nonce.GetUniqueID ();
			else
				boostUid = upgradesDictionary ["boostId"].ToString ();
			
			//IDictionary currentKeyRingDictionary = this.upgradesDictionary[TAG_CURRENT_KEY_RING] as IDictionary;
			IDictionary newKeyRingDictionary = this.upgradesDictionary [TAG_NEXT_KEY_RING_UPGRADE] as IDictionary;
			
			//KeyRing oldKeyRing = GameManager._dataBank.GetKeyRingForID(currentKeyRingDictionary["id"].ToString());
			KeyRing newKeyRing = GameManager._dataBank.GetKeyRingForID (newKeyRingDictionary ["id"].ToString ());
			
			long endTime = long.Parse (upgradesDictionary ["EndTime"].ToString ());
			long currentTime = long.Parse (upgradesDictionary ["CurrentTime"].ToString ());
			long timeRemaining = -1;//long.Parse(upgradesDictionary["TimeRemaining"].ToString());
			return new KeyRingUpdateProgress (/*oldKeyRing,*/currentTime, endTime, newKeyRing, uid, boostUid, timeRemaining).ToDictionary ();
		}

		
		public IDictionary UpgradeKeyRing (long currentTime = -1, long endTime = -1)
		{
			Debug.Log ("UpgradeKeyRing CALLED ");
			KeyRing newKeyRing = null;
			for (int i = 0; i<GameManager._dataBank.sortedKeyRingList.Count; i++) {
				if (GameManager._dataBank.sortedKeyRingList [i].level == this.keyRing.level) {
					if (this.keyRing.level != GameManager._dataBank.GetMaxLevelKeyRing ().level)
						newKeyRing = GameManager._dataBank.sortedKeyRingList [i + 1].Clone ();
					else
						newKeyRing = GameManager._dataBank.GetMaxLevelKeyRing ();
				}
			}
			
			if (this.keyRing.capacity >= newKeyRing.capacity)
				return null;
			
			string uid = Nonce.GetUniqueID ();
			string boostUid = Nonce.GetUniqueID ();
			PurchaseRequest.PurchaseRequestType responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.KeyRingUpgrade, uid, "", (int)newKeyRing.gCost, (int)newKeyRing.dCost, newKeyRing.skullLevel, newKeyRing.id);
			if (responseType == PurchaseRequest.PurchaseRequestType.Error) {
				if (newKeyRing.gCost > 0) {
					Debug.Log ("responseType == PurchaseRequest.PurchaseRequestType.Error");
					PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, (int)(newKeyRing.gCost - GameManager._gameState.User._inventory.gems), "Upgrade Key Ring", "Buy Gems");
				}
				return null;
			}
			return new KeyRingUpdateProgress (/*this.keyRing,*/currentTime, endTime, newKeyRing, uid, boostUid).ToDictionary ();
		}

		private PurchaseRequest.PurchaseRequestType PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType purchaseType, string uid, string parentUid, int gCost, int dCost, int skullLevel, string itemId)
		{
			TransactionRequest tRequest = new TransactionRequest (purchaseType, uid, parentUid, gCost, dCost, skullLevel, itemId);
			return PurchaseManager.Instance.PerformTransaction (tRequest);
		}
		public void OnKeyRingUpgradeMatured ()
		{
			KeyRing newKeyRing = null;
			for (int i = 0; i<GameManager._dataBank.sortedKeyRingList.Count; i++) {
				if (GameManager._dataBank.sortedKeyRingList [i].level == this.keyRing.level) {
					if (this.keyRing.level != GameManager._dataBank.GetMaxLevelKeyRing ().level)
						newKeyRing = GameManager._dataBank.sortedKeyRingList [i + 1].Clone ();
					else
						newKeyRing = GameManager._dataBank.GetMaxLevelKeyRing ();
				}
			}

			UIManager.instance.generalSwf.upgradeUI.ShowUpgradeCompletePopup (newKeyRing.id, newKeyRing.id, newKeyRing.level.ToString ());

			this.keyRing.id = newKeyRing.id;
			this.keyRing.level = newKeyRing.level;
			this.keyRing.skullLevel = newKeyRing.skullLevel;
			this.keyRing.dCost = newKeyRing.dCost;
			this.keyRing.uTime = newKeyRing.uTime;
			this.keyRing.capacity = newKeyRing.capacity;
			
			ResetUpgradeDictionary ();
//			GameManager.instance.SaveGameState(false);
		}
		#endregion
		
		
		#region PotionBelt Upgrade
		private IDictionary _pBeltUpgradeProgressDictionary;
		public IDictionary pBeltUpgradeProgressDictionary {
			get {
				return _pBeltUpgradeProgressDictionary;
			}
			set {
				_pBeltUpgradeProgressDictionary = value;
				upgradesDictionary ["PotionBeltUpgrade"] = _pBeltUpgradeProgressDictionary;
			}
			
		}
		
		public class PotionBeltUpdateProgress
		{
			//private PotionBelt oldBelt;
			private PotionBelt newBelt;
			long endTime;
			long currentTime;
			//int dCost;
			string uid;
			string boostUid;
			public PotionBeltUpdateProgress (/*PotionBelt oldBelt,*/long currentTime, long endTime, PotionBelt newBelt, string uid = null, string boostUid = null, long timeRemaining = -1)
			{
			
				//this.oldBelt = oldBelt;
				this.newBelt = newBelt;
				this.currentTime = currentTime;
				this.endTime = endTime;
				this.uid = uid;
				this.timeRemaining = timeRemaining;
				this.boostUid = boostUid;
			}
			
			private double _timeRemaining = -1;
			public double timeRemaining {
				get {
					//long timeRem = endTime - currentTime;
					//if(GameManager.PRINT_LOGS) Debug.Log("PotionBelt UPGRADES ----- (matureTime - Helpers.GetSecondsFromEpoch(System.DateTime.UtcNow)).TotalSeconds" + timeRem);
					return _timeRemaining;
				}
				private set{ _timeRemaining = value;}
			}
			
			public IDictionary ToDictionary ()
			{
				IDictionary beltUpgradeDictionary = new Dictionary<string, object> ();
				
				beltUpgradeDictionary ["CurrentTime"] = this.currentTime;
				beltUpgradeDictionary ["EndTime"] = this.endTime;
				beltUpgradeDictionary ["TimeRemaining"] = this.timeRemaining;
				beltUpgradeDictionary ["BoostCost"] = GameManager._dataBank.GetbCostForSkullLevel (newBelt.skullLevel);
				
				if (this.uid != null)
					beltUpgradeDictionary ["transactionId"] = this.uid;
				
				if (this.boostUid != null)
					beltUpgradeDictionary ["boostId"] = this.boostUid;
				return beltUpgradeDictionary;
			}
		}

		public IDictionary LoadPotionBeltProgress (IDictionary upgradesDictionary)
		{
			string uid;
			string boostUid;
			if (!upgradesDictionary.Contains ("transactionId"))
				uid = Nonce.GetUniqueID ();
			else
				uid = upgradesDictionary ["transactionId"].ToString ();
			
			if (!upgradesDictionary.Contains ("boostId"))
				boostUid = Nonce.GetUniqueID ();
			else
				boostUid = upgradesDictionary ["boostId"].ToString ();
			
			//IDictionary currentPBeltDictionary = this.upgradesDictionary[TAG_CURRENT_POTION_BELT] as IDictionary;
			IDictionary newPBeltDictionary = this.upgradesDictionary [TAG_NEXT_KEY_POTION_BELT_UPGRADE] as IDictionary;
			
			//PotionBelt oldBelt = GameManager._dataBank.GetPotionBeltForID(currentPBeltDictionary["id"].ToString());
			PotionBelt newBelt = GameManager._dataBank.GetPotionBeltForID (newPBeltDictionary ["id"].ToString ());
			
			long endTime = long.Parse (upgradesDictionary ["EndTime"].ToString ());
			long currentTime = long.Parse (upgradesDictionary ["CurrentTime"].ToString ());
			long timeRemaining = -1;//long.Parse(upgradesDictionary["TimeRemaining"].ToString());
			return new PotionBeltUpdateProgress (/*oldBelt,*/currentTime, endTime, newBelt, uid, boostUid, timeRemaining).ToDictionary ();
		}
		
		public IDictionary UpgradePotionBelt (long endTime = -1, long currentTime = -1)
		{
			Debug.Log ("UpgradePotionBelt CALLED ");
			PotionBelt newPBelt = null;
			for (int i = 0; i<GameManager._dataBank.sortedPotionBeltList.Count; i++) {
				if (GameManager._dataBank.sortedPotionBeltList [i].level == this.potionBelt.level) {
					if (this.potionBelt.level != GameManager._dataBank.GetMaxLevelPotionBelt ().level)
						newPBelt = GameManager._dataBank.sortedPotionBeltList [i + 1].Clone ();
					else
						newPBelt = GameManager._dataBank.GetMaxLevelPotionBelt ();
				}
			}
			
			if (this.potionBelt.Count () >= newPBelt.capacity)
				return null;
			string uid = Nonce.GetUniqueID ();
			PurchaseRequest.PurchaseRequestType responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade, uid, "", (int)newPBelt.gCost, (int)newPBelt.dCost, newPBelt.skullLevel, newPBelt.id);
			if (responseType == PurchaseRequest.PurchaseRequestType.Error) {
				Debug.Log ("responseType == PurchaseRequest.PurchaseRequestType.Error");

				if (newPBelt.gCost > 0) {
					PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, (int)(newPBelt.gCost - GameManager._gameState.User._inventory.gems), "Upgrade Potion Belt", "Buy Gems");
				}
				return null;
			}
			
			return new PotionBeltUpdateProgress (/*this.potionBelt,*/currentTime, endTime, newPBelt, uid).ToDictionary ();
		}
		
		public void OnPotionBeltUpgradeMatured ()
		{
			PotionBelt newPBelt = null;
			for (int i = 0; i<GameManager._dataBank.sortedPotionBeltList.Count; i++) {
				if (GameManager._dataBank.sortedPotionBeltList [i].level == this.potionBelt.level) {
					if (this.potionBelt.level != GameManager._dataBank.GetMaxLevelPotionBelt ().level)
						newPBelt = GameManager._dataBank.sortedPotionBeltList [i + 1].Clone ();
					else
						newPBelt = GameManager._dataBank.GetMaxLevelPotionBelt ();
				}
			}

			UIManager.instance.generalSwf.upgradeUI.ShowUpgradeCompletePopup (newPBelt.id, newPBelt.id, newPBelt.level.ToString ());

			this.potionBelt.id = newPBelt.id;
			this.potionBelt.level = newPBelt.level;
			this.potionBelt.dCost = newPBelt.dCost;
			this.potionBelt.level = newPBelt.level;
			this.potionBelt.skullLevel = newPBelt.skullLevel;
			this.potionBelt.uTime = newPBelt.uTime;
			this.potionBelt.capacity = newPBelt.capacity;
			this.potionBelt.healAmount = newPBelt.healAmount;

			ResetUpgradeDictionary ();
//			GameManager.instance.SaveGameState(false);
		}
		#endregion
		
		private void ResetUpgradeDictionary ()
		{
			GameManager._gameState.User._inventory.upgradesDictionary ["UpgradeInfo"] = "";
			GameManager._gameState.User._inventory.upgradesDictionary ["CurrentUpgrade"] = InventorySystem.Inventory.UpgradeType.kNone + "";
		}
		
		public void SaveBagUpgrade (IDictionary bagUpgradeDictionary)
		{
			Debug.Log ("SaveBagUpgrade");
			GameManager._gameState.User._inventory.upgradesDictionary ["UpgradeInfo"] = bagUpgradeDictionary;
			GameManager._gameState.User._inventory.upgradesDictionary ["CurrentUpgrade"] = InventorySystem.Inventory.UpgradeType.kBag + "";
//			GameManager.instance.SaveGameState(false);
		}
		
		public void SaveKeyRingUpgrade (IDictionary keyRingUpgradeDictionary)
		{
			Debug.Log ("SaveKeyRingUpgrade");
			GameManager._gameState.User._inventory.upgradesDictionary ["UpgradeInfo"] = keyRingUpgradeDictionary;
			GameManager._gameState.User._inventory.upgradesDictionary ["CurrentUpgrade"] = InventorySystem.Inventory.UpgradeType.kKeyRing + "";
//			GameManager.instance.SaveGameState(false);
		}
		
		public void SavePotionBeltUpgrade (IDictionary pBeltUpgradeDictionary)
		{
			Debug.Log ("SavePotionBeltUpgrade");
			GameManager._gameState.User._inventory.upgradesDictionary ["UpgradeInfo"] = pBeltUpgradeDictionary;
			GameManager._gameState.User._inventory.upgradesDictionary ["CurrentUpgrade"] = InventorySystem.Inventory.UpgradeType.kPotionBelt + "";
//			GameManager.instance.SaveGameState(false);
		}
		
		public void SaveTCubeUpgrade (IDictionary tCubeUpgradeDictionary)
		{
			Debug.Log ("SaveTCubeUpgrade");
			GameManager._gameState.User._inventory.upgradesDictionary ["UpgradeInfo"] = tCubeUpgradeDictionary;
			GameManager._gameState.User._inventory.upgradesDictionary ["CurrentUpgrade"] = InventorySystem.Inventory.UpgradeType.kTCube + "";
//			GameManager.instance.SaveGameState(false);
		}
		
		public bool isCurrentlyUpgrading ()
		{
			string currentUpgrade = upgradesDictionary ["CurrentUpgrade"].ToString ();
			Debug.Log ("isCurrentlyUpgrading >> CURRENT UPGRADE >> " + currentUpgrade);
			return !currentUpgrade.Equals (UpgradeType.kNone.ToString ());
		}
		
		private int GetNewUpgradeSkullLevel (string itemId, out string nextItemId)
		{
			int skullLevel = -1;
			nextItemId = "";
			switch (itemId) {
			case "BAG":
				IDictionary nextBagUpgrade = this.upgradesDictionary ["NextBagUpgrade"] as IDictionary;
				skullLevel = Int32.Parse (nextBagUpgrade ["SkullLevel"].ToString ());
				nextItemId = nextBagUpgrade ["id"].ToString ();
				break;
				
			case "KEY_RING":
				IDictionary nextKeyRingUpgrade = this.upgradesDictionary ["NextKeyRingUpgrade"] as IDictionary;
				skullLevel = Int32.Parse (nextKeyRingUpgrade ["SkullLevel"].ToString ());
				nextItemId = nextKeyRingUpgrade ["id"].ToString ();
				break;
				
			case "POTION_BELT":
				IDictionary nextPBeltUpgrade = this.upgradesDictionary ["NextPotionBeltUpgrade"] as IDictionary;
				skullLevel = Int32.Parse (nextPBeltUpgrade ["SkullLevel"].ToString ());
				nextItemId = nextPBeltUpgrade ["id"].ToString ();
				break;
				
			case "T_CUBE":
				IDictionary nextTCubeUpgrade = this.upgradesDictionary ["NextTCubeUpgrade"] as IDictionary;
				skullLevel = Int32.Parse (nextTCubeUpgrade ["SkullLevel"].ToString ());
				nextItemId = nextTCubeUpgrade ["id"].ToString ();
				break;
				
			}
			return skullLevel;
		}
	
		public void OnStartUpgrade (string upgradeItemId, Action<bool> successCallback = null)
		{
			Debug.Log ("OnStartUpgrade > " + upgradeItemId);
//			if(this.isCurrentlyUpgrading())
//				return;
			string uid = null;
			
			IDictionary newBagDictionary = null;
			IDictionary newKeyRingDictionary = null;
			IDictionary newPotionBeltDictionary = null;
			IDictionary newTCubeDictionary = null;
			
			string id;
			string itemId = upgradeItemId.Substring (0, upgradeItemId.Length - 1);
			int skullLevel = GetNewUpgradeSkullLevel (itemId, out id);
			
			Debug.Log (" UPGRADE >> itemId :::::::::::: " + itemId);
			
			switch (itemId) {
			case "BAG":
				newBagDictionary = this.UpgradeBag ();
				if (newBagDictionary != null)
					uid = newBagDictionary ["transactionId"].ToString ();
				break;
			
			case "KEY_RING":
				newKeyRingDictionary = this.UpgradeKeyRing ();
				if (newKeyRingDictionary != null)
					uid = newKeyRingDictionary ["transactionId"].ToString ();
				break;
			
			case "POTION_BELT":
				newPotionBeltDictionary = this.UpgradePotionBelt ();
				if (newPotionBeltDictionary != null)
					uid = newPotionBeltDictionary ["transactionId"].ToString ();
				break;
			
			case "T_CUBE":
				newTCubeDictionary = this.UpgradeTCube ();
				if (newTCubeDictionary != null)
					uid = newTCubeDictionary ["transactionId"].ToString ();
				break;
				
			}

			Debug.Log ("HELLO SUPPP!!! 1");

			if (this.isCurrentlyUpgrading ()) {
				Debug.Log ("HELLO SUPPP!!! 2");

				IDictionary upgradeDictionary = this.upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary;
				
				uid = upgradeDictionary ["transactionId"].ToString ();
				IDictionary currentUpgradeInfo = upgradesDictionary [Inventory.TAG_UPGRADE_INFO] as IDictionary;
				
				if (upgradeDictionary != null)
					GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (currentUpgradeInfo ["TimeRemaining"] + "|" + upgradeDictionary ["BoostCost"].ToString (), true);

				if(!InitGameVersions._offlineMode) {
//					Debug.LogError("OnStartUpgrade 1 - "+id+" uid - "+uid);
					ServerRequestParam requestParam = new ServerRequestParam (uid, "", skullLevel, id, 0);
					ServerManager.Instance.ProcessRequest (new UpgradeRequest (ServerRequest.ServerRequestType.Upgrade, this.ProcessResponse, requestParam));
				}				
				return; // An upgrade in progress - can't upgrade simultaneously!!!
			}
				
			switch (itemId) {
			case "BAG":
				if (newBagDictionary != null)
					this.SaveBagUpgrade (newBagDictionary);
				break;
				
			case "KEY_RING":
				if (newKeyRingDictionary != null)
					this.SaveKeyRingUpgrade (newKeyRingDictionary);
				break;
				
			case "POTION_BELT":
				if (newPotionBeltDictionary != null)
					this.SavePotionBeltUpgrade (newPotionBeltDictionary);
				break;
			
			case "T_CUBE":
				if (newTCubeDictionary != null)
					this.SaveTCubeUpgrade (newTCubeDictionary);
				break;
			}
			if (uid != null) {
				Debug.Log ("HELLO SUPPP!!! 3");
				GameManager.instance.scaleformCamera.generalSwf.UpgradeStarted ();
				PurchaseManager.Instance.CommitTransaction (uid, false);

				if(!InitGameVersions._offlineMode) {
//					Debug.LogError("OnStartUpgrade 2 - "+id+" uid - "+uid);
					ServerRequestParam requestParam = new ServerRequestParam (uid, "", skullLevel, id, 0);
					ServerManager.Instance.ProcessRequest (new UpgradeRequest (ServerRequest.ServerRequestType.Upgrade, this.ProcessResponse, requestParam));
				}
				IDictionary upgradeDictionary = this.upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary;
				IDictionary currentUpgradeInfo = upgradesDictionary [Inventory.TAG_UPGRADE_INFO] as IDictionary;

				if (upgradeDictionary != null)
					GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (currentUpgradeInfo ["TimeRemaining"] + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
				
				if (successCallback != null)
					successCallback (true);
			} else {
				Debug.Log ("HELLO SUPPP!!! 4");

				if (successCallback != null)
					successCallback (false);
			}
			GameManager.instance.SaveGameState (false);
			Debug.Log ("IS UPGRADING >> " + this.isCurrentlyUpgrading () + "   upgradesDictionary >> " + MiniJSON.Json.Serialize (upgradesDictionary));
		}
		
		public void ProcessResponse (ServerResponse response)
		{
//			if(response == null) 
//			{
//				Debug.Log("response >> NULL");
//				return;
//			}
//			if(GameManager.PRINT_LOGS) Debug.Log("Response received here...ProcessResponse ::::::::: INVENTORY");
			IDictionary upgradeDictionary = this.upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary;
			
			if (response == null) {
				if (upgradeDictionary != null) {
					Debug.Log ("upgradeDictionary not null");
//					GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (-1 + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
				}
				
				Debug.Log ("Gotta return");
				return;
			}
			
			UpgradeResponse upgradeResponse = null;
		
			int upgradeTotalBoostCost = -1;
			double totalTimeRemaining = -1;
			string newItemID = "";
			if (upgradeDictionary != null) {
				string currentUpgradeStr = upgradesDictionary [TAG_CURRENT_UPGRADE].ToString ();

				switch (currentUpgradeStr) {
				case "kBag":
					IDictionary newBagDictionary = this.upgradesDictionary [TAG_NEXT_BAG_UPGRADE] as IDictionary;
					newItemID = newBagDictionary ["id"].ToString ();
					Bag newBag = GameManager._dataBank.GetBagForID (newItemID);
					
					upgradeTotalBoostCost = GameManager._dataBank.GetbCostForSkullLevel (newBag.skullLevel);
					totalTimeRemaining = Convert.ToDouble (GameManager._dataBank.GetUTimeForSkullLevel (newBag.skullLevel) / User.DEBUG_TIMER_CUT);
					break;
				case "kPotionBelt":
					IDictionary newPBeltDictionary = this.upgradesDictionary [TAG_NEXT_KEY_POTION_BELT_UPGRADE] as IDictionary;
					newItemID = newPBeltDictionary ["id"].ToString ();
					PotionBelt newBelt = GameManager._dataBank.GetPotionBeltForID (newItemID);
					
					upgradeTotalBoostCost = GameManager._dataBank.GetbCostForSkullLevel (newBelt.skullLevel);
					totalTimeRemaining = Convert.ToDouble (GameManager._dataBank.GetUTimeForSkullLevel (newBelt.skullLevel) / User.DEBUG_TIMER_CUT);
					break;
				case "kKeyRing":
					IDictionary newKeyRingDictionary = this.upgradesDictionary [TAG_NEXT_KEY_RING_UPGRADE] as IDictionary;
					newItemID = newKeyRingDictionary ["id"].ToString ();
					KeyRing newKeyRing = GameManager._dataBank.GetKeyRingForID (newItemID);
					
					upgradeTotalBoostCost = GameManager._dataBank.GetbCostForSkullLevel (newKeyRing.skullLevel);
					totalTimeRemaining = Convert.ToDouble (GameManager._dataBank.GetUTimeForSkullLevel (newKeyRing.skullLevel) / User.DEBUG_TIMER_CUT);
					break;
				case "kTCube":
					IDictionary newTCubeDictionary = this.upgradesDictionary ["NextTCubeUpgrade"] as IDictionary;
					newItemID = newTCubeDictionary ["id"].ToString ();
					TransmutationCube newTCube = GameManager._dataBank.GetTransmutationCubeForID (newItemID);
					
					upgradeTotalBoostCost = GameManager._dataBank.GetbCostForSkullLevel (newTCube.skullLevel);
					totalTimeRemaining = Convert.ToDouble (GameManager._dataBank.GetUTimeForSkullLevel (newTCube.skullLevel) / User.DEBUG_TIMER_CUT);
					break;
				}
				Debug.LogError ("Tag: " + currentUpgradeStr + ", upgradeTotalBoostCost: " + upgradeTotalBoostCost + ", totalTimeRemaining: " + totalTimeRemaining);
			}
			/*if(response == null)
			{
				if(isCurrentlyUpgrading())
					GameManager.instance.scaleformCamera.generalSwf.SetUpdateData(-1 + "|" + upgradeDictionary["BoostCost"].ToString(), true);
				return;
			}*/
			switch (response.Request.RequestType) {
			case ServerRequest.ServerRequestType.All:
				Debug.Log (":::::::::::case ServerRequest.ServerRequestType.All:::::::::::::");
				UpdateResponse updateResponse = (UpdateResponse)response;
				
				if (upgradeDictionary != null) {
					if (GameManager.PRINT_LOGS)
						Debug.Log ("SetUpdateData true");
//					GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (-1 + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
					if (GameManager.PRINT_LOGS)
						Debug.Log ("upgradeDictionary[\"BoostCost\"].ToString() >> " + double.Parse (upgradeDictionary ["BoostCost"].ToString ()));
					//Debug.Log("timeRatio >> " + timeRatio + "   newBoostCost >> " + newBoostCost + " upgradeTotalBoostCost>>> " + upgradeTotalBoostCost);
				}
				
				if (updateResponse.IsSuccess && updateResponse.Upgrade != null) {
					
					if (upgradeDictionary != null && upgradeDictionary ["transactionId"].ToString ().Equals (updateResponse.Upgrade.TransactionId)) {
						double currentTimeRemaining = Convert.ToDouble (updateResponse.Upgrade.RemainingTime);
//						Debug.LogError ("currentTimeRemaining: " + currentTimeRemaining + ", totalTimeRemaining: " + totalTimeRemaining);
						double timeRatio = currentTimeRemaining / totalTimeRemaining;
						int newBoostCost = Convert.ToInt32 (timeRatio * upgradeTotalBoostCost);
						
						if (GameManager.PRINT_LOGS)
							Debug.Log ("timeRatio >> " + timeRatio + "   newBoostCost >> " + newBoostCost + " upgradeTotalBoostCost>>> " + upgradeTotalBoostCost);
						
						if (GameManager.PRINT_LOGS)
							Debug.Log ("upgradeDictionary[\"TimeRemaining\"]" + upgradeDictionary ["TimeRemaining"].ToString ());
						
						upgradeDictionary ["TimeRemaining"] = updateResponse.Upgrade.RemainingTime;
						upgradeDictionary ["BoostCost"] = newBoostCost;
						/*********************************************************************/
						upgradeDictionary ["EndTime"] = updateResponse.Upgrade.EndTime;
//						upgradeDictionary ["CurrentTime"] = updateResponse.Upgrade.StartTime;
						upgradeDictionary.Add("StartTime", updateResponse.Upgrade.StartTime);
						/*********************************************************************/
						Debug.Log ("added StartTime - " +MiniJSON.Json.Serialize (upgradesDictionary));


						if (GameManager.PRINT_LOGS)
							Debug.Log ("SetUpdateData true");
						GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (upgradeDictionary ["TimeRemaining"].ToString () + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
						
						/* Don't think its needed - or if it is, in what case? Removed it from spirit upgrades too.
						 * 
						 * if(updateResponse.Upgrade.IsCompleted)
						{
							if(updateResponse.Upgrade.Boost == 1)
							{
								Analytics.logEvent(Analytics.EventName.Upgrade_Boost);
								PurchaseManager.Instance.CompleteTransaction(updateResponse.Upgrade.BoostId, updateResponse.Upgrade.TransactionId);
							}
							else
							{
								Analytics.logEvent(Analytics.EventName.Upgrade_Start);
								PurchaseManager.Instance.CompleteTransaction(updateResponse.Upgrade.TransactionId, "");
							}
						}*/
					} else {
						/*if(!string.IsNullOrEmpty(newItemID))
						 	;//OnStartUpgrade(newItemID);*/
						
					}
				}
				
				break;
			case ServerRequest.ServerRequestType.Upgrade:
//				if(GameManager.PRINT_LOGS) Debug.Log(":::::::::: case ServerRequest.ServerRequestType.Upgrade::::::");
				upgradeResponse = (UpgradeResponse)response;
				if (upgradeResponse.Upgrade != null) {
//					if(GameManager.PRINT_LOGS) Debug.Log("upgradeResponse.Upgrade != null");
//					if(GameManager.PRINT_LOGS) Debug.Log("SetUpdateData true");
					if (upgradeDictionary != null) {
//						GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (-1 + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
					}

					if (upgradeDictionary != null && upgradeDictionary ["transactionId"].ToString ().Equals (upgradeResponse.Upgrade.TransactionId)) {
						PurchaseManager.Instance.CommitTransaction (upgradeResponse.Upgrade.TransactionId, upgradeResponse.IsSuccess);
						if (upgradeResponse.IsSuccess) {
							if (GameManager.PRINT_LOGS)
								Debug.Log ("upgradeResponse successful : upgradeResponse.Upgrade.RemainingTime:::::::::::::::" + upgradeResponse.Upgrade.RemainingTime);
							upgradeDictionary ["TimeRemaining"] = upgradeResponse.Upgrade.RemainingTime;
							if (GameManager.PRINT_LOGS)
								Debug.Log ("SetUpdateData true");
							GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (upgradeDictionary ["TimeRemaining"].ToString () + "|" + upgradeDictionary ["BoostCost"].ToString (), true);
							if (upgradeResponse.Upgrade.IsCompleted) {
								if (upgradeResponse.Upgrade.Boost == 1) {
									Analytics.logEvent (Analytics.EventName.Upgrade_Boost);
									PurchaseManager.Instance.CompleteTransaction (upgradeResponse.Upgrade.BoostId, upgradeResponse.Upgrade.TransactionId);
								} else {
									Analytics.logEvent (Analytics.EventName.Upgrade_Start);
									PurchaseManager.Instance.CompleteTransaction (upgradeResponse.Upgrade.TransactionId, "");
								}
							}
						}
					}
				}
				break;
			case ServerRequest.ServerRequestType.UpgradeCompleted: 
				if (GameManager.PRINT_LOGS)
					Debug.Log ("case ServerRequest.ServerRequestType.UpgradeCompleted:");
				string upgradeJson = "-1";
				upgradeResponse = (UpgradeResponse)response;
				if (upgradeResponse.Upgrade != null) {
					if (upgradeDictionary != null && upgradeDictionary ["transactionId"].ToString ().Equals (upgradeResponse.Upgrade.TransactionId) && upgradeDictionary ["boostId"].ToString ().Equals (upgradeResponse.Upgrade.BoostId)) {
						if (upgradeResponse.Upgrade.Boost == 1) {
							PurchaseManager.Instance.CommitTransaction (upgradeResponse.Upgrade.BoostId, upgradeResponse.IsSuccess);
							//GameManager.instance.scaleformCamera.generalSwf.SetUpdateData(upgradeJson, false);
							Analytics.logEvent (Analytics.EventName.Upgrade_Collect);
							return;
						}
						if (upgradeResponse.IsSuccess && upgradeResponse.Upgrade.IsCompleted) {
							onUpgradeComplete (response.Request.RequestType);
							upgradeJson = GameManager._gameState.User.json;
							
							if (upgradeResponse.Upgrade.Boost == 1) {
								GameManager.instance.scaleformCamera.generalSwf.BoostResult (true);
								PurchaseManager.Instance.CompleteTransaction (upgradeResponse.Upgrade.BoostId, upgradeResponse.Upgrade.TransactionId);
								//GameManager.instance.scaleformCamera.generalSwf.SetUpdateData(upgradeJson, false);
							} else {
								PurchaseManager.Instance.CompleteTransaction (upgradeResponse.Upgrade.TransactionId, "");
								GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (upgradeJson, false);
							}
							Analytics.logEvent (Analytics.EventName.Upgrade_Collect);
							GameManager.instance.SaveGameState (false);
							return;
						}
						
						GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (upgradeJson, upgradeJson.Equals ("-1"));
					}
				}
//				if(GameManager.PRINT_LOGS) Debug.Log("SetUpdateData upgradeJson.Equals(\"-1\")" + upgradeJson.Equals("-1"));
//				GameManager.instance.scaleformCamera.generalSwf.SetUpdateData(upgradeJson, upgradeJson.Equals("-1"));
				break;
			}
			GameManager.instance.SaveGameState (false);

		}
		
		public void BoostUpgrade ()
		{
			if (!this.isCurrentlyUpgrading ()) {
				Debug.Log ("NOT currently upgrading! WTF!");
				return;
			}
			string activity = "";
			int gems = 0;
			string currentUpgrade = upgradesDictionary ["CurrentUpgrade"].ToString ();
			string uid = (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) ["transactionId"].ToString ();
			string boostUid = (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) ["boostId"].ToString ();
			PurchaseRequest.PurchaseRequestType responseType = PurchaseRequest.PurchaseRequestType.Error;
			if (currentUpgrade.Equals (UpgradeType.kBag.ToString ())) {
				Bag newBag = null;
				for (int i = 0; i<GameManager._dataBank.sortedBagList.Count; i++) {
					if (GameManager._dataBank.sortedBagList [i].level == this.bag.level) {
						if (this.bag.level != GameManager._dataBank.GetMaxLevelBag ().level)
							newBag = GameManager._dataBank.sortedBagList [i + 1].Clone ();
						else
							newBag = GameManager._dataBank.GetMaxLevelBag ();
					}
				}
				
				if (this.bag.BagCount () > newBag.capacity)
					return;
				activity = "Boost Bag Upgrade";
				gems = GameManager._dataBank.GetbCostForSkullLevel (newBag.skullLevel);
				responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.BagUpgradeBoost, boostUid, uid, gems, 0/*(int) newBag.dCost*/, newBag.skullLevel, newBag.id);
			} else if (currentUpgrade.Equals (UpgradeType.kPotionBelt.ToString ())) {
				PotionBelt newBelt = null;
				for (int i = 0; i<GameManager._dataBank.sortedPotionBeltList.Count; i++) {
					if (GameManager._dataBank.sortedPotionBeltList [i].level == this.potionBelt.level) {
						if (this.potionBelt.level != GameManager._dataBank.GetMaxLevelPotionBelt ().level)
							newBelt = GameManager._dataBank.sortedPotionBeltList [i + 1].Clone ();
						else
							newBelt = GameManager._dataBank.GetMaxLevelPotionBelt ();
					}
				}
				
				if (this.potionBelt.capacity > newBelt.capacity)
					return;
				activity = "Boost Potion Belt Upgrade";
				gems = GameManager._dataBank.GetbCostForSkullLevel (newBelt.skullLevel);
				responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.PotionBeltUpgradeBoost, boostUid, uid, GameManager._dataBank.GetbCostForSkullLevel (newBelt.skullLevel), 0/*(int) newBelt.dCost*/, newBelt.skullLevel, newBelt.id);
			} else if (currentUpgrade.Equals (UpgradeType.kKeyRing.ToString ())) {
				KeyRing newKeyRing = null;
				for (int i = 0; i<GameManager._dataBank.sortedKeyRingList.Count; i++) {
					if (GameManager._dataBank.sortedKeyRingList [i].level == this.keyRing.level) {
						if (this.keyRing.level != GameManager._dataBank.GetMaxLevelKeyRing ().level)
							newKeyRing = GameManager._dataBank.sortedKeyRingList [i + 1].Clone ();
						else
							newKeyRing = GameManager._dataBank.GetMaxLevelKeyRing ();
					}
				}
				
				if (this.keyRing.capacity > newKeyRing.capacity)
					return;
				activity = "Boost Key Ring Upgrade";
				gems = GameManager._dataBank.GetbCostForSkullLevel (newKeyRing.skullLevel);
				responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.KeyRingUpgradeBoost, boostUid, uid, GameManager._dataBank.GetbCostForSkullLevel (newKeyRing.skullLevel), 0 /*(int) newKeyRing.dCost*/, newKeyRing.skullLevel, newKeyRing.id);
			} else { //TCube section...
				TransmutationCube newTCube = null;
				for (int i = 0; i<GameManager._dataBank.sortedTCubeList.Count; i++) {
					if (GameManager._dataBank.sortedTCubeList [i].level == this.transmutationCube.level) {
						if (this.bag.level != GameManager._dataBank.GetMaxLevelTCube ().level)
							newTCube = GameManager._dataBank.sortedTCubeList [i + 1].Clone ();
						else
							newTCube = GameManager._dataBank.GetMaxLevelTCube ();
					}
				}

				if (this.transmutationCube.capacity > newTCube.capacity)
					return;
				activity = "Boost Transmutation Cube Upgrade";
				gems = GameManager._dataBank.GetbCostForSkullLevel (newTCube.skullLevel);
				responseType = PerformPurchaseRequest (PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgradeBoost, boostUid, uid, GameManager._dataBank.GetbCostForSkullLevel (newTCube.skullLevel), /*(int) newTCube.dCost*/0, newTCube.skullLevel, newTCube.id);
			}
			
			if (responseType == PurchaseRequest.PurchaseRequestType.Success) {
				Debug.Log ("BOOST RESPONSE SUCCESFUL!!!");
				GameManager.instance.scaleformCamera.generalSwf.Init ();
	
				onUpgradeComplete (ServerRequest.ServerRequestType.UpgradeBoost);
				
				string currentUpgradeStr2 = upgradesDictionary [TAG_CURRENT_UPGRADE].ToString (); // I think this part never gets executed... 
				switch (currentUpgradeStr2) {
				case "kBag":
					OnBagUpgradeMatured ();
					break;
				case "kPotionBelt":
					OnPotionBeltUpgradeMatured ();
					break;
				case "kKeyRing":
					OnKeyRingUpgradeMatured ();
					break;
				case "kTCube":
					OnTCubeUpgradeMatured ();
					break;
				}
				
				GameManager.instance.scaleformCamera.generalSwf.BoostResult (true);
				if (GameManager.PRINT_LOGS)
					Debug.Log ("SetUpdateData false");
				GameManager.instance.scaleformCamera.generalSwf.SetUpdateData (GameManager._gameState.User.json, false);
				GameManager.instance.SaveGameState (false);
				//}

//				IOSNotificationController.Instance.CancelLocalNotificationById((int)ItemNotificationType.Upgrade);
			} else {
				if (gems > 0) {
					PurchaseManager.Instance.ShowGeneralPopup2 (PurchaseManager.GeneralPopupType.InsufficientGems, gems - GameManager._gameState.User._inventory.gems, activity, "Buy Gems");
				}
			}


		}
		
		public void onUpgradeComplete (ServerRequest.ServerRequestType requestType)
		{
			if (!this.isCurrentlyUpgrading ()) {
				Debug.Log ("No upgrade to complete... ");
				return; // there is no upgrade to complete
			}
			string id;
			string uid = (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) ["transactionId"].ToString ();
			int skullLevel = -1;
			string currentUpgradeStr = upgradesDictionary [TAG_CURRENT_UPGRADE].ToString ();
			IDictionary upgrade = null;
			switch (currentUpgradeStr) {
			case "kBag":
				upgrade = upgradesDictionary [TAG_NEXT_BAG_UPGRADE] as IDictionary;
				break;
			case "kPotionBelt":
				upgrade = upgradesDictionary [TAG_NEXT_KEY_POTION_BELT_UPGRADE] as IDictionary;
					
				break;
			case "kKeyRing":
				upgrade = upgradesDictionary [TAG_NEXT_KEY_RING_UPGRADE] as IDictionary;
				break;
			case "kTCube":
				upgrade = upgradesDictionary ["NextTCubeUpgrade"] as IDictionary;
				break;
			}
			skullLevel = System.Convert.ToInt32 (upgrade ["SkullLevel"]);
			id = upgrade ["id"].ToString ();
			if (uid != null) {
				int boost = 0;
				string boostUid = "";
				if (requestType == ServerRequest.ServerRequestType.UpgradeBoost) {
					boost = 1;
					boostUid = (upgradesDictionary [TAG_UPGRADE_INFO] as IDictionary) ["boostId"].ToString ();
					
								
					PurchaseManager.Instance.CommitTransaction (boostUid, false);
					PurchaseManager.Instance.CompleteTransaction (boostUid, uid);
					PurchaseManager.Instance.MarkCommittedAndCompleteTrue (uid);
				} else {
					PurchaseManager.Instance.CommitTransaction (uid, false);
					PurchaseManager.Instance.CompleteTransaction (uid, "");
				}
				
//				Debug.LogError("OnStartUpgrade 3 - "+id+" uid - "+uid);
				ServerRequestParam requestParam = new ServerRequestParam (uid, boostUid, skullLevel, id, boost);
				ServerManager.Instance.ProcessRequest (new UpgradeRequest (ServerRequest.ServerRequestType.UpgradeCompleted, this.ProcessResponse, requestParam));
				
				string currentUpgradeStr2 = upgradesDictionary [TAG_CURRENT_UPGRADE].ToString ();
				switch (currentUpgradeStr2) {
				case "kBag":
					OnBagUpgradeMatured ();
					break;
				case "kPotionBelt":
					OnPotionBeltUpgradeMatured ();
					break;
				case "kKeyRing":
					OnKeyRingUpgradeMatured ();
					break;
				case "kTCube":
					OnTCubeUpgradeMatured ();
					break;
				}	
			}
			GameManager.instance.SaveGameState (false);
		}
	}

}