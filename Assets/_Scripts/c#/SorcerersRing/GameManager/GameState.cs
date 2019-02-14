using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameStateModule{

	[System.Serializable]
	public class GameState 
	{
		public User User;
		public LevelState LevelState=null;
		List<string> displayedCutscenes = new List<string>();
		public IList shrineList;

		public GameState()
		{
			User=new User();
			LevelState=new LevelState();
			bossAttemptDictionary = new Dictionary<string, object>();
			shrineList = new List<IDictionary>();
		}

		public int runeProgress;
		public int spiritProgress;

		public List<string> DisplayedCutscenes
		{
			get
			{
				return displayedCutscenes;
			}
			set
			{
				displayedCutscenes = value;
			}
		}
//		public void SetGameState()
//		{
//		}
////		
//		public User User
//		{
//			return this.User;
//		}
		
		public bool firstEnemyDefeated = false;
		
		public void SetUser(User user)
		{
			this.User = user;
		}
		
//		public LevelState LevelState
//		{
//			return this._levelState;
//		}
		
		
		public void OnUserStateRecieved(GameState gState)
		{
			this.User=gState.User;
		}
		
		private int _skullLevel;
		public int skullLevel
		{
			get
			{
				return _skullLevel;
			}
			set
			{
				_skullLevel = value;
				
				if(GameManager.enableAnalytics)
					Analytics.logEvent(Analytics.CurrencyTrackType.TrackLevel, _skullLevel);
			}
		}
		
		public bool _unlockRunesShown = false;
		public bool _unlockSpiritsShown = false;
		public bool _unlockUpgradesShown = false;
		public bool _unlockTransmutationShown = false;
		
		private int _combatsFought;
		public int combatsFought
		{
			get
			{
				return _combatsFought;
			}
			set
			{
				_combatsFought = value;
				
				if(GameManager.enableAnalytics)
					Analytics.logEvent(Analytics.CurrencyTrackType.TrackCombats, _combatsFought);
			}
		}
		
		
		private int _combatsLost;
		public int combatsLost
		{
			get
			{
				return _combatsLost;
			}
			set
			{
				_combatsLost = value;
				
				if(GameManager.enableAnalytics)
					Analytics.logEvent(Analytics.CurrencyTrackType.TrackGameOver, _combatsLost);
			}
		}
		
		
		public int dayCount;

		public int nexDefeated;

		
		public IDictionary  bossAttemptDictionary;
		
		public void BossDefeated(string enemyId)
		{
			if(!bossAttemptDictionary.Contains(enemyId) || Int32.Parse(bossAttemptDictionary[enemyId].ToString()) == 0)
				bossAttemptDictionary[enemyId] = 1;

				
			bool increment = false;
			
			if(enemyId.Equals("PRIMUS_NEX"))
				increment = true;
			else
			{
				AIModel bossModel;
				bossModel = GameManager._dataBank.GetModelForMonsterByEncounter(enemyId, 1 + Int32.Parse(bossAttemptDictionary[enemyId].ToString()));
				
				AIModel nexModel;
				if(Int32.Parse(bossAttemptDictionary["PRIMUS_NEX"].ToString()) < 1)
					nexModel = GameManager._dataBank.GetModelForMonsterByEncounter("PRIMUS_NEX", Int32.Parse(bossAttemptDictionary["PRIMUS_NEX"].ToString())+1);
				else
					nexModel = GameManager._dataBank.GetModelForMonsterByEncounter("PRIMUS_NEX", Int32.Parse(bossAttemptDictionary["PRIMUS_NEX"].ToString()));
				
				if(GameManager.PRINT_LOGS) Debug.Log("bossModel.skullLevel -->>> " + bossModel.skullLevel + " ::::::::::: nexModel.skullLevel -->>> " + nexModel.skullLevel);
				if(bossModel.skullLevel < nexModel.skullLevel)
					increment = true;
				
				if(skullLevel > (nexModel.skullLevel - 1))
					skullLevel=nexModel.skullLevel;
			}
			
			if(increment)
				bossAttemptDictionary[enemyId] = System.Int32.Parse(bossAttemptDictionary[enemyId].ToString()) +1;
		}
		
		public long version;
		public bool isMute = false;
		public float musicVolume = 1.0f;
		public float gfxVolume = 1.0f;
		public int ringShards;
		public Dictionary<string, object> tutorialStatusDictionary = null;

	
		public static void UnlockGame(GameState gameState)
		{
			gameState.dayCount = 4;
			gameState.skullLevel = 4;
			
			gameState.User._inventory.souls = 99999;
			gameState.User._inventory.gems = 99999;
			
			gameState.ringShards = 1;
			gameState.version+=10;
			
			GameManager.instance.SaveGameState(false);
		}
	}
}
