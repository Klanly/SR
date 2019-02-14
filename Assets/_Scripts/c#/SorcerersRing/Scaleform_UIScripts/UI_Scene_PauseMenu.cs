
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class UI_Scene_PauseMenu : MonoBehaviour
{
	//protected Value pauseMenu = null;
	private ScaleformCamera sfCamera = null;
	
	
	/*
	// Required to implement this constructor.
	public UI_Scene_PauseMenu(ScaleformCamera parent,SFManager sfmgr, SFMovieCreationParams cp):
		base(sfmgr, cp)
	{
		AdvanceWhenGamePaused = true;
		sfCamera=parent;
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
		/*if(GameManager.PRINT_LOGS) Debug.Log(a);
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
		if(GameManager.PRINT_LOGS) Debug.Log("UI_Scene_PauseMenu::OnRegisterSWFCallback()");
		pauseMenu = movieRef;
		Console.WriteLine("pausemenu type = " + pauseMenu.Type);
		Init();
	}
	*/
	
	public void Init()
	{
	//	sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;	 
	}
	
	/*
	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void RegisterMovies(Value movieRef)
	{
		pauseMenu = movieRef;
	}
	*/
	
	
	public void pauseGame()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("pauseCalled");
		Time.timeScale=0;
		sfCamera.currentMainCamera=Camera.main;
	
		//sfCamera.currentMainCamera.gameObject.AddComponent<BoxCollider>();
	//	sfCamera.currentMainCamera.gameObject.GetComponent<BoxCollider>().size=new Vector3(10,10,10);
		//sfCamera.currentMainCamera.gameObject.GetComponent<BoxCollider>().center=new Vector3(0,0,1);
		//GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
	//	Camera.mainCamera.enabled = false;
	}
	
	public void resumeGame()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("ResumeCalled");
		Time.timeScale=1;
		//GameObject.FindGameObjectWithTag("MainCamera").SetActive(true);
//		
//		sfCamera.DestroyLevelHUD();
//		
//		sfCamera.CreateInventoryMenu();
		
		//Time.timeScale=1;
		//Camera.main.enabled = true;
		//sfCamera.currentMainCamera.enabled=true;
	}
	
	public void onRingsButton()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Open Inventory called!");
		
	//	sfCamera.OnInventoryOpenRequest();
	}
	
	public void onOptionsButton()
	{
		//sfCamera.LoadCheatSheet();
	}
/*
	public override void Update()
	{
		if ((movie1 != null) && (movie1.type == Value.ValueType.VT_DisplayObject))
		{
			SFDisplayInfo dinfo1 = movie1.GetDisplayInfo();
			SFDisplayMatrix mat1 = movie1.GetDisplayMatrix();

			if (dinfo1 != null)
			{
				//   dinfo1.Print();
				if (ToggleZ)
				{
					dinfo1.Rotation = dinfo1.Rotation + 0.3;
					movie1.SetDisplayInfo(dinfo1);
				}
			}
		}
		if ((movie1 != null) && (movie1.type == Value.ValueType.VT_DisplayObject))
		{
			SFDisplayInfo dinfo2 = movie2.GetDisplayInfo();
			if (dinfo2 != null)
			{
				//   dinfo1.Print();
				if (ToggleY)
				{
					dinfo2.YRotation = dinfo2.YRotation + 0.3;
					movie2.SetDisplayInfo(dinfo2);
				}
			}
		}
	}
*/	
	// Callback from the content to launch the game when the "close" animation has finished playing.
//	public void OnStartGameCallback()
//	{
//		Application.LoadLevelAsync("StarshipDown");
//		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
//		
//		sfMgr.DestroyMovie(this);
//		ScaleformCamera sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
//		sfCamera.mainMenu = null;
//		sfCamera.OnLevelLoadStart();
//	}

	// Callback from the content to launch the game when the "close" animation has finished playing.
//	public void OnExitGameCallback()
//	{
//		Console.WriteLine("In OnExitGameCallback");
//		//sfMgr.DestroyMovie(this);
//		// Application.Quit() is Ignored in the editor!
//		Application.Quit();
//		// Application.LoadLevelAsync("Level");
//		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
//	}
	
	
}