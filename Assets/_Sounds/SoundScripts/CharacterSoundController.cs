using UnityEngine;
using System.Collections;

public class CharacterSoundController : MonoBehaviour {
	
	//Fire Sounds
	private SoundSourceAgent fireCastSound;
	private SoundSourceAgent fireChargingSound;
	private SoundSourceAgent fireImpactSound;
	
	//Water Sounds
	private SoundSourceAgent waterCastSound;
	private SoundSourceAgent waterChargingSound;
	private SoundSourceAgent waterImpactSound;
	
	//Earth Sounds
	private SoundSourceAgent earthCastSound;
	private SoundSourceAgent earthChargingSound;
	private SoundSourceAgent earthImpactSound;
	
	//Lightning Sounds
	private SoundSourceAgent lightningCastSound;
	private SoundSourceAgent lightningChargingSound;
	private SoundSourceAgent lightningImpactSound;
	
	//Damage Sounds
	private SoundSourceAgent damageSound;
	private SoundSourceAgent damageAltSound;
	
	//Death Sounds
	private SoundSourceAgent deathSound;
	
	//Focus Sounds
	private SoundSourceAgent focusSound_01;
	private SoundSourceAgent focusSound_02;
	private SoundSourceAgent focusSound_03;
	
	//Misc Sounds
	private SoundSourceAgent heartBeatSound;
	private SoundSourceAgent heavyBreathingSound;
	
	
	// Use this for initialization
	void Start () {
		fireCastSound = gameObject.transform.FindChild("Spells/Fire/s_Cast").gameObject.GetComponent<SoundSourceAgent>();
		fireChargingSound = gameObject.transform.FindChild("Spells/Fire/s_Charging").gameObject.GetComponent<SoundSourceAgent>();
		fireImpactSound = gameObject.transform.FindChild("Spells/Fire/s_Impact").gameObject.GetComponent<SoundSourceAgent>();
		fireChargingSound.LoopSound(true);
		//print("object name " + fireCastSound.gameObject.name);
		
		waterCastSound = gameObject.transform.FindChild("Spells/Water/s_Cast").gameObject.GetComponent<SoundSourceAgent>();
		waterChargingSound = gameObject.transform.FindChild("Spells/Water/s_Charging").gameObject.GetComponent<SoundSourceAgent>();
		waterImpactSound = gameObject.transform.FindChild("Spells/Water/s_Impact").gameObject.GetComponent<SoundSourceAgent>();
		waterChargingSound.LoopSound(true);
		
		earthCastSound = gameObject.transform.FindChild("Spells/Earth/s_Cast").gameObject.GetComponent<SoundSourceAgent>();
		earthChargingSound = gameObject.transform.FindChild("Spells/Earth/s_Charging").gameObject.GetComponent<SoundSourceAgent>();
		earthImpactSound = gameObject.transform.FindChild("Spells/Earth/s_Impact").gameObject.GetComponent<SoundSourceAgent>();
		earthChargingSound.LoopSound(true);
		
		lightningCastSound = gameObject.transform.FindChild("Spells/Lightning/s_Cast").gameObject.GetComponent<SoundSourceAgent>();
		lightningChargingSound = gameObject.transform.FindChild("Spells/Lightning/s_Charging").gameObject.GetComponent<SoundSourceAgent>();
		lightningImpactSound = gameObject.transform.FindChild("Spells/Lightning/s_Impact").gameObject.GetComponent<SoundSourceAgent>();
		lightningChargingSound.LoopSound(true);
		
		damageSound = gameObject.transform.FindChild("Damage/s_Damage_01").gameObject.GetComponent<SoundSourceAgent>();
		damageAltSound = gameObject.transform.FindChild("Damage/s_Damage_02").gameObject.GetComponent<SoundSourceAgent>();
		
		deathSound = gameObject.transform.FindChild("Death/s_Death").gameObject.GetComponent<SoundSourceAgent>();
		
		focusSound_01 = gameObject.transform.FindChild("Focus/s_Chanting_01").gameObject.GetComponent<SoundSourceAgent>();
		focusSound_02 = gameObject.transform.FindChild("Focus/s_Chanting_02").gameObject.GetComponent<SoundSourceAgent>();
		focusSound_03 = gameObject.transform.FindChild("Focus/s_Chanting_03").gameObject.GetComponent<SoundSourceAgent>();
		
		if(gameObject.transform.FindChild("Misc/s_HeavyBreathing") != null){
			//heartBeatSound = gameObject.transform.FindChild("Misc/s_HeartBeat").gameObject.GetComponent<SoundSourceAgent>();
			heavyBreathingSound = gameObject.transform.FindChild("Misc/s_HeavyBreathing").gameObject.GetComponent<SoundSourceAgent>();
			//heartBeatSound.LoopSound(true);
			heavyBreathingSound.LoopSound(true);
		}
		//RegisterAllListners();
	}
	
	// Misc Sounds - start
	/*
	public void PlayHeartBeatSound()
	{
		if(heartBeatSound != null){
			heartBeatSound.PlaySound();
		}
	}
	
	public void StopHeartBeatSound()
	{
		if(heartBeatSound != null){
			heartBeatSound.StopSound();
		}
	}
	*/
	
	public void PlayHeavyBreathingSound()
	{
		if(heavyBreathingSound != null){
			heavyBreathingSound.PlaySound();
		}
	}
	
	public void StopHeavyBreathingSound()
	{
		if(heavyBreathingSound != null){
			heavyBreathingSound.StopSound();
		}
	}
	// Misc Sounds - end
	
	
	// Fire Sounds - start
	public void PlayFireChargingSound()
	{
		fireChargingSound.PlaySound();
	}
	
	public void PlayFireCastSound()
	{
		fireChargingSound.StopSound();
		fireCastSound.PlaySound();
	}
	
	public void PlayFireImpactSound()
	{
		fireCastSound.StopSound();
		StopAllChargingSounds();
		fireImpactSound.PlaySound();
	}
	// Fire Sounds - end
	
	// Water Sounds - start
	public void PlayWaterChargingSound()
	{
		waterChargingSound.PlaySound();
	}
	
	public void PlayWaterCastSound()
	{
		waterChargingSound.StopSound();
		waterCastSound.PlaySound();
	}
	
	public void PlayWaterImpactSound()
	{
		waterCastSound.StopSound();
		StopAllChargingSounds();
		waterImpactSound.PlaySound();
	}
	// Water Sounds - end
	
	// Earth Sounds - start
	public void PlayEarthChargingSound()
	{
		earthChargingSound.PlaySound();
	}
	
	public void PlayEarthCastSound()
	{
		earthChargingSound.StopSound();
		earthCastSound.PlaySound();
	}
	
	public void PlayEarthImpactSound()
	{
		earthCastSound.StopSound();
		StopAllChargingSounds();
		earthImpactSound.PlaySound();
	}
	// Earth Sounds - end
	
	// Lightning Sounds - start
	public void PlayLightningChargingSound()
	{
		lightningChargingSound.PlaySound();
	}
	
	public void PlayLightningCastSound()
	{
		lightningChargingSound.StopSound();
		lightningCastSound.PlaySound();
	}
	
	public void PlayLightningImpactSound()
	{
		lightningCastSound.StopSound();
		StopAllChargingSounds();
		lightningImpactSound.PlaySound();
	}
	// Lightning Sounds - end
	
	// Damage Sounds - start
	public void PlayDamageSound(){
		StopAllChargingSounds();
		switch( Random.Range(0,2) ){
		case 0:
			damageSound.PlaySound();
			break;
		case 1:
			damageAltSound.PlaySound();
			break;
		default:
			damageSound.PlaySound();
			break;
		}
			
	}
	
	public void PlayDamageSound(int soundNo){
		StopAllChargingSounds();
		switch(soundNo){
		case 0:
			damageSound.PlaySound();
			break;
		case 1:
			damageAltSound.PlaySound();
			break;
		default:
			damageSound.PlaySound();
			break;
		}
	}
	// Damage Sounds - end
	
	
	// Death Sound - start
	public void PlayDeathSound(){
		StopAllChargingSounds();
		damageSound.StopSound();
		damageAltSound.StopSound();
		deathSound.PlaySound();
	}
	// Death Sound - end
	
	
	// Play Focus Sounds - start
	public void PlayFocusSound(){
		StopAllChargingSounds();
		switch( Random.Range(0,3) ){
		case 0:
			focusSound_01.PlaySound();
			break;
		case 1:
			focusSound_02.PlaySound();
			break;
		case 2:
			focusSound_03.PlaySound();
			break;
		default:
			focusSound_01.PlaySound();
			break;
		}
			
	}
	
	public void PlayFocusSound(int soundNo){
		StopAllChargingSounds();
		switch(soundNo){
		case 0:
			focusSound_01.PlaySound();
			break;
		case 1:
			focusSound_02.PlaySound();
			break;
		case 2:
			focusSound_03.PlaySound();
			break;
		default:
			focusSound_01.PlaySound();
			break;
		}
	}
	// Play Focus Sounds - end
	
	
	
	// Stop All Charging Sounds Currently Playing on Character - start
	public void StopAllChargingSounds()
	{
		lightningChargingSound.StopSound();
		fireChargingSound.StopSound();
		waterChargingSound.StopSound();
		earthChargingSound.StopSound();
	}
	// Stop All Charging Sounds Currently Playing on Character - end
	
	// Register All Listners - start
	public void RegisterAllListners()
	{
		fireCastSound.RegisterListener();
		fireChargingSound.RegisterListener();
		fireImpactSound.RegisterListener();
		//print("object name " + fireCastSound.gameObject.name);
		
		waterCastSound.RegisterListener();
		waterChargingSound.RegisterListener();
		waterImpactSound.RegisterListener();
		
		earthCastSound.RegisterListener();
		earthChargingSound.RegisterListener();
		earthImpactSound.RegisterListener();
		
		lightningCastSound.RegisterListener();
		lightningChargingSound.RegisterListener();
		lightningImpactSound.RegisterListener();
		
		damageSound.RegisterListener();
		damageAltSound.RegisterListener();
		
		deathSound.RegisterListener();
		
		focusSound_01.RegisterListener();
		focusSound_02.RegisterListener();
		focusSound_03.RegisterListener();
	}
	// Register All Listners - end

}
