using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneHandler : MonoBehaviour
{
		public Transform Has;
		private Imp_CinematicControlScript cntHas;
		public Transform HasLookAtNode;
		public Transform Sorcerer;
		private Sorcerer_CinematicControlScript cntSorcerer;
		public Transform SorcererLookAtNode;
		public GameObject storyCam;

		public Cutscene cutscene;
		private int currentDialog = -1;
		private bool ended = false;
		private Camera previousCamera;
	
		public void StartCutscene()
		{
			if(GameManager.PRINT_LOGS) Debug.Log("Showing cutscene: " + cutscene.Name);
			cntHas = Has.GetComponent<Imp_CinematicControlScript> ();
			cntSorcerer = Sorcerer.GetComponent<Sorcerer_CinematicControlScript> ();
			previousCamera = Camera.main;
			
			GameManager.instance.scaleformCamera.generalSwf.ShowCutsceneDialog(0, 0);	
			GameManager.instance.scaleformCamera.generalSwf.OnNextClicked = () => {
				if (ended)
					return;

				if (currentDialog <= -1)
				{
					StartCutscene ();
				}
				else if (currentDialog >= cutscene.Dialogues.Count - 1)
				{
					EndCutscene();
				}
				else
				{
					ShowNextDialog();
				}
			};
		
			previousCamera.active = false;
			storyCam.camera.active = true;

			ShowNextDialog();
		}

		private void ShowNextDialog()
		{
			currentDialog++;
			GameManager.instance.scaleformCamera.generalSwf.ChangeCutsceneDialog(cutscene.Dialogues[currentDialog].Actor, cutscene.Dialogues[currentDialog].Script, 0, 0);
			updateActor();
		}
		
	private void updateActor ()
		{
			Transform target;
			if (cutscene.Dialogues[currentDialog].Actor.Trim().ToLower()  == "ruh"){
				iTween.LookTo (storyCam, SorcererLookAtNode.transform.position, 1.0f);
				cntHas.StartListening ();
				cntSorcerer.StartTalking ();
	
			} else {
				iTween.LookTo (storyCam, HasLookAtNode.transform.position, 1.0f);
				cntHas.StartTalking ();
				cntSorcerer.StartListening ();			
			}
			
		}

		public void EndCutscene()
		{
			Debug.LogError("EndCutscene - FadeOutEvent");

			ended = true;
			PlayMakerFSM.BroadcastEvent("FadeOutEvent");
			Invoke("finished", 2.0f);
		Debug.LogWarning("FadeOutEvent - CutsceneHandler");
	}
		
		private void finished()
		{
			GameManager.instance.scaleformCamera.generalSwf.HideCutsceneDialog();
			if (! GameManager._gameState.DisplayedCutscenes.Contains(cutscene.Name))
				GameManager._gameState.DisplayedCutscenes.Add(cutscene.Name);
			GameManager.instance.SaveGameState(false);

			storyCam.camera.active = false;
			previousCamera.active = true;
			
			Destroy(gameObject);
			GameManager.instance._levelManager.ContinueLoadingStoryMode();
		}
}