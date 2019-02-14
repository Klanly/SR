using UnityEngine;
using System.Collections;

public class PVPBattleManager : BattleManager
{

	private const string TAG = "PVPBattleManager";
	
	public SRCharacterController _enemyController;
	
	public GameObject aiGameObject;
	
	//public GameObject _gestureObject;
	//private GestureEmitter _gestureEmitter;
	
	//private enum State{kIdle, kFocus, kWaitingForGesture, kRevert, kRecognizedCast, kLaunch, kDamage, kNone};
	//State currentState = State.kNone;
	
	public GameObject PlayerPos;
	public GameObject EnemyPos;
	public GameObject CameraPos;
	public GameObject pvpCamera;
	
	private int myWardCount;
	private int hisWardCount;
	
	private int myLife;
	private int hisLife;
	
	private int myDamage;
	private int hisDamage;
	
	RaidPortalNavigator portalNavigator;
	
	void Start ()
	{
		portalNavigator = gameObject.GetComponent<RaidPortalNavigator> ();
	}
	void OnEnable ()
	{
	
	}
	
	public void OnAiLoaded (GameObject playerGameObject, GameObject aiGameObject)
	{
		//_gestureObject = GameManager.instance._levelManager.battleManager._gestureObject;
		
		this.aiGameObject = aiGameObject;
		this.aiGameObject.tag = "Enemy";
		
		_enemyController = aiGameObject.GetComponent<SRCharacterController> ();
		_characterController = playerGameObject.GetComponent<SRCharacterController> ();
		
		_enemyController.enemyHitTransform = _characterController.characterHitTransform.gameObject;
		_characterController.enemyHitTransform = _enemyController.characterHitTransform.gameObject;
		
		GameManager.instance.scaleformCamera.OnLoadBattleStart ();
		
		Invoke ("Initialize", 1.0f);
	}
	
	public override void Initialize ()
	{
		_gestureObject = GameManager.instance._levelManager.battleManager._gestureObject;
		_gestureObject.SetActive (true);
		
		_gestureEmitter = _gestureObject.GetComponent<GestureEmitter> ();
		
		GameManager.instance._levelManager.battleManager = gameObject.GetComponent<PVPBattleManager> ();
		
		_gestureEmitter.SetDelegate (this.OnGestureReceived);
		_gestureEmitter.SetFingerDownDelegate (this.OnFingerEvent);
		EnableGestureInput (false);
		
		_characterController.SetFocusState ();
		_enemyController.SetFocusState ();
		
		_characterController.gameObject.transform.parent = PlayerPos.transform;
		_characterController.gameObject.transform.rotation = PlayerPos.transform.rotation;
		_characterController.gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
		_characterController.gameObject.transform.localRotation = Quaternion.identity; 
		myWardCount = _characterController._user._wards;
		myLife = _characterController._user.life;
		myDamage = _characterController._user.damage;
		
		_enemyController.gameObject.transform.parent = EnemyPos.transform;
		_enemyController.gameObject.transform.rotation = EnemyPos.transform.rotation;
		_enemyController.gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
		_enemyController.gameObject.transform.localRotation = Quaternion.identity;
		hisWardCount = _enemyController._user._wards;
		hisLife = _enemyController._user.life;
		hisDamage = _enemyController._user.damage;
		
		Camera.main.gameObject.SetActive (false);
		pvpCamera.SetActive (true);
		
		_characterController.RegisterForCallbacksFromStateController ();
		_enemyController.RegisterForCallbacksFromStateController ();
		
		battleEnded = false;
		
		PlayMakerFSM.BroadcastEvent ("FadeInEvent");
		Debug.LogWarning("PvpBattleManager Initialize");
		Invoke ("StartBattleSet", 2f);
	}
	
	//Tell server that we are ready to input
	private void StartBattleSet ()
	{
		if (!battleEnded)
			PVPServerManager.Instance.TellPVPReady (this.OnPVPReady);
	}
	
	//Response from server, to start battle
	private void OnPVPReady (bool requestSucceeded)
	{
		StartGestureInputTimer ();
	}
	
	private const float TURN_TIME = 5.0f;
	private float turnTimer;
	GestureEmitter.Gesture currentGesture;

	private void StartGestureInputTimer ()
	{
		EnableGestureInput (true);
		currentGesture = GestureEmitter.Gesture.kInvalid;
		turnTimer = TURN_TIME;
		
		_enemyController.OnGestureStarted ();
	}

	System.Diagnostics.Stopwatch stopwatch;

	private void EnableGestureInput (bool yesNo)
	{

		if(yesNo) {
			stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
		} else {
			stopwatch.Stop();
			Debug.LogError(stopwatch.ElapsedMilliseconds/1000);
		}
		_gestureObject.SetActive (yesNo);
		_gestureEmitter.enabled = yesNo;
		showCastCue = yesNo;
		GameManager.instance.scaleformCamera.hud.ShowCastNow (yesNo);

	}
	
	protected void OnFingerEvent (bool fingerIsDown)
	{
		if (fingerIsDown) {
			if (_gestureEmitter.enabled)
				OnFingerDown ();
		} else
			OnFingerUp ();
	}
	
	private void OnFingerDown ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log (TAG + "OnFingerDown");
		
		currentState = State.kWaitingForGesture;
		_characterController.OnGestureStarted ();
	}
	
	private void OnGestureReceived (GestureEmitter.Gesture gesture)
	{
		currentGesture = gesture;
		if (gesture != GestureEmitter.Gesture.kInvalid) {
			currentState = _characterController.castSpell (gesture, false, false) ? State.kRecognizedCast : State.kRevert;
			EnableGestureInput (false);
		} else
			EnableGestureInput (true);
	}
	
	private void OnGestureInputTimerExpired ()
	{
		SendTurnToServer (currentGesture);
	}
	
	private void SendTurnToServer (GestureEmitter.Gesture gesture)
	{
		PVPServerManager.Instance.SubmitPVPTurn (gesture.ToString (), OnTurnResultReceived);
	}
	
	//Gesture casted by enemy would be received in the param: "obj" from server
	private void OnTurnResultReceived (GestureEmitter.Gesture gesture)
	{
		_enemyController.castSpell (gesture, false, false);
		
		StartCoroutine (BothPlayersThrow (1.0f));
	}
	
	private IEnumerator BothPlayersThrow (float delay)
	{
		yield return new WaitForSeconds (delay);
		_characterController.ThrowCast (this.OnCastComplete);
		_enemyController.ThrowCast (this.OnCastComplete, true);
		
		Invoke ("StartBattleSet", 2f);
	}
	
	protected void OnCastComplete (SRCharacterController.CastState cState)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log (TAG + "OnCastComplete - " + cState.ToString ());
	}
	
	private void OnFingerUp ()
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log (TAG + "OnFingerUp");
	}
	
	public override void OnGameobjectCollided (GameObject anObject, ProjectileStatsComparator projectileStats, bool destroyProj = true)
	{
		if (anObject != null) {
			if (anObject.tag.Equals (SpellCollideScript.PLAYER)) {
				if (myWardCount > 0) {
					_characterController.OnWardHit ();
					myWardCount--;
					GameManager.instance.scaleformCamera.hud.TurnOffWard ();
				} else {
					_characterController.OnHit (true, projectileStats._projectileType);
					
					if ((myLife - projectileStats.Strength) <= 0.99) {
						myLife = 0; 
						OnBattleEnded (true);
						return;
					} else {
						myLife -= Mathf.CeilToInt (projectileStats.Strength);
					}
				}
			} else if (anObject.tag.Equals (SpellCollideScript.ENEMY)) {
				if (hisWardCount > 0) {
					_enemyController.OnWardHit ();
					hisWardCount--;
				} else {
					_characterController.OnHit (true, projectileStats._projectileType);
					
					if ((hisLife - projectileStats.Strength) <= 0.99) {
						hisLife = 0; 
						OnBattleEnded (false);
						return;
					} else {
						hisLife -= Mathf.CeilToInt (projectileStats.Strength);
					}
				}
			}
		}
	}
	
	
	private void OnBattleEnded (bool playerDied)
	{
		battleEnded = true;
		
		EnableGestureInput (false);
		
		if (playerDied)
			OnPlayerDied ();
		else
			OnEnemyDied ();
		portalNavigator.MoveToNeutralPosition ();
	}
	
	protected override void OnPlayerDied ()
	{
		_characterController.OnDeath ();
		_characterController.StopBreathingSound ();
		StartCoroutine (FadeOutAndLoadDefeatScreen (1.5f));
		GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup ("Booooo...", "YOU LOST!!!");
	}
	
	protected override void OnEnemyDied ()
	{
		_enemyController.OnDeath ();
		_enemyController.StopBreathingSound ();
		GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup ("Congratulations", "YOU WON!!!");
		Destroy (_enemyController.gameObject);
	}
	
	private void Update ()
	{
		if (turnTimer > 0)
			turnTimer -= Time.deltaTime;
		if (turnTimer < 0) {
			turnTimer = 0;
			if (!battleEnded)
				OnGestureInputTimerExpired ();
		}
	}
}
