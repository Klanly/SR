using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MonoHelpers : MonoBehaviour
{
	
	public delegate void LoadFileDelegate (bool isError,string fileContent);
	
	public Light mainDirectionalLight;
	public IEnumerator LoadIntoPersistantDataPathFromLocalPath (string filename, GameManager.FileLoadedIntoPersistant fileLoaded)
	{
		string urlToRead = Helpers.formatLocalUrlToRead (filename);
		if (GameManager.PRINT_LOGS)
			Debug.Log ("urlToRead" + urlToRead);
		
		WWW www = new WWW (urlToRead);
		
		yield return www;
		
		if (www.error == null) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("www.text" + www.text);
			WriteIntoPersistantDataPathFromLocalPath (www.text, filename, fileLoaded);
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("PersistantPathERROR: " + www.error);
		}	
	}
	public void SendEmail (string emaiAddress, string emailSubject, string emailBody)
	{
		string email = emaiAddress;
		string subject = MyEscapeURL (emailSubject);
		string body = MyEscapeURL (emailBody);
	 
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
	 
	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL (url).Replace ("+", "%20");
	}

	public string formatCommaSeperatedString (List<string> tempList)
	{
		string temp = "";
		if (tempList.Count > 0) {
			for (int i=0; i<tempList.Count; i++) {
				temp += tempList [i].ToString ();
				if (i != (tempList.Count - 1)) {
					temp += ",";
				}
			}
		}
		return temp;
	}
	
//	private	void APICallback(FBResult result)                                                                                              
//	{                                                                                                                              
//	    if(GameManager.PRINT_LOGS) FbDebug.Log("APICallback");                                                                                                
//	    if (result.Error != null)                                                                                                  
//	    {                                                                                                                          
//	        FbDebug.Error(result.Error);                                                                                           
//	        FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, APICallback);     
//	        return;                                                                                                                
//	    }                                                                                                                          
//	
//	 		IDictionary profile = Json.Deserialize(result.Text) as IDictionary;
//			
//	                                                                        
//	} 
	
	public bool CheckFileExistance (string filename)
	{
		string url;
		if (Application.isEditor) {
			url = Helpers.formatLocalUrlToWrite (filename);
		} else {
			url = Helpers.formatLocalPersistentUrlToWrite (filename);
		}
		
		bool success = false;
		
		if (File.Exists (url)) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~File Exists : Check File Existance Function~~~~~~~~~" + filename);
			success = true;
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~File Does Not Exists : In Check File Existance Function~~~~~~~~~" + filename);
			success = false;
		}
		
		return success;
		
	}
	
	public bool DeleteFile (string filename)
	{
		string url;
		if (Application.isEditor) {
			url = Helpers.formatLocalUrlToWrite (filename);
		} else {
			url = Helpers.formatLocalPersistentUrlToWrite (filename);
		}
		
		bool success = false;
		
		try {
			File.Delete (url);
			success = true;
		} catch (System.Exception e) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("" + e.ToString ());
			success = false;
		}
		
		return success;
	}
	
	public void WriteIntoPersistantDataPathFromLocalPath (string dataString, string filename, GameManager.FileLoadedIntoPersistant fileLoaded)
	{
		string urlToWrite = Helpers.formatLocalPersistentUrlToWrite (filename);
		
		if (GameManager.PRINT_LOGS)
			Debug.Log ("urlToWrite" + urlToWrite);
		if (File.Exists (urlToWrite)) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~File Exists~~~~" + filename);
			File.WriteAllText (urlToWrite, dataString);
		
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~File Does Not Exists~~~~" + filename);
			StreamWriter sw = File.CreateText (urlToWrite);
			sw.WriteLine (dataString);
			//File.WriteAllText(urlToWrite,dataString);
			
			sw.Close ();
		}
	
		fileLoaded (filename);
		
	}
	
	public bool FileExistsAtWriteablePath (string filename)
	{
		string urlToWrite;
		if (Application.isEditor)
			urlToWrite = Helpers.formatLocalUrlToWrite (filename);
		else
			urlToWrite = Helpers.formatLocalPersistentUrlToWrite (filename);

		if (File.Exists (urlToWrite))
			return true;
		return false;
	}
	
	public void WriteIntoPersistantDataPath (string dataString, string filename)
	{
		string urlToWrite;
		if (Application.isEditor) {
			urlToWrite = Helpers.formatLocalUrlToWrite (filename);
		} else {
			urlToWrite = Helpers.formatLocalPersistentUrlToWrite (filename);
		}
		
//		if(GameManager.PRINT_LOGS) Debug.Log("urlToWrite"+urlToWrite + " filename = " + filename);
		if (File.Exists (urlToWrite)) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~File Exists~~~~" + filename);
			try {
				File.WriteAllText (urlToWrite, dataString);
			} catch (System.Exception e) {
				/*if(GameManager.PRINT_LOGS)*/
				Debug.Log ("error writing file : " + filename + " error = " + e.ToString ());
			}
		
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~File Does Not Exists~~~~" + filename);
			
			StreamWriter sw = File.CreateText (urlToWrite);
			sw.WriteLine (dataString);
			//File.WriteAllText(urlToWrite,dataString);
			
			sw.Close ();
		}
		
	}
	
	public IEnumerator CopyIntoAnotherFileOnPersistantDataPath (string fromFileName, string toFileName)
	{
		string urlToRead;
		//string urlToWrite;
		
		if (Application.isEditor) {
			urlToRead = Helpers.formatLocalUrlToRead (fromFileName);
			//urlToWrite=Helpers.formatLocalUrlToWrite(toFileName);
		} else {
			urlToRead = Helpers.formatLocalPersistentUrlToRead (fromFileName);
			//urlToWrite=Helpers.formatLocalPersistentUrlToWrite(toFileName);
		}
		
		WWW www = new WWW (urlToRead);
		
		yield return www;

		if (www.error == null) {
//			if(GameManager.PRINT_LOGS) Debug.Log("Coppying from "+fromFileName+" to "+toFileName+ www.text);
			//File.WriteAllText(urlToWrite,www.text);
			WriteIntoPersistantDataPath (www.text, toFileName);
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Error While Copying file: " + www.error);
		}
		
		yield return null;
		
		
		
	}

	public void CopyDefaultToCurrentGamestate() {
		StartCoroutine("CopyDefaultGameStateToCurrentGameState");
	}


	public IEnumerator CopyDefaultGameStateToCurrentGameState ()
	{
		string fromFileName = "DefaultGameState.txt";
		string toFileName = "CurrentGameState.txt";
		
		string urlToRead = Helpers.formatLocalUrlToRead (fromFileName);
		
		WWW www = new WWW (urlToRead);
		
		yield return www;

		if (www.error == null) {
//			if(GameManager.PRINT_LOGS) Debug.Log("Coppying from "+fromFileName+" to "+toFileName+ www.text);
			//File.WriteAllText(urlToWrite,www.text);
			WriteIntoPersistantDataPath (www.text, toFileName);
			if (GameManager.instance.isResetGameToDefault) {
				GameManager.instance.RequestGameState ();
			}
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Error While Copying file: " + www.error);
		}
		
		yield return null;
	}

	public IEnumerator CopySwfFile (string fileName)
	{
		//string fromFileName = "GameUI.swf";
		//string toFileName = "GameUI.swf";
		if (Application.isEditor) {
			if (!Directory.Exists (Helpers.formatLocalUrlToWrite ("PersistentSwfs"))) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Directory Does Not Already Existtss " + fileName);
				Directory.CreateDirectory (Helpers.formatLocalUrlToWrite ("PersistentSwfs"));
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Directory Already Existtss " + fileName);
			}
		} else {
			if (!Directory.Exists (Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs"))) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Directory Does Not Already Existtss " + fileName);
				Directory.CreateDirectory (Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs"));
				if (GameManager.PRINT_LOGS)
					Debug.Log (Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs"));
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Directory Already Existtss " + fileName);
			}
		}
		

		string urlToRead = Helpers.formatLocalUrlToRead (fileName);
		
		WWW www = new WWW (urlToRead);
		
		yield return www;
		if (www.error == null) {
			string urlToWrite;
			string[] nameArr = fileName.Split ('/');
			if (GameManager.PRINT_LOGS)
				Debug.Log ("nameArrLength==" + nameArr.Length);
			if (nameArr != null && nameArr.Length > 1) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("==================FOLDERNAME" + nameArr [0] + fileName + "=============");
				if (Application.isEditor) {
					if (!Directory.Exists (Helpers.formatLocalUrlToWrite ("PersistentSwfs/" + nameArr [0]))) {
						if (GameManager.PRINT_LOGS)
							Debug.Log ("Directory Does Not Already Existtss " + fileName);
						Directory.CreateDirectory (Helpers.formatLocalUrlToWrite ("PersistentSwfs/" + nameArr [0]));
					} else {
						if (GameManager.PRINT_LOGS)
							Debug.Log ("Directory Already Existtss " + fileName);
					}
				} else {
					if (!Directory.Exists (Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs/" + nameArr [0]))) {
						if (GameManager.PRINT_LOGS)
							Debug.Log ("Directory Does Not Already Existtss " + fileName);
						Directory.CreateDirectory (Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs/" + nameArr [0]));
						Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs/" + nameArr [1]);
					} else {
						if (GameManager.PRINT_LOGS)
							Debug.Log ("Directory Already Existtss " + fileName);
					}
				}
				nameArr = null;
			}
			if (Application.isEditor) {
				urlToWrite = Helpers.formatLocalUrlToWrite ("PersistentSwfs/" + fileName);
			} else {
				urlToWrite = Helpers.formatLocalPersistentUrlToWrite ("PersistentSwfs/" + fileName);
			}
			
			if (GameManager.PRINT_LOGS)
				Debug.Log ("urlToWrite" + urlToWrite + "==" + www.bytes.Length);
			using (FileStream fileStream = new FileStream(urlToWrite, FileMode.OpenOrCreate, 
			                                              FileAccess.ReadWrite, 
			                                              FileShare.None)) {
				fileStream.Write (www.bytes, 0, www.bytes.Length);
				
			}
			if (fileName.Equals ("GameUI.swf")) {
				BundleDownloadManager.instance.SFCamera.LoadGeneralSwf (BundleDownloadManager.instance.OnGeneralSwfLoaded);
			}
			//GameManager.instance.loadSwf();
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Error While Copying file: " + www.error);
		}
		
		yield return new WaitForSeconds (0.2f);
		
		
		
	}
	
	public string LoadTextFromWriteablePath (string filename, out string filePath)
	{
		string _filepath = Helpers.formatLocalUrlToRead (filename);
		WWW www = new WWW (_filepath);
		
		filePath = _filepath;
		if (www.error == null)
			return www.text;
		return null;
	}
	
	//Function to load file text from streaming assets
	public IEnumerator LoadFromFile (string fileName, bool isLocalPath, LoadFileDelegate fileDelegate)
	{
		string filePath = "";
		if (isLocalPath) {
			filePath = Helpers.formatLocalUrlToRead (fileName);
		} else {
			filePath = Helpers.formatLocalPersistentUrlToRead (fileName);
		}

		WWW www = new WWW (filePath);
		
		yield return www;

		if (www.error == null) {
//			if(GameManager.PRINT_LOGS) Debug.Log("JSON -->>" + www.text);
			fileDelegate (false, www.text);
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("filePathError: " + www.error);
			fileDelegate (true, "");
		}
		
		yield return null;
	}
	
	public void CheckForSwfsExistance ()
	{
		if (GameManager.instance.scaleformCamera.levelScene == null) {
			GameManager.instance.scaleformCamera.OnLoadLevelStart ();
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~Level Swf AlreadyLoaded~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			//GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(true);
			UIManager.instance.levelScene.SetDisplayVisible (true);
		}

//		if(GameManager.instance.scaleformCamera.guildUI ==null)
//		{
//			GameManager.instance.scaleformCamera.LoadGuildUI();
//		}
//		else
//		{
//			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~~~~~~~~~~~~Level Swf AlreadyLoaded~~~~~~~~~~~~~~~~~~~~~~~~~~~");
//			//GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(true);
//			UIManager.instance.guildUI.gameObject.SetActive(true);
//        }

		if (GameManager.instance.scaleformCamera.hud == null) {
			UIManager.instance.OnLoadBattleStart ();
			//GameManager.instance.scaleformCamera.OnLoadBattleStart();
			//_levelCameras.sfCamera.hud.SetDisplayVisible(false);
//			UnityEngine.Debug.Log(">>> SetDisplayVisible > F");
			UIManager.instance.hud.SetDisplayVisible (false);
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~Battle Swf AlreadyLoaded~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			
			UnityEngine.Debug.Log (">>> SetDisplayVisible > F");
			GameManager.instance.scaleformCamera.hud.SetDisplayVisible (false);
		}
		
		if (GameManager.instance.scaleformCamera.generalSwf == null) {
			GameManager.instance.scaleformCamera.LoadGeneralSwf ();
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("~~~~~~~~~~~~~~~~~~~~~~General Swf AlreadyLoaded~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			GameManager.instance.scaleformCamera.generalSwf.Init ();
			//_levelCameras.sfCamera.hud.SetDisplayVisible(true);
			
//			UnityEngine.Debug.Log(">>> SetDisplayVisible > T");
			UIManager.instance.hud.SetDisplayVisible (false);
		}
	}
	public void SetCurrentLevelsRendererSettings ()
	{
		ShadowsOn ();
		if(GameManager._gameState != null) {
			SoundManager.instance.SetSfxVolume(GameManager._gameState.gfxVolume);
			SoundManager.instance.SetMVolume(GameManager._gameState.musicVolume);
		}

		switch (GameManager.instance._levelManager.currentLevel) {
		case "ToHollowTree":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.01f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "HollowTree":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.01f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "StatuePath":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.01f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "ThreeGods":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.01f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "ToCaveEntrance":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.02f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "CaveEntrance":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.Linear;
			RenderSettings.fogColor = new Color32 (128, 128, 128, 255);
			RenderSettings.fogDensity = 0.015f;
			RenderSettings.fogStartDistance = -520.0f;
			RenderSettings.fogEndDistance = 1742.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "ForbiddenCave":
			ShadowsOff ();
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			break;
		case "DemonCanyon":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_mountain");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (223, 248, 255, 255);
			RenderSettings.fogDensity = 0.0002f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "ToDemonFalls":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_mountain");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (179, 239, 255, 255);
			RenderSettings.fogDensity = 0.0002f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		case "DemonFalls":
			SoundManager.instance.changeBackgroundSoundTo ("background_magical_forest");
			SoundManager.instance.changeAmbientSoundTo ("ambient_magical_forest");
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogColor = new Color32 (179, 239, 255, 255);
			RenderSettings.fogDensity = 0.0002f;
			RenderSettings.fogStartDistance = 0.0f;
			RenderSettings.fogEndDistance = 300.0f;
			RenderSettings.ambientLight = new Color32 (100, 100, 100, 255);
			break;
		}
		//Shader.WarmupAllShaders();
	}
	
	public void ShadowsOn ()
	{
		mainDirectionalLight.shadows = LightShadows.Hard;
	}
	
	public void ShadowsOff ()
	{
		mainDirectionalLight.shadows = LightShadows.None;
	}
	
	public void MainLightOn ()
	{
		mainDirectionalLight.enabled = true;
	}
	
	public void MainLightOff ()
	{
		mainDirectionalLight.enabled = false;
	}
	
	private const string TAG_TAG_SPLITTER = "|";
	public static string[] GetSeparateTags (string tag)
	{
		return tag.Split (TAG_TAG_SPLITTER.ToCharArray ());
	}
	
	public static T[] CombineArrayExcludingCommon<T> (T[] arrayOne, T[] arrayTwo)
	{
		List<T> uniqueList = new List<T> ();
		T anItem;
		int length = arrayOne.Length;
		for (int i = 0; i < length; i++) {
			anItem = arrayOne [i];
			T match = uniqueList.Find (str => str.Equals (anItem));
			if (match == null || match.Equals (""))
				uniqueList.Add (match);
		}
		length = arrayTwo.Length;
		for (int i = 0; i < length; i++) {
			anItem = arrayTwo [i];
			T match = uniqueList.Find (str => str.Equals (anItem));
			if (match == null || match.Equals (""))
				uniqueList.Add (match);
		}
		return uniqueList.ToArray ();
	}

	public IEnumerator GetTextFromWriteablePathFile (string filename, System.Action<string> textListener)
	{
		string url;
		if (Application.isEditor) {
			url = Helpers.formatLocalUrlToRead (filename);
		} else {
			url = Helpers.formatLocalPersistentUrlToRead (filename);
		}

		WWW www = new WWW (url);
		
		yield return www;
		if (www.error == null) {
			textListener (www.text);
		} else {
			textListener (null);
		}
	}
}
