using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameStateModule;
using MiniJSON;
using InventorySystem;
using Newtonsoft.Json;

public class GameStateParser
{
	public  delegate void GameStateReceiveDelegate (GameState gameState);
	//static System.DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
	public static string ConvertGameStateToJson (GameState gState)
	{
		string jsonGameState = "{" +
			"\"User\": " + gState.User.json + "," +
			"\"Level\":" + Json.Serialize (gState.LevelState.ToDictionary ()) +
			",\"DisplayedCutscenes\":" + Json.Serialize (gState.DisplayedCutscenes) +
			",\"ShrineList\":" + Json.Serialize (ShrineManager.Instance.ToDictionary ()) +
			",\"TutorialStatus\":" + Json.Serialize (TutorialManager.instance.tutorialStatusDictionary) +
			",\"skull_level\":" + gState.skullLevel + ",\"day_count\":" + gState.dayCount +
			",\"nex_defeated\":" + gState.nexDefeated + 
			",\"combatsFought\":" + gState.combatsFought + ",\"combatsLost\":" + gState.combatsLost;
		if (gState._unlockRunesShown)
			jsonGameState += ",\"_unlockRunesShown\":" + "true" + ",";
		else
			jsonGameState += ",\"_unlockRunesShown\":" + "false" + ",";
		
		if (gState._unlockUpgradesShown)
			jsonGameState += "\"_unlockUpgradesShown\":" + "true" + ",";
		else
			jsonGameState += "\"_unlockUpgradesShown\":" + "false" + ",";
				
		if (gState._unlockSpiritsShown)
			jsonGameState += "\"_unlockSpiritsShown\":" + "true" + ",";
		else
			jsonGameState += "\"_unlockSpiritsShown\":" + "false" + ",";
				
		if (gState._unlockTransmutationShown)
			jsonGameState += "\"_unlockTransmutationShown\":" + "true" + ",";
		else
			jsonGameState += "\"_unlockTransmutationShown\":" + "false" + ",";
				 
				
		if (gState.firstEnemyDefeated)
			jsonGameState += "\"FirstEnemyDefeated\":" + "true" + ",";
		else
			jsonGameState += "\"FirstEnemyDefeated\":" + "false" + ",";
		
		if (gState.isMute)
			jsonGameState += "\"" + FIELD_IS_MUTE + "\"" + " : true,";
		else
			jsonGameState += "\"" + FIELD_IS_MUTE + "\"" + " : false,"; 
		jsonGameState += "\"" + FIELD_MUSIC_VOLUME + "\"" + " : " + gState.musicVolume + ","; 
		jsonGameState += "\"" + FIELD_GFX_VOLUME + "\"" + " : " + gState.gfxVolume + ",";
		jsonGameState += "\"" + FIELD_RING_FIELD_RING_SHARDS + "\"" + " : " + gState.ringShards + ",";
		jsonGameState += "\"" + RUNE_PROGRESS + "\" : " + GameManager.instance.runeProgress + ",";
		jsonGameState += "\"" + SPIRIT_PROGRESS + "\" : " + GameManager.instance.spiritProgress + ",";

		jsonGameState += "\"" + TAG_BOSS_ATTEMPTS + "\"" + " : " + Json.Serialize (gState.bossAttemptDictionary) + ","; 
		 
		jsonGameState += "\"version\" : " + gState.version + "}";
				
		if (GameManager.PRINT_LOGS)
			Debug.Log ("jsonGameState ::::::::::::: " + jsonGameState);
//		
		GameStateWriter gWriter = new GameStateWriter ();

		gWriter.SaveJson ("CurrentGameState.txt", jsonGameState);
		
		return jsonGameState;
	}
	private const string TAG_USER = "User";
	private const string TAG_BOSS_ATTEMPTS = "BossAttempts";
	private const string FIELD_ID = "id";
	private const string FIELD_NAME = "name";
	//private const string FIELD_WIZARD_TYPE = "wizard_type";
	//private const string FIELD_EQUIPPED_ITEM_ID = "equiped_item_id";
	private const string FIELD_EQUIPPED_RINGS = "Equipped Rings";
	private const string FIELD_WARDS = "wards";
	private const string FIELD_STAFF = "Staff";
	private const string FIELD_SOULS = "Souls";
	private const string FIELD_GEMS = "Gems";
	private const string FIELD_BAG = "BAG";
	private const string FIELD_BAG_ITEMS = "Bag Items";
	private const string FIELD_KEY_RING = "KEY_RING";
	private const string FIELD_POTION_BELT = "Potion Belt";
	private const string FIELD_T_CUBE = "T_CUBE";
	private const string FIELD_POTIONS = "Potions";
	private const string FIELD_INVENTORY = "Inventory";
	private const string FIELD_USER_LIFE = "life";
	private const string FIELD_USER_MULTIPLAYER_LIFE = "multiplayerLife";
	private const string FIELD_USER_CURRENT_LIFE = "currentLife";
	private const string FIELD_USER_DAMAGE = "damage";
	private const string FIELD_USER_ARCANA = "arcanePoints";
	private const string FIELD_TRANSMUTATION_PROGRESS = "TransmutationProgress";
	private const string FIELD_USER_MODEL_NAME = "modelName";
	private const string FIELD_USER_USERNAME = "username";
	
	private const string FIELD_LEVEL = "Level";
	private const string FIELD_LEVEL_ID = "LevelID";
	private const string FIELD_LEVEL_ZONEID = "ZoneID";
	private const string FIELD_LEVEL_POILISTTAG = "PoiList";
	private const string FIELD_LEVEL_LOOTLIST = "LootList";
	private const string FIELD_LEVEL_SHRINELIST = "ShrineList";
	private const string FIELD_FIRST_ENEMY_DEFEATED = "FirstEnemyDefeated";
	private const string FIELD_LEVEL_ENEMYLIST = "EnemyList";
	private const string FIELD_SPIRIT = "SPIRIT";
	private const string FIELD_GUILD = "Guild";
	
	private const string FIELD_LEVEL_TUTORIALSTATUS = "TutorialStatus";
	private const string FIELD_SKULL_LEVEL = "skull_level";
	private const string FIELD_COMBATS_FOUGHT = "combatsFought";
	private const string FIELD_COMBATS_LOST = "combatsLost";
	private const string FIELD_DAY_COUNT = "day_count";
	private const string FIELD_IS_MUTE = "Mute";
	private const string FIELD_MUSIC_VOLUME = "Music_Volume";
	private const string FIELD_GFX_VOLUME = "GFX_Volume";
	private const string FIELD_RING_FIELD_RING_SHARDS = "RingShards";
	
	private const string RUNE_PROGRESS = "runeProgress";
	private const string SPIRIT_PROGRESS = "spiritProgress";

	//Converts a JSON string into User objects and shows a user out of it on the screen
	public static void ProcessGameState (GameManager.GameStateProcessDelegate del, string jsonString, GameState gState, string filename)
	{
		bool jsonValiditySuccess;
//		Debug.Log("ProcessGameState ::"+filename+"::: " + jsonString);

		IDictionary fullJson = Json.Deserialize (jsonString) as IDictionary;
		jsonValiditySuccess = CheckGameStateJsonValidation (fullJson);
		//GameManager.instance.CheckGameStateValidation(fullJson);
		if (jsonValiditySuccess) {
			/*
			 * Add code here - for keeping dictionary for the number of times a boss is defeated
			 */
			IDictionary bossAttemptsDictionary = fullJson [TAG_BOSS_ATTEMPTS] as IDictionary;
			gState.bossAttemptDictionary = bossAttemptsDictionary;
			
			
			/* 
			 */
			IDictionary jsonUsers = fullJson [TAG_USER] as IDictionary;
			
			IDictionary jsonLevels = fullJson [FIELD_LEVEL] as IDictionary;
			//ShrineManager.Instance.LoadShrineState(fullJson[FIELD_LEVEL_SHRINELIST] as IList);
			gState.User._wards = Int32.Parse (jsonUsers [FIELD_WARDS].ToString ());
			gState.runeProgress = Int32.Parse (fullJson [RUNE_PROGRESS].ToString ());	
			
			IList petList = jsonUsers ["availablePets"] as IList;
			for (int i = 0; i<petList.Count; i++)
				gState.User.AddToAvailablePets (petList [i].ToString ());

			IDictionary petUpgradeDictionary = jsonUsers ["PetUpgrade"] as IDictionary;
			gState.User.petUpgradeDictionary = petUpgradeDictionary;
			
			string currentSpiritId = jsonUsers [FIELD_SPIRIT].ToString ();
			if (petUpgradeDictionary ["CurrentUpgrade"] == null)
				gState.User.spiritId = currentSpiritId;
			else if ((petUpgradeDictionary ["CurrentUpgrade"] as IDictionary) ["OldPetId"] == null) {
			} else if ((petUpgradeDictionary ["CurrentUpgrade"] as IDictionary) ["OldPetId"].ToString ().Equals (currentSpiritId)) {
				gState.User.spiritId = "";
			} else {
				gState.User.spiritId = currentSpiritId;
			}
			
			int userLife = Int32.Parse (jsonUsers [FIELD_USER_LIFE].ToString ());
			gState.User.defaultLife = userLife;
			
			if (jsonUsers.Contains (FIELD_USER_USERNAME)) {
				gState.User.username = jsonUsers [FIELD_USER_USERNAME].ToString ();
				gState.User.modelName = jsonUsers [FIELD_USER_MODEL_NAME].ToString ();
			}
			
			gState.firstEnemyDefeated = (bool)fullJson [FIELD_FIRST_ENEMY_DEFEATED];
			
			int userDamage = Int32.Parse (jsonUsers [FIELD_USER_DAMAGE].ToString ());
			
			gState.User.defaultDamage = userDamage;
			
			gState.User.arcanePoints = float.Parse (jsonUsers [FIELD_USER_ARCANA].ToString ());
			
			IDictionary inventoryDictionary = jsonUsers [FIELD_INVENTORY] as IDictionary;
			gState.User._inventory = InventoryLoader.LoadInventory (inventoryDictionary);
			gState.User.SetRingsAndPetStats ();
			
			gState.User.life = Int32.Parse (jsonUsers [FIELD_USER_CURRENT_LIFE].ToString ());
			
			int multiLife = Int32.Parse (jsonUsers [FIELD_USER_MULTIPLAYER_LIFE].ToString ());
			if (GameManager.PRINT_LOGS)
				Debug.Log ("multiplayerLife = " + multiLife);
			gState.User.multiplayerLife = multiLife;
			
			gState.LevelState.levelID = jsonLevels [FIELD_LEVEL_ID].ToString ();
			gState.LevelState.zoneID = jsonLevels [FIELD_LEVEL_ZONEID].ToString ();
			gState.LevelState.poiIsClearList = LevelStateLoader.LoadIsClearPoiList ((jsonLevels [FIELD_LEVEL_POILISTTAG] as IList));
			gState.LevelState.lootList = LevelStateLoader.LoadLootList (jsonLevels [FIELD_LEVEL_LOOTLIST] as IList);
			gState.LevelState.POIEnemyKeyVal = LevelStateLoader.LoadPreviousInstantiatedEnemyList (jsonLevels [FIELD_LEVEL_ENEMYLIST] as IList);
			
			gState.skullLevel = Int32.Parse (fullJson [FIELD_SKULL_LEVEL].ToString ());
			gState.combatsFought = Int32.Parse (fullJson [FIELD_COMBATS_FOUGHT].ToString ());
			gState.combatsLost = Int32.Parse (fullJson [FIELD_COMBATS_LOST].ToString ());
			
			gState.dayCount = Int32.Parse (fullJson [FIELD_DAY_COUNT].ToString ());
			gState.nexDefeated = Int32.Parse (fullJson ["nex_defeated"].ToString ());

			long version = Int64.Parse (fullJson ["version"].ToString ());
			if (GameManager._gameState != null && GameManager._gameState.version > version) {
				version = GameManager._gameState.version;
			}
			gState.version = version;
			gState.ringShards = System.Convert.ToInt32 (fullJson ["RingShards"]);
			gState.isMute = System.Convert.ToBoolean (fullJson [FIELD_IS_MUTE]);
			//gState.isMute = Boolean.Parse(fullJson[FIELD_IS_MUTE].ToString());
			gState.musicVolume = (float)System.Convert.ToDouble (fullJson [FIELD_MUSIC_VOLUME]);
			gState.gfxVolume = (float)System.Convert.ToDouble (fullJson [FIELD_GFX_VOLUME]);
			
			gState.User.transmutationDictionary = jsonUsers ["TransmutationProgress"] as IDictionary;
			//		if(GameManager.PRINT_LOGS) Debug.Log("MiniJSON.Json.Serialize(gState.User.transmutationDictionary) >>>>>>>>>>>> "+ MiniJSON.Json.Serialize(gState.User.transmutationDictionary));
			
			IDictionary upgradesDictionary = inventoryDictionary ["Upgrades"] as IDictionary;
			gState.User._inventory.upgradesDictionary = upgradesDictionary;
	
			List<object> displayed = ((List<object>)fullJson ["DisplayedCutscenes"]);
			if (displayed != null) {
				foreach (object obj in displayed)
					gState.DisplayedCutscenes.Add (obj.ToString ());
			}
			
			gState._unlockRunesShown = bool.Parse (fullJson ["_unlockRunesShown"].ToString ());
			gState._unlockUpgradesShown = bool.Parse (fullJson ["_unlockUpgradesShown"].ToString ());
			gState._unlockSpiritsShown = bool.Parse (fullJson ["_unlockSpiritsShown"].ToString ());
			gState._unlockTransmutationShown = bool.Parse (fullJson ["_unlockTransmutationShown"].ToString ());
			gState.tutorialStatusDictionary = fullJson ["TutorialStatus"] as Dictionary<string, object>;
			gState.shrineList = fullJson [FIELD_LEVEL_SHRINELIST] as IList;
			del (gState, filename, true);
		} else {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("=============> GAME STATE IS NOT VALID !! DEBUG THE ERROR IN " + filename + "~~~~~~~~~~~~~~~~");
			del (gState, filename, false);
			// Replace current game state with last game state
		}
	}
	
	public static bool CheckGameStateJsonValidation (IDictionary json)
	{
		bool isGameStateJsonValid = true;
		
		#region User TAG Validation
		if (json.Contains (TAG_USER)) {
			IDictionary user = json [TAG_USER] as IDictionary;
			
			if (!user.Contains (FIELD_USER_LIFE)) {
				isGameStateJsonValid = false;
			} else {
				if (Convert.ToInt32 (user [FIELD_USER_LIFE]) <= 0) {
					isGameStateJsonValid = false;
				}
					
			}
			if (!user.Contains (FIELD_USER_CURRENT_LIFE)) {
				isGameStateJsonValid = false;
			}
			
			if (!user.Contains (FIELD_USER_DAMAGE)) {
				isGameStateJsonValid = false;
			} else {
				if (Convert.ToInt32 (user [FIELD_USER_DAMAGE]) <= 0) {
					isGameStateJsonValid = false;
				}
			}
			if (!user.Contains (FIELD_WARDS)) {
				isGameStateJsonValid = false;
			}
//			if(!user.Contains(FIELD_TRANSMUTATION_PROGRESS))
//			{
//				isGameStateJsonValid=false;
//			}
			
			#region Inventory TAG validation
			if (user.Contains (FIELD_INVENTORY)) {
				IDictionary inventory = user [FIELD_INVENTORY] as IDictionary;
				
				if (!inventory.Contains (FIELD_BAG_ITEMS)) {
					isGameStateJsonValid = false;
				}
				
				if (!inventory.Contains (FIELD_EQUIPPED_RINGS)) {
					isGameStateJsonValid = false;
				}
				
				if (!inventory.Contains (FIELD_STAFF)) {
					isGameStateJsonValid = false;
				}
				
				if (!inventory.Contains (FIELD_SOULS)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_GEMS)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_BAG)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_KEY_RING)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_POTION_BELT)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_T_CUBE)) {
					isGameStateJsonValid = false;
				}
				if (!inventory.Contains (FIELD_POTIONS)) {
					isGameStateJsonValid = false;
				}
				
				
			} else {
				isGameStateJsonValid = false;
			}
			#endregion
			
			
		} else {
			isGameStateJsonValid = false;
		}
		#endregion
		
		#region Level TAG Validation
		if (json.Contains (FIELD_LEVEL)) {
			IDictionary levels = json [FIELD_LEVEL] as IDictionary;
			
			if (!levels.Contains (FIELD_LEVEL_ID)) {
				isGameStateJsonValid = false;
			}
			
			if (!levels.Contains (FIELD_LEVEL_ZONEID)) {
				isGameStateJsonValid = false;
			}
			
			if (!levels.Contains (FIELD_LEVEL_LOOTLIST)) {
				isGameStateJsonValid = false;
			}
			
			if (!levels.Contains (FIELD_LEVEL_ENEMYLIST)) {
				isGameStateJsonValid = false;
			} else {
				IList enemyList = levels [FIELD_LEVEL_ENEMYLIST] as IList;
				if (!levels [FIELD_LEVEL_ID].ToString ().Equals ("BeachCamp")) {
					if (enemyList.Count < 1) {
						if (!levels [FIELD_LEVEL_ID].ToString ().Equals ("ArcanumRuhalis"))
							isGameStateJsonValid = false;
					}
				}
			}
		} else {
			isGameStateJsonValid = false;
		}
		#endregion
		
		if (!json.Contains (FIELD_LEVEL_TUTORIALSTATUS)) {
			isGameStateJsonValid = false;
		}
		
		if (!json.Contains (FIELD_SKULL_LEVEL)) {
			isGameStateJsonValid = false;
		}
		
		
		return isGameStateJsonValid;
	}

}


