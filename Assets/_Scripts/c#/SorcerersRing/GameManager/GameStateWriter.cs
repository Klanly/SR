using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace GameStateModule{

	public class GameStateWriter 
	{
		
		public GameManager.GameStateReadDelegate gStateWriteEvent;
		
		//GameStateParser gParser=null;
		
		public GameStateWriter()
		{
			//gParser=new GameStateParser();
		}
			
		public void WriteGameState(GameState gameState)
		{
			GameManager.instance._monoHelpers.WriteIntoPersistantDataPath(GameStateParser.ConvertGameStateToJson(gameState), "CurrentGameState.txt");
			
			this.OnGameStateChange(gameState);
		}
		
		
		private void OnGameStateChange(GameState gState)
		{
			 if (gStateWriteEvent != null)
	         {
	            gStateWriteEvent(gState);
	         }
		}
		
		public void SaveJson(string fileName,string json)
		{
//			Debug.Log(" ::::::::::: SaveJson :::::::::::");
			//while writing gamestate check for its validity save it into the
			//currentGamestate file and move current gamestate into last gamestate
			GameManager.instance._monoHelpers.WriteIntoPersistantDataPath(json,fileName); //farhan
		}
		
		//public void 
	}
	
}
	