using System;
using System.Collections;
using UnityEngine;

public class UI_HUD : MonoBehaviour, BattleUIInterface
{
    // Reference to the primary MovieClip (hud) within the UI_Hud.swf movie. This is not the root!
    public NGBattleUIScript hud = null;
    // Reference to the singleton dataStore (attached to Main Camera) where the Player's data is kept.
    //protected SFDataStore _dataStore = null;
	
    // Reference to the TextField that displays the letter grade of their score.

	
    // Reference to the health bar that displays the player's health using scaleX.
    protected int enemyHealth = 0;
    // Reference to the energy bar that displays the player's weapon energy using scaleX. 
    protected int enemyHUDHealth = 0;
	
    // Player's current health according to the dataStore.
    protected int currentHealth = 0;
    // Player's current health displayed in the HUD.
    protected int HUDHealth = 0;
    // Player's max health to calculate the percentage for healthBar.gotoAndStop().
    protected int maxHealth = 200;

	
    // true if the SWF has registered itself and this class is initialized; false otherwise.
    protected bool _isInitialized = false;
    // true if the game is currently paused; false otherwise.
    protected bool _isPaused = false;
    // true if the HUD is displaying the Warning Indicator (energyWarn); false if it is hidden.
    protected bool _isWarningEnergy = false;
	
    BattleManager BM;
    bool isCastNow = false;
    //ScaleformCamera sfCamera= null;
//	Value retVal = new Value();
	
    // Required to implement this constructor.
    /*
	public UI_HUD(//ScaleformCamera parent,
		SFManager sfmgr, SFMovieCreationParams cp ):
		base( sfmgr,new MovieDef( SFManager.GetScaleformContentPath() + "in_game_ui_v2.swf" ),  cp )
	{
		//sfCamera=parent;
		AdvanceWhenGamePaused = true;
	}
	*/
    SRCharacterController sr;
    // Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
    public void OnRegisterSWFCallback(NGBattleUIScript movieRef)
    {
//        if (GameManager.PRINT_LOGS) Debug.Log("UI-HUD here 1");
        hud = movieRef;
		
//		UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
//		SetDisplayVisible(false);
        //Value retVal = new Value();
        //if(Application.loadedLevelName=="LevelScene")

        if (GameManager.instance != null)
            SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);

//        if (GameManager.PRINT_LOGS) Debug.Log("UI-HUD here 2");
		
        BM = GameManager.instance._levelManager.battleManager;//GameObject.Find("BattleManager").GetComponent<BattleManager>();
        //sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;	
        sendFlashPotionBeltLevel();
		BM.sendReadyForSpellCast += OnReadyForSpellCastTime;
		
    }

	void OnReadyForSpellCastTime(float time) {
		StopCoroutine("LoadingFillSequence");
		StartCoroutine("LoadingFillSequence", time);
	}

	#region Spirit in game tutorial
    public void SpiritFightTutorialStart()
    {
        /*
		Value[] valarr = {};
		hud.Invoke("SpiriteFightTutorialStart",valarr);
		*/
        hud.SpiritFightTutorialStart();
        Time.timeScale = 0.0f;
        GameManager.instance.scaleformCamera.isPaused = true;
    }
	
    public void SpiriteFightTutorialEnd()
    {
        if (TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.spiritFightTutorialStart))
        {
            TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.spiritFightTutorialStart, true);
            Time.timeScale = 1.0f;
            GameManager.instance.scaleformCamera.isPaused = false;
        }
    }
	
	#endregion
	
    public void ShowPlayerElementStats(bool isFire, bool isWater, bool isLightning, bool isEarth)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("isWater" + isWater + "isEarth" + isEarth + "isLightning" + isLightning + "isFire" + isFire);
        /*
		Value val1 =new Value(isFire,MovieID);
		Value val2 =new Value(isWater,MovieID);
		Value val3 =new Value(isLightning,MovieID);
		Value val4 =new Value(isEarth,MovieID);
		Value[] args = {val1,val2,val3,val4};
		hud.Invoke("PlayerElementStats",args);
		*/
        hud.PlayerElementStats(isFire, isWater, isLightning, isEarth);
    }

    public void ShowBossFightText(string bossName)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("ShowBossFightText >>>>>>>>>>>>>>> " + bossName);
        /*
		Value val1 =new Value(bossName,MovieID);
		Value[] args = {val1};
		hud.Invoke("bossFights",args);
		*/
        hud.BossFights(bossName);
    }
	
    /*public void missingString(string missingThings)
	{
		if(Debug.isDebugBuild)
		{
			if(GameManager.instance.scaleformCamera.generalSwf!=null)
			{
				GameManager.instance.scaleformCamera.generalSwf.words +=missingThings+"\n";
				if(GameManager.PRINT_LOGS) Debug.Log("UI_HUD : Word Recieved :::: "+missingThings+" Current Words :::"+GameManager.instance.scaleformCamera.generalSwf.words);
			}
		}
	}*/
	
    public void HideBossFightText()
    {
        /*
		Value[] args = {};
		hud.Invoke("turnOffBossFights",args);
		*/
        hud.TurnOffBossFights();
    }

    /* 	<summary>
			Highlights the unavailable element on battle UI.
		<param name='unavailableGesture'>
			The unavailable gesture to highlight & show text for.	*/
    public void HighlightUnavailableElement(GestureEmitter.Gesture unavailableGesture)
    {
        string gestureString = "";
        if (unavailableGesture == GestureEmitter.Gesture.kEarth)
            gestureString = "Earth";
        if (unavailableGesture == GestureEmitter.Gesture.kFire)
            gestureString = "Fire";
        if (unavailableGesture == GestureEmitter.Gesture.kWater)
            gestureString = "Water";
        if (unavailableGesture == GestureEmitter.Gesture.kLightning)
            gestureString = "Lightning";
		
        /*
		Value val1 =new Value(gestureString,MovieID);
		Value[] args = {val1};
		hud.Invoke("ElementBlink",args);
		*/
		
        hud.ElementBlink(gestureString);
    }
	
    public void callSwfFunction(int current, int total)
    {
        /*
		//if(GameManager.PRINT_LOGS) Debug.Log(a);
		//Value retVal = new Value();
		Value val=new Value(0.5f,MovieID);
		Value[] args1={val};
		//mainMenu.SetMember
		hud.Invoke("updateLoadingBar",args1);
		*/
        hud.updateLoadingBar(0.5f);
    }
	
    public void sendFlashPotionBeltLevel()
    {
        /*
		Value val=new Value(GameManager._gameState.User._inventory.potionBelt.level,MovieID);
		Value val2=new Value(GameManager._gameState.User._inventory.potionBelt.Count(),MovieID);
		Value val3=new Value(GameManager._gameState.User.isSpiritEnabled,MovieID);

		Value[] args1={val,val2,val3};
		
		hud.Invoke("gamePotionLevel",args1);
		*/
		
        hud.GamePotionLevel(GameManager._gameState.User._inventory.potionBelt.level, GameManager._gameState.User._inventory.potionBelt.Count(), GameManager._gameState.User.isSpiritEnabled);
    }
	
	
    public void usePotionInBattle(bool usePotion)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log(((GameManager._gameState.User._inventory.potionBelt.healAmount) * (GameManager.instance._levelManager.battleManager._battleState._user.totalLife)) / 100 + "  ------  potion button pressed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		
        //GameManager.instance._levelManager.battleManager._battleState._user.life
        if (usePotion)
        {
            if (GameManager.instance._levelManager.battleManager._battleState != null)
            {
                if (GameManager.instance._levelManager.battleManager._battleState._user.life == GameManager.instance._levelManager.battleManager._battleState._user.totalLife)
                {
                    Debug.LogError("Life is equal to total life.");
                    return;
                }

                if (GameManager.instance._levelManager.battleManager._battleState._user.UsePotion(InventorySystem.InventoryItem.Type.kHealthPotion))
                {
                    GameManager.instance._levelManager.battleManager._battleState._user.life += ((GameManager._gameState.User._inventory.potionBelt.healAmount) * (GameManager.instance._levelManager.battleManager._battleState._user.totalLife)) / 100;
					SetPlayerFill(GameManager.instance._levelManager.battleManager._battleState._user.life, GameManager.instance._levelManager.battleManager._battleState._user.totalLife);
				}
            }
        }
    }
		
    public void SetPlayerFill(int currentHealth, int totalHealth)
    {
        /*
		Value val=new Value(currentHealth,MovieID);
		Value val1=new Value(totalHealth,MovieID);	
		Value[] args1={val,val1};
		hud.Invoke("setPlayerFill",args1);
		*/
        hud.SetPlayerFill(currentHealth, totalHealth);
    }
	
    public void SetDisplayVisible(bool isVisible)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("[UI_HUD] ---------             isVisible        :::::::    " + isVisible);
        if (UIManager.instance.generalSwf != null)
        {
//			UnityEngine.Debug.Log("generalSwf.ToggleTopStats > " + !isVisible);
            UIManager.instance.generalSwf.ToggleTopStats(!isVisible);
        }
        /*
		bool isAlreadyLoaded = (bool)hud.GetMember("visible");
		hud.SetMember("visible", new Value(isVisible, hud.MovieId));
		*/
        hud.SetVisible(isVisible); // IDK wat it does :/
    }
	
    public void SetEnemyFill(int currentHealth, int totalHealth)
    {
        /*
		Value val=new Value(currentHealth,MovieID);
		Value val1=new Value(totalHealth,MovieID);	
		Value[] args1={val,val1};
		hud.Invoke("setEnemyFill",args1);
		*/
        hud.SetEnemyFill(currentHealth, totalHealth);
    }
	
    public void StartCastBar(int firstBarTime, int secondBarTime) //Unused...
    {
        /*
		Value casttime1=new Value(firstBarTime,MovieID);
		Value casttime2=new Value(secondBarTime,MovieID);
		Value[] casttimeArr={casttime1,casttime2};
		hud.Invoke("startCastBar",casttimeArr);
		*/
		
    }
	
    public void AddPlayerDebuff(string name, bool onSelf = true, bool tutorialMode = false)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("ADD PLAYER debuff called --->>> " + name + " ON SELF ????" + onSelf + " TUTORIAL MODE????" + tutorialMode);
        /*
		Value val1 = new Value(name,MovieID);
		Value val2 = new Value(onSelf,MovieID);
		Value val3 = new Value(tutorialMode,MovieID);
		Value[] args = {val1, val2, val3};
		
		hud.Invoke("addPlayerDebuff",args);
		*/
        hud.AddPlayerDebuff(name, onSelf, tutorialMode);
    }
	
    public void TurnOffWard()
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("WARD TURN OFF CALLED!!!");
        /*
		hud.Invoke("turnOffWard");
		*/
        hud.TurnOffWard();
		
    }
	
    public void RemovePlayerDebuff(string name)
    {
        /*
		Value val1 = new Value(name,MovieID);
		Value[] args = {val1};
		
		hud.Invoke("removePlayerDebuff",args);
		*/
		
        hud.RemovePlayerDebuff(name);
    }
	
    public void AddEnemyDebuff(string name, bool onSelf = true, bool tutorialMode = false)
    {
		
//		if(name.ToLower().Equals("leech seed"))
//			name = "entangle";
//		Debug.Log("ADD ENEMY debuff called --->>> " + name + "::::::: ON SELF ::::::: "+ onSelf +" TUTORIAL MODE????" + tutorialMode);
		
        /*
		Value val1 = new Value(name,MovieID);
		Value val2 = new Value(onSelf,MovieID);
		Value val3 = new Value(tutorialMode,MovieID);
		
		Value[] args = {val1, val2, val3};
		
		hud.Invoke("addEnemyDebuff",args);
		
		*/
		
        hud.AddEnemyDebuff(name, onSelf, tutorialMode);
    }
	
    public void RemoveEnemyDebuff(string name)
    {
        //if(name.ToLower().Equals("leech seed"))
        //	name = "entangle";
		
        /*
		Value val1 = new Value(name,MovieID);
		Value[] args = {val1};
		
		hud.Invoke("removeEnemyDebuff",args);
		*/
		
        hud.RemoveEnemyDebuff(name);
    }
	
    public void TurnOnWard(int wardNumber)
    {
        /*
		Value ward=new Value(wardNumber,MovieID);
		Value[] ward1={ward};
		hud.Invoke("turnOnWard",ward1);
		*/
		
        hud.TurnOnWard(wardNumber);
		
    }
	
    public void StartChargeBar(float x, float y, float time) //Unused...
    {
        /*
		Value castBarX=new Value(x,MovieID);
		Value castBarY=new Value(y,MovieID);
		Value castingTime=new Value(time,MovieID);
		Value[] castBarPos= {castBarX,castBarY,castingTime};
		hud.Invoke("startChargeBar",castBarPos);
		*/
    }
	
    public void StopChargeBar()
    {
        /*
		Value[] args= {};
		hud.Invoke("stopChargeBar",args);
		*/
    }

    public void CastNow()
    {
        /*
		Value[] args={};
		hud.Invoke("castNow",args);
		*/
        hud.CastNow();
    }
	
	public void SetLoadingFill(bool status) {
		hud.SetLoadingFill(status);
	}

	public void SetLoadingFillAmount(float amount) {
		hud.SetLoadingFillAmount(amount);
	}


	public void ChainStun(bool status)
    {
        /*
		Value[] args={};
		hud.Invoke("chainStun",args);
		*/
        hud.ChainStun(status);
    }
	
	
    public void useSpiritInBattle() //Callback from Scaleform, for spirit button tap...
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SPIRIT TAP DETECTED!!!");
        GameManager.instance._levelManager.battleManager._petController.OnSpiritTap();
    }
	
    public void TurnOffCastNow()
    {
        /*
		Value[] args={};
		hud.Invoke("turnOffCastNow",args);
		*/
		
        hud.TurnOffCastNow();
    }
	
    public void FindNex()
    {
        /*
		Value[] args={};
		hud.Invoke("findNex",args);
		*/
		
        hud.FindNex();
    }
	
    public void TurnOffFindNex()
    {
        /*
		Value[] args={};
		hud.Invoke("turnOffFindNex",args);
		*/
		
        hud.TurnOffFindNex();
    }
    //Value bar=mainMenu.GetMember("loadingBar").GetMember("barFill");
    //	bar.SetMember("scaleX",new Value(1,bar.MovieId));
    //}
	
//	public void OnRegisterScoreGrade(Value val)
//	{
//		scoreGrade = val;	
//	}
//	public void OnRegisterScoreNumber(Value val)
//	{
//		scoreNumber = val;	
//	}
//	public void OnRegisterHealthBar(Value val)
//	{
//		healthBar = val;	
//	}
//	public void OnRegisterEnergyBar(Value val)
//	{
//		energyBar = val;	
//	}
//	public void OnRegisterEnergyWarn(Value val)
//	{
//		energyWarn = val;	
//	}
//	public void OnRegisterAchievement(Value val)
//	{
//		achievement = val;	
//	}
//	public void OnRegisterGamepad(Value val)
//	{
//		//	
//	}
//	
	public void Update()
    {
        if (GameManager._gameState != null)
        {
            if (BM != null && BM._battleState != null)
            {
                currentHealth = BM._battleState._user.life;

				if (HUDHealth != currentHealth)
                {
                    HUDHealth = currentHealth;
                    SetPlayerFill(HUDHealth, BM._battleState._user.totalLife);
                }
				
                enemyHealth = BM._battleState._enemy.life;
				
                if (enemyHUDHealth != enemyHealth)
                {
                    enemyHUDHealth = enemyHealth;
                    SetEnemyFill(enemyHUDHealth, BM._battleState._enemy.totalLife);
                }
//                if (BM.showCastCue && !isCastNow)
//                if (GameManager.PRINT_LOGS)
//                    Debug.Log("isCastNow = " + isCastNow + "                ***********                 showCastCue =" + BM.showCastCue);
				
                if (BM.showCastCue && !isCastNow)
                {
                    TurnOffFindNex();
                    CastNow();
                    isCastNow = true;
                } else if (!BM.showCastCue)
                {
					TurnOffCastNow();
                    isCastNow = false;
				}
			
                sendFlashPotionBeltLevel();
            }
        }
    }
	
    public void ShowCastNow(bool yesNo)
    {
        if (yesNo)
        {
            CastNow();
        } else
            TurnOffCastNow();
    }
		
	public void ShowLoadingFill(bool status)
	{
//		SetLoadingFill(status);
		if(status) {
//			StartCoroutine("LoadingFillSequence");
		} else {
//			StopCoroutine("LoadingFillSequence");
		}
	}

	IEnumerator LoadingFillSequence(float time) {
		float elapsedTime = 0.0f;
		while(elapsedTime < time) {
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
			SetLoadingFillAmount(elapsedTime/time);
		}
	}

	#region Staff Charge
    public void StaffEnable(bool yesNo)
    {
        /*
		Value val1 = new Value(yesNo,MovieID);
		Value[] args = {val1};
		hud.Invoke("staffEnable",args);
		*/
        hud.StaffEnable(yesNo);
    }
	
    public void StaffModeEnable(bool yesNo)
    {
        /*
		Value val1 = new Value(yesNo,MovieID);
		Value[] args = {val1};
		hud.Invoke("staffModeEnable",args);
		*/
		
        hud.StaffModeEnable(yesNo);
    }
	
    public void BattleStaff(int number) // number 1-6 for Filling the bar, 7+ for fully charging/enabling button press
    {
        /*
		Value val1 = new Value(number,MovieID);
		Value[] args = {val1};
		hud.Invoke("BattleStaff",args);
		*/
		
        hud.BattleStaff(number);
    }
	
    public void StaffCharged() //Callback when staff is pressed, only available when staff is fully charged...
    {
        BM.OnRuneAttackButtonPressed();
    }

    public void SpiritCharged()
    {
//		BM.OnSpiritThrow();
        GameManager.instance._levelManager.battleManager._petController.OnSpiritTap();
    }
	
    public void SetRuneGestures(string gesturesArray)
    {
        Debug.Log("gesturesArray TO SCALEFORMMMMMMMMMMMMM ---------->>>>>>>>>>>>                    " + gesturesArray);
        /*
		Value val1 = new Value(gesturesArray,MovieID);
		Value[] args = {val1};
		hud.Invoke("setRuneGestures",args);
		*/
        hud.setRuneGestures(gesturesArray);
    }
	
    public void PlayTutorial(string tutorialId)
    {
//		Value tutorialIdVal = new Value(tutorialId, MovieID);
//		Value[] tutorialIdValArr = {tutorialIdVal};
        hud.Invoke(tutorialId, 0);
    }
	
    public void RuneSpellTutorialStart()
    {
        /*
		if(GameManager.PRINT_LOGS) Debug.Log("RuneSpellTutorial");
		Value[] args = {};
		hud.Invoke("runeSpellTutorialStart",args);
		*/
		
        //hud.RuneSpellTutorialStart();
    }
	
    public void onRuneSpellTutorialStart()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onRuneSpellTutorialStart");
        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnRuneSpellTutorialStart);
    }
	
    public void RuneSpellTutorial2()
    {
        /*
		if(GameManager.PRINT_LOGS) Debug.Log("RuneSpellTutorial2");
		Value[] args = {};
		hud.Invoke("runeSpellTutorial2",args);
		*/
		
        //hud.RuneSpellTutorial2();
    }
	
    public void RuneSpellTutorialEnd()
    {
        /*
		if(GameManager.PRINT_LOGS) Debug.Log("RuneSpellTutorialEnd");
		Value[] args = {};
		hud.Invoke("runeSpellTutorialEnd",args);
		*/
		
        //hud.RuneSpellTutorialEnd();
    }
	
    public void onRuneSpellTutorialEnd()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onRuneSpellTutorialEnd");
        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnRuneSpellTutorialEnd);
    }
	
    public void onRuneSpellTutorial2()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onRuneSpellTutorial2");
        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnRuneSpellTutorial2);
    }
	
    public void spellBurstTutorialStart(int degreeBetweenPlayerAndEnemy)
    {
        Debug.Log("[UI_HUD] spellBurstTutorialStart");
        /*
		Value tutorialIdVal = new Value(degreeBetweenPlayerAndEnemy, MovieID);	
		Value[] tutorialIdValArr = {tutorialIdVal};
		hud.Invoke("spellBurstTutorialStart",tutorialIdValArr);
		*/
		
        //hud.SpellBurstTutorialStart(degreeBetweenPlayerAndEnemy);
    }
	
    public void onSpellBurstTutorialStart()
    {
        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnSpellBurstTutorialStart);
    }
	
    public void spellBurstTutorialEnd()
    {
        Debug.Log("[UI_HUD] spellBurstTutorialEnd");
        //hud.Invoke("spellBurstTutorialEnd");
		
        //hud.SpellBurstTutorialEnd();
    }
	
    public void onSpellBurstTutorialEnd()
    {
        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.OnSpellBurstTutorialEnd);
    }
	
	#endregion
	
	#region SPIRIT/PET Charge
    public void SpiritEnable(bool yesNo)
    {
        Debug.Log("SpiritEnable enabled :::::::::::::::::::::::::::::: " + yesNo);
        /*
		Value val1 = new Value(yesNo,MovieID);
		Value[] args = {val1};
		hud.Invoke("spiritEnable",args);
		*/
		
        Debug.Log("SPIRIT ENABLE > " + yesNo);
        hud.SpiritEnable(yesNo);
    }
	
    public void BattleSpirit(int number) // number 1-6 for Filling the bar, 7+ for fully charging/enabling button press
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("BattleSpirit count :::::::::::::::::::::::::::::: " + number);
        /*
		Value val1 = new Value(number,MovieID);
		Value[] args = {val1};
		hud.Invoke("BattleSpirit",args);
		*/
		
        hud.BattleSpirit(number);
    }
	
    public void SpritCharged() //Callback when spirit button is pressed, only available when the button is fully charged...
    {
        useSpiritInBattle();
    }
	#endregion
	
    private void SetLanguage(string languageString, string charSetID)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("UI_HUD private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
        /*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		hud.Invoke("setLanguage",args1);
		*/
        hud.SetLanguage(languageString, charSetID);
    }
	
	#region Spirit tutorial
    public void onSpiritFightTutorial2()
    {
//		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpiritFightTutorial2);
    }
	
    public void onSpiritFightTutorialEnd()
    {
//		TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.onSpiritFightTutorialEnd);
    }
	
	#endregion
	
    public void onRatingSound()
    {
		
    }
	
    public void onSpiritFightTutorialEnding()
    {
        Debug.Log("WTF method name??!!! :@");
    }

	public void StartBattle() {
		HUDHealth = 0;
	}
}