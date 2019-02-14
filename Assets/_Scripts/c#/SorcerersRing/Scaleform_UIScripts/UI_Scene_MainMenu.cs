
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class UI_Scene_MainMenu : MonoBehaviour
{
	//protected Value mainMenu = null;
	//ScaleformCamera sfCamera = null;
	
	/*
	// Required to implement this constructor.
	public UI_Scene_MainMenu(//ScaleformCamera parent,
		SFManager sfmgr, SFMovieCreationParams cp):
		base(sfmgr,new MovieDef( SFManager.GetScaleformContentPath()+"Title.swf"), cp)
	{
		//sfCamera=parent;
//		if (MovieID != -1)
//		{
//			
//		//	SetBackgroundAlpha(1);
//		}
//		
		// 
	}
	*/
	
//	public void loadGame(string a)
//	{
//		Application.LoadLevelAsync("TestScene");
//		sfMgr.DestroyMovie(this);
//		ScaleformCamera sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
//		sfCamera.mainMenu = null;
//		sfCamera.OnLevelLoadStart();
//	}
	/*public void doSomething(string a)
	{
		/*Debug.Log(a);
		Value retVal = new Value();
		Value val=new Value(0.5f,MovieID);
		Value[] args1={val};
		//mainMenu.SetMember
		mainMenu.Invoke("updateLoadingBar",args1,ref retVal);*/
		
		//Value bar=mainMenu.GetMember("loadingBar").GetMember("barFill");
	//	bar.SetMember("scaleX",new Value(1,bar.MovieId));
	//}
	
	/*
	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void OnRegisterSWFCallback(Value movieRef)
	{
//		Debug.Log("UI_Scene_MainMenu::OnRegisterSWFCallback()");
		mainMenu = movieRef;
		Console.WriteLine("mainmenu type = " + mainMenu.Type);
		Init();
	}
	*/
	public void PlayOk()
	{
		SoundManager.instance.PlayMenuOkSound();
	}
	
	public void onContinue()
	{
		Debug.LogWarning("OnContinue called - FadeOutEvent");
		PlayMakerFSM.BroadcastEvent("FadeOutEvent");
		GameManager.instance.Invoke("StartStoryMode",2.0f);
	}
	
	public void onMultiplayer()
	{
		Debug.Log("::: onMultiplayer :::");
		GameManager.instance.StartMultiPlayerMode();
	
	}
	public void Init()
	{
		 
	}
	public void EnableMultiplayer()
	{
		/*
		Value[] args = {};
		mainMenu.Invoke("enableMultiplayer",args);
		*/
	}
	
	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	/*
	public void RegisterMovies(Value movieRef)
	{
		mainMenu = movieRef;
	}
	*/
}