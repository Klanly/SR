using System;
using UnityEngine;
using System.Runtime.InteropServices;
using MiniJSON;
using System.Collections;
using InventorySystem;

public class UI_Scene_Inventory : MonoBehaviour
{
	//public Value inventoryScene = null;
	
	ScaleformCamera sfCamera;
	
	public UI_Scene_Inventory(ScaleformCamera parent)
	{
		sfCamera=parent;
	}
	
	public void RegisterMovies()
	{
	//	inventoryScene = movieRef;
	}
	
	public void OnRegisterSWFCallback()
	{
		if(GameManager.PRINT_LOGS) Debug.Log("----------------------->>>>>>>>>>>>>>>>     ON REGISTER SWF FOR INVENTORYYYYYYY <<<<<<<<<<<<<<----------------------");
		
	//	inventoryScene = movieRef;
		
		SetLanguage(sfCamera.languageSet, sfCamera.currentLangauge);
		
		//sfCamera.setFocusOnPauseMenu();
		//Invoke("Init", 1.0f);
		
		Init();
	}
	
	/*
	public void OnSwfLoaded(Value inventorySwf,string menuName)
	{
		if(menuName=="Inventory")
		{
			inventoryScene=inventorySwf;
			Init();
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("SwfNAME===="+menuName);
		}
	}
	*/	
	public void Init()
	{	
		//if(GameManager.PRINT_LOGS) Debug.Log(GameManager._gameState.User._inventory.json);
		 //SetInventoryData(GameManager._gameState.User._inventory.json);
	}
	
	
	public void SetInventoryData(string jsonString,long MovieIDD)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("JSON SENT TO SCALEFORM - "+jsonString);
		
	}
	
	public void backButtonClicked(string jsonString)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("JSON STRING ON INVENTORUY SAVE---------->>>>>>>"+jsonString);
		
		IDictionary fullJson = Json.Deserialize(jsonString) as IDictionary;
		
		IDictionary inventoryJson = fullJson["Inventory"] as IDictionary;
		
		GameManager._gameState.User._inventory = InventoryLoader.LoadInventory(inventoryJson);
		
	//	sfCamera.OnBackButtonClicked();
	}
	
	private void UpdateSoulGems(int number)
	{
		//inventoryScene.Invoke("updateSoulGems",number);
	}
	
	private void SetLanguage(string languageString, string charSetID)
	{
		/*
		if(GameManager.PRINT_LOGS) Debug.Log("UI INVENTORY SCENE --- private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		inventoryScene.Invoke("setLanguage",args1);
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
