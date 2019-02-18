using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;


public class SoundManager : Singleton<SoundManager> 
{

	public bool isSoundMuted = false;
	//	
	public float musicVolume;
	public float sfxVolume;
	//	
	static IDictionary jsonSoundsDictionary = new Dictionary<string, string>();
	private AudioClip audioClip;
	//	
	//	
	public delegate void MuteSoundEventHandler(bool isMuted);
	public event MuteSoundEventHandler SoundMuted;
	//	
	public delegate void SetMusicEventHandler(float musicVolume);
	public event SetMusicEventHandler SetMusicVolume;
	//	
	public delegate void SetSFXEventHandler(float sfxVolume);
	public event SetSFXEventHandler SetSFXVolume;
	//	
	public bool isSoundManagerReady = false;

	//TODO
	//Delete this!!! 
	public bool _MuteEditor;

	public void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		TextAsset	textFile = Resources.Load("SRMasteraudioSounds", typeof(TextAsset)) as TextAsset;
		LoadJson(textFile);


		#if UNITY_EDITOR
			if(_MuteEditor)
				MasterAudio.MuteEverything();
		#else

		#if UNITY_ANDROID
		QualitySettings.SetQualityLevel(4);
		#else
		if((iPhone.generation.ToString()).IndexOf("iPhone") > -1) {
			QualitySettings.SetQualityLevel(0);
		} else {
			QualitySettings.SetQualityLevel(4);
		}
		#endif

		#endif

	}

	protected override void Start()
	{
		base.Start ();
	}

	private void LoadJson(TextAsset textFile)
	{
		jsonSoundsDictionary = Json.Deserialize(textFile.text) as IDictionary;
		isSoundManagerReady = true;
	}

	public void MuteSound(bool isMuted)
	{
		if (isMuted) 
		{
			MasterAudio.MuteEverything ();
		}
		else 
		{
			MasterAudio.UnmuteEverything();
		}
	}
	
	public void SetMVolume(float mVolume)
	{
		MasterAudio.SetBusVolumeByName ("Background", mVolume);
		MasterAudio.SetBusVolumeByName ("Ambiance", mVolume);
	}
	
	public void SetSfxVolume(float sVolume)
	{
		MasterAudio.SetBusVolumeByName ("Sfx", sVolume);
	}

	public void changeBackgroundSoundTo (string soundID) 
	{
		MasterAudio.StopBus ("Background");
		MasterAudio.PlaySound (jsonSoundsDictionary[soundID].ToString());
	}
	
	public void changeAmbientSoundTo (string soundID) 
	{
		MasterAudio.StopBus ("Ambiance");
		MasterAudio.PlaySound (jsonSoundsDictionary[soundID].ToString());
	}
	
	public void changeBackgroundClipTo (string soundID) 
	{
		MasterAudio.StopBus ("Background");
		MasterAudio.PlaySound (jsonSoundsDictionary[soundID].ToString());
	}

	public void playBattleBackGround()
	{
		StopBackgroundSound ();
		StopAmbientSound ();
		MasterAudio.PlaySound("Battle Music Loop");
	}
	
	public void playDefaultBackGround()
	{
		StopBackgroundSound ();
		StopAmbientSound ();
		MasterAudio.PlaySound ("Beach Camp Loop");
		MasterAudio.PlaySound ("Beach Ambience");
	}
	
	public void playDownloadDefaultBackGround()
	{
		playDefaultBackGround ();
	}

	public void PlayAudioClipForId(string Id)
	{
		MasterAudio.PlaySound (jsonSoundsDictionary[Id].ToString());
	}

	public void StopBackgroundSound()
	{
		MasterAudio.StopBus ("Background");
	}

	public void StopBattleBackgroundSound()
	{
		MasterAudio.StopBus ("Background");
	}

	public void StopAmbientSound()
	{
		MasterAudio.StopBus ("Ambiance");
	}

	public void PauseAudioClipForId(string Id)
	{
		MasterAudio.PauseSoundGroup (jsonSoundsDictionary[Id].ToString());
	}


	//Menu Sounds
	public void PlayMenuOkSound()
	{
		MasterAudio.PlaySound ("SR_OK");
	}
	
	public void PlayMenuOpenSound()
	{
		MasterAudio.PlaySound ("SR_OPEN");
	}
	
	public void PlayMenuCancelSound()
	{
		MasterAudio.PlaySound ("SR_CANCEL");
	}
	
	public void PlayMenuTickSound()
	{
		MasterAudio.PlaySound ("SR_OK");
	}
	
	public void PlayMenuVictorySound()
	{
		MasterAudio.PlaySound ("SR_BATTLE_WIN");
	}
	
	public void PlayMenuDefeatSound()
	{
		MasterAudio.PlaySound ("SR_BATTLE_LOSS");
	}
	
	public void PlayTreasureOpenSound()
	{
		MasterAudio.PlaySound ("SR_CHEST_OPEN");
	}
	
	public void PlayTreasureLootSound()
	{
		MasterAudio.PlaySound ("SR_CHEST_LOOT");
	}
	
	public void PlayRatingSound()
	{
		MasterAudio.PlaySound ("SR_Rating");
	}
	
	public void PlayShrineActivateSound()
	{
		MasterAudio.PlaySound ("sr_shrine_v2");
	}

	public void PlayScrollSound()
	{
		MasterAudio.PlaySound ("SR_SCROLL");
	}

	public void PlayHealthLowSound(bool play)
	{
		if(play)
			MasterAudio.PlaySound ("SR_LOW_HP");
		else
			MasterAudio.StopBus("Health");
	}

	public AudioClip GetAudioClipForId(string Id)
	{
		//if(GameManager.PRINT_LOGS) Debug.Log("GetAudioClipForId - OUT - " + Id);
		string audioClipPath = GetAudioPathForId(Id);
		
		if(audioClipPath != null)
		{
			AudioClip audioClip =Resources.Load(audioClipPath) as AudioClip;
			return audioClip;
		}
		else{
			if(GameManager.PRINT_LOGS) Debug.Log("audioClip not found for Path: " + audioClipPath);
			return null;
		}
	}
	//	
	//	
	private string GetAudioPathForId(string Id)
	{
		//if(GameManager.PRINT_LOGS) Debug.Log("GetAudioPathForId === " + Id);
		
		string audioClipPath = jsonSoundsDictionary[Id].ToString();
		
		if(audioClipPath != null)
		{
			return audioClipPath;
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("audioPath not found for Id: " + Id);
			return null;
		}
		
	}
	
	bool isSoundMuteTest;

//	
//	// Ambient, Background and Battle Sounds
//	public SoundSourceAgent ambientPlayer;
//	public SoundSourceAgent backGroundPlayer;
//	public SoundSourceAgent battleBackGroundPlayer;
//	/*
//	// Ambient, Background and Battle Sounds
//	private SoundSourceAgent ambient_Beach;
//	private SoundSourceAgent ambient_MagicalForest;
//	private SoundSourceAgent background_Beach;
//	private SoundSourceAgent background_MagicalForest;
//	private SoundSourceAgent background_Battle;
//	*/
//	// Menu Sounds
//	private SoundSourceAgent menuOkSound;
//	private SoundSourceAgent menuOpenSound;
//	private SoundSourceAgent menuCancelSound;
//	private SoundSourceAgent menuTickSound;
//	private SoundSourceAgent menuVictorySound;
//	private SoundSourceAgent menuDefeatSound;
//	private SoundSourceAgent menuTreasureOpenSound;
//	private SoundSourceAgent menuLootChestSound;
//	private SoundSourceAgent menuRatingSound;
//	private SoundSourceAgent menuShrineActivateSound;
	

//	
//	public void MuteSound(bool isMuted)
//	{
//		if (SoundMuted != null)
//		{
//        	SoundMuted (isMuted);
//		}
//	}
//	
//	public void SetMVolume(float mVolume)
//	{
//		if (SetMusicVolume != null)
//		{
//			musicVolume = mVolume;
//        	SetMusicVolume (mVolume);
//		}
//	}
//	
//	public void SetSfxVolume(float sVolume)
//	{
//		if (SetSFXVolume != null)
//		{
//			sfxVolume = sVolume;
//        	SetSFXVolume (sVolume);
//		}
//	}
//	

//	
//	// Use this for initialization
//	void Awake()
//	{	
//		DontDestroyOnLoad(this.gameObject);
//		TextAsset	textFile = Resources.Load("SRSounds", typeof(TextAsset)) as TextAsset;
//		//jsonSoundsDictionary = Json.Deserialize(textFile.text) as IDictionary;
//		LoadJson(textFile);
//		//if(GameManager.PRINT_LOGS) Debug.Log(textFile);
//		
//	/*		
//		ambientPlayer=GameObject.Find("s_Ambient").GetComponent<SoundSourceAgent>();
//		backGroundPlayer=GameObject.Find("s_Background").GetComponent<SoundSourceAgent>();
//		battleBackGroundPlayer=GameObject.Find("s_Background_Battle").GetComponent<SoundSourceAgent>();
//		
//		menuOkSound=GameObject.Find("Menu/s_Ok").GetComponent<SoundSourceAgent>();
//		menuOpenSound=GameObject.Find("Menu/s_Open").GetComponent<SoundSourceAgent>();
//		menuCancelSound=GameObject.Find("Menu/s_Cancel").GetComponent<SoundSourceAgent>();
//		menuTickSound=GameObject.Find("Menu/s_Tick").GetComponent<SoundSourceAgent>();
//		menuVictorySound=GameObject.Find("Menu/s_Victory").GetComponent<SoundSourceAgent>();
//		menuDefeatSound=GameObject.Find("Menu/s_Defeat").GetComponent<SoundSourceAgent>();
//		menuTreasureOpenSound=GameObject.Find("Menu/s_TreasureOpen").GetComponent<SoundSourceAgent>();
//		menuLootChestSound=GameObject.Find("Menu/s_LootChest").GetComponent<SoundSourceAgent>();
//		menuRatingSound=GameObject.Find("Menu/s_Rating").GetComponent<SoundSourceAgent>();
//	*/
//	}
//	
//	protected override void Start()
//	{
//		base.Start();
//	
//		
////		MuteSound(isSoundMuted);
////		isSoundMuteTest = isSoundMuted;
////		//playDefaultBackGround();
////		backGroundPlayer.RegisterListener();
////		ambientPlayer.RegisterListener();
////		battleBackGroundPlayer.RegisterListener();
////		
////		isSoundManagerReady = true;
//	}
//	
//	public void changeBackgroundSoundTo (string soundID) {
//		backGroundPlayer.PlaySound(soundID);
//		StartCoroutine(backGroundPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		
//	}
//	
//	public void changeAmbientSoundTo (string soundID) {
//		ambientPlayer.PlaySound(soundID);
//		StartCoroutine(ambientPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		
//	}
//	
//	public void changeBackgroundClipTo (string soundID) {
//		backGroundPlayer.ChangeClip(soundID);
//	}
//	
//	public void playBattleBackGround()
//	{
//		if(GameManager.PRINT_LOGS) Debug.Log("Play Battle SOund in Start");
//		//AudioClip audioClip1=Resources.Load(battleSound) as AudioClip;
//	//	if(GameManager.PRINT_LOGS) Debug.Log("Audio NAme----"+audioClip1.name);
//		//backGroundPlayer.clip = audioClip1;
//		//backGroundPlayer.Play();
//		//backGroundPlayer.StopSound();
//		StartCoroutine(backGroundPlayer.FadeAudio(3.0f,   SoundSourceAgent.Fade.Out, 0.0f, 1.0f));
//		
//		backGroundPlayer.UnRegisterListener();
//		battleBackGroundPlayer.RegisterListener();
//		ambientPlayer.RegisterListener();
//		
//		//battleBackGroundPlayer.SoundVolume(0.6f);
//		//ambientPlayer.SoundVolume(0.3f);
//		
//		battleBackGroundPlayer.PlaySound();
//		ambientPlayer.PlaySound();
//		
//		StartCoroutine(battleBackGroundPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//	}
//	
//	public void playDefaultBackGround()
//	{
//		
//		if(GameManager.PRINT_LOGS) Debug.Log("Play bg SOund in Start");
//		battleBackGroundPlayer.UnRegisterListener();
//		StartCoroutine(battleBackGroundPlayer.FadeAudio(3.0f,   SoundSourceAgent.Fade.Out, 0.0f, 1.0f));
//		
//		backGroundPlayer.RegisterListener();
//		ambientPlayer.RegisterListener();
//		
//		//backGroundPlayer.SoundVolume(0.8f);
//		//ambientPlayer.SoundVolume(0.4f);
//		
//		backGroundPlayer.PlaySound();
//		ambientPlayer.PlaySound();
//		
//		StartCoroutine(backGroundPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		StartCoroutine(ambientPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		//StartCoroutine(FadeAudio(6.0f, Fade.In, 0.0f, 1.0f));
//		
//		
//		//testing
//		//Invoke("testSoundChange",4.0f);
//		
//	}
//	
//	public void playDownloadDefaultBackGround()
//	{
//		
//		if(GameManager.PRINT_LOGS) Debug.Log("Play bg SOund in Start");
//		battleBackGroundPlayer.UnRegisterListener();
//		StartCoroutine(battleBackGroundPlayer.FadeAudio(3.0f,   SoundSourceAgent.Fade.Out, 0.0f, 1.0f));
//		
//		backGroundPlayer.RegisterListener();
//		//ambientPlayer.RegisterListener();
//		
//		//backGroundPlayer.SoundVolume(0.8f);
//		//ambientPlayer.SoundVolume(0.4f);
//		
//		backGroundPlayer.PlaySound();
//		//ambientPlayer.PlaySound();
//		
//		StartCoroutine(backGroundPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		//StartCoroutine(ambientPlayer.FadeAudio(6.0f,   SoundSourceAgent.Fade.In, 0.0f, 1.0f));
//		//StartCoroutine(FadeAudio(6.0f, Fade.In, 0.0f, 1.0f));
//		
//		
//		//testing
//		//Invoke("testSoundChange",4.0f);
//		
//	}
//	
//	void testSoundChange() {
//		//backGroundPlayer.PlaySound("background_battle");
//		//PlayAudioClipForId("background_battle");
//	}
//	
//	
//	void Update()
//	{
//		if(isSoundMuteTest !=  isSoundMuted){
//			isSoundMuteTest = isSoundMuted;
//			MuteSound(isSoundMuted);
//			//PlayAudioClipForId("background_battle");
//		}
//	}
//	
//	public void PlayAudioClipForId(string Id)
//	{
//		if(GameManager.PRINT_LOGS) Debug.Log("playing audio clip: " + Id);
//		/*GameObject audioObject = new GameObject("s_"+Id);
//		audioObject.AddComponent<SoundSourceAgent>();
//		audioObject.audio.clip = GetAudioClipForId(Id);
//		audioObject.audio.PlayOneShot(GetAudioClipForId(Id));*/
//		if(!isSoundMuted){
//			if(GameManager.PRINT_LOGS) Debug.Log("PLaying Audio Clip at Point " + Id);
//			AudioSource.PlayClipAtPoint(GetAudioClipForId(Id),new Vector3(0,0,0));
//		}
//	}
//	
//	// Menu Sounds
//	public void PlayMenuOkSound(){
//		menuOkSound.PlaySound();
//	}
//	
//	public void PlayMenuOpenSound(){
//		menuOpenSound.PlaySound();
//	}
//	
//	public void PlayMenuCancelSound(){
//		menuCancelSound.PlaySound();
//	}
//	
//	public void PlayMenuTickSound(){
//		menuTickSound.PlaySound();
//	}
//	
//	public void PlayMenuVictorySound(){
//		menuVictorySound.PlaySound();
//	}
//	
//	public void PlayMenuDefeatSound(){
//		menuDefeatSound.PlaySound();
//	}
//	
//	public void PlayTreasureOpenSound(){
//		menuTreasureOpenSound.PlaySound();
//	}
//	
//	public void PlayTreasureLootSound(){
//		menuLootChestSound.PlaySound();
//	}
//	
//	public void PlayRatingSound(){
//		menuRatingSound.PlaySound();
//	}
//	
//	public void PlayShrineActivateSound(){
//		menuShrineActivateSound.PlaySound();
//	}
}
