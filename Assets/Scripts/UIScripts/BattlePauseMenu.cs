using UnityEngine;
using System.Collections;

public class BattlePauseMenu : MonoBehaviour
{
	public dfButton _resumeButton;

	void Start()
	{
		UIManager.instance.generalSwf.pauseGame();
		gameObject.GetComponent<dfPanel>().PerformLayout();
	}

	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source.name.Equals( _resumeButton.name))
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		UIManager.instance.generalSwf.resumeGame();
	}

}