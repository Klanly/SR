using UnityEngine;
using System.Collections;
using System.Collections.Generic;
		
public class AICharacterControllerNEWAgain : SRBaseCharacterController
{
	
    public static Dictionary<GestureEmitter.Gesture, TutorialManager.TutorialsAndCallback> gestureTutorialDictionary = PopulateGestureTutorialDictionary();
		
    private static Dictionary<GestureEmitter.Gesture, TutorialManager.TutorialsAndCallback> PopulateGestureTutorialDictionary()
    {
        Dictionary<GestureEmitter.Gesture, TutorialManager.TutorialsAndCallback> aDictionary = new Dictionary<GestureEmitter.Gesture, TutorialManager.TutorialsAndCallback>();
        aDictionary.Add(GestureEmitter.Gesture.kEarth, TutorialManager.TutorialsAndCallback.FireTutorialStart);
        aDictionary.Add(GestureEmitter.Gesture.kFire, TutorialManager.TutorialsAndCallback.WaterTutorialStart);
        aDictionary.Add(GestureEmitter.Gesture.kLightning, TutorialManager.TutorialsAndCallback.EarthTutorialStart);
        aDictionary.Add(GestureEmitter.Gesture.kWater, TutorialManager.TutorialsAndCallback.LightningTutorialStart);
        aDictionary.Add(GestureEmitter.Gesture.kInvalid, TutorialManager.TutorialsAndCallback.Idle); // Have to remove this piece of crap later! :@
        return aDictionary;
    }
	
    private const string TAG = "*** AICharacterController ...";
	
    private const int ENEMY_LONG_CAST_MULTIPLIER = 2;
	
    ProjectileParticleResolver particleResolver;
		
    public GameObject projectileGameObject = null;
	
    private const float ATTACK_DELAY = 1.5f;
	
    public delegate void StunStateDelegate();
	
    private float ENEMY_HEIGHT;
	
    public delegate void StunStateEndDelegate();
	
    public StunStateDelegate _stundelegate;
	
    public StunStateEndDelegate _stunEndDelegate;
	
    public delegate void AiWillStunDelegate();
	
    public AiWillStunDelegate _aiWillStunDelegate;
		
    public delegate void OnFocusStateListener();
	
    private bool isShieldTut;
	
    private static System.Collections.Generic.Dictionary<AIModel.Speed, float> speedValDictionary;
	
    //CharacterSoundController stuff
    public CharacterSoundController AISoundController;
    private string spellBeingCasted;
	
    static AICharacterControllerNEWAgain()
    {
        speedValDictionary = new Dictionary<AIModel.Speed, float>();
        speedValDictionary.Add(AIModel.Speed.SLOW, DatabankSystem.Databank.ATTACK_SPEED_SLOW/*1.25f*/);
        speedValDictionary.Add(AIModel.Speed.MEDIUM, DatabankSystem.Databank.ATTACK_SPEED_MEDIUM);
        speedValDictionary.Add(AIModel.Speed.FAST, DatabankSystem.Databank.ATTACK_SPEED_FAST);
        speedValDictionary.Add(AIModel.Speed.HASTE, DatabankSystem.Databank.ATTACK_SPEED_HASTE);
    }
	
    public OnFocusStateListener _onFocusStateListener
    {
        set;
        get;
    }
	
    public void OnSetStart()
    {
    }
	
    public delegate void OnLongCastListener();
	
    public bool discardPreviousCallbacks
    {
        get;
        set;
    }
	
    public AIModel _aiModel
	{ get; set; }
	
	#region FocusStateListenerInterface methods
	
    void Start()
    {
        ENEMY_HEIGHT = gameObject.GetComponent<BoxCollider>().size.y;
    }

	
	#endregion
	
    public GameObject enemyHitTransform;
	
    public Transform projectileHitTransform;
	
    public Transform singleCastProjectileSpawnTransform;
	
    public Transform longCastProjectileSpawnTransform;
	
    private SRCharacterStateControllerNEW _aiCharStateController;

    private enum CastType
    {
        kShort,
        kLong}
    ;
	
    private CastType _castType;
	
    private bool _endRound = false;
	
    private int lastAttackNumber = 0;
	
	#region Hit/Damage handling
	
    private int aiHitsToStun;
	
    private int aiLifeToStun;
	
    private int totalHitsLanded = 0;
	
    private float lifeLost = 0;
	
    private int[] attacksHit;
	
    public static float STUN_DURATION = 3.5f;
	
    public void OnStunStateReached()
    {
        if (_stundelegate != null)
        {
            _stundelegate();
        }
    }
	
    public void SetIdleState()
    {
        _aiCharStateController.SetIdleState();
    }
	
    public void SetFocus()
    {
        _aiCharStateController.SetFocusState();
    }
	
    public void OnDeath()
    {
        _aiCharStateController.SetDeathState();
		
        gameObject.collider.enabled = false;
		
        AISoundController.PlayDeathSound();
    }
	
    public void OnHit(float strengthOfHit, bool destroyProj, GestureEmitter.Gesture aGesture, bool definiteStun = false, System.Action definiteStunCB = null, float dur = -1)
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("DURATION - " + dur);
        bool shouldStun = (definiteStun) ? true : false;
		
        if (definiteStun && definiteStunCB != null)
            definiteStunCB();
		
        if (destroyProj)
            StartCoroutine(DestroyProjectile());
        else
			if (GameManager.PRINT_LOGS)
            Debug.Log("Dont destroy ENEMY projectile!!");
		
        if (dur < 0)
        {
            SaveTurnOfHit();
		
            totalHitsLanded++;
		
            lifeLost += strengthOfHit;
        }
        //hit sounds start
        string spellHit = "";
        switch (aGesture)
        {
            case GestureEmitter.Gesture.kEarth:
                spellHit = "Earth";
                break;
            case GestureEmitter.Gesture.kFire:
                spellHit = "Fire";
                break;
            case GestureEmitter.Gesture.kWater:
                spellHit = "Water";
                break;
            case GestureEmitter.Gesture.kLightning:
                spellHit = "Lightning";
                break;
        }

        AISoundController.Invoke("Play" + spellHit + "ImpactSound", 0.0f);
		
        AISoundController.PlayDamageSound();
        // hit sounds end
		
		
//		if(GameManager.PRINT_LOGS) Debug.Log("strengthOfHit - " + strengthOfHit + " - lifeLost - " + lifeLost + " - aiLifeToStun - " + aiLifeToStun + " - shouldStun - " + shouldStun);
        if (lifeLost >= aiLifeToStun && !shouldStun)
        {
            if (totalHitsLanded >= aiHitsToStun)
            {
                if (GotConsecutiveHit())
                {
                    shouldStun = true;
					
                    lifeLost = 0;
                    totalHitsLanded = 0;
                    ClearConsecutiveHits();
                }
            }
        }
		
        if (shouldStun && dur < 0)
		//if(delLater %3 == 0)
        {
            if (_aiWillStunDelegate != null)
                _aiWillStunDelegate();
            StunEnemy();
        } else
        {	
            _aiCharStateController.SetDamagedState(() => {
				
                if (GameManager.instance._levelManager.battleManager.stunTimer > 0)
                {
                    _aiCharStateController.SetStunnedState(1000f, () => {}, () => {});
                    return;
                }
				
                _aiCharStateController.SetStateToFocus(() => {
                    if (_onFocusStateListener != null)
                        _onFocusStateListener();
					
                    //if(GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kReady && GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kStunEnded)
                    if (GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kBusy)
                        _endRound = true;
                });
            });
        }
    }
	

    private void StunEnemy()
    {
//		stunTimer = STUN_DURATION;
        _aiCharStateController.SetDamagedState(() => {
				
            _aiCharStateController.SetStunnedState(float.MaxValue, () => {

                if (_stundelegate != null)
                    _stundelegate();
				
            }, () => {
				
//				if(!discardPreviousCallbacks)
//				{
//					if(_stunEndDelegate != null && TutorialManager.instance.currentTutorial != TutorialManager.TutorialsAndCallback.SecondStunStart)
//						_stunEndDelegate();
//					
//					//if(GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kReady && GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kStunEnded)
//					if(GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kBusy)
//						_endRound = true;
//					_aiCharStateController.SetStateToFocus( () => { 
//						if(_onFocusStateListener != null)
//							_onFocusStateListener();	
//					});
//				}
//				_aiCharStateController.SetStateToFocus( () => {
//					if(_onFocusStateListener != null)
//						_onFocusStateListener();
//				});
//				discardPreviousCallbacks = false;
            });
        });
    }
	
    public void EndStun()
    {
        _aiCharStateController.SetStateToFocus(() => { 
            if (_onFocusStateListener != null)
                _onFocusStateListener();	
        });
    }
	
	
    private bool GotConsecutiveHit() //Compares 3 consecutive hits
    {
        if (attacksHit [0] - 1 == attacksHit [1])
        {
            if (attacksHit [1] - 1 == attacksHit [2])
            {
                return true;
            }
        }
        return false;
    }
	
    private void SaveTurnOfHit()
    {
        attacksHit [2] = attacksHit [1];
		
        attacksHit [1] = attacksHit [0];
		
        attacksHit [0] = lastAttackNumber;
    }
	
    public void ClearConsecutiveHits()
    {
        attacksHit [2] = -1;
		
        attacksHit [1] = -1;
		
        attacksHit [0] = -1;
    }
	
	
	#endregion
	
    void PopulateFromModel()
    {
        attacksHit = new int [3] {-1,-1,-1};
			
        aiHitsToStun = this._aiModel.hitsToStun;
		
        aiLifeToStun = _aiModel.lifeToStun;
		
        fireChance = _aiModel.fire;
        waterChance = _aiModel.water;
        earthChance = _aiModel.earth;
    }
	
	
	
    void Awake()
    {
        _aiCharStateController = gameObject.GetComponent<SRCharacterStateControllerNEW>();
		
        particleResolver = gameObject.GetComponent<ProjectileParticleResolver>();
		
        AISoundController = gameObject.transform.FindChild("SFX").GetComponent<CharacterSoundController>();
    }
	
	#region Pick elements by the model's element chance...
    private int fireChance = 0;
    private int waterChance = 0;
    private int earthChance = 0;

    private GestureEmitter.Gesture GetGestureAccordingToModelInfo()
    {
        int rand = new System.Random().Next(1, 101);

		
        int tempWater = waterChance;
        int tempFire = tempWater + fireChance;
        int tempEarth = tempFire + earthChance;
		
        if (rand <= tempWater)
        {
            return GestureEmitter.Gesture.kWater;
        } else if (rand <= tempFire)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("FIRE TURN!!!");
            return GestureEmitter.Gesture.kFire;
        } else if (rand <= tempEarth)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("EARTH TURN!!!");
            return GestureEmitter.Gesture.kEarth;
        } else
        {
            return GestureEmitter.Gesture.kLightning;
        }
    }
	#endregion
	
    private IEnumerator DestroyProjectile()
    {
        if (projectileGameObject != null)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::: ENEMY :::::::::: PROJECTILE DESTROYED!!!");
            Destroy(projectileGameObject);
        }
        yield return null;
    }
    //static GestureEmitter.Gesture tempGesture = GestureEmitter.Gesture.kLightning;
    public void Attack(int turnNumber, bool longCast, bool isShieldTut, BattleManager.AIGestureDelegate aiGestureDel)
    {
        GestureEmitter.Gesture gestureToCast;
        if (turnNumber == 1)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("DATA CLEARED!!! ::::::::::::::::::::::::::::::::::::::::::");
            PopulateFromModel();
            ClearConsecutiveHits();
            lifeLost = 0;
            totalHitsLanded = 0;
        }
		
	
        if (GameManager._gameState.firstEnemyDefeated)
            gestureToCast = GetGestureAccordingToModelInfo();
        else
            gestureToCast = GestureEmitter.Gesture.kFire;
		
        lastAttackNumber++;
		
        _endRound = false;
		
        /* 
		 * Logic to see if this gesture has cleared the tutorial.
		 * If it has not, call the delegate with the gesture...
		 */
		var battleManager = GameManager.instance._levelManager.battleManager;
        //TutorialManager.instance.ShowTutorial(gestureTutorialDictionary[tempGesture]);
        GestureEmitter.Gesture counterGesture = ProjectileStatsComparator.counterDictionary [gestureToCast];
		if (!battleManager.specialAttack && !(battleManager._petController != null && battleManager._petController.PetThrowState()) && battleManager._battleState._user.HasAvailableGesture(counterGesture))
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("USER HAS AVAILABLE - THE GESTURE --->>> " + counterGesture + "   ---- Enemy Casted --- " + gestureToCast);
            if (TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None)
                TutorialManager.instance.ShowTutorial(gestureTutorialDictionary [gestureToCast]);
        }
		
        //aiGestureDel(tempGesture);
        aiGestureDel(gestureToCast);
		
        this.isShieldTut = isShieldTut;

        this.castSpell(gestureToCast, false); //Disable enemy going into long cast!
    }
	
	
    private void setSpellTypeForSounds(GestureEmitter.Gesture aGesture)
    {
		
        switch (aGesture)
        {
            case GestureEmitter.Gesture.kEarth:
                spellBeingCasted = "Earth";
                break;
            case GestureEmitter.Gesture.kFire:
                spellBeingCasted = "Fire";
                break;
            case GestureEmitter.Gesture.kWater:
                spellBeingCasted = "Water";
                break;
            case GestureEmitter.Gesture.kLightning:
                spellBeingCasted = "Lightning";
                break;
        }
    }

	
	
    public void castSpell(GestureEmitter.Gesture gesture, bool longCast)
    {
        if (gesture == GestureEmitter.Gesture.kInvalid)
        {
            return;
        }
		
        base.SetEmitterMaterial(gesture);
		
        setSpellTypeForSounds(gesture);
        AISoundController.Invoke("Play" + spellBeingCasted + "ChargingSound", 0.0f);
		
        if (_endRound)
            return;
		
        projectileGameObject = particleResolver.GetParticleObjectForGestureWithPositionAndRotation(gesture, singleCastProjectileSpawnTransform.position, transform.rotation);
        projectileGameObject.transform.parent = singleCastProjectileSpawnTransform.gameObject.transform;
        //projectileGameObject.AddComponent<DeleteScript>();
        #region Projectile emitter size settings
        ParticleEmitter projectileParticleEmitter = projectileGameObject.GetComponent<ParticleEmitter>();
        float currentProjectileHeight = projectileParticleEmitter.maxSize;
        projectileParticleEmitter.maxSize = ENEMY_HEIGHT / ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT * currentProjectileHeight;
        projectileParticleEmitter.minSize = ENEMY_HEIGHT / ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT * currentProjectileHeight;
		
        #endregion
		
        Physics.IgnoreCollision(gameObject.collider, projectileGameObject.collider, true);
		
        projectileGameObject.tag = SpellCollideScript.ENEMY_PROJECTILE;
		
        if (!longCast)
            MakeShortCast();
        else
            MakeLongCast();
    }
	

    //static int temp = 1;
    private void MakeLongCast()
    {
        _castType = CastType.kLong;

        _aiCharStateController.SetCastingStateForDirectCharging(() => {
            AISoundController.PlayFocusSound();
            _aiCharStateController.IsCharging = true;
            if (!_endRound)
            {
                _aiCharStateController.SetChargeLoopState(speedValDictionary [_aiModel.speed], () => {
                    if (!_endRound)
                    {
                        if (!isShieldTut)
                            ExecuteLongThrow(0f);
                    }	
                });
            }
        });
    }
	

	
    private void MakeShortCast()
    {
        _castType = CastType.kShort;
			
        _aiCharStateController.SetCastingState(() => {
            AISoundController.PlayFocusSound();
            if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.WaterTutorialStart
                || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.FireTutorialStart
                || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.LightningTutorialStart
                || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.EarthTutorialStart
                || TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.NexFightStart
                || TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.runeSpellTutorialStart) {
                ExecuteShortThrow(float.MaxValue);

				// Turn off loading fill during tutorial after the animation has finished
				GameManager.instance.scaleformCamera.hud.SetLoadingFill(false);

			} else
                ExecuteShortThrow(speedValDictionary [_aiModel.speed] / Time.timeScale);
        });
    }
	
    enum RequestExecuted
    {
        kRequested,
        kNone,
        kExecuted
    }
    RequestExecuted _requestExecuted = RequestExecuted.kNone;
	
    private void ExecuteLongThrow(float speed)
    {
		
        //	_aiCharStateController.SetChargeLoopState(speed, () => {
        _requestExecuted = RequestExecuted.kRequested;
        if (!_endRound)
        {
            _aiCharStateController.ChargeThrow = true;
            _aiCharStateController.SetChargeToThrowState(() => {
                AISoundController.Invoke("Play" + spellBeingCasted + "CastSound", 0.0f);
//					if(!_endRound)
//					{
                AddStrengthToProjectile(GameObject.Find("BattleManager").GetComponent<BattleManager>()._battleState._enemy.damage * ENEMY_LONG_CAST_MULTIPLIER);
                ThrowCast(longCastProjectileSpawnTransform);
                _requestExecuted = RequestExecuted.kExecuted;
                //if(GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kReady && GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kStunEnded)
                if (GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kBusy)
                    _endRound = true;
//					}
                _aiCharStateController.SetStateToFocus(() => {
                    if (_onFocusStateListener != null)
                        _onFocusStateListener();
                });
            });
        }
        //	});				
    }
	
    private void ExecuteShortThrow(float speed)
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("_aiModel.speed" + speed);
        _aiCharStateController.SetCastToChargeState(speed, () => {
            //AISoundController.Invoke("Play"+spellBeingCasted+"ChargingSound",0.0f);
            if (!_endRound)
            {
                if (!GameManager.instance._levelManager.battleManager.specialAttack)
                {
                    GameManager.instance._levelManager.battleManager.EnemyAboutToThrow();
                    _aiCharStateController.CastThrow = true; //DELETE????
                    _aiCharStateController.SetCastToThrowState(() => {
                        AISoundController.Invoke("Play" + spellBeingCasted + "CastSound", 0.0f);
                        if (!_endRound)
                        {
                            AddStrengthToProjectile(GameObject.Find("BattleManager").GetComponent<BattleManager>()._battleState._enemy.damage);
						
                            ThrowCast(longCastProjectileSpawnTransform);
							
                            if (GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kBusy)
                                _endRound = true;
                        }
                        _aiCharStateController.SetStateToFocus(() => {
                            if (_onFocusStateListener != null)
                                _onFocusStateListener();
                        });
                    });
                }
            }
			
        });
    }
	
    private void AddStrengthToProjectile(float strength)
    {
        if (projectileGameObject != null)
        {
            projectileGameObject.GetComponent<ProjectileStatsComparator>().Strength = strength;
        }
    }

		
    private void ThrowSpell(GestureEmitter.Gesture gesture, Transform trans)
    {
        ThrowCast(trans);
    }
	
    public void ThrowCast(Transform trans)
    {	
        Vector3 toPos = enemyHitTransform.transform.position;
		
        if (projectileGameObject != null)
        {
            projectileGameObject.transform.parent = null;
            if (Time.timeScale < 1.0f)
                TweenPosition.Begin(projectileGameObject, BattleConstants.PROJECTILE_TIME_IN_AIR / (Time.timeScale / 2), toPos);
            else
                TweenPosition.Begin(projectileGameObject, BattleConstants.PROJECTILE_TIME_IN_AIR / Time.timeScale, toPos);
			
            if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.NexFightTutorialEnd)
                TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexCameraEnd);
        } else
			if (GameManager.PRINT_LOGS)
            Debug.Log("------------------------------------- projectileGameObject IS NULL -------------------------------------");
    }

	
	
    public void ThrowImmediate(bool isShieldTut)
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("THROW IMMEDIATE - AICharacterController - with bool value = " + isShieldTut + " _castType " + _castType);
		
        if (isShieldTut)
            Invoke("MakeThrow", 0f);//ExecuteLongThrow(0);
		
        if (_castType == CastType.kLong)
        {
            Invoke("MakeThrow", 0f);//ExecuteLongThrow(0f);
        } else if (_castType == CastType.kShort)
        {
            Invoke("MakeThrow", 0f); //Add delay between immediately throwing. Without the delay, projectile would ALWAYS hit player first!
        }
    }
	
	
    private void MakeLongThrow()
    {
        if (!_endRound)
        {
            _aiCharStateController.IsCharging = true;
            _aiCharStateController.ChargeThrow = true;
			
	
            _aiCharStateController.SetStateToFocus(() => {
                if (_onFocusStateListener != null)
                    _onFocusStateListener();
            });
        }
    }
	
    private void MakeThrow()
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("MakeThrow");
        if (!_endRound)
        {
            GameManager.instance._levelManager.battleManager.EnemyAboutToThrow();
            _aiCharStateController.SetCastToThrowState(() => {
				
                if (!_endRound)
                {
                    AddStrengthToProjectile(GameObject.Find("BattleManager").GetComponent<BattleManager>()._battleState._enemy.damage);
				
                    ThrowCast(singleCastProjectileSpawnTransform);
                    //if(GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kReady && GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kStunEnded)
                    if (GameManager.instance._levelManager.battleManager.enemyState == BattleManager.EnemyState.kBusy)
                        _endRound = true;
                }
	
                _aiCharStateController.SetStateToFocus(() => {
                    if (_onFocusStateListener != null)
                        _onFocusStateListener();
                });
            });
        }
    }
	
    void Update()
    {
        /*
		if(projectileGameObject != null)
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(projectileGameObject.transform.position);
		
			if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.WaterTutorial2 || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.FireTutorial2
				|| TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.LightningTutorial2 || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.EarthTutorial2)
				TutorialManager.instance.SetMask(screenPos.x , screenPos.y);
		}
		*/

        if (projectileGameObject != null)
        {
//			Vector3 screenPos = Camera.main.WorldToScreenPoint(projectileGameObject.transform.position);
            if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.WaterTutorial2 || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.FireTutorial2
                || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.LightningTutorial2 || TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.EarthTutorial2)
            {
//				Mask.instance.SetMask(screenPos.x , screenPos.y);

			
                if (!UIManager.instance._mask._mask.gameObject.activeInHierarchy)
                {
                    UIManager.instance._mask.SetMask();
                }
            }
        }

        if (projectileGameObject != null && !_aiCharStateController.IsCasting)
        {
            if (GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kWillStun
                || GameManager.instance._levelManager.battleManager._enemyStunState == BattleManager.EnemyStunState.kStunStarted)
                StartCoroutine(DestroyProjectile());
        }	
		
/*		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.NexFightTutorialEnd)
			Debug.Log("<<<--- NEX FIGHT MOVE TUT --->>>");*/
    }
}
