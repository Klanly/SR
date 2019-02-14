using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem;
using Game;

[System.Serializable]
public partial class User : CharacterModel
{
	public const string KEY_MULTIPLAYER_ID = "multiplayerID";
	public const string KEY_MODEL_NAME = "modelName";

	[Newtonsoft.Json.JsonIgnore]
	public string
		name;
	
	public delegate void UsernameChangeDelegate (string username);
	public event UsernameChangeDelegate usernameChangeEvent;
	public string modelName = "";
	private string _username = "";
	public string username {
		get {
			return _username;
		}
		set {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("TRYING SET USERNAME TO >>>> " + value);
			if (string.IsNullOrEmpty (value))
				return;
			
			if (_username.Equals (value))
				return;

			string userID = GameManager.instance.fragNetworkingNew.GetUserID ();
			Debug.Log ("userID >>>>>> " + userID + "    IS SAME AS VAL?>> " + userID == value + "    value is > " + value);
			if ((!string.IsNullOrEmpty (userID)) && value == userID)
				return;

			if (GameManager.PRINT_LOGS)
				Debug.Log ("USERNAME SETTING TO >>>> " + value);
			_username = value;
			
			if (usernameChangeEvent != null) {
				usernameChangeEvent (_username);
			}
			else
			{
				Debug.LogError("Event is null");
			}
		}
	}
	#region Buff Handling
	
	protected override void RemoveBuff (string buffName)
	{
		buffName = buffName.ToLower ();
		GameManager.instance.scaleformCamera.hud.RemovePlayerDebuff (buffName);
	}

	public override void ApplyBuff (Buff buff, BattleManager battleManager, bool showSWFInfo = true, bool showTutorial = true)
	{
		base.ApplyBuff (buff, battleManager, showSWFInfo, showTutorial);
		
		if (buff.negatesBuffName == null)
			return;
		
		List<Buff> removeBuffs = new List<Buff> ();
		Buff aBuff = null;
		List<Buff> buffList = battleManager._battleState._enemy._buffList;
		int count = buffList.Count;
		for (int i = 0; i < count; i++) {
			aBuff = buffList [i];
			if (GameManager.PRINT_LOGS)
				Debug.Log ("buff.negatesBuffName" + buff.negatesBuffName + "aBuff" + aBuff);
			if (buff.negatesBuffName.ToLower ().Equals (aBuff.id.ToLower ())) {
				removeBuffs.Add (aBuff);
			}
		}
		battleManager._battleState._enemy.RemoveBuffs (battleManager._battleState._enemy._buffList, removeBuffs, battleManager);
	}
	
	protected override void AddBuff (string buffName, bool onSelf, bool showSwfInfo = true, bool showTutorial = true)
	{
		buffName = buffName.ToLower ();
		if (GameManager.PRINT_LOGS)
			Debug.Log (":::::::::::::::::::: SCALEFORM METHOD for buff name " + buffName + " --->>> " + GetTutorialMethodForBuffName (buffName) + "SHOW SWF INFO ::::: " + showSwfInfo);
		
		if (TutorialManager.instance.IsTutorialCompleted (GetTutorialMethodForBuffName (buffName)))
			GameManager.instance.scaleformCamera.hud.AddPlayerDebuff (buffName, onSelf, showSwfInfo);
		else {
			if (!GameManager.instance._levelManager.battleManager.IsBattleEnded) {
				GameManager.instance.scaleformCamera.hud.AddPlayerDebuff (buffName, onSelf, showSwfInfo);
				if (showTutorial && TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None)
					TutorialManager.instance.ShowTutorial (GetTutorialMethodForBuffName (buffName));
			}
		}
	}
	
	
	public Buff GetBuffForSpell (string spellString)
	{
		InventorySystem.ItemRune aRune = null;
		List<InventorySystem.ItemRune> runeList = this._inventory.staffRunes;
		int count = runeList.Count;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("countcountcountcountcountcountcountcount -------------------------- " + count + " spellString " + spellString);
		for (int i = 0; i < count; i++) {
			aRune = runeList [i];
			if (InventorySystem.ItemRune.GetElementForElementString (spellString) == aRune.element) {
				return aRune.buff;
			}
		}
		return null;
	}
		
	public Buff GetBuffForBuffName (string buffName)
	{
		InventorySystem.ItemRune aRune = null;
		List<InventorySystem.ItemRune> runeList = this._inventory.staffRunes;
		int count = runeList.Count;
		for (int i = 0; i < count; i++) {
			aRune = runeList [i];
			if (aRune.buff.id.ToLower ().Equals (buffName.ToLower ())) {
				return aRune.buff;
			}
		}
		return null;
	}
	
	
	protected override string GetName ()
	{
		return "Player";
	}
	
	#endregion
	
	#region User's json
	[Newtonsoft.Json.JsonIgnore]
	private string
		_json;

	[Newtonsoft.Json.JsonIgnore]
	public string json {
		get {
			string jsonString = "{";
			jsonString += "\"Inventory\" :" + this._inventory.json + ",";
			jsonString += "\"life\" :" + this.defaultLife + ",";
			jsonString += "\"multiplayerLife\" :" + this.multiplayerLife + ",";
			jsonString += "\"SPIRIT\" :" + Helpers.AddQuotes (this.spiritId) + ",";
			jsonString += "\"availablePets\" :" + MiniJSON.Json.Serialize (this.availablePets) + ",";
			jsonString += "\"currentLife\" :" + this.life + ",";
			jsonString += "\"damage\" :" + this.defaultDamage + ",";
			jsonString += "\"arcanePoints\" :" + this.arcanePoints + ",";
			jsonString += "\"wards\" :" + Helpers.AddQuotes (this._wards + "") + ",";
			jsonString += "\"username\" :" + Helpers.AddQuotes (this.username + "") + ",";
			jsonString += "\"modelName\" :" + Helpers.AddQuotes (this.modelName + "") + ",";
			jsonString += "\"PetUpgrade\" :" + MiniJSON.Json.Serialize (this.petUpgradeDictionary) + ",";
			/*if(this.guild != null)
				jsonString += "\"Guild\" :" + MiniJSON.Json.Serialize(this.guild.ToDictionary()) + ",";
			else
				jsonString += "\"Guild\" :" + MiniJSON.Json.Serialize(null) + ",";*/
			jsonString += "\"TransmutationProgress\" : " + MiniJSON.Json.Serialize (LoadTransmutationProgress (this.transmutationDictionary));
			jsonString += "}";
		
//			if(GameManager.PRINT_LOGS) Debug.Log("*********************************************" + jsonString);
			_json = jsonString;
			return this._json;
		}
		set {
			_json = value;
		}
	}
	#endregion
	
	public User Clone ()
	{
		User user = new User ();
		user.id = this.id;
		user.name = this.name;
		user.defaultLife = this.defaultLife;
		user.defaultDamage = this.defaultDamage;
		user.totalLife = user.totalLife;
		user._inventory = this._inventory;
		user.availablePets = this.availablePets;
		user.spiritId = this.spiritId;
		user.SetRingsAndPetStats ();
		user.username = this.username;
		user.modelName = this.modelName;
		user.life = this.life;
		user.multiplayerLife = this.multiplayerLife;
		user.damage = this.damage;
		user._wards = this._wards;
		user.team = this.team;
		user.arcanePoints = this.arcanePoints;
		user.transmutationDictionary = this.transmutationDictionary;
		user.petUpgradeDictionary = this.petUpgradeDictionary;
		user.guild = this.guild;
		user.availablePets = this.availablePets;
		user.canTransmute = this.canTransmute;
		//        if (GameManager.PRINT_LOGS) Debug.Log("AFTER ::: total life:::" + this.totalLife + "::: damage:::" + this.damage);
		return user;
	}
	
	public int id;
	
	public void setName (string name)
	{
		this.name = name;
	}
	
	public string getName ()
	{
		return this.name;
	}
	
	public bool HasAvailableGesture (GestureEmitter.Gesture aGesture, bool runeSpell = false)
	{
		if (runeSpell) {
			for (int i = 0; i<availableRuneGestures.Length; i++) {
				if (availableRuneGestures [i] == aGesture)
					return true;
			}
		} else {
			for (int i = 0; i<availableGestures.Length; i++) {
				if (availableGestures [i] == aGesture)
					return true;
			}
		}
		return false;
	}


	[Newtonsoft.Json.JsonIgnore]
	public GestureEmitter.Gesture[] availableRuneGestures {
		get {
			List<GestureEmitter.Gesture> availList = new List<GestureEmitter.Gesture> ();
			
			List<ItemRune> staffRunes = _inventory.staffRunes;
			for (int i = 0; i < staffRunes.Count; i++) {
				GestureEmitter.Gesture staffGesture = SRCharacterController.GetGestureForSpellName (staffRunes [i].element.ToString ());
//				if (GameManager.PRINT_LOGS)
//					Debug.Log ("staffGesture" + staffGesture);
				if (staffGesture != GestureEmitter.Gesture.kInvalid) {
					availList.Add (staffGesture);
				}
			}
			return availList.ToArray ();
		}
	}


	[Newtonsoft.Json.JsonIgnore]
	public GestureEmitter.Gesture[] availableGestures {
		get {
			SetRingsAndPetStats ();
			List<GestureEmitter.Gesture> availList = new List<GestureEmitter.Gesture> ();
			if (_hasEarth)
				availList.Add (GestureEmitter.Gesture.kEarth);
			if (_hasFire)
				availList.Add (GestureEmitter.Gesture.kFire);
			if (_hasWater)
				availList.Add (GestureEmitter.Gesture.kWater);
			if (_hasLightning)
				availList.Add (GestureEmitter.Gesture.kLightning);
			return availList.ToArray ();
		}
	}
	
	public bool UsePotion (InventorySystem.InventoryItem.Type potionType)
	{
		if (potionType == InventorySystem.InventoryItem.Type.kHealthPotion)
			return _inventory.potionBelt.RemovePotion (InventorySystem.InventoryItem.Type.kHealthPotion);
		else
			return _inventory.potionBelt.RemovePotion (InventorySystem.InventoryItem.Type.kWardPotion);
	}
	
	private InventorySystem.Inventory userInventory;
	
	public InventorySystem.Inventory _inventory {
		set {
			userInventory = value;
		}
		get {
			return this.userInventory;
		}
	}
	
	public bool canTransmute {
		private set {	}
		get {
			if (_inventory.bag.BagCount () >= 3)
				return true;
			bool foundRing = false;
			bool foundRune = false;
			for (int i = 0; i<_inventory.bag.bagItems.Count; i++) {
				if (_inventory.bag.bagItems [i] as ItemRing != null) {
					if (foundRing) 
						return true;
					foundRing = true;
				}
				if (_inventory.bag.bagItems [i] as ItemRune != null) {
					if (foundRune) 
						return true;
					foundRune = true;
				}
			}
			
			return false;
		}
	}
	
	public int defaultLife;
	
	public void SetRingsAndPetStats ()
	{
 int ringHP = 0;
		int ringDamage = 0;
		int ringWards = 0;
		_hasEarth = false;
		_hasFire = false;
		_hasWater = false;
		_hasLightning = false;
		
		for (int i = 0; i<_inventory.equippedRings.Count; i++) {			
			InventorySystem.ItemRing ring = _inventory.equippedRings [i];
			
			ringHP += ring.life;
			ringDamage += ring.damage;
			ringWards += ring.wards;
			if (ring.earth > 0)
				_hasEarth = true;
			if (ring.fire > 0)
				_hasFire = true;
			if (ring.water > 0)
				_hasWater = true;
			if (ring.lightning > 0)
				_hasLightning = true;
		}
		
		if (this.defaultLife <= 0 || this.defaultDamage <= 0)
			throw new System.Exception ("PLAYER DEFAULT VALUES NOT SET! PLEASE SET DEFAULT LIFE AND DAMAGE FIRST ");
		
		this.damage = this.defaultDamage + ringDamage;
		this.totalLife = this.defaultLife + ringHP;
		this._wards = ringWards;


		#region PET_PASIVE_STAT
		if (this.spiritId == null || this.spiritId.Equals ("")) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Pet is null in User.");
			return;
		}

		PetModel pet = new PetModel (this.spiritId);

		if (pet.abilityStat == "LIFE") {
			int inc = Mathf.CeilToInt ((((float)pet.buffTime) / 100) * (float)totalLife);
			totalLife += inc;
		}
		
		if (pet.abilityStat == "DAMAGE") {
			int inc = Mathf.CeilToInt ((((float)pet.buffTime) / 100) * (float)damage);
			damage += inc;
		}
				
		if (pet.abilityStat == "WARD") {
			_wards += Mathf.CeilToInt ((((float)pet.buffTime) / 100) * (float)_wards);
		}
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Ability: " + pet.abilityStat + ", Current Life: " + life + ", Total Life: " + totalLife);
		#endregion PET_PASIVE_STAT
	}
	
	public void SetLifeToFull ()
	{
		if (GameManager.instance.isMultiPlayerMode)
			this.multiplayerLife = this.totalLife;
		
		this.life = this.totalLife;
	}
	
	
	
	private int _multiplayerLife; 
	public int multiplayerLife {
		set {
//			if(GameManager.PRINT_LOGS) Debug.Log("value for multiplayer life ================= " + value);
			if (value <= 0)
				_multiplayerLife = 0;
			else if (value >= this.totalLife)
				_multiplayerLife = this.totalLife;
			else
				_multiplayerLife = value;
		}
		get {
			return _multiplayerLife;
		}
	}
	
	public int defaultDamage;
	
	public bool isSpiritEnabled {
		private set	{	}
		get {
			return !string.IsNullOrEmpty (spiritId);
			//if(spiritId != null && !spiritId.Equals(""))
			//	return true;
			//return false;
		}
	}
	
	public int _wards;
	public bool _hasFire;
	public bool _hasLightning;
	public bool _hasEarth;
	public bool _hasWater;


	private string _spiritId;
	public string spiritId {
		set {
			_spiritId = value;
			if (_spiritId != null && !_spiritId.Equals ("")) {
				bool exists = false;
				string petId = "";
				int count = availablePets.Count;
				for (int i = 0; i < count; i++) {
					petId = availablePets [i];
					if (spiritId.Equals (petId))
						exists = true;
				}
				if (!exists)
					AddToAvailablePets (_spiritId);
			}
		}
		get {
 return _spiritId;
		}
	}
	
	protected override void PopulateProperties ()
	{
		if (propertyDictionary.ContainsKey ("life"))
			propertyDictionary ["life"] = this.life;
		else
			propertyDictionary.Add ("life", this.life);
		
		if (propertyDictionary.ContainsKey ("damage"))
			propertyDictionary ["damage"] = this.damage;
		else
			propertyDictionary.Add ("damage", this.damage);
		
		if (propertyDictionary.ContainsKey ("wards"))
			propertyDictionary ["wards"] = this._wards;
		else
			propertyDictionary.Add ("wards", this._wards);
	}
	

	public GuildSystem.Guild guild;
	public Team team;

	
	public float arcanePoints = 100;
}