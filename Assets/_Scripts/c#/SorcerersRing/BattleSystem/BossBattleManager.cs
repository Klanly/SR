using UnityEngine;
using System.Collections;

public class BossBattleManager : BattleManagerInterface
{  
	BattleManager parent;
	SmoothLookAt lookAtComponent = null;
	GameObject nexBattleControllerGameObject = null;
	BattleControllerWithVectors battleController = null;
	private bool battleEnded = true;
	
	public BossBattleManager(BattleManager parent)
	{
		this.parent = parent;
		battleEnded = false;
	}
	
	
	public void OnInitializationComplete()
	{
		nexBattleControllerGameObject = GameObject.Find("BattleController");
		battleController = nexBattleControllerGameObject.GetComponent<BattleControllerWithVectors>();
		battleController.enemyObject = parent._aiController.gameObject;
		battleController.playerObject = parent.playerGameObject;
		nexBattleControllerGameObject.SetActive(true);
		
		lookAtComponent = battleController.cameraObj.GetComponent<SmoothLookAt>();
		lookAtComponent.target = battleController.enemyObject.transform;
		battleController.allowTilt = false;
		//parent.InitiateBattle(4f);
		GameManager.instance.scaleformCamera.hud.ShowBossFightText(parent._aiController._aiModel.name);
		parent.OnInitializationComplete();
		
	}
	
	public void OnPauseBeforeFight()
	{
		parent.StartCoroutine(HoldFight());
	}
	
	
	IEnumerator HoldFight()
	{
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.NexFightStart))
		{
			GameManager.instance.scaleformCamera.loadNewTutorial();
		}
		yield return new WaitForSeconds(3.0f);
		if(!TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.NexFightStart))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("Displaying nex fight start tutorials");
			//GameManager.instance.scaleformCamera.NewTutorialSwf.NexFightTutorialStart();
			TutorialManager.instance.NexFightTutorialStart();
			//GameManager.instance.scaleformCamera..NexFightTutorialStart();
			
		}
		battleController.MoveEnemyToStartPos();
		parent.InitiateBattle(3.0f);
	}
	
	public void BattleSetStart()
	{
		//TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexFightStart);
//		battleController.allowTilt = true;
		parent.BattleSetStart();
	}
	
	public void OnEnemyRendererVisible()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("OnEnemyRendererVisible");
		GameManager.instance.scaleformCamera.hud.HideBossFightText();
		
		if(lookAtComponent != null)
		{
			lookAtComponent.enabled = true;
			parent.DisableLookAt(lookAtComponent, 1.0f);
		}
		
		//TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexCameraEnd);
		
		battleController.allowTilt = false;
		GameManager.instance.scaleformCamera.hud.TurnOffFindNex();
		
		if(parent.IsInvoking("PerformAIFocusStateReached"))
		{
			parent.CancelInvoke("PerformAIFocusStateReached");

			parent.Invoke("PerformAIFocusStateReached", 0.2f);
		}
		
//		if(parent.IsInvoking("InitiateBattle"))
//		{
//			parent.CancelInvoke("InitiateBattle");
//			InitiateRound(true);
//		}
	}

	public void OnEnemyRendererInvisible()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("OnEnemyRendererInvisible in BBM ------------------------ >>>>>>>>>>>>       Invisible");
		if(lookAtComponent != null)
			lookAtComponent.enabled = false;
		
		battleController.allowTilt = true;
		if(!battleEnded)
		{
			GameManager.instance.scaleformCamera.hud.FindNex();
			GameManager.instance.scaleformCamera.hud.HideBossFightText();
		}
	}
	
	public void OnFocusStateWhileEnemyStun()
	{
		//parent.InitiateBattle(2.0f);
	}
	
	public void OnCollisionAfterStunEnd()
	{
		//parent.InitiateBattle(5.0f);
	}
	
	public void InitiateRound(bool startImmediately)
	{
		GameManager.instance.scaleformCamera.hud.SetPlayerFill(GameManager.instance._levelManager.battleManager._battleState._user.life, GameManager.instance._levelManager.battleManager._battleState._user.totalLife);

		if(GameManager.PRINT_LOGS) Debug.Log("INITIATE IN BOSS BM!!! ---------->>>>>>>>>>>>>>>>>>>>>>> startImmediately      " + startImmediately);
		if(startImmediately)
			parent.InitiateBattle(0.8f);
		else
			parent.InitiateBattle(2.0f);
	}
	
	public void PerformAIFocusStateReached()
	{
		battleController.Move();
		if(!battleEnded)
			GameManager.instance.scaleformCamera.hud.FindNex();
		
		parent.Invoke("PerformAIFocusStateReached", 3.0f);
		//parent.PerformAIFocusStateReached();
	}
	
	public TouchDetector.SwipeDirection GetSwipeDirection()
	{
		return TouchDetector.SwipeDirection.kUp;
	}
	
	public void OnBattleEnded(bool restartFlag,bool isBossFight=false)
	{
		if(restartFlag)
		{
			if(GameManager._gameState.skullLevel < parent._aiController._aiModel.skullLevel - 1) //Gamestate skull level should be "nex's level - 1" at MAX!
			{
				GameManager._gameState.skullLevel++;
			}
		}
		else
		{
			if(GameManager._gameState.ringShards<5) {
				GameManager._gameState.ringShards++;
				isBossFight=true;
			} else {
				// TODO Add sorcerer ring to inventory
			}
			GameManager._gameState.skullLevel+=3;
			if(GameManager._gameState.dayCount < 3)
				GameManager._gameState.dayCount = 3;
		}
		
		battleEnded = true;
		
		if(!restartFlag)
			GameManager._gameState.BossDefeated(this.parent._aiController._aiModel.name);
		
		GameManager.instance.scaleformCamera.hud.TurnOffFindNex();
		//GameManager.instance.SaveGameState(false);
		parent.OnBattleEnded(restartFlag,isBossFight);
	}
	
	public void SetEnemyReady()
	{
		parent.Invoke("SetEnemyReady", 2.0f);
	}
	
}
