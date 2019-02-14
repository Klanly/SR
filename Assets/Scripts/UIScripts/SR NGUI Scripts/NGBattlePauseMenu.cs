using UnityEngine;
using System.Collections;

public class NGBattlePauseMenu : MonoBehaviour
{
	void Start()
	{
		UIManager.instance.generalSwf.pauseGame();
	}
	
	public void OnClick()
	{
		Destroy(gameObject);
	}
	
	void OnDestroy()
	{
		UIManager.instance.generalSwf.resumeGame();
	}
	
}