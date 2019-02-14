using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
 
public class TutorialManager : Singleton<TutorialManager>{
	
	public const string FILE_NAME = "TutorialStatus.txt";
	public Dictionary<string, object> tutorialStatusDictionary = new Dictionary<string, object>();
	
	private List<TutorialsAndCallback> queuedTutorials = new List<TutorialsAndCallback>();
	
	public bool isGamePause = false;
	
	public UI_Tutorial tutorialUIScript;
	
	public TutorialsAndCallback currentTutorial = TutorialsAndCallback.None;
	public TutorialsAndCallback state = TutorialsAndCallback.Idle;
	public TutorialsAndCallback queuedTutorial = TutorialsAndCallback.None;
	
	private List<Tutorial> manualTutorials;
	
	void Start()
	{
		manualTutorials = new List<Tutorial>();
	}
	
	public enum TutorialsAndCallback{
		None,
		Idle,
		WaterTutorialStart,
		OnWaterTutorialStart,
		WaterTutorial2,
		OnWaterTutorial2,
		WaterTutorial3,
		OnWaterTutorial3,
		WaterTutorialEnd,
		OnWaterTutorialEnd,
		FireTutorialStart,
		OnFireTutorialStart,
		FireTutorial2,
		OnFireTutorial2,
		FireTutorialEnd,
		OnFireTutorialEnd,
		LightningTutorialStart,
		OnLightningTutorialStart,
		LightningTutorial2,
		OnLightningTutorial2,
		LightningTutorialEnd,
		OnLightningTutorialEnd,
		EarthTutorialStart,
		OnEarthTutorialStart,
		EarthTutorial2,
		OnEarthTutorial2,
		EarthTutorialEnd,
		OnEarthTutorialEnd,

		#region Enemy buffs
		SpellAmplify,
		onSpellAmplify,
		SpellHaste,
		onSpellHaste,
		SpellLock,
		onSpellLock,
		SpellPrison,
		onSpellPrison,
		SpellRegen,
		onSpellRegen,
		SpellShield,
		onSpellShield,
		#endregion
		
		#region Hero buffs
		SpellIgnite,
		onSpellIgnite,
		SpellLeechseed,
		onSpellLeechseed,
		SpellDrain,
		onSpellDrain,
		SpellDaze,
		onSpellDaze,
		#endregion
		#region Not known where to show these tutorials
		EnemyElementFire,
		OnEnemyElementFire,
		EnemyElementEarth,
		OnEnemyElementEarth,
		EnemyElementWater,
		OnEnemyElementWater,
		EnemyElementLightning,
		OnEnemyElementLightning,
		NoCounter,
		OnNoCounter,
		DazeRune,
		OnDazeRune,
		HeroChargeBuff,
		#endregion
		#region Stun tutorials
		FirstStun,
		OnFirstStun,
		SecondStunStart,
		OnSecondStunStart,
		SecondStunEnd,
		OnSecondStunEnd,
		SecondStunTryAgain,
		OnSecondStunTryAgain,
		#endregion
		
		#region UI-Ring Tutorial
		RingTutorialStart,
		RingTutorial2_3x2,
		RingTutorial2_4x3,
		RingTutorial2_16x9,
		RingTutorial3_3x2,
		RingTutorial3_4x3,
		RingTutorial3_16x9,
		RingTutorialComplete,
		RingTutorialCompleted,
		#endregion
		#region UI-Level Tutorial
		LevelTutorialStart_3x2,
		LevelTutorialStart_4x3,
		LevelTutorialStart_16x9,
		OnLevelTutorialStart,
		LevelTutorialComplete_3x2,
		LevelTutorialComplete_4x3,
		LevelTutorialComplete_16x9,
		LevelTutorialCompleted,
		#endregion
		#region UI-Rune Tutorial
		OnRuneTutorialStart,
		OnRuneTutorialEnd,
		RuneTutorialCompleted,
		#endregion
		#region UI-Upgrade Tutorial
		UpgradeTutorialStart,
		OnUpgradeTutorialStart,
		UpgradeTutorialCompleted,
		OnUpgradeTutorialEnd,
		#endregion
		#region UI-Transmutation Tutorial
		TransmutationTutorialStart,
		TransmutationTutorial2,
		TransmutationTutorial2_3x2,
		TransmutationTutorial2_4x3,
		TransmutationTutorial2_16x9,
		TransmutationTutorial3,
		TransmutationTutorial3_3x2,
		TransmutationTutorial3_4x3,
		TransmutationTutorial3_16x9,
		TransmutationTutorial4,
		TransmutationTutorial4_3x2,
		TransmutationTutorial4_4x3,
		TransmutationTutorial4_16x9,
		TransmutationTutorialEnd,
		TransmutationTutorialEnd_3x2,
		TransmutationTutorialEnd_4x3,
		TransmutationTutorialEnd_16x9,
		TransmutationTutorialCompleted,
		#endregion
		#region shrineTutorial Tutorial
		ShrineHelpTutorial,
		ShrineTutorial3,
		ShrineTutorial4,
		ShrineTutorial5,
		EarthShrineTutorialCompleted,
		LightningShrineTutorialCompleted,
		FireShrineTutorialCompleted,
		WaterShrineTutorialCompleted,
		#endregion
		#region arcane Tutorial
		arcKeysTutorialCompleted,		
		#endregion
		#region BattleEndCollectPowerRing Tutorial
		BattleEndTutorialStart,
		BattleEndTutorial2,
		BattleEndTutorialComplete,		
		BattleEndTutorialCompleted,		
		#endregion
		#region PotionTutorial Tutorial
		PotionTutorialStart,
		PotionTutorial2,
		PotionTutorialCompleted,
		PotionTutorialEnd,
		#endregion
		#region Rune Attack Tutorial
		runeSpellTutorialStart,
		OnRuneSpellTutorialStart,
		runeSpellTutorial2,
		OnRuneSpellTutorial2,
		runeSpellTutorialEnd,
		OnRuneSpellTutorialEnd,
		#endregion
		#region Spell Burst tutorial
		spellBurstTutorialStart,
		OnSpellBurstTutorialStart,
		spellBurstTutorialEnd,
		OnSpellBurstTutorialEnd,
		#endregion
		
		#region Nex Tutorial
		NexCamera,
		NexFightStart,
		NexFightTutorialEnd,
		NexCameraEnd,
		NexFightTutorialCameraEnd,
		//NexFightMove,
		//NexFightMoveEnd,
		#endregion
		
		#region Spirit Tutorial
		SpiritTutorialStart,
		onSpiritTutorialEnd,
		
		spiritFightTutorialStart,
		spiritFightTutorial2,
		spiritFightTutorial3,
		spiritFightTutorialEnd,
		#endregion
		
		findTreasure
		
	};
	
	
	private bool AddTutorialToQueue(TutorialsAndCallback tutorialId)
	{
		if(tutorialId == TutorialsAndCallback.WaterTutorialStart ||
			tutorialId == TutorialsAndCallback.FireTutorialStart ||
			tutorialId == TutorialsAndCallback.LightningTutorialStart ||
			tutorialId == TutorialsAndCallback.EarthTutorialStart ||
			tutorialId == TutorialsAndCallback.SpellAmplify ||
			tutorialId == TutorialsAndCallback.SpellHaste ||
			tutorialId == TutorialsAndCallback.SpellLock ||
			tutorialId == TutorialsAndCallback.SpellPrison ||
			tutorialId == TutorialsAndCallback.SpellRegen ||
			tutorialId == TutorialsAndCallback.SpellDaze ||
			tutorialId == TutorialsAndCallback.SpellDrain ||
			tutorialId == TutorialsAndCallback.SpellLeechseed ||
			//tutorialId == TutorialsAndCallback.HeroBuffGreed ||
			tutorialId == TutorialsAndCallback.PotionTutorialStart ||
			tutorialId == TutorialsAndCallback.SpellIgnite)
		{
			
			Debug.Log("QUEUED >>" + tutorialId);
			queuedTutorials.Add(tutorialId);
			return true;
		}
		return false;
	}
	
	
	private void RemoveQueuedTutorial()
	{
		if(queuedTutorials.Count >0)
		{
			TutorialsAndCallback tutorialId = queuedTutorials.ToArray()[0];
			
			queuedTutorials.RemoveAt(0);
			
			if(tutorialId == TutorialsAndCallback.SecondStunStart || 
				tutorialId == TutorialsAndCallback.OnSecondStunStart ||
				tutorialId == TutorialsAndCallback.SecondStunEnd ||
				tutorialId == TutorialsAndCallback.OnSecondStunEnd)
				ShowStunTutorial(tutorialId);
			else 
				ShowTutorial(tutorialId);
		}
	}
	
	public void ShowTutorial(TutorialsAndCallback tutorialId)
	{
		ShowTutorial(tutorialId, 0);
	}
	
	public void ShowTutorial(TutorialsAndCallback tutorialId, int param)
	{
//		Debug.Log("ShowTutorial > " + tutorialId);
		if(!IsTutorialCompleted(tutorialId))
		{
			Debug.Log("ShowTutorial > " + tutorialId);
			
			if(currentTutorial != TutorialsAndCallback.None)
			{
				if(currentTutorial.ToString().StartsWith("WaterTutorial") || currentTutorial.ToString().StartsWith("OnWaterTutorial"))
					if(!(tutorialId.ToString().StartsWith("WaterTutorial") || tutorialId.ToString().StartsWith("OnWaterTutorial")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("FireTutorial") || currentTutorial.ToString().StartsWith("OnFireTutorial"))
					if(!(tutorialId.ToString().StartsWith("FireTutorial") || tutorialId.ToString().StartsWith("OnFireTutorial")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("LightningTutorial") || currentTutorial.ToString().StartsWith("OnLightningTutorial"))
					if(!(tutorialId.ToString().StartsWith("LightningTutorial") || tutorialId.ToString().StartsWith("OnLightningTutorial")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("EarthTutorial") || currentTutorial.ToString().StartsWith("OnEarthTutorial"))
					if(!(tutorialId.ToString().StartsWith("EarthTutorial") || tutorialId.ToString().StartsWith("OnEarthTutorial")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("spellBurstTutorial") || currentTutorial.ToString().StartsWith("OnSpellBurstTutorial"))
					if(!(tutorialId.ToString().StartsWith("spellBurstTutorial") || tutorialId.ToString().StartsWith("OnSpellBurstTutorial")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("Spell") || currentTutorial.ToString().StartsWith("onSpell"))
					if(!(tutorialId.ToString().StartsWith("Spell") || tutorialId.ToString().StartsWith("onSpell")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("spellBurst") || currentTutorial.ToString().StartsWith("OnSpellBurst"))
					if(!(tutorialId.ToString().StartsWith("spellBurst") || tutorialId.ToString().StartsWith("OnSpellBurst")))
						return;
					
				else if(currentTutorial.ToString().StartsWith("runeSpell") || currentTutorial.ToString().StartsWith("OnRuneSpell"))
					if(!(tutorialId.ToString().StartsWith("runeSpell") || tutorialId.ToString().StartsWith("OnRuneSpell")))
						return;
				
				else if(currentTutorial.ToString().StartsWith("spiritFight") || currentTutorial.ToString().StartsWith("onSpiritFight"))
					if(!(tutorialId.ToString().StartsWith("spiritFight") || tutorialId.ToString().StartsWith("onSpiritFight")))
						return;	
					
				if(AddTutorialToQueue(tutorialId))
					return;
			}
			
			state = tutorialId;
			switch(tutorialId)
			{
//				case TutorialsAndCallback.NexFightStart:
//					currentTutorial = tutorialId;
//					PlayTutorial(tutorialId);
//				break;
				case TutorialsAndCallback.NexFightTutorialEnd:
					//AddTutorialIdWithStatus(TutorialsAndCallback.NexFightStart, true);
					//SaveTutorialStatus();
					state = TutorialsAndCallback.NexFightTutorialEnd;
					currentTutorial = TutorialsAndCallback.NexFightStart;
					ResumeGame();
					//PlayTutorial(TutorialsAndCallback.NexCamera);
					NexCamera();
				break;
				case TutorialsAndCallback.NexCameraEnd:
					//SaveTutorialStatus();
					state = TutorialsAndCallback.NexCameraEnd;
					currentTutorial = TutorialsAndCallback.NexFightStart;
					//PlayTutorial(tutorialId);
					EndNexCameraTutorial();
				break;
				case TutorialsAndCallback.NexFightTutorialCameraEnd:
				Debug.Log("TutorialsAndCallback.NexFightTutorialCameraEnd");
				
					AddTutorialIdWithStatus(TutorialsAndCallback.NexFightStart, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
				break;
				case TutorialsAndCallback.WaterTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, tutorialUIScript.onWaterTutorialStart);
					Analytics.logEvent(Analytics.EventName.BattleWater_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnWaterTutorialStart:
					ShowTutorial(TutorialsAndCallback.WaterTutorial2);
					break;
				case TutorialsAndCallback.WaterTutorial2:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.auto, tutorialUIScript.onWaterTutorial2);
					break;
				case TutorialsAndCallback.OnWaterTutorial2:
					UIManager.instance._mask.RemoveMask();
					ShowTutorial(TutorialsAndCallback.WaterTutorialEnd);
					break;
				case TutorialsAndCallback.WaterTutorial3: //Unused i think
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, tutorialUIScript.onWaterTutorial3);
					break;
				case TutorialsAndCallback.OnWaterTutorial3:
					ShowTutorial(TutorialsAndCallback.WaterTutorialEnd); //Unused i think
					break;
				case TutorialsAndCallback.WaterTutorialEnd:
					//PlayTutorial(tutorialId);
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, tutorialUIScript.onWaterTutorialEnd);
					break;
				case TutorialsAndCallback.OnWaterTutorialEnd:
					if(GameManager.PRINT_LOGS) Debug.Log("WATER TUTORIAL ENDED!!!");
					AddTutorialIdWithStatus(TutorialsAndCallback.WaterTutorialStart, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.BattleWater_Tutorial_End);
					break;
				case TutorialsAndCallback.FireTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, tutorialUIScript.onFireTutorialStart);
					Analytics.logEvent(Analytics.EventName.BattleFire_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnFireTutorialStart:
					ShowTutorial(TutorialsAndCallback.FireTutorial2);
					break;
				case TutorialsAndCallback.FireTutorial2:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.auto, tutorialUIScript.onFireTutorial2);
					break;
				case TutorialsAndCallback.OnFireTutorial2:
					UIManager.instance._mask.RemoveMask();
					ShowTutorial(TutorialsAndCallback.FireTutorialEnd);
					break;
				case TutorialsAndCallback.FireTutorialEnd:
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, tutorialUIScript.onFireTutorialEnd);
					break;
				case TutorialsAndCallback.OnFireTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.FireTutorialStart, true);
					Analytics.logEvent(Analytics.EventName.BattleFire_Tutorial_End);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					break;
				case TutorialsAndCallback.LightningTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, tutorialUIScript.onLightningTutorialStart);
					Analytics.logEvent(Analytics.EventName.BattleLightning_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnLightningTutorialStart:
					ShowTutorial(TutorialsAndCallback.LightningTutorial2);
					break;
				case TutorialsAndCallback.LightningTutorial2:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.auto, tutorialUIScript.onLightningTutorial2);
					break;
				case TutorialsAndCallback.OnLightningTutorial2:
					UIManager.instance._mask.RemoveMask();
					ShowTutorial(TutorialsAndCallback.LightningTutorialEnd);
					break;
				case TutorialsAndCallback.LightningTutorialEnd:
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, tutorialUIScript.onLightningTutorialEnd);
					break;
				case TutorialsAndCallback.OnLightningTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.LightningTutorialStart, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.BattleLightning_Tutorial_End);
					break;
				case TutorialsAndCallback.EarthTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, tutorialUIScript.onEarthTutorialStart);
					Analytics.logEvent(Analytics.EventName.BattleEarth_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnEarthTutorialStart:
					ShowTutorial(TutorialsAndCallback.EarthTutorial2);
					break;
				case TutorialsAndCallback.EarthTutorial2:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.auto, tutorialUIScript.onEarthTutorial2);
					break;
				case TutorialsAndCallback.OnEarthTutorial2:
					UIManager.instance._mask.RemoveMask();
					ShowTutorial(TutorialsAndCallback.EarthTutorialEnd);
					break;
				case TutorialsAndCallback.EarthTutorialEnd:
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, tutorialUIScript.onEarthTutorialEnd);
					break;
				case TutorialsAndCallback.OnEarthTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.EarthTutorialStart, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.BattleEarth_Tutorial_End);
					break;
				#region enemy buffs
				case TutorialsAndCallback.SpellAmplify:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleAmplify_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellHaste:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleHaste_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellLock:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleLock_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellShield:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleShield_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellRegen:
					Debug.Log("TutorialsAndCallback.SpellRegen");
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleRegen_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellPrison:
					PlaySpellTutorial(tutorialId);
					break;
				case TutorialsAndCallback.onSpellAmplify:
					EndSpellTutorial(TutorialsAndCallback.SpellAmplify);
					break;
				case TutorialsAndCallback.onSpellHaste:
					EndSpellTutorial(TutorialsAndCallback.SpellHaste);
					break;
				
				case TutorialsAndCallback.onSpellLock:
					EndSpellTutorial(TutorialsAndCallback.SpellLock);
					break;
				
				case TutorialsAndCallback.onSpellShield:
					EndSpellTutorial(TutorialsAndCallback.SpellShield);
					break;
				
				case TutorialsAndCallback.onSpellPrison:
					EndSpellTutorial(TutorialsAndCallback.SpellLock);
					break;
				
				case TutorialsAndCallback.onSpellRegen:
					EndSpellTutorial(TutorialsAndCallback.SpellRegen);
					break;
				#endregion
				
				#region Hero Buffs
				case TutorialsAndCallback.SpellIgnite:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleIgnite_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellLeechseed:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleLeechSeed_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellDrain:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleDrain_Tutorial_Start);
					break;
				case TutorialsAndCallback.SpellDaze:
					PlaySpellTutorial(tutorialId);
					Analytics.logEvent(Analytics.EventName.BattleDaze_Tutorial_Start);
					break;
				case TutorialsAndCallback.onSpellIgnite:
					EndSpellTutorial(TutorialsAndCallback.SpellIgnite);
					break;
				case TutorialsAndCallback.onSpellLeechseed:
					EndSpellTutorial(TutorialsAndCallback.SpellLeechseed);
					break;
				case TutorialsAndCallback.onSpellDrain:
					EndSpellTutorial(TutorialsAndCallback.SpellDrain);
					break;
				case TutorialsAndCallback.onSpellDaze:
					EndSpellTutorial(TutorialsAndCallback.SpellDaze);
					break;
				#endregion
				
				#region RingTutorial
				case TutorialsAndCallback.RingTutorialStart:
					currentTutorial = tutorialId;
					float cameraAspect = (float)Screen.width/(float)Screen.height;
					Debug.Log("CameraAspect ==> " + cameraAspect);
					if(cameraAspect > 1.3f && cameraAspect < 1.45)
					{
						//Condition 2
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_4x3));
					}
					else if(cameraAspect >= 1.45f && cameraAspect < 1.7f)
					{
						//Condition 3
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_3x2));
					}
					else 
					{
						//Condition 1
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_16x9));
					}
					
					/*
					if(cameraAspect > 1.7f && cameraAspect < 1.8f)
					{
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_16x9));
					}
					else if(cameraAspect > 1.3f && cameraAspect < 1.4f)
					{
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_4x3));
					}
					else if(cameraAspect > 1.45f && cameraAspect < 1.55f)
					{
						PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => ShowTutorial(TutorialsAndCallback.RingTutorial2_3x2));
					}
					*/
					
					break;

				case TutorialsAndCallback.RingTutorial2_3x2:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorial2_4x3:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorial2_16x9:
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorial3_3x2:
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial2_3x2);
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorial3_4x3:
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial2_4x3);
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorial3_16x9:
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial2_16x9);
					PlayTutorial(tutorialId,Tutorial.CallbackMech.manual, () => {});//ShowTutorial(TutorialsAndCallback.RingTutorialComplete));
					break;

				case TutorialsAndCallback.RingTutorialComplete:
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial3_3x2);
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial3_4x3);
					DestroyManualTutorial(TutorialsAndCallback.RingTutorial3_16x9);
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.RingTutorialCompleted));
					break;
				
				case TutorialsAndCallback.RingTutorialCompleted:
					
					//DestroyManualTutorial(TutorialsAndCallback.RingTutorialComplete);
					AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.RingTutorialCompleted, true);
					//GameManager.instance.scaleformCamera.DestroyRingTutorial();	
					
					UIManager.instance.generalSwf.closeLoadedUI();
					UIManager.instance.generalSwf.resumeGame();
					InputWrapper.disableTouch = false;
					if(GameManager.PRINT_LOGS) Debug.Log("RingTutorial Completed!!!");
					break;
					
				#endregion

				#region Rune UI Tutorial

				case TutorialsAndCallback.RuneTutorialCompleted:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, ()=> ShowTutorial (TutorialsAndCallback.OnRuneTutorialStart));
					break;

				case TutorialsAndCallback.OnRuneTutorialStart:
					ShowTutorial(TutorialsAndCallback.OnRuneTutorialEnd);
					break;

				case TutorialsAndCallback.OnRuneTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.RuneTutorialCompleted, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.RuneUI_Tutorial_End);
					break;
				
				#endregion

				#region Upgrade Tutorial
				case TutorialsAndCallback.UpgradeTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.OnUpgradeTutorialStart));
					Analytics.logEvent(Analytics.EventName.UpgradesUI_Tutorial_Start);
					break;

				case TutorialsAndCallback.OnUpgradeTutorialStart:
					ShowTutorial(TutorialsAndCallback.UpgradeTutorialCompleted);
					break;

				case TutorialsAndCallback.UpgradeTutorialCompleted:
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.OnUpgradeTutorialEnd));
					break;

				case TutorialsAndCallback.OnUpgradeTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.UpgradeTutorialStart, true);
					AddTutorialIdWithStatus(TutorialsAndCallback.UpgradeTutorialCompleted, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.UpgradesUI_Tutorial_End);
					break;


				#endregion
				
				#region Rune spell attack Tutorial
				case TutorialsAndCallback.runeSpellTutorialStart:
					currentTutorial = TutorialsAndCallback.runeSpellTutorialStart;
					state = TutorialsAndCallback.runeSpellTutorialStart;
					//GameManager.instance.scaleformCamera.hud.RuneSpellTutorialStart();
					RuneSpellTutorialStart();
					PauseGame();
					Analytics.logEvent(Analytics.EventName.RuneSpell_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnRuneSpellTutorialStart:
					currentTutorial = TutorialsAndCallback.runeSpellTutorialStart;
					state = tutorialId;
					break;
				case TutorialsAndCallback.runeSpellTutorial2:
					currentTutorial = TutorialsAndCallback.runeSpellTutorialStart;
					state = tutorialId;

					//ResumeGame();
					//PlayBattleTutorial(TutorialsAndCallback.runeSpellTutorial2);
					//GameManager.instance.scaleformCamera.hud.RuneSpellTutorial2();
					RuneSpellTutorial2();
					break;
				case TutorialsAndCallback.OnRuneSpellTutorial2:
					currentTutorial = TutorialsAndCallback.runeSpellTutorialStart;
					state = tutorialId;
					break;
				case TutorialsAndCallback.runeSpellTutorialEnd:
					//PlayBattleTutorial(TutorialsAndCallback.runeSpellTutorial2);
					//GameManager.instance.scaleformCamera.hud.RuneSpellTutorialEnd();
					RuneSpellTutorialEnd();
					break;
				case TutorialsAndCallback.OnRuneSpellTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.runeSpellTutorialStart, true);
					SaveTutorialStatus();
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					ResumeGame();
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.RuneSpell_Tutorial_End);
					break;
				#endregion
				
				#region spell burst tutorial
				case TutorialsAndCallback.spellBurstTutorialStart:
					currentTutorial = TutorialsAndCallback.spellBurstTutorialStart;
					state = TutorialsAndCallback.spellBurstTutorialStart;
					//GameManager.instance.scaleformCamera.hud.spellBurstTutorialStart(param);
				
					PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, null, () => ShowTutorial(TutorialsAndCallback.OnSpellBurstTutorialStart));
					Analytics.logEvent(Analytics.EventName.SpellBurst_Tutorial_Start);
					break;
				case TutorialsAndCallback.OnSpellBurstTutorialStart:
					currentTutorial = TutorialsAndCallback.spellBurstTutorialStart;
					state = tutorialId;
					break;
				case TutorialsAndCallback.spellBurstTutorialEnd:
					currentTutorial = TutorialsAndCallback.spellBurstTutorialStart;
					state = tutorialId;
					//GameManager.instance.scaleformCamera.hud.spellBurstTutorialEnd();
					DestroyManualTutorial(TutorialsAndCallback.spellBurstTutorialStart, () => ShowTutorial(TutorialsAndCallback.OnSpellBurstTutorialEnd));
					break;
				case TutorialsAndCallback.OnSpellBurstTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.spellBurstTutorialStart, true);
					SaveTutorialStatus();
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.SpellBurst_Tutorial_End);
					break;
				#endregion
				#region potionTutorial
				case TutorialsAndCallback.PotionTutorialStart:
					currentTutorial = TutorialsAndCallback.PotionTutorialStart;
					state = TutorialsAndCallback.PotionTutorialStart;
					GameManager._gameState.User._inventory.potionBelt.AddPotion(GameManager._dataBank.GetPotionForPotionID("HEALTH_POTION"));
					//GameManager.instance.scaleformCamera.loadUIPotionTutorial();
					PlayTutorial(TutorialsAndCallback.PotionTutorialStart, Tutorial.CallbackMech.manual, () => {});
					Analytics.logEvent(Analytics.EventName.LowHealth_Tutorial_Start);
					break;
				case TutorialsAndCallback.PotionTutorialCompleted:
					DestroyManualTutorial(TutorialsAndCallback.PotionTutorial2);
					if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.PotionTutorialStart))
					{
						PlayTutorial(TutorialsAndCallback.PotionTutorialEnd, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.PotionTutorialEnd));
					}
					break;
				case TutorialsAndCallback.PotionTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.PotionTutorialStart, true);
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					RemoveQueuedTutorial();
					Analytics.logEvent(Analytics.EventName.LowHealth_Tutorial_End);
					InputWrapper.disableTouch = false;
					break;
				#endregion 
				#region spirit fight Tutorials
				case TutorialsAndCallback.spiritFightTutorialStart:
					currentTutorial = TutorialsAndCallback.spiritFightTutorialStart;
					state = TutorialsAndCallback.spiritFightTutorialStart;
//					PlayBattleTutorial(currentTutorial);
					PlayTutorial(TutorialsAndCallback.spiritFightTutorialStart, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.spiritFightTutorial2));
					PauseGame();
					break;
				case TutorialsAndCallback.spiritFightTutorial2:
					state = TutorialsAndCallback.spiritFightTutorial2;
					PlayTutorial(TutorialsAndCallback.spiritFightTutorial2, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.spiritFightTutorial3));
					break;
				case TutorialsAndCallback.spiritFightTutorial3:
					state = TutorialsAndCallback.spiritFightTutorial3;
					PlayTutorial(TutorialsAndCallback.spiritFightTutorial3, Tutorial.CallbackMech.manual, () => {});
					break;
				case TutorialsAndCallback.spiritFightTutorialEnd:
					if(!IsTutorialCompleted(TutorialsAndCallback.spiritFightTutorialStart))
					{
						DestroyAllManualTutorials();
						AddTutorialIdWithStatus(TutorialsAndCallback.spiritFightTutorialStart, true);
						SaveTutorialStatus();
						currentTutorial = TutorialsAndCallback.None;
						state = TutorialsAndCallback.Idle;
						RemoveQueuedTutorial();
						ResumeGame();
					}
					break;
				
				#endregion
				/*case TutorialsAndCallback.NexFightMove:
					currentTutorial = TutorialsAndCallback.NexFightMove;
					state = TutorialsAndCallback.NexFightMove;
					
					break;
				
				case TutorialsAndCallback.NexFightMoveEnd:
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
				
					AddTutorialIdWithStatus(TutorialsAndCallback.NexFightMove, true);
				
					SaveTutorialStatus();
					break;
					*/
				#region Spirit normal UI Tutorial
				case TutorialsAndCallback.SpiritTutorialStart:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => ShowTutorial(TutorialsAndCallback.onSpiritTutorialEnd));
					break;

				case TutorialsAndCallback.onSpiritTutorialEnd:
					AddTutorialIdWithStatus(TutorialsAndCallback.SpiritTutorialStart, true);
					SaveTutorialStatus();
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					RemoveQueuedTutorial();
					break;
				#endregion
					
				#region Transmutation
			case TutorialsAndCallback.TransmutationTutorialStart:
				currentTutorial = tutorialId;
				state = tutorialId;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => 
				{
					state = TutorialsAndCallback.TransmutationTutorial2;
					if(UIManager.instance.isResolution3by2)
						ShowTutorial(TutorialsAndCallback.TransmutationTutorial2_3x2);
					else if(UIManager.instance.isResolution4by3)
						ShowTutorial(TutorialsAndCallback.TransmutationTutorial2_4x3);
					else if(UIManager.instance.isResolution16by9)
						ShowTutorial(TutorialsAndCallback.TransmutationTutorial2_16x9);
				});
				break;
			case TutorialsAndCallback.TransmutationTutorial3:
				state = tutorialId;
				if(UIManager.instance.isResolution3by2)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial3_3x2);
				else if(UIManager.instance.isResolution4by3)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial3_4x3);
				else if(UIManager.instance.isResolution16by9)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial3_16x9);
				break;
			case TutorialsAndCallback.TransmutationTutorial4:
				state = tutorialId;
				//DestroyAllManualTutorials();
				if(UIManager.instance.isResolution3by2)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial4_3x2);
				else if(UIManager.instance.isResolution4by3)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial4_4x3);
				else if(UIManager.instance.isResolution16by9)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorial4_16x9);
				break;
			case TutorialsAndCallback.TransmutationTutorialEnd:
				state = tutorialId;
				//DestroyAllManualTutorials();
				if(UIManager.instance.isResolution3by2)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorialEnd_3x2);
				else if(UIManager.instance.isResolution4by3)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorialEnd_4x3);
				else if(UIManager.instance.isResolution16by9)
					ShowTutorial(TutorialsAndCallback.TransmutationTutorialEnd_16x9);
				break;
			case TutorialsAndCallback.TransmutationTutorial2_3x2:
				state = TutorialsAndCallback.TransmutationTutorial2;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial2_4x3:
				state = TutorialsAndCallback.TransmutationTutorial2;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial2_16x9:
				state = TutorialsAndCallback.TransmutationTutorial2;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial3_3x2:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial2_3x2);
				state = TutorialsAndCallback.TransmutationTutorial3;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial3_4x3:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial2_4x3);
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial3);
				state = TutorialsAndCallback.TransmutationTutorial3;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial3_16x9:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial2_16x9);
				state = TutorialsAndCallback.TransmutationTutorial3;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial4_3x2:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial3_3x2);
				state = TutorialsAndCallback.TransmutationTutorial4;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial4_4x3:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial3_4x3);
				state = TutorialsAndCallback.TransmutationTutorial4;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorial4_16x9:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial3_16x9);
				state = TutorialsAndCallback.TransmutationTutorial4;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorialEnd_3x2:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial4_3x2);
				state = TutorialsAndCallback.TransmutationTutorialEnd;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorialEnd_4x3:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial4_4x3);
				state = TutorialsAndCallback.TransmutationTutorialEnd;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorialEnd_16x9:
				DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorial4_16x9);
				state = TutorialsAndCallback.TransmutationTutorialEnd;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {});
				break;
			case TutorialsAndCallback.TransmutationTutorialCompleted:
				
				if(UIManager.instance.isResolution3by2)
					DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorialEnd_3x2);
				else if(UIManager.instance.isResolution4by3)
					DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorialEnd_4x3);
				else if(UIManager.instance.isResolution16by9)
					DestroyManualTutorial(TutorialsAndCallback.TransmutationTutorialEnd_16x9);

				currentTutorial = TutorialsAndCallback.None;
				state = TutorialsAndCallback.Idle;
				AddTutorialIdWithStatus(TutorialsAndCallback.TransmutationTutorialCompleted, true);
				SaveTutorialStatus();
				break;
				#endregion
				
				
				#region Level tutorial NEW
				
			case TutorialsAndCallback.LevelTutorialStart_3x2:
				currentTutorial = tutorialId;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("OnLevelTutorialStart");
					
					ShowTutorial(TutorialsAndCallback.LevelTutorialComplete_3x2);
				});
				break;
			case TutorialsAndCallback.LevelTutorialComplete_3x2:
				currentTutorial = TutorialsAndCallback.LevelTutorialStart_3x2;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("LevelTutorialCompleted!!!");
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					
					AddTutorialIdWithStatus(TutorialsAndCallback.LevelTutorialCompleted, true);
					InputWrapper.disableTouch = false;
					SaveTutorialStatus();
				});
				break;

			case TutorialsAndCallback.LevelTutorialStart_4x3:
				currentTutorial = tutorialId;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("OnLevelTutorialStart");
					
					ShowTutorial(TutorialsAndCallback.LevelTutorialComplete_4x3);
				});
				break;
			case TutorialsAndCallback.LevelTutorialComplete_4x3:
				currentTutorial = TutorialsAndCallback.LevelTutorialStart_4x3;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("LevelTutorialCompleted!!!");
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					
					AddTutorialIdWithStatus(TutorialsAndCallback.LevelTutorialCompleted, true);
					InputWrapper.disableTouch = false;
					SaveTutorialStatus();
				});
				break;

			case TutorialsAndCallback.LevelTutorialStart_16x9:
				currentTutorial = tutorialId;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("OnLevelTutorialStart");
					
					ShowTutorial(TutorialsAndCallback.LevelTutorialComplete_16x9);
				});
				break;
			case TutorialsAndCallback.LevelTutorialComplete_16x9:
				currentTutorial = TutorialsAndCallback.LevelTutorialStart_16x9;
				state = tutorialId;
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {
					Debug.Log("LevelTutorialCompleted!!!");
					currentTutorial = TutorialsAndCallback.None;
					state = TutorialsAndCallback.Idle;
					
					AddTutorialIdWithStatus(TutorialsAndCallback.LevelTutorialCompleted, true);
					InputWrapper.disableTouch = false;
					SaveTutorialStatus();
				});
				break;

			case  TutorialsAndCallback.BattleEndTutorialStart:
				currentTutorial = TutorialsAndCallback.BattleEndTutorialStart;
				InputWrapper.disableTouch = true;
				PlayTutorial(tutorialId, Tutorial.CallbackMech.auto, () => {ShowTutorial(TutorialsAndCallback.BattleEndTutorial2);});
				break;
			
			case  TutorialsAndCallback.BattleEndTutorial2:
				
				currentTutorial = TutorialsAndCallback.BattleEndTutorialStart;
				//UIManager.instance.generalSwf.generalSwf.GetComponent<dfPanel>().BringToFront();
				UIManager.instance.generalSwf.generalSwf.GlowMenuIcon();
				
				
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {
					UIManager.instance.generalSwf.generalSwf.RemoveGlowMenuIcon();
//					GetComponent<dfPanel>().BringToFront();
				});
				break;
				
			case  TutorialsAndCallback.BattleEndTutorialComplete:
				
				currentTutorial = TutorialsAndCallback.BattleEndTutorialStart;
				
				
				PlayTutorial(tutorialId, Tutorial.CallbackMech.manual, () => {
				});
				break;
				
			case  TutorialsAndCallback.BattleEndTutorialCompleted:
				
				currentTutorial = TutorialsAndCallback.BattleEndTutorialStart;
				DestroyManualTutorial(TutorialsAndCallback.BattleEndTutorialComplete);
				
				AddTutorialIdWithStatus(TutorialsAndCallback.BattleEndTutorialCompleted, true);
				InputWrapper.disableTouch = false;
					
				
				break;
					
				#endregion
			}
		}
	}
	
	
	private void EndSpellTutorial(TutorialsAndCallback tutorialId)
	{
		AddTutorialIdWithStatus(tutorialId, true);
		SaveTutorialStatus();
		state = TutorialsAndCallback.Idle;
		currentTutorial = TutorialsAndCallback.None;
		RemoveQueuedTutorial();
		ResumeGame();
	}
		
	/*
	private void PlaySpellTutorial(TutorialsAndCallback tutorialId)
	{
		currentTutorial = tutorialId;
		state = tutorialId;
		PauseGame();
		
		//GameManager gameManager = GameManager.instance;
		//ScaleformCamera sfCamera = gameManager.scaleformCamera;
		//PauseGame();
		//GameManager.instance.scaleformCamera.tutorialSwf.PlaySpellTutorial(tutorialId.ToString());
		
		
	}
	*/
	
	private void PlaySpellTutorial(TutorialsAndCallback tutorialId)
	{
		if(currentTutorial != TutorialsAndCallback.None)
			return;
		Debug.Log("PlaySpellTutorial > " + tutorialId);
		currentTutorial = tutorialId;
		state = tutorialId;
		PlayTutorial(tutorialId,Tutorial.CallbackMech.auto,() => {
			EndSpellTutorial(tutorialId);
		});
		PauseGame();
	}
	
	public void AddTutorialIdWithStatus(TutorialsAndCallback tutorialId, bool tutorialStatus)
	{
		Debug.Log("AddTutorialIdWithStatus >> " + tutorialId.ToString());
		
		if(!tutorialStatusDictionary.ContainsKey(tutorialId.ToString()))
		{
			tutorialStatusDictionary.Add(tutorialId.ToString(), tutorialStatus);
		}
		else
		{
			tutorialStatusDictionary.Remove(tutorialId.ToString());
			tutorialStatusDictionary.Add(tutorialId.ToString(), tutorialStatus);
		}
			currentTutorial = TutorialsAndCallback.None;
        	GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kAll;
	}
	
//	public void PrintAllQueuedBuffs()
//	{
//		string s="";
//		foreach(TutorialsAndCallback tut in queuedTutorials)
//		{
//			s+= ":::::::::::"+tut+":::::::::::";
//		}
//		if(GameManager.PRINT_LOGS) Debug.Log(s);
//	}
	
	public bool IsTutorialCompleted(TutorialsAndCallback tutorialId)
	{
		bool isCompleted = false;
		object obj;
		if(tutorialStatusDictionary.TryGetValue(tutorialId.ToString(), out obj))
		{
			isCompleted = (bool)obj;
		}
	
		return isCompleted;
	}
	
	public void ShowBuffTutorial(TutorialsAndCallback tutorialId)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Show Buff ");
		if(!IsTutorialCompleted(tutorialId))
		{
			state = tutorialId;
			switch(tutorialId)
			{
				case TutorialsAndCallback.FirstStun:
					if(GameManager.PRINT_LOGS) Debug.Log("ENEMY CHARGED >>>>>>>>>>>>><<<<<<<<<<<<<<<< TUTORIAL MANAGER");
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId);
					//PauseGame();
					break;
				case TutorialsAndCallback.OnFirstStun:
					currentTutorial = tutorialId;
					PauseGame();
					PlayTutorial(tutorialId);
					break;
			}
		}
		
	}
	
	public void ShowStunTutorial(TutorialsAndCallback tutorialId = TutorialsAndCallback.SecondStunStart)
	{		
		if(!IsTutorialCompleted(tutorialId))
		{
			if(currentTutorial != TutorialsAndCallback.None)
			{
				if(AddTutorialToQueue(tutorialId))
					return;
			}
			
			state = tutorialId;
			switch(tutorialId)
			{
				case TutorialsAndCallback.FirstStun:
					currentTutorial = tutorialId;

					PlayTutorial(tutorialId);
					break;
				case TutorialsAndCallback.OnFirstStun:
				if(GameManager.PRINT_LOGS) Debug.Log("OnFirstSTUN CALLED ******************************************************************");
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					tutorialStatusDictionary.Add(TutorialsAndCallback.FirstStun.ToString(), true);
					SaveTutorialStatus();
					RemoveQueuedTutorial();
					//ResumeGame();
					break;
				case TutorialsAndCallback.SecondStunStart:
				/*
				 	if(GameManager.PRINT_LOGS) Debug.Log("On :::::: SECOND :::: STUN CALLED ******************************************************************");
					currentTutorial = tutorialId;
					//PauseGame();
					PlayTutorial(tutorialId);
					*/
					break;
				case TutorialsAndCallback.OnSecondStunStart:
					//state = TutorialsAndCallback.Idle;
					//currentTutorial = TutorialsAndCallback.None;
					//tutorialStatusDictionary.Add(TutorialsAndCallback.SecondStun.ToString(), true);
					//SaveTutorialStatus();
					//ResumeGame();
					/*ShowStunTutorial(TutorialsAndCallback.SecondStunEnd);*/
					break;
				case TutorialsAndCallback.SecondStunEnd:
					/*currentTutorial = tutorialId;
					//PauseGame();
					PlayTutorial(tutorialId);
					*/
					break;
				case TutorialsAndCallback.OnSecondStunEnd:
					state = TutorialsAndCallback.Idle;
					currentTutorial = TutorialsAndCallback.None;
					tutorialStatusDictionary.Add(TutorialsAndCallback.SecondStunStart.ToString(), true);
					SaveTutorialStatus();
					RemoveQueuedTutorial();
					//ResumeGame();
					break;
				case TutorialsAndCallback.SecondStunTryAgain:
					currentTutorial = tutorialId;
					PlayTutorial(tutorialId);
					break;
			}
		}
		else
		{
			if(tutorialId == TutorialsAndCallback.FirstStun)
				ShowStunTutorial(TutorialsAndCallback.SecondStunStart);
		}
	}
	
	public void CombatResponse()
	{
		switch(currentTutorial)
		{
			case TutorialsAndCallback.WaterTutorialStart:
				CombatResponse(TutorialsAndCallback.WaterTutorialEnd);
				break;
			case TutorialsAndCallback.FireTutorialStart:
				CombatResponse(TutorialsAndCallback.FireTutorialEnd);
				break;
			case TutorialsAndCallback.LightningTutorialStart:
				CombatResponse(TutorialsAndCallback.LightningTutorialEnd);
				break;
			case TutorialsAndCallback.EarthTutorialStart:
				CombatResponse(TutorialsAndCallback.EarthTutorialEnd);
				break;
		}
	}
	
	public void CombatResponse(TutorialsAndCallback tutorialId)
	{
		//GameManager gameManager = GameManager.instance;
		//ScaleformCamera sfCamera = gameManager.scaleformCamera;
		//PauseGame();
		//GameManager.instance.scaleformCamera.tutorialSwf.CombatResponse(tutorialId.ToString());
		
		DestroyManualTutorial(tutorialId);
	}
	
	private void DestroyAllManualTutorials(Action cb = null)
	{
		Debug.Log("!!! DestroyAllManualTutorials !!!");
		manualTutorials.ForEach((tut) => Tutorial.Destroy(tut, true));
		
		if(cb != null)
			cb();
	}
	
	public void DestroyManualTutorial(TutorialsAndCallback tutorialId, Action cb = null)
	{
		Tutorial tutorial = manualTutorials.Find((obj) => obj._tutorialId == tutorialId);
		if(tutorial != null)
			Tutorial.Destroy(tutorial, true);
		
		if(cb != null)
			cb();
	}
	
	public void EndNexCameraTutorial()
	{
		//GameManager.instance.scaleformCamera.NewTutorialSwf.NexCameraTutorialEnd();
		DestroyManualTutorial(TutorialsAndCallback.NexCamera, () => ShowTutorial(TutorialsAndCallback.NexFightTutorialCameraEnd));
	}

	private void PlayTutorial(TutorialsAndCallback tutorialId)
	{
		//GameManager gameManager = GameManager.instance;
		//ScaleformCamera sfCamera = gameManager.scaleformCamera;
		//PauseGame();
	//	GameManager.instance.scaleformCamera.tutorialSwf.PlayTutorial(tutorialId.ToString());
		
//		Tutorial tut = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/" + tutorialId) as GameObject).GetComponent<Tutorial>();
		
	}
	
	
	
	private void PlayTutorial(TutorialsAndCallback tutorialId, Tutorial.CallbackMech cbMech, Action cb, Action showCb = null)
	{
		Debug.Log("[PlayTutorial] tutorialId > " + tutorialId);
		//Tutorial tut = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/" + tutorialId) as GameObject).GetComponent<Tutorial>();

		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/" + tutorialId);
		Tutorial tut = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).GetComponent<Tutorial>();
		tut.Show(tutorialId, cbMech, cb, showCb);

		if(cbMech == Tutorial.CallbackMech.manual)
			manualTutorials.Add(tut);
		Resources.UnloadUnusedAssets();
	}
			
	private void PlayBattleTutorial(TutorialsAndCallback tutorialId)
	{
		//GameManager gameManager = GameManager.instance;
		//ScaleformCamera sfCamera = gameManager.scaleformCamera;
		//PauseGame();
		GameManager.instance.scaleformCamera.hud.PlayTutorial(tutorialId.ToString());
	}
	
	public void LoadTutorialStatus()
	{
		if(tutorialStatusDictionary == null || tutorialStatusDictionary.Count == 0)
		{
			MonoHelpers monoHelper = GameManager.instance._monoHelpers;
			monoHelper.StartCoroutine(monoHelper.LoadFromFile(FILE_NAME, false, OnFileContentReceived));
		}
	}
	
	private void SaveTutorialStatus()
	{
//		string tutorialStatus = Json.Serialize(tutorialStatusDictionary);		
//		GameManager.instance._monoHelpers.WriteIntoPersistantDataPath(tutorialStatus, FILE_NAME);
//		RemoveMask();
		GameManager.instance.SaveGameState(false);
	}
		
	private void ResumeGame()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("ResumeGame CALLED!!! + isGamePause =" + isGamePause);
		Time.timeScale = 1;
		isGamePause = false;
	}
	
	public void PauseGame()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("PauseGame CALLED!!! + isGamePause =" + isGamePause);
		if(!isGamePause)
		{
			Time.timeScale = 0;
			isGamePause = true;
		}
	}
	
	private void OnFileContentReceived(bool isError, string fileContent)
	{
		if(fileContent != null && !fileContent.Equals(""))
		{
			tutorialStatusDictionary = Json.Deserialize(fileContent) as Dictionary<string, object>;
			if(tutorialStatusDictionary == null)
			{
				tutorialStatusDictionary = new Dictionary<string, object>();
			}
		}
	}
	
	/*
	public void SetMask(float x, float y)
	{
		float xRpos = x/Screen.width;
		float yRpos = 1- y/Screen.height;

//		if(GameManager.PRINT_LOGS) Debug.Log("X ratio value = " + xRpos + " Y ratio value = " + yRpos);
		GameManager.instance.scaleformCamera.tutorialSwf.SetMask(xRpos,yRpos);
	}
	*/
	
//	void OnGUI () {
//		if (GUI.Button (new Rect (xRpos * Screen.width,yRpos * Screen.height,20,20), "O")) {
//			print ("You clicked the button!");
//		}
//	}
	
	
	public void RemoveMask()
	{
		//UIManager.instance.tutorialSwf.RemoveMask();
		//TutorialManager.instance.RemoveMask();
	}
	
	public void decideInLevelTutorial(bool isShrine)
	{
		if(GameManager.instance.scaleformCamera.generalSwf.tempMenuName.Equals("Spirits"))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("::::: TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.SpiritTutorialStart);");
			TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.SpiritTutorialStart);
		}
		else if(isShrine)
		{
			Debug.Log("GameManager.instance._levelManager.currentLevel >> " + GameManager.instance._levelManager.currentLevel + ">>>>>>>>>> SHRINE TIMEEEEE");
			
			Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
			analParams.Add (Analytics.ParamName.ShrineLevelName, GameManager.instance._levelManager.currentLevel);
			Analytics.logEvent(Analytics.EventName.Shrine_Tutorial_Start, analParams);
			string message = "";

			if(GameManager.instance._levelManager.currentLevel.Equals("HollowTree"))
			{
				//GameManager.instance.scaleformCamera.NewTutorialSwf.ShrineTutorialStart("This enemy has lightning! You will need earth to beat him. Look for a Shrine that has the earth ring!");
//				TutorialManager.instance.ShrineTutorialStart("This enemy has lightning! You will need earth to beat him. Look for a Shrine that has the earth ring!");
//				GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kShrine;

				if(!GameManager._gameState.User._hasEarth) {
					message = "This enemy has lightning! You will need earth to beat him. Look for a Shrine that has the earth ring!";
				} else {
					message = "Look for a shrine to activate";
				}

				TutorialManager.instance.ShrineTutorialStart(message);
				GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kShrine;
			}
			if(GameManager.instance._levelManager.currentLevel.Equals("ThreeGods"))
			{
//				TutorialManager.instance.ShrineTutorialStart("This enemy has water! You will need lightning to beat him. Look for a Shrine that has the lightning ring!");
//				GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kShrine;

				if(!GameManager._gameState.User._hasLightning) {
					message = "This enemy has water! You will need lightning to beat him. Look for a Shrine that has the lightning ring!";
				} else {
					message = "Look for a shrine to activate";
				}

				TutorialManager.instance.ShrineTutorialStart(message);
				GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kShrine;

			}
			if(GameManager.instance._levelManager.currentLevel.Equals("CaveEntrance"))
			{
//				if(!ShrineManager.Instance.GetShrineForLevel("ForbiddenCave_Shrine").isActivated)
//				TutorialManager.instance.ShrineTutorialStart("This enemy has earth! You will need fire to beat him. Look for a Shrine that has the fire ring!");
//				//GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kShrine;
//
				if(!GameManager._gameState.User._hasFire) {
					message = "This enemy has earth! You will need fire to beat him. Look for a Shrine that has the fire ring!";
				} else if(!ShrineManager.Instance.GetShrineForLevel("ForbiddenCave_Shrine").isActivated) {
					message = "Look for a shrine to activate";
				}
				TutorialManager.instance.ShrineTutorialStart(message);

			}
		}
		else
		{
			if(GameManager.instance._levelManager.currentLevel.Equals("StatuePath")){
				ArcaneKeysStart();
				GameManager.instance._levelManager.poiState=LevelManager.AllowedPoiTypes.kLoot;
			}
		}
	}
	public void ShowInLevelTutorial(string levelName)
	{
		if(currentTutorial != TutorialsAndCallback.None)
		{
			Debug.Log("currentTutorial >> " + currentTutorial.ToString());
			return;
		}
			
		
		switch(levelName)
		{
		case "ToHollowTree":
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LevelTutorialCompleted))
				{
					InputWrapper.disableTouch = true;
				
					currentTutorial=TutorialManager.TutorialsAndCallback.LevelTutorialCompleted;
					GameManager.instance.scaleformCamera.loadUILevelTutorial();				//Time.timeScale = 0.0f;
					GameManager.instance.scaleformCamera.isPaused = true;
					GameManager.instance._levelManager.poiState = LevelManager.AllowedPoiTypes.kDuel;
					Analytics.logEvent(Analytics.EventName.Level_Tutorial_Start);
			}
			break;
			
			case "HollowTree":
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted))
				{
					currentTutorial=TutorialManager.TutorialsAndCallback.EarthShrineTutorialCompleted;
					GameManager.instance.scaleformCamera.loadNewTutorial();				//Time.timeScale = 0.0f;
					GameManager.instance.scaleformCamera.isPaused = true;
				}
			break;
			
			case "ThreeGods":
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.LightningShrineTutorialCompleted))
				{
					currentTutorial=TutorialManager.TutorialsAndCallback.LightningShrineTutorialCompleted;
					GameManager.instance.scaleformCamera.loadNewTutorial();				//Time.timeScale = 0.0f;
					GameManager.instance.scaleformCamera.isPaused = true;
				}
			break;
			
			case "CaveEntrance":
			if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted))
				{
					if(!ShrineManager.Instance.GetShrineForLevel("ForbiddenCave_Shrine").isActivated)
					{
						currentTutorial=TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted;
						GameManager.instance.scaleformCamera.loadNewTutorial();				//Time.timeScale = 0.0f;
						GameManager.instance.scaleformCamera.isPaused = true;
					}
				}
			break;
			
//			case "BeachCamp":
//				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.WaterShrineTutorialCompleted))
//				{
//					GameManager.instance.scaleformCamera.loadNewTutorial();				//Time.timeScale = 0.0f;
//					GameManager.instance.scaleformCamera.isPaused = true;
//				}
//			break;
			case "StatuePath":
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted))
				{
					currentTutorial=TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted;
					GameManager.instance.scaleformCamera.loadNewTutorial();				//Time.timeScale = 0.0f;
					Analytics.logEvent(Analytics.EventName.ArcaneKeys_Tutorial_Start);
				}
			break;
			
		}
		
	}
	
//	public void OnGUI()
//	{
//		if (GUI.Button(new Rect(10, 10, 150, 30), "Play water tutorial"))
//			ShowTutorial(TutorialsAndCallback.WaterTutorialStart);
//		if (GUI.Button(new Rect(10, 50, 150, 30), "add souls"))
//			GameManager._gameState.User._inventory.souls += 90000;
//		
//		if (GUI.Button(new Rect(10, 90, 150, 30), "clear level tutorial"))
//		{
//			AddTutorialIdWithStatus(TutorialsAndCallback.LevelTutorialCompleted, false);
//		}
//
//		if (GUI.Button(new Rect(10, 120, 150, 30), "show mask"))
//		{
//			Mask.instance.SetMask(500.0f,500.0f);
//		}
		
//	}
	
	#region Code addition... to work with UI_Tutorial (previous in scaleform)
	
	public void SetLanguage(string languageString, string charSetID)
	{
		
	}
	
	public void loadTutorialSwf()
	{
		
	}
	
	public void RingTutorial2()
	{
		
	}
	
	public void RingTutorial3()
	{
		
	}
	
	public void RingTutorial4()
	{
		
	}
	
	public void RingTutorialEnd()
	{
		
	}
	
	public void closeUIScreen(string tutorialID)
	{
		
	}
	
	public void RuneTutorialEnd()
	{
		
	}
	
	public void UpgradeTutorialEnd()
	{
		
	}
	
	public void TransmutationTutorial2()
	{
		
	}
	
	public void TransmutationTutorial3()
	{
		
	}
	
	public void TransmutationTutorial4()
	{
		
	}
	
	public void TransmutationTutorialEnd()
	{
		
	}
	
	public void potionTutorialStart()
	{
		
	}
	
	public void PotionTutorial2()
	{
		DestroyManualTutorial(TutorialsAndCallback.PotionTutorialStart);
		PlayTutorial(TutorialsAndCallback.PotionTutorial2, Tutorial.CallbackMech.manual, () => {});
	}
	
	public void PotionTutorialEnd()
	{
		DestroyManualTutorial(TutorialsAndCallback.PotionTutorial2);
		ShowTutorial(TutorialsAndCallback.PotionTutorialCompleted);
	}
	
	public void NexFightTutorialStart()
	{
		PauseGame();
		Debug.Log("NexFightTutorialStart CALLED");
		
		PlayTutorial(TutorialsAndCallback.NexFightStart, Tutorial.CallbackMech.auto, () => {
			ShowTutorial(TutorialsAndCallback.NexFightTutorialEnd);
		});
	}
	
	public void NexCamera()
	{
		PlayTutorial(TutorialsAndCallback.NexCamera, Tutorial.CallbackMech.manual, () => {
			Debug.Log("NexCamera DESTROYED!");
		});
	}
	
	public void NexCameraEnd()
	{
		Debug.Log("NexCameraEnd CALLED");
	}
	
	public void spellTutorial(string tutorialId)
	{
		
	}
	
	public void playTutorial(string tutorialId)
	{
		
	}

	public UIPanel findShrineLabelPrefab;
	/*
	public dfPanel findNexLabelPrefab;
	public dfPanel castNowLabelPrefab;
	*/
	private GameObject findShrineLabel;
	private GameObject findNexLabel;
	private GameObject castNowLabel;
	
	public void ShrineTutorial2()
	{
		if(findShrineLabel == null)
			findShrineLabel = NGUITools.AddChild(this.gameObject, findShrineLabelPrefab.gameObject);
	}
	
	public void removeFindShrine()
	{
		if(findShrineLabel != null)
		{
			Destroy(findShrineLabel);
			findShrineLabel = null;
		}
	}


	public bool IsFindShrine() {

		return findShrineLabel != null;
	}

	public void ShrineChargeErrorTutorial() {
		removeFindShrine();
		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineChargeErrorPopup");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).AddComponent<Tutorial>();
		tutorial.Show(TutorialsAndCallback.ShrineHelpTutorial, Tutorial.CallbackMech.auto, () => {
			Debug.Log("Shrine error msg - charging only once!");
		});
		manualTutorials.Add(tutorial);
	}

	public void ShrineHelpTutorial() {
		removeFindShrine();
		UnityEngine.Object tutorialAsset = Resources.Load("UIPrefabs/NGUI/ShrineHelp");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).AddComponent<Tutorial>();
		tutorial.Show(TutorialsAndCallback.ShrineHelpTutorial, Tutorial.CallbackMech.manual, () => {
			Debug.Log("Shrine Help Tutorial closed!");
		});
		manualTutorials.Add(tutorial);
	}

	public void ShrineTutorialStart(string text)
	{
		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineTextPopup");
		ShrineTextPopup popup = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).GetComponent<ShrineTextPopup>();
		popup.showPopup(text, tutorialUIScript.ShrineTutorialStartEnd);


		Debug.Log("ShrineTutorialStart >> " + text);
//		ShrineTextPopup popup = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/ShrineTextPopup") as GameObject).GetComponent<ShrineTextPopup>();
//		popup.showPopup(text, tutorialUIScript.ShrineTutorialStartEnd);
	}
	
	public void ShrineTutorial3() //show "tap to charge shrine"
	{
		removeFindShrine();

		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineTapToChargePopup");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).AddComponent<Tutorial>();
		tutorial.Show(TutorialsAndCallback.ShrineTutorial3, Tutorial.CallbackMech.manual, () => {
			Debug.Log("Shrine tutorial 3 closed!");
		});
		manualTutorials.Add(tutorial);


//		dfControl shrineTut = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/ShrineTapToChargePopup") as GameObject);
//		Tutorial tutorial = shrineTut.gameObject.AddComponent<Tutorial>();
//		tutorial.Show(TutorialsAndCallback.ShrineTutorial3, Tutorial.CallbackMech.manual, () => {
//			Debug.Log("Shrine tutorial 3 closed!");
//		});
//		manualTutorials.Add(tutorial);
	}
	
	public void ShrineTutorial4() // show "Good job! the shrine is fully charged. Collect your reward!"
	{
		DestroyManualTutorial(TutorialsAndCallback.ShrineTutorial3, () => {
			Debug.Log("Call to destroy Shrine tutorial 3 !!");
		});


		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineCollectRewardPopup");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).AddComponent<Tutorial>();
		tutorial.Show(TutorialsAndCallback.ShrineTutorial4, Tutorial.CallbackMech.manual, () => {
			Debug.Log("Shrine tutorial 4 closed!");
		});
		manualTutorials.Add(tutorial);
//
//		dfControl shrineTut = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/ShrineCollectRewardPopup") as GameObject);
//		Tutorial tutorial = shrineTut.gameObject.AddComponent<Tutorial>();
//		tutorial.Show(TutorialsAndCallback.ShrineTutorial4, Tutorial.CallbackMech.manual, () => {
//			Debug.Log("Shrine tutorial 4 closed!");
//		});
//		manualTutorials.Add(tutorial);
	}
	
	public void ShrineTutorial5(Action cb) // show "Nice find! Don't forget to equip the earth ring so you can use it in battle!"
	{
		DestroyManualTutorial(TutorialsAndCallback.ShrineTutorial4, () => {
			Debug.Log("Call to destroy Shrine tutorial 4 !!");
		});

		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineEquipEarthRing");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).GetComponent<Tutorial>();

//		dfControl shrineTut = GetComponent<dfPanel>().AddPrefab(Resources.Load("Tutorials/ShrineEquipEarthRing") as GameObject);
//		Tutorial tutorial = shrineTut.gameObject.AddComponent<Tutorial>();
		tutorial.Show(TutorialsAndCallback.ShrineTutorial5, Tutorial.CallbackMech.auto, () => {
			Debug.Log("Shrine tutorial 5 closed!");
			if(cb != null)
				cb();
		});
	}
	
	public void ShrineTutorialEnd()
	{
		Debug.Log("Shrine Tutorial End... need to add callback to onShrineTutorialEnd! Look it up!");
	}
	
	public void ArcaneKeysStart() // text popup that says "You can use Arcane keys to ... Look around to find a treasure chest"
	{
		if(!IsTutorialCompleted(TutorialsAndCallback.arcKeysTutorialCompleted))
		{
			UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/ShrineTextPopup");
			ShrineTextPopup popup = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).GetComponent<ShrineTextPopup>();

			popup.showPopup("You can use Arcane Keys to open Treasure Chests.\n Look around to find a treasure Chest!", () => {
				Debug.Log("Treasure chest popup needs to close!");
				ArcaneKeys2();
			});
		}
	}
	
	public void RemoveFindTreasure()
	{
		DestroyManualTutorial(TutorialsAndCallback.findTreasure, () => {
			Debug.Log("Call to destroy FIND TREASURE!!! !!");
		});
	}
		
	public void ArcaneKeys2() //Find treasure 
	{
		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/FindTreasure");

		Tutorial findTreasureTutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).AddComponent<Tutorial>();
		findTreasureTutorial.Show(TutorialsAndCallback.findTreasure,Tutorial.CallbackMech.manual, () => {
			Debug.Log("Remove find treasure chest label!");
		});
		manualTutorials.Add(findTreasureTutorial);
	}
	
	public void ArcaneKeys3() // Nice find popup!
	{
		DestroyManualTutorial(TutorialsAndCallback.findTreasure, () => {
			Debug.Log("Call to destroy ArcaneKeys3 !!");
		});

		UnityEngine.Object tutorialAsset = Resources.Load("Tutorials/TreasureNiceFind");
		Tutorial tutorial = NGUITools.AddChild(this.gameObject, tutorialAsset as GameObject).GetComponent<Tutorial>();

		tutorial.Show(TutorialsAndCallback.ShrineTutorial5, Tutorial.CallbackMech.auto, () => {
			
		});
	}
	
	public void ArcaneKeysEnd()
	{
		Debug.Log(":::: ArcaneKeysEnd ::::");
		
		UIManager.instance.generalSwf.onArcaneKeysEnd();
	}
	
	public void setMask(float x, float y) //Unused
	{
		
	}
	
	public void removeMask() //Unused
	{
		
	}
	
	public void SpiritTutorialStart()
	{
		
	}
	
	public void LoadUIRingTutorial()
	{
		ShowTutorial(TutorialsAndCallback.RingTutorialStart);
	}

	public void LoadUIStaffTutorial()
	{
		ShowTutorial(TutorialsAndCallback.RuneTutorialCompleted);
	}
	
	public void loadUIUpgradeTutorial()
	{
		ShowTutorial(TutorialsAndCallback.UpgradeTutorialStart);
	}
	
	public void DestroyRingTutorial()
	{
		if(!IsTutorialCompleted(TutorialsAndCallback.RingTutorialCompleted))
			ShowTutorial(TutorialsAndCallback.RingTutorialCompleted);
	}
	
	public void RuneSpellTutorialStart()
	{
		PlayTutorial(TutorialsAndCallback.runeSpellTutorialStart, Tutorial.CallbackMech.manual, () => {}, () => {
			ShowTutorial(TutorialsAndCallback.OnRuneSpellTutorialStart);
		});
	}
	
	public void RuneSpellTutorial2()
	{
		DestroyManualTutorial(TutorialsAndCallback.runeSpellTutorialStart);
		PlayTutorial(TutorialsAndCallback.runeSpellTutorial2, Tutorial.CallbackMech.manual, () => {}, () => {
			ShowTutorial(TutorialsAndCallback.OnRuneSpellTutorial2);
		});
	}
	
	public void RuneSpellTutorialEnd()
	{
		DestroyManualTutorial(TutorialsAndCallback.runeSpellTutorial2, () => {
			ShowTutorial(TutorialsAndCallback.OnRuneSpellTutorialEnd);
		});
	}
	#endregion

	#region Debugging only...

	public void OnGUI()
	{
//
//
//		if(GUI.Button(new Rect(10, 10, 60, 40), "ShrineTutorial2"))
//		   ShrineTutorial2();
//
//		if(GUI.Button(new Rect(10, 60, 60, 40), "ShrineTutorial3"))
//			ShrineTutorial3();
//
//		if(GUI.Button(new Rect(10, 110, 60, 40), "ShrineTutorial4"))
//			ShrineTutorial4();
	}

	#endregion
}