using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

public class AILoader : MonoBehaviour{

//	public GameObject ogreShamanPrefab;
//	
//	public GameObject ogreWarlockPrefab;
//	
//	public GameObject ogreWarlordPrefab;
//	
//	public GameObject ogreBattlemagePrefab;
//	
//	public GameObject humanAcolytePrefab;
//	
//	public GameObject humanMagickerPrefab;
//	
//	public GameObject primusNexPrefab;
//	
//	public GameObject fiendPrefab;
	
	public List<string> enemyNameList;
	
	string temporaryKey;
	
	bool isOldEnemey;
	
	AssetBundleLoader AIAssetBundleLoader;

	public delegate void MonsterModelDelegate (GameObject aiGameObject, Vector3 atPosition,Quaternion atAngle, AIModel aiModel,int index);
	
	void Start(){
		AIAssetBundleLoader=this.GetComponent<AssetBundleLoader>();
	    AIAssetBundleLoader.SetDelegate(this.onAssetBundleRecieved);
	}
	
	 public void onAssetBundleRecieved(AssetBundle bundle)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("Prefab Bundle's Asset Name: " + bundle.mainAsset.name);
		GameObject aiGameObject = bundle.mainAsset as GameObject;
		if(GameManager.PRINT_LOGS) Debug.Log("onAssetBundleRecieved" + aiModel.skullLevel);
		if(GameManager.PRINT_LOGS) Debug.Log("AIBundler Unload ---- " + GetPrefabForModelName(aiModel.name));
		tempDel(aiGameObject,tempPoint.position,tempPoint.rotation, aiModel,tempIndex);
		
		//AIAssetBundleLoader.UnloadAssetBundleWithoutObject(GetPrefabForModelName(aiModel.name));
		
	}
	
//	public void OnEnemyListForZoneRecieved(List<string> enemyList)
//	{
//		enemyNameList=new List<string>();
//		
//		enemyNameList=enemyList;
//	}
	private MonsterModelDelegate tempDel;
	Transform tempPoint;
	int tempIndex;
	AIModel aiModel;
	public void LoadAI(string currentLevel, MonsterModelDelegate del,Transform point,int index)
	{
		if(GameManager.PRINT_LOGS) Debug.Log(" LoadAI(string currentLevel, MonsterModelDelegate del,Transform point,int index)");
		tempDel = del;

		decideEnemy(enemyNameList,currentLevel);

		if(GameManager.PRINT_LOGS) Debug.Log("Call to load BOSS from TEMPORARY KEY" + temporaryKey);
		
		aiModel = new AIModel();
		if(GameManager._gameState.bossAttemptDictionary.Contains(temporaryKey) &&  Int32.Parse(GameManager._gameState.bossAttemptDictionary[temporaryKey].ToString()) == 0)
			GameManager._gameState.bossAttemptDictionary[temporaryKey] = 1;
		
		if(GameManager._gameState.bossAttemptDictionary.Contains(temporaryKey))
			aiModel = GameManager._dataBank.GetModelForMonsterByEncounter(temporaryKey, Int32.Parse(GameManager._gameState.bossAttemptDictionary[temporaryKey].ToString()));
		else
			aiModel = GameManager._dataBank.GetModelForMonster(temporaryKey, GameManager._gameState.skullLevel);

		AIAssetBundleLoader.InvokeLoadPrefabAssetBundle(GetPrefabForModelName(aiModel.name));
		
		tempPoint = point;
		tempIndex = index;
		
	}
	
	public void LoadAIWithModel(string currentLevel, MonsterModelDelegate del,Transform point,int index, AIModel aModel)
	{
		if(GameManager.PRINT_LOGS) Debug.Log(" LoadAIWithModel(string currentLevel, MonsterModelDelegate del,Transform point,int index, AIModel aModel)");
		
		this.aiModel = aModel;
		
		if(GameManager.PRINT_LOGS) Debug.Log("name = " + this.aiModel.name + "  this.aiModel.skullLevel" + this.aiModel.skullLevel);
		tempDel = del;

		AIAssetBundleLoader.InvokeLoadPrefabAssetBundle(GetPrefabForModelName(aiModel.name));
		
		tempPoint = point;
		tempIndex = index;
		
	}
	
	public static string GetPrefabForModelName(string aiName)
	{
		if(aiName.Equals(AIModel.NameTypes.OGRE_SHAMAN.ToString()))
		{
			return "PREFAB_OGRE_SHAMAN";
		}
		else if(aiName.Equals(AIModel.NameTypes.OGRE_WARLOCK.ToString()))
		{
			return "PREFAB_OGRE_WARLOCK";
		}
		else if(aiName.Equals(AIModel.NameTypes.OGRE_WARLORD.ToString()))
		{
			return "PREFAB_OGRE_WARLORD";
		}
		else if(aiName.Equals(AIModel.NameTypes.HUMAN_ACOLYTE.ToString()))
		{
			return "PREFAB_HUMAN_ACOLYTE";
		}
		else if(aiName.Equals(AIModel.NameTypes.HUMAN_MAGICKER.ToString()))
		{
			return "PREFAB_HUMAN_MAGICKER";
		}
		else if(aiName.Equals(AIModel.NameTypes.OGRE_BATTLEMAGE.ToString()))
		{
			return "PREFAB_OGRE_BATTLEMAGE";
		}
		else if(aiName.Equals(AIModel.NameTypes.PRIMUS_NEX.ToString()))
		{
			return "PREFAB_NEX";
		}
		else if(aiName.Equals(AIModel.NameTypes.PIT_FIEND.ToString()))
		{
			return "PREFAB_FIEND";
		}
		else if(aiName.Equals(AIModel.NameTypes.VALKYRIE.ToString()))
		{
			return "PREFAB_VALKYRIE";
		} 
		else if(aiName.Equals(AIModel.NameTypes.Jaghdarr.ToString()))
		{
			return "PREFAB_OGRE_WARLORD";
		}
		else
		{
			throw new System.Exception("Gameobject not found! Make sure the model names match those of prefabs");
		}
	}
	
	public void LoadAI(string currrentLevel, MonsterModelDelegate del,Transform point,int index,string poi_id)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("LoadAI(string currrentLevel, MonsterModelDelegate del,Transform point,int index,string poi_id))");
		tempDel = del;
		temporaryKey = GameManager.instance.getEnemyFromGameState(poi_id);
		
		aiModel = new AIModel();
		
		if(GameManager._gameState.bossAttemptDictionary.Contains(temporaryKey) && Int32.Parse(GameManager._gameState.bossAttemptDictionary[temporaryKey].ToString()) == 0)
			GameManager._gameState.bossAttemptDictionary[temporaryKey] = 1;
		
		if(GameManager._gameState.bossAttemptDictionary.Contains(temporaryKey))
			aiModel = GameManager._dataBank.GetModelForMonsterByEncounter(temporaryKey, Int32.Parse(GameManager._gameState.bossAttemptDictionary[temporaryKey].ToString()));
		else
			aiModel = GameManager._dataBank.GetModelForMonster(temporaryKey, GameManager._gameState.skullLevel);
			
		AIAssetBundleLoader.InvokeLoadPrefabAssetBundle(GetPrefabForModelName(aiModel.name));
		
		tempPoint = point;
		tempIndex = index;
	}

	public static Dictionary<KeyValuePair<string, int>, AIModel> LoadMonsterData(IList monstersList, out Dictionary<string, int> monsterToMaxLevelOUTDictionary, out Dictionary<string, Dictionary<int, int>> encounterToSkullOUTDictionary)
	{
		Dictionary<KeyValuePair<string, int>, AIModel> monstersModelDictionary = new Dictionary<KeyValuePair<string, int>, AIModel>();
		
		Dictionary<string, Dictionary<int, int>> encounterToSkullDictionary = new Dictionary<string, Dictionary<int, int>>();
			
		Dictionary<string, int>  monsterToMaxLevelDictionary =  new Dictionary<string, int>();
		AIModel aiModel = null;
		
		for(int i = 0;i<monstersList.Count;i++)
		{
			#region val setting
			IDictionary monsterDictionary = monstersList[i] as IDictionary;
			aiModel = new AIModel();
			aiModel = GetAIModelFromDictionary(monsterDictionary, aiModel);
			if(!encounterToSkullDictionary.ContainsKey(aiModel.name))
			{
				 encounterToSkullDictionary[aiModel.name] = new Dictionary<int, int>();
			}
			
			#endregion
//			if(monsterDictionary.Contains("Encounter"))
			if(monsterDictionary.Contains("Encounter"))
			{
				int encounter = Convert.ToInt32(monsterDictionary["Encounter"].ToString());
				(encounterToSkullDictionary[aiModel.name])[encounter] = aiModel.skullLevel;
			}
			
			monstersModelDictionary.Add(new KeyValuePair<string, int>(aiModel.name, aiModel.skullLevel), aiModel);
			
			if(monsterToMaxLevelDictionary.ContainsKey(aiModel.name))
			{
				if(monsterToMaxLevelDictionary[aiModel.name] < aiModel.skullLevel)
					monsterToMaxLevelDictionary[aiModel.name] = aiModel.skullLevel;
			}
			else
			{
				monsterToMaxLevelDictionary[aiModel.name] = aiModel.skullLevel;
			}
		}
		
		monsterToMaxLevelOUTDictionary = monsterToMaxLevelDictionary;
		encounterToSkullOUTDictionary = encounterToSkullDictionary;
		return monstersModelDictionary;
	}
	
	
	public static AIModel GetAIModelFromDictionary(IDictionary monsterDictionary, AIModel aiModel)
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("MONSTER DICTIONARY --->>> " + MiniJSON.Json.Serialize(monsterDictionary));
		if(monsterDictionary.Contains(AIConstants.MODEL_NAME))
			aiModel.name = monsterDictionary[AIConstants.MODEL_NAME].ToString();
		aiModel.modelName = aiModel.name;
		if(monsterDictionary.Contains(AIConstants.MODEL_SKULL_LEVEL))
			aiModel.skullLevel = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_SKULL_LEVEL].ToString());
		aiModel.fire = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_FIRE].ToString());
		aiModel.water = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_WATER].ToString());
		aiModel.earth = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_EARTH].ToString());
		aiModel.lightning = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_LIGHTNING].ToString());

		string speedVal = monsterDictionary[AIConstants.MODEL_SPEED].ToString();
		
		if(speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_SLOW))
			aiModel.speed = AIModel.Speed.SLOW;
		else if(speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_MEDIUM))
			aiModel.speed = AIModel.Speed.MEDIUM;
		else if(speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_FAST))
			aiModel.speed = AIModel.Speed.FAST;
		else if(speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_HASTE))
			aiModel.speed = AIModel.Speed.HASTE;
		else
			aiModel.speed = AIModel.Speed.NOT_AVAILABLE;
		
		string raceVal = monsterDictionary[AIConstants.MODEL_RACE].ToString();
		
		if(raceVal.Equals(AIConstants.FIELD_MODEL_RACE_OGRE))
			aiModel.race = AIModel.Race.OGRE;
		else if(raceVal.Equals(AIConstants.FIELD_MODEL_RACE_HUMAN))
			aiModel.race = AIModel.Race.HUMAN;
		else if(raceVal.Equals(AIConstants.FIELD_MODEL_RACE_GOLEM))
			aiModel.race = AIModel.Race.GOLEM;
		
		string firstSpell = monsterDictionary[AIConstants.MODEL_SPELL_ONE].ToString();
		string secondSpell = monsterDictionary[AIConstants.MODEL_SPELL_TWO].ToString();
		string thirdSpell = monsterDictionary[AIConstants.MODEL_SPELL_THREE].ToString();
		
		if(!firstSpell.Equals("NONE"))
			aiModel.availableBuffs.Add(firstSpell);
//		if(GameManager.PRINT_LOGS) Debug.Log("aiModel.availableBuffs::::::::::: firstSpell::::::::::::::" + firstSpell);
		if(!secondSpell.Equals("NONE"))
			aiModel.availableBuffs.Add(secondSpell);
		
		if(!thirdSpell.Equals("NONE"))
			aiModel.availableBuffs.Add(thirdSpell);
		
		
		aiModel.buffTime = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_BUFF_TIME].ToString());
		aiModel.totalLife = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_LIFE].ToString());
		aiModel.life = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_LIFE].ToString());
		aiModel.damage = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_DAMAGE].ToString());
		aiModel.hitsToStun = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_HITS_TO_STUN].ToString());
		aiModel.lifeToStun = Convert.ToInt32(monsterDictionary[AIConstants.MODEL_LIFE_TO_STUN].ToString());
		
		return aiModel;
	}
	
	
	public void decideEnemy(List<string> enemyList,string currentLevel) //tempraryFunction as no zone base percentage available
	{
		if(GameManager._gameState.skullLevel==1)
		{
			switch(currentLevel)
			{
			case "ToHollowTree":
			temporaryKey="OGRE_WARLOCK";
			break;
			case "HollowTree":
			temporaryKey="HUMAN_ACOLYTE";
			break;
			case "StatuePath":
			temporaryKey="OGRE_BATTLEMAGE"; //BATTLE_MAGE needs to come here
			break;
			case "ThreeGods":
			temporaryKey="OGRE_WARLORD";
			break;
			case "ToCaveEntrance":
			temporaryKey="HUMAN_ACOLYTE";
			break;
			case "CaveEntrance":
			temporaryKey="OGRE_SHAMAN";
			break;
			case "ForbiddenCave":
			temporaryKey="PIT_FIEND"; //FIEND needs to come here
			break;
			case "DemonCanyon":
			temporaryKey="HUMAN_ACOLYTE";
			break;
			case "ToDemonFalls":
			temporaryKey="HUMAN_MAGICKER";
			break;
			case "DemonFalls":
			temporaryKey="PRIMUS_NEX";
			break;
				
			}
		}
		else
		{
			int rand = new System.Random().Next(0,enemyList.Count);
			
			temporaryKey = enemyList[rand];
		}
	}
}
