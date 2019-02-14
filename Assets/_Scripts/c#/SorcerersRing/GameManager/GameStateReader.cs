using UnityEngine;
using System.Collections;

namespace GameStateModule
{
	
	public class GameStateReader : MonoBehaviour
	{
	
		public GameManager.GameStateReadDelegate gStateReaderEvent;
		
		
		//GameStateParser gParser=null;
		
		public GameStateReader ()
		{
			//gParser=new GameStateParser();
		}
	
		public void ReadGameState ()
		{
			if (GameManager.HasInstance && GameManager.instance._monoHelpers.CheckFileExistance ("CurrentGameState.txt")) {
				StartCoroutine (ReadGameStateData ("CurrentGameState.txt"));	
			} else {
				StartCoroutine (ReadGameStateData ("LastGameState.txt"));
			}
		}
		
		public void OnGameStateChange (GameState gState, string filename, bool success)
		{
			if (success) {
				if (gStateReaderEvent != null) {
					gStateReaderEvent (gState);
				}
			} else {
				if (filename.Equals ("CurrentGameState.txt")) {
					StartCoroutine (ReadGameStateData ("LastGameState.txt")); 
				} else if (filename.Equals ("LastGameState.txt")) {
					if (GameManager.PRINT_LOGS)
						Debug.Log ("Call Server Because all the local game states are corrupted");
				}
			}
		}
		
		IEnumerator ReadGameStateData (string filename)
		{
			if (GameManager.PRINT_LOGS)
				Debug.Log ("ReadGameStateData: " + filename);
			string url;
			if (Application.isEditor) {
				url = Helpers.formatLocalUrlToRead (filename);
			} else {
				url = Helpers.formatLocalPersistentUrlToRead (filename);
			}
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~Reading GameState From===" + url);
			WWW www = new WWW (url);
			
			yield return www;
			if (www.error == null) {
				Debug.Log ("DISK GAMESTATE REQUEST!!!:::::::::::::::::::::::: > "+www.text);
				GameStateParser.ProcessGameState (this.OnGameStateChange, www.text, new GameState (), filename);

			} else {
//				if (GameManager.PRINT_LOGS)
					Debug.Log ("GameStateReaderERROR: " + www.error);
				throw new System.Exception ();
			}
		}
		
	}
}