using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public const string firstLevel = "BeachCamp";
    public ZoneModel _zone;
    public BattleManager battleManager;
	
    public Transform player = null;
    public  Transform enemy;
    public string currentLevel;
	
    List<POIModel> poi = new List<POIModel>();
    List<string> PoiListForLevel;
    List<GameObject> LOCKED = new List<GameObject>();
	
	
    public GameObject daySkybox;
    Texture2D BeachCampLightMapDay;
    GameObject currentLevelShrine = null;
	
    protected LevelCameraHandler _levelCameras;
    protected AILoader _aiLoader;
    protected Level _level;
    bool isEnemyDefeated = false;
    public AssetBundleLoader LevelsAssetBundles;
	
    protected Ray ray;
    protected RaycastHit hit;
    Vector3 touchPos1 = Vector3.zero;
    Vector3 touchPos2 = Vector3.zero;
	
    GameObject BeachCutScene;
    GameObject BeachCutSceneNex;
	
    public bool isEnemyClicked;
    public string lastCollidedTag = "";
    protected bool isHitRecieved = false;
    protected bool _isNeutral = false;

    public bool isPlayerNeutral
    {
        set
        {
            _isNeutral = value;
            if (_isNeutral)
            {
				if (GameManager.instance.scaleformCamera.generalSwf != null && !TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted) && TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None)
				{
					if (GameManager.instance._levelManager.currentLevel.Equals("ToHollowTree") && GameManager._gameState.skullLevel == 1 && isEnemyDefeated)
					{
						Debug.Log("Play battle tutorial!!!");
						
						//GameManager.instance.scaleformCamera.generalSwf.playBattleTutorial();
						TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.BattleEndTutorialStart);
						//TutorialManager.instance.currentTutorial=TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted;
						poiState = AllowedPoiTypes.kNone;
						Debug.LogError("Gamecenter Init called!!!");
						GameCenter.instance.Init();
					}
				}

				//if(GameManager.PRINT_LOGS) Debug.Log("PLAYER IS NEUTRALLLLL - TIME TO RUN TUTORIAL!!!");
                if (!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.PotionTutorialStart) && TutorialManager.instance.currentTutorial == TutorialManager.TutorialsAndCallback.None)
                {
                    User TempUser = GameManager._gameState.User;
                    if (TempUser.life < TempUser.totalLife * 0.3f)
                    {

                        TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.PotionTutorialStart);
                        poiState = AllowedPoiTypes.kNone;
                    }
                }
				
            }
        }
        get
        {
            return _isNeutral;
        }
    }
    bool isCutScenePlaying = false;
    bool isPlayCaveEntranceOtherAnim = false;
    protected int POINumber = -1;

    protected GameObject levelLoaded;
    Transform playerNeutralPoint;
    AnimationClip currentSceneAnimation;
	
    LightmapData[] lightmap = new LightmapData[1];
	
    private bool isAILoading = false;
    private bool allowTransition = false;
	
    public enum AllowedPoiTypes
    {
        kDuel,
        kExit,
        kLoot,
        kShrine,
        kAll,
        kNone}
    ;
	
    public AllowedPoiTypes poiState = AllowedPoiTypes.kAll;


    protected Cutscene cutsceneToPlay = null;

    void OnEnable()
    {
        LevelManagerAwake();
        LevelManagerStart();
    }

    void OnDisable()
    {
		
    }

    /// <summary>
    /// Setting the event onAssetBundleRecieved and calling the DecideNextLevel.
    /// </summary>
    protected void LevelManagerAwake()
    {
        PlayMakerFSM.BroadcastEvent("FadeInEvent");
		Debug.LogWarning("FadeIn LevelManagerAwake - OnEnable");

		
        if (!PlayerPrefs.HasKey("AssetBundleLoadedForFirstTime"))
        { // PlayerPref to check if the game is started for first time after install
            PlayerPrefs.SetInt("AssetBundleLoadedForFirstTime", 0);
        }
        _levelCameras = new LevelCameraHandler();
        _levelCameras.sfCamera = GameManager.instance.scaleformCamera;//Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
		
        LevelsAssetBundles = this.GetComponent<AssetBundleLoader>();
        LevelsAssetBundles.SetDelegate(this.onAssetBundleRecieved);
		
        DecideNextLevel();
        _levelCameras._poiCam = this.OnNextCameraLoaded;
        if (GameManager.PRINT_LOGS)
            Debug.Log("LEVEL MANAGER AWAKE :::::: CALLING Init() !!!");
        StartCoroutine(Init());
		
        if (!GameManager.instance.isMultiPlayerMode)
            GameManager.instance._levelManager = gameObject.GetComponent<LevelManager>();

    }

    /// <summary>
    /// Starts the Level Manager.
    /// </summary>
    protected void LevelManagerStart()
    {
        if (daySkybox == null)
            daySkybox = GameObject.Find("sky_01");
		
        GameManager.instance._monoHelpers.mainDirectionalLight = GameObject.Find("Directional light").light;
		Debug.LogError("::::::::::: LoadMarket Data called - - - - - - - -");
		PurchaseManager.Instance.LoadMarketData();
    }

    /// <summary>
    /// This function is called when we receive the bundles from the server seccussfully.
    /// </summary>
    /// <param name="bundle">Bundle.</param>
    public  void onAssetBundleRecieved(AssetBundle bundle)
    {
        //tempBundle = bundle;
        if (bundle)
        {
            StartCoroutine(OnAssetBundleProcess(bundle));
        } else
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("Bundle Was NULL invoking again");
            LevelsAssetBundles.InvokeLoadAssetBundle(currentLevel);
        }
    }

    /// <summary>
    /// Loading the Shrines from the Bundle.
    /// </summary>
    /// <returns>The for shrines in bundle.</returns>
    /// <param name="abRequest">Ab request.</param>
    /// <param name="bundle">Bundle.</param>
    IEnumerator CheckForShrinesInBundle(AssetBundleRequest abRequest, AssetBundle bundle)
    {
        switch (currentLevel)
        {
            case "BeachCamp":
                abRequest = bundle.LoadAsync("BeachCamp_Shrine_Point", typeof(GameObject));
                yield return abRequest;
                currentLevelShrine = Instantiate(abRequest.asset as GameObject) as GameObject;
                currentLevelShrine.tag = "FourthShrine";
                break;
            case "HollowTree":
                abRequest = bundle.LoadAsync("HollowTree_Shrine_Point", typeof(GameObject));
                yield return abRequest;
                currentLevelShrine = Instantiate(abRequest.asset as GameObject) as GameObject;
                currentLevelShrine.tag = "FirstShrine";
                break;
            case "ThreeGods":
                abRequest = bundle.LoadAsync("ThreeGods_Shrine_Point", typeof(GameObject));
                yield return abRequest;
                currentLevelShrine = Instantiate(abRequest.asset as GameObject) as GameObject;
                currentLevelShrine.tag = "SecondShrine";
                break;
            case "ForbiddenCave":
                abRequest = bundle.LoadAsync("ForbiddenCave_Shrine_Point", typeof(GameObject));
                yield return abRequest;
                currentLevelShrine = Instantiate(abRequest.asset as GameObject) as GameObject;
                currentLevelShrine.tag = "ThirdShrine";
                break;
        }
        yield return null;
    }

    /// <summary>
    /// Raises the asset bundle process event on receiving the Bundles from the server.
    /// Loading the Bundles.
    /// </summary>
    /// <param name="bundle">Bundle.</param>
    IEnumerator OnAssetBundleProcess(AssetBundle bundle)
    {
        AssetBundleRequest abRequest = bundle.LoadAsync(currentLevel, typeof(GameObject));
        //bundleObject=bundle.LoadAsync(currentLevel, typeof(GameObject)) as GameObject;
        yield return abRequest;
		
        GameObject bundleObject = abRequest.asset as GameObject;
		
        lightmap [0] = new LightmapData();
        //if(GameManager.PRINT_LOGS) Debug.Log("beachcamp");
		
        StartCoroutine(CheckForShrinesInBundle(abRequest, bundle));
		
        if (currentLevel.Equals("BeachCamp"))
        {
            abRequest = bundle.LoadAsync("LightmapFar-0-night", typeof(Texture2D));
            yield return abRequest;
            lightmap [0].lightmapFar = abRequest.asset as Texture2D;
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color32(0, 39, 84, 255);
            RenderSettings.fogDensity = 0.02f;
            RenderSettings.ambientLight = new Color32(165, 165, 165, 255);
            RenderSettings.skybox = Resources.Load("NightSky") as Material;
			
            abRequest = bundle.LoadAsync("LightmapFar-0-day", typeof(Texture2D));
            yield return abRequest;
            BeachCampLightMapDay = abRequest.asset as Texture2D;
			
            abRequest = bundle.LoadAsync("BeachCampWaterPrefab", typeof(GameObject));
            yield return abRequest;
            GameObject tempWater = Instantiate(abRequest.asset as GameObject) as GameObject;
			
            abRequest = bundle.LoadAsync("BeachCampCampPrefab", typeof(GameObject));
            yield return abRequest;
            GameObject tempCamp = Instantiate(abRequest.asset as GameObject) as GameObject;
			
            abRequest = bundle.LoadAsync("AlchemyMeditation", typeof(GameObject));
            yield return abRequest;
            GameObject tempAlchemy = Instantiate(abRequest.asset as GameObject) as GameObject;
			
            LightmapSettings.lightmaps = lightmap;
		
            abRequest = bundle.LoadAsync("LightProbes", typeof(LightProbes));
            yield return abRequest;
            LightmapSettings.lightProbes = abRequest.asset as LightProbes;
			
            abRequest = bundle.LoadAsync("BeachCutscene", typeof(GameObject));
            yield return abRequest;
            BeachCutScene = abRequest.asset as GameObject;
			
            abRequest = bundle.LoadAsync("BeachCutsceneNex", typeof(GameObject));
            yield return abRequest;
            BeachCutSceneNex = abRequest.asset as GameObject;
			
            LoadNextLevel(bundleObject);
			
            if (currentLevelShrine != null)
            {
                currentLevelShrine.transform.parent = levelLoaded.transform;
            }
			
            tempWater.transform.parent = levelLoaded.transform.Find("ELEMENTS");
            tempCamp.transform.parent = levelLoaded.transform.Find("ELEMENTS");
            tempAlchemy.transform.parent = levelLoaded.transform;
			
			
            SoundManager.instance.playDefaultBackGround();
        } else
        {	
            /*AssetBundleRequest lightProbeRequest; 
			lightProbeRequest = bundle.LoadAsync("LightProbes",typeof(LightProbes));
			yield return lightProbeRequest;
			
			
			abRequest=bundle.LoadAsync("LightmapFar-0",typeof(Texture2D));
			yield return abRequest;
			lightmap[0].lightmapFar=abRequest.asset as Texture2D;
			*/
            //if(false/*currentLevel.Equals("ArcanumRuhalis")*/)
            /*{
				GameManager.instance.ruhalisLightMap = lightmap[0].lightmapFar;
				
				GameManager.instance.ruhalisLightProbe = lightProbeRequest.asset as LightProbes;
			}
			else
			{
				LightmapSettings.lightmaps=lightmap;
				LightmapSettings.lightProbes=lightProbeRequest.asset as LightProbes;
			}
			
			GameManager.instance._monoHelpers.SetCurrentLevelsRendererSettings();
			
			LoadNextLevel(bundleObject);
			
			if(currentLevelShrine!=null)
			{
				currentLevelShrine.transform.parent = levelLoaded.transform;
			}*/

            abRequest = bundle.LoadAsync("LightmapFar-0", typeof(Texture2D));
            yield return abRequest;
            lightmap [0].lightmapFar = abRequest.asset as Texture2D;
            GameManager.instance._monoHelpers.SetCurrentLevelsRendererSettings();
			
            LightmapSettings.lightmaps = lightmap;
		
            abRequest = bundle.LoadAsync("LightProbes", typeof(LightProbes));
            yield return abRequest;
            LightmapSettings.lightProbes = abRequest.asset as LightProbes;
			
            LoadNextLevel(bundleObject);
			
            if (currentLevelShrine != null)
            {
                currentLevelShrine.transform.parent = levelLoaded.transform;
            }
        }
		UIManager.instance.LoadGuildUI();
		Debug.LogError("LoadGuildUI asset bundle loaded >> " + currentLevel);
		
        if (currentLevel == "BeachCamp")
        {
			//TODO Will work but find a better place to call this
			if(UIManager.instance.guildUI != null)
				NGUITools.SetActive(UIManager.instance.guildUI.gameObject, false);

			UIManager.instance.ShowMainScreenMenu(() => {
                StartStoryMode();
            }, () => {
                StartMultiPlayerMode();
				if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.PotionTutorialStart)) {
					TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.PotionTutorialStart, true);	
				}
			});
			//TODO Check Logic for FastForward
			if(UIManager.instance.levelScene != null && UIManager.instance.levelScene.levelScene != null)
				UIManager.instance.levelScene.levelScene.SetCanSkip(false);

		}
    }
	
    /// <summary>
    /// Loads the next level.
    /// </summary>
    /// <param name="bundleObject">Bundle object.</param>
    public void LoadNextLevel(GameObject bundleObject)
    {
		Debug.LogError("[LevelManager] LoadNextLevel ::: current level > " + currentLevel);
        LoadBundleObject(bundleObject);
        InstantiateLevelCameras();
        GeneratePointOfIntrests();
		
        if (!isAILoading)
        {
            SnycPOIWithGameState();
            ActiveUnProtectedPOIAtStart();
			Debug.LogError("LoadNextLevel >>>>>>>>>>>>>>>>>>>>>>>>> " + currentLevel);
			CheckLootList();
            GameManager._gameState.LevelState.UpdateLevelState(currentLevel, _level.zoneID, poi);
            GameManager.instance.SaveGameState(false);
            GameManager.instance.isGameLoadedForFirstTime = true;
            playerNeutralPoint = getTransform("PlayerNeutralPoint");
            if (currentLevel != "BeachCamp")
            {
                DeleteDynamicBeachcampObjects();
				
                if (player == null)
                {
                    OnUserModelLoaded();
                } else
                {
                    if (currentLevel.Equals("ArcanumRuhalis"))
                    {
                        //player.localScale*=0.17f;
                        //GameManager.instance.isMultiPlayerMode=true;
                        //daySkybox.SetActive(false);
                        SoundManager.instance.StopAmbientSound();
                        isCutScenePlaying = true;
                        RenderSettings.skybox = Resources.Load("DSR") as Material;
//						Debug.LogError("1 IsCutscenePlaying "+isCutScenePlaying);
                    }
                    player.position = getTransform("PlayerEntryPoint").position;
                    player.rotation = getTransform("PlayerEntryPoint").rotation;
                }
				
                GameManager.instance._monoHelpers.CheckForSwfsExistance();
            }
			
			
            _levelCameras.sfCamera.generalSwf.UnLoadLoadingScreen();
            if (currentLevel.Equals("BeachCamp"))
            {
//				UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
                //hide Gen swf top ring...
                _levelCameras.sfCamera.generalSwf.ToggleTopStats(false);
                daySkybox.SetActive(false);	
                ShowMainScreen();
            } else
            {
                if (_levelCameras.sfCamera.levelScene != null)
                {
                    //UnityEngine.Debug.Log(">>> SetDisplayVisible > T");
                    _levelCameras.sfCamera.levelScene.SetDisplayVisible(true);
                }
				
                if (player != null)
                    PlayEntryAnimation();
            }
            if (_levelCameras.sfCamera.generalSwf != null)
                _levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);
            Resources.UnloadUnusedAssets();
        }
		
//        if (GameManager.PRINT_LOGS) Debug.Log("Listing before clearing!!! ----                   1");
        ListLevelObjects();
		
        ClearBeachClones();
    }

    /// <summary>
    /// Deletes the Object fromthe Last scene.
    /// </summary>
    private void ClearBeachClones()
    {
        if (currentLevel.Equals("ToHollowTree"))
        {
            DeleteDynamicBeachcampObjects();
        }
    }

    /// <summary>
    /// Continues the loading level.
    /// </summary>
    public void ContinueLoadingLevel()
    {
		
//		Debug.LogError("ContinueLoadingLevel >>>>>>>>>>>>>>>>>> " + currentLevel);
        SnycPOIWithGameState();
        ActiveUnProtectedPOIAtStart();
        CheckLootList();
        GameManager._gameState.LevelState.UpdateLevelState(currentLevel, _level.zoneID, poi);
        GameManager.instance.SaveGameState(true);
        GameManager.instance.isGameLoadedForFirstTime = true;
        playerNeutralPoint = getTransform("PlayerNeutralPoint");
        if (currentLevel != "BeachCamp")
        {
            if (player == null)
            {
                OnUserModelLoaded();
                return;
            } else
            {
                player.position = getTransform("PlayerEntryPoint").position;
                player.rotation = getTransform("PlayerEntryPoint").rotation;
            }
			
            GameManager.instance._monoHelpers.CheckForSwfsExistance();
        }
        _levelCameras.sfCamera.generalSwf.UnLoadLoadingScreen();
        if (currentLevel.Equals("BeachCamp"))
        {
            //_levelCameras.sfCamera.generalSwf.ToggleTopStats(false);
            daySkybox.SetActive(false);
            ShowMainScreen();
        } else
        {
            PlayEntryAnimation();
        }
        _levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);
        Resources.UnloadUnusedAssets();
	
        isAILoading = false;
    }

    /// <summary>
    /// Continues the loading level after hero loaded.
    /// </summary>
    public void ContinueLoadingLevelAfterHeroLoaded()
    {
        //player = Instantiate bundle.mainAsset as Transform;
        //player =  (Instantiate(bundle.mainAsset as GameObject, getTransform("PlayerEntryPoint").position, getTransform("PlayerEntryPoint").rotation) as GameObject).transform;
        player.position = getTransform("PlayerEntryPoint").position;
        player.rotation = getTransform("PlayerEntryPoint").rotation;
        player.gameObject.layer = LayerMask.NameToLayer("DynamicCharLayer");
		
        if (currentLevel.Equals("ArcanumRuhalis"))
        {
            //player.localScale*=0.17f;
            //daySkybox.SetActive(false);
            SoundManager.instance.StopAmbientSound();
            RenderSettings.skybox = Resources.Load("DSR") as Material;
            //player.gameObject.SetActive(true);
            //GameManager.instance.isMultiPlayerMode=true;
        }
		
        player.gameObject.GetComponent<SRCharacterController>()._user = GameManager._gameState.User;
		
        player.position = getTransform("PlayerEntryPoint").position;
        player.rotation = getTransform("PlayerEntryPoint").rotation;
						
        GameManager.instance._monoHelpers.CheckForSwfsExistance();
			
			
        _levelCameras.sfCamera.generalSwf.UnLoadLoadingScreen();
        if (currentLevel.Equals("BeachCamp"))
        {
            //_levelCameras.sfCamera.generalSwf.ToggleTopStats(false);
			
            UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
            UIManager.instance.generalSwf.ToggleTopStats(true);
            if (UIManager.instance.hud != null)
            {
                UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
                UIManager.instance.hud.SetDisplayVisible(false);
            }
            daySkybox.SetActive(false);
            ShowMainScreen();
        } else
        {
            PlayEntryAnimation();
        }
        _levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);
        Resources.UnloadUnusedAssets();
	
        isAILoading = false;
		
			
        if (GameManager.PRINT_LOGS)
            Debug.Log("Listing before clearing!!! ----                   2");
        ListLevelObjects();
		
		
    }
	 
    void ShowMainScreen()
    {
        //_levelCameras.sfCamera.LoadGeneralSwf(GameManager.instance.OnGeneralSwfReloaded);
    }
	
    public void StartStoryMode()
    {
        PlayMakerFSM.BroadcastEvent("FadeOutInstantEvent");
		Debug.LogWarning("FadeOut StartStoryMode");
		Destroy(GameObject.Find("Sorcerer"));
		
        GameManager.instance.StartCoroutine(GameManager.instance._monoHelpers.LoadFromFile("Cutscenes.json", true, LoadCutscene));
		Debug.LogError ("~~~~~~~~~~~~~~~~~~~StartStoryMode~~~~~~~~~~~~~~~~~~~~~");
//		GameManager.instance.scaleformCamera.LoadGuildUI();
    }
	
    private void LoadCutscene(bool isError, string fileContent)
    {
        List<Cutscene> cutscenes = Cutscenes.ParseCutscenes(fileContent);
        Cutscenes.SupplyValues(cutscenes, GetValues());
        Cutscene cutscene = Cutscenes.SelectCutscene(cutscenes);
        if (cutscene != null)
        {
            if (cutscene.Place == "BeachHass")
            {
                ReplaceSkybox();
                GameObject obj = GameManager.Instantiate(BeachCutScene) as GameObject;
                CutsceneHandler handler = obj.GetComponent<CutsceneHandler>();
                handler.cutscene = cutscene;
                handler.StartCutscene();
                PlayMakerFSM.BroadcastEvent("FadeInEvent");
				Debug.LogWarning("FadeIn BeachHass");
            } else if (cutscene.Place == "BeachNex")
            {
                GameObject obj = GameManager.Instantiate(BeachCutSceneNex) as GameObject;
                NexCutsceneHandler handler = obj.GetComponent<NexCutsceneHandler>();
                handler.Cutscene = cutscene;
                handler.StartCutscene();
                PlayMakerFSM.BroadcastEvent("FadeInEvent");
				Debug.LogWarning("FadeIn BeachNex");
			}
        } else
        {
            ReplaceSkybox();
            ContinueLoadingStoryMode();
        }
    }

    public void ReplaceSkybox()
    {
        daySkybox.SetActive(false);
        Resources.UnloadAsset(RenderSettings.skybox);
        RenderSettings.skybox = null;
        daySkybox.SetActive(true);
    }

    private Dictionary<string, object> GetValues()
    {
        Dictionary<string, object> values = new Dictionary<string, object>();
        values.Add("day", (long)GameManager._gameState.dayCount);
        values.Add("nexDefeated", (long)Mathf.Max(0, Int32.Parse(GameManager._gameState.bossAttemptDictionary ["PRIMUS_NEX"].ToString()) - 1));
        return values;
    }
	
    public void ContinueLoadingStoryMode()
    {
        GameManager.instance.isMultiPlayerMode = false;
        GameManager.instance.currentMultiPlayerLevel = "";
        Debug.Log("ContinueLoadingStoryMode");
        if (UIManager.instance.levelScene != null)
        {
            Debug.Log("HideMonsterIcon");
            UIManager.instance.levelScene.HideMonsterIcon();
        }
        UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
        _levelCameras.sfCamera.generalSwf.ToggleTopStats(true);
        _levelCameras.defaultCamera.gameObject.SetActive(false);
        _levelCameras.defaultCamera = levelLoaded.transform.FindChild("NeutralCamera").camera;
        _levelCameras.defaultCamera.gameObject.SetActive(true);
        _levelCameras.ActiveCamera = _levelCameras.defaultCamera;
        _levelCameras.ActiveCamera.tag = "MainCamera";

        if (player == null)
        {
            OnUserModelLoadedBeachCamp();
            return;
        } else
        {
            PlayMakerFSM.BroadcastEvent("FadeOutInstantEvent");
			Debug.LogWarning("FadeOut ContinueLoadingstoryMode");
			player.position = getTransform("PlayerEntryPoint").position;
            player.rotation = getTransform("PlayerEntryPoint").rotation;
		}
		
        GameManager.instance._monoHelpers.CheckForSwfsExistance();
        lightmap [0] = new LightmapData();
        lightmap [0].lightmapFar = BeachCampLightMapDay;
		
        LightmapSettings.lightmaps = lightmap;
        RenderSettings.fogColor = new Color32(60, 60, 60, 255);
        StartCoroutine(WaitForTimeAndPlayEntryAnimation(1.5f));
		Debug.LogWarning("FadeIn ContinueLoadingStoryMode");
	}
	
    public void ContinueStartGame(GameObject playerModelObject)
    {
		
        playerModelObject.transform.parent = player.transform;
        playerModelObject.transform.localPosition = Vector3.zero;
		
        PlayMakerFSM.BroadcastEvent("FadeOutInstantEvent");
		Debug.LogWarning("FadeOut ContinueStoryMode");
		//player =  (Instantiate(bundle.mainAsset as GameObject, getTransform("PlayerEntryPoint").position, getTransform("PlayerEntryPoint").rotation) as GameObject).transform;
        player.position = getTransform("PlayerEntryPoint").position;
        player.rotation = getTransform("PlayerEntryPoint").rotation;
        player.gameObject.layer = LayerMask.NameToLayer("DynamicCharLayer");
		
        player.gameObject.GetComponent<SRCharacterController>()._user = GameManager._gameState.User;	
        GameManager.instance._monoHelpers.CheckForSwfsExistance();
        lightmap [0] = new LightmapData();
        lightmap [0].lightmapFar = BeachCampLightMapDay;
		
        LightmapSettings.lightmaps = lightmap;
        RenderSettings.fogColor = new Color32(60, 60, 60, 255);
        GameManager.instance.scaleformCamera.generalSwf.UnLoadLoadingScreen();
        StartCoroutine(WaitForTimeAndPlayEntryAnimation(1.5f));
		Debug.LogWarning("FadeIn ContinueStartGame");
	}
	
    public void LoadBundleObject(GameObject loadObject)
    {
//		Debug.Log("loadObject==="+loadObject.name);
        if (!currentLevel.Equals("ToDemonFalls"))
        {
            if (levelLoaded == null)
                levelLoaded = (GameObject)Instantiate(loadObject, Vector3.zero, Quaternion.identity);
        } else
        {
            if (levelLoaded == null)
                levelLoaded = (GameObject)Instantiate(loadObject, Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 60.0f, 0.0f)));
        }
        StaticBatchingUtility.Combine(levelLoaded);
		
        levelLoaded.SetActive(true);
		
        levelLoaded.transform.FindChild("Terrain").gameObject.layer = LayerMask.NameToLayer("DynamicCharLayer");
		
        if (currentLevel.Equals("DemonFalls"))
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("loadRingFromResources!");
            GameObject tempCinematic = Instantiate(Resources.Load("Monster_Intros/_prefabs/Sequence_Intro_DemonFalls", typeof(GameObject))) as GameObject;
            tempCinematic.transform.parent = levelLoaded.transform;
            if (GameManager.PRINT_LOGS) 
            if (tempCinematic == null)
                Debug.Log("CANT FIND CINEMATIC! WTF!!!");
			
            GameObject tempCinematicOutro = Instantiate(Resources.Load("Monster_Intros/_prefabs/Sequence_Outro_DemonFalls", typeof(GameObject))) as GameObject;
            tempCinematicOutro.transform.parent = levelLoaded.transform;
        }
        //PlayMakerFSM.BroadcastEvent("FadeInEvent"); // 1.
    }
	
    public void PlayOutroSequence()
    {
        if (GameObject.FindGameObjectWithTag("Cinematic_Outro"))
        {
			
            //GameObject CinematicObj =  Instantiate(Resources.Load("Monster_Intros/_prefabs/Sequence_Intro_DemonFalls",typeof(GameObject))) as GameObject;
            GameObject CinematicObj = GameObject.FindGameObjectWithTag("Cinematic_Outro");
            CinematicObj.SendMessage("PlaySequence");
            poi [enemyIndexTemp].relatedObject.SetActive(false);
            //player.gameObject.SetActive(false);
            CinematicObj.SetActive(true);
            _levelCameras.ActiveCamera.gameObject.camera.enabled = false;
            return;
        }
    }
	
    public void OnOutroCompleted()
    {
        StartCoroutine(battleManager.WaitForEndAnimationAndReturn(3.0f, true));
        if (GameManager.PRINT_LOGS)
            Debug.Log("On Outro Completed!");
    }
	
    void InstantiateLevelCameras()
    {
        levelLoaded.SetActive(true);
        if (currentLevel.Equals("BeachCamp"))
        {
            _levelCameras.defaultCamera = GameObject.Find("EntryPointCameraNight").camera;
            levelLoaded.transform.FindChild("NeutralCamera").gameObject.SetActive(false);
            GameObject [] playerObjects = GameObject.FindGameObjectsWithTag("Player");
//			Debug.Log("playerObjects in beachcamp > " + playerObjects.Length);
            Array.ForEach<GameObject>(playerObjects, playerObject => {
                Sorcerer_CinematicControlScript animController = playerObject.GetComponent<Sorcerer_CinematicControlScript>();
                if (animController != null)
                    animController.startYoga();
            });
        } else
        {
            _levelCameras.defaultCamera = levelLoaded.transform.FindChild("NeutralCamera").camera;
            _levelCameras.defaultCamera.animation.enabled = false;
			
            if (_levelCameras.defaultCamera.GetComponent<RotateCamera>() != null)
                _levelCameras.defaultCamera.GetComponent<RotateCamera>().enabled = false;
			
            if (_levelCameras.defaultCamera.transform.FindChild("POICamera") != null)
                _levelCameras.defaultCamera.transform.FindChild("POICamera").gameObject.SetActive(false);
			
            if (GameManager.instance.isMultiPlayerMode && _levelCameras.defaultCamera.GetComponent<SmoothLookAt>() != null)
                _levelCameras.defaultCamera.GetComponent<SmoothLookAt>().enabled = false;
        }
		
        _levelCameras.defaultCamera.gameObject.SetActive(true);
        _levelCameras.ActiveCamera = _levelCameras.defaultCamera;
        _levelCameras.ActiveCamera.tag = "MainCamera";
    }

//	private void OnGameStateReceived(GameStateModule.GameState gState)
//	{
//		_gameState = gState;
//	}
//	
//	private IEnumerator InitGameState()
//	{
//		_gameState = GameManager._gameState;
//		
//		yield return null;
//	}
//	
	public void DebugInit() {
//		StartCoroutine(Init ());default
		StartCoroutine(ExitToNextLevel(1.3f));
	}

    protected IEnumerator Init()
    {
//        if (GameManager.PRINT_LOGS) Debug.Log("Init() CALLED!!!");
        //if(GameManager.PRINT_LOGS) Debug.Log("loading INIT");
        //	yield return StartCoroutine(InitGameState());
        //if(GameManager.PRINT_LOGS) Debug.Log("loading gamestate");
		
        if (!GameManager.instance.isGameLoadedForFirstTime)
        {
			_level = GameManager._dataBank.GetLevelDetails(GameManager._gameState.LevelState.levelID);//LevelStateLoader.GenerateLevelData(poi);
            currentLevel = _level.levelID;
            GameManager._gameState.LevelState.UpdateLevelState(currentLevel, _level.zoneID, poi);
		} else
        {
			int debugValue = PlayerPrefs.GetInt("DEBUGNEXTLEVEL", 0);
			if(Debug.isDebugBuild && debugValue == 1) {
				PlayerPrefs.SetInt("DEBUGNEXTLEVEL", 0);
				currentLevel = GameManager._gameState.LevelState.levelID;
			}
			_level = GameManager._dataBank.GetLevelDetails(currentLevel);
		}
//		Debug.LogError(_level.levelID+" "+_level.zoneID+" "+_level.poiNameList.Count);
        _zone = GameManager._dataBank.GetZoneDetails(_level.zoneID);
        PoiListForLevel = _level.poiNameList;
        _aiLoader = GameObject.FindGameObjectWithTag("AILoader").GetComponent<AILoader>();
        _aiLoader.enemyNameList = _zone.enemyList;
        if (GameManager.PRINT_LOGS)
            Debug.Log("About To Load Asset Bundle");
        //lightmap[0].lightmapFar=null;
        LevelsAssetBundles.UnloadAssetBundle(currentLevel); 
        yield return new  WaitForSeconds(1.0f);
//		if(GameManager.PRINT_LOGS) Debug.Log("loading level : " + currentLevel);
        LevelsAssetBundles.InvokeLoadAssetBundle(currentLevel);
    }
	
    public void OnApplicationQuit()
    {
        PurchaseManager.Instance.Destroy();
        ServerManager.Instance.Destroy();
        //Time.timeScale=0.0f;
        PlayMakerFSM.BroadcastEvent("FadeOutInstantEvent");
		Debug.LogWarning("FadeOut OnApplicationQuit");
		Application.Quit();
        if (GameManager.PRINT_LOGS)
            Debug.Log("~~~~~~~~~~Application Quit~~~~~");
        //GameManager.instance.SaveGameState();
        //lightmap[0].lightmapFar=null;
        //LevelsAssetBundles.UnloadAssetBundle(currentLevel);
        Application.CancelQuit();
        Resources.UnloadUnusedAssets();
        StartCoroutine(WaitForTime(2.0f));

		
    }
	
    private void LoadTreasureObject(Transform poiPoint, int index, string chestType)
    {
        switch (chestType)
        {
            case "1 KEY":
                poi [index].relatedObject = Instantiate(GameManager.instance.LoadPrefab("CommonTreasureChest"), poiPoint.position, poiPoint.rotation) as GameObject;
                break;
            case "3 KEYS":
                poi [index].relatedObject = Instantiate(GameManager.instance.LoadPrefab("RareTreasureChest"), poiPoint.position, poiPoint.rotation) as GameObject;
                break;
            case "5 KEYS":
                poi [index].relatedObject = Instantiate(GameManager.instance.LoadPrefab("SuperRareTreasureChest"), poiPoint.position, poiPoint.rotation) as GameObject;
                break;
        }
    }

    private void OnAILoaded(GameObject enemyObject, Vector3 atPostion, Quaternion atAngle, AIModel aiModel, int index)
    {	
        if (enemyObject == null)
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("ENEMY OBJECT NULL");
        }
        if (GameManager.PRINT_LOGS)
            Debug.LogError("Enemy name " + aiModel.name+" "+aiModel.skullLevel);
        enemy = (Instantiate(enemyObject, atPostion, atAngle) as GameObject).transform;
        enemy.gameObject.GetComponent<AICharacterControllerNEWAgain>()._aiModel = aiModel;
        GameManager._gameState.LevelState.UpdateEnemyList(poi [index].poiId, aiModel.name);
        enemy.gameObject.collider.enabled = false;
        enemy.gameObject.layer = LayerMask.NameToLayer("DynamicCharLayer");
        enemy.LookAt(poi [index].poiMoveTo.position);
		
        if (currentLevel.Equals("DemonFalls"))
            enemy.gameObject.SetActive(false);
			
        poi [index].relatedObject = enemy.gameObject;
        ContinueLoadingLevel();
    }

    protected virtual void OnUserModelLoaded()
    {
        GameObject playerPrefab = GameObject.Instantiate(Resources.Load("PREFAB_PLAYER") as GameObject) as GameObject;
		
        playerPrefab.GetComponent<CharacterSelectController>().SetModel(CharacterSelectController.ModelType.SINGLEPLAYER_MALE, this.OnPlayerModelLoaded);
		
        player = playerPrefab.transform;
        //player =  (Instantiate(GameManager.instance.LoadPrefab("playerGameObject"), getTransform("PlayerEntryPoint").position, getTransform("PlayerEntryPoint").rotation) as GameObject).transform;

        //User user = _gameState.User;
		
        //player.gameObject.GetComponent<SRCharacterController>()._user = user;

    }
	
    protected void OnPlayerModelLoaded(GameObject playerModelObject)
    {
        playerModelObject.transform.parent = player.transform;
        playerModelObject.transform.localPosition = Vector3.zero;
        ContinueLoadingLevelAfterHeroLoaded();
    }
	
    private void OnUserModelLoadedBeachCamp()
    {
        GameManager.instance.scaleformCamera.generalSwf.LoadLoadingScreen();
        GameObject playerPrefab;
		
        GameObject currentPlayerObj = GameObject.Find("PREFAB_PLAYER");
        if (currentPlayerObj != null)
            playerPrefab = currentPlayerObj;
        else
            playerPrefab = GameObject.Instantiate(Resources.Load("PREFAB_PLAYER") as GameObject) as GameObject;
		
        playerPrefab.GetComponent<CharacterSelectController>().SetModel(CharacterSelectController.ModelType.SINGLEPLAYER_MALE, this.ContinueStartGame);
		
        player = playerPrefab.transform;

    }
    void DecideNextLevel()
    {
        if (GameManager.instance.isMultiPlayerMode)
        {
//			Debug.Log("MULTIPLAYER SCENE ON!!!");
            if (GameManager.instance.currentMultiPlayerLevel != null && !GameManager.instance.currentMultiPlayerLevel.Equals(""))
            {
                currentLevel = GameManager.instance.currentMultiPlayerLevel;
            }
        } else
        {
//            Debug.Log("MULTIPLAYER SCENE OFF :( !!!");
			
            if (POINumber != -1)
            {
                if (poi [POINumber].poiNextLevel.Equals("CaveEntrance"))
                {
                    if (currentLevel.Equals("ForbiddenCave"))
                    {
                        isPlayCaveEntranceOtherAnim = true;
                    }
                }
                currentLevel = poi [POINumber].poiNextLevel;
            } else
            {
                currentLevel = "ToHollowTree";
            }
        }
//		Debug.Log("DecideNextLevel >> " + currentLevel);
    }

    /// <summary>
    /// Reset this instance.
    /// </summary>
    protected void Reset()
    {
        DecideNextLevel();
        multiPlayerLevelManagerClosed();
		
    }

    /// <summary>
    /// Multis the player level manager closed.
    /// </summary>
    protected void multiPlayerLevelManagerClosed()
    {
        GameManager._gameState.LevelState.POIEnemyKeyVal.Clear();
        GameManager._gameState.LevelState.POIEnemyKeyVal.TrimExcess();
        deletePOIs();
        PoiListForLevel.Clear();
        PoiListForLevel.TrimExcess();
        _level = null;
        _zone = null;
        _levelCameras.ResetCameras();
        isCutScenePlaying = false;
//		Debug.LogError("2 IsCutscenePlaying "+isCutScenePlaying);
		isHitRecieved = false;
        isPlayerNeutral = false;
        currentLevelShrine = null;
        DeleteDynamicBeachcampObjects();
        Destroy(levelLoaded);
        levelLoaded = null;
        lightmap [0].lightmapFar = null;
        LevelsAssetBundles.UnloadAssetBundle(currentLevel);
        POINumber = -1;	
    }

    /// <summary>
    /// Deletes the dynamic beachcamp objects.
    /// </summary>
    void DeleteDynamicBeachcampObjects()
    {
        GameObject _bcwPrefab = GameObject.Find("BeachCampWaterPrefab(Clone)");
        if (_bcwPrefab != null)
            Destroy(_bcwPrefab);
        
        GameObject _bcsPoint = GameObject.Find("BeachCamp_Shrine_Point(Clone)");
        if (_bcsPoint != null)
            Destroy(_bcsPoint);

        GameObject _am = GameObject.Find("AlchemyMeditation(Clone)");
        if (_am != null)
            Destroy(_am);

/*        if (GameObject.Find("BeachCampWaterPrefab(Clone)") != null)
            Destroy(GameObject.Find("BeachCampWaterPrefab(Clone)"));
		
        if (GameObject.Find("BeachCamp_Shrine_Point(Clone)") != null)
            Destroy(GameObject.Find("BeachCamp_Shrine_Point(Clone)"));
		
        if (GameObject.Find("AlchemyMeditation(Clone)") != null)
            Destroy(GameObject.Find("AlchemyMeditation(Clone)"));*/
    }

    /// <summary>
    /// Deletes the POIs.
    /// </summary>
    void deletePOIs()
    {
        for (int i=0; i<poi.Count; i++)
        {
            Destroy(poi [i].poiObject);
            Destroy(poi [i].relatedObject);
        }
		
        for (int j=0; j<LOCKED.Count; j++)
        {
            Destroy(LOCKED [j]);
        }
        poi.Clear();
        poi.TrimExcess();
		
    }
	
    Transform getTransform(string pointName)
    {
//        if (GameManager.PRINT_LOGS) Debug.Log("pointtNameee====" + pointName);
        return levelLoaded.transform.FindChild(pointName).transform;
    }
	
    void GetAnimationForObject(string poiId)
    {
        currentSceneAnimation = Resources.Load("SceneAnimations/" + poiId + "Anim") as AnimationClip;
    }
	
    public void StartMultiPlayerMode()
    {
        if (_levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].speed == -1)
        {
            _levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].speed = 1;
            _levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].time = 0;
        }
		
        _levelCameras.defaultCamera.transform.parent.gameObject.animation.Play();
		Debug.LogError ("~~~~~~~~~~~~~~~~~~~StartMultiPlayerMode~~~~~~~~~~~~~~~~~~~~~");
//		GameManager.instance.scaleformCamera.LoadGuildUI();
    }

    /// <summary>
    /// Multi the player animation completed.
    /// </summary>
    public void MultiPlayerAnimationCompleted()
    {
//		Debug.Log("MultiPlayerAnimationCompleted");
        if (GameManager._gameState.ringShards >= 1)
        {
            GameObject portalPrefab = GameObject.Find("portal");
            portalPrefab.transform.FindChild("portalParticles").gameObject.SetActive(true);
            //StartCoroutine(AnimateLight(portalPrefab.light));
            ExitToPortalNow();
        } else
        {
            if (_levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].speed != -1)
            {
                _levelCameras.sfCamera.generalSwf.generalSwf.showUiGeneralPopup("PORTAL BLOCKED", "Requires a shard of the Sorcerer's Ring", () => {
                    GoBackToMainMenu();
                    UIManager.instance.ShowMainScreenMenu(() => {
                        StartStoryMode();
                    }, () => {
                        StartMultiPlayerMode();
						if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.PotionTutorialStart)) {
							TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.PotionTutorialStart, true);	
						}
                    }); 
                });
                _levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].time = _levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].length;
            }
        }
    }
		
    /// <summary>
    /// Animates the light.
    /// </summary>
    /// <returns>The light.</returns>
    /// <param name="light">Light.</param>
    IEnumerator AnimateLight(Light light)
    {
        float temp = 0.0f;
        while (temp<=2.0f)
        {	
            temp += 0.01f;
            if (light != null)
                light.intensity = temp;
            yield return new WaitForSeconds(0.01f);
        }
//		if(GameManager.PRINT_LOGS) Debug.Log(temp+"<<<Animatingggg");
        ExitToPortalNow();
        yield return null;
    }
	
    public void GoBackToMainMenu()
    {
        GameManager.instance.isMultiPlayerMode = false;
        _levelCameras.defaultCamera.transform.parent.gameObject.animation ["BaseCamp_MultiplayerPortalViewop2"].speed = -1;
        _levelCameras.defaultCamera.transform.parent.gameObject.animation.Play();
        _levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);
        daySkybox.SetActive(true);
//		_levelCameras.sfCamera.mainMenu.EnableMultiplayer();
    }

    /// <summary>
    /// Plaies the entry animation.
    /// </summary>
    public void PlayEntryAnimation()
    {
        if (isPlayCaveEntranceOtherAnim)
        {
            GetAnimationForObject(currentLevel + "_OtherEntry");
            isPlayCaveEntranceOtherAnim = false;
        } else
        {
            GetAnimationForObject(currentLevel + "_Entry");
        }
        if (player.animation != null && player.animation.GetClip("EntryAnim") != null)
        {
            player.animation.RemoveClip("EntryAnim");
        }
        //if(GameManager.PRINT_LOGS) Debug.Log("PLAYING ENTRY ANIMATION===="+currentSceneAnimation.name);
        _levelCameras.defaultCamera.animation.enabled = true;
        _levelCameras.defaultCamera.animation.Stop();
        _levelCameras.defaultCamera.animation.Play();
        if (currentSceneAnimation != null)
        {
            player.animation.AddClip(currentSceneAnimation, "EntryAnim");
            player.animation.Play("EntryAnim");
        }
        isCutScenePlaying = true;
//		Debug.LogError("3 IsCutscenePlaying "+isCutScenePlaying);
		if(UIManager.instance.levelScene != null && UIManager.instance.levelScene.levelScene != null)
			UIManager.instance.levelScene.levelScene.SetCanSkip(true);
		if (UIManager.instance.levelScene != null)
        {
			if(currentLevel == "BeachCamp" || currentLevel == "ArcanumRuhalis" ) {
            	UIManager.instance.levelScene.HideMonsterIcon();
			} else {
				UIManager.instance.levelScene.ShowMonsterIcon();
			}
        }
    }
	
    public virtual void ReturnToCamp()
    {
        SoundManager.instance.StopBackgroundSound();
        SoundManager.instance.StopAmbientSound();
		
        battleManager.gameObject.SetActive(false);
		Debug.LogError("ReturnToCamp in LM - "+battleManager.gameObject.activeSelf);
        isEnemyClicked = false;
        if (PlayerPrefs.GetInt("isDayCountIncremented") == 1)
        {
            ShrineManager.Instance.RefreshShrines();
            GameManager._gameState.LevelState.lootList.Clear();
            GameManager._gameState.LevelState.lootList.TrimExcess();
        }
		
        _levelCameras.sfCamera.generalSwf.LoadLoadingScreen();
        if (!GameManager.instance.isResetGameToDefault)
        {
            GameManager._gameState.User.SetLifeToFull();
        }
		
        POINumber = -1;
		
        Reset();
        player.gameObject.SendMessage("SetIdle");
		
        currentLevel = firstLevel;
        if (GameManager.instance.isResetGameToDefault)
        {
            currentLevel = "ToHollowTree";
        }
	
        if (GameManager.PRINT_LOGS)
            Debug.Log("ReturnToCamp :::::: CALLING Init() !!!");
        StartCoroutine(Init());
		
        SoundManager.instance.playDefaultBackGround();
        _levelCameras.sfCamera.levelScene.SetDisplayVisible(false);
        Debug.Log("HideMonsterIcon");
        _levelCameras.sfCamera.levelScene.HideMonsterIcon();
        PlayerPrefs.SetInt("isDayCountIncremented", 0);
    }

    /// <summary>
    /// Restarts the battle.
    /// </summary>
    public void RestartBattle()
    {
        StartCoroutine(SetPlayerToNeutralPositon());
        poi [POINumber].isCleared = false;
        poi [POINumber].poiObject.SetActive(true);
        poi [POINumber].poiObject.collider.enabled = true;
        poi [POINumber].relatedObject.collider.enabled = false;
        isEnemyClicked = false;
		Debug.LogError("Restart battle poiObject true");
        //_levelCameras.sfCamera.DestroyBattleHUD();
        if (_levelCameras.sfCamera.hud != null)
        {
            UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
            _levelCameras.sfCamera.hud.SetDisplayVisible(false);
        } else
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("Battle UI IS NULLL !!");
        }
	
        SoundManager.instance.playDefaultBackGround();
        Debug.Log("Toggle top stats true!");
		
        UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
        _levelCameras.sfCamera.generalSwf.ToggleTopStats(true);
        _levelCameras.sfCamera.levelScene.SetDisplayVisible(true);
        //_levelCameras.sfCamera.OnLoadLevelStart();
        POINumber = -1;
        WaitForTimeAndFadeIn(1.0f);
		Debug.LogWarning("FadeIn RestartBattle");

		if (currentLevel.Equals("ArcanumRuhalis") ) {
			UIManager.instance.levelScene.HideMonsterIcon();
		} else {
			UIManager.instance.levelScene.ShowMonsterIcon();
		}
    }

    /// <summary>
    /// Actives the un protected POIs.
    /// </summary>
    public void ActiveUnProtectedPOI()
    {
        if (poi [POINumber].isCleared)
        {
            for (int i=0; i<PoiListForLevel.Count; i++)
            {
                if (poi [i].poiProtectedBy.Equals(poi [POINumber].poiId))
                {
                    poi [i].poiObject.SetActive(true);
					Debug.LogError(poi[i].poiId+" "+poi[i].poiObject.name+" "+poi[i].poiObject.transform.position);
                }
            }
        }
    }

    /// <summary>
    /// Snycs the state of the POI with game.
    /// </summary>
    private void SnycPOIWithGameState()
    {
        List<KeyValuePair<string,int>> poiKeyVal = GameManager._gameState.LevelState.poiIsClearList;
		
        for (int i=0; i<poi.Count; i++)
        {
            for (int j=0; j<poiKeyVal.Count; j++)
            {
                if (_level.poiNameList [i].Equals(poiKeyVal [j].Key))
                    poi [i].isCleared = Convert.ToBoolean(poiKeyVal [j].Value);
            }
        }
		
    }

    /// <summary>
    /// Actives the un protected POI at start.
    /// </summary>
    private void ActiveUnProtectedPOIAtStart()
    {
        for (int i=0; i<poi.Count; i++)
        {
            if (poi [i].isCleared)
            {	
                if (poi [i].poiType.Equals("Duel"))
                    isEnemyDefeated = true;
				
                for (int j=0; j<poi.Count; j++)
                {
                    if (poi [j].poiProtectedBy.Equals(poi [i].poiId))
                        poi [j].poiObject.SetActive(true);
					Debug.LogError("POI in activeUnprotectedPOIAtStart");
					
                    if (TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted) && poi [i].poiType.Equals("Exit"))
                        poi [j].poiObject.SetActive(false);
                }
                poi [i].poiObject.SetActive(false);
                poi [i].relatedObject.SetActive(false);
            }
        }
    }
	
    protected virtual void Update()
    {
        //if(GameManager.PRINT_LOGS) Debug.Log("deviceName "+SystemInfo.deviceName+"deviceType "+SystemInfo.deviceType+"deviceModel "+SystemInfo.deviceModel);
        if (battleManager.gameObject.activeInHierarchy)
        if (battleManager.scaleformBattleEnded && battleManager.IsTouchReleased)
        {	
            if (poi [POINumber].poiNextLevel.Equals("BeachCamp"))
            {
                ReturnToCamp();
                //battleManager.scaleformBattleEnded = false;
            } else
            {
                isEnemyDefeated = true;
                poi [POINumber].isCleared = true;
				Debug.LogWarning("Shader FadeIn");
                AnimateTransparentShader [] shaderFadeArray = poi [POINumber].relatedObject.GetComponentsInChildren<AnimateTransparentShader>();
				
                int count = shaderFadeArray.Length;
                AnimateTransparentShader aShader = null;
                for (int i = 0; i<count; i++)
                {
                    aShader = shaderFadeArray [i];
                    aShader.Invoke("AnimateShader", 0.1f);
                }
				
                Invoke("DestroyEnemy", 1.5f);
//				if(TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted))
//				{
                ActiveUnProtectedPOI();
                //}
                battleManager.gameObject.SetActive(false);
                //if((TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.BattleEndTutorialCompleted) && currentLevel.Equals("ToHollowTree")) || (TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted) && currentLevel.Equals("StatuePath")))
                POINumber = -1;
                isEnemyClicked = false;
                //_levelCameras.sfCamera.OnLoadLevelStart();
                _levelCameras.sfCamera.levelScene.SetDisplayVisible(true);
                StartCoroutine(SetPlayerToNeutralPositon());
				
                SoundManager.instance.playDefaultBackGround();
                GameManager._gameState.LevelState.UpdateLevelState(currentLevel, _level.zoneID, poi);
//				GameManager.instance.SaveGameState(true);
            }
            battleManager.scaleformBattleEnded = false;
            Debug.Log("HideMonsterIcon");
            _levelCameras.sfCamera.levelScene.HideMonsterIcon();
        }
		

		
        if (isPlayerNeutral)
            TransitionToPOI();
		
        if (!currentLevel.Equals("LavaScene") && GameManager.instance.isMultiPlayerMode && !InputWrapper.disableTouch) {
            PortalClicked();
			Debug.LogError("LM Is Mutiplayer and Portal Clicked "+currentLevel);
		}
    }
	
	
    public bool allowDebug = true;

    public void PortalClicked()
    {
        //Debug.Log("GameManager.instance.isMultiPlayerMode > " + GameManager.instance.isMultiPlayerMode);
		
        if (GameManager._gameState.ringShards >= 1)
        {
            if (ServerManager.IsInternetAvailable())
            {
                CheckForClick();
                ExitToPortal();
            } else
            {
				Debug.LogError("Connection Error popup being called");
				PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("CONNECTION ERROR", "CHECK YOUR INTERNET CONNECTIVITY");
            }
        }
    }

    /// <summary>
    /// Destroies the enemy.
    /// </summary>
    public void DestroyEnemy()
    {
        Destroy(enemy.gameObject);
    }

    /// <summary>
    /// Raises the next camera loaded event.
    /// </summary>
    /// <param name="loaded">If set to <c>true</c> loaded.</param>
    void OnNextCameraLoaded(bool loaded)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("CameraLoaded");
        if (loaded)
        {
            isHitRecieved = true;
            isPlayerNeutral = false;
            PlayerTransition();
			
        }
    }
    public delegate void PortalListener(string portalTag);
    public static PortalListener portalListener;
    public virtual void ExitToPortal()
    {
        Debug.Log("ExitToPortal");
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.LogError("LEVEL MANAGER hit.collider.tag =" + hit.collider.tag);
            if (hit.collider.tag.Equals("BeachCampPortal") && !isHitRecieved)
            {
                Debug.Log("BeachCampPortal CLICKED!");
                isHitRecieved = true;
                ExitToPortalNow();
				
            }
            hit = new RaycastHit();
            ray = new Ray();
        }
    }
	
    private const string KEY_MULTIPLAYER_ID = "multiplayerID";
    protected void ExitToPortalNow()
    {
//		Debug.Log("ExitToPortalNow");
        /*
		if(!PlayerPrefs.HasKey(KEY_MULTIPLAYER_ID))
		{
			_levelCameras.sfCamera.DestroyMainMenu();

			Resources.UnloadUnusedAssets();
			Reset();
			Resources.UnloadUnusedAssets();
			Application.LoadLevelAsync("CharacterSelectScene");
			return;
		}
		*/
		
        GameManager.instance.currentMultiPlayerLevel = "ArcanumRuhalis"; 
        RenderSettings.skybox = null;
        RenderSettings.skybox = Resources.Load("DSR") as Material;	
        SoundManager.instance.StopAmbientSound();

        _levelCameras.sfCamera.generalSwf.LoadLoadingScreen();
        if (_levelCameras.sfCamera.levelScene != null)
        {
            Debug.Log("HideMonsterIcon");
            _levelCameras.sfCamera.levelScene.HideMonsterIcon();
        }
        daySkybox.SetActive(false);
        GameObject levelManagerGameObject = this.gameObject;
        Resources.UnloadUnusedAssets();
        GameManager.instance.isMultiPlayerMode = true;
        Reset();
        Resources.UnloadUnusedAssets();
        MultiPlayerLevelManager mLevelManager = levelManagerGameObject.GetComponent<MultiPlayerLevelManager>();
        mLevelManager.player = player;
        mLevelManager.daySkybox = daySkybox;
        this.enabled = false;
        mLevelManager.enabled = true;
        //GameManager.instance.isMultiPlayerMode=true;
        GameManager.instance._levelManager = mLevelManager;
		
		
        //mLevelManager.enabled=false;
        //StartCoroutine(ExitToNextLevel(1.3f));
    }
	
    private void ListLevelObjects()
    {
		
        /*Transform[] transforms = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];
		
		for(int i = 0 ; i < transforms.Length; i ++)
			if(transforms[i].tag == "Untagged")
				Debug.Log("INDEX >> " + i + "   object name >> " + transforms[i].name);
				*/
    }

    /// <summary>
    /// Executes the POI functionality.
    /// </summary>
    public void ExecutePoiFunctionality()
    {
        if (isCutScenePlaying)
        {
			Debug.LogError("CUtscenePlaying -  ");

			if (currentLevel.Equals("BeachCamp"))
            {
                _levelCameras.sfCamera.generalSwf.ShowUnLockMainMenuPopup(GameManager._gameState.dayCount);
            } else
            {
//				Debug.LogError("Current Leve - "+currentLevel);
				if (currentLevel.Equals("ArcanumRuhalis")) {
					UIManager.instance.levelScene.HideMonsterIcon();
				} else
                {
					UIManager.instance.levelScene.ShowMonsterIcon();
                }
            }
			
            playerNeutralPoint.position = player.position;
            playerNeutralPoint.rotation = player.rotation;
            StartCoroutine(SetPlayerToNeutralPositon());
            isCutScenePlaying = false;
//			Debug.LogError("2 IsCutscenePlaying "+isCutScenePlaying);
//			_levelCameras.sfCamera.generalSwf.DisplayCenterButton(true);

//			//TODO Check Logic for FastForward
//			if(UIManager.instance.levelScene.levelScene != null)
//				UIManager.instance.levelScene.levelScene.SetCanSkip(false);
		} else
        {
			Debug.LogError("ExecutePOIFunctionality "+poi [POINumber].poiType);
            switch (poi [POINumber].poiType)
            {
                case "Exit":
				
                    TutorialManager.instance.removeFindShrine();
				
                    UIManager.instance.levelScene.SetDisplayVisible(false);
				
                    if (UIManager.instance.hud != null)
                    {
                        UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
                        UIManager.instance.hud.SetDisplayVisible(false);
                    }
				//_levelCameras.sfCamera.levelScene.SetDisplayVisible(false);
                    StartCoroutine(ExitToNextLevel(1.3f));
					Debug.LogError("ExecutePOIFunctionality - ExitToNextLevel");

				break;
                case "Loot":
				PlayMakerFSM.BroadcastEvent ("FadeInEvent");

				if (!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted))
                    {
                        TutorialManager.instance.ArcaneKeys3();
                    }
                    SoundManager.instance.PlayTreasureLootSound();
                    _levelCameras.sfCamera.LoadLootScreen(poi [POINumber].poiProtectedBy);
                    poi [POINumber].poiObject.SetActive(false);
                    StartCoroutine(SetPlayerToNeutralPositon());
                    isHitRecieved = false;
                    poi [POINumber].isCleared = true;
                    GameManager._gameState.LevelState.lootList.Add(poi [POINumber].poiId);
                    GameManager._gameState.LevelState.UpdateLevelState(currentLevel, _level.zoneID, poi);
                    GameManager.instance.SaveGameState(false);
			
                    break;
                case "Duel":
                    player.gameObject.SendMessage("SetIdle");
				
                    UIManager.instance.levelScene.SetDisplayVisible(false);
				
                    UnityEngine.Debug.LogError(">>> SetDisplayVisible > T");
					player.LookAt(poi [POINumber].relatedObject.transform.position);
//					UIManager.instance.hud.SetDisplayVisible(true);
//					UIManager.instance.hud.StartBattle();
//                    UIManager.instance.hud.ShowPlayerElementStats(GameManager._gameState.User._hasFire, GameManager._gameState.User._hasWater, GameManager._gameState.User._hasLightning, GameManager._gameState.User._hasEarth);
                    StartCoroutine(StartBattle());
				PlayMakerFSM.BroadcastEvent ("FadeInEvent");

				Debug.LogError("ExecutePOIFunctionality - startBattle");
                    break;
                case "Shrine":
				PlayMakerFSM.BroadcastEvent ("FadeInEvent");

				if (!ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated)
                    {
                        currentLevelShrine.animation ["Take 001"].speed = 0.8f;
                        currentLevelShrine.animation.Play();
                        SoundManager.instance.PlayShrineActivateSound();
                        Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
                        analParams.Add(Analytics.ParamName.ShrineElement, ShrineManager.Instance.element);
                        Analytics.logEvent(Analytics.EventName.Shrine_Activate, analParams);
                    }
                    StartCoroutine(WaitForAnimationToFinish(currentLevelShrine.animation));
                    break;
				
            }
        }
    }

    /// <summary>
    /// Starts the battle.
    /// </summary>
    /// <returns>The battle.</returns>
    IEnumerator StartBattle()
    {
        poi [POINumber].poiObject.SetActive(false);
        SoundManager.instance.playBattleBackGround();
		
        yield return new WaitForSeconds(1.5f);
		
        battleManager.setPlayer(player.gameObject);
        battleManager.setEnemy(poi [POINumber].relatedObject);
        battleManager.gameObject.SetActive(true);
        isHitRecieved = false;
	

    }

    /// <summary>
    /// Checks for click.
    /// </summary>
    protected void CheckForClick()
    {
        if (!InputWrapper.disableTouch && InputWrapper.touchCount > 0 && InputWrapper.GetTouch(0).tapCount >= 1 && Time.timeScale != 0)
        {
            //if(GameManager.PRINT_LOGS) Debug.Log("CheckingClick");
            if (InputWrapper.GetTouch(0).phase == TouchPhase.Ended)
            {
                isHitRecieved = false;
                ray = _levelCameras.defaultCamera.ScreenPointToRay(Input.mousePosition);
            }
			
        } else if (Time.timeScale != 0)
        {
            if (InputWrapper.GetMouseButtonDown(0))
            {
                touchPos1 = Input.mousePosition;
            }
			
            if (InputWrapper.GetMouseButtonUp(0))
            {
                touchPos2 = Input.mousePosition;
                float touchdiff = Vector3.Distance(touchPos1, touchPos2);
                if (touchdiff < 0.01f)
                {
                    isHitRecieved = false;
                    ray = _levelCameras.defaultCamera.ScreenPointToRay(Input.mousePosition);
                    touchPos1 = Vector3.zero;
                    touchPos2 = Vector3.zero;
                }
            }
        }
    }

    /// <summary>
    /// Sets the player to neutral positon.
    /// </summary>
    /// <returns>The player to neutral positon.</returns>
    IEnumerator SetPlayerToNeutralPositon()
    {
		_levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);

		player.gameObject.SendMessage("SetIdle");
		
		if (currentLevel == "BeachCamp" || currentLevel == "ArcanumRuhalis" )//farhan
            UIManager.instance.levelScene.HideMonsterIcon();
        else
        {
			UIManager.instance.levelScene.ShowMonsterIcon();
        }
		
        yield return new WaitForSeconds(2.0f);
		_levelCameras.sfCamera.generalSwf.DisplayCenterButton(true);
		if(UIManager.instance.levelScene != null && UIManager.instance.levelScene.levelScene != null)
			UIManager.instance.levelScene.levelScene.SetCanSkip(false);
		
		
		//_levelCameras.defaultCamera.gameObject.SetActive(true);
        if (_levelCameras.defaultCamera.gameObject.animation.enabled)
        {
            _levelCameras.defaultCamera.gameObject.animation.enabled = false;
            if (_levelCameras.defaultCamera.gameObject.GetComponent<RotateCamera>() != null)
                _levelCameras.defaultCamera.gameObject.GetComponent<RotateCamera>().enabled = true;
            if (_levelCameras.defaultCamera.transform.FindChild("POICamera") != null)
                _levelCameras.defaultCamera.transform.FindChild("POICamera").gameObject.SetActive(true);
			
            if (GameManager.instance.isMultiPlayerMode && _levelCameras.defaultCamera.GetComponent<SmoothLookAt>() != null)
            {
                _levelCameras.defaultCamera.GetComponent<SmoothLookAt>().enabled = true;
                player.transform.parent = _levelCameras.defaultCamera.transform;
            }
			
            TutorialManager.instance.ShowInLevelTutorial(currentLevel);
        } else
        {
            _levelCameras.ActiveCamera.gameObject.SetActive(false);
            _levelCameras.ActiveCamera.tag = null;
            _levelCameras.defaultCamera.gameObject.SetActive(true);
			
            _levelCameras.ActiveCamera = _levelCameras.defaultCamera;
            _levelCameras.ActiveCamera.tag = "MainCamera";
            //player.LookAt(playerNeutralPoint.position);
            player.position = playerNeutralPoint.position;
            player.rotation = playerNeutralPoint.rotation;
			
            if (POINumber != -1)
            {
				if (poi [POINumber].poiType.Equals("Shrine")) {
//					if (ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isCharged) {
//						poi [POINumber].poiObject.SetActive(false);
//						Debug.LogError("IsCharged");
//					}
//					if(ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").shrineLevel > 1) {
//						poi [POINumber].poiObject.SetActive(true);
//						Debug.LogError("shrineLevel - "+ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").shrineLevel);
//					}
				}
            }
        }
        if (currentLevel.Equals("ArcanumRuhalis"))
        {
            //daySkybox.SetActive(false);
            RenderSettings.skybox = null;
            RenderSettings.skybox = Resources.Load("DSR") as Material;	
            SoundManager.instance.StopAmbientSound();
			
            DeleteDynamicBeachcampObjects();
        }
        allowTransition = false;
        isPlayerNeutral = true;	
        Resources.UnloadUnusedAssets();
//		StartCoroutine(WaitForTimeAndFadeIn(1.0f));
        if (currentLevel.Equals("BeachCamp"))
            _levelCameras.sfCamera.levelScene.setDaysText(GameManager._gameState.dayCount);
        else {
            StartCoroutine(WaitForTimeAndFadeIn(1.0f));
			Debug.LogWarning("FadeIn SetPlayerToNeutralPosition - commented");

		}
    }

    /// <summary>
    /// Player transition.
    /// </summary>
    void PlayerTransition()
    {	
        if (currentLevel.Equals("DemonFalls"))
        {
            ExecutePoiFunctionality();
        } else
        {
            if (POINumber != -1)
            {
                GetAnimationForObject(poi [POINumber].poiId + "_MoveTo");
                if (player.animation.GetClip("currentAnimation") != null)
                {
                    player.animation.RemoveClip("currentAnimation");
                }
                player.animation.AddClip(currentSceneAnimation, "currentAnimation");
                player.animation.Play("currentAnimation");
				
				
            }
        }
        isPlayerNeutral = false;
		
        //GameManager.instance.scaleformCamera.levelScene.onMonsterIcon();
    }

    /// <summary>
    /// Exits to next level.
    /// </summary>
    /// <returns>The to next level.</returns>
    /// <param name="time">Time.</param>
    /// <param name="aiModel">Ai model.</param>
    public virtual IEnumerator ExitToNextLevel(float time, AIModel aiModel = null)
    {	
        PlayMakerFSM.BroadcastEvent("FadeOutEvent");
		Debug.LogWarning("FadeOut ExitToNextLevel");
		SoundManager.instance.StopBackgroundSound();
        SoundManager.instance.StopAmbientSound();
        isHitRecieved = false;
        yield return new WaitForSeconds(time);
        if (UIManager.instance.hud != null)
            UIManager.instance.hud.hud.ResetGrids();
        _levelCameras.sfCamera.generalSwf.LoadLoadingScreen();
        PlayMakerFSM.BroadcastEvent("FadeInEvent");
		Debug.LogWarning("FadeIn ExitToNextLevel");

		Resources.UnloadUnusedAssets();
        Reset();
        Resources.UnloadUnusedAssets();
        if (GameManager.PRINT_LOGS)
            Debug.Log("EXIT TO NEXT LEVEL :::::: CALLING Init() !!!");
        StartCoroutine(Init());
        if (GameManager.PRINT_LOGS)
            Debug.Log("Exit Found");
		if(UIManager.instance.levelScene != null && UIManager.instance.levelScene.levelScene != null)
			UIManager.instance.levelScene.levelScene.SetCanSkip(true);
	}

    /// <summary>
    /// Actives the not cleared POI.
    /// </summary>
    void ActiveNotClearedPOI()
    {
        for (int i=0; i<PoiListForLevel.Count; i++)
        {
            if (poi [i].isCleared)
            {
                poi [i].poiObject.SetActive(false);
            } else
            {
                poi [i].poiObject.SetActive(true);
				Debug.LogError(poi[i].poiId+" "+poi[i].poiObject.name+" "+poi[i].poiObject.transform.position);

			}
        }
    }
	
    protected virtual void LoadAI(string currentLevel, AILoader.MonsterModelDelegate del, Transform poiTransform, int index, string poiId = null)
    {
        if (!GameManager.instance.isGameLoadedForFirstTime)
        {
//			Debug.Log("LOAD AI DUDE!");
            _aiLoader.LoadAI(currentLevel, del, poiTransform, index, poiId);
        } else
        {
            _aiLoader.LoadAI(currentLevel, del, poiTransform, index);
        }
    }

    /// <summary>
    /// Generates the point of intrests.
    /// </summary>
    protected virtual void GeneratePointOfIntrests()
    {
        isEnemyDefeated = false;
        Transform PoiTransform;
        for (int i=0; i<PoiListForLevel.Count; i++)
        {
			
			
            POISet aSet = GameManager._dataBank.GetPoiDetials(PoiListForLevel [i]);
			
//            if (GameManager.PRINT_LOGS) Debug.Log("PoiListForLevel[i] -->>> " + PoiListForLevel [i].ToString() + "       ::::::: SET TOSTR --->>>> " + aSet.ToString());
			
			
            if (aSet.type.Equals("Duel"))
            {
//                if (GameManager.PRINT_LOGS) Debug.Log("DUEL TYPE POI");
				
                PoiTransform = getTransform(aSet.id + "_Point");
                GameObject temp = (GameObject)Instantiate(GameManager.instance.LoadPrefab("poiGameObject"), new Vector3(PoiTransform.position.x, PoiTransform.position.y + 1.84f, PoiTransform.position.z), Quaternion.identity);
                temp.tag = i.ToString();
				Debug.LogError("Generating POIs - "+temp.tag+" "+temp.gameObject.name);
                POIModel poiObject;
//                if (GameManager.PRINT_LOGS) Debug.Log("GEN POI - 1");
                if (currentLevel.Equals("DemonFalls"))
                {
//                    if (GameManager.PRINT_LOGS) Debug.Log("GEN POI - 2");
                    GameObject GoldenRing = GameObject.Find("goldenRing");
                    if (GameManager.PRINT_LOGS)
                    if (GoldenRing == null)
                        Debug.Log("GOLDEN RING NOT FOUND!");
                    temp.transform.position = GoldenRing.transform.position;
                    poiObject = new POIModel(i.ToString(), aSet.id.ToString(), aSet.type.ToString(), aSet.protectedBy.ToString(), aSet.nextLevel.ToString(), PoiTransform, getTransform(aSet.id + "_MoveTo"), GameObject.Find("DemonFalls_DuelA_CameraChild").camera, temp, aSet.isCleared);
//                    if (GameManager.PRINT_LOGS) Debug.Log("GEN POI - 3");
                } else
                {
                    poiObject = new POIModel(i.ToString(), aSet.id.ToString(), aSet.type.ToString(), aSet.protectedBy.ToString(), aSet.nextLevel.ToString(), PoiTransform, getTransform(aSet.id + "_MoveTo"), levelLoaded.transform.FindChild(aSet.id + "_Camera").camera, temp, aSet.isCleared);
                }
				
                poi.Add(poiObject);
//				Debug.LogError("Poi Added here and &&&&&&&&&&&&&&");
//				Debug.LogError(poi[i].poiId+" "+poi[i].poiObject.name+" "+poi[i].poiObject.transform.position);

				/*if(!GameManager.instance.isGameLoadedForFirstTime)
				{
					_aiLoader.LoadAI(currentLevel, this.OnAILoaded,PoiTransform,i,aSet.id);
				}
				else
				{
					_aiLoader.LoadAI(currentLevel, this.OnAILoaded,PoiTransform,i);
				}*/
                LoadAI(currentLevel, this.OnAILoaded, PoiTransform, i, aSet.id);
                isAILoading = true;
            } else if (aSet.type.Equals("Loot"))
            {
//                if (GameManager.PRINT_LOGS) Debug.Log("LOOT TYPE");
				
                PoiTransform = getTransform(aSet.id + "_Point");

                GameObject temp = (GameObject)Instantiate(GameManager.instance.LoadPrefab("poiGameObject"), new Vector3(PoiTransform.position.x, PoiTransform.position.y + 0.5f, PoiTransform.position.z), Quaternion.identity);
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                temp.tag = i.ToString();
				
                POIModel poiObject = new POIModel(i.ToString(), aSet.id.ToString(), aSet.type.ToString(), aSet.protectedBy.ToString(), aSet.nextLevel.ToString(), PoiTransform, getTransform(aSet.id + "_MoveTo"), levelLoaded.transform.FindChild(aSet.id + "_Camera").camera, temp, aSet.isCleared);
				
                poi.Add(poiObject);
                LoadTreasureObject(PoiTransform, i, aSet.protectedBy);
            } else if (aSet.type.Equals("Exit"))
            {
//                if (GameManager.PRINT_LOGS) Debug.Log("EXIT TYPE");
				
                PoiTransform = getTransform(aSet.id + "_Point");
				
                PoiTransform.tag = i.ToString();
				
                POIModel poiObject = new POIModel(i.ToString(), aSet.id.ToString(), aSet.type.ToString(), aSet.protectedBy.ToString(), aSet.nextLevel.ToString(), PoiTransform, getTransform(aSet.id + "_MoveTo"), levelLoaded.transform.FindChild(aSet.id + "_Camera").camera, PoiTransform.gameObject, aSet.isCleared);
				
                poi.Add(poiObject);
            } else if (aSet.type.Equals("Shrine"))
            {
                if (GameManager.PRINT_LOGS)
                    Debug.Log("SHRINE TYPE");
				
                PoiTransform = getTransform(aSet.id + "_Point");
				
                GameObject temp = (GameObject)Instantiate(GameManager.instance.LoadPrefab("poiGameObject"), new Vector3(PoiTransform.position.x, PoiTransform.position.y + 0.6f, PoiTransform.position.z), Quaternion.identity);
                temp.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                temp.tag = i.ToString();
				
                POIModel poiObject = new POIModel(i.ToString(), aSet.id.ToString(), aSet.type.ToString(), aSet.protectedBy.ToString(), aSet.nextLevel.ToString(), PoiTransform, getTransform(aSet.id + "_MoveTo"), levelLoaded.transform.FindChild(aSet.id + "_Camera").camera, temp, aSet.isCleared);
                poiObject.relatedObject = PoiTransform.gameObject;
                if (ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated)
                {
//                    if (GameManager.PRINT_LOGS) Debug.Log("animationLength= " + currentLevelShrine.animation ["Take 001"].length + "  animationTime= " + currentLevelShrine.animation ["Take 001"].time);
                    currentLevelShrine.animation ["Take 001"].time = currentLevelShrine.animation ["Take 001"].length;
                    currentLevelShrine.animation ["Take 001"].speed = 0.0f;
                    currentLevelShrine.animation.Play("Take 001");	
                }
				
//                if (ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isCharged)
//                    temp.SetActive(false);
//				if(ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").shrineLevel > 1) {
//					temp.SetActive(true);
//					Debug.LogError("shrineLevel - "+ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").shrineLevel);
//				}

				
                poi.Add(poiObject);
            }
			
            if (poi [i].poiNextLevel.Equals("LOCKED"))
            {
                poi [i].poiObject.tag = "LOCK";
            } else if (!poi [i].poiProtectedBy.Equals(""))
            {
                if (poi [i].poiType.Equals("Loot"))
                {
                    poi [i].poiObject.tag = "NEED KEYS";
                } else if (!poi [i].poiProtectedBy.Equals("BeachCamp"))
                {
                    poi [i].poiObject.SetActive(false);
                }
            }
			
        }
    }

    /// <summary>
    /// Checks the loot list.
    /// </summary>
    void CheckLootList()
    {
        List<string> tempLootList = GameManager._gameState.LevelState.lootList;	
        for (int i=0; i<tempLootList.Count; i++)
        {
            for (int j=0; j<poi.Count; j++)
            {
                if (poi [j].poiId.Equals(tempLootList [i]))
                {
                    poi [j].isCleared = true;
                    poi [j].poiObject.SetActive(false);
                    poi [j].relatedObject.animation.Play("open");
                }
            }
        }
    }
	
    public int enemyIndexTemp;

    /// <summary>
    /// Transit to a battle.
    /// </summary>
    /// <param name="enemyIndex">Enemy index.</param>
    public void TransitionToBattle(int enemyIndex)
    {
        enemyIndexTemp = enemyIndex;
        isHitRecieved = true;
        if (GameObject.FindGameObjectWithTag("Cinematic_Intro"))
        {
			
            //GameObject CinematicObj =  Instantiate(Resources.Load("Monster_Intros/_prefabs/Sequence_Intro_DemonFalls",typeof(GameObject))) as GameObject;
            GameObject CinematicObj = GameObject.FindGameObjectWithTag("Cinematic_Intro");
            CinematicObj.SendMessage("PlaySequence");
            poi [enemyIndex].poiObject.collider.enabled = false;
            poi [enemyIndex].poiObject.SetActive(false);
            poi [enemyIndex].relatedObject.SetActive(false);
            player.gameObject.SetActive(false);
            CinematicObj.SetActive(true);
            _levelCameras.ActiveCamera.gameObject.camera.enabled = false;
            return;
        } else
        {

            TutorialManager.instance.removeFindShrine();
			
            isEnemyClicked = true;
			
            if (UIManager.instance.generalSwf != null)
            {
                UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
                UIManager.instance.generalSwf.ToggleTopStats(false);
            }
			
            if (UIManager.instance.levelScene != null)
                UIManager.instance.levelScene.SetDisplayVisible(false);
			
            poi [enemyIndex].poiObject.SetActive(false);
            poi [enemyIndex].relatedObject.collider.enabled = true;
            POINumber = enemyIndex;
            if (!currentLevel.Equals("ToDemonFalls") && !GameManager.instance.isMultiPlayerMode)
            {
                _levelCameras.sfCamera.DestroyGeneralSWF();
                _levelCameras.sfCamera.levelScene.SetDisplayVisible(false);
                StartCoroutine(GameManager.instance.LoadGeneralSwf());
            }
            StartCoroutine(_levelCameras.LoadCameraAfterWaitForTime(1.0f, poi [POINumber].poiCameraObject));
        }
    }

    /// <summary>
    /// Transitions to battle after cinematic.
    /// </summary>
    public void TransitionToBattleAfterCinematic()
    {
        POINumber = enemyIndexTemp;
        StartCoroutine(_levelCameras.LoadCameraAfterWaitForTime(0.0f, poi [POINumber].poiCameraObject));
        poi [enemyIndexTemp].relatedObject.SetActive(true);
        player.transform.position = poi [enemyIndexTemp].poiMoveTo.position;
        player.gameObject.SetActive(true);
		
        TutorialManager.instance.removeFindShrine();
	
        GameObject CinematicObj = GameObject.FindGameObjectWithTag("Cinematic_Intro");
        Destroy(CinematicObj);
        isEnemyClicked = true;
		
        UnityEngine.Debug.Log("generalSwf.ToggleTopStats > F");
        UIManager.instance.generalSwf.ToggleTopStats(false);
        poi [enemyIndexTemp].poiObject.SetActive(false);
        poi [enemyIndexTemp].relatedObject.collider.enabled = true;
        _levelCameras.ActiveCamera.gameObject.camera.enabled = true;
		
    }

    /// <summary>
    /// Transitions to POI.
    /// </summary>
    protected virtual void TransitionToPOI()
    {	
//		Debug.LogError("TransitionToPOI "+poiState);
        if (poiState != AllowedPoiTypes.kNone)
        {
            CheckForClick();
            if (Physics.Raycast(ray, out hit, 100) && !isHitRecieved)
            {
                if (GameManager.PRINT_LOGS)
                    Debug.LogError("hit.collider.tag" + hit.collider.tag+" "+hit.collider.name);
                lastCollidedTag = hit.collider.tag;
                for (int i=0; i<PoiListForLevel.Count; i++)
                {
                    if (hit.collider.tag == i.ToString())
                    {
                        // hit.collider.enabled=false;
						
                        Debug.Log("poi[i].poiType >> " + poi [i].poiType + "   InputWrapper.disableTouch >> " + InputWrapper.disableTouch + "    poiState >> " + poiState.ToString());
						
                        if (InputWrapper.disableTouch && UIManager.instance.generalSwf._loadedMenuUI != null && UIManager.instance.generalSwf._loadedMenuUI.gameObject != null)
                            return;

                        switch (poi [i].poiType)
                        {
                            case "Duel":
							PlayMakerFSM.BroadcastEvent ("FadeInEvent");

							if(TutorialManager.instance.IsFindShrine()) {
								Debug.LogError("Find shrine label is on");	
							} else {
								Debug.LogError("POIState" + poiState);

								if ((poiState == AllowedPoiTypes.kDuel) || (poiState == AllowedPoiTypes.kAll))
                                {
                                    if (currentLevel == "CaveEntrance")
                                        TutorialManager.instance.removeFindShrine();

                                    Dictionary<string, object> comparisonDictionary = GameManager.instance.PlayerAndEnemyStatsComparision();
                                    string message = comparisonDictionary ["messageString"].ToString();

                                    if (message.Equals(""))
                                    {
                                        TransitionToBattle(i);
                                    } else
                                    {
                                        //Check what enemy have and player needs !!!
                                        allowTransition = false;
                                        //hit.collider.enabled=true;
									
                                        List<string> missingTagList = comparisonDictionary ["missingTags"] as List<string>;
                                        GameManager.instance.scaleformCamera.generalSwf.SetMissingRingTags(missingTagList);
									
                                        if (GameManager._gameState.User._inventory.HasRingInBagWithTags(missingTagList.ToArray()))
                                            GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3("Recommended for this fight", message, "Equip Now !", "Fight !");
                                        else
                                            GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3("Recommended for this fight", message, "Buy Now !", "Fight !");
                                    }
                                }
							}
                                break;
                            case "Loot":
							PlayMakerFSM.BroadcastEvent ("FadeInEvent");
							if ((poiState == AllowedPoiTypes.kLoot) || (poiState == AllowedPoiTypes.kAll))
                                {
                                    hit.collider.enabled = false;
                                    allowTransition = true;
                                    poi [i].poiObject.SetActive(false);
                                }
                                break;
                            case "Exit":
                                if ((poiState == AllowedPoiTypes.kExit) || (poiState == AllowedPoiTypes.kAll))
                                {
                                    hit.collider.enabled = false;
                                    allowTransition = true;
                                    poi [i].poiPoint.gameObject.SetActive(false);
									// TODO disable center pause button after clicking POI
									_levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);

                                }
                                break;
                            case "Shrine":
							PlayMakerFSM.BroadcastEvent ("FadeInEvent");
							if ((poiState == AllowedPoiTypes.kShrine) || (poiState == AllowedPoiTypes.kAll))
                                {
                                    hit.collider.enabled = true;
                                    if (!ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated && !currentLevel.Equals("ThreeGods"))
                                    {
                                        TutorialManager.instance.removeFindShrine();
									
                                        if (TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted))
                                        {
                                            Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
                                            analParams.Add(Analytics.ParamName.ShrineLevelName, GameManager.instance._levelManager.currentLevel);
                                            Analytics.logEvent(Analytics.EventName.Shrine_Tutorial_End, analParams);
								
                                            TutorialManager.instance.AddTutorialIdWithStatus(TutorialManager.TutorialsAndCallback.FireShrineTutorialCompleted, true);
                                            GameManager.instance.SaveGameState(false);
                                        }
                                    }
									bool isShrineTutorial = TutorialManager.instance.currentTutorial.ToString().Contains("shrine", true);
									Debug.LogError("Shrine activated for level - "+currentLevel+" "+ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated);

//								Debug.LogError("Tutorial shrine - "+TutorialManager.instance.currentTutorial+" status - "+isShrineTutorial+" isInternet - "+ServerManager.IsInternetAvailable() +" isTeam " +GameManager._gameState.User.team != null);
								Debug.LogError("Tutorial shrine - "+TutorialManager.instance.currentTutorial);
								Debug.LogError(" status - "+isShrineTutorial);
								Debug.LogError(" isInternet - "+ServerManager.IsInternetAvailable());
								Debug.LogError(" isTeam - " +(GameManager._gameState.User.team != null));

								if (isShrineTutorial || (ServerManager.IsInternetAvailable() && GameManager._gameState.User.team != null))
//								if (isShrineTutorial || (ServerManager.IsInternetAvailable()))
								{
                                	if (ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated) {
										ShrineManager.Instance.isShrineInfoRecieved = false;
										ServerManager.Instance.GetShrineInformation(ShrineManager.Instance.GetShrineInformationHandler);
									}
									
                                    allowTransition = true;
                                } else
                                {
									if (!ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated) {
										allowTransition = true;
									} else if(!ServerManager.IsInternetAvailable()){
										Debug.LogError("Connection Error popup being called");

										UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("CONNECTION ERROR", "CHECK YOUR INTERNET CONNECTIVITY", () => {});
            	                        allowTransition = false;
									} else if(GameManager._gameState.User.team == null) {
										Debug.LogError("not partof any guild Error popup being called");

										UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("SHRINE ERROR", "YOU ARE NOT A PART OF ANY GUILD. JOIN OR CREATE ONE", () => {});
										allowTransition = false;
									}
                                }
                        	}
                                break;
                        }
						
                        if (allowTransition)
                        {
                            AllowTransiton(hit.collider.tag);
                        }
                    } else if (hit.collider.tag.Equals("LOCK"))
                    {
                        switch (poi [i].poiProtectedBy)
                        {
                            case "SKULL_LOCK":
                                UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("PATH BLOCKED", "YOU ARE NOT POWERFUL ENOUGH", () => {});
                                break;
							
                            case "FIRE_WALK":
                                UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("DISGRACE!", "CAN'T WALK ON FIRE SORCERER?", () => {});
                                break;
							
                            case "STRENGTH":
                                UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("TOO WEAK!!!", "GROW UP AND COME BACK SOME DAY", () => {});
                                break;
							
                            case "FLY":
                                UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("WINGS?", "SEEMS LIKE YOU AIN'T GOT THEM!", () => {});
                                break;
                        }
                    } else if (hit.collider.tag.Equals("NEED KEYS") && poi [i].poiType.Equals("Loot"))
                    {
                        if ((poiState == AllowedPoiTypes.kLoot) || (poiState == AllowedPoiTypes.kAll))
                        {
                            if (GameManager.PRINT_LOGS)
                                Debug.Log("poi[i].poiProtectedBy==========" + poi [i].poiProtectedBy);
                            string[] keysArray = poi [i].poiProtectedBy.Split(' ');
                            if (LootManager.CheckForKeys(Convert.ToInt32(keysArray [0])))
                            {
                                if (!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted))
                                {
                                    TutorialManager.instance.RemoveFindTreasure();
                                }
                                hit.collider.enabled = false;
                                POINumber = Convert.ToInt32(poi [i].poiIndex);
                                StartCoroutine(_levelCameras.LoadCameraAfterWaitForTime(1.0f, poi [POINumber].poiCameraObject));
                            } else
                            {
                                GameManager.instance.scaleformCamera.generalSwf.ShowNeedKeysPopUp(GameManager._gameState.User._inventory.keyRing.keyCount, GameManager._dataBank.inApps.GetGCost(InApp.InAppTypes.KEYS), Convert.ToInt32(keysArray [0]));//ShowGeneralPopup("KEYS","YOU NEDD 1 KEY TO UNLOCK THIS CHEST");
                            }
                        }
                    }
                }
                hit = new RaycastHit();
                ray = new Ray();
            }
        }
    }
	
    /// <summary>
    /// Allows the transiton.
    /// </summary>
    /// <param name="collidername">Collidername.</param>
    protected void AllowTransiton(string collidername)
    {
        if (currentLevel.Equals("BeachCamp"))
            _levelCameras.sfCamera.levelScene.hideDaysText();
		

//        if (GameManager.instance.isMultiPlayerMode)
//            return;

        POINumber = Convert.ToInt32(collidername);
        lastCollidedTag = collidername;
        StartCoroutine(_levelCameras.LoadCameraAfterWaitForTime(1.0f, poi [POINumber].poiCameraObject));
    }

    /// <summary>
    /// Waits for animation to finish.
    /// </summary>
    /// <returns>The for animation to finish.</returns>
    /// <param name="Anim">Animation.</param>
    IEnumerator WaitForAnimationToFinish(Animation Anim)
    {	
        TutorialManager.instance.removeFindShrine();

        if (!ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated)
        {
            while (Anim.isPlaying)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (ShrineManager.Instance.GetShrineForLevel(currentLevel + "_Shrine").isActivated && !ShrineManager.Instance.shrineConnectionProblem)
        {
			GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
            while (!ShrineManager.Instance.isShrineInfoRecieved)
            {
                yield return new WaitForSeconds(0.1f);
            }
            GameManager.instance.scaleformCamera.generalSwf.HideUILoadingScreen(true);
        }
        isHitRecieved = false;


		// TODO
//        StartCoroutine(SetPlayerToNeutralPositon());
        ShrineManager.Instance.ShowShrinePopUp();
    }


	public void ResetPlayerToNeutralPosition()
	{
		StartCoroutine(SetPlayerToNeutralPositon());
	}
	
	/// <summary>
    /// Waits for time and play entry animation.
    /// </summary>
    /// <returns>The for time and play entry animation.</returns>
    /// <param name="time">Time.</param>
    IEnumerator WaitForTimeAndPlayEntryAnimation(float time)
    {	
        yield return new WaitForSeconds(time);
        PlayEntryAnimation();
        PlayMakerFSM.BroadcastEvent("FadeInEvent");
		Debug.LogWarning("FadeIn WaitForTime");
    }

    /// <summary>
    /// Waits for time and fade in.
    /// </summary>
    /// <returns>The for time and fade in.</returns>
    /// <param name="time">Time.</param>
    IEnumerator WaitForTimeAndFadeIn(float time)
    {	
        yield return new WaitForSeconds(time);
        PlayMakerFSM.BroadcastEvent("FadeInEvent");
		Debug.LogWarning("FadeIn WaitFor");
    }

    /// <summary>
    /// Waits for time.
    /// </summary>
    /// <returns>The for time.</returns>
    /// <param name="time">Time.</param>
    IEnumerator WaitForTime(float time)
    {	
        yield return new WaitForSeconds(time);
        Application.Quit();
    }
	
    /*
	public List<GuildSystem.ChatHistoryObject> chatMessages;
	static int tempInt = 100;
	void OnGUI()
	{
		if(GUI.Button(new Rect(120,140,180,120), "SEND MESSAGE?")) 
		{
			 GuildSystem.GuildsManager.Instance.SendGuildMessage("Hello from techies + " + ++tempInt,(success, errorMessage) => {
			if(success)
				if(GameManager.PRINT_LOGS) Debug.Log("MESSAGE SENT SUCCESFULLY TO GUILD :D");
			else
				if(GameManager.PRINT_LOGS) Debug.Log("MESSAGE failed  :( ------ with error = " + errorMessage);
			});
		}
		
		if(chatMessages != null)
		{
			string chatText = "";
			foreach(GuildSystem.ChatHistoryObject hist in chatMessages)
			{
				chatText+= (hist.message + "\n");
				if(GameManager.PRINT_LOGS) Debug.Log("HIST:::" + MiniJSON.Json.Serialize(hist.ToDictionary()));
			}
			 GUI.Label(new Rect(240, 180, 400, 800), chatText);
		}
	}*/

//	void OnGUI()
//	{
//		if(currentLevel.Equals("ArcanumRuhalis"))
//		{
//			if(GUI.Button(new Rect((Screen.width*0.04f), (Screen.height*0.05f), ((Screen.width+Screen.height)*0.1f), ((Screen.width+Screen.height)*0.03f)),"BACK !!"))
//			{
//				
//				
//			}
//		}
//	}
}