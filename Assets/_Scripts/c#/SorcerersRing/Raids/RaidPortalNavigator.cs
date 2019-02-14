using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaidPortalNavigator : MonoBehaviour, UI_Raid.LeaderBoardListener, UI_Raid.CharacterSelectionHandler
{

    private const string TAG = "RaidPortalNavigator - ";
    public Transform[] sortedPortalList;
    private List<Transform> populatedPortalList;
    public GameObject lookAtGameObject;
    private List<RaidAIModel> monsters;
    private RaidSwipeDetector _raidSwipeDetector;
    private int currentPortalIndex;
    private SmoothLookAt lookAtComp;
    Dictionary<int, RaidAIModel> indexToModelMap;
    public GameObject MarkerPOI;
    private List<GameObject> markersList;
    public GameObject pvpCamera;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public GameObject neutralCamera;
    private GameObject playerGameObject;
    private NGRaidsUI raidUI = null;
    public Transform malePoint;
    public Transform femalePoint;
    Color ambientColor;
    Color ambientBlackColor = Color.black;
    private bool hasMultiplayerProfile = false;
    Texture2D initialTex;
        

    void Start()
    {
        playerGameObject = GameObject.Find("PREFAB_HERO(Clone)");
        raidUI = UIManager.instance.CreateRaidUI(this);
        raidUI._portalNavigator = this;
        OnRaidUILoaded();
    }
    
    private GameObject persistentPlayerObject;
    private GameObject multiMale;
    private GameObject multiFemale;
    GameObject characterParentObject;

    private void OnRaidUILoaded()
    {
        currentPosInLeaderboard = 0;
        
        Debug.Log("GameManager._gameState.User.username >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + GameManager._gameState.User.username);
        
        if (string.IsNullOrEmpty(GameManager._gameState.User.username))
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("========================================================================== raidUI" + raidUI);
            
            //raidUI.CreateCharacter(UI_Raid.Gender.male);
            
            raidUI.ShowAvatarWindow(this);
            
            hasMultiplayerProfile = false;
            //GameManager.instance._levelManager.player.gameObject.SetActive(false);
            GameManager.instance._monoHelpers.MainLightOff();
            persistentPlayerObject = GameObject.Find("PREFAB_PLAYER(Clone)");
            persistentPlayerObject.SetActive(false);
            
            multiMale = GameObject.Instantiate(Resources.Load("Prefabs/LevelPrefabs/PREFAB_MULTI_MALE")) as GameObject;
            multiFemale = GameObject.Instantiate(Resources.Load("Prefabs/LevelPrefabs/PREFAB_MULTI_FEMALE")) as GameObject;
            
            multiFemale.transform.position = femalePoint.position;
            multiFemale.transform.rotation = femalePoint.rotation;
            multiFemale.transform.localScale = new Vector3(1, 1, 1);
            //multiFemale.transform.localScale = femalePoint.localScale;
            
            multiMale.transform.position = malePoint.position;
            multiMale.transform.rotation = malePoint.rotation;
            multiMale.transform.localScale = new Vector3(1, 1, 1);
            //multiMale.transform.localScale = malePoint.localScale;
            
            HighlightCharacter(multiMale, true);
            HighlightCharacter(multiFemale, false);
            
            malePoint.gameObject.SetActive(true);
            femalePoint.gameObject.SetActive(false);

            modelName = CharacterSelectController.ModelType.MULTIPLAYER_MALE.ToString();
        } else
        {
//          Debug.Log("TIME TO REQUEST RAID BOSSES!!!");
            Transform persistentPlayerTransform = null;
            persistentPlayerTransform = neutralCamera.transform.FindChild("PREFAB_PLAYER(Clone)");
            
            if (persistentPlayerTransform == null)
                persistentPlayerObject = GameObject.Find("PREFAB_PLAYER(Clone)");
            else
                persistentPlayerObject = persistentPlayerTransform.gameObject;
            
            
            persistentPlayerObject.SetActive(true);
            persistentPlayerObject.GetComponent<CharacterSelectController>().SetModel(GameManager._gameState.User.modelName, (gObject) => {
                gObject.transform.parent = persistentPlayerObject.transform;
                gObject.transform.localPosition = Vector3.zero;
                gObject.transform.localRotation = Quaternion.identity;
            });
            
            
            persistentPlayerObject.GetComponent<SRCharacterController>()._user = GameManager._gameState.User;
            
            hasMultiplayerProfile = true;
            
            malePoint.gameObject.SetActive(false);
            femalePoint.gameObject.SetActive(false);
            
            //persistentPlayerObject.SendMessage("SetIdle");
            
			GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);

            RaidsManager.Instance.GetAllRaidBosses(this.OnRaidBossesLoaded);
        }
    }
    
	IEnumerator UpdateRaidBosses() {
		while(true) {
			GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
			RaidsManager.Instance.GetAllRaidBosses(this.OnRaidBossesLoaded);
			yield return new WaitForSeconds(30.0f);
		}
	}

    
#region CharacterSelectionHandler CBs
    
    private string modelName;

    public void OnMaleCharacterSelected()
    {
        Debug.Log(" :::::::::::::: OnMaleCharacterSelected CB :::::::::::::: ");
        malePoint.gameObject.SetActive(true);
        femalePoint.gameObject.SetActive(false);
        HighlightCharacter(multiMale, true);
        HighlightCharacter(multiFemale, false);
        
        modelName = CharacterSelectController.ModelType.MULTIPLAYER_MALE.ToString();
        /*characterParentObject.GetComponent<CharacterSelectController>().SetModel(CharacterSelectController.ModelType.MULTIPLAYER_MALE, OnPlayerModelLoaded);*/
    }
        
    public void OnFemaleCharacterSelected()
    {
        Debug.Log(" :::::::::::::: OnFemaleCharacterSelected CB :::::::::::::: ");
        HighlightCharacter(multiMale, false);
        HighlightCharacter(multiFemale, true);
        
        femalePoint.gameObject.SetActive(true);
        malePoint.gameObject.SetActive(false);
        modelName = CharacterSelectController.ModelType.MULTIPLAYER_FEMALE.ToString();
        //characterParentObject.GetComponent<CharacterSelectController>().SetModel(CharacterSelectController.ModelType.MULTIPLAYER_FEMALE, OnPlayerModelLoaded);
    }
    
    private void OnPlayerModelLoaded(GameObject playerModelObject)
    {
        playerModelObject.transform.parent = characterParentObject.transform;

        playerModelObject.transform.localPosition = Vector3.zero;
        playerModelObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
    private string newUserName = null;
    private string inputUserName = null;
    private bool allowContinuePress = true;
    
    public void OnContinueClick(string nameText, bool isMale)
    {
		Debug.LogError(string.Format("OnContinueClick > name = {0} and isMale = {1}", nameText, isMale) +" allowcontinuePress"+allowContinuePress+" - disable "+InputWrapper.disableTouch);
		if (!allowContinuePress || InputWrapper.disableTouch)
            return;
        
        nameText = nameText.Trim();
            
        if (string.IsNullOrEmpty(nameText))
        {
            raidUI.ShowInvalidUsernameError("Username can't be empty!");
            return;
        }
        
        allowContinuePress = false;
        /* if(GameManager.PRINT_LOGS) */
        Debug.Log(" :::::::::::::: OnContinueClick CB :::::::::::::: nameText = " + nameText + " ::::::     isMale = " + isMale);
        inputUserName = nameText;
        //nameText = "testUSERNamEyar";
        ServerManager.Instance.UpdateUserNameForUser(nameText, this.DispatchResult);
    }
    
    public void OnCollectButton(string portalID)
    {
        if (InputWrapper.disableTouch)
            return;
        
        foreach (RaidAIModel aiModel in monsters)
        {
            if (aiModel.portalID.Equals(portalID))
            {
                RaidsManager.Instance.LootRaidBoss(aiModel.name, aiModel.skullLevel, (requestSucceeded) => {
                    if (!requestSucceeded)
                        return;
                    
//                  int gemsToAward = System.Int32.Parse(aiModel.rawDictionary ["reward"].ToString());
                    int gemsToAward = aiModel.rewardGem;
                    
                    GameManager.instance.scaleformCamera.generalSwf.LoadLoot(new InventorySystem.ItemGem(gemsToAward));
                    GameManager._gameState.User._inventory.gems += gemsToAward;
                    GameManager.instance.scaleformCamera.generalSwf.Init();
                    
                    ClearRaidMonstersData();
                        
//					GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
					OnRaidUILoaded();
                });
            }
        }
    }
    
    private void DispatchResult(object responseParameters, object error, ServerRequest request)
    {
        allowContinuePress = true;
        Debug.LogError("responseParameters = DispatchResult FOR USER :::: " + MiniJSON.Json.Serialize(responseParameters));
        if (error == null)
        {
            IDictionary response = responseParameters as IDictionary;
            if (GameManager.PRINT_LOGS)
                Debug.Log("response = " + MiniJSON.Json.Serialize(response));
            bool responseSuccess = bool.Parse(response ["success"].ToString());
            
            if (responseSuccess)
            {
                string username = response ["userName"].ToString();
                SaveUsername(username);
				Debug.LogError("new username = "+username);
//				raidUI.HideAvatarWindow();
			} else
            {
                inputUserName = null;
                
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Username already taken", "Try with a different name", () => {});
				Debug.LogError("response error");
				if (response.Contains("errorCode"))
                    raidUI.ShowInvalidUsernameError("Username already exists!");
                else
                    raidUI.ShowInvalidUsernameError("Unable to process request!");
            }
        } else
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("RaidPortalNavigator :::::::::: DispatchResult ::::: ERROR : " + error.ToString());
            inputUserName = null;
            raidUI.ShowInvalidUsernameError("Error : " + error.ToString());
        }
        
        //RaidsManager.Instance.GetAllRaidBosses(this.OnRaidBossesLoaded);
    }
    
    private void SaveUsername(string username)
    {
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("username" + username + "modelName" + modelName);
        //PlayerPrefs.SetString(User.KEY_MULTIPLAYER_ID, username);
        GameManager.instance.fragNetworkingNew.UpdateLocalUsername(username);

        GameManager._gameState.User.username = username;
        GameManager._gameState.User.modelName = modelName;

        GameManager.instance.SaveGameState(true);
        
        malePoint.gameObject.SetActive(false);
        femalePoint.gameObject.SetActive(false);

        GameObject.Destroy(multiMale);
        GameObject.Destroy(multiFemale);
        
        raidUI.HideAvatarWindow();
        
        OnRaidUILoaded();
    }
    
    private void HighlightCharacter(GameObject gameObject, bool yesNo)
    {
        foreach (SkinnedMeshRenderer renderer in gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>())
            renderer.useLightProbes = yesNo;
    }
    
#endregion
    
    public void OnRaidBossesLoaded()
    {
		GameManager.instance.scaleformCamera.generalSwf.HideUILoadingScreen(true);

        MakeRaidInitializations();
        
        pveBattleManager = GameManager.instance._levelManager.battleManager;
        
        monsters = RaidsManager.Instance.monsters;
        if (monsters == null || monsters.Count == 0)
        {
            GameManager.instance.scaleformCamera.generalSwf.generalSwf.showUiGeneralPopup("Network failure!", "Failed to load monsters' data, please try again later", () => {});
            return;
        }
        
//      monsters.ForEach(mon => Debug.Log("MON >> " + mon.ToString()));
        
        PostLoadingInitializations();
    }
    
    private void MakeRaidInitializations()
    {
        indexToModelMap = new Dictionary<int, RaidAIModel>();
        _raidSwipeDetector = gameObject.GetComponent<RaidSwipeDetector>();
        lookAtComp = lookAtGameObject.GetComponent<SmoothLookAt>();
        markersList = new List<GameObject>();
    }
    
    private void PostLoadingInitializations()
    {
        //raidUI._leaderBoardListener = this; //farhan
        allowPortalClick = true;
        
        MakeEnemyModelsAndDisplay();
        
        _raidSwipeDetector._swipeDirectionListener = this.OnSwipe;
        _raidSwipeDetector.enabled = true;
        
//		currentPortalIndex = 3;
		currentPortalIndex = PlayerPrefs.GetInt("PortalIndex", 3);
		lookAtComp.target = sortedPortalList [currentPortalIndex];
//		lookAtComp.enabled = true;
        Invoke("ShowMonsterUI", 5.0f);
		Debug.LogError("PostLoadingInitializations");
    }
    
    private void MakeEnemyModelsAndDisplay()
    {
        ClearRaidMonstersData();
        for (int i = 0; i < monsters.Count; i++)
        {
            RaidAIModel aModel = monsters [i];

            for (int j = 0; j<sortedPortalList.Length; j++)
            {
//              if(GameManager.PRINT_LOGS) Debug.Log("sortedPortalList[j].gameObject.name = " + sortedPortalList[j].gameObject.name + " ----------- aModel.portalID >>> " + aModel.portalID);
                GameObject aPoi = null;
                if (sortedPortalList [j].gameObject.name.Equals(aModel.portalID))
                {
                    indexToModelMap [j] = aModel;
					string tag = j.ToString();
					GameObject marker = GameObject.FindGameObjectWithTag(tag);
					if(marker == null) {
	                    aPoi = GameObject.Instantiate(MarkerPOI, sortedPortalList [j].transform.position, Quaternion.identity) as GameObject;
	                    aPoi.tag = tag;
	                    //aPoi.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
	                    aPoi.transform.parent = sortedPortalList [j];
	                    aPoi.AddComponent<SmoothLookAt>().target = lookAtGameObject.transform;
	                    markersList.Add(aPoi);
					}                
//                                      Debug.LogError ("MakeEnemyModelsAndDisplay-Monsters: " + monsters.Count + "   PortalList: " + sortedPortalList.Length + "   i:" + i + "  j:" + j + "\nObject: " + aPoi.name);
                    //if(GameManager.PRINT_LOGS) Debug.Log("indexToModelMap[j] - j = " + j);
                }
            }
        }
        LevelManager.portalListener = this.OnPortalTap;
        
        InvokeRepeating("DecrementTimers", 1f, 1f); //Start after 1 second, and run every 1 second
    }
    
    public void MoveToNeutralPosition()
    {
        this.gameObject.GetComponent<PVPBattleManager>()._gestureObject.SetActive(false);
        this.gameObject.GetComponent<PVPBattleManager>().enabled = false;
        
        GameManager.instance._levelManager.battleManager = pveBattleManager;
        
        if (GameManager.instance._levelManager.battleManager == null)
        if (GameManager.PRINT_LOGS)
            Debug.Log("GameManager.instance._levelManager.battleManager");
        playerGameObject = GameObject.Find("PREFAB_HERO(Clone)");
        playerGameObject.transform.position = playerPosition;
        playerGameObject.transform.rotation = playerRotation;
        
        playerGameObject.transform.parent = neutralCamera.transform;
        playerGameObject.SetActive(false);
        
        neutralCamera.SetActive(true);
        pvpCamera.SetActive(false);

        allowSwipe = true;
        
        //GameManager.instance.singlePlayerLevelManager.player = GameObject.Find("PREFAB_HERO(Clone)").transform;
    }
    
    private bool allowSwipe = true;
    BattleManager pveBattleManager;
    
    private void OnSwipe(RaidSwipeDetector.SwipeDirection direction)
    {
        if (InputWrapper.disableTouch)
            return;
        
        if (allowSwipe)
        {
//                      Debug.LogError ("OnSwipe-currentPortalIndex-Before: " + currentPortalIndex);
            switch (direction)
            {
                case RaidSwipeDetector.SwipeDirection.Left:
                    if (currentPortalIndex == 0) {
						currentPortalIndex = monsterCount - 1;

					//						currentPortalIndex = sortedPortalList.Length - 1;
					} else
						currentPortalIndex--;
				break;
                case RaidSwipeDetector.SwipeDirection.Right:
//				if (currentPortalIndex == sortedPortalList.Length - 1)
				if (currentPortalIndex == monsterCount - 1)
						currentPortalIndex = 0;
                    else
                        currentPortalIndex++;
				break;
            }
//            if (GameManager.PRINT_LOGS) Debug.Log(TAG + "currentPortalIndex " + currentPortalIndex);
//                      Debug.LogError ("OnSwipe-currentPortalIndex-After: " + currentPortalIndex);
			PlayerPrefs.SetInt("PortalIndex", currentPortalIndex);
			lookAtComp.target = sortedPortalList [currentPortalIndex];
            raidUI.ShowRaidInfo(true);
            Invoke("ShowMonsterUI", 0.4f);
        }
    }
    
    private void ShowMonsterUI()
    {
        if (monsters != null)
        {
            IDictionary dictionaryToShow = new Dictionary<string, object>();
            dictionaryToShow.Add("ratingsInfo", RaidsManager.Instance.topRatings);
            
            if (indexToModelMap.ContainsKey(currentPortalIndex))
                dictionaryToShow.Add("currentMonsterInfo", indexToModelMap [currentPortalIndex].ToDictionary());
            
//            if (GameManager.PRINT_LOGS) Debug.Log("MiniJSON.Json.Serialize(dictionaryToShow) = " + MiniJSON.Json.Serialize(dictionaryToShow));
            raidUI.ShowMonsterUI(dictionaryToShow);
			if(lookAtComp != null)
				lookAtComp.enabled = true;

        }
    }
    
    public void OnPortalTap(string portalTag)
    {
        if (InputWrapper.disableTouch)
            return;
        if (!allowPortalClick)
            return;
        
//      int portalNumber;
        int portalNumber = int.MinValue;
        int.TryParse(portalTag, out portalNumber);
        if (portalNumber == int.MinValue)
            return;
        
        /*if(portalTag.Equals("4"))
        {
            PVPServerManager.Instance.QueueForPVP(this.OnOpponentFound, this.OnQueueResponse);
            GameManager.instance.scaleformCamera.raidUI.ShowRaidInfo(false);
            return; 
        }*/

        Debug.LogError("OnPortalTap: " + portalNumber + "   Index: " + indexToModelMap.ContainsKey(portalNumber) + "   Count: " + indexToModelMap.Count);
        persistentPlayerObject.transform.parent = null;
        //persistentPlayerObject = null;
        RaidAIModel aiModel = indexToModelMap [portalNumber];
        currentPosInLeaderboard = aiModel.GetPositionOfUserInLeaderboard(float.Parse(GameManager.instance.fragNetworkingNew.GetUserID()));
        if (!aiModel.isActive)
        {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Boss unavailable!", "Try again later", () => {});
            return;
        }
        if (aiModel.life <= 0)
        {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Boss defeated already!", "Try again later", () => {});
            return;
        }
		Debug.LogError("aiModel.arenaName = "+aiModel.arenaName);
        GameManager.instance.currentMultiPlayerLevel = aiModel.arenaName;
		        
        if (GameManager.PRINT_LOGS)
            Debug.Log("aiModel" + aiModel.ToString());
        StartCoroutine(GameManager.instance._levelManager.ExitToNextLevel(1.3f, aiModel));
        
        GameManager.instance.scaleformCamera.DestroyRaidUI();
        
        ClearRaidMonstersData();
		Invoke("RemoveClickListener", 1.0f);
	}

	private void RemoveClickListener() {
		LevelManager.portalListener = null;
	}

	public void ClearRaidMonstersData()
    {       
        if (markersList == null || indexToModelMap == null)
            return;
        if (GameManager.PRINT_LOGS)
            Debug.Log("markersList size = " + markersList.Count + " indexToModelMap size = " + indexToModelMap.Count);
        for (int i = 0; i<markersList.Count; i++)
        {
            Destroy(markersList [i]);
        }
        markersList.Clear();
        markersList.TrimExcess();

        indexToModelMap.Clear();
    }
    
    private void DecrementTimers()
    {
        foreach (KeyValuePair<int, RaidAIModel> aPair in indexToModelMap)
        {
            aPair.Value.DecrementTimers(1);
        }
    }
    
    void Update()
    {
        /*if(GameManager.instance._levelManager.isPlayerNeutral)
            lookAtComp.enabled = true;*/
    }
    
    private void OnQueueResponse(bool yesNo)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log(TAG + "OnQueueResponse = " + yesNo);
        this.OnOpponentFound(null, null);
    }
    
    private void OnOpponentFound(string name, GameStateModule.GameState opponentState)
    {
        PlayMakerFSM.BroadcastEvent("FadeOutEvent");
		Debug.LogWarning("FadeOutEvent - OnOpponentFound");
			if (GameManager.PRINT_LOGS)
			Debug.Log(TAG + "OnOpponentFound = FadeOutEvent");

        // TODO: Comment this and uncomment the line afterward when server calls are ready
        StartCoroutine(InitiateBattleWithAIUser(GameManager._gameState.User));
        //StartCoroutine(InitiateBattleWithAIUser(opponentState.User));
    }
    
    public GameObject PREFAB_HERO;

    private IEnumerator InitiateBattleWithAIUser(User user)
    {
        /*GameObject player = GameObject.Find("PREFAB_HERO(Clone)");
        yield return new WaitForSeconds(1.0f);
        GameObject userObject = GameObject.Instantiate(player) as GameObject; //To be loaded using AssetBundleLoader
        userObject.GetComponent<SRCharacterController>()._user = user;
        userObject.transform.localScale = new Vector3(1f,1f,1f);
        this.gameObject.GetComponent<PVPBattleManager>().OnAiLoaded(player, userObject);*/
        yield return null;
    }
    
    private bool allowPortalClick = false;



    public void OnLeaderBoardClicked()
    {
        if (InputWrapper.disableTouch)
            return;
        
        allowPortalClick = false;

//        if (GameManager.PRINT_LOGS)
//            Debug.Log("OnLeaderBoardClicked - raid portal");
    }
    
    public void OnLeaderBoardClosed()
    {
        InputWrapper.disableTouch = false;
        
        allowPortalClick = true;
//        if (GameManager.PRINT_LOGS)
//            Debug.Log("OnLeaderBoardClosed - raid portal");
    }
    
    public void DestroyCharacterObjects()
    {
        if (multiMale != null)
            Destroy(multiMale);
        if (multiFemale != null)
            DestroyObject(multiFemale);
    }
    
	public void SetMonsterCount(int count) {
		monsterCount = count;
	}
	private int monsterCount = 0;
    public static int currentPosInLeaderboard = 0;
}

