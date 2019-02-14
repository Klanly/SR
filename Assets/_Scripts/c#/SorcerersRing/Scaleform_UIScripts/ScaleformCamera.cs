using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
//using Scaleform.GFx;

// Note that SFCamera is currently a Singleton as it creates a new SFMgr in Start().
public class ScaleformCamera : MonoBehaviour {
	public UI_Raid 					raidUI			= null;
	public UI_Guild 				guildUI			= null;
	public UI_GeneralSwf 			generalSwf = null;
	public bool						isPaused		= false;
    public bool                     isInventoryLoaded = false;
	public bool						showRingShredsPopUp = false;
	public Camera currentMainCamera;

	
	public TextAsset languageStringFile;
	public string _languageSet;
	public string languageSet
	{
		get
		{
			return _languageSet;
		}
		private set	{	_languageSet = value;}
	}
	
	public string currentLangauge
	{
		private set;
		get;
	}
	
	private void OnDelegateCallback( long MovieID, String command, IntPtr pValue, int numArgs, int valueSize)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("CALLBACK FROM DEEGATE RECEIVED!!!!!!!!!!!!!!!!!!!");
	}
	
	
	private void OnGuildUILoaded()
	{
		OnUsernameListener(GameManager._gameState.User.username);
		if(GameManager.PRINT_LOGS) Debug.Log("Guild UI loaded!");
	}
	
	public void OnUsernameListener(string username) 
	{
		/* if(GameManager.PRINT_LOGS) */ Debug.Log("::::::::::::::::: OnUsernameListener username" + username);
		if(string.IsNullOrEmpty(username))
		{
			DestroyGuildUI();
			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: OnUsernameListener username NULL" );
		}
		/* else if(guildUI == null)
		{
			CreateGuildUI(this.OnGuildUILoaded);
			if(GameManager.PRINT_LOGS) Debug.Log("::::::::::::::::: GUILD UI ==== NULL" );
		}*/
	}
	
	public IEnumerator CreateRaidUI(System.Action ack, UI_Raid.CharacterSelectionHandler charSelectHandler = null)
	{
		//Delayed call to the scaleform because scaleform cannot handle multiple calls unless a callback isnt back for previous swf
		if(!GameManager.instance.isGeneralSwfLoaded)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("GameManager.instance.isGeneralSwfLoaded ----------------- XXXXXXXXXXXXXXXXXXXXXXXXXXXXX ");
			yield return new WaitForSeconds(0.1f);
			StartCoroutine(CreateRaidUI(ack, charSelectHandler));
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("GameManager.instance.isGeneralSwfLoaded ----------------- ********************************************** ");

			#if (UNITY_IPHONE || UNITY_EDITOR)
			//raidUI = new UI_Raid(SFMgr, CreateMovieCreationParams("Raidsinterface.swf"), ack, charSelectHandler);
			#elif UNITY_ANDROID
			/*
			IDictionary aSwfData = SFSwfsUrl["Raidsinterface"] as IDictionary; 
			Byte[] raidSwfContent = getSwfInBytes(aSwfData["url"].ToString());
			raidUI = new UI_Raid(SFMgr, CreateMovieCreationParams("Raidsinterface.swf",1,raidSwfContent, new Color(255,255,255,0), true),ack,charSelectHandler);
			*/
			#endif
		}
	}
	
	public void LoadGuildUI()
	{
		if(GameManager.PRINT_LOGS) Debug.Log(":::::::::::::::::::::: LoadGuildUI :::::::::::::::::::::::::::::::::::: ");
		GameManager._gameState.User.usernameChangeEvent += new User.UsernameChangeDelegate(this.OnUsernameListener);
		OnUsernameListener(GameManager._gameState.User.username);
	}
	
	public void CreateGuildUI(System.Action ack)
	{
		if(guildUI == null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("guildUI NULL ::: CreateGuildUI CALLED");
			//guildUI = new UI_Guild(SFMgr, CreateMovieCreationParams(UI_Guild.GUILD_SWF_NAME, 100), ack);
		}
	}

	
	public void DestroyRaidUI()
	{
		if(raidUI!=null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyRaidUI()");
			//SFMgr.DestroyMovie(raidUI);
			
			raidUI=null;
		}
	}
	
	public void DestroyGuildUI()
	{
		if(guildUI!=null)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("ScaleformCamera::DestroyGildUI()");
			//SFMgr.DestroyMovie(guildUI);
			
			guildUI=null;
		}
	}

	public bool CheckFileExistance(string filename)
	{
		string url;
		if(Application.isEditor)
		{
			url=Helpers.formatLocalUrlToWrite(filename);
		}
		else
		{
			url=Helpers.formatLocalPersistentUrlToWrite(filename);
		}
		
		bool success=false;
		
		if(File.Exists(url))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Exists : Check File Existance Function~~~~~~~~~"+filename);
			success=true;
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~File Does Not Exists : In Check File Existance Function~~~~~~~~~"+filename);
			success=false;
		}
		
		return success;
		
	}
	
	
}