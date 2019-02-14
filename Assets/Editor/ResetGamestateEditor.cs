using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResetGamestateEditor : MonoBehaviour {

	// Use this for initialization
	[MenuItem ("Custom/Reset Gamestate")]
	static void ResetGameState () {

		PlayerPrefs.DeleteAll();
//		GameManager.instance._monoHelpers.CopyDefaultToCurrentGamestate();
		CopyDefaultGameStateToCurrentGameState();

		WriteIntoPersistantDataPath("", PurchaseManager.FILE_NAME);
		WriteIntoPersistantDataPath("", PurchaseManager.RECEIPTS_FILE_NAME);
	}

	public static void WriteIntoPersistantDataPath (string dataString, string filename)
	{
		string urlToWrite;
		if (Application.isEditor) {
			urlToWrite = Helpers.formatLocalUrlToWrite (filename);
		} else {
			urlToWrite = Helpers.formatLocalPersistentUrlToWrite (filename);
		}
		
		//		if(GameManager.PRINT_LOGS) Debug.Log("urlToWrite"+urlToWrite + " filename = " + filename);
		Debug.LogError(urlToWrite);
		if (File.Exists (urlToWrite)) {
			try {
				File.WriteAllText (urlToWrite, dataString);
			} catch (System.Exception e) {
				/*if(GameManager.PRINT_LOGS)*/
				Debug.Log ("error writing file : " + filename + " error = " + e.ToString ());
			}
			
		} else {

			StreamWriter sw = File.CreateText (urlToWrite);
			sw.WriteLine (dataString);
			//File.WriteAllText(urlToWrite,dataString);
			
			sw.Close ();
		}
		
	}

	public static void CopyDefaultGameStateToCurrentGameState ()
	{
		string fromFileName = "DefaultGameState.txt";
		string toFileNameCurrent = "CurrentGameState.txt";
		string toFileNameLast = "LastGameState.txt";

		string urlToRead = Helpers.formatLocalUrlToRead (fromFileName);
		
		string text = File.ReadAllText(Application.streamingAssetsPath+"/"+fromFileName);
//		Debug.LogError("text "+text);
		WriteIntoPersistantDataPath (text, toFileNameCurrent);
		WriteIntoPersistantDataPath (text, toFileNameLast);
	}

	[MenuItem ("Custom/Delete Player Prefs")]
	static void DeletePlayerPrefs () {		
		PlayerPrefs.DeleteAll();
	}
	
	static bool isMute = false;
	[MenuItem ("Custom/Mute Sound")]
	static void MuteSound () {	
		isMute = PlayerPrefs.GetInt("IsEditorMuted", 0) == 1 ? true : false;
		if(isMute)
			MasterAudio.UnmuteEverything();
		else
			MasterAudio.MuteEverything();
		isMute = !isMute;
		PlayerPrefs.SetInt("IsEditorMuted", isMute ? 1 : 0);
	}

}
