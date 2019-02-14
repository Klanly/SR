using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class SRCharacterController : SRGenericCharacterController,StateImplementerInterface
{
    ProjectileParticleResolver particleResolver;
	
    float tempTime = 0.0f;
	
    private const string TAG = "*** SRCharacterController !!!......";
	
    public const string CHARGED_PROJECTILE_TAG = "Charged Projectile";
	
    public const string SIMPLE_PROJECTILE_TAG = "Projectile";
	
    private float CHARGED_DAMAGE_MULTIPLIER
    {
        get
        {
            return DatabankSystem.Databank.CHARGED_DAMAGE_MULTIPLIER;
        }
    }
	
    private float CHARGED_DAMAGE_SPELLBURST
    {
        get
        {
            return DatabankSystem.Databank.CHARGED_DAMAGE_SPELLBURST;
        }
    }
	
    private const float SIMPLE_DAMAGE_MULTIPLIER = 1.0f;
	
    private int BURST_DAMAGE_SUPPRESSION
    {
        get
        {
            return DatabankSystem.Databank.BURST_DAMAGE_SUPPRESSION;
        }
    }
	
	
    private const float CAST_THROW_DELAY = 0.2f;
	
    private const float CHARGE_THROW_DELAY = 0.2f;
	
    public enum ProjectileType
    {
        kSimple,
        kCharged
    }
	
    private const float TO_FOCUS_TIME = 0.3f;	
	
    public GameObject enemyHitTransform;
	
    public Transform projectileSpawnPoint;
	
    public Transform rapidFireSpawnPoint;
	
    public SRCharacterStateControllerYetAgain charStateController
    {
        get
        {
            if (_charStateController == null)
                _charStateController = gameObject.transform.GetComponentInChildren<SRCharacterStateControllerYetAgain>();
            return _charStateController;
        }
        set
        {
            _charStateController = value;
        }
    }
	
    private SRCharacterStateControllerYetAgain _charStateController;
	
    private GameObject lastCastedProjectile = null;
	
    private static int run = 0;
	
    private float PLAYER_HEIGHT;
	
    public Transform characterHitTransform;

    public delegate void PlayerThrowDelegate(CastState castState);
	
    public PlayerThrowDelegate _playerThrowDelegate;
	
    public delegate void CharacterFocusStateListener();
	
    public CharacterFocusStateListener _charFocusStateListener;
	
    public delegate void CharacterLongCastListener();
	
    public CharacterLongCastListener characterLongCastListener
    {
        set;
        get;
    }
	
    public enum CastState
    {
        kShortNormalCast,
        kShortCast,
        kLongCast,
        kInvalid,
        kPostFocus,
        kLaunchShort,
        kLaunchLong
    }
	
    private CastState currentState = CastState.kInvalid;
	
    public bool showFocusBar = false;
	
    public bool hideFocusBar = false;
	
//	private float tempCastTime = 0;

    private bool throwRequestReceived = false;
	
    public User _user
    {
        set;
        get;
    }
	
    public List<GameObject> livingProjectiles;

    public CharacterSoundController SorcererSoundController;
    private string spellBeingCasted;
	
    public GameObject hoverParticle;
    public GameObject hoverClone;
	
    void Awake()
    {
        //charStateController = gameObject.GetComponent<SRCharacterStateControllerYetAgain>(); //set it by characterselectcontroller
		
        particleResolver = gameObject.GetComponent<ProjectileParticleResolver>();
        //SorcererSoundController = gameObject.transform.FindChild("SFX").GetComponent<CharacterSoundController>(); //set it by characterselectcontroller
    }
	
    void Start()
    {
        PLAYER_HEIGHT = gameObject.GetComponent<BoxCollider>().size.y;
        livingProjectiles = new List<GameObject>();
    }
    /*
	public void PlayHeartBeatSound(){
		SorcererSoundController.PlayHeartBeatSound();
	}
	
	public void StopHeartBeatSound(){
		SorcererSoundController.StopHeartBeatSound();
	}
	*/
    public void PlayBreathingSound()
    {
        SorcererSoundController.PlayHeavyBreathingSound();
    }
	
    public void StopBreathingSound()
    {
        SorcererSoundController.StopHeavyBreathingSound();
    }
	
	
    public override void OnSetStart()
    {
        if (run == 0)
        {
            RegisterForCallbacksFromStateController();
            run = 1;
        }
    }
	
    public void RegisterForCallbacksFromStateController()
    {
        charStateController.SetStateImplementer(this);
    }
	
    public void CallLevelManagerFromAnimator()
    {
		Debug.LogError("CallLevelManagerForAnimationFinished - SRCharacterController "+gameObject.name);

		GameManager.instance._levelManager.ExecutePoiFunctionality();
    }
	
    public void OnFocusStateReached()
    {
//		if(GameManager.PRINT_LOGS) Debug.Log(" ----------- _charFocusStateListener ---------- ");
        if (_charFocusStateListener == null)
        if (GameManager.PRINT_LOGS)
            Debug.Log("_charFocusStateListener IS NUKLLLLLLLLLLLLLLLLLLLLLLLLLLL---------------------------------------------------------------------------------------");
        if (_charFocusStateListener != null)
            _charFocusStateListener();
    }
	
    public void OnGestureStarted()
    {	
        charStateController.SetCastingState();
    }
	
	#region Unused callbacks
    public void OnSpellCastStarted()
    {
    }

    public void OnFocusToCastStateReached()
    {	
        SorcererSoundController.PlayFocusSound();
    }
	
    public void OnCastingLoopReached()
    {	
    }
	
    public void OnCastToChargeReached()
    {
    }
		
    public void OnChargingLoopReached()
    {
    }
	#endregion
	
    public static string GetSpellNameForGesture(GestureEmitter.Gesture aGesture)
    {
        string spellString = null;
        switch (aGesture)
        {
            case GestureEmitter.Gesture.kEarth:
                spellString = "earth";
                break;
            case GestureEmitter.Gesture.kFire:
                spellString = "fire";
                break;
            case GestureEmitter.Gesture.kWater:
                spellString = "water";
                break;
            case GestureEmitter.Gesture.kLightning:
                spellString = "lightning";
                break;
            case GestureEmitter.Gesture.Ignite:
                spellString = "fire";
                break;
            case GestureEmitter.Gesture.Daze:
                spellString = "lightning";
                break;
            case GestureEmitter.Gesture.Drain:
                spellString = "water";
                break;
            case GestureEmitter.Gesture.LeechSeed:
                spellString = "earth";
                break;
        }
        return spellString;
    }
	
    public static string GetRuneNameForRuneGesture(GestureEmitter.Gesture aGesture)
    {
        string spellString = null;
        switch (aGesture)
        {
            case GestureEmitter.Gesture.Ignite:
                spellString = "Ignite";
                break;
            case GestureEmitter.Gesture.Daze:
                spellString = "Daze";
                break;
            case GestureEmitter.Gesture.Drain:
                spellString = "Drain";
                break;
            case GestureEmitter.Gesture.LeechSeed:
                spellString = "Leechseed";
                break;
        }
        return spellString;
    }
	
    public static GestureEmitter.Gesture GetGestureForSpellName(string spellName)
    {
        spellName = spellName.ToLower();
        if (spellName.Equals("fire"))
            return GestureEmitter.Gesture.Ignite;
        else if (spellName.Equals("lightning"))
            return GestureEmitter.Gesture.Daze;
        else if (spellName.Equals("water"))
            return GestureEmitter.Gesture.Drain;
        else if (spellName.Equals("earth"))
            return GestureEmitter.Gesture.LeechSeed;
        return GestureEmitter.Gesture.kInvalid;
    }
	
	
    public Buff GetBuffForChargedSpell(GestureEmitter.Gesture aGesture)
    {
        return _user.GetBuffForSpell(GetSpellNameForGesture(aGesture));
    }
	
    public void OnBurstLoopReached()
    {
        SpawnBurstProjectiles(DatabankSystem.Databank.TOTAL_BURST_PROJECTILES);
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
	
	
    private void ThrowLast() //This is NEXT LEVEL BAD CODING!!! but khair hai for now  - plus couldn't come up with a better solution :(
    {
        lastBurstProjectile.transform.parent = null;
        if (lastBurstProjectile != null)
            ProcessProjectileStrength(true, true, lastBurstProjectile);
        else
			if (GameManager.PRINT_LOGS)
            Debug.Log("lastProj ------------------ null");
        lastBurstProjectile.AddComponent<DeleteScript>();
//		Debug.LogError("time = "+BattleConstants.PROJECTILE_TIME_IN_AIR/Time.timeScale+" timescale - "+Time.timeScale);
        TweenPosition.Begin(lastBurstProjectile, (BattleConstants.PROJECTILE_TIME_IN_AIR) / Time.timeScale, enemyHitTransform.transform.position);
    }

    public void OnPowerThrowReached()
    {
		// TODO Don't know why the two take different time to finish animation but meh! this kind of fixes the problem
		if(GameManager.instance.isMultiPlayerMode) {
			Invoke("ThrowLast", 2.0f);
		} else {
			Invoke("ThrowLast", 1.1f);
		}
	}
	
    private GameObject lastBurstProjectile = null;
    public bool castSpell(GestureEmitter.Gesture gesture, bool rapidCastMode = false, bool specialAttack = false)
    {	
//		if(GameManager.PRINT_LOGS) Debug.Log("CAST SPELL - specialAttack " + specialAttack + " gesture " + gesture);
        if (rapidCastMode)
        {
            int randomIndex = livingProjectiles.Count - 1;
            if (randomIndex >= 0)
            {
                if (randomIndex == 0)
                {
                    GameManager.instance._levelManager.battleManager.FinalRapidAttackStarted();
                    lastBurstProjectile = livingProjectiles [0];
                    lastBurstProjectile.transform.position = rapidFireSpawnPoint.transform.position;
                    charStateController.PowerThrow = true;
                    livingProjectiles.Clear();
                } else
                {
                    if (randomIndex == 4 && TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.OnSpellBurstTutorialStart)
                    {
                        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.spellBurstTutorialEnd);
                        GameManager.instance._levelManager.battleManager.EndSpellBurstTutorial();
                    }
					
                    livingProjectiles [randomIndex].transform.parent = null;
                    ProcessProjectileStrength(true, false, livingProjectiles [randomIndex]);
                    TweenPosition.Begin(livingProjectiles [randomIndex], (BattleConstants.PROJECTILE_TIME_IN_AIR - 0.2f) / Time.timeScale, enemyHitTransform.transform.position);
                    livingProjectiles.RemoveAt(randomIndex);
                }
            }

            return rapidCastMode;
        }
		
        bool recognized = false;
        throwRequestReceived = false;
		
        if (gesture != GestureEmitter.Gesture.kInvalid)
        {	
            if (specialAttack)
            {
                Time.timeScale = 0.5f;
                if (GameManager.PRINT_LOGS)
                    Debug.Log("-----------specialAttack--------- gesture = " + gesture);
                if (GestureEmitter.specialGestures.Contains(gesture))
                {
                    recognized = true;
                    currentState = CastState.kShortCast;
                    if (GameManager.PRINT_LOGS)
                        Debug.Log("currentState = " + currentState);
					
                    setSpellTypeForSounds(gesture);
                    SorcererSoundController.Invoke("Play" + spellBeingCasted + "ChargingSound", 0.0f);
					
                    SetProjectileSizeAndEmitters(gesture);
			
                    tempTime = Time.time;
                } else
                {
                    currentState = CastState.kInvalid;
                    if (GameManager.PRINT_LOGS)
                        Debug.Log("currentState = " + currentState);
                }
            } else
            {
                if (GestureEmitter.normalGestures.Contains(gesture))
                {
                    recognized = true;
                    //currentState = CastState.kShortCast;
                    currentState = CastState.kShortNormalCast;
//					if(GameManager.PRINT_LOGS) Debug.Log("currentState = " + currentState);
                    setSpellTypeForSounds(gesture);
                    SorcererSoundController.Invoke("Play" + spellBeingCasted + "ChargingSound", 0.0f);
					
                    SetProjectileSizeAndEmitters(gesture);
                    tempTime = 0;
                } else
                {
                    currentState = CastState.kInvalid;
                    if (GameManager.PRINT_LOGS)
                        Debug.Log("currentState = " + currentState);
                }
            }
        } else
        {
            currentState = CastState.kInvalid;
            if (GameManager.PRINT_LOGS)
                Debug.Log("currentState = " + currentState);
        }
			
        if (currentState == CastState.kInvalid)
            RevertCast();
			
        return recognized;
    }
	
    private void SetProjectileSizeAndEmitters(GestureEmitter.Gesture gesture)
    {
        lastCastedProjectile = particleResolver.GetParticleObjectForGestureWithPositionAndRotation(gesture, projectileSpawnPoint.position, transform.rotation);
        lastCastedProjectile.transform.parent = projectileSpawnPoint.gameObject.transform;
			
        livingProjectiles.Add(lastCastedProjectile);
		
        #region Projectile emitter size settings
        ParticleEmitter projectileParticleEmitter = lastCastedProjectile.GetComponent<ParticleEmitter>();
        float currentProjectileHeight = projectileParticleEmitter.maxSize;
        projectileParticleEmitter.maxSize = PLAYER_HEIGHT / ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT * currentProjectileHeight;
        projectileParticleEmitter.minSize = PLAYER_HEIGHT / ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT * currentProjectileHeight;
        #endregion
		
        Physics.IgnoreCollision(gameObject.collider, lastCastedProjectile.collider, true);
    }
	
    void Update()
    {
        CheckForFocus();
		
        UpdateStateIfFocus();
		
        if (currentState == CastState.kLaunchShort || currentState == CastState.kLaunchLong)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("Throw() REQUESTED!!!");
            Throw();
        }
		
        CheckIfThrowRequested();
    }
	
    private void CheckIfThrowRequested()
    {
        if (throwRequestReceived)
        {
            hideFocusBar = true;
		
            if (currentState != CastState.kInvalid)
            {
                switch (currentState)
                {
                    case CastState.kShortNormalCast:
                        currentState = CastState.kLaunchShort;
					
                        break;
                    case CastState.kShortCast:
                        currentState = CastState.kInvalid;
                        RevertCast();
                        DestroyProjectile();
                        if (_playerThrowDelegate != null)
                            _playerThrowDelegate(currentState);
                        break;
                    case CastState.kPostFocus:
                        currentState = CastState.kLaunchLong;
                        break;
                }
//				if(GameManager.PRINT_LOGS) Debug.Log("currentState = " + currentState);
            } else
            {
//				if(GameManager.PRINT_LOGS) Debug.Log("CheckIfThrowRequested - currentState" + currentState);
                if (_playerThrowDelegate != null)
                    _playerThrowDelegate(currentState);
            }
			
            throwRequestReceived = false;
        }
    }
	
    public void RevertCast()
    {
        charStateController.Revert = true;
    }
	
    private void CheckForFocus()
    {
        if (currentState == CastState.kShortCast)
        {
            if (Time.time > (tempTime + TO_FOCUS_TIME))
            {
                currentState = CastState.kLongCast; // cast progress - focus
                if (GameManager.PRINT_LOGS)
                    Debug.Log("currentState = " + currentState);
            }
        }
    }
	
    private void UpdateStateIfFocus()
    {
        if (currentState == CastState.kLongCast)
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("LONG CAST!!!");
			
            if (characterLongCastListener != null)
                characterLongCastListener();
			
            charStateController.SetCastToChargeState();	
            SorcererSoundController.PlayFocusSound();
            showFocusBar = true;
            currentState = CastState.kPostFocus;
            if (GameManager.PRINT_LOGS)
                Debug.Log("currentState = " + currentState);
            GrowProjectile();
        }
    }
	
    private void GrowProjectile()
    {
        if (lastCastedProjectile != null)
        {
            ParticleEmitter projectileParticleEmitter = lastCastedProjectile.GetComponent<ParticleEmitter>();
            projectileParticleEmitter.maxSize = projectileParticleEmitter.maxSize * 2;
            projectileParticleEmitter.minSize = projectileParticleEmitter.minSize * 2;
			
            projectileParticleEmitter.maxEmission = projectileParticleEmitter.maxEmission * 2;
            projectileParticleEmitter.minEmission = projectileParticleEmitter.minEmission * 2;
        }
    }
	
    public void Throw()
    {
        if (currentState == CastState.kLaunchLong)
        {
            if (TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.OnRuneSpellTutorial2)
            {
                //TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnRuneSpellTutorial2);
                TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.runeSpellTutorialEnd);
            }
            charStateController.SetChargeToThrowState();
        } else
            charStateController.SetCastToThrowState();
		
//		currentState = CastState.kInvalid;
//		if(GameManager.PRINT_LOGS) Debug.Log("currentState = " + currentState);
    }
	
    private bool fakeAsEnemy = false;
    public void ThrowCast(PlayerThrowDelegate del, bool fakeAsEnemy = false)
    {	
        this.fakeAsEnemy = fakeAsEnemy;
        this._playerThrowDelegate = del;
        throwRequestReceived = true;
    }
	
    public void OnCastThrowReached()
    {
		
        if (lastCastedProjectile != null)
        {
            ProcessProjectileStrength();
            StartCoroutine(DetachAndThrow(CAST_THROW_DELAY, ProjectileType.kSimple));
        }
		
        if (_playerThrowDelegate != null)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("currentState at cast throw..." + currentState);
            _playerThrowDelegate(currentState);
        }
		
        currentState = CastState.kInvalid;
    }
	
    private IEnumerator DetachAndThrow(float delay, ProjectileType projectileType)
    {
        yield return new WaitForSeconds(delay);
        DetachProjectileAndThrow(projectileType);
		
        SorcererSoundController.Invoke("Play" + spellBeingCasted + "CastSound", 0.0f);
    }
	
    public void OnChargeThrowReached()
    {
        if (lastCastedProjectile != null)
        {
            ProcessProjectileStrength();
			
            StartCoroutine(DetachAndThrow(CHARGE_THROW_DELAY, ProjectileType.kCharged));
        }
		
        if (_playerThrowDelegate != null)
            _playerThrowDelegate(currentState);
        currentState = CastState.kInvalid;

    }
	
	
    public void OnHit(bool destroyProj, GestureEmitter.Gesture aGesture)
    {		
        if (destroyProj)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("PLAYER HIT - DESTROY projectile!!");
            DestroyProjectile();
        } else
			if (GameManager.PRINT_LOGS)
            Debug.Log("Dont destroy projectile!!");
		
        charStateController.SetDamagedState();
		
        throwRequestReceived = false;
		
        showFocusBar = false;
		
        hideFocusBar = true;
		
//		tempCastTime = 0;
		
		
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
        //SorcererSoundController.Invoke("Play"+spellHit+"ImpactSound",0.0f);
        SorcererSoundController.PlayDamageSound();
        SorcererSoundController.Invoke("Play" + spellHit + "ImpactSound", 0.0f);
    }
	
    public void OnWardStateReached()
    {
    }
	
    public void OnWardHit(bool destroyProj = true)
    {
        if (destroyProj)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("PLAYER HIT - DESTROY projectile!!");
            DestroyProjectile();
        } else
			if (GameManager.PRINT_LOGS)
            Debug.Log("Dont destroy projectile!!");
		
        charStateController.SetWardState();
		
        throwRequestReceived = false;
		
        showFocusBar = false;
		
        hideFocusBar = true;
		
//		tempCastTime = 0;
		
    }
	
    public void OnDeath()
    {
        //StartCoroutine(DestroyBattleHud());
        UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
        GameManager.instance.scaleformCamera.hud.SetDisplayVisible(false);
        charStateController.SetDeathState();
        SorcererSoundController.PlayDeathSound();
    }
	

	#region Emitters
    public Material generalAura;
    public Material fireAura;
    public Material lightningAura;
    public Material waterAura;
    public Material earthAura;
		
    /*
	private void SetEmitterMaterial(GestureEmitter.Gesture gesture)
	{
		if(gesture != GestureEmitter.Gesture.kInvalid)
		{
			if(gesture==GestureEmitter.Gesture.kFire)
			{
				leftHandEmitter.renderer.material=fireAura;
		
				rightHandEmitter.renderer.material=fireAura;
			}
			else if(gesture==GestureEmitter.Gesture.kLightning)
			{
				leftHandEmitter.renderer.material=lightningAura;
		
				rightHandEmitter.renderer.material=lightningAura;
			}
			else if(gesture==GestureEmitter.Gesture.kEarth)
			{
				leftHandEmitter.renderer.material=earthAura;
		
				rightHandEmitter.renderer.material=earthAura;
			}
			else if(gesture==GestureEmitter.Gesture.kWater)
			{
				leftHandEmitter.renderer.material=waterAura;
		
				rightHandEmitter.renderer.material=waterAura;
			}
			else
			{
				leftHandEmitter.renderer.material=generalAura;
			
				rightHandEmitter.renderer.material=generalAura;
			}
		}
		else
		{
			leftHandEmitter.renderer.material=generalAura;
			
			rightHandEmitter.renderer.material=generalAura;
		}
	}
	*/
	#endregion
	
	
	#region Projectile handling
    private void ProcessProjectileStrength(bool burstMode = false, bool isFinalBurstAttack = false, GameObject burstObject = null) // Pass gameobject only in case of last projectile in spell burst
    {
        float playerDamage;
        if (GameManager.instance.isMultiPlayerMode)
            playerDamage = this._user.damage;
        else
            playerDamage = GameManager.instance._levelManager.battleManager._battleState._user.damage;
		
        if (burstMode)
        {
            if (isFinalBurstAttack)
                burstObject.GetComponent<ProjectileStatsComparator>().Strength = playerDamage * CHARGED_DAMAGE_SPELLBURST; // burst damage
			else
                burstObject.GetComponent<ProjectileStatsComparator>().Strength = playerDamage / BURST_DAMAGE_SUPPRESSION;
        } else
        {
            if (lastCastedProjectile != null)
                lastCastedProjectile.GetComponent<ProjectileStatsComparator>().Strength = playerDamage;
        }
    }
	
    public void DestroyProjectile()
    {
        if (livingProjectiles.Contains(lastCastedProjectile))
            livingProjectiles.Remove(lastCastedProjectile);
        if (lastCastedProjectile != null)
        {
//			if(GameManager.PRINT_LOGS) Debug.Log("::::::: PLAYER :::::::::: PROJECTILE DESTROYED!!!");
            Destroy(lastCastedProjectile);
        }
    }
	
    private void DetachProjectileAndThrow(ProjectileType pType)
    {
        if (lastCastedProjectile != null)
        {
            float playerDamage;
            if (GameManager.instance.isMultiPlayerMode)
                playerDamage = this._user.damage;
            else
                playerDamage = GameManager.instance._levelManager.battleManager._battleState._user.damage;
			
			
			
            lastCastedProjectile.transform.parent = null;
				
            if (fakeAsEnemy)
                lastCastedProjectile.tag = SpellCollideScript.ENEMY_PROJECTILE;
            else
                lastCastedProjectile.tag = (pType == ProjectileType.kCharged) ? CHARGED_PROJECTILE_TAG : SIMPLE_PROJECTILE_TAG;

            lastCastedProjectile.GetComponent<ProjectileStatsComparator>().Strength = (pType == ProjectileType.kCharged) ? playerDamage * CHARGED_DAMAGE_MULTIPLIER : playerDamage * SIMPLE_DAMAGE_MULTIPLIER;
			
            lastCastedProjectile.GetComponent<ProjectileStatsComparator>().isCharged = (pType == ProjectileType.kCharged) ? true : false;
			
            lastCastedProjectile.gameObject.AddComponent<DeleteScript>();

            TweenPosition.Begin(lastCastedProjectile, BattleConstants.PROJECTILE_TIME_IN_AIR / Time.timeScale, enemyHitTransform.transform.position);
        }
    }
	#endregion
	
	#region Burst mode
    public void SpawnBurstProjectiles(int points)
    {
        StartCoroutine(SpawnProjectiles(points));
    }
	
    private IEnumerator SpawnProjectiles(int points)
    {
        float deg = 360 / points;
        float radius = 40f;
		
        float x;
        float y;
		
        Vector3 spawnPoint = Vector3.zero;
		
        GameObject projectile = null;
		
        for (int i =0; i<points; i++)
        {
            float theta = Mathf.Deg2Rad * (deg * i);
			
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
			
            spawnPoint = new Vector3(x, 0, y);
			
            projectile = particleResolver.GetParticleObjectForGestureWithPositionAndRotation(GestureEmitter.Gesture.Ignite, spawnPoint, transform.rotation);
            projectile.transform.parent = rapidFireSpawnPoint;
            //projectile.GetComponent<ParticleEmitter>().enabled = false;
			
            projectile.transform.localPosition = spawnPoint;
            projectile.tag = SIMPLE_PROJECTILE_TAG;
			
            Physics.IgnoreCollision(gameObject.collider, projectile.collider, true);
			
            livingProjectiles.Add(projectile);
            yield return new WaitForSeconds(0.1f);
        }
		
        livingProjectiles.Shuffle();
        InvokeRepeating("RotateRapidFireRotation", 0.0f, 0.1f);
		
        EnableBurstAttack();
    }
	
    public void EnableBurstAttack()
    {
        GameManager.instance._levelManager.battleManager.AllowRapidAttack();
    }
	
    private void RotateRapidFireRotation()
    {
        rapidFireSpawnPoint.transform.Rotate(Vector3.up, 2f);
    }
	
    public void EnableBurst()
    {
        charStateController.Burst = true;
        hoverClone = GameObject.Instantiate(hoverParticle, transform.position, transform.rotation) as GameObject;
//		hoverClone.transform.parent = this.gameObject.transform;
    }
	
    public void DisableBurst()
    {
        charStateController.Burst = false;
		
        CancelInvoke("RotateRapidFireRotation");
		
        for (int i = 0; i<livingProjectiles.Count; i++)
            Destroy(livingProjectiles [i]);
		
        livingProjectiles.Clear();
		
        if (hoverClone != null)
            Destroy(hoverClone);
    }
	#endregion
    public void ResetStates()
    {
        charStateController.ResetStates();
	
        showFocusBar = false;
	
        hideFocusBar = false;
	
//	 	tempCastTime = 0;
	
        run = 0;
    }
	
    public void ResetRun()
    {
        run = 0;
    }
	
    public void SetFocusState()
    {
        charStateController.SetIdleToFocusState();
    }
	
    public void SetIdle()
    {
        charStateController.SetIdle();
    }
		
    public void TurnLeft()
    {
        charStateController.TurnLeft = true;		
    }
	
    public void TurnRight()
    {
        charStateController.TurnRight = true;
    }
	
    public void OnDamageStateReached()
    {
    }
	
    public void Walk()
    {
        //Idle=false;
        charStateController.Walk();
        //_animator.SetBool("Walk", true);
    }
    public void KneelDown()
    {
        charStateController.KneelDown();
		
    }
    public void KneelSwitchTurnOff()
    {
        charStateController.KneelSwitchTurnOff();
    }
    public void Stop()
    {
        charStateController.Stop();
    }
	
    public void TreasureChestOpen()
    {
        charStateController.TreasureChestOpen();
    }
    public void TreasureChestClose()
    {
        charStateController.TreasureChestClose();
    }
}
