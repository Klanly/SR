
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class UI_CheatSheet : MonoBehaviour
{
	//public Value cheatSheet = null;
	
	ScaleformCamera sfCamera;
	
	/*
	// Required to implement this constructor.
	public UI_CheatSheet(SFManager sfmgr, SFMovieCreationParams cp):
		base(sfmgr, cp)
	{
		if (MovieID != -1)
		{
		//	SetBackgroundAlpha(1);
			AdvanceWhenGamePaused = true;
		}
		
		Init();
	}
	*/
	
	public UI_CheatSheet(ScaleformCamera SFCamera)
	{
		this.sfCamera=SFCamera;
	}
	/*
	public void OnRegisterSWFCallback(Value movieRef)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("UI_CheatSheet::OnRegisterSWFCallback()");
		cheatSheet = movieRef;
		Console.WriteLine("mainmenu type = " + cheatSheet.Type);
		//if(GameManager.instance != null)
			//SetLanguage(GameManager.instance.scaleformCamera.languageSet, GameManager.instance.scaleformCamera.currentLangauge);
		Init();
	}
	*/
	
	/*
	public void OnSwfLoaded(Value cheatSheetSwf,string menuName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("CALEDD==========");
		if(menuName=="CheatSheet")
		{
			cheatSheet=cheatSheetSwf;
			Init();
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("SwfNAME===="+menuName);
		}
	}
	*/
	
	public void onCloseButton()
	{
		//sfCamera.generalSwf.UnloadSwf();
		sfCamera.generalSwf.UnloadSwf();
		//cheatSheet=null;
		if(!sfCamera.currentMainCamera)
		Time.timeScale=1.0f;
	}
	
	public void Init()
	{
	 sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
	}
	
	private void SetLanguage(string languageString, string charSetID)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("RAID UI --- private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
		/*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		cheatSheet.Invoke("setLanguage",args1);
		*/
	}
	
	/*public void missingString(string missingThings)
	{
		if(Debug.isDebugBuild)
		{
			GameManager.instance.scaleformCamera.generalSwf.words +=missingThings+"\n";
			if(GameManager.PRINT_LOGS) Debug.Log("Word Recieved :::: "+missingThings+" Current Words :::"+GameManager.instance.scaleformCamera.generalSwf.words);
		}
	}*/
}