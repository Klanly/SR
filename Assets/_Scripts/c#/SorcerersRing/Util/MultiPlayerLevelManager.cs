using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiPlayerLevelManager : LevelManager
{
	
	AIModel forcedAIModel;
	private bool isFightAvailable;
	
	void MultiPlayerLevelManagerStart ()
	{
		//Destroy(this.gameObject.GetComponent<LevelManager>());
		base.LevelManagerAwake ();
		base.LevelManagerStart ();
		//	GameManager.instance.scaleformCamera.levelScene.HideMonsterIcon();
		
		//currentLevel="ToHollowTree";
	}
	
	private int lifeBeforeMultiplayer;

	void OnEnable ()
	{
		MultiPlayerLevelManagerStart ();
		
		
		lifeBeforeMultiplayer = GameManager._gameState.User.life;
		GameManager.instance.multiplayerHealListener = OnPlayerHOTTick;
		OnPlayerHOTTick ();
	}
	
	private void OnPlayerHOTTick () //On player "heal over time" ticked...
	{
		GameManager._gameState.User.life = GameManager._gameState.User.multiplayerLife;
		GameManager.instance.scaleformCamera.generalSwf.Init ();
	}
	
	void OnDisable ()
	{
		Resources.UnloadUnusedAssets ();
		multiPlayerLevelManagerClosed ();
		Resources.UnloadUnusedAssets ();
		RenderSettings.skybox = Resources.Load ("NightSky") as Material;
		GameManager.instance.scaleformCamera.DestroyRaidUI ();
		GameManager.instance.isGameLoadedForFirstTime = false;
		//player.gameObject.collider.enabled=true;
		
//		if(GameManager.PRINT_LOGS) Debug.Log("GameManager._gameState.User.multiplayerLife = GameManager._gameState.User.life;");
		GameManager._gameState.User.multiplayerLife = GameManager._gameState.User.life;
		GameManager.instance.multiplayerHealListener = null;
		GameManager._gameState.User.life = lifeBeforeMultiplayer;
		GameManager.instance.SaveGameState (true);
	}
	
	protected override void Update ()
	{
		if (battleManager.gameObject.activeInHierarchy)
		if (battleManager.scaleformBattleEnded && battleManager.IsTouchReleased) {
			ReturnToCamp ();
			Debug.LogWarning ("FadeIn Shader");
			AnimateTransparentShader [] shaderFadeArray = enemy.GetComponentsInChildren<AnimateTransparentShader> ();
				
			int count = shaderFadeArray.Length;
			AnimateTransparentShader aShader = null;
			for (int i = 0; i<count; i++) {
				aShader = shaderFadeArray [i];
				aShader.Invoke ("AnimateShader", 0.1f);
			}
			POINumber = -1;
			isEnemyClicked = false;
			battleManager.scaleformBattleEnded = false;
			battleManager.gameObject.SetActive (false);
		}
		
		if (isPlayerNeutral)
			TransitionToPOI ();
		
		if (GameManager.instance.isMultiPlayerMode)
			PortalClicked ();
	}
	
	
//	protected override IEnumerator Init ()
//	{
//		return base.Init ();
//	}
	
	public override IEnumerator ExitToNextLevel (float time, AIModel aiModel = null)
	{	
		if (aiModel != null) {
			forcedAIModel = aiModel;
			string aiName = forcedAIModel.name;
			forcedAIModel.name = forcedAIModel.modelName;
			forcedAIModel.modelName = aiName;
		}
		
		PlayMakerFSM.BroadcastEvent ("FadeOutEvent");
		Debug.LogWarning ("FadeOut - MultiplayerLevelManager");
		player.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		SoundManager.instance.StopBackgroundSound ();
		SoundManager.instance.StopAmbientSound ();
		isHitRecieved = false;
		yield return new WaitForSeconds (time);
		_levelCameras.sfCamera.generalSwf.LoadLoadingScreen ();
		PlayMakerFSM.BroadcastEvent ("FadeInEvent");
		Debug.LogWarning ("FadeIn - MultiplayerLevelManager");
		Resources.UnloadUnusedAssets ();
		base.Reset ();
		Resources.UnloadUnusedAssets ();
		StartCoroutine (base.Init ());
		if (GameManager.PRINT_LOGS)
			Debug.Log ("Exit Found");
	}
	
	protected override void GeneratePointOfIntrests ()
	{
		if (currentLevel.Equals ("ArcanumRuhalis")) {
			isFightAvailable = false;
			//.gameObject.SetActive(false);
		} else {
			base.GeneratePointOfIntrests ();
//			isFightAvailable=true;
		}
		
	}
	
	protected override void OnUserModelLoaded ()
	{
		GameObject playerPrefab = GameObject.Instantiate (Resources.Load ("PREFAB_PLAYER") as GameObject) as GameObject;
		CharacterSelectController charController = playerPrefab.GetComponent<CharacterSelectController> ();
		charController.SetModel (charController.currentModel, this.OnPlayerModelLoaded);
		
		player = playerPrefab.transform;
		//player =  (Instantiate(GameManager.instance.LoadPrefab("playerGameObject"), getTransform("PlayerEntryPoint").position, getTransform("PlayerEntryPoint").rotation) as GameObject).transform;

		//User user = _gameState.User;
		
		//player.gameObject.GetComponent<SRCharacterController>()._user = user;

	}
	
	protected override void LoadAI (string currentLevel, AILoader.MonsterModelDelegate del, Transform poiTransform, int index, string poiId = null)
	{
		if (forcedAIModel != null)
			Debug.Log ("LoadAIWithModel > " + forcedAIModel.ToString ());
		else
			Debug.Log ("forcedAIModel NULL!");
		
		_aiLoader.LoadAIWithModel (currentLevel, del, poiTransform, index, forcedAIModel);


	}
	
	protected override void TransitionToPOI ()
	{
		if (!currentLevel.Equals ("LavaScene")) {
			return;
		}
		bool allowTransition = false;
		player.gameObject.collider.enabled = true;
		Debug.LogError ("_levelCameras.defaultCamera - " + _levelCameras.defaultCamera.name);
		_levelCameras.defaultCamera.GetComponent<RotateCamera> ().enabled = false;
		CheckForClick ();
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider.gameObject.name.Contains ("POI", true)) {
				isHitRecieved = true;
			}

			if (isHitRecieved && !isFightAvailable) {
				Dictionary<string, object> comparisonDictionary = GameManager.instance.PlayerAndEnemyStatsComparision (forcedAIModel);
				string message = comparisonDictionary ["messageString"].ToString ();
				if (message.Equals ("")) {
					TransitionToBattle (0);

					isFightAvailable = false;
					allowTransition = true;
				} else {
					lastCollidedTag = "0";
					List<string> missingTagList = comparisonDictionary ["missingTags"] as List<string>;
					GameManager.instance.scaleformCamera.generalSwf.SetMissingRingTags (missingTagList);
				
					if (GameManager._gameState.User._inventory.HasRingInBagWithTags (missingTagList.ToArray ()))
						GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3 ("Recommended for this fight", message, "Equip Now !", "Fight !");
					else
						GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3 ("Recommended for this fight", message, "Buy Now !", "Fight !");

				
//				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3("Recommended for this fight",message,"Buy Now !","Fight !");
				}
				if (allowTransition) {
					AllowTransiton (lastCollidedTag);
					isFightAvailable = true;
					_levelCameras.defaultCamera.GetComponent<RotateCamera> ().enabled = true;
				}

				
			}
			hit = new RaycastHit ();
			ray = new Ray ();

		}
	}
	
	public override void ReturnToCamp ()
	{
		SoundManager.instance.StopBackgroundSound ();
		SoundManager.instance.StopAmbientSound ();
		battleManager.gameObject.SetActive (false);
		isEnemyClicked = false;
		_levelCameras.sfCamera.generalSwf.LoadLoadingScreen ();
		//base.ReturnToCamp ();
		GameManager.instance.currentMultiPlayerLevel = "ArcanumRuhalis";
		
		POINumber = -1;
		Resources.UnloadUnusedAssets ();
		Reset ();
		Resources.UnloadUnusedAssets ();
		player.gameObject.SendMessage ("SetIdle");
		
		StartCoroutine (Init ());
		
		SoundManager.instance.playDefaultBackGround ();
		_levelCameras.sfCamera.levelScene.HideMonsterIcon ();
		player.gameObject.collider.enabled = false;
		
	}
	
	public override void ExitToPortal ()
	{
//		Debug.Log ("MULTIPLAYER LEVEL MANAGER > ExitToPortal");
		if (InputWrapper.disableTouch)
			return;
		//this.portalListener = _portalListener;
		if (Physics.Raycast (ray, out hit, 100)) {		
			Debug.Log ("hit.collider.tag = >>>> " + hit.collider.tag);

			int tagNumber;
			bool isNumeric = int.TryParse (hit.collider.tag, out tagNumber);

			if (isNumeric && portalListener != null && !string.IsNullOrEmpty (hit.collider.tag)) {
				portalListener (hit.collider.tag);
				Debug.Log ("PORTAL >> hit.collider.tag " + hit.collider.tag);
			}
			hit = new RaycastHit ();
			ray = new Ray ();
		}
	}
	
//	public void PortalClicked()
//	{
//		if(GameManager._gameState.ringShards>=1)
//		{
//			if(ServerManager.IsInternetAvailable())
//			{
//				CheckForClick();
//				ExitToPortal();
//			}
//			else
//			{
//				GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("CONNECTION ERROR", "CHECK YOUR INTERNET CONNECTIVITY");
//			}
//		}
//	}
//	
//	public void ExitToPortal()
//	{
//		if(Physics.Raycast(ray, out hit,100))
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("hit.collider.tag ="+ hit.collider.tag);
//			if(hit.collider.tag.Equals("BeachCampPortal") && !isHitRecieved)
//			{
//				isHitRecieved=true;
//				ExitToPortalNow();
//				
//			}
//			hit=new RaycastHit();
//			ray=new Ray();
//		}
//	}
//	
//	void ExitToPortalNow()
//	{
//		GameManager.instance.currentMultiPlayerLevel="ArcanumRuhalis";
//		_levelCameras.sfCamera.DestroyMainMenu();
//		daySkybox.SetActive(false);
//		StartCoroutine(ExitToNextLevel(1.3f));
//	}
//	
//	IEnumerator AnimateLight(Light light)
//	{
//		float temp=0.0f;
//		while(temp<=2.0f)
//		{	
//			temp+=0.01f;
//			light.intensity=temp;
//			yield return new WaitForSeconds(0.01f);
//		}
//		if(GameManager.PRINT_LOGS) Debug.Log(temp+"<<<Animatingggg");
//		ExitToPortalNow();
//		yield return null;
//	}
//	
//	public override void MultiPlayerAnimationCompleted()
//	{
//		if(GameManager._gameState.ringShards>=1)
//		{
//			GameObject portalPrefab = GameObject.Find("portal");
//			portalPrefab.transform.FindChild("portalParticles").gameObject.SetActive(true);
//			StartCoroutine(AnimateLight(portalPrefab.light));
//		}
//		else
//		{
//			if(_levelCameras.defaultCamera.transform.parent.gameObject.animation["BaseCamp_MultiplayerPortalViewop2"].speed!=-1)
//			{
//				_levelCameras.sfCamera.generalSwf.ShowGeneralPopup("PORTAL BLOCKED","Requires a shard of the Sorcerer's Ring");
//				_levelCameras.defaultCamera.transform.parent.gameObject.animation["BaseCamp_MultiplayerPortalViewop2"].time=_levelCameras.defaultCamera.transform.parent.gameObject.animation["BaseCamp_MultiplayerPortalViewop2"].length;
//			}
//		}
//	}
//	
//	public override void GoBackToMainMenu()
//	{
//		_levelCameras.defaultCamera.transform.parent.gameObject.animation["BaseCamp_MultiplayerPortalViewop2"].speed=-1;
//		_levelCameras.defaultCamera.transform.parent.gameObject.animation.Play();
//		_levelCameras.sfCamera.generalSwf.DisplayCenterButton(false);
//		daySkybox.SetActive(true);
//		_levelCameras.sfCamera.mainMenu.EnableMultiplayer();
//	}
}
