using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MiniJSON;
using InventorySystem;

namespace DatabankSystem
{
	public delegate void DatabankLoadDelegate (Databank dataBank);

	public class DatabankLoader : MonoBehaviour
	{
	
		private const string VERSION = "version";
		private const string GAMESTATEVERSION = "gamestateVersion";
		private const string INAPPS = "inapps";
		private const string SECTION_CONSTANTS = "CONSTANTS";
		private const string SECTION_SKULL_STAT = "SkullToStat";
		private const string SECTION_RINGS = "RingBank";
		private const string SECTION_KEY_RING = "Key Rings";
		private const string SECTION_MONSTERS = "Monsters";
		private const string SECTION_BELTS = "Potion Belts";
		private const string SECTION_CUBES = "T_CUBE";
		private const string SECTION_BAGS = "Bags";
		private const string SECTION_POTIONS = "Potions";
		private const string SECTION_LEVELS = "Levels";
		private const string SECTION_POI_INFO = "Pois";
		private const string SECTION_ZONES = "Zones";
		private const string SECTION_BUFFS = "Buffs";
		private const string SECTION_SPIRITS = "SPIRITS";
		private const string SECTION_RUNE = "Runes";
		private const string SECTION_CHEST_LOOT = "Chest Loot";
		private const string SECTION_SHRINE_LOOT = "Shrine Loot";
		private const string SECTION_CREDITS = "Credits";

		
		public void LoadDatabank (DatabankLoadDelegate dbDel, bool isVersion)
		{
			if (GameManager.PRINT_LOGS)
				Debug.Log ("LoadDatabank");
			StartCoroutine (LoadFromFile (dbDel, isVersion));
		}
		
		public IEnumerator LoadFromFile (DatabankLoadDelegate del, bool isVersion)
		{
			string dbFilePath;
			
			if (Application.isEditor) {
				dbFilePath = Helpers.formatLocalUrlToRead ("DataBank.txt");
			} else {
				dbFilePath = Helpers.formatLocalPersistentUrlToRead ("DataBank.txt");
			}
			
	
			WWW www = new WWW (dbFilePath);
			
			yield return www;
	
			if (www.error == null) {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("JSON -->>" + www.text);
//				Debug.LogError("In Databank - "+GameUtils.Caesar(www.text, 5));
//				Debug.LogError("In Databank encode - "+GameUtils.Base64Encode(www.text));
				LoadFromJSON (www.text, del, isVersion);
			} else {
				if (GameManager.PRINT_LOGS)
					Debug.Log ("dbLoaderERROR: " + www.error);
			}
			
			yield return null;
		}
		
		public void LoadFromJSON (string jsonString, DatabankLoadDelegate del, bool isVersion)
		{
			Databank dbBank = new Databank ();
			IDictionary jsonDictionary = Json.Deserialize (jsonString) as IDictionary;
			if(jsonDictionary == null) {
				Debug.LogError("Json Dictionary is null");
			}
			if (isVersion) {
				dbBank.version = jsonDictionary [VERSION].ToString ();
			} else {
				SetConstants (jsonDictionary [SECTION_CONSTANTS] as IDictionary);
				IList skullLevelStatsList = jsonDictionary [SECTION_SKULL_STAT] as IList;
				IList ringList = jsonDictionary [SECTION_RINGS] as IList;
				IList monstersList = jsonDictionary [SECTION_MONSTERS] as IList;
				IList beltList = jsonDictionary [SECTION_BELTS] as IList;
				IList tCubeList = jsonDictionary [SECTION_CUBES] as IList;
				IList bagList = jsonDictionary [SECTION_BAGS] as IList;
				IList potionList = jsonDictionary [SECTION_POTIONS] as IList;
				IList levelsList = jsonDictionary [SECTION_LEVELS] as IList;
				IList poiDataList = jsonDictionary [SECTION_POI_INFO] as IList;
				IList zonesList = jsonDictionary [SECTION_ZONES] as IList;
				IList buffList = jsonDictionary [SECTION_BUFFS] as IList;
				IList keyRingList = jsonDictionary [SECTION_KEY_RING] as IList;
				IList runesIList = jsonDictionary [SECTION_RUNE] as IList;
				IList chestLoot = jsonDictionary [SECTION_CHEST_LOOT] as IList;
				IList spiritsList = jsonDictionary [SECTION_SPIRITS] as IList;
				IList shrineList = jsonDictionary [SECTION_SHRINE_LOOT] as IList;
				IList creditsList = jsonDictionary [SECTION_CREDITS] as IList;
				dbBank.version = jsonDictionary [VERSION].ToString ();
				dbBank.gamestateVersion = Convert.ToInt32 (jsonDictionary [GAMESTATEVERSION].ToString ());
				dbBank.inApps = LoadInApps (jsonDictionary [INAPPS] as IList);
				dbBank.buffDictionary = LoadBuffs (buffList);
				dbBank.Levels = LoadLevelsData (levelsList);//LoadLevels(levelsList,poiDataList,zonesList);
				dbBank.Pois = LoadPOIData (poiDataList);
				dbBank.Zones = LoadZonesData (zonesList);
				
				Dictionary<int, int> [] skullStatList = LoadStatPointsFromSkull (skullLevelStatsList);
				dbBank.skullToStatDictionary = skullStatList [0];
				dbBank.statToSkullDictionary = skullStatList [1];
				
				List<IntValPair<IDictionary>> impList = new List<IntValPair<IDictionary>> ();
				List<IntValPair<IDictionary>> golemList = new List<IntValPair<IDictionary>> ();
				List<IntValPair<IDictionary>> aquaList = new List<IntValPair<IDictionary>> ();
				List<IntValPair<IDictionary>> drakeList = new List<IntValPair<IDictionary>> ();
				dbBank.spiritsDictionary = LoadSpirits (spiritsList, out impList, out golemList, out aquaList, out drakeList);
				dbBank.sortedImpList = impList;
				dbBank.sortedGolemList = golemList;
				dbBank.sortedAquaList = aquaList;
				dbBank.sortedDrakeList = drakeList;
				
				dbBank.skullToUTimeDictionary = LoadUTimeDictionary (skullLevelStatsList);
				dbBank.skullToRingTierDictionary = LoadRingTierFromSkull (skullLevelStatsList);
				dbBank.skullToDustRequiredDictionary = LoadDustRequiredFromSkull (skullLevelStatsList);
				dbBank.skullTotCostDictionary = LoadtCostFromSkull (skullLevelStatsList);
				dbBank.skullTobCostDictionary = LoadbCostFromSkull (skullLevelStatsList);
				dbBank.skullTogCostDictionary = LoadgCostFromSkull (skullLevelStatsList);
				dbBank.skullTotLootPercentageDictionary = LoadLootPercentageFromSkull (skullLevelStatsList);
				dbBank.skullToBattleSoulLootDictionary = LoadBattleSoulLootFromSkull (skullLevelStatsList);
				dbBank.skullToChestSoulLootDictionary = LoadChestSoulLootFromSkull (skullLevelStatsList);
				dbBank.skullToChestGemsLootDictionary = LoadChestLootGemsFromSkull (skullLevelStatsList);
				
				List<InventorySystem.ItemRing> [] ringsList = LoadRings (ringList); 
				dbBank.premiumRingsList = ringsList [0];
				dbBank.nonpremiumRingsList = ringsList [1];
				
				//dbBank.runeDictionary = LoadRunes(runesList);
				
				List<IDictionary> premiumRunesIDictionaryList = new List<IDictionary> ();
				List<IDictionary> nonpremiumRunesIDictionaryList = new List<IDictionary> ();
				dbBank.runeDictionary = LoadRunes (runesIList, out premiumRunesIDictionaryList, out nonpremiumRunesIDictionaryList);
				dbBank.premiumRunesList = premiumRunesIDictionaryList;
				dbBank.nonpremiumRunesList = nonpremiumRunesIDictionaryList;
				
				
				Dictionary<string, Dictionary<int, int>> encounterToSkullLevelDictionary = new Dictionary<string, Dictionary<int, int>> ();
				Dictionary<string, int> monsterToMaxLevelDictionary = new Dictionary<string, int> ();
				dbBank.monstersDictionary = AILoader.LoadMonsterData (monstersList, out monsterToMaxLevelDictionary, out encounterToSkullLevelDictionary);
				dbBank.aiToMaxLevelDictionary = monsterToMaxLevelDictionary;
				dbBank.encounterToSkullLevelDictionary = encounterToSkullLevelDictionary;
				
				List<PotionBelt> sortedPotionBeltList = new List<PotionBelt> ();
				dbBank.potionBeltDictionary = LoadBelts (beltList, out sortedPotionBeltList);
				dbBank.sortedPotionBeltList = sortedPotionBeltList;
				
				List<Bag> sortedBagList = new List<Bag> ();
				dbBank.bagDictionary = LoadBags (bagList, out sortedBagList);
				dbBank.sortedBagList = sortedBagList;
				
				dbBank.potionsDictionary = LoadPotions (potionList);
				
				List<TransmutationCube> sortedTCubeList = new List<TransmutationCube> ();
				dbBank.transmutationCubeDictionary = LoadTransmutationCubes (tCubeList, out sortedTCubeList);
				dbBank.sortedTCubeList = sortedTCubeList;
				
				
				
				List<KeyRing> sortedKeyRings = new List<KeyRing> ();
				dbBank.keyRingDictionary = LoadKeyRings (keyRingList, out sortedKeyRings);
				dbBank.keysToSkullModDictionary = LoadKeysToSkullModDictionary (chestLoot);
				dbBank.keysToSoulsPercentageDictionary = LoadKeysToSoulsPercentageDictionary (chestLoot);
				dbBank.keysToRingsPercentageDictionary = LoadKeysToRingsPercentageDictionary (chestLoot);
				dbBank.keysToRunesPercentageDictionary = LoadKeysToRunesPercentageDictionary (chestLoot);
				dbBank.keysToPremiumRingsPercentageDictionary = LoadKeysToPremiumRingsPercentageDictionary (chestLoot);
				dbBank.keysToPremiumRunesPercentageDictionary = LoadKeysToPremiumRunesPercentageDictionary (chestLoot);
				dbBank.ShrineDataDictionary = ShrineInfo.LoadShrineInfo (shrineList);
		
				dbBank.creditsDictionary = NGCredits.LoadCredits(creditsList);

				dbBank.sortedKeyRingList = sortedKeyRings;
			}
			del (dbBank);
		}
		
		
		private Dictionary<string, InventorySystem.KeyRing> LoadKeyRings (IList keyRings, out List<KeyRing> keyRingListToSort)
		{
			Dictionary<string, InventorySystem.KeyRing> keyRingDictionary = new Dictionary<string, InventorySystem.KeyRing> ();
			
			List<InventorySystem.KeyRing> listOfKeyRings = new List<InventorySystem.KeyRing> ();
			IDictionary aKeyRing;
			
			InventorySystem.KeyRing keyRing;
			
			for (int i = 0; i<keyRings.Count; i++) {
				aKeyRing = keyRings [i] as IDictionary;
				
				int capacity = Convert.ToInt32 (aKeyRing ["Capacity"].ToString ());
				
				string cubeId = aKeyRing ["id"].ToString ();
				
				int cubeLevel = Convert.ToInt32 (aKeyRing ["uLevel"].ToString ());
				
				int skullLevel = Convert.ToInt32 (aKeyRing ["SkullLevel"].ToString ());
				
				float upgradeDustCost = Convert.ToSingle (aKeyRing ["dCost"].ToString ());
				
				float upgradeTimeCost = Convert.ToSingle (aKeyRing ["uTime"].ToString ());
				
				string description = aKeyRing ["Description"].ToString ();
				
				keyRing = new InventorySystem.KeyRing (capacity, skullLevel, cubeId, cubeLevel, upgradeDustCost, upgradeTimeCost, description);
				listOfKeyRings.Add (keyRing);
				keyRingDictionary.Add (cubeId, keyRing);
			}
			
			listOfKeyRings.Sort ();
			keyRingListToSort = listOfKeyRings;
			
			return keyRingDictionary;
		}
		
		public List<InventorySystem.ItemRing> [] LoadRings (IList ringList)
		{
			List<InventorySystem.ItemRing> [] ringsList = new List<InventorySystem.ItemRing>[2];
			List<InventorySystem.ItemRing> nonpremiumRingsList = new List<InventorySystem.ItemRing> ();
			
			List<InventorySystem.ItemRing> premiumRingsList = new List<InventorySystem.ItemRing> ();
			
			bool isPremium;
			IDictionary ringDictionary;
			
			for (int i = 0; i<ringList.Count; i++) {
				isPremium = false;
// 				if(GameManager.PRINT_LOGS) Debug.Log("UNIQUE ID ::::::::::: " + Nonce.GetUniqueID());
				
				InventorySystem.ItemRing ring = new InventorySystem.ItemRing ();
				
				ringDictionary = ringList [i] as IDictionary;
				
				string ringName = ringDictionary [InventoryLoader.KEY_RING_NAME].ToString ();
				
				ring.SetItemName (ringName);
				
				ring.id = ringName;
				
				ring.skullLevel = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_SKULL_LEVEL].ToString ());
				
				ring.fire = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_FIRE].ToString ());
				
				ring.lightning = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_LIGHTNING].ToString ());
				
				ring.water = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_WATER].ToString ());
				
				ring.earth = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_EARTH].ToString ());
				
				ring.wards = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_WARDS].ToString ());
				
				ring.life = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_LIFE].ToString ());
				
				ring.damage = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_DAMAGE].ToString ());
				
				ring.stats = Convert.ToInt32 (ringDictionary [InventoryLoader.KEY_INVENTORY_STATS].ToString ());
				
				ring.dCost = Convert.ToInt32 (ringDictionary [InventoryLoader.TAG_DUST_COST].ToString ());
			
				ring.tag = ringDictionary [InventoryLoader.TAG_TAG].ToString ();
				
				ring.gCost = Convert.ToInt32 (ringDictionary [InventoryLoader.TAG_GEMS_COST].ToString ());
				
				if (ring.gCost > 0)
					isPremium = true;
					
				if (ringDictionary [InventoryLoader.KEY_RING_TYPE].ToString ().Equals (InventoryLoader.KEY_RING_TYPE_DAMAGE)) {
					ring.ringType = ItemRing.RingType.kDamage;
				} else if (ringDictionary [InventoryLoader.KEY_RING_TYPE].ToString ().Equals (InventoryLoader.KEY_RING_TYPE_LIFE)) {
					ring.ringType = ItemRing.RingType.kLife;
				} else if (ringDictionary [InventoryLoader.KEY_RING_TYPE].ToString ().Equals (InventoryLoader.KEY_RING_TYPE_WARD)) {
					ring.ringType = ItemRing.RingType.kLife;
				} else {
					ring.ringType = ItemRing.RingType.kElement;
				}
				
				if (isPremium)
					premiumRingsList.Add (ring);
				else
					nonpremiumRingsList.Add (ring);
			}
			ringsList [0] = premiumRingsList;
			ringsList [1] = nonpremiumRingsList;
			return ringsList;
		}
		
		private Dictionary<KeyValuePair<string, int>, IDictionary> LoadRunes (IList runesList, out List<IDictionary> premiumDictionaryList, out List<IDictionary> nonpremiumDictionaryList)
		{
			List<IDictionary> pDictionaryList = new List<IDictionary> ();
			List<IDictionary> npDictionaryList = new List<IDictionary> ();
			Dictionary<KeyValuePair<string, int>, IDictionary> runesDictionary = new Dictionary<KeyValuePair<string, int>, IDictionary> ();
			
			for (int i = 0; i < runesList.Count; i++) {
				IDictionary aRuneDictionary = runesList [i] as IDictionary;
				string runeName = aRuneDictionary ["RuneName"].ToString ();
				int runeSkullLevel = System.Convert.ToInt32 (aRuneDictionary ["SkullLevel"].ToString ());
				
				if (!Databank.skullToRuneNameListDictionary [runeSkullLevel].Contains (runeName)) {
					Databank.skullToRuneNameListDictionary [runeSkullLevel].Add (runeName);
					if (Int32.Parse (aRuneDictionary ["gCost"].ToString ()) > 0)
						pDictionaryList.Add (aRuneDictionary);
					else
						npDictionaryList.Add (aRuneDictionary);
				}
				
				runesDictionary.Add (new KeyValuePair<string, int> (runeName, runeSkullLevel), aRuneDictionary);
			}
			
			premiumDictionaryList = pDictionaryList;
			nonpremiumDictionaryList = npDictionaryList;
			return runesDictionary;
		}
		
		
		private Dictionary<KeyValuePair<string, int>, IDictionary> LoadBuffs (IList buffList)
		{
			Dictionary<KeyValuePair<string, int>, IDictionary> buffsDictionary = new Dictionary<KeyValuePair<string, int>, IDictionary> ();
			
			for (int i = 0; i < buffList.Count; i++) {
				IDictionary aBuffDictionary = buffList [i] as IDictionary;

				buffsDictionary.Add (new KeyValuePair<string, int> (aBuffDictionary ["SpellName"].ToString (),
					System.Convert.ToInt32 (aBuffDictionary ["SkullLevel"].ToString ())), aBuffDictionary);
			}
			
			return buffsDictionary;
		}
		
		private Dictionary<string,Level> LoadLevelsData (IList levelsList)
		{
			IDictionary aLevel = null;
			Dictionary<string,Level> levelDetails = new Dictionary<string, Level> ();
			for (int i=0; i<levelsList.Count; i++) {
				Level aLevelDetail = null;
				aLevel = levelsList [i] as IDictionary;
				string zoneID = aLevel ["ZONE ID"].ToString ();
				string levelID = aLevel ["LEVEL ID"].ToString ();
				aLevelDetail = new Level (levelID, zoneID);
				IList poiList = aLevel ["POI ID"] as IList;
				
				
				aLevelDetail.poiNameList = new List<string> ();
				for (int j=0; j<poiList.Count; j++) {
					aLevelDetail.poiNameList.Add (poiList [j].ToString ());
				}
				levelDetails.Add (levelID, aLevelDetail);
				//if(GameManager.PRINT_LOGS) Debug.Log("LevelPoiName=="+aLevelDetail.poiNameList[0]);
			}
			return levelDetails;
		}
		
		private Dictionary<string,POISet> LoadPOIData (IList poiDataList)
		{
			Dictionary<string,POISet> PoiDetails = new Dictionary<string, POISet> ();
			for (int k=0; k<poiDataList.Count; k++) {
				POISet aSet = new POISet ();
				IDictionary aPoiListItem = poiDataList [k] as IDictionary;
				aSet.id = aPoiListItem ["POI ID"].ToString ();
				aSet.type = aPoiListItem ["Type"].ToString ();
				aSet.protectedBy = aPoiListItem ["ProtectedBy"].ToString ();
				aSet.nextLevel = aPoiListItem ["NextLevel"].ToString ();
				aSet.isCleared = false;
				PoiDetails.Add (aSet.id, aSet);
			}
			
			return PoiDetails;
		}
		
		private Dictionary<string,ZoneModel> LoadZonesData (IList zonesList)
		{
			Dictionary<string,ZoneModel> zoneDetails = new Dictionary<string, ZoneModel> ();
			
			for (int i=0; i<zonesList.Count; i++) {
				ZoneModel aZoneDetail = new ZoneModel ();
				IDictionary aZoneListItem = zonesList [i] as IDictionary;
				
				aZoneDetail.zoneId = aZoneListItem ["Zone ID"].ToString ();
				//aZoneDetail.skullMod=Convert.ToInt32(aZoneListItem["SkullMod"].ToString());
				aZoneDetail.potionsPercentage = Convert.ToInt32 (aZoneListItem ["Potions"].ToString ());
				aZoneDetail.keysPercentage = Convert.ToInt32 (aZoneListItem ["Keys"].ToString ());
				//aZoneDetail.ConsumablesPercentage=Convert.ToInt32(aZoneListItem["Consumables"].ToString());
				aZoneDetail.CharmsPercentage = Convert.ToInt32 (aZoneListItem ["Charms"].ToString ());
				IList enemiesList = aZoneListItem ["EnemyList"] as IList;
				aZoneDetail.enemyList = new List<string> ();
				for (int l=0; l<enemiesList.Count; l++) {
					aZoneDetail.enemyList.Add (enemiesList [l].ToString ());
				}
				
				zoneDetails.Add (aZoneDetail.zoneId, aZoneDetail);
			}
			
			return zoneDetails;
		}
		
		private Dictionary<string, InventorySystem.TransmutationCube> LoadTransmutationCubes (IList tCubes, out List<TransmutationCube> sortedTCubeList)
		{
			Dictionary<string, InventorySystem.TransmutationCube> tCubeDictionary = new Dictionary<string, InventorySystem.TransmutationCube> ();
			
			List<TransmutationCube> listOfCubes = new List<TransmutationCube> ();
			IDictionary aTCube;
			
			InventorySystem.TransmutationCube transmutationCube;
			
			for (int i = 0; i<tCubes.Count; i++) {
				aTCube = tCubes [i] as IDictionary;
				
				int cubeCapacity = Convert.ToInt32 (aTCube ["Capacity"].ToString ());
				
				string cubeId = aTCube ["id"].ToString ();
				
				int cubeLevel = Convert.ToInt32 (aTCube ["uLevel"].ToString ());
				
				int skullLevel = Convert.ToInt32 (aTCube ["SkullLevel"].ToString ());
				
				float upgradeGemCost = Convert.ToSingle (aTCube ["gCost"].ToString ());
				
				float upgradeTimeCost = Convert.ToSingle (aTCube ["uTime"].ToString ());
				 
				int tLoss = Convert.ToInt32 (aTCube ["tLoss"].ToString ());
				
				string description = aTCube ["Description"].ToString ();
				
				transmutationCube = new InventorySystem.TransmutationCube (cubeCapacity, skullLevel, cubeId, cubeLevel, tLoss, upgradeGemCost, upgradeTimeCost, description);
				
				listOfCubes.Add (transmutationCube);
				tCubeDictionary.Add (cubeId, transmutationCube);
			}
			
			listOfCubes.Sort ();
			sortedTCubeList = listOfCubes;
			return tCubeDictionary;
		}
		
		
		private Dictionary<string, InventorySystem.ItemPotion> LoadPotions (IList potionsList)
		{
			Dictionary<string, InventorySystem.ItemPotion> potionDictionary = new Dictionary<string, InventorySystem.ItemPotion> ();
			
			IDictionary aPotion;
			
			InventorySystem.ItemPotion potion;
			
			for (int i = 0; i < potionsList.Count; i++) {
				aPotion = potionsList [i] as IDictionary;
				
				string potionId = aPotion ["id"].ToString ();
				
				string potionName = aPotion ["name"].ToString ();
				
				if (potionName.Equals ("WARD POTION")) {
					potion = new InventorySystem.ItemWardPotion ();
				} else {
					potion = new InventorySystem.ItemHealthPotion ();
				}
				
				potion.SetItemName (potionName);
				potion.id = potionId;
				
				potionDictionary.Add (potion.id, potion);
			}
			
			return potionDictionary;
		}
		
		private Dictionary<string, InventorySystem.Bag> LoadBags (IList bagList, out List<Bag> bagsList)
		{
			List<Bag> listOfBags = new List<Bag> ();
			Dictionary<string, InventorySystem.Bag> bagDictionary = new Dictionary<string, InventorySystem.Bag> ();
			
			IDictionary aBag;
			
			InventorySystem.Bag bag;
			
			for (int i =0; i<bagList.Count; i++) {	
				aBag = bagList [i] as IDictionary;
				
				int bagCapacity = Convert.ToInt32 (aBag ["Capacity"].ToString ());
				
				string bagId = aBag ["id"].ToString ();
				
				int skullLevel = Convert.ToInt32 (aBag ["SkullLevel"].ToString ());
				
				int bagLevel = Convert.ToInt32 (aBag ["uLevel"].ToString ());
				
				float upgradeGemCost = Convert.ToSingle (aBag ["dCost"].ToString ());
				
				float upgradeTimeCost = Convert.ToSingle (aBag ["uTime"].ToString ());
				
				string description = aBag ["Description"].ToString ();
				
				float soulCapacity = Convert.ToSingle (aBag [BAG_SOUL_CAPACITY].ToString ());
				
				//if(GameManager.PRINT_LOGS) Debug.Log("SOUL CAPACITY ::::::: " + soulCapacity);
				
				bag = new InventorySystem.Bag (bagCapacity, skullLevel, bagId, bagLevel, upgradeGemCost, upgradeTimeCost, description, soulCapacity);
								
				listOfBags.Add (bag);
				bagDictionary.Add (bagId, bag);
			}
			
			listOfBags.Sort ();
			bagsList = listOfBags;
			
//			for(int i = 0;i<bagsList.Count;i++)
//				if(GameManager.PRINT_LOGS) Debug.Log("BAG :::::: INDEX =  "+ i + "TO STRING ::::::" +bagsList.ToString());
				
			return bagDictionary;
		}
		
		public const string BAG_SOUL_CAPACITY = "maxDust";
		
		private Dictionary<string, InventorySystem.PotionBelt> LoadBelts (IList beltsList, out List<PotionBelt> pBeltList)
		{
			Dictionary<string, InventorySystem.PotionBelt> beltDictionary = new Dictionary<string, InventorySystem.PotionBelt> ();
			List<PotionBelt> listOfBelts = new List<PotionBelt> ();
			
			IDictionary aBelt;
			
			InventorySystem.PotionBelt potionBelt;
			
			for (int i =0; i<beltsList.Count; i++) {	
				aBelt = beltsList [i] as IDictionary;
				
				int beltCapacity = Convert.ToInt32 (aBelt ["Capacity"].ToString ());
				
				string beltId = aBelt ["id"].ToString ();
				
				int beltLevel = Convert.ToInt32 (aBelt ["uLevel"].ToString ());
				
				int skullLevel = Convert.ToInt32 (aBelt ["SkullLevel"].ToString ());
				
				float upgradeGemCost = Convert.ToSingle (aBelt ["dCost"].ToString ());
				
				float upgradeTimeCost = Convert.ToSingle (aBelt ["uTime"].ToString ());
				
				string description = aBelt ["Description"].ToString ();
				
				int healAmount = Convert.ToInt32 (aBelt ["battleHeal"].ToString ());
				potionBelt = new InventorySystem.PotionBelt (beltCapacity, skullLevel, beltId, beltLevel, upgradeGemCost, upgradeTimeCost, description, healAmount);
				listOfBelts.Add (potionBelt);
				beltDictionary.Add (beltId, potionBelt);
			}
			
			listOfBelts.Sort ();
			pBeltList = listOfBelts;
			return beltDictionary;
		}
		
		private Dictionary<int, float> LoadUTimeDictionary (IList skullStatList)
		{
			Dictionary<int, float> statUTimeDictionary = new Dictionary<int, float> ();
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				statUTimeDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToSingle (keyValPair ["uTime"].ToString ()));
			}
			
			return statUTimeDictionary;
		}
		
		private Dictionary<string, int> LoadKeysToSkullModDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToSkullModDictionary = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToSkullModDictionary.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["SkullMod"].ToString ()));
			}	
			
			return KeysToSkullModDictionary;
		}
		
		
		private Dictionary<string, int> LoadKeysToSoulsPercentageDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToSoulsPercentage = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToSoulsPercentage.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["Souls"].ToString ()));
			}	
			
			return KeysToSoulsPercentage;
		}
		
		private Dictionary<string, int> LoadKeysToRingsPercentageDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToRingsPercentage = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToRingsPercentage.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["Ring"].ToString ()));
			}	
			
			return KeysToRingsPercentage;
		}
		
		private Dictionary<string, int> LoadKeysToRunesPercentageDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToRunesPercentage = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToRunesPercentage.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["Rune"].ToString ()));
			}	
			
			return KeysToRunesPercentage;
		}
		
		private Dictionary<string, int> LoadKeysToPremiumRingsPercentageDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToPremiumRingsPercentage = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToPremiumRingsPercentage.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["PremiumRing"].ToString ()));
			}	
			
			return KeysToPremiumRingsPercentage;
		}
		
		private Dictionary<string, int> LoadKeysToPremiumRunesPercentageDictionary (IList chestLoot)
		{
			Dictionary<string, int> KeysToPremiumRunesPercentage = new Dictionary<string, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<chestLoot.Count; i++) {
				keyValPair = chestLoot [i] as IDictionary;
				
				KeysToPremiumRunesPercentage.Add (keyValPair ["LOOT ID"].ToString (), Convert.ToInt32 (keyValPair ["PremiumRune"].ToString ()));
			}	
			
			return KeysToPremiumRunesPercentage;
		}
		
		private Dictionary<int, int> [] LoadStatPointsFromSkull (IList skullStatList)
		{
			Dictionary<int, int> [] dictionaryList = new Dictionary<int, int>[2];
			
			Dictionary<int, int> skullStatDictionary = new Dictionary<int, int> ();
			Dictionary<int, int> statSkullDictionary = new Dictionary<int, int> ();
			
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				skullStatDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["Stats"].ToString ()));
				statSkullDictionary.Add (Convert.ToInt32 (keyValPair ["Stats"].ToString ()), Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()));
			}
			
			dictionaryList [0] = skullStatDictionary;
			dictionaryList [1] = statSkullDictionary;
			
			return dictionaryList;
		}
		
		private Dictionary<int, string> LoadRingTierFromSkull (IList skullStatList)
		{
			Dictionary<int, string> RingTierDictionary = new Dictionary<int, string> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				RingTierDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), keyValPair ["Tier"].ToString ());
			}	
			
			return RingTierDictionary;
		}
		
		private Dictionary<int, int> LoadDustRequiredFromSkull (IList skullStatList)
		{
			Dictionary<int, int> DustRequiredDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				DustRequiredDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["dCost"].ToString ()));
			}	
			
			return DustRequiredDictionary;
		}
		
		private Dictionary<int, int> LoadgCostFromSkull (IList skullStatList)
		{
			Dictionary<int, int> gCostDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				gCostDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["gCost"].ToString ()));
			}	
			
			return gCostDictionary;
		}
		private Dictionary<int, int> LoadbCostFromSkull (IList skullStatList)
		{
			Dictionary<int, int> bCostDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				bCostDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["bCost"].ToString ()));
			}	
			
			return bCostDictionary;
		}
		
		private Dictionary<int, int> LoadtCostFromSkull (IList skullStatList)
		{
			Dictionary<int, int> tCostDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				tCostDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["tCost"].ToString ()));
			}	
			
			return tCostDictionary;
		}
		
		private Dictionary<int, int> LoadBattleSoulLootFromSkull (IList skullStatList)
		{
			Dictionary<int, int> SoulLootDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				SoulLootDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["DustDrop"].ToString ()));
//				if(GameManager.PRINT_LOGS) Debug.Log("BattleSoulLoot "+i+"="+keyValPair["DustDrop"].ToString());
			}	
			
			return SoulLootDictionary;
		}
		
		private Dictionary<int, int> LoadChestSoulLootFromSkull (IList skullStatList)
		{
			Dictionary<int, int> ChestSoulLootDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
//			IDictionary dustKeyVal;

			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
//				dustKeyVal = keyValPair["Dust"] as IDictionary;

				ChestSoulLootDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["Dust/CP"].ToString ()));
//				ChestSoulLootDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (dustKeyVal ["CP"].ToString ()));
//				if(GameManager.PRINT_LOGS) Debug.Log("ChestSoulLoot "+i+"="+keyValPair["Dust/CP"].ToString());
			}	
			
			return ChestSoulLootDictionary;
		}
		
		private Dictionary<int, int> LoadChestLootGemsFromSkull (IList skullStatList)
		{
			Dictionary<int, int> GemLootDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
//			IDictionary gemKeyVal;

			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
//				gemKeyVal = keyValPair["Gems"] as IDictionary;

				GemLootDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["Gems/CP"].ToString ()));
//				GemLootDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (gemKeyVal ["CP"].ToString ()));

//				if(GameManager.PRINT_LOGS) Debug.Log("ChestSoulLoot "+i+"="+keyValPair["Gems/CP"].ToString());
			}	
			
			return GemLootDictionary;
		}
		
		private Dictionary<int, int> LoadLootPercentageFromSkull (IList skullStatList)
		{
			Dictionary<int, int> LootPercentageDictionary = new Dictionary<int, int> ();
			
			IDictionary keyValPair;
			
			for (int i = 0; i<skullStatList.Count; i++) {
				keyValPair = skullStatList [i] as IDictionary;
				
				LootPercentageDictionary.Add (Convert.ToInt32 (keyValPair ["SkullLevel"].ToString ()), Convert.ToInt32 (keyValPair ["LootDrop"].ToString ()));
			}	
			
			return LootPercentageDictionary;
		}
		
		private Dictionary<string, IDictionary> LoadSpirits (IList spiritIList, out List<IntValPair<IDictionary>> sortedImpList, out List<IntValPair<IDictionary>> sortedGolemList, out List<IntValPair<IDictionary>> sortedWispList, out List<IntValPair<IDictionary>> sortedDrakeList)
		{
			Dictionary<string, IDictionary> spiritsDictionary = new Dictionary<string, IDictionary> ();
			
			List<IntValPair<IDictionary>> sImpList = new List<IntValPair<IDictionary>> ();
			List<IntValPair<IDictionary>> sGolemList = new List<IntValPair<IDictionary>> ();
			List<IntValPair<IDictionary>> sWispList = new List<IntValPair<IDictionary>> ();
			List<IntValPair<IDictionary>> sDrakeList = new List<IntValPair<IDictionary>> ();
				
			for (int i = 0; i < spiritIList.Count; i++) {
				IDictionary spiritDictionary = spiritIList [i] as IDictionary;
				
				string spiritId = spiritDictionary ["id"].ToString ();
				string spiritName = spiritId.Substring (0, spiritId.Length - 1);
				int spiritULevel = Int32.Parse (spiritId.Substring (spiritId.Length - 1, 1));
				
				switch (spiritName) {
				case "IMP":
					sImpList.Add (new IntValPair<IDictionary> (spiritULevel, spiritDictionary));
					break;
				case "GOLEM":
					sGolemList.Add (new IntValPair<IDictionary> (spiritULevel, spiritDictionary));
					break;
				case "AQUA":
					sWispList.Add (new IntValPair<IDictionary> (spiritULevel, spiritDictionary));
					break;
				case "DRAKE":
					sDrakeList.Add (new IntValPair<IDictionary> (spiritULevel, spiritDictionary));
					break;
				}
				spiritsDictionary.Add (spiritId, spiritDictionary);
			}
			
			sImpList.Sort ();
			sortedImpList = sImpList;
			sGolemList.Sort ();
			sortedGolemList = sGolemList;
			sWispList.Sort ();
			sortedWispList = sWispList;
			sDrakeList.Sort ();
			sortedDrakeList = sDrakeList;
			
			return spiritsDictionary;
		}
		
		private InApps LoadInApps (IList inAppsIList)
		{	
			List<InApp> inAppsList = new List<InApp> ();
			InApp inApp = null;
			
			int count = inAppsIList.Count;
			IDictionary inAppDictionary = null;
			for (int i = 0; i< count; i++) {
				inAppDictionary = inAppsIList [i] as IDictionary;
				
				if (inAppDictionary.Contains (InApp.InAppTypes.PORTIONS.ToString ())) {
					IDictionary inAppPortion = inAppDictionary [InApp.InAppTypes.PORTIONS.ToString ()] as IDictionary;
					inApp = new InAppProtion (InApp.InAppTypes.PORTIONS, inAppPortion ["name"].ToString (), System.Convert.ToInt32 (inAppPortion ["gCost"]));
				} else if (inAppDictionary.Contains (InApp.InAppTypes.KEYS.ToString ())) {
					IDictionary inAppKeys = inAppDictionary [InApp.InAppTypes.KEYS.ToString ()] as IDictionary;
					inApp = new InAppKeys (InApp.InAppTypes.KEYS, inAppKeys ["name"].ToString (), System.Convert.ToInt32 (inAppKeys ["gCost"]));
				}
				if (inApp != null) {
					inAppsList.Add (inApp);
				}
			}
			
			return new InApps (inAppsList);
		}
		
		private void SetConstants (IDictionary constantsDictionary)
		{
			Databank.ATTACK_SPEED_FAST = float.Parse (constantsDictionary ["ATTACK_SPEED_FAST"].ToString ());
			Databank.ATTACK_SPEED_SLOW = float.Parse (constantsDictionary ["ATTACK_SPEED_SLOW"].ToString ());
			Databank.ATTACK_SPEED_HASTE = float.Parse (constantsDictionary ["ATTACK_SPEED_HASTE"].ToString ());
			Databank.ATTACK_SPEED_MEDIUM = float.Parse (constantsDictionary ["ATTACK_SPEED_MEDIUM"].ToString ());
			Databank.BURST_DAMAGE_SUPPRESSION = int.Parse (constantsDictionary ["BURST_DAMAGE_SUPPRESSION"].ToString ());
			Databank.CHARGED_DAMAGE_MULTIPLIER = float.Parse (constantsDictionary ["CHARGED_DAMAGE_MULTIPLIER"].ToString ());
			Databank.CHARGED_DAMAGE_SPELLBURST = float.Parse (constantsDictionary ["CHARGED_DAMAGE_SPELLBURST"].ToString ());
			Databank.TOTAL_BURST_PROJECTILES = int.Parse (constantsDictionary ["TOTAL_BURST_PROJECTILES"].ToString ());
			Databank.MULTIPLAYER_HEAL_INTERVAL = int.Parse (constantsDictionary ["MULTIPLAYER_HEAL_INTERVAL"].ToString ());
			Databank.PERCENT_MULTIPLAYER_HEAL_AMOUNT = float.Parse (constantsDictionary ["PERCENT_MULTIPLAYER_HEAL_AMOUNT"].ToString ());
			Databank.GUILD_CREATE_COST = float.Parse (constantsDictionary ["GUILD_CREATE_COST"].ToString ());
		}
	}
}
