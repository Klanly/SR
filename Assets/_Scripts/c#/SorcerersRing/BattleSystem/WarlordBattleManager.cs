using UnityEngine;
using System.Collections;

public class WarlordBattleManager : BattleManagerInterface
{  
	private bool battleEnded = true;
	BattleManager parent;




	public WarlordBattleManager(BattleManager parent)
	{
		this.parent = parent;
		battleEnded = false;
	}
	
	
	public void OnInitializationComplete()
	{
		GameManager.instance.scaleformCamera.hud.ShowBossFightText(parent._aiController._aiModel.name);
		parent.OnInitializationComplete();
	}
	
	public void BattleSetStart()
	{
		parent.BattleSetStart();
	}
	
	public void OnPauseBeforeFight()
	{
		parent.OnPauseBeforeFight();
	}
	public void OnEnemyRendererVisible()
	{
	
	}

	public void OnEnemyRendererInvisible()
	{
	}
	
	public void OnFocusStateWhileEnemyStun()
	{
	}
	
	public void OnCollisionAfterStunEnd()
	{
	}
	
	public void InitiateRound(bool startImmediately)
	{
		parent.InitiateRound(startImmediately);
	}
	
	public void PerformAIFocusStateReached()
	{
		parent.PerformAIFocusStateReached();
	}
	
	public TouchDetector.SwipeDirection GetSwipeDirection()
	{
		return TouchDetector.SwipeDirection.kLeftRight;
	}
	
	public void OnBattleEnded(bool restartFlag,bool isBossFight=false)
	{
		battleEnded = true;
		GameManager._gameState.dayCount++;
		//GameManager.instance.isDayCountIncremented=true;
		if(PlayerPrefs.GetInt("isDayCountIncremented")==0)
		{
			PlayerPrefs.SetInt("isDayCountIncremented",1);
		}
		if(!restartFlag)
		GameManager._gameState.BossDefeated(this.parent._aiController._aiModel.name);
		parent.OnBattleEnded(restartFlag,isBossFight);
	}
	
	public void SetEnemyReady()
	{
		parent.SetEnemyReady();
		//parent.Invoke("SetEnemyReady", 2.0f);
	}
	
}
