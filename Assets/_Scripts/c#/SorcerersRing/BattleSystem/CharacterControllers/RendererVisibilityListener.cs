using UnityEngine;
using System.Collections;

public class RendererVisibilityListener : MonoBehaviour {
	
	void Start()
	{
		
	}
	
	void OnBecameVisible()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("OnBecameVisible");
		
		if(GameManager.instance._levelManager.battleManager.enabled)
			GameManager.instance._levelManager.battleManager.OnEnemyVisibleInCamera();
	}
	
	void OnBecameInvisible()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("OnBecameInvisible");
		
		if(GameManager.instance._levelManager.battleManager.enabled)
			GameManager.instance._levelManager.battleManager.OnEnemyInvisibleInCamera();
	}
}
