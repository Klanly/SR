using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour, BattleManagerInterface
{
	
	#region varialbes
	
	Dictionary<string, BattleManagerInterface> nameToBattleManagerDictionary = null;
	
	private const string TAG = "*** BattleManager ...";
	
	private const int ENEMY_DAMAGE_AMPLIFICATION = 1;
	
	private int lifeAtBattleStart;
	
	public BattleState _battleState;
	
	public SRCharacterController _characterController;
	
	public AICharacterControllerNEWAgain _aiController;
	
	public GestureEmitter _gestureEmitter;
	
	public GameObject _gestureObject;
	
	private GameObject wardObject;
	
	public GameObject shieldObject;
	
	public GameObject hasteObject;
	
	public GameObject regenObject;
	
	public GameObject amplifyObject;
	
	private GameObject shieldClone;
	
	public bool _aiThrown = false;
	
	private int _numberOfTurns = 0;
	
	public bool showCastCue = false;
	
	private bool stunEnded = true;
	
	private int wardCount = 0;
	
	private GestureEmitter.Gesture counterGesture = GestureEmitter.Gesture.kInvalid;
	
	private bool isShieldTut = false;
	
	private bool playerLongCasted = false;
	
	#region Victory data
	private int maxDamage;
	
	private int healthLost;
	
	private int soulsWon;
	
	private float startTime;
	private float endTime;
	private int enemyInitialHealth;
	#region victory grade criteria
	
	int gradeTime = 0;
	int gradeMaxDamageDone = 0;
	int gradeDamageTaken = 1;
	#endregion


	public delegate void ReadyForSpellCast(float seconds);
	public event ReadyForSpellCast sendReadyForSpellCast;

	
	public enum BattleProgressState
	{
		kNone,
		kPlayerFocus,
		kEnemyFocus,
		kBothFocus,
		kCollisionOccurred,
		kEnemyStun,
		kEnemyBeforeStun,
		kFirstTurn
	}
	public BattleProgressState _battleProgressState = BattleProgressState.kNone;
	
	#endregion
	
	public enum EnemyState
	{
		kReady,
		kBusy,
		kStun,
		kPostReleaseFocus,
		kHit
	}
	private GestureEmitter.Gesture lockedGesture = GestureEmitter.Gesture.kInvalid;
	private GestureEmitter.Gesture enemyGesture = GestureEmitter.Gesture.kInvalid;
	public delegate void AIGestureDelegate (GestureEmitter.Gesture aGesture);
	private GestureEmitter.Gesture recognizedGesture;

	public EnemyState enemyState = EnemyState.kReady;
	
	bool enemyLongCast = false;
	
	private bool _isTouchReleased = true;
	public bool IsTouchReleased {
		private set {
			this._isTouchReleased = value;
		}
		get {
			return this._isTouchReleased;
		}
	}
	
	public bool scaleformBattleEnded = false;
	
	protected bool battleEnded = true;
	
	public GameObject playerGameObject;
	
	public GameObject _aiGameObject;
	
	private int buffCount;
	
	private GameObject spiritObject;
	public GameObject spiritPosGameObject;
	
	private PetLoader _petLoader;
	
	public PetController _petController;
	private int petBuffTimer = -1;
	
	#endregion
	public enum State
	{
		kIdle,
		kFocus,
		kWaitingForGesture,
		kRevert,
		kRecognizedCast,
		kLaunch,
		kDamage,
		kNone}
	;
	
	public enum EnemyStunState
	{
		kNone,
		kWillStun,
		kStunStarted,
		kStunEnded
	}
	public EnemyStunState _enemyStunState = EnemyStunState.kStunEnded;
	
	protected State currentState = State.kNone;
	
	public float stunTimer;
	
	private enum RuneAttackStatus
	{
		kBlocked,
		kReady,
		kQueued
	}
	private RuneAttackStatus _runeAttackStatus = RuneAttackStatus.kReady;

	public enum AttackMode
	{
		kRapid,
		kNormal
	}
	AttackMode _aMode;
	AttackMode _attackMode {
		set {
			if (_aMode != value)
				_aMode = value;	
		}
		get {
			return _aMode;
		}
	}
	
	private TouchDetector _touchDetector;
	
	/// <summary>
	/// Allow player/enemy damage. Only for debugging/testing purposes!
	/// </summary>
	public bool allowPlayerDamage;
	public bool allowEnemyDamage;
	public bool wardOn;

	public int runeDayCount = 2;

	protected void OnAiLoaded (GameObject aiGameObject)
	{
		aiGameObject.tag = "Enemy";
		
		_aiController = aiGameObject.GetComponent<AICharacterControllerNEWAgain> ();
		_aiController.enemyHitTransform = _characterController.characterHitTransform.gameObject;
		_aiController._stundelegate = this.OnAIStun;
		_aiController._aiWillStunDelegate = this.OnAIWillStun;
		_aiController._stunEndDelegate = this.OnAiStunEnded;
		
		_characterController.enemyHitTransform = _aiController.projectileHitTransform.gameObject;
		
		_battleState._enemy = null;
		_battleState._enemy = _aiController._aiModel;

		spiritPosGameObject = GameObject.Find ("PetPosition");

		//ResetBattleState();
		_characterController.ResetRun ();
		Initialize ();
		
	}
	
	public void InstantiateAIModelToBattleManagerDictionary ()
	{
		nameToBattleManagerDictionary = new Dictionary<string, BattleManagerInterface> ();
		nameToBattleManagerDictionary [AIModel.NameTypes.HUMAN_ACOLYTE.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.HUMAN_MAGICKER.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.OGRE_SHAMAN.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.OGRE_WARLOCK.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.OGRE_BATTLEMAGE.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.PIT_FIEND.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.VALKYRIE.ToString ()] = this;
		nameToBattleManagerDictionary [AIModel.NameTypes.PRIMUS_NEX.ToString ()] = new BossBattleManager (this);
		nameToBattleManagerDictionary [AIModel.NameTypes.OGRE_WARLORD.ToString ()] = new WarlordBattleManager (this);
		nameToBattleManagerDictionary [AIModel.NameTypes.Jaghdarr.ToString ()] = new WarlordBattleManager (this);
	}
	
	void Update ()
	{
		if ((_battleProgressState == BattleProgressState.kEnemyStun || _battleProgressState == BattleProgressState.kBothFocus) && (enemyState == EnemyState.kReady) && (currentState == State.kFocus) && _attackMode == AttackMode.kNormal && _characterController.livingProjectiles.Count == 0 && stunTimer == 0) {
			_battleProgressState = BattleProgressState.kNone;
			if (_battleProgressState == BattleProgressState.kEnemyStun) {
				nameToBattleManagerDictionary [_aiController._aiModel.name].InitiateRound (false);
			} else {
				nameToBattleManagerDictionary [_aiController._aiModel.name].InitiateRound (true);
			}
			
		}
		if (stunTimer > 0)
			stunTimer -= Time.deltaTime;
		if (stunTimer < 0) {
			stunTimer = 0;
			_aiController.EndStun ();
//			_attackMode = AttackMode.kNormal;
			_characterController.DisableBurst ();
			maxDamage = (burstModeDamage > maxDamage) ? burstModeDamage : maxDamage;
			burstModeDamage = 0;
			StartCoroutine (DelayEnemyStunOut (1.8f));
		} else if (stunTimer > 0) {
			_gestureEmitter.enabled = false;
			_attackMode = AttackMode.kRapid;
		}
		
		if (_attackMode == AttackMode.kRapid) {
		} else {
			_touchDetector.StopRapidFireUpdates ();
			allowRapidAttack = false;
			if (_battleState._enemy.life < 1 && !battleEnded) {
				OnEnemyDied ();
			}
		}
	}
	
	private IEnumerator DelayEnemyStunOut (float time)
	{
		yield return new WaitForSeconds (time);
		ShowStaff ();
//		StaffMode(false);
//        if (GameManager.PRINT_LOGS) Debug.Log("new WaitForSeconds ENDED!!!");
		OnAiStunEnded ();
	}
	
	bool enemyDelayStun = false;
	
	public virtual void Initialize ()
	{
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 0");
		
		if (GameManager.instance.isMultiPlayerMode)
			GameManager.instance.allowHealOverTime = false;
		
		TutorialManager.instance.currentTutorial = TutorialManager.TutorialsAndCallback.None;
		TutorialManager.instance.state = TutorialManager.TutorialsAndCallback.Idle;
		
		if (nameToBattleManagerDictionary == null || nameToBattleManagerDictionary.Keys.Count <= 0)
			InstantiateAIModelToBattleManagerDictionary ();
		
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 1");
		
		#region Victory case
		maxDamage = 0;
		healthLost = 0;
		soulsWon = 0;
		
		gradeTime = 0;
		gradeMaxDamageDone = 0;
		gradeDamageTaken = 1;
		#endregion
		startTime = Time.time;
		
		buffCount = 0;
		battleEnded = false;
		
		_battleState._user = GameManager._gameState.User.Clone ();
		
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 2");
		
		lifeAtBattleStart = _battleState._user.life;
		_battleState._user.ClearBuffs (this);
		_battleState._enemy.ClearBuffs (this);
		
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 3");
		
		wardCount = _battleState._user._wards;
		_numberOfTurns = 0;
		stunEnded = true;

		_characterController._charFocusStateListener = this.OnFocusStateReached;
		//if(GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 3 B");
		_characterController.characterLongCastListener = this.OnPlayerChargedCast;
		//if(GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 3 C");
		_characterController.SetFocusState ();
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 4");
		
		//Camera.main.gameObject.AddComponent<UICamera>();
		
		_petLoader = gameObject.GetComponent<PetLoader> ();
		
		_aiController._onFocusStateListener = this.OnAIFocusStateReached;
		
		_aiController.SetFocus ();
	
//        if (GameManager.PRINT_LOGS) Debug.Log("Initialize!!! 5");
		
		if (wardCount > 0)
			GameManager.instance.scaleformCamera.hud.TurnOnWard (wardCount);
		
//        if (GameManager.PRINT_LOGS) Debug.Log("AI MODEL NAME --->>>" + _aiController._aiModel.name);
		_touchDetector = gameObject.GetComponent<TouchDetector> ();
		nameToBattleManagerDictionary [_aiController._aiModel.name].OnInitializationComplete ();
		_attackMode = AttackMode.kNormal;
		
		
		/* if(_runeAttackStatus == RuneAttackStatus.kReady)
			_gestureEmitter.SetNormalLibrary(true);
		else
			_gestureEmitter.SetNormalLibrary(false); */
		
		_enemyStunState = EnemyStunState.kStunEnded;
//        if (GameManager.PRINT_LOGS) Debug.Log("------------------------------------------------------------------->>> AttackMode.kNormal");
		enemyInitialSpeed = _aiController._aiModel.speed;
		
		runeString = "";
			
		for (int i = 0; i<_battleState._user.availableRuneGestures.Length; i++) {
			if (SRCharacterController.GetRuneNameForRuneGesture (_battleState._user.availableRuneGestures [i]) != null)
				runeString += SRCharacterController.GetRuneNameForRuneGesture (_battleState._user.availableRuneGestures [i]);
			
			if (i < _battleState._user.availableRuneGestures.Length - 1)
				runeString += ",";
		}
		if(UIManager.instance.guildUI != null) {
			NGUITools.SetActive(UIManager.instance.guildUI.gameObject, false);
		}

		GameManager.instance.scaleformCamera.hud.SetLoadingFill(true);

	}
	
	string runeString;
	
	private void ShowStaff ()
	{
//        if (GameManager.PRINT_LOGS) Debug.Log("ShowStaff :::::::: GameManager._gameState.dayCount > 2" + (GameManager._gameState.dayCount > 2) + "runeGestureCount > 0" + (runeGestureCount > 0));
		if (GameManager._gameState.dayCount > runeDayCount)
			GameManager.instance.scaleformCamera.hud.StaffEnable (runeGestureCount > 0);
		else
			GameManager.instance.scaleformCamera.hud.StaffEnable (false);
	}
	
	private void StaffMode (bool yesNo)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("GameManager.instance.scaleformCamera.hud.StaffModeEnable = " + yesNo);
		GameManager.instance.scaleformCamera.hud.StaffModeEnable (yesNo);
	}
	
	private float raidModelUnchangedTotalLife;
	private int runeGestureCount = 0;
	public void OnInitializationComplete ()
	{
		enemyInitialHealth = _battleState._enemy.life;
//        Debug.Log("OnInitializationComplete ::: enemyInitialHealth > " + enemyInitialHealth);
		if ((_battleState._enemy as  RaidAIModel) != null) {
			raidModelUnchangedTotalLife = _battleState._enemy.totalLife;
			
			_battleState._enemy.totalLife = enemyInitialHealth;
		}
		
		_touchDetector.playerGameObject = this._characterController.gameObject;
		_touchDetector.enemyGameObject = this._aiController.gameObject;
		_touchDetector._positionListener = this.RapidFireTouch;
		SummonPet ();

		// Updating the User Stats with the addition of the Pet Passive Stat Abilities.
		//Debug.LogError("Default Life: "+_battleState._user.defaultLife+", "+"Life: "+_battleState._user.life+", "+"Total Life: "+_battleState._user.totalLife);
		//Debug.LogError("Ability Stat: "+_petController._petModel.abilityStat+", %age: "+_petController._petModel.buffTime);
		#region PET_PASIVE_STAT
//        if (_petController != null && _petController._petModel.abilityStat == "LIFE")
//        {
//            int inc = Mathf.CeilToInt((((float)_petController._petModel.buffTime) / 100) * (float)_battleState._user.totalLife);
//            _battleState._user._totalLife += inc;
//            _battleState._user._life += inc;
//            _battleState._user.defaultLife += inc;
//        }
//
//        if (_petController != null && _petController._petModel.abilityStat == "DAMAGE")
//        {
//            int inc = Mathf.CeilToInt((((float)_petController._petModel.buffTime) / 100) * (float)_battleState._user._damage);
//            _battleState._user._damage += inc;
//            _battleState._user.defaultDamage += inc;
//        }
//		
//        if (_petController != null && _petController._petModel.abilityStat == "WARD")
//        {
//            _battleState._user.wards += Mathf.CeilToInt((((float)_petController._petModel.buffTime) / 100) * (float)_battleState._user.wards);
//            wardCount = _battleState._user.wards;
//            if (wardCount > 0)
//                GameManager.instance.scaleformCamera.hud.TurnOnWard(wardCount);
//        }
		#endregion PET_PASIVE_STAT
		//Debug.LogError("Default Life: "+_battleState._user.defaultLife+", "+"Life: "+_battleState._user.life+", "+"Total Life: "+_battleState._user.totalLife);

		runeGestureCount = _battleState._user.availableRuneGestures.Length;
//        if (GameManager.PRINT_LOGS) Debug.Log("======runeGestureCount========" + runeGestureCount);
		if (runeGestureCount > 0) {
//            if (GameManager.PRINT_LOGS) Debug.Log("_battleState._user.availableRuneGestures.Length > 0");
		} else {
//			GameManager.instance.scaleformCamera.hud.StaffEnable(false);
//            if (GameManager.PRINT_LOGS) Debug.Log("_battleState._user.availableRuneGestures.Length <<<<<<<<<<<<<<<<<<<<<<< 0");
		}
		
		ShowStaff ();
		
		if (GameManager.enableAnalytics)
			Analytics.logEvent (Analytics.CurrencyTrackType.TrackCombats, ++GameManager._gameState.combatsFought);
		
		nameToBattleManagerDictionary [_aiController._aiModel.name].OnPauseBeforeFight ();
	}
	
	public void OnPauseBeforeFight ()
	{
		
		InitiateBattle (4.0f);
	}
	
	public void OnAIFocusStateReached ()
	{	
		nameToBattleManagerDictionary [_aiController._aiModel.name].PerformAIFocusStateReached ();
		
		if (stunTimer == 0 && enemyDelayStun)
			_enemyStunState = EnemyStunState.kStunEnded;
		
		if (stunTimer == 0) {
			_attackMode = AttackMode.kNormal;
		}
	}
	
	public void InitiateBattle (float time)
	{
		Invoke ("InitiateBattle", time);
		if (!battleEnded) {
			GameManager.instance.scaleformCamera.hud.SetLoadingFill(true);
			GameManager.instance.scaleformCamera.hud.ChainStun (false);
			sendReadyForSpellCast(time);
			showCastCue = false;
		}

	}

	protected void InitiateBattle ()
	{
		if (!battleEnded)
			nameToBattleManagerDictionary [_aiController._aiModel.name].BattleSetStart ();
		destroyProjectiles = true;
	}
	
	public void OnPlayerChargedCast ()
	{
		playerLongCasted = true;
		ShowHoldGesture (true);
	}
	
	protected void ShowHoldGesture (bool yesNo)
	{
		_gestureEmitter.emitter.gameObject.GetComponent<ParticleEmitter> ().enabled = yesNo;
		_gestureEmitter.emitter.gameObject.GetComponent<ParticleRenderer> ().enabled = yesNo;
	}
	
//	private const int RUNE_ROUND_FREQUENCY = 7;
	public int RUNE_ROUND_FREQUENCY = 7;

	public void BattleSetStart ()
	{
		if (battleEnded)
			return;

		GameManager.instance.scaleformCamera.hud.HideBossFightText ();
		EnableSpirit();
		ShowStaff ();

		//        if (GameManager.PRINT_LOGS) Debug.Log("BATTLE SET START - CURRENT State" + currentState + ":::::::::: TUTORIAL STATE :::::::" 
//                + TutorialManager.instance.state + ":::::::::: CURRENT TUTORIAL  :::::::" + TutorialManager.instance.currentTutorial);
		_characterController.OnSetStart ();
		
		if (_runeAttackStatus == RuneAttackStatus.kQueued)
			ExecuteRuneAttack ();
		
		if (GameManager.instance.runeProgress < RUNE_ROUND_FREQUENCY) {
			GameManager.instance.scaleformCamera.hud.BattleStaff (GameManager.instance.runeProgress++);
		}
		if (GameManager.instance.runeProgress >= RUNE_ROUND_FREQUENCY) {
			GameManager.instance.scaleformCamera.hud.BattleStaff (10 + GameManager.instance.runeProgress);
			if (GameManager._gameState.dayCount > runeDayCount && runeGestureCount > 0)
				TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.runeSpellTutorialStart);
		}
		
		if (spiritObject != null) {
			_petController.OnSetStart ();
			//OnSpiritPassive();
		}
			
		#region Buff tick & death handling
		if (_battleState._user != null) {
			if (!battleEnded)
				_battleState._user.TickBuffs (this);
			if (_battleState._user.life <= 0) {
				OnPlayerDied ();
				return;
			}
		}
		
		if (_battleState._enemy != null) {
			if (!battleEnded)
				_battleState._enemy.TickBuffs (this);

			if (_battleState._enemy.life <= 0) {
				OnEnemyDied ();
				return;
			}
		}
		#endregion
		
		Buff aiBuffToApply = ApplyBuffIfApplicable ();
		if (aiBuffToApply != null && aiBuffToApply.id.ToLower ().Equals ("prison"))
			aiBuffToApply = null;
		if (aiBuffToApply != null) {
			if (aiBuffToApply.id.ToLower ().Equals ("amplify"))
				ShowAmplifyEffect (false);
			else if (aiBuffToApply.id.ToLower ().Equals ("regen"))
				ShowRegenEffect (false);
			
			if (aiBuffToApply.buffType == Buff.BuffType.kPositive)
				_battleState._enemy.ApplyBuff (aiBuffToApply, this);
			else
				_battleState._user.ApplyBuff (aiBuffToApply, this);
			aiBuffToApply = null;
		}

		if (_battleState._user.HasApplied ("DISPEL")) {
			Debug.LogError ("Dispel is still here but it has done the work it was supposed to do.");
		}

		// Spirit Smash Spell
		if (_battleState._enemy.HasApplied ("SMASH")) {
			_aiController.OnHit (0f, false, GestureEmitter.Gesture.kSmash, definiteStun: true);
			Debug.Log ("Smashed!!!");
			_battleState._enemy.availableBuffs.Remove ("SMASH");
		}

		if (_attackMode == AttackMode.kNormal)
			_gestureEmitter.enabled = true;
		
		if (_enemyStunState == EnemyStunState.kStunStarted) {	
			enemyState = EnemyState.kBusy;
		} else {
//			enemyState = EnemyState.kBusy;
//            if (GameManager.PRINT_LOGS) Debug.Log("_aiController.Attack CALLED ----------------    ");
			if (_enemyStunState == EnemyStunState.kStunEnded) {
				_aiController.Attack (++_numberOfTurns, enemyLongCast, isShieldTut, (GestureEmitter.Gesture aGesture) =>
				{
					enemyState = EnemyState.kBusy;
					
					enemyGesture = aGesture;
				
					if (TutorialManager.instance.currentTutorial == AICharacterControllerNEWAgain.gestureTutorialDictionary [aGesture]) {
						if (ProjectileStatsComparator.counterDictionary.ContainsKey (aGesture)) {
							counterGesture = ProjectileStatsComparator.counterDictionary [aGesture];
							Debug.LogError("Showing tutorial - "+TutorialManager.instance.currentTutorial+" counter gesture - "+ counterGesture.ToString());
						} else {
							counterGesture = GestureEmitter.Gesture.kInvalid;
							Debug.LogError("Showing tutorial - "+TutorialManager.instance.currentTutorial+" counter gesture - kInvalid");
						}
					}
				});
			}
		}
		
		if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.WaterTutorialEnd 
			|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.FireTutorialEnd 
			|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.LightningTutorialEnd 
			|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.EarthTutorialEnd
		//|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.NexFightTutorialEnd
			|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.Idle) {
			showCastCue = true;
//			Debug.LogError("BattleSet start Showcast cue "+showCastCue);
			GameManager.instance.scaleformCamera.hud.SetLoadingFill(false);
		}
	}
	
	AIModel.Speed enemyInitialSpeed;
	public void HasteApplied () //enemy only buff!
	{
		_aiController._aiModel.speed = AIModel.Speed.HASTE;
		
#region Particle effect
		GameObject hasteClone = GameObject.Instantiate (hasteObject) as GameObject;
		hasteClone.transform.position = _aiGameObject.transform.position;
		hasteClone.AddComponent<DeleteScript> ();
		
		DeleteScript delScript = hasteClone.GetComponent<DeleteScript> ();
		delScript.DeleteObjectIn (2f);
		
#endregion
	}
	
	private void ShowRegenEffect (bool onUser)
	{
		GameObject regenClone = GameObject.Instantiate (regenObject) as GameObject;
		if (onUser)
			regenClone.transform.position = playerGameObject.transform.position;
		else
			regenClone.transform.position = _aiGameObject.transform.position;
		
		regenClone.AddComponent<DeleteScript> ();
		
		DeleteScript delScript = regenClone.GetComponent<DeleteScript> ();
		delScript.DeleteObjectIn (2f);
	}
	
	private void ShowAmplifyEffect (bool onUser)
	{
		GameObject amplifyClone = GameObject.Instantiate (amplifyObject) as GameObject;
		if (onUser)
			amplifyClone.transform.position = playerGameObject.transform.position;
		else
			amplifyClone.transform.position = _aiGameObject.transform.position;
		
		amplifyClone.AddComponent<DeleteScript> ();
		
		DeleteScript delScript = amplifyClone.GetComponent<DeleteScript> ();
		delScript.DeleteObjectIn (2f);
	}
	
	public void HasteRemoved ()
	{
		_aiController._aiModel.speed = enemyInitialSpeed;
	}
	
	protected void OnFocusStateReached ()
	{
//        if (GameManager.PRINT_LOGS) Debug.Log("OnFocusStateReached + ========= currentState ============ " + currentState);
		if (_enemyStunState == EnemyStunState.kWillStun || _enemyStunState == EnemyStunState.kStunStarted)
			currentState = State.kNone;
		
		switch (currentState) {
		case State.kRevert:
		case State.kNone:
			currentState = State.kFocus;
			//TODO Change made here. Test if anything breaks
			// Allow gesture to made only when cast now is showing
			// Fixed. Player could cast anytime
			if (_attackMode == AttackMode.kNormal && showCastCue) {
//				Debug.LogError("Enabling gesture emitter");
				_gestureEmitter.enabled = true;
			}
//			if (_attackMode == AttackMode.kNormal) {
//				_gestureEmitter.enabled = true;
//			}
			break;
		default:
			_gestureEmitter.enabled = false;
			currentState = State.kFocus;
			if (_enemyStunState == EnemyStunState.kStunStarted || _enemyStunState == EnemyStunState.kWillStun) {
				_aiController.discardPreviousCallbacks = true;
				nameToBattleManagerDictionary [_aiController._aiModel.name].OnFocusStateWhileEnemyStun ();
			}
			break;
		}
	}
	

	
	protected void OnFingerEvent (bool fingerIsDown)
	{
		if (battleEnded)
			return;
		
		if (fingerIsDown)
			OnFingerDown ();
		else
			OnFingerUp ();
	}
	
	public void EndSpellBurstTutorial ()
	{
		stunTimer = 3.0f;
	}
	
	public void AllowRapidAttack ()
	{
		int angleDiff = 0;
		_touchDetector.UpdatePositions (nameToBattleManagerDictionary [_aiController._aiModel.name].GetSwipeDirection (), ( int angleD) => {
			angleDiff = angleD;
		});
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("angleDiff = 0;" + angleDiff);
		
		if (TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.spellBurstTutorialStart))
			stunTimer = AICharacterControllerNEWAgain.STUN_DURATION;
		else {
			TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.spellBurstTutorialStart, angleDiff);
			stunTimer = float.MaxValue;
		}
		allowRapidAttack = true;
		GameManager.instance.scaleformCamera.hud.ChainStun (true);
	}
	
	public void FinalRapidAttackStarted ()
	{
		stunTimer = 3f;
	}
	
	public TouchDetector.SwipeDirection GetSwipeDirection ()
	{
		return TouchDetector.SwipeDirection.kLeftRight;
	}
	
	bool allowRapidAttack = false;
	int burstModeDamage = 0;
	private void RapidFireTouch ()
	{
		if (allowRapidAttack) {
			GestureEmitter.Gesture gesture = GestureEmitter.Gesture.kEarth;
			_characterController.castSpell (gesture, true);
		}
	}
	
	protected void OnFingerDown ()
	{
		Debug.Log ("On Finger Down - CURRENT State" + currentState + ":::::::::: TUTORIAL STATE :::::::" + TutorialManager.instance.state + ":::::::::: CURRENT TUTORIAL  :::::::" + TutorialManager.instance.currentTutorial);
		
		if (_attackMode == AttackMode.kRapid) {
			return;
		}
		
		showCastCue = false; // ScaleForm information...

		IsTouchReleased = false; // ScaleForm information...
//		if(GameManager.PRINT_LOGS) Debug.Log("On Finger Down - currentState" + currentState);
		switch (currentState) {
		case State.kFocus:
				
			if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.WaterTutorialEnd 
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.FireTutorialEnd 
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.LightningTutorialEnd 
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.EarthTutorialEnd 
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.SecondStunEnd
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.NexFightTutorialEnd
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.OnRuneSpellTutorial2
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.Idle) {
				currentState = State.kWaitingForGesture;
//				if(GameManager.PRINT_LOGS) Debug.Log("State.kWaitingForGesture          State.kWaitingForGesture");
				_characterController.OnGestureStarted ();
			}	
			break;
		default:
			
//				if(GameManager.PRINT_LOGS) Debug.Log("On Finger Down - Unknown State" + currentState);
			break;
		}
	}
	
	public bool specialAttack {
		get {
			return _specialAttack;
		}
		set {
			_specialAttack = value;
			if (_specialAttack)
				_gestureEmitter.SetNormalLibrary (false);
			else
				_gestureEmitter.SetNormalLibrary (true);
		}
		
	}
	private bool _specialAttack;
	
	public void OnRuneAttackButtonPressed ()
	{
		if (_runeAttackStatus == RuneAttackStatus.kBlocked || _attackMode == AttackMode.kRapid)
			_runeAttackStatus = RuneAttackStatus.kQueued;
		else
			ExecuteRuneAttack ();
	}
	
	private void ExecuteRuneAttack ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("TutorialManager.instance.state" + TutorialManager.instance.state + "TutorialManager.instance.currentTutorial" + TutorialManager.instance.currentTutorial);
		if (!battleEnded && _attackMode != AttackMode.kRapid && (TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None
			|| TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.runeSpellTutorialStart)) {
			if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.OnRuneSpellTutorialStart)
				TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.runeSpellTutorial2);
			
			if (GameManager._gameState.dayCount > runeDayCount)
				GameManager.instance.scaleformCamera.hud.SetRuneGestures (runeString);
			
			///////////////////////
			StaffMode (true);
			ShowStaff ();
			///////////////////////
			Time.timeScale = 0.3f;
			specialAttack = true;
			//_gestureEmitter.SetNormalLibrary(false);
			GameManager.instance.runeProgress = 0;
			_runeAttackStatus = RuneAttackStatus.kReady;
		}
	}
	
	public void OnGestureReceived (GestureEmitter.Gesture gesture)
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("GESTURE RECEIVED :::::::" + gesture + " --------- counter gesture is ----------" + counterGesture);
		if (_attackMode == AttackMode.kRapid) {
			if (gesture != GestureEmitter.Gesture.kInvalid)
				_characterController.castSpell (gesture, true);
			return;
		} else {
			switch (currentState) {
			case State.kWaitingForGesture:
				Debug.LogError("counterGesture - "+counterGesture.ToString()+" gesture - "+gesture.ToString());

				//if(GameManager.PRINT_LOGS) Debug.Log("---------------GESTURE RECEIVED - specialAttackRound------------------" + specialAttackRound + "GESTURE RECEIVED" + gesture + " specialAttack" + specialAttack);
				if (counterGesture != GestureEmitter.Gesture.kInvalid) { //This means that we are in tutorial - counterGesture is always Invalid except in tutorial
					if (gesture != counterGesture) {
						Debug.LogError("gesture "+gesture.ToString()+" is not equal to countergesture "+counterGesture.ToString());
//						if(gesture == GestureEmitter.Gesture.kInvalid) {
							showCastCue = true;
//						Debug.LogError("if Showcast cue "+showCastCue);
						//						}
						currentState = _characterController.castSpell (GestureEmitter.Gesture.kInvalid, false, specialAttack) ? State.kRecognizedCast : State.kRevert;
					} else {
						Debug.LogError("gesture is equal == to counter gesture");
						currentState = _characterController.castSpell (gesture, false, specialAttack) ? State.kRecognizedCast : State.kRevert;
						recognizedGesture = gesture;
						counterGesture = GestureEmitter.Gesture.kInvalid;
					}
				} else {
					// Added kInvalid so it would not let make a gesture after a failed attempt
					// Added elemental gestures because they HAVE to be drawn even if you miss
					if(gesture == GestureEmitter.Gesture.kInvalid || gesture == GestureEmitter.Gesture.Ignite || gesture == GestureEmitter.Gesture.LeechSeed
					   || gesture == GestureEmitter.Gesture.Daze || gesture == GestureEmitter.Gesture.Drain) {
//					if(gesture == GestureEmitter.Gesture.kInvalid) {
						showCastCue = true;
//						Debug.LogError("else Showcast cue "+showCastCue);
					}
					Debug.LogError("counterGesture is invalid "+gesture);

					
					if (!_battleState._user.HasAvailableGesture (gesture, specialAttack)) {
						GameManager.instance.scaleformCamera.hud.HighlightUnavailableElement (gesture);
						currentState = _characterController.castSpell (GestureEmitter.Gesture.kInvalid, false, specialAttack) ? State.kRecognizedCast : State.kRevert;
					} else {
						
						currentState = _characterController.castSpell (gesture, false, specialAttack) ? State.kRecognizedCast : State.kRevert;
						recognizedGesture = gesture;
						enemyGesture = recognizedGesture;

					}
				}
//				if(GameManager.PRINT_LOGS) Debug.Log("OnGestureReceived ---- State.kWaitingForGesture          State.kWaitingForGesture" + currentState);
				break;
			default:
//					if(GameManager.PRINT_LOGS) Debug.Log("On Gesture Received - Unknown State --->>>" + currentState);
				break;
			}
		}
	}
	
	protected void OnFingerUp ()
	{
		if (_attackMode == AttackMode.kRapid) {
			return;
		}
		
//        if (GameManager.PRINT_LOGS) Debug.Log("On Finger UP - CURRENT State" + currentState + ":::::::::: TUTORIAL STATE :::::::" + TutorialManager.instance.state + ":::::::::: CURRENT TUTORIAL  :::::::" + TutorialManager.instance.currentTutorial);
		ShowHoldGesture (false);
		
		IsTouchReleased = true;
		
		switch (currentState) {
		case State.kRecognizedCast:
			
			/*
			if(isShieldTut || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.SecondStunEnd 
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.SecondStunTryAgain)
			{
				if(playerLongCasted)
				{
					if(GameManager.PRINT_LOGS) Debug.Log("::::::::IF::::::::::CURRENT TUTORIAL >>>" + TutorialManager.instance.currentTutorial);
					
					if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.SecondStunEnd
						|| TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.SecondStunTryAgain)
						
						TutorialManager.instance.CombatResponse(TutorialManager.TutorialsAndCallback.SecondStunEnd);
				}
				else
				{
					if(GameManager.PRINT_LOGS) Debug.Log("::::::::ELSE::::::::::CURRENT TUTORIAL >>>" + TutorialManager.instance.currentTutorial);
//					if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.SecondStunEnd
//						|| TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.SecondStunTryAgain)
//						
//						TutorialManager.instance.CombatResponse(TutorialManager.TutorialsAndCallback.SecondStunTryAgain);

					currentState = State.kRevert;
					_characterController.castSpell(recognizedGesture);
					if(GameManager.PRINT_LOGS) Debug.Log("_characterController.DestroyProjectile();");
					_characterController.DestroyProjectile();
					_gestureEmitter.enabled = false;
					return;
				}
			}*/
			
			if (lockedGesture != recognizedGesture) {
//				if(GameManager.PRINT_LOGS) Debug.Log("THROW CAST!!! CALLED FOR CHARACTER----------------------------------------------------");
//                    if (GameManager.PRINT_LOGS) Debug.Log("OnFingerUp - TutorialManager.instance.state" + TutorialManager.instance.state + "TutorialManager.instance.currentTutorial" + TutorialManager.instance.currentTutorial);
				_characterController.ThrowCast (this.OnCastComplete);
			} else {
				_characterController.castSpell (GestureEmitter.Gesture.kInvalid);
				if (GameManager.PRINT_LOGS)
					Debug.Log ("_characterController.DestroyProjectile();");
				_characterController.DestroyProjectile ();
			}
			_gestureEmitter.enabled = false;
				
			break;
		case State.kRevert:
				
			_characterController.ThrowCast (this.OnCastComplete);
			_gestureEmitter.enabled = false;
				
			break;
		case State.kDamage:
			_gestureEmitter.enabled = false;
			break;
		default:
				
//				if(GameManager.PRINT_LOGS) Debug.Log("On FINGER UP Reached - Unknown State --->>>" + currentState);
			break;
		}
	}
	
	public void EnemyAboutToThrow ()
	{
		if (_runeAttackStatus == RuneAttackStatus.kQueued)
			_runeAttackStatus = RuneAttackStatus.kBlocked;
	}
	
	protected void OnCastComplete (SRCharacterController.CastState cState)
	{	
//        if (GameManager.PRINT_LOGS) Debug.Log("On Cast Complete Reached - CURRENT State --->>> " + currentState + " --- PLAYER CAST STATE ------->" + cState);
		if (cState == SRCharacterController.CastState.kInvalid)
			currentState = State.kRevert;
		switch (currentState) {
		case State.kRecognizedCast:
				
				
			_aiController.ThrowImmediate (isShieldTut);
				
			//	if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.NexFightTutorialEnd)
			//		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexCameraEnd);
			
			isShieldTut = false;
			playerLongCasted = false;
			break;

		default:
			recognizedGesture = GestureEmitter.Gesture.kInvalid;
//                if (GameManager.PRINT_LOGS) Debug.Log("On Cast Complete Reached - Unknown State --->>> " + currentState);
				
			break;
		}
	}
	
	protected void OnAIWillStun ()
	{
		
		if (!battleEnded) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("AI GETTING STUNNED!!! CURENT TUTORIAL :::::::::::: " + TutorialManager.instance.currentTutorial);
			if (TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.FirstStun || 
				TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.SecondStunStart)
				AICharacterControllerNEWAgain.STUN_DURATION = float.MaxValue;
			else
				AICharacterControllerNEWAgain.STUN_DURATION = 3.5f;
		
		} else
			AICharacterControllerNEWAgain.STUN_DURATION = 3.5f;
		
		_enemyStunState = EnemyStunState.kWillStun;
		_battleProgressState = BattleProgressState.kEnemyBeforeStun;
		_gestureEmitter.enabled = false;
		enemyDelayStun = false;
		_attackMode = AttackMode.kRapid;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("------------------------------------------------------------------->>> AttackMode.kRapid");
		_characterController.EnableBurst ();
	}
	
	protected void OnAIStun ()
	{
		enemyState = EnemyState.kStun;
		_battleProgressState = BattleProgressState.kEnemyStun;
		_enemyStunState = EnemyStunState.kStunStarted;

		stunEnded = false;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("InitiateBattle TO BE CALLED WITH AI STUN  CALLED");
		//InitiateBattle(0f); 
	}
	
	protected void OnAiStunEnded ()
	{
		if (_enemyStunState == EnemyStunState.kStunStarted) {
			_enemyStunState = EnemyStunState.kStunEnded;
			enemyState = EnemyState.kReady;
		}
		_battleProgressState = BattleProgressState.kBothFocus;
		enemyDelayStun = true;
		_attackMode = AttackMode.kNormal;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("------------------------------------------------------------------->>> AttackMode.kNormal");
	}
	

	private bool destroyProjectiles;
	
	
	
	
		
	#region Player/Enemy collision detection
	public virtual void OnGameobjectCollided (GameObject anObject, ProjectileStatsComparator projectileStats, bool destroyProj = true)
	{
		
		if (!destroyProj) {
			destroyProjectiles = false;
			return;
		}
		if (AICharacterControllerNEWAgain.gestureTutorialDictionary.ContainsKey (enemyGesture)) {
			if (TutorialManager.instance.currentTutorial == AICharacterControllerNEWAgain.gestureTutorialDictionary [enemyGesture])
				TutorialManager.instance.CombatResponse ();
		}
		if (anObject == null) //It means that the spells that collided were the same and cancelled the other out...
			nameToBattleManagerDictionary [_aiController._aiModel.name].SetEnemyReady ();
		
		if (anObject != null) {
//			GameManager.instance.scaleformCamera.hud.SetLoadingFill(true);
			if (anObject.tag.Equals ("Ward"))
				nameToBattleManagerDictionary [_aiController._aiModel.name].SetEnemyReady ();


			showCastCue = false; //Scaleform Information...

			if (anObject.tag.Equals (playerGameObject.tag)) {
				if (destroyProjectiles) {
					if (enemyState == EnemyState.kBusy || enemyState == EnemyState.kPostReleaseFocus)
						nameToBattleManagerDictionary [_aiController._aiModel.name].SetEnemyReady ();
				}
				

				currentState = State.kDamage;
				
				if (wardOn) {
					_characterController.OnWardHit (destroyProjectiles);
//					wardCount--;
//					GameManager.instance.scaleformCamera.hud.TurnOffWard ();
				} else if (wardCount > 0) {
					_characterController.OnWardHit (destroyProjectiles);
					wardCount--;
					GameManager.instance.scaleformCamera.hud.TurnOffWard ();
				} else {
					if (!_aiController._aiModel.name.Equals (AIModel.NameTypes.PRIMUS_NEX.ToString ()))
						GameManager.instance.motionMaker.Shake (GameObject.FindGameObjectWithTag ("MainCamera"), 0.15f);
					
					gradeDamageTaken = 0;
					
//					if(GameManager.PRINT_LOGS) Debug.Log("_battleState._enemy.skullLevel" + _battleState._enemy.skullLevel + " projectileStats.Strength >> "+projectileStats.Strength);
					
					_characterController.OnHit (destroyProjectiles, projectileStats._projectileType);
					
					int playerCurrentHP = _battleState._user.life;
					if ((playerCurrentHP - (ENEMY_DAMAGE_AMPLIFICATION * projectileStats.Strength)) <= 0.99) {
						playerCurrentHP = 0; 
						_battleState._user.life = playerCurrentHP;
						
						OnPlayerDied ();

//						GameManager.instance._levelManager.player.GetComponent<SRCharacterController> ().StopBreathingSound ();

						return;
					} else {
						if (allowPlayerDamage)
							_battleState._user.life = Mathf.CeilToInt (_battleState._user.life - (ENEMY_DAMAGE_AMPLIFICATION * projectileStats.Strength)); 
						healthLost += Mathf.CeilToInt (ENEMY_DAMAGE_AMPLIFICATION * projectileStats.Strength);
						
//						if (_battleState._user.life < (_battleState._user.totalLife * 0.3f)) {
//							GameManager.instance._levelManager.player.GetComponent<SRCharacterController> ().PlayBreathingSound ();
//							SoundManager.instance.PlayHealthLowSound(true);
//						} else {
//							SoundManager.instance.PlayHealthLowSound(false);
//						}
					}
				}
			} else if (anObject.tag.Equals (_aiGameObject.tag)) {
				#region PET_ELEMENT_ABILITY
				// Updating the Element Specific Stats with the addition of the Pet Passive Element Abilities.
				//Debug.LogError("_battleState._enemy.life Before: "+_battleState._enemy.life );
				string name = projectileStats._projectileType.ToString ().ToUpper ();
				name = name.StartsWith ("K") ? name.Substring (1) : name;

				//Debug.LogError("Spell Name: "+name+", Element Name: "+_petController._petModel.abilityElement);
//                Debug.Log("Previous Strength: " + projectileStats.Strength);

				if (_petController != null && _petController._petModel.abilityElement == name)
					projectileStats.Strength *= ((_petController._petModel.buffTime / 100f) + 1f);
				
//                Debug.Log("New Strength: " + projectileStats.Strength);
				#endregion PET_ELEMENT_ABILITY


//				Debug.LogError("aiGameObject");
				int damageVal = 0;
				damageVal = Mathf.FloorToInt (projectileStats.Strength);
				damageVal = (damageVal >= _battleState._enemy.life) ? _battleState._enemy.life : damageVal;
				
				if (enemyState == EnemyState.kPostReleaseFocus || enemyState == EnemyState.kBusy) 
					enemyState = EnemyState.kHit;

				if (_attackMode == AttackMode.kRapid) {
//					showCastCue = true;
//					Debug.LogError("attackmode - "+showCastCue);
					burstModeDamage += damageVal;
					if (allowEnemyDamage)
						_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType, false, null, 0);
					
					if (damageVal >= _battleState._enemy.life)
						_battleState._enemy.life = 0;
					else
						_battleState._enemy.life = Mathf.FloorToInt (_battleState._enemy.life - damageVal); 
					return;
				}
				if (projectileStats.isCharged) {
					specialAttack = false;
					StaffMode (false);
					Invoke ("EnableSpirit", 2f);
				}
				
				int enemyCurrentHP = _battleState._enemy.life;
				if (enemyCurrentHP <= 0) {
					_battleState._enemy.life = 0;
					OnEnemyDied ();
					return;
				}
				if (damageVal < enemyCurrentHP) {
					if (projectileStats.isCharged) {
						Buff buff = _characterController.GetBuffForChargedSpell (projectileStats._projectileType);
						if (buff != null) {
							if (buff.buffType == Buff.BuffType.kPositive)
								_battleState._user.ApplyBuff (buff, this);
							else {
								_battleState._enemy.ApplyBuff (buff, this);
								
								if (buff.id.Equals ("DRAIN")) {
									_battleState._user.ApplyBuff (new Buff ("AMPLIFY", (int)(Mathf.Floor (1 + buff.skullLevel / 2))), this, true, false);
									ShowAmplifyEffect (true);
								}
							}	
						}
					}
				}
				
				int healPercent = _battleState._enemy.GetHealIfLeechApplied ();

				if (_battleState._enemy.HasApplied ("DAZE")) {
					if (projectileStats.isCharged) { 
						if (_battleState._enemy.ProcDaze ())
							_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType, true, () => 
							{
								GameManager.instance.scaleformCamera.hud.ChainStun (true);
							});
						else
							_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType);
					} else
						_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType);
				} else {
					if (_enemyStunState == EnemyStunState.kWillStun || _enemyStunState == EnemyStunState.kStunStarted)
						_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType, true, null, stunTimer);
					else
						_aiController.OnHit (damageVal, destroyProjectiles, projectileStats._projectileType);
				}
				
				int healAmount = Mathf.CeilToInt ((damageVal * healPercent) / 100.0f);
				_battleState._user.life += healAmount;
				
				maxDamage = (maxDamage < damageVal) ? damageVal : maxDamage;
				if (damageVal >= enemyCurrentHP) {
					enemyCurrentHP = 0;
					_battleState._enemy.life = enemyCurrentHP;
					OnEnemyDied ();

					return;
				} else {
					if (allowEnemyDamage)
						_battleState._enemy.life = Mathf.FloorToInt (_battleState._enemy.life - damageVal);				
				}
//                Debug.Log("_battleState._enemy.life After: " + _battleState._enemy.life);
			}
		}
	
		_battleProgressState = BattleProgressState.kBothFocus;
		if (!TutorialManager.instance.isGamePause)
			Time.timeScale = 1.0f;
	}
	#endregion
	
	public void EnableSpirit ()
	{
		if (spiritObject != null)
			_petController.EnableSpirit (true);
	}
	
	public void DisableSpirit ()
	{
		if (spiritObject != null)
			_petController.EnableSpirit (true);
	}
	
	public void GettingDestroyed (GameObject projectile)
	{
		if (_characterController.livingProjectiles.Contains (projectile))
			_characterController.livingProjectiles.Remove (projectile);
	}
	
	void OnEnable ()
	{
		_gestureObject.SetActive (true);
		_battleState = new BattleState ();

		_characterController = playerGameObject.GetComponent<SRCharacterController> ();
		
		_gestureEmitter = _gestureObject.GetComponent<GestureEmitter> ();
		_gestureEmitter.SetDelegate (this.OnGestureReceived);
		_gestureEmitter.SetFingerDownDelegate (this.OnFingerEvent);
		_gestureEmitter.enabled = false;
		
		_characterController.ResetStates ();
		_numberOfTurns = 0;
		
		enemyState = EnemyState.kReady;
			
		UIButtonMessage messageScript = null;
		UIButtonMessage[] messageList = _characterController.GetComponents<UIButtonMessage> ();
		int count = messageList.Length;
		for (int i = 0; i<count; i++) {
			messageScript = messageList [i];
			messageScript.target = this.gameObject;
		}
		OnAiLoaded (_aiGameObject);
		UIManager.instance.hud.SetDisplayVisible(true);
		UIManager.instance.hud.StartBattle();
		UIManager.instance.hud.ShowPlayerElementStats(GameManager._gameState.User._hasFire, GameManager._gameState.User._hasWater, GameManager._gameState.User._hasLightning, GameManager._gameState.User._hasEarth);

	}
	
	void OnDisable ()
	{
		//TODO
		if(_gestureEmitter != null) {
			_gestureEmitter.enabled = false;
		}
		if (_gestureObject != null)
			_gestureObject.SetActive (false);
	}
		
	public void InitiateRound (bool startImmediately = true)
	{
		if (startImmediately) 
			InitiateBattle (0.8f);
		else
			InitiateBattle (0.8f);
	}
	
	#region BAD BAD CODEE!!!!
	
	
	#region Death handling
	protected virtual void OnPlayerDied ()
	{
		currentState = State.kFocus;
		//_battleState._enemy.life = enemyInitialHealth;
		
		_characterController.OnDeath ();
		nameToBattleManagerDictionary [_aiController._aiModel.name].OnBattleEnded (true);
		GameManager.instance.scaleformCamera.hud.SetLoadingFill(false);
		showCastCue = false;
		GameManager.instance.scaleformCamera.hud.ChainStun (false);

		GameManager.instance._levelManager.isEnemyClicked = false;

	}
	
	protected virtual void OnEnemyDied ()
	{
		//currentState = State.kIdle;
		_characterController.SetIdle ();
		
		
		GameManager._gameState.firstEnemyDefeated = true;
		
		Debug.LogError("Setting Battle State user to Gamestate user");
		GameManager._gameState.SetUser (this._battleState._user);

		_aiController.OnDeath ();
		
		nameToBattleManagerDictionary [_aiController._aiModel.name].OnBattleEnded (false);
		GameManager.instance.scaleformCamera.hud.SetLoadingFill(false);
//		Debug.LogError("OnEmeyDied - BM");
		showCastCue = false;
		GameManager.instance.scaleformCamera.hud.ChainStun (false);

		GameManager.instance._levelManager.isEnemyClicked = false;

	}
	
	private void ResetBattleState ()
	{
		currentState = State.kNone;
		stunEnded = false;
		playerWon = false;
		battleEnded = false;
		destroyProjectiles = false;
		totalDamageDealt = 0;
		enemyState = EnemyState.kReady;
		_numberOfTurns = 0;

		//TODO
		specialAttack = false;
		Time.timeScale = 1.0f;
	}
	
	
	
	private int totalDamageDealt;
	private bool playerWon;
	#endregion
	public void OnBattleEnded (bool restartFlag, bool isBossFight=false)
	{
		OnFingerUp ();
		totalDamageDealt = enemyInitialHealth - _battleState._enemy.life;
		_battleProgressState = BattleProgressState.kNone;
		SoundManager.instance.playDefaultBackGround ();
		GameManager.instance.scaleformCamera.generalSwf.Init ();
		ShowHoldGesture (false);
		battleEnded = true;
		_runeAttackStatus = RuneAttackStatus.kReady;
		//burst mode disable...
		_attackMode = AttackMode.kNormal;
		_characterController.DisableBurst ();
		specialAttack = false;
		Time.timeScale = 1.0f;
		_gestureEmitter.enabled = false;
		//

		Unshield ();
		if (_characterController.hoverClone != null)
			Destroy (_characterController.hoverClone);
		_battleState._user.ClearBuffs (this);
		_battleState._enemy.ClearBuffs (this);
		if (spiritObject != null) {
			DisableSpirit ();
			GameObject.Destroy (spiritObject);
		}

		currentState = State.kNone;

		if (restartFlag) {
			if (GameManager.enableAnalytics)
				Analytics.logEvent (Analytics.CurrencyTrackType.TrackGameOver, ++GameManager._gameState.combatsLost);
			
			playerWon = false;
			_aiController.SetIdleState ();
			
			UnityEngine.Debug.Log ("generalSwf.ToggleTopStats > F");
			GameManager.instance.scaleformCamera.generalSwf.ToggleTopStats (false);
			if (GameManager.instance._levelManager.currentLevel.Equals ("DemonFalls") || GameManager.instance._levelManager.currentLevel.Equals ("ThreeGods")) {
				StartCoroutine (FadeOutAndLoadBossDefeatScreen (1.5f));
			} else {
				StartCoroutine (FadeOutAndLoadDefeatScreen (1.5f));
				//GameManager.instance.Invoke("ReturnToCamp",1.5f);
			}
			
		} else {
			playerWon = true;
			endTime = Time.time;
			GameManager.instance.scaleformCamera.generalSwf.DisplayCenterButton (false);
			GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);

			if (isBossFight) {
				GameManager.instance._levelManager.Invoke ("PlayOutroSequence", 1.0f);
			} else {
				Invoke ("PlayVictorySound", 1.5f);
				StartCoroutine (WaitForEndAnimationAndReturn (3.0f, isBossFight));
				
			}
		}
	}

	public void PlayVictorySound ()
	{
		SoundManager.instance.PlayMenuVictorySound ();
	}
	
	IEnumerator FadeOutAndLoadBossDefeatScreen (float initialDelay)
	{
		Debug.LogWarning("FadeOut 2 BattleManager");
		yield return new WaitForSeconds (initialDelay); //Initial delay
		
		while (!IsTouchReleased) {
			yield return new WaitForSeconds (0.2f);
		}
		
		_gestureEmitter.enabled = false;
		Invoke ("LoadBossDefeatScreen", 1.0f);
	}
	 
	protected void LoadBossDefeatScreen ()
	{
		UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
		_gestureEmitter.enabled = false;

		GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);
		GameManager.instance.scaleformCamera.generalSwf.ShowBossDefeatPopup ("Defeated!!", "You must become stronger to obtain the Sorcerer's Ring");
		
		if (!GameManager.instance.isMultiPlayerMode)
			GameManager._gameState.User.life = lifeAtBattleStart;
	}
	
	public IEnumerator WaitForEndAnimationAndReturn (float initialDelay, bool isBossFight)
	{
		yield return new WaitForSeconds (initialDelay);

		if (GameManager.instance._levelManager.currentLevel != "DemonFalls") {
			IEnumerator playerFocusAnim = AnimateCameraToFocusPlayer ();
			while (playerFocusAnim.MoveNext()) {
				float frameDelay = (float)playerFocusAnim.Current;
//				Debug.Log("frameDelay > " + frameDelay + " TimeDelta: " + Time.time);
				if (frameDelay > 0f)
					yield return new WaitForSeconds (frameDelay);
				else
					break;
			}
		}

		while (!IsTouchReleased) {
			yield return new WaitForSeconds (0.2f);
		}
		
		_gestureEmitter.enabled = false;
		UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
		GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("BATTLE WON ::::::::: HEALTH LOST --->>> " + healthLost + "::::::::::::::: DURATION --->>>" + (int)(endTime - startTime) + ":::::::::MAX DMG --->>>" + maxDamage);
		int timeTakenForBattle = (int)(endTime - startTime);
		if (timeTakenForBattle < 30)
			gradeTime = 1;
		if (maxDamage > _aiController._aiModel.life / 4)   
			gradeMaxDamageDone = 1;
		string finalGrade = GetGradeForPoints (gradeTime + gradeMaxDamageDone + gradeDamageTaken);
		
		// TODO Check why delay was added
		if (GameManager.instance.isMultiPlayerMode)
			StartCoroutine (PostResultToServer (0.0f));
		else
			GameManager.instance.scaleformCamera.LoadVictoryScreen (healthLost, timeTakenForBattle, maxDamage, finalGrade, isBossFight);
		//ResetBattleState();

	}

	private IEnumerator AnimateCameraToFocusPlayer ()
	{

		Transform targetTransform = playerGameObject.transform.FindChild ("CameraPoint");
		Transform lookAtPoint = playerGameObject.transform.FindChild ("LookAtPoint");
		Transform mainCameraTransform = Camera.main.transform;
		
		Quaternion initialRotation = mainCameraTransform.localRotation;
		Vector3 initialPosition = mainCameraTransform.localPosition;
		
		float moveSpeed = 0.01666667f;
		
		while (Vector3.Distance(mainCameraTransform.position, targetTransform.position) > (moveSpeed * 8)) {
			if (Input.GetMouseButtonDown (0) || Input.touchCount > 0) {
				mainCameraTransform.position = targetTransform.position;
				mainCameraTransform.LookAt (lookAtPoint);
				yield return 0f;
			}

			mainCameraTransform.LookAt (lookAtPoint);
			mainCameraTransform.position = Vector3.MoveTowards (mainCameraTransform.position, targetTransform.position, moveSpeed * 8);
			yield return moveSpeed;
		}
		
		yield return 0f;
	}
	
	#endregion
	
	IEnumerator PostResultToServer (float delay)
	{
		Debug.LogError ("PostResultToServer-_battleState._enemy.modelName: " + _battleState._enemy.modelName + "   Skull: " + _battleState._enemy.skullLevel + "   _battleState._enemy.name: " + _battleState._enemy.name);
		Debug.LogError("And why was the delay added here - "+delay);
		yield return new WaitForSeconds (delay);
		RaidsManager.Instance.GetBossBattleResult (playerWon, totalDamageDealt, _battleState._enemy.modelName, _battleState._enemy.skullLevel, this.OnResultPostedToServer);
		
//		int raiders = 0;
//		long _endTimeTS = 0;
//		int bossCurrentHealth = 0;
//		long bossLifeLost = 0;
//		
//		RaidAIModel raidModel = _battleState._enemy as  RaidAIModel;
//		if (raidModel != null) {
//			raiders = raidModel.numberOfRaiders;
//			_endTimeTS = (long)raidModel._endTimeTS;
////			Debug.LogError("End time = "+raidModel._endTimeTS+" start time = "+raidModel._startTimeTS+" and difference = "+ (raidModel._endTimeTS - raidModel._startTimeTS));
//		}	
//		raidModel.totalLife = (int)raidModelUnchangedTotalLife;
//		if (GameManager.PRINT_LOGS)
//			Debug.LogError (raidModel.ToString () + "------- raidModelUnchangedTotalLife = " + raidModelUnchangedTotalLife + " ------raidModel.totalLife" + raidModel.totalLife);
//		if (playerWon) {
//			if (GameManager.PRINT_LOGS)
//				Debug.LogError (raidModel.ToString () + "------- raidModelUnchangedTotalLife = " + raidModelUnchangedTotalLife + " ------raidModel.totalLife" + raidModel.totalLife);
//			bossLifeLost = enemyInitialHealth;
//			bossCurrentHealth = Mathf.FloorToInt ((raidModel.totalLife - bossLifeLost) / raidModel.maxLife * 100);
//			GameManager._gameState.User.multiplayerLife = _battleState._user.life;
//
//			GameManager.instance.scaleformCamera.generalSwf.ShowRaidsVictoryPopup (RaidPortalNavigator.currentPosInLeaderboard, (int)GameManager._gameState.User.arcanePoints, maxDamage, bossCurrentHealth, raiders, _endTimeTS);
//		} else {
//			bossLifeLost = enemyInitialHealth - _battleState._enemy.life;
//			GameManager._gameState.User.multiplayerLife = 0;
//			bossCurrentHealth = Mathf.FloorToInt ((raidModel.totalLife - bossLifeLost) / raidModel.maxLife * 100);
//			
//			GameManager.instance.scaleformCamera.generalSwf.ShowRaidsDefeatPopup (RaidPortalNavigator.currentPosInLeaderboard, (int)GameManager._gameState.User.arcanePoints, maxDamage, bossCurrentHealth, raiders, _endTimeTS);
//		}
//		
//		GameManager.instance.allowHealOverTime = true;
//		_battleState._enemy.life = enemyInitialHealth;
//		
//		currentState = State.kNone;
//			
//		Time.timeScale = 0;
//		GameManager.instance.scaleformCamera.isPaused = true;
	}
	
	protected void SummonPet ()
	{
		if (spiritPosGameObject == null) {
			return;
		}
		spiritObject = _petLoader.GetSpiritObjectForId (_battleState._user.spiritId);
		if (spiritObject != null) {
			spiritObject.transform.position = new Vector3 (spiritPosGameObject.transform.position.x, spiritPosGameObject.transform.position.y - 2f, spiritPosGameObject.transform.position.z);
			spiritObject.transform.rotation = playerGameObject.transform.rotation;
			TweenPosition.Begin (spiritObject, 2f, new Vector3 (spiritPosGameObject.transform.position.x, spiritPosGameObject.transform.position.y, spiritPosGameObject.transform.position.z));
			_petController = spiritObject.GetComponent<PetController> ();
			spiritObject.SetActive (true);
			UnprisonPet ();
		}
	}
	
	
	public void PrisonPet () //enemy only buff
	{
		Debug.Log ("SPIRIT ENABLE > F");
		GameManager.instance.scaleformCamera.hud.SpiritEnable (false);
	}
	
	public void UnprisonPet ()
	{
		Debug.Log ("SPIRIT ENABLE > T");
		GameManager.instance.scaleformCamera.hud.SpiritEnable (true);
	}
	
	public void OnEnemyVisibleInCamera ()
	{
		if (gameObject.activeInHierarchy) {
			if (nameToBattleManagerDictionary == null || nameToBattleManagerDictionary.Keys.Count <= 0)
				InstantiateAIModelToBattleManagerDictionary ();
			
			nameToBattleManagerDictionary [_aiController._aiModel.name].OnEnemyRendererVisible ();
		}
	}
	
	public void OnEnemyInvisibleInCamera ()
	{
		if (gameObject.activeInHierarchy) {
			if (nameToBattleManagerDictionary == null || nameToBattleManagerDictionary.Keys.Count <= 0)
				InstantiateAIModelToBattleManagerDictionary ();
			nameToBattleManagerDictionary [_aiController._aiModel.name].OnEnemyRendererInvisible ();
		}
	}
	
		
	public void OnEnemyRendererVisible ()
	{
		
	}
	
	public void OnEnemyRendererInvisible ()
	{
		
	}
	
	public string GetGradeForPoints (int points)
	{
		string finalGrade = "D";
		
		switch (points) {
		case 3:
			finalGrade = "A";
			break;
		case 2:
			finalGrade = "B";
			break;
		case 1:
			finalGrade = "C";
			break;
		}
		
		return finalGrade;
	}
	
	public void OnSpiritPassive ()
	{
		if (_petController._petModel.abilityStat == null)
			return;
		Buff buff = new Buff (_petController._petModel.abilityStat, _petController._petModel.skullLevel);
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("buff.buffType" + buff.buffType);
		
		if (buff.buffType == Buff.BuffType.kPositive) {
			if (buff.id.Equals ("AMPLIFY")) {
				_battleState._user.ApplyBuff (buff, this, false, false);
				ShowAmplifyEffect (true);
			} else {
				_battleState._user.ApplyBuff (buff, this, false);
				if (buff.id.ToLower ().Equals ("regen"))
					ShowRegenEffect (true);
			}
		} else
			_battleState._enemy.ApplyBuff (buff, this, false);
	}
	
	public void OnSpiritThrow ()
	{
		if (!TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.spiritFightTutorialStart))
			TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.spiritFightTutorialEnd);

		GameManager.instance.scaleformCamera.hud.BattleSpirit (0);

		Buff buff = new Buff (_petController._petModel.activeSpell, _petController._petModel.skullLevel);
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("buff.buffType" + buff.buffType);
		
		if (buff.buffType == Buff.BuffType.kPositive) {
			if (buff.id.Equals ("AMPLIFY")) {
				_battleState._user.ApplyBuff (buff, this, false, false);
				ShowAmplifyEffect (true);
			} else
				_battleState._user.ApplyBuff (buff, this, false, false);
		} else
			_battleState._enemy.ApplyBuff (buff, this, false, false);
	}
	
	protected void LoadDefeatScreen ()
	{
		if (GameManager.instance.isMultiPlayerMode)
			StartCoroutine (PostResultToServer (0.0f));
		else {
			UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
			GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);
			GameManager.instance.scaleformCamera.LoadDefeatScreen ();
			
			GameManager._gameState.User.life = lifeAtBattleStart;
			GameManager.instance.allowHealOverTime = true;
			_battleState._enemy.life = enemyInitialHealth;
		}
	}
	
//	private void OnResultPostedToServer (bool yesNo)
	private void OnResultPostedToServer (object responseParameters, object error, ServerRequest request)
	{
		if (GameManager.PRINT_LOGS)
			Debug.LogError("OnResultPostedToServer  ::::::::: " + MiniJSON.Json.Serialize(responseParameters as IDictionary));

		int arcane = 0;
		int deltaArcane = 0;
		bool isResponse = false;
		if (error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			if (MiniJSON.Json.Serialize(responseParameters as IDictionary) == null || (response == null))
			{
			} else
			{
				isResponse = true;
				arcane = System.Int32.Parse(response["arcane"].ToString());
				deltaArcane = System.Int32.Parse(response["deltaArcane"].ToString());
				int rank = System.Int32.Parse(response["rank"].ToString());

					//				int arcane = response.Contains("arcane") ? response ["arcane"] : null;
//				int rank = response.Contains("rank") ? response ["rank"] : null;

				GameManager._gameState.User.arcanePoints = arcane;
				RaidPortalNavigator.currentPosInLeaderboard = rank;
//				Debug.LogError("arcnae = "+arcane + " rank = "+ rank+" deltaArcane - "+deltaArcane);
			}
		}
		int raiders = 0;
		long _endTimeTS = 0;
		int bossCurrentHealth = 0;
		long bossLifeLost = 0;
		
		RaidAIModel raidModel = _battleState._enemy as  RaidAIModel;
		if (raidModel != null) {
			raiders = raidModel.numberOfRaiders;
			_endTimeTS = (long)raidModel._endTimeTS;
			//			Debug.LogError("End time = "+raidModel._endTimeTS+" start time = "+raidModel._startTimeTS+" and difference = "+ (raidModel._endTimeTS - raidModel._startTimeTS));
		}	
		raidModel.totalLife = (int)raidModelUnchangedTotalLife;
		if (GameManager.PRINT_LOGS)
			Debug.LogError (raidModel.ToString () + "------- raidModelUnchangedTotalLife = " + raidModelUnchangedTotalLife + " ------raidModel.totalLife" + raidModel.totalLife);
		if (playerWon) {
			if (GameManager.PRINT_LOGS)
				Debug.LogError (raidModel.ToString () + "------- raidModelUnchangedTotalLife = " + raidModelUnchangedTotalLife + " ------raidModel.totalLife" + raidModel.totalLife);
			bossLifeLost = enemyInitialHealth;
			bossCurrentHealth = Mathf.FloorToInt ((raidModel.totalLife - bossLifeLost) / raidModel.maxLife * 100);
			GameManager._gameState.User.multiplayerLife = _battleState._user.life;
			
			GameManager.instance.scaleformCamera.generalSwf.ShowRaidsVictoryPopup (RaidPortalNavigator.currentPosInLeaderboard, arcane, deltaArcane, maxDamage, bossCurrentHealth, raiders, _endTimeTS);
		} else {
			bossLifeLost = enemyInitialHealth - _battleState._enemy.life;
			GameManager._gameState.User.multiplayerLife = 0;
			bossCurrentHealth = Mathf.FloorToInt ((raidModel.totalLife - bossLifeLost) / raidModel.maxLife * 100);
			
			GameManager.instance.scaleformCamera.generalSwf.ShowRaidsDefeatPopup (RaidPortalNavigator.currentPosInLeaderboard, arcane, deltaArcane, maxDamage, bossCurrentHealth, raiders, _endTimeTS);
		}
		if(isResponse) {
			GameManager._gameState.User.arcanePoints = arcane;
		}
		GameManager.instance.allowHealOverTime = true;
		_battleState._enemy.life = enemyInitialHealth;
		
		currentState = State.kNone;
		
		Time.timeScale = 0;
		GameManager.instance.scaleformCamera.isPaused = true;

		//		if(GameManager.PRINT_LOGS) Debug.Log("OnResultPostedToServer = " + yesNo);
		
		//GameManager.instance._levelManager.battleManager.scaleformBattleEnded=true;
		//Time.timeScale=1.0f;
		//GameManager.instance.scaleformCamera.isPaused=true;
		
		//GameManager.instance._levelManager.ReturnToCamp();
	}

	public void OnCollisionAfterStunEnd ()
	{
	}
	
	protected	IEnumerator FadeOutAndLoadDefeatScreen (float initialDelay)
	{
		Debug.LogWarning("FadeOut 1 BattleManager");
		yield return new WaitForSeconds (initialDelay); //Initial delay
		
		while (!IsTouchReleased) {
			yield return new WaitForSeconds (0.2f);
		}
		_gestureEmitter.enabled = false;
		
		Invoke ("LoadDefeatScreen", 1.0f);
	}
	
	public void PerformAIFocusStateReached ()
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("enemyState  --- " + enemyState);
		if (enemyState == EnemyState.kBusy)
			enemyState = EnemyState.kPostReleaseFocus;
		else if (enemyState == EnemyState.kHit)
			enemyState = EnemyState.kReady;
	}
	
	public void SpellLock () //enemy only buff
	{
		GestureEmitter.Gesture [] availableGestures = _battleState._user.availableGestures;
		lockedGesture = availableGestures [new System.Random ().Next (0, availableGestures.Length)];
	}
	
	public void SpellUnlock ()
	{
		lockedGesture = GestureEmitter.Gesture.kInvalid;
	}
	
	public void OnFocusStateWhileEnemyStun ()
	{
	}
	
	public void DisableLookAt (SmoothLookAt lookAt, float delay)
	{
		StartCoroutine (StopLookAt (lookAt, delay));
	}
	
	private IEnumerator StopLookAt (SmoothLookAt lookAt, float delay)
	{
		yield return new WaitForSeconds (delay);
		lookAt.enabled = false;
	}
	
	#region not required for bug fixing
	
	public void setPlayer (GameObject player)
	{
		this.playerGameObject = player;
	}
	
	public void setEnemy (GameObject enemy)
	{
		this._aiGameObject = enemy;
	}
	#endregion
	
	public bool IsBattleEnded {
		get	{ return battleEnded;	}
	}
	
	#region Buff Manager callbacks
	public void Dispel ()
	{
		// Removing +ve Buffs from Enemy.
		List<Buff> toRemoveBuffList = _battleState._enemy._buffList.Where (buff => buff.buffType == Buff.BuffType.kPositive).ToList ();
		string buffNames = "";
		toRemoveBuffList.ForEach (buff => buffNames += buff.id + ", ");
		Debug.Log ("Going to Remove Enemy +ve Buffs: " + buffNames);
		_battleState._enemy.RemoveBuffs (_battleState._enemy._buffList, toRemoveBuffList, this);

		// Removing -ve Buffs and the Dispel from the User. The reason to remove Dispel is that it have done the work.
		toRemoveBuffList = _battleState._user._buffList.Where (buff => buff.buffType == Buff.BuffType.kNegative || buff.id == "DISPEL").ToList ();
		buffNames = "";
		toRemoveBuffList.ForEach (buff => buffNames += buff.id + ", ");
		Debug.Log ("Going to Remove User -ve Buffs: " + buffNames);
		_battleState._user.RemoveBuffs (_battleState._user._buffList, toRemoveBuffList, this);

		//_battleState._user.availableBuffs.Remove("DISPEL");
	}

	public void Shield ()
	{
		if (shieldObject != null)
			shieldClone = Instantiate (shieldObject, _aiController.gameObject.transform.position, _aiController.gameObject.transform.rotation) as GameObject;

		enemyLongCast = true;
		
		//Check if shield tutorial has run already...
		isShieldTut = false;
		isShieldTut = !TutorialManager.instance.IsTutorialCompleted (TutorialManager.TutorialsAndCallback.SpellShield);
		if (isShieldTut)
			TutorialManager.instance.ShowTutorial (TutorialManager.TutorialsAndCallback.SpellShield);
	}
	
	public void Unshield ()
	{
		if (shieldClone != null)
			Destroy (shieldClone);
		
		enemyLongCast = false;
	}
	
	protected Buff ApplyBuffIfApplicable ()
	{
		buffCount--;
//        if (GameManager.PRINT_LOGS) Debug.Log("_battleState._enemy.availableBuffs.Count ================== " + _battleState._enemy.availableBuffs.Count);
		if (buffCount <= 0 && _battleState._enemy.availableBuffs.Count > 0) {
			Buff buff = _battleState._enemy.GetUnusedBuff ();
			
			if (buff == null)
				buff = new Buff (_battleState._enemy.availableBuffs [new System.Random ().Next (0, _battleState._enemy.availableBuffs.Count - 1)], GameManager._gameState.skullLevel);
			
			buffCount = _battleState._enemy.buffTime;
//            if (GameManager.PRINT_LOGS) Debug.Log("ApplyBuffIfApplicable ::::::::: " + buff.id);
			return buff;
		}
//        if (GameManager.PRINT_LOGS) Debug.Log("ApplyBuffIfApplicable ::::::::: NULL");
		return null;
	}
	#endregion
	public void SetEnemyReady ()
	{
		enemyState = EnemyState.kReady;
	}
}
