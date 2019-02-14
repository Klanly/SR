using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Holoville.HOTween;


public class NGBattleUIScript : MonoBehaviour
{
	
    private BattleUIInterface _battleInterface;
    //Ingame Battle UI elements
//    private int skullLevel;
    public UIPanel BattleUI;
    public UILabel enemyHealthLabel;
    public UILabel playerHealthLabel;
	public UIProgressBar enemyHealthBar;
	public UIProgressBar playerHealthBar;
	public UIProgressBar enemyHealthMaskBar;
	public UIProgressBar playerHealthMaskBar;

	public UIButton potionsButton;
    public UILabel numberOfPotions;
	
	public UISprite loadingFillForeround;
	public UISprite loadingFillBackground;

    public UIPanel bottomPanel;		//off/On during/after staff sequence 
	
    public UISprite activeFire;
    public UISprite activeWater;
    public UISprite activeLightening;
    public UISprite activeEarth;
	
    public UISprite runeGesture1;
    public UISprite runeGesture2;
    public UISprite runeGesture3;
    public UISprite runeGesture4;
	
    public UILabel findNexLabel;
    public UIPanel bossFightPanel;
    public UILabel bossFightNameLabel;
    public UILabel castNowLabel;
    public UILabel chainStunLabel;
	
	
    public UIGrid playerWardsPanel;
    public UIGrid playerDebufsPanel;
    public UIGrid enemyDebufsPanel;
	
    public UIButton spiritsButton;
    public List<UISprite> spiritsList;
	
    public UIButton staffButton;
    public List<UISprite> staffList;
	
    private List<UISprite> wardArray;
    public GameObject wardPrefab;
	
    public GameObject ignitePrefab;
    public GameObject hastePrefab;
    public GameObject prisonPrefab;
    public GameObject regenPrefab;
    public GameObject lockPrefab;
    public GameObject drainPrefab;
    public GameObject leechwoodPrefab;
    public GameObject amplifyPrefab;
    public GameObject shieldPrefab;
    private List<UISprite> playerDebuffArray;
    private List<UISprite> enemyDebuffArray;
	
	
	
    //	public dfTweenGroup tg;
    //	public dfSprite dot1;
    /*************************************************/

	private Tweener playerHealthAlphaTween;
	private Tweener playerHealthMaskAlphaTween;
	private Tweener enemyHealthAlphaTween;
	private Tweener enemyHealthMaskAlphaTween;

	void Awake() {
//		skullLevel = 0;
		wardArray = new List<UISprite>();
		playerDebuffArray = new List<UISprite>();
		
		//		Debug.LogError("Rings wards = "+GameManager._gameState.User._wards);
		TurnOnWard(0);
		//		TurnOnWard(GameManager._gameState.User._wards);
		enemyDebuffArray = new List<UISprite>();

		ResetRuneSpells();

	}

//	void Start()
//	{
//		skullLevel = 0;
//		wardArray = new List<UISprite>();
//		playerDebuffArray = new List<UISprite>();
//		
//		//		Debug.LogError("Rings wards = "+GameManager._gameState.User._wards);
//		TurnOnWard(0);
//		//		TurnOnWard(GameManager._gameState.User._wards);
//		enemyDebuffArray = new List<UISprite>();
//		
//		if (_battleInterface != null)
//			_battleInterface.OnRegisterSWFCallback(this);
//		
//		ResetRuneSpells();
//	}
	
    // Update is called once per frame
    void Update()
    {
		
    }


//	public void OnGUI() {
//		if (GUILayout.Button("Increase Quality Level")) {
//			QualitySettings.IncreaseLevel(true);
//			Debug.LogError("Quality Level - "+QualitySettings.GetQualityLevel());
//		}
//		
//		if (GUILayout.Button("Decrease Quality Level")) {
//			QualitySettings.DecreaseLevel(true);
//			Debug.LogError("Quality Level - "+QualitySettings.GetQualityLevel());
//		}
//	}

    public void SetInterface(BattleUIInterface battleInterface)
    {
        _battleInterface = battleInterface;
    }

	public void Reset() {
//		skullLevel = 0;
		TurnOnWard(0);
		ResetRuneSpells();
		if (_battleInterface != null)
			_battleInterface.OnRegisterSWFCallback(this);

		foreach(Transform obj in playerDebufsPanel.transform) {
			Destroy(obj.gameObject);
		}
		foreach(Transform obj in enemyDebufsPanel.transform) {
			Destroy(obj.gameObject);
		}
		playerDebuffArray.Clear();
		enemyDebuffArray.Clear();
	}
	
    private void ResetRuneSpells()  //farhan
    {
        runeGesture1.gameObject.SetActive(false);
        runeGesture2.gameObject.SetActive(false);
        runeGesture3.gameObject.SetActive(false);
        runeGesture4.gameObject.SetActive(false);
    }
	
    public void setRuneGestures(string name)
    {
        string[] splits = name.Split(',');
		
        Array.ForEach<string>(splits, (obj) => Debug.Log(obj));
		
        ResetRuneSpells();

        for (int i = 0; i < splits.Length; i++)
        {
            if (splits [i].Equals("Ignite"))
            {
                runeGesture1.gameObject.SetActive(true);
            } else if (splits [i].Equals("Daze"))
            {
                runeGesture2.gameObject.SetActive(true);
            } else if (splits [i].Equals("Drain"))
            {
                runeGesture3.gameObject.SetActive(true);
            } else if (splits [i].Equals("Leechseed"))
            {
                runeGesture4.gameObject.SetActive(true);
            }
        }
    }
	
    public void StaffModeEnable(bool isTrue)
    {
        Debug.Log("StaffModeEnable > " + isTrue);
        bottomPanel.gameObject.SetActive(!isTrue);
		
        if (!isTrue)
        {
            ResetRuneSpells();
            BattleStaff(0);
        }
    }
	
    public void SpiritEnable(bool isTrue)
    {
        Debug.Log("::: SpiritEnable " + isTrue);
		
        spiritsButton.gameObject.SetActive(isTrue);
//		if(isTrue)
//			BattleSpirit(0);
    }
	
    public void StaffEnable(bool isTrue)
    {
//		Debug.Log("StaffEnable > " + isTrue);
		
        staffButton.gameObject.SetActive(isTrue);
        //		if(isTrue)
        //			BattleStaff(0);
        staffButton.SetState(isTrue ? UIButtonColor.State.Normal : UIButtonColor.State.Hover, true);
    }
	
    public void GamePotionLevel(int potionBeltLevel, int myPotions, bool mySprite = false)
    {
//        skullLevel = potionBeltLevel;
        if (potionBeltLevel == 1 || myPotions == 0)
        {
            if (potionsButton != null)
                potionsButton.gameObject.SetActive(false);
            return;
        } else
        {
            if (potionsButton != null)
            {
                potionsButton.gameObject.SetActive(true);
                numberOfPotions.text = "x" + myPotions.ToString();
            } else
                Debug.LogError("potionsButton is Destroyed.");
        }
		
        //		Debug.Log("SPIRIT ENABLE > " + mySprite);
        //		SpiritEnable(mySprite);
    }
    public void UsePotion()
    {
        // on potions button click
        // can implement this is on click of function
    }

    public void SetPlayerFill(float current, float total)
    {
        SetBarFill(true, current, total);
        playerHealthLabel.text = current.ToString();
        //		StartCoroutine(UpdateHealthPoints(playerHealthLabel, current));
    }
	
    public void SetEnemyFill(float current, float total)
    {
        SetBarFill(false, current, total);
        enemyHealthLabel.text = current.ToString();
        //		StartCoroutine(UpdateHealthPoints(enemyHealthLabel, current));
    }
	
    public void SetBarFill(bool player, float current, float total)
    {
//		Debug.LogError((player ? "Player" : "") + " current = "+current + " total " + total);

        float Magnitude = current / total;
        float time = Magnitude * 3.0f;
        float val = Magnitude;
        if (player)
        {
			if(Magnitude <= 0) {
				SoundManager.instance.PlayHealthLowSound(false);
			}
			if(Magnitude < 0.3f) {
				SoundManager.instance.PlayHealthLowSound(true);
				playerHealthAlphaTween = HOTween.To(playerHealthBar, 0.4f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo).Id("playerGlow"));
				playerHealthMaskAlphaTween = HOTween.To(playerHealthMaskBar, 0.4f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo).Id("playerGlow"));
			} else {
				SoundManager.instance.PlayHealthLowSound(false);
				if(playerHealthAlphaTween != null) {
					playerHealthAlphaTween.Kill();
					playerHealthMaskAlphaTween.Kill();
					playerHealthBar.alpha = 1.0f;
					playerHealthMaskBar.alpha = 1.0f;
				}
				List<IHOTweenComponent> list = HOTween.GetTweensById("playerGlow", false);
				foreach (IHOTweenComponent ihot in list) {
					ihot.Kill();
				}
			}
			HOTween.To(playerHealthBar, time, new TweenParms().Prop("value", val));
			HOTween.To(playerHealthMaskBar, time, new TweenParms().Prop("value", val).Delay(0.4f));
		} else
		{
			if(Magnitude < 0.3f) {
				enemyHealthAlphaTween = HOTween.To(enemyHealthBar, 0.4f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo).Id("enemyGlow"));
				enemyHealthMaskAlphaTween = HOTween.To(enemyHealthMaskBar, 0.4f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo).Id("enemyGlow"));
			} else {
				if(enemyHealthAlphaTween != null) {
					enemyHealthAlphaTween.Kill();
					enemyHealthMaskAlphaTween.Kill();
					enemyHealthMaskBar.alpha = 1.0f;
				}
				List<IHOTweenComponent> list = HOTween.GetTweensById("enemyGlow", false);
				foreach (IHOTweenComponent ihot in list) {
					ihot.Kill();
				}
			}

//			if(enemyHealthAlphaTween == null && Magnitude < 0.3f) {
//				enemyHealthAlphaTween = HOTween.To(enemyHealthBar, 0.4f, new TweenParms().Prop("alpha", 0.3f).Loops(-1, LoopType.Yoyo));
//			} else {
//				if(enemyHealthAlphaTween != null) {
//					enemyHealthAlphaTween.Kill();
//				}
//			}
//			HOTween.To(enemyHealthBar, time, new TweenParms().Prop("value", val).OnComplete((Mag) => { if(Mag == 0.0f) { Debug.LogError("Magnitude is zero so stop tween here");} else {Debug.LogError("Magnitude is "+Mag);} }), Magnitude);
			HOTween.To(enemyHealthBar, time, new TweenParms().Prop("value", val));
			HOTween.To(enemyHealthMaskBar, time, new TweenParms().Prop("value", val).Delay(0.4f));
		}
    }
	
    public void WardGlow(GameObject wardObj)
    {
        UISprite sprite;
        for (int i = 0; i < wardArray.Count; i++)
        {
            sprite = wardArray [i] as UISprite;
            //HOTween.To(sprite, 0.5f, new TweenParms().Prop("Opacity", 0.5f).Loops(-1, LoopType.Yoyo).Delay(i*0.1f));
        }
		
    }
	
	
    public void TurnOnWard(int wardNumber)
    {
        UISprite sprite;
        for (int i = wardArray.Count - 1; i >= 0; i--)
        {
            sprite = wardArray [i] as UISprite;
            Destroy(sprite);
        }
		
        for (int i = 0; i< wardNumber; i++)
        {
            UISprite instance = NGUITools.AddChild(playerWardsPanel.gameObject, wardPrefab).GetComponent<UISprite>();
            instance.name += i.ToString();
            wardArray.Add(instance);
            //playerWardsPanel.enabled = true;
            playerWardsPanel.Reposition();
        }
        WardGlow(null);
    }
	
    public void PlayerElementStats(bool fire, bool water, bool lightening, bool earth)
    {
        activeFire.gameObject.SetActive(fire);
        activeWater.gameObject.SetActive(water);
        activeLightening.gameObject.SetActive(lightening);
        activeEarth.gameObject.SetActive(earth);
    }
	
    public void ElementBlink(string element)
    {
        if (element.Equals("Fire"))
        {
            StartCoroutine(Blink(activeFire));			
        } else if (element.Equals("Water"))
        {
            StartCoroutine(Blink(activeWater));
        } else if (element.Equals("Lightning"))
        {
            StartCoroutine(Blink(activeLightening));		
        } else if (element.Equals("Earth"))
        {
            StartCoroutine(Blink(activeEarth));			
        }
    }
	
    IEnumerator Blink(UISprite sprite)
    {
        for (int i = 0; i < 10; i++)
        {
            sprite.gameObject.SetActive(!sprite.gameObject.activeInHierarchy);
            if (i % 2 == 0)
                yield return new WaitForSeconds(0.1f);
            else
                yield return new WaitForSeconds(0.3f);
        }
    }
	
    public void TurnOffWard()
    {
        UISprite sprite = wardArray [wardArray.Count - 1] as UISprite;
        wardArray.RemoveAt(wardArray.Count - 1);
        Destroy(sprite.gameObject);
//		playerWardsPanel.enabled = true;
        playerWardsPanel.Reposition();
    }
	
    public void BossFights(string BossName)
    {
        bossFightPanel.gameObject.SetActive(true);
        bossFightNameLabel.text = BossName.Replace('_', ' ');
    }
	
    public void TurnOffBossFights()
    {
        bossFightPanel.gameObject.SetActive(false);
    }
	
    public void CastNow()
    {
		Debug.LogError("Current tutorial cast now - "+TutorialManager.instance.currentTutorial.ToString()+" "+TutorialManager.instance.currentTutorial.ToString().Contains("Rune", true));
		if(TutorialManager.instance.currentTutorial.ToString().Contains("Rune", true)) {
			castNowLabel.transform.localPosition = new Vector3(0, 140.0f, 0);
			Debug.LogError("found rune tutorial "+castNowLabel.transform.localPosition);
		} else {
			Debug.LogError("did not find rune tutorial "+castNowLabel.transform.localPosition);
			castNowLabel.transform.localPosition = Vector3.zero;
		}
        castNowLabel.gameObject.SetActive(true);
        //HOTween.To(castNowLabel, 0.2f ,new TweenParms().Prop("Opacity", 1.0f));
    }

	public void SetLoadingFill(bool status) {
		NGUITools.SetActive(loadingFillForeround.gameObject, status);
		loadingFillBackground.fillAmount = 0.0f;
	}

	public void SetLoadingFillAmount(float value) {
		loadingFillBackground.fillAmount = value;
	}


    public void ChainStun(bool status)
    {			
		chainStunLabel.gameObject.SetActive(status);
        //HOTween.To(castNowLabel, 2.0f ,new TweenParms().Prop("Opacity", 0.0f));
    }
	
    public void FindNex()
    {
        findNexLabel.gameObject.SetActive(true);

        //HOTween.To(findNexLabel, 0.2f ,new TweenParms().Prop("Opacity", 1.0f));	
    }
	
    public void TurnOffCastNow()
    {
        if (castNowLabel != null)
            castNowLabel.gameObject.SetActive(false);
    }
	
    public void TurnOffFindNex()
    {
        findNexLabel.gameObject.SetActive(false);
    }
	
    public void AddPlayerDebuff(string debuffName, bool self = true, bool tutorialMode = false)
    {	
        Debug.Log("AddPlayerDebuff > " + debuffName + "self > " + self + "tutorialMode > " + tutorialMode);
        UISprite db = null;		
        switch (debuffName)
        {
			case "leech seed":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, leechwoodPrefab).GetComponent<UISprite>();
                break;
            case "ignite":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, ignitePrefab).GetComponent<UISprite>();
                break;
            case "shield":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, shieldPrefab).GetComponent<UISprite>();
                break;
            case "regen":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, regenPrefab).GetComponent<UISprite>();
                break;
            case "prison":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, prisonPrefab).GetComponent<UISprite>();
                break;
            case "amplify":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, amplifyPrefab).GetComponent<UISprite>();
                break;
            case "drain":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, drainPrefab).GetComponent<UISprite>();
                break;
            case "lock":
                db = NGUITools.AddChild(playerDebufsPanel.gameObject, lockPrefab).GetComponent<UISprite>();
                break;
            default:
                return;
        }
        playerDebuffArray.Add(db);
        //playerDebufsPanel.enabled = true;
        playerDebufsPanel.Reposition();
		
    }
	
    public void AddEnemyDebuff(string debuffName, bool self = true, bool tutorialMode = false)
    {
//		Debug.Log("AddEnemyDebuff > " + debuffName + "self > " + self + "tutorialMode > " + tutorialMode);
		
        UISprite db = null;		
        switch (debuffName)
        {
            case "leech seed":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, leechwoodPrefab).GetComponent<UISprite>();
                break;
            case "ignite":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, ignitePrefab).GetComponent<UISprite>();
                break;
            case "shield":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, shieldPrefab).GetComponent<UISprite>();
                break;
            case "regen":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, regenPrefab).GetComponent<UISprite>();
                break;
            case "prison":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, prisonPrefab).GetComponent<UISprite>();
                break;
            case "amplify":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, amplifyPrefab).GetComponent<UISprite>();
                break;
            case "drain":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, drainPrefab).GetComponent<UISprite>();
                break;
            case "lock":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, lockPrefab).GetComponent<UISprite>();
                break;
            case "haste":
                db = NGUITools.AddChild(enemyDebufsPanel.gameObject, hastePrefab).GetComponent<UISprite>();
                break;
            default:
                break;
        }
        if (db != null)
            enemyDebuffArray.Add(db);
//		enemyDebufsPanel.enabled = true;
        enemyDebufsPanel.Reposition();
    }
	
    public void RemovePlayerDebuff(string debuffName)
    {
        Debug.Log("RemovePlayerDebuff > " + debuffName);
		
        for (int i = 0; i < playerDebuffArray.Count; i++)
        {
            if (playerDebuffArray [i].gameObject.name.Contains("ng" + debuffName + "Prefab(Clone)"))
            {
                UISprite control = playerDebuffArray [i];
                Destroy(control.gameObject);
                playerDebuffArray.RemoveAt(i);
                break;
            }
        }
    }
	
    public void RemoveEnemyDebuff(string debuffName)
    {
//		Debug.Log("RemoveEnemyDebuff > " + debuffName);
		
        for (int i = 0; i < enemyDebuffArray.Count; i++)
        {
            if (enemyDebuffArray [i].gameObject.name.ToLower().Contains(("ng" + debuffName + "Prefab(Clone)").ToLower()))
            {
                UISprite control = enemyDebuffArray [i];
                Destroy(control.gameObject);
                enemyDebuffArray.RemoveAt(i);
                break;
            }
        }		
    }
	
//	IEnumerator UpdateHealthPoints(dfLabel label, float value)
//	{
//		float time = 2f;
//		int current = System.Convert.ToInt32(label.Text);
//		float delay = time / Mathf.Abs(current - value); 
//		if (current < value)
//		{
//			for (int i = current; i <= value; i++)
//			{
//				playerHealthLabel.text = i.ToString();
//				yield return new WaitForSeconds(delay);
//			}
//		}
//		else if (current > value)
//		{
//			for (int i = current; i >= value; i--)
//			{
//				playerHealthLabel.text = i.ToString();
//				yield return new WaitForSeconds(delay);	
//			}
//		}
//	}
	
    public void BattleSpirit(int charge)
    {
//		Debug.Log("BattleSpirit - charge > " + charge);
        SpiritCharge(charge, "spirit");
    }
	
    public void BattleStaff(int charge)
    {
//		Debug.Log("BattleStaff - charge > " + charge);
        SpiritCharge(charge, "staff");		
    }

    private bool allowSpiritButton = false;
    private bool allowStaffButton = false;

    private void SpiritCharge(float charge, string spirit)
    {
        List<UISprite> tempList = null;
        if (spirit.Equals("spirit"))
        {
            tempList = spiritsList;
            if (charge >= 6)
            {
//				spiritsButton.IsInteractive = true;
                allowSpiritButton = true;
                spiritsButton.SetState(UIButtonColor.State.Normal, true);
            } else
                allowSpiritButton = false;
        } else if (spirit.Equals("staff"))
        {
            tempList = staffList;
            if (charge >= 6)
            {
                staffButton.SetState(UIButtonColor.State.Normal, true);
                allowStaffButton = true;
            } else
                allowStaffButton = false;
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            if (i >= charge)
            {
                tempList [i].gameObject.SetActive(false);
            } else
            {
                //HOTween.To(tempList[i], 1.0f,new TweenParms().Prop("IsVisible", true).Delay(0.2f));
                tempList [i].gameObject.SetActive(true);
            }
        }
    }	
	
    public void RingNArrow(GameObject Circle, float angle)
    {
		
    }
	
    public void RingNArrowOff(GameObject Circle)
    {
		
    }
	
    public void RingAnimation(GameObject obje)
    {
		
    }
	
	
    public void SpiritFightTutorialStart()
    {
		
    }
	
    public void SpiritFightTutorial2()
    {
		
    }
	
    public void SpiritFightTutorialEnd()
    {
		
    }
	
    public void SpiritFightTutorialEnding()
    {
		
    }
	
    public void SetLanguage(string strInput, string lan)
    {
		
    }
	
    public void OnClick(UIButton button)
    {
        if (button == staffButton && allowStaffButton)
        {
            _battleInterface.StaffCharged();
			SpiritEnable(false);
        } else if (button == potionsButton)
        {
            _battleInterface.usePotionInBattle(true);
        } else if (button == spiritsButton && allowSpiritButton)
        {
            //add call to interface's spirit button event handler...
            _battleInterface.SpiritCharged();
			StaffEnable(false);

//			BattleSpirit(0);
        }
    }
	
    public void updateLoadingBar(float val)
    {
		
    }
	
    public void SetVisible(bool yesNo)
    {
        gameObject.SetActive(yesNo);


		if(GameManager.instance._levelManager.battleManager.IsBattleEnded) {
			if(playerHealthAlphaTween != null) {
				playerHealthAlphaTween.Kill();
			}
			if(enemyHealthAlphaTween != null) {
				enemyHealthAlphaTween.Kill();
			}
			List<IHOTweenComponent> list = HOTween.GetTweensById("playerGlow", false);
			foreach (IHOTweenComponent ihot in list) {
				ihot.Kill();
			}
			list = HOTween.GetTweensById("enemyGlow", false);
			foreach (IHOTweenComponent ihot in list) {
				ihot.Kill();
			}
			playerHealthBar.alpha = 1.0f;
			playerHealthMaskBar.alpha = 1.0f;
			enemyHealthBar.alpha = 1.0f;
			enemyHealthMaskBar.alpha = 1.0f;
			Debug.LogError("SetVisible BAttleUI - "+yesNo);



			var diff = DateTime.Now - FRAG.NetworkingNEW.dt;
			string totalTime = diff.TotalMilliseconds.ToString();
			FRAG.NetworkingNEW.dt = DateTime.Now;
			Debug.LogError("Total time taken for asset and scene loading - "+totalTime);
			float val = PlayerPrefs.GetFloat("LevelTime", 0);
//			float.Parse(PlayerPrefs.GetString("LevelTime"), System.Globalization.CultureInfo.InvariantCulture); 
			if(val < (float)diff.TotalMilliseconds)
				PlayerPrefs.SetFloat("LevelTime", (float)diff.TotalMilliseconds);

			BattleSpirit(GameManager.instance.spiritProgress);
			ResetRuneSpells();
		} else {
			Debug.LogError("isBattleEnded = false");
		}
		//GetComponent<dfPanel>().IsVisible = yesNo;
    }

    public void ResetGrids()
    {
//		Debug.LogError("Resetting grids - Battle UI");

        List<GameObject> objectsToDelete = new List<GameObject>();
        playerWardsPanel.GetChildList().ForEach(trans => objectsToDelete.Add(trans.gameObject));
        playerDebufsPanel.GetChildList().ForEach(trans => objectsToDelete.Add(trans.gameObject));
        enemyDebufsPanel.GetChildList().ForEach(trans => objectsToDelete.Add(trans.gameObject));

        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            Destroy(objectsToDelete [i]);
        }

        if (wardArray != null)
            wardArray.Clear();
        if (playerDebuffArray != null)
            playerDebuffArray.Clear();
        if (enemyDebuffArray != null)
            enemyDebuffArray.Clear();
    }
}
