using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NexCutsceneHandler : MonoBehaviour
{
	public GameObject mainCamera;
	
	public GameObject char1Controller;
	private Sorcerer_CinematicControlScript char1ControlScript;
	public Transform char1LookAtNode;

	public GameObject char2Controller;
	private Nex_CinematicControlScript char2ControlScript;
	public Transform char2LookAtNode;

	public Cutscene Cutscene;
	private int currDialogIndex;
	private string currActor;
	private string currScript;
	private bool ended = false;
	protected bool previousFog;

	public void StartCutscene()
	{
		previousFog = RenderSettings.fog;
		RenderSettings.fog = false;

		char1ControlScript = char1Controller.GetComponent<Sorcerer_CinematicControlScript>();
		char2ControlScript = char2Controller.GetComponent<Nex_CinematicControlScript>();
	
		GameManager.instance.scaleformCamera.generalSwf.ShowCutsceneDialog(0, 0);
		GameManager.instance.scaleformCamera.generalSwf.OnNextClicked = () => {
			if (ended)
			return;
			
			nextDialog();
		};

		// set first dialog ready
		char1ControlScript.startYoga();
		currDialogIndex = 0;
		setDialog();
	}

	public void nextDialog() {
		currDialogIndex++;
		if (currDialogIndex < Cutscene.Dialogues.Count ) {
			setDialog();
		} else {
			EndCutscene();
		}
	}
	public void setDialog() {
		currActor = Cutscene.Dialogues[currDialogIndex].Actor;
		currScript = Cutscene.Dialogues[currDialogIndex].Script;

		setUIState();
		setActorStates();
		setCameraLookAt();	
	}
	public void setActorStates() {
		if (currActor.Trim().ToLower() == "ruh") {
			char1ControlScript.startYogaTalking();
			char2ControlScript.startListening();
		} else {
			char1ControlScript.startYogaListening();
			char2ControlScript.startTalking();
		}
	}
	public void setUIState() {
		GameManager.instance.scaleformCamera.generalSwf.ChangeCutsceneDialog(currActor, currScript, 0, 0);
	}
	
	public void setCameraLookAt() {
		if (currActor.Trim().ToLower() == "ruh") {
			iTween.LookTo(mainCamera,char1LookAtNode.transform.position,1.0f);
		} else {
			iTween.LookTo(mainCamera,char2LookAtNode.transform.position,1.0f);
		}
	}

	public void EndCutscene()
	{
		ended = true;
		PlayMakerFSM.BroadcastEvent("FadeOutEvent");
		Invoke("finished", 2.0f);
		Debug.LogWarning("FadeOutEvent - EndCutscene");
	}

	public void finished()
	{
		GameManager.instance.scaleformCamera.generalSwf.HideCutsceneDialog();
		if (! GameManager._gameState.DisplayedCutscenes.Contains(Cutscene.Name))
			GameManager._gameState.DisplayedCutscenes.Add(Cutscene.Name);
		GameManager.instance.SaveGameState(false);

		RenderSettings.fog = previousFog;
		Destroy(gameObject);
		GameManager.instance._levelManager.ReplaceSkybox();
		GameManager.instance._levelManager.ContinueLoadingStoryMode();
	}
}
