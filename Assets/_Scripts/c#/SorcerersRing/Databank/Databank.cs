using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace DatabankSystem
{
	public class Databank
	{
		public const int NUMBER_OF_LEVELS = 35;
		
		private const int MAX_ATTEMPTS = 10;
		public string version = "";
		public int gamestateVersion;
		public InApps inApps;
		public Dictionary<KeyValuePair<string, int>, IDictionary> buffDictionary;
		public Dictionary<string,Level>Levels;
		public Dictionary<string,POISet> Pois;
		public Dictionary<string,ZoneModel> Zones;
		public Dictionary<int, int> skullToStatDictionary;
		public Dictionary<int, float> skullToUTimeDictionary;
		public Dictionary<int, int> statToSkullDictionary;
		public Dictionary<int, string> skullToRingTierDictionary;
		public Dictionary<int, int> skullToDustRequiredDictionary;
		public Dictionary<int, int> skullTogCostDictionary;
		public Dictionary<int, int> skullTotCostDictionary;
		public Dictionary<int, int> skullTobCostDictionary;
		public Dictionary<int, int> skullToBattleSoulLootDictionary;
		public Dictionary<int, int> skullToChestSoulLootDictionary;
		public Dictionary<int, int> skullToChestGemsLootDictionary;
		public Dictionary<int, int> skullTotLootPercentageDictionary;
		public Dictionary<string,int> keysToSkullModDictionary;
		public Dictionary<string,int> keysToSoulsPercentageDictionary;
		public Dictionary<string,int> keysToRingsPercentageDictionary;
		public Dictionary<string,int> keysToRunesPercentageDictionary;
		public Dictionary<string,int> keysToPremiumRingsPercentageDictionary;
		public Dictionary<string,int> keysToPremiumRunesPercentageDictionary;
		public Dictionary<KeyValuePair<string, int>, AIModel> monstersDictionary;
		public Dictionary<string, InventorySystem.PotionBelt> potionBeltDictionary;
		public Dictionary<string, InventorySystem.TransmutationCube> transmutationCubeDictionary;
		public Dictionary<string, InventorySystem.Bag> bagDictionary;
		public Dictionary<string, InventorySystem.ItemPotion> potionsDictionary;
		public Dictionary<string, InventorySystem.KeyRing> keyRingDictionary;
		public List<InventorySystem.Bag> sortedBagList;
		public List<InventorySystem.TransmutationCube> sortedTCubeList;
		public List<InventorySystem.KeyRing> sortedKeyRingList;
		public List<InventorySystem.PotionBelt> sortedPotionBeltList;
		public Dictionary<string, IDictionary> spiritsDictionary;
		public Dictionary<string, int> aiToMaxLevelDictionary;
		public Dictionary<string, Dictionary<int, int>> encounterToSkullLevelDictionary;		
		public Dictionary<int,ShrineInfo> ShrineDataDictionary;
		public Dictionary<string ,string> creditsDictionary;

		#region pet upgrade data etc
		public List<IntValPair<IDictionary>> sortedImpList;
		public List<IntValPair<IDictionary>> sortedAquaList;
		public List<IntValPair<IDictionary>> sortedGolemList;
		public List<IntValPair<IDictionary>> sortedDrakeList;
		
		
		public string getComaSeparatedRingNames()
		{
			string names="";
			for(int i=0;i<premiumRingsList.Count;i++)
			{
				names +=premiumRingsList[i].ItemName()+",";
			}
			
			for(int i=0;i<nonpremiumRingsList.Count;i++)
			{
				names += nonpremiumRingsList[i].ItemName();
				if(i<nonpremiumRingsList.Count-1)
				{
					names +=",";
				}
			}
			
			if(GameManager.PRINT_LOGS) Debug.Log("RingNameList: " + names);
			return names;
		}
		
		public string getComaSeparatedRuneNames()
		{
			string names="";
			IDictionary dictionary =null;
			for(int i=0;i<premiumRunesList .Count;i++)
			{
				dictionary= premiumRunesList[i] as IDictionary;
				names +=dictionary["RuneName"].ToString()+",";
			}
			
			for(int i=0;i<nonpremiumRunesList .Count;i++)
			{
				dictionary= nonpremiumRunesList[i] as IDictionary;
				names += dictionary["RuneName"].ToString();
				if(i<nonpremiumRunesList.Count-1)
				{
					names +=",";
				}
			}
			
			if(GameManager.PRINT_LOGS) Debug.Log("RuneNameList: " + names);
			return names;
		}
		
		public IDictionary GetImpForLevel(int level)
		{
			int count = sortedImpList.Count;
			IntValPair<IDictionary> impValDic = null;
			for(int i = 0;i<count;i++)
			{
				impValDic = sortedImpList[i];
				if(impValDic.intKey == level)
					return impValDic.val;
			}
			return null;
		}
		
		public IDictionary GetGolemForLevel(int level)
		{
			int count = sortedGolemList.Count;
			IntValPair<IDictionary> golemValDic = null;
			for(int i = 0;i<count;i++)
			{
				golemValDic = sortedGolemList[i];
				if(golemValDic.intKey == level)
					return golemValDic.val;
			}
			
			return null;
		}
		
		public IDictionary GetAquaForLevel(int level)
		{
			int count = sortedAquaList.Count;
			IntValPair<IDictionary> wispValDic = null;
			for(int i = 0;i<count;i++)
			{
				wispValDic = sortedAquaList[i];
				if(wispValDic.intKey == level)
					return wispValDic.val;
			}
			return null;
		}
		
		public IDictionary GetDrakeForLevel(int level)
		{
			IntValPair<IDictionary> drakeValDic = null;
			int count = sortedDrakeList.Count;
			for(int i = 0;i<count;i++)
			{
				drakeValDic = sortedDrakeList[i];
				if(drakeValDic.intKey == level)
					return drakeValDic.val;
			}
			return null;
		}
		
		public IDictionary GetMaxLevelImp()
		{
			return sortedImpList[sortedImpList.Count - 1].val;
		}
		
		public IDictionary GetMaxLevelGolem()
		{
			return sortedGolemList[sortedGolemList.Count - 1].val;
		}
		
		public IDictionary GetMaxLevelAqua()
		{
			return sortedAquaList[sortedAquaList.Count - 1].val;
		}
		
		public IDictionary GetMaxLevelDrake()
		{
			return sortedDrakeList[sortedDrakeList.Count - 1].val;
		}
		#endregion
		
		public IDictionary GetSpiritDictionaryForID(string spiritId)
		{
			return spiritsDictionary[spiritId];
		}
		
		public InventorySystem.PotionBelt GetMaxLevelPotionBelt()
		{
			return sortedPotionBeltList[sortedPotionBeltList.Count - 1];
		}
		
		public InventorySystem.KeyRing GetMaxLevelKeyRing()
		{
			return sortedKeyRingList[sortedKeyRingList.Count - 1];
		}
		
		public InventorySystem.Bag GetMaxLevelBag()
		{
			return sortedBagList[sortedBagList.Count - 1];
		}
		
		public InventorySystem.TransmutationCube GetMaxLevelTCube()
		{
			return sortedTCubeList[sortedTCubeList.Count - 1];
		}
		
		#region Runes Data
		private static Dictionary<int, List<string>> _skullToRuneNameListDictionary;
		public static Dictionary<int, List<string>> skullToRuneNameListDictionary
		{
			private set
			{
				
			}
			get
			{
				if(_skullToRuneNameListDictionary == null)
				{
					_skullToRuneNameListDictionary = new Dictionary<int, List<string>>();
				
					for(int i = 1;i<=NUMBER_OF_LEVELS;i++)
					{
						_skullToRuneNameListDictionary.Add(i, new List<string>());
					}
				}	
				
				return _skullToRuneNameListDictionary;
			}
		}
			
		public bool HasStatToSkullValue(int statValue)
		{
			if(statToSkullDictionary.ContainsKey(statValue))
				return true;
			return false;
		}
		
		public float GetUTimeForSkullLevel(int skullLevel)
		{
//			if(GameManager.PRINT_LOGS) Debug.Log("SKULL LEVEL LOOKUP agaINST ::::::::::::::: " + skullLevel);
			return skullToUTimeDictionary[skullLevel];
		}
		public Dictionary<KeyValuePair<string, int>, IDictionary> runeDictionary;
		
		public IDictionary GetRuneForRuneID(string runeID, int skullLevel)
		{
//			if(GameManager.PRINT_LOGS) Debug.Log("runeID===="+runeID+" skullLevel===="+skullLevel);
			return runeDictionary[new KeyValuePair<string, int>(runeID, skullLevel)];
		}
		
		public static InventorySystem.ItemRune GetRandomRuneForSkullLevel(Databank dbBank, int skullLevel)
		{
			int randomIndex = new System.Random().Next(0,skullToRuneNameListDictionary[skullLevel].Count);
			
			InventorySystem.ItemRune randomRune = new InventorySystem.ItemRune(skullToRuneNameListDictionary[skullLevel][randomIndex], skullLevel);
			
			return randomRune;
		}
		
		public static List<InventorySystem.ItemRune> GetRandomRunesForSkullLevel(Databank dbBank, int skullLevel, int howMany)
		{
			if(howMany > skullToRuneNameListDictionary[skullLevel].Count)
				throw new System.Exception("Not that many Runes found!");
			
			List<InventorySystem.ItemRune> randomRunesList = new List<InventorySystem.ItemRune>();
			
			bool found;
			while(randomRunesList.Count != howMany)
			{
				found = false;
				InventorySystem.ItemRune aRandomRune = GetRandomRuneForSkullLevel(dbBank, skullLevel);
				
				InventorySystem.ItemRune aRune = null;
				int count = randomRunesList.Count;
				for(int i = 0;i<count;i++)
				{
					aRune = randomRunesList[i];
					
					if(aRune.Equals(aRandomRune))
					{
						found = true;
						break;
					}
				}
				
				if(!found)
					randomRunesList.Add(aRandomRune);
			}
			
			return randomRunesList;
		}
		#endregion
		
		public IDictionary GetBuffForBuffID(string buffID, int skullLevel)
		{
			return buffDictionary[new KeyValuePair<string, int>(buffID, skullLevel)];
		}
		
		public ZoneModel GetZoneDetails(string zoneID)
		{
			return Zones[zoneID].Clone();
		}
		
		public POISet GetPoiDetials(string poiID)
		{
			return Pois[poiID].Clone();
		}
		
		public Level GetLevelDetails(string levelID)
		{
			return Levels[levelID].Clone();
		}
		public InventorySystem.ItemPotion GetPotionForPotionID(string potionId)
		{
			return potionsDictionary[potionId];
		}
		
		public InventorySystem.TransmutationCube GetTransmutationCubeForID(string cubeId)
		{
			return transmutationCubeDictionary[cubeId];
		}
		
		public InventorySystem.Bag GetBagForID(string bagId)
		{
			return bagDictionary[bagId].Clone();
		}
		
		public InventorySystem.PotionBelt GetPotionBeltForID(string pBeltId)
		{
			return potionBeltDictionary[pBeltId].Clone();
		}
		
		public InventorySystem.KeyRing GetKeyRingForID(string keyRingId)
		{
			return keyRingDictionary[keyRingId].Clone();
		}
		
		public InventorySystem.PotionBelt GetBeltForID(string beltId)
		{
			return potionBeltDictionary[beltId];
		}
			
		public AIModel GetModelForMonster(string monsterName, int skullLevel)
		{
//			if(GameManager.PRINT_LOGS) 
				Debug.LogError("MONSTER NAME = " + monsterName + " SKULL LEVEL " + skullLevel);
			if(skullLevel <= aiToMaxLevelDictionary[monsterName])
				return this.monstersDictionary[new KeyValuePair<string, int>(monsterName, skullLevel)].Clone();
			else
				return this.monstersDictionary[new KeyValuePair<string, int>(monsterName, aiToMaxLevelDictionary[monsterName])].Clone();
		}
		
		public AIModel GetModelForMonsterByEncounter(string monsterName, int encounter)
		{
			if(encounter == 0)
				encounter =1;
			if(GameManager.PRINT_LOGS) Debug.Log("monsterName" + monsterName + "encounter" + encounter);
//			Debug.LogError("monsterName" + monsterName + "encounter" + encounter);
			int skullLevel = (encounterToSkullLevelDictionary[monsterName])[Mathf.Min(encounter, 5)];
			if(skullLevel <= aiToMaxLevelDictionary[monsterName])
				return this.monstersDictionary[new KeyValuePair<string, int>(monsterName, skullLevel)].Clone();
			else
				return this.monstersDictionary[new KeyValuePair<string, int>(monsterName, aiToMaxLevelDictionary[monsterName])].Clone();
		}
		
		public int GetStatsForSkullLevel(int skullLevel)
		{
			if(skullToStatDictionary != null)
			{
				return skullToStatDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public string GetRingTierForSkullLevel(int skullLevel)
		{
			
			return skullToRingTierDictionary[skullLevel];		
			
		}
		
		public int GetDustRequiredForSkullLevel(int skullLevel)
		{
			if(skullToDustRequiredDictionary != null)
			{
				return skullToDustRequiredDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetBattleSoulLootForSkullLevel(int skullLevel)
		{
			if(skullToBattleSoulLootDictionary != null)
			{
				return skullToBattleSoulLootDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetChestSoulLootForSkullLevel(int skullLevel)
		{
			if(skullToChestSoulLootDictionary != null)
			{
				return skullToChestSoulLootDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetChestGemsLootForSkullLevel(int skullLevel)
		{
			if(skullToChestGemsLootDictionary != null)
			{
				return skullToChestGemsLootDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GettCostForSkullLevel(int skullLevel)
		{
			if(skullTotCostDictionary != null)
			{
				return skullTotCostDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetbCostForSkullLevel(int skullLevel)
		{
			if(skullTobCostDictionary != null)
			{
				return skullTobCostDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetgCostForSkullLevel(int skullLevel)
		{
			if(skullTogCostDictionary != null)
			{
				return skullTogCostDictionary[skullLevel];
			}
			
			return 0;
		}
		
		public int GetLootPercentageForSkullLevel(int skullLevel)
		{
			if(skullTotLootPercentageDictionary != null)
			{
				return skullTotLootPercentageDictionary[skullLevel];
			}
			
			return 0;
		}
		
		#region RINGS DATABANK...
		public List<InventorySystem.ItemRing> premiumRingsList;
		
		public List<InventorySystem.ItemRing> nonpremiumRingsList;
		
		public List<IDictionary> premiumRunesList;
		
		public List<IDictionary> nonpremiumRunesList;
		
		public static InventorySystem.ItemRune GetRandomPremiumRuneForSkullLevel(Databank dbBank, int skullLevel)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<IDictionary> premiumRunesInSkullLevel = new List<IDictionary>();
			IDictionary dictionary = null;
			int count = dbBank.premiumRunesList.Count;
			for(int i = 0;i< count ;i++)
			{
				dictionary = dbBank.premiumRunesList[i];
				
				if(System.Int32.Parse(dictionary["SkullLevel"].ToString()) == skullLevel)
					premiumRunesInSkullLevel.Add(dictionary);
			}
			
			int numberOfRunesMatchingSkullLevel = premiumRunesInSkullLevel.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRunesMatchingSkullLevel);
			
			if(numberOfRunesMatchingSkullLevel == 0)
			{
				return null;
			}
			
			IDictionary finalRuneDictionary = premiumRunesInSkullLevel[randomIndex];
			
			return new InventorySystem.ItemRune(finalRuneDictionary["RuneName"].ToString(), System.Int32.Parse(finalRuneDictionary["SkullLevel"].ToString()));
		}
		
		public static InventorySystem.ItemRune GetRandomNonpremiumRuneForSkullLevel(Databank dbBank, int skullLevel)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<IDictionary> nonpremiumRunesInSkullLevel = new List<IDictionary>();
			
			int count = dbBank.nonpremiumRunesList.Count;
			IDictionary dictionary = null;
			for(int i= 0;i<count;i++)
			{
				dictionary = dbBank.nonpremiumRunesList[i];
				
				if(System.Int32.Parse(dictionary["SkullLevel"].ToString()) == skullLevel)
					nonpremiumRunesInSkullLevel.Add(dictionary);
			}
			
			int numberOfRunesMatchingSkullLevel = nonpremiumRunesInSkullLevel.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRunesMatchingSkullLevel);
			
			if(numberOfRunesMatchingSkullLevel == 0)
			{
				return null;
			}
			
			IDictionary finalRuneDictionary = nonpremiumRunesInSkullLevel[randomIndex];
			
			return new InventorySystem.ItemRune(finalRuneDictionary["RuneName"].ToString(), System.Int32.Parse(finalRuneDictionary["SkullLevel"].ToString()));
		}
		
		public static InventorySystem.ItemRing GetRandomPremiumRingForSkullLevel(Databank dbBank, int skullLevel)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			
			InventorySystem.ItemRing aRing = null;
			int count = dbBank.premiumRingsList.Count;
			for(int i = 0;i<count;i++)
			{
				aRing = dbBank.premiumRingsList[i];
			
				if(aRing.skullLevel == skullLevel)
				{
					skullLevelRingsList.Add(aRing.Clone());
				}
			}
			
			int numberOfRingsMatchingSkullLevel = skullLevelRingsList.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRingsMatchingSkullLevel);
			
			if(numberOfRingsMatchingSkullLevel == 0)
			{
				return null;
			}
			
			return skullLevelRingsList[randomIndex];
		}
		
		public static InventorySystem.ItemRune GetRandomPremiumRuneForSkullLevel(Databank dbBank, int skullLevel, string marker)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<IDictionary> premiumRunesInSkullLevel = new List<IDictionary>();
			
			IDictionary dictionary = null;
			int count = dbBank.premiumRunesList.Count;
			for(int i = 0;i<count;i++)
			{
				dictionary = dbBank.premiumRunesList[i];
				if(System.Int32.Parse(dictionary["SkullLevel"].ToString()) == skullLevel && dictionary["Tag"].ToString().Contains(marker))
					premiumRunesInSkullLevel.Add(dictionary);
			}
			
			int numberOfRunesMatchingSkullLevel = premiumRunesInSkullLevel.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRunesMatchingSkullLevel);
			
			if(numberOfRunesMatchingSkullLevel == 0)
			{
				return null;
			}
			
			IDictionary finalRuneDictionary = premiumRunesInSkullLevel[randomIndex];
			
			return new InventorySystem.ItemRune(finalRuneDictionary["RuneName"].ToString(), System.Int32.Parse(finalRuneDictionary["SkullLevel"].ToString()));
		}
		
		public static InventorySystem.ItemRune GetRandomRuneForSkullLevel(Databank dbBank, int skullLevel, string[] markers, bool isPremiumSearch)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRune> randomRunePool = new List<InventorySystem.ItemRune>();
			
			int count = markers.Length;
			string marker = null;
			InventorySystem.ItemRune randomRune = null;
			for(int i = 0;i<count;i++)
			{
				marker = markers[i];
				if(isPremiumSearch)
					randomRune = GetRandomPremiumRuneForSkullLevel(dbBank, skullLevel, marker);
				else
					randomRune = GetRandomNonpremiumRuneForSkullLevel(dbBank, skullLevel, marker);
				if(randomRune != null)
					randomRunePool.Add(randomRune);
				
			}
			
			if(randomRunePool.Count > 0)
				return randomRunePool[new System.Random().Next(0,randomRunePool.Count)];
			return null;
		}
		
		public static InventorySystem.ItemRune GetRandomNonpremiumRuneForSkullLevel(Databank dbBank, int skullLevel, string marker)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<IDictionary> nonpremiumRunesInSkullLevel = new List<IDictionary>();
			
			int count = dbBank.nonpremiumRunesList.Count;
			IDictionary dictionary = null;
			for(int i = 0;i<count;i++)
			{
				dictionary = dbBank.nonpremiumRunesList[i];
				
				if(System.Int32.Parse(dictionary["SkullLevel"].ToString()) == skullLevel && dictionary["Tag"].ToString().Contains(marker))
					nonpremiumRunesInSkullLevel.Add(dictionary);
			}
			
			int numberOfRunesMatchingSkullLevel = nonpremiumRunesInSkullLevel.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRunesMatchingSkullLevel);
			
			if(numberOfRunesMatchingSkullLevel == 0)
			{
				return null;
			}
			
			IDictionary finalRuneDictionary = nonpremiumRunesInSkullLevel[randomIndex];
			
			return new InventorySystem.ItemRune(finalRuneDictionary["RuneName"].ToString(), System.Int32.Parse(finalRuneDictionary["SkullLevel"].ToString()));
		}
		
		public static InventorySystem.ItemRing GetRandomRingForSkullLevel(Databank dbBank, int skullLevel, string[] markers, bool isPremiumSearch)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			if(GameManager.PRINT_LOGS) Debug.Log("skullLevel  " + skullLevel + " markers  " + markers.ToString() + " isPremiumSearch " + isPremiumSearch);
			List<InventorySystem.ItemRing> randomRingPool = new List<InventorySystem.ItemRing>();
			
			string marker = null;
			int count = markers.Length;
			InventorySystem.ItemRing randomRing = null;
			for(int i = 0;i<count;i++)
			{
				marker = markers[i];
				if(isPremiumSearch)
					randomRing = GetRandomPremiumRingForSkullLevel(dbBank, skullLevel, marker);
				else
					randomRing = GetRandomNonpremiumRingForSkullLevel(dbBank, skullLevel, marker);
				if(randomRing != null)
					randomRingPool.Add(randomRing);
				
			}
			
			if(randomRingPool.Count > 0)
				return randomRingPool[new System.Random().Next(0,randomRingPool.Count)];
			return null;
		}
		
		public static InventorySystem.ItemRing GetRandomPremiumRingForSkullLevel(Databank dbBank, int skullLevel, string marker)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			
			InventorySystem.ItemRing aRing = null;
			int count = dbBank.premiumRingsList.Count;
			for(int i = 0;i<count;i++)
			{
				aRing = dbBank.premiumRingsList[i];
				if(aRing.skullLevel == skullLevel && aRing.tag.Contains(marker))
				{
					skullLevelRingsList.Add(aRing.Clone());
				}
			}
			
			int numberOfRingsMatchingSkullLevel = skullLevelRingsList.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRingsMatchingSkullLevel);
			
			if(numberOfRingsMatchingSkullLevel == 0)
			{
				return null;
			}
			
			return skullLevelRingsList[randomIndex];
		}
		
		public static InventorySystem.ItemRing GetRandomNonpremiumRingForSkullLevel(Databank dbBank, int skullLevel)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			
			InventorySystem.ItemRing aRing = null;
			int count = dbBank.nonpremiumRingsList.Count;
			for(int i =0 ;i<count;i++)
			{
				aRing = dbBank.nonpremiumRingsList[i];
				if(aRing.skullLevel == skullLevel)
				{
					skullLevelRingsList.Add(aRing.Clone());
				}
			}
			
			int numberOfRingsMatchingSkullLevel = skullLevelRingsList.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRingsMatchingSkullLevel);
			
			if(numberOfRingsMatchingSkullLevel == 0)
			{
				return null;
			}
			
			return skullLevelRingsList[randomIndex];
		}
		
		public static InventorySystem.ItemRing GetRandomNonpremiumRingForSkullLevel(Databank dbBank, int skullLevel, string marker)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			
			InventorySystem.ItemRing aRing = null;
			int count = dbBank.nonpremiumRingsList.Count;
			for(int i = 0;i<count;i++)
			{
				aRing = dbBank.nonpremiumRingsList[i];
				if(aRing.skullLevel == skullLevel && aRing.tag.Contains(marker))
				{
					skullLevelRingsList.Add(aRing.Clone());
				}
			}
			
			int numberOfRingsMatchingSkullLevel = skullLevelRingsList.Count;
			
			int randomIndex = new System.Random().Next(0,numberOfRingsMatchingSkullLevel);
			
			if(numberOfRingsMatchingSkullLevel == 0)
			{
				return null;
			}
			
			return skullLevelRingsList[randomIndex];
		}
		
		public static  List<InventorySystem.ItemRing> GetRandomRingsForSkullLevel(Databank dbBank, int skullLevel, int howMany)
		{
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			
			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			InventorySystem.ItemRing aRandomRing = null;
			while(skullLevelRingsList.Count != howMany)
			{
				aRandomRing = GetRandomNonpremiumRingForSkullLevel(dbBank, skullLevel);
				if(aRandomRing == null)
					throw new System.Exception("No Record For Rings at skull level = " + skullLevel + " found!!!");
				
				if(!skullLevelRingsList.Contains(aRandomRing))
				{
					skullLevelRingsList.Add(aRandomRing);
				}
			}
			
			return skullLevelRingsList;
		}

		public static InventorySystem.ItemRing GetSorcererRingForSkullLevel(Databank dbBank, int skullLevel)
		{
			Debug.LogError("skulllevel for Serover ring - before "+skullLevel);
			if(skullLevel >= GameManager.NUMBER_OF_LEVELS)
				skullLevel = GameManager.NUMBER_OF_LEVELS;
			Debug.LogError("skulllevel for Serover ring - "+skullLevel);
//			List<InventorySystem.ItemRing> skullLevelRingsList = new List<InventorySystem.ItemRing>();
			
			InventorySystem.ItemRing aRing = null;
			int count = dbBank.premiumRingsList.Count;
			for(int i = 0;i<count;i++)
			{
				aRing = dbBank.premiumRingsList[i];
				if(aRing.skullLevel == skullLevel && aRing._itemName.Equals("Sorcerer's Ring"))
				{
					return aRing;
				}
			}
			return null;
			
		}

		#endregion
		
		#region constants
		public static int BURST_DAMAGE_SUPPRESSION;
		public static float CHARGED_DAMAGE_MULTIPLIER;
		public static float CHARGED_DAMAGE_SPELLBURST;
		public static float ATTACK_SPEED_SLOW;
		public static float ATTACK_SPEED_MEDIUM;
		public static float ATTACK_SPEED_FAST;
		public static float ATTACK_SPEED_HASTE;
		public static int TOTAL_BURST_PROJECTILES;
		public static int MULTIPLAYER_HEAL_INTERVAL;
		public static float PERCENT_MULTIPLAYER_HEAL_AMOUNT;
		public static float GUILD_CREATE_COST;
		
		#endregion
	}
}