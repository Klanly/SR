using UnityEngine;
using System.Collections;

public class PetController : MonoBehaviour, SRPetStateController.PetStateInterface
{
	SRPetStateController _petStateController;
	BattleManager _battleManager;
	
	public Transform spellSpawnPoint;
	ProjectileParticleResolver _particleResolver;
	
	private GameObject _spellObject;
	
	private float petHeight;
	
	public PetModel _petModel;
	int attackNumber;
	int totalBuffTime = 0;
	
	bool _spiritEnabled;
	bool spiritEnabled
	{
		get
		{
			return _spiritEnabled;
		}
		set
		{
			if(_spiritEnabled == value)
				return;
			_spiritEnabled = value;
		}
	}
	
	void Awake()
	{
		_petStateController = gameObject.GetComponent<SRPetStateController>();
		_particleResolver = gameObject.GetComponent<ProjectileParticleResolver>();
	}
	
	void Start()
	{
		_petStateController._petStateListener = this;
		petHeight = gameObject.GetComponent<BoxCollider>().size.y;
		
		attackNumber = 6 - GameManager.instance.spiritProgress;//_petModel.buffTime;
//		gameObject.GetComponent<UIButtonMessage>().enabled = false;
		
		totalBuffTime = GameManager.instance.spiritProgress;
		spiritEnabled = false;
		
		EnableSpirit(true);
	}
	
	public void EnableSpirit(bool yesNo)
	{
		Debug.Log("SPIRIT ENABLE > T" );
		GameManager.instance.scaleformCamera.hud.SpiritEnable(true);
	}
	
	public void OnSetStart()
	{	
		if(!_petStateController.Ready)
		{
			totalBuffTime++;
			GameManager.instance.spiritProgress = totalBuffTime;
//			Debug.LogError("Spirit progress is being incremented = "+totalBuffTime+" attack number "+attackNumber);
			attackNumber--;
			if(attackNumber <= 0)
			{
				attackNumber = 6;//_petModel.buffTime;
				_petStateController.Ready = true;
				totalBuffTime = 10;
			}
		}
		if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::::::::::::::::::::::::::: UpdateBufftimeCalled!");
		//Invoke("UpdateBuffTime", 0.5f);
		UpdateBuffTime(GameManager.instance.spiritProgress);
//		UpdateBuffTime();
	}
	
	public void UpdateBuffTime(int givenBuffTime = 0)
	{
//		if(givenBuffTime != 0)
		totalBuffTime = givenBuffTime;
		GameManager.instance.scaleformCamera.hud.BattleSpirit(totalBuffTime);
	}


	public void CastSpell()
	{
		_petStateController.CastSpell();
	}
	
	
	#region Pet Interface methods
	public void OnReadyStateReached()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::::::::::::::::::::::::::: OnReadyStateReached()!!! attack Number => " + attackNumber);
		
		if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None)
		{
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.spiritFightTutorialStart);
			EnableSpirit(false);
		}
	}
	
	
	public void OnCastingStateReached()
	{
		Debug.Log("SPIRIT ENABLE > T" );
		GameManager.instance.scaleformCamera.hud.SpiritEnable(true);
		
		_spellObject = _particleResolver.GetParticleObjectForGestureWithPositionAndRotation(GestureEmitter.Gesture.kFire, spellSpawnPoint.position, transform.rotation);
		
		ParticleEmitter projectileParticleEmitter = _spellObject.GetComponent<ParticleEmitter>();
		float currentProjectileHeight = projectileParticleEmitter.maxSize;
		projectileParticleEmitter.maxSize = petHeight / (ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT+ 6) * currentProjectileHeight;
		projectileParticleEmitter.minSize = petHeight / (ProjectileParticleResolver.PROJECTILE_MAX_HEIGHT + 6) * currentProjectileHeight;
		
		_spellObject.transform.parent = spellSpawnPoint;
	}
	
	public void OnSummonStateReached()
	{
		CastSpell();
	}
	
	public void OnThrowStateReached()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("PetController :::: OnThrowStateReached");
		
		_spellObject.AddComponent<DeleteScript>();
		_spellObject.transform.localRotation = Quaternion.identity;
		TweenPosition.Begin(_spellObject, 0.3f, new Vector3(_spellObject.transform.localPosition.x, _spellObject.transform.localPosition.y, _spellObject.transform.localPosition.z + 400));

//		GameObject.Find("BattleManager").GetComponent<BattleManager>().OnSpiritThrow();
		GameManager.instance._levelManager.battleManager.OnSpiritThrow();
		totalBuffTime = 0;
	}
	#endregion
	
	public void Summon()
	{
		
	}
	
	public void OnSpiritTap()
	{
		if(TutorialManager.instance.state == TutorialManager.TutorialsAndCallback.spiritFightTutorial3)
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.spiritFightTutorialEnd);
		if(GameManager.PRINT_LOGS) Debug.Log("PetController :::: OnSpiritTap");
		_petStateController.Throw = true;
	}

	public bool PetThrowState() {
		return _petStateController.Throw;
	}
}
