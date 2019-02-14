using UnityEngine;
using System.Collections;
	
namespace GameStateModule{
	
	public class GameStateHandler : MonoBehaviour
	{
		
		private const string TAG = "GameStateHandler";

		GameStateIO gStateIO = null;
		
		void Awake()
		{
			gStateIO = new GameStateIO();	
		}
		
		
		public void Subscribe(GameManager.GameStateReadDelegate gameStateReadDel)
		{
			gStateIO.Subscribe(gameStateReadDel);
		}
		
		
		public void GetGameState()
		{
			StartCoroutine(GetGameStateCoroutine());
		}
		
		
		IEnumerator GetGameStateCoroutine()
		{
			gStateIO.ReadGameState();
			
			yield return null;
		}
		
		
		public void SaveGameState(GameState gameState)
		{
			gStateIO.WriteGameState(gameState);
		}
	
		
	
		/*
		 * In case you want to reset a user's progress to the start!
		 */
		public bool ResetGameState()
		{
			return false;
		}
		
		
		
		
	}
}