using UnityEngine;
using System.Collections;

public interface BattleManagerInterface 
{
	void OnInitializationComplete();
	void OnFocusStateWhileEnemyStun();
	void OnCollisionAfterStunEnd();
	void PerformAIFocusStateReached();
	void OnEnemyRendererVisible();
	void OnEnemyRendererInvisible();
	void OnBattleEnded(bool restartFlag,bool isBossFight=false);
	void BattleSetStart();
	void InitiateRound(bool intiateImmediately = true);
	void SetEnemyReady();
	void OnPauseBeforeFight();
	TouchDetector.SwipeDirection GetSwipeDirection();
}
