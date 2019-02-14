using UnityEngine;
using System.Collections;
	
namespace GameStateModule{
	
	public class GameStateIO 
	{
		private const string TAG = "*** GAME STATE IO ...";
		
		GameStateWriter gStateWriter;
		
		GameStateReader gStateReader;
		GameState gState;
		
		public delegate void ReadUserDataDelegate(GameState gameState);
		public delegate void ReadZoneDataDelegate(GameState gameState);
		
		public ReadUserDataDelegate myUserDelagate;
		public ReadZoneDataDelegate myZoneDelegate;
		
		public GameStateIO()
		{
			gStateReader =GameObject.Find("gReader").GetComponent<GameStateReader>();
		
			gStateWriter = new GameStateWriter();
			
			gState = new GameState();
			
			SubscribeGameState();
		}
		
		
		public void SetUserState(ReadUserDataDelegate userDelegate)
		{
			this.myUserDelagate=userDelegate;
		}
	
		public void SetZoneState(ReadZoneDataDelegate zoneDelegate)
		{
			this.myZoneDelegate=zoneDelegate;
		}
		
		
		public void SubscribeGameState()
		{
			SetUserState(gState.OnUserStateRecieved);
		}
		
		
		public void WriteGameState(GameState gameState)
		{
			gStateWriter.WriteGameState(gameState);
		}
		
		
		public void ReadGameState()
		{
			gStateReader.ReadGameState();
		}
		
		public void OnUserStateRecieved(GameState gState)
		{
			this.myUserDelagate(gState);
			gStateReader.OnGameStateChange(gState,"",true);
		}
		
//		public void OnZoneStateRecieved(GameState gState)
//		{
//			this.myZoneDelegate(gState);
//			gStateReader.OnGameStateChange(gState);
//		}
		
		
		public void Subscribe(GameManager.GameStateReadDelegate gameStateReadDel)
		{
			gStateReader.gStateReaderEvent = gameStateReadDel;
			gStateWriter.gStateWriteEvent = gameStateReadDel;
		}
		
	}
}