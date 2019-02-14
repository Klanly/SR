using UnityEngine;
using System.Collections;

public class DestroyTutorial : MonoBehaviour {

	public void OnClick(UISprite mouseEvent)
	{
		Debug.Log("In ON Click " +mouseEvent.name);
		SoundManager.instance.PlayMenuOpenSound();
		if (mouseEvent.name == "BackBtn")
			Destroy(gameObject);
	}
}
