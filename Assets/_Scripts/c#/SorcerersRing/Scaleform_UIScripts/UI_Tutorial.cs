using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Tutorial : MonoBehaviour {
	
	//Value tutorialSwf = null;
	//Value NewTutorialSwf = null;
	//ScaleformCamera sfCamera = null;
	public int swapCount = 0;
	public bool ringTutorialatEnd = false;
	
	public void Start()
	{
		UIManager.instance.uiLevelTutorialSwf = this;
	}
	
	/*
	// Required to implement this constructor.
	public UI_Tutorial(string swfPath, SFManager sfmgr, SFMovieCreationParams cp): 
	base(sfmgr,new MovieDef(swfPath), cp)
	{
		//sfCamera = parent;
		AdvanceWhenGamePaused = true;
	}
	
	public void OnRegisterSWFCallback(Value movieRef)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Tutorial SWF loaded.");
		tutorialSwf = movieRef;
		if(GameManager.instance != null)
			SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);

		//loadTutorialSwf();

		this.SetDepth(10); // Tutorials Layer
		//ToturialManager.showTutorial();
		//ShrineTutorialStart();
	}
	
	
	public void OnRegisterUISWFCallback(Value movieRef)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Tutorial SWF loaded.");
		tutorialSwf = movieRef;
		if(GameManager.instance != null)
			SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);
		
		loadTutorialSwf();
		
		this.SetDepth(10); // Tutorials Layer
		//ToturialManager.showTutorial();
		//ShrineTutorialStart();
	}
	//public void missingString(string missingThings)
	//{
	//	if(Debug.isDebugBuild)
	//	{
	//		GameManager.instance.scaleformCamera.generalSwf.words +=missingThings+"\n";
	//		if(GameManager.PRINT_LOGS) Debug.Log("Word Recieved :::: "+missingThings+" Current Words :::"+GameManager.instance.scaleformCamera.generalSwf.words);
	//	}
	//}
	
	
	public void OnRegisterSWFCallbackShrine(Value movieRef)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Tutorial Shrine SWF loaded.");
		tutorialSwf = movieRef;
		this.SetDepth(10); // Tutorials Layer
		//ToturialManager.showTutorial();
		
		if(GameManager.instance != null)
			SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);


		if(GameManager.instance._levelManager.currentLevel=="StatuePath")
		{
			TutorialManager.instance.decideInLevelTutorial(false);
		}
		else
		{
			TutorialManager.instance.decideInLevelTutorial(true);
		}
	}
	
	
	*/
	private void SetLanguage(string languageString, string charSetID)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("SetLanguage Tutorial Swf:::::::::::::::::::::::::::::::::::::::::: " + languageString);
		/*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		tutorialSwf.Invoke("setLanguage",args1);
		*/
		TutorialManager.instance.SetLanguage(languageString, charSetID);
	}
	
	

	public void loadTutorialSwf() //I think it is for rings... but dont know :/
	{
		/*
		Value[] tutorialIdValArr = {};	
		tutorialSwf.Invoke("loadTutorial", tutorialIdValArr);	
		*/
		TutorialManager.instance.loadTutorialSwf();
	}

	#region Ring Tutuorials
	public void onRingTutorialStart()
	{
		/*
		tutorialSwf = movieRef;
		//SFDisplayInfo tempDis =  tutorialSwf.GetDisplayInfo();
		//tempDis.Z = 100;
		//tutorialSwf.SetDisplayInfo(tempDis);
		this.SetDepth(10);
		Value tutorialIdVal = new Value("RingTutorial2", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial 2");
		
		movieRef.Invoke("playTutorial", tutorialIdValArr);	
		*/
		TutorialManager.instance.RingTutorial2();
	}
	
	public void onRingTutorial2()
	{
		/*
		tutorialSwf = movieRef;
		//SFDisplayInfo tempDis =  tutorialSwf.GetDisplayInfo();
		//tempDis.Z = 100;
		this.SetDepth(10);
		//tutorialSwf.SetDisplayInfo(tempDis);
		Value tutorialIdVal = new Value("RingTutorial3", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial 3");
		movieRef.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		TutorialManager.instance.RingTutorial3();
	}
	
	public void onRingTutorial3()
	{
		/*
		tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("RingTutorial4", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial 4");
		movieRef.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		TutorialManager.instance.RingTutorial4();
	}
	
	public void onRingTutorial4()
	{
		TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.RingTutorialCompleted, true);
		
		GameManager.instance.scaleformCamera.generalSwf.closeLoadedUI();
		GameManager.instance.scaleformCamera.generalSwf.resumeGame();
	}
	
	public void ShowRingTutorial3()
	{
		/*
		//tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("RingTutorial3", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial 3");
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		TutorialManager.instance.RingTutorial3();
		
	}
	
	public void ShowRingTutorial4()
	{
		ringTutorialatEnd = true;
		
		/*
		//tutorialSwf = movieRef;
		
		Value tutorialIdVal = new Value("RingTutorial4", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial 4");
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		TutorialManager.instance.RingTutorial4();
	}
	
	public void ShowRingTutorialEnd()
	{
		ringTutorialatEnd = true;
		
		/*
		//tutorialSwf = movieRef;
		
		Value tutorialIdVal = new Value("RingTutorialEnd", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		if(GameManager.PRINT_LOGS) Debug.Log("start ring tutorial End");
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		TutorialManager.instance.RingTutorialEnd();
	}
	
	public void closeUIScreen(string tutorialID)
	{
		//if(tutorialSwf!=null)
		//{
			if(GameManager.PRINT_LOGS) Debug.Log("Screen Closed-" + tutorialID);
		//	Value tutorialIdVal = new Value(tutorialID, MovieID);	
		//	Value[] tutorialIdValArr = {tutorialIdVal};	
		//	tutorialSwf.Invoke("UiResponse",tutorialIdValArr);
		//}
		
		TutorialManager.instance.closeUIScreen(tutorialID);
	}
	#endregion
	
	#region Level Tutorial
	public void onLevelTutorial()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("onLevelTutorial CB on unity side! >> TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted) >> "
			+ TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted)
			 + "GameManager.instance.scaleformCamera.uiLevelTutorialSwf != null" + (GameManager.instance.scaleformCamera.uiLevelTutorialSwf != null));
		
//		this.SetDepth(10);
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted))
		{
			Time.timeScale = 1.0f;
			GameManager.instance.scaleformCamera.isPaused = false;
			TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted, true);
			GameManager.instance.SaveGameState(false);
			InputWrapper.disableTouch = false;
		}	
	}
	#endregion
	
	#region Rune Tutorial
	public void onRuneTutorialStart()
	{
		/*
		tutorialSwf = movieRef;
		this.SetDepth(10);
		Value tutorialIdVal = new Value("RuneTutorialEnd", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("on rune tutorial start");
		
		TutorialManager.instance.RuneTutorialEnd();
	}
	
	public void onRuneTutorialEnd()
	{
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.RuneTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiRuneTutorialSwf != null)
		{
			TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.RuneTutorialCompleted, true);
			GameManager.instance.SaveGameState(false);

			Analytics.logEvent(Analytics.EventName.RuneUI_Tutorial_End);
		}	
		if(GameManager.PRINT_LOGS) Debug.Log("on rune tutorial end");
	}
	#endregion
	
	#region Rune Tutorial
	public void onUpgradeTutorialStart()
	{
		/*
		this.SetDepth(10);
		tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("UpgradeTutorialEnd", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("on upgrade tutorial start");
		
		TutorialManager.instance.UpgradeTutorialEnd();
	}
	
	public void onUpgradeTutorialEnd()
	{
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.UpgradeTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiUpgradeTutorialSwf != null)
		{
			TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.UpgradeTutorialCompleted, true);
			GameManager.instance.SaveGameState(false);
		}	
		if(GameManager.PRINT_LOGS) Debug.Log("on upgrade tutorial end");
	}
	#endregion
	
	#region Transmutation Tutorial
	
	// Where is the call to start transmutation tutorial ??? :@ 
	
	
	public void onTransmutationTutorialStart()
	{
		/*
		tutorialSwf = movieRef;
		this.SetDepth(10);
		Value tutorialIdVal = new Value("TransmutationTutorial2", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("on Transmutation tutorial start");
		
		TutorialManager.instance.TransmutationTutorial2();
	}
	
	public void ShowTransmutationTutorial3()
	{
		/*
		//tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("TransmutationTutorial3", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/
		
		if(GameManager.PRINT_LOGS) Debug.Log("on Transmutation tutorial 3");
		
		TutorialManager.instance.TransmutationTutorial3();
	}
	
	public void ShowTransmutationTutorial4()
	{
		/*
		//tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("TransmutationTutorial4", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("on Transmutation tutorial 4");
		
		TutorialManager.instance.TransmutationTutorial4();
	}
	
	public void ShowTransmutationTutorialEnd()
	{
		/*
		//tutorialSwf = movieRef;
		Value tutorialIdVal = new Value("TransmutationTutorialEnd", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};	
		
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);	
		*/ 
		if(GameManager.PRINT_LOGS) Debug.Log("on Transmutation tutorial end");
		TutorialManager.instance.TransmutationTutorialEnd();
	}
	
	public void onTransmutationTutorialEnd()
	{
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted)
			&& GameManager.instance.scaleformCamera.uiTransmutationTutorialSwf != null)
		{
			TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.TransmutationTutorialCompleted, true);
			GameManager.instance.SaveGameState(false);
			Analytics.logEvent(Analytics.EventName.TransmuteUI_Tutorial_End);
		}	
		if(GameManager.PRINT_LOGS) Debug.Log("on transmutation tutorial end");
	}
	#endregion
	
	#region PotionTutorial 
	public void PotionTutorialStart()
	{
		/*
		tutorialSwf = movieRef;
		this.SetDepth(10);
		Value tutorialIdVal = new Value("potionTutorialStart", MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		*/
		
		TutorialManager.instance.potionTutorialStart();
	}
	public void PotionTutorial2()
	{
		/*
		if(tutorialSwf!=null)
		{
			Value tutorialIdVal = new Value("potionTutorial2", MovieID);	
			Value[] tutorialIdValArr = {tutorialIdVal};
			tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		}
		*/
		
		TutorialManager.instance.PotionTutorial2();
	}
	
	public void PotionTutorialEnd()
	{
		/*
		if(tutorialSwf!=null)
		{
			Value tutorialIdVal = new Value("potionTutorialEnd", MovieID);	
			Value[] tutorialIdValArr = {tutorialIdVal};
			tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		}
		*/
		TutorialManager.instance.PotionTutorialEnd();
	}
	
	public void onPotionTutorialEnd()
	{
		if(TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.PotionTutorialStart)
		{
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.PotionTutorialCompleted);
		}
	}
	
	#endregion
	
	public void NexFightTutorialEnd()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("NexFightTutorialEnd");
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.NexFightStart))
		{
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexFightTutorialEnd);
			//TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.NexFightStart,true);
			Time.timeScale=1.0f;
			GameManager.instance.scaleformCamera.isPaused=false;
			//GameManager.instance.scaleformCamera.DestroyShrineTutorial();
		}
	}
	
	public void NexFightTutorialCameraEnd()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("NexFightTutorialCameraEnd");
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexFightTutorialCameraEnd);
	}
	
	public void SpiritTutorialStart()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("SpiritTutorialStart OUTSIDE IF");
		
		/*
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.SpiritTutorialStart) && GameManager.instance.scaleformCamera.NewTutorialSwf!=null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("SpiritTutorialStart INSIDE IF");
			Value[] valarr	= {};
			tutorialSwf.Invoke("SpiritTutorialStart",valarr);
			Time.timeScale=0.0f;
			GameManager.instance.scaleformCamera.isPaused=true;
		}
		*/
		
		TutorialManager.instance.SpiritTutorialStart();
	}
	
	public void onSpiritTutorialEnd()
	{
		
		if(GameManager.PRINT_LOGS) Debug.Log("::::::: onSpiritTutorialEnd OUTSIDE IF");
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.SpiritTutorialStart))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("::::::: onSpiritTutorialEnd INSIDE IF");
			
			TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.SpiritTutorialStart,true);
			Time.timeScale=1.0f;
			GameManager.instance.scaleformCamera.isPaused=false;
		}
	}
	
	public void NexFightTutorialStart()
	{
		/*
		Value[] valarr= {};
		tutorialSwf.Invoke("NexFightTutorialStart",valarr);
		*/
		
		TutorialManager.instance.NexFightTutorialStart();
		
		Time.timeScale=0.0f;
		GameManager.instance.scaleformCamera.isPaused=true;
		
		
	}
	
	public void NexCameraTutorial()
	{
		/*
		Value[] valarr= {};
		tutorialSwf.Invoke("NexCamera",valarr);
		*/
		
		TutorialManager.instance.NexCamera();
	}
	
	public void NexCameraTutorialEnd()
	{
		/*
		Value[] valarr= {};
		tutorialSwf.Invoke("NexCameraEnd",valarr);
		*/
		
		TutorialManager.instance.NexCameraEnd();
	}
	
	public void PlaySpellTutorial(string tutorialId)
	{
		/*
		Value tutorialIdVal = new Value(tutorialId, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("spellTutorial", tutorialIdValArr);
		*/
		
		TutorialManager.instance.spellTutorial(tutorialId);
	}
	
	
	public void PlayTutorial(string tutorialId)
	{
		/*
		Value tutorialIdVal = new Value(tutorialId, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("playTutorial", tutorialIdValArr);
		*/
		
		TutorialManager.instance.playTutorial(tutorialId);
	}
	
	public void CombatResponse(string tutorialId)
	{
		/*
		Value tutorialIdVal = new Value(tutorialId, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		tutorialSwf.Invoke("CombatResponse", tutorialIdValArr);
		*/
		
		//Don't know what to do here!!!
	}
	
	//ShrineTutorial -- from New Tutorial # code added by Mufti //
	public void ShowFindShrine()
	{
		Debug.Log("ShowFindShrine!!!!!!!!!>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
		
		/*
		Value[] args = {};
		tutorialSwf.Invoke("ShrineTutorial2", args);
		*/
		
		TutorialManager.instance.ShrineTutorial2();
	}
	
	public void RemoveFindShrine()
	{
		/*
		Value[] args = {};
		tutorialSwf.Invoke("removeFindShrine", args);
		*/
		
		TutorialManager.instance.removeFindShrine();
	}
	
	public void ShrineTutorialStart(string text)
	{
		Debug.Log("ShrineTutorialStart >>>>>>>>>>>>>> " + text);
		/*
		Value textval = new Value(text, MovieID);	
		Value[] args = {textval};
		tutorialSwf.Invoke("ShrineTutorialStart", args);
		*/
		
		TutorialManager.instance.ShrineTutorialStart(text);
	}
	public void ShrineTutorialStartEnd()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("ShrineTutorialStartEnd CALLED !!!");
		if(!GameManager.instance._levelManager.currentLevel.Equals("ThreeGods"))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("Calling ShowFindShrine()");
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted))
			ShowFindShrine();
		}
		else
		{
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LightningShrineTutorialCompleted))
			{
				Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
				analParams.Add (Analytics.ParamName.ShrineLevelName, GameManager.instance._levelManager.currentLevel);
				Analytics.logEvent(Analytics.EventName.Shrine_Tutorial_End, analParams);

				TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.LightningShrineTutorialCompleted,true);
				GameManager.instance.SaveGameState(false);
			}

	
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted) && GameManager.instance._levelManager.currentLevel.Equals("ForbiddenCave"))
			{
				Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
				analParams.Add (Analytics.ParamName.ShrineLevelName, GameManager.instance._levelManager.currentLevel);
				Analytics.logEvent(Analytics.EventName.Shrine_Tutorial_End, analParams);
		
				TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted,true);

				GameManager.instance.SaveGameState(false);
			}
		}
		GameManager.instance.scaleformCamera.isPaused = false;
		
	}
	public void ShrineTutorial3()
	{
		TutorialManager.instance.ShrineTutorial3();
	}
	
	public void ShrineTutorial4()
	{
		TutorialManager.instance.ShrineTutorial4();
	}
	
	public void ShrineTutorial5()
	{
		TutorialManager.instance.ShrineTutorial5(UIManager.instance.generalSwf.onShrineTutorialEnd);
	}
	
	public void ShrineTutorialEnd()
	{
		TutorialManager.instance.ShrineTutorialEnd();
	}

	
	//ShrineTutorial -- from New Tutorial # code added by Mufti //
	

	//for lootTutorial -- from New Tutorial # code added by mufti /// 
	
	public void ArcaneKeysStart()
	{
		TutorialManager.instance.ArcaneKeysStart();
	}
	public void ArcaneKeys2()
	{
		TutorialManager.instance.ArcaneKeys2();
	}
	public void ArcaneKeys3()
	{
		TutorialManager.instance.ArcaneKeys3();
	}
	public void ArcaneKeysEnd()
	{
		Debug.Log("UI_TUTORIAL >> ArcaneKeysEnd");
	
		TutorialManager.instance.ArcaneKeysEnd();
	}
	
	//for lootTutorial -- from New Tutorial # code added by mufti /// 
	
	
	public void onWaterTutorialStart()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnWaterTutorialStart);
	}
	
	public void onWaterTutorial2()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnWaterTutorial2);
	}
	
	public void onWaterTutorial3()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnWaterTutorial3);
	}
	
	public void onWaterTutorialEnd()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnWaterTutorialEnd);
	}
	
	public void onFireTutorialStart()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnFireTutorialStart);
	}
	
	public void onFireTutorial2()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnFireTutorial2);
	}
	
	public void onFireTutorialEnd()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnFireTutorialEnd);
	}
	
	public void onLightningTutorialStart()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnLightningTutorialStart);
	}
	
	public void onLightningTutorial2()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnLightningTutorial2);
	}
	
	public void onLightningTutorialEnd()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnLightningTutorialEnd);
	}
	
	public void onEarthTutorialStart()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEarthTutorialStart);
	}
	
	public void onEarthTutorial2()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEarthTutorial2);
	}
	
	public void onEarthTutorialEnd()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEarthTutorialEnd);
	}
	#region enemy buff tutorial callbacks
	public void onSpellAmplify()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellAmplify);
	}
	
	public void onSpellHaste()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellHaste);
	}
	
	public void onSpellLock()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellLock);
	}
	
	public void onSpellPrison()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellPrison);
	}
	
	public void onSpellRegen()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellRegen);
	}
	
	public void onSpellShield()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellShield);
	}
	#endregion
	#region hero buffs tutorial callbacks
	public void onSpellIgnite()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellIgnite);
	}
	
	public void onSpellLeechseed()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellLeechseed);
	}
	
	public void onSpellDrain()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellDrain);
	}
	
	public void onSpellDaze()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpellDaze);
	}
	#endregion
	public void onEnemyElementFire()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEnemyElementFire);
	}
	
	public void onEnemyElementEarth()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEnemyElementEarth);
	}
	
	public void onEnemyElementWater()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEnemyElementWater);
	}
	
	public void onEnemyElementLightning()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnEnemyElementLightning);
	}
	
	public void onNoCounter()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnNoCounter);
	}
	
	public void onDazeRune()
	{
		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnDazeRune);
	}
	
	public void onFirstStun()
	{
		TutorialManager.instance.ShowStunTutorial(TutorialManager.TutorialsAndCallback.OnFirstStun);
	}
	
	public void onSecondStunStart()
	{
		TutorialManager.instance.ShowStunTutorial(TutorialManager.TutorialsAndCallback.OnSecondStunStart);
	}
	
	public void onSecondStunEnd()
	{
		TutorialManager.instance.ShowStunTutorial(TutorialManager.TutorialsAndCallback.OnSecondStunEnd);
	}
	
	public void SetMask(float x, float y)
	{
		/*
		Value xVal = new Value(x, MovieID);	
		Value yVal = new Value(y, MovieID);	
		Value[] tutorialIdValArr = {xVal, yVal};
		
		//if(GameManager.PRINT_LOGS) Debug.Log("");
		tutorialSwf.Invoke("setMask", tutorialIdValArr);
		*/
		TutorialManager.instance.setMask(x,y);
	}
	
	public void RemoveMask()
	{
		//tutorialSwf.Invoke("removeMask");
		
		TutorialManager.instance.removeMask();
	}
}
