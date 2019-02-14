using UnityEngine;
using System.Collections;
public class LootManager {
	
	public const int chestLootModifier= 5;
	public static int getSoulDust(int skullLevel)
	{
		return GameManager._dataBank.GetBattleSoulLootForSkullLevel(skullLevel);
	}
	
	public static bool isLoot(string Type,int skullLevel)
	{
		if(Type.Equals("Duel"))
		{
			if(TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted))
			{
				int randomNumber1=Random.Range(1,101);
				int LootPercentage=GameManager._dataBank.GetLootPercentageForSkullLevel(skullLevel);
				if(randomNumber1<LootPercentage)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}
		else
		{
			return true;
		}
	}
	
	
	public static InventorySystem.InventoryItem DecideLoot(string type,ZoneModel zone, string keysRequired)
	{
		InventorySystem.InventoryItem LootedItem=null;
		
		int randomNumber1=Random.Range(1,101);
		
		
		if(type.Equals("Loot"))
		{
			Debug.Log("~~~~~~randomNUMBERRRRLOOOOTT!!!!!!"+randomNumber1);
			int LootSkullLevel=Helpers.CheckForSkullLevelBounds(GameManager._dataBank.keysToSkullModDictionary[keysRequired]);
			
			if(randomNumber1<=GameManager._dataBank.keysToSoulsPercentageDictionary[keysRequired])
			{
				LootedItem = new InventorySystem.ItemSoulBag(GameManager._dataBank.GetChestSoulLootForSkullLevel(LootSkullLevel)/chestLootModifier);
			}
			else if(randomNumber1>GameManager._dataBank.keysToSoulsPercentageDictionary[keysRequired] && randomNumber1<=GameManager._dataBank.keysToRingsPercentageDictionary[keysRequired])
			{
				LootedItem = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel(GameManager._dataBank,LootSkullLevel); //Rings
			}
			else if(randomNumber1>GameManager._dataBank.keysToRingsPercentageDictionary[keysRequired] && randomNumber1<=GameManager._dataBank.keysToRunesPercentageDictionary[keysRequired])
			{
				LootedItem = DatabankSystem.Databank.GetRandomRuneForSkullLevel(GameManager._dataBank,LootSkullLevel); //Runes
			}
			else if(randomNumber1>GameManager._dataBank.keysToRunesPercentageDictionary[keysRequired] && randomNumber1<=GameManager._dataBank.keysToPremiumRingsPercentageDictionary[keysRequired])
			{
				LootedItem = DatabankSystem.Databank.GetRandomPremiumRingForSkullLevel(GameManager._dataBank,LootSkullLevel); //Premium Rings
				if(LootedItem==null)
				{
					LootedItem = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel(GameManager._dataBank,LootSkullLevel);
				}
			}
			else if(randomNumber1>GameManager._dataBank.keysToPremiumRingsPercentageDictionary[keysRequired] && randomNumber1<=GameManager._dataBank.keysToPremiumRunesPercentageDictionary[keysRequired])
			{
				LootedItem = DatabankSystem.Databank.GetRandomRuneForSkullLevel(GameManager._dataBank,LootSkullLevel); //Premium Runes
				
				if(LootedItem==null)
				{
					LootedItem = DatabankSystem.Databank.GetRandomPremiumRuneForSkullLevel(GameManager._dataBank,LootSkullLevel);
				}
			}
//			string[] keysArray=keysRequired.Split(' ');
//			GameManager._gameState.User._inventory.keyRing.RemoveKeys(System.Convert.ToInt32(keysArray[0]));
			
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~randomNUMBERRRRDUELLLL!!!!!!"+randomNumber1);
			if(TutorialManager.instance.IsTutorialCompleted(TutorialManager.TutorialsAndCallback.arcKeysTutorialCompleted))
			{
				if(randomNumber1<=zone.potionsPercentage)
				{
						LootedItem = GameManager._dataBank.GetPotionForPotionID("HEALTH_POTION");
				}
				else if(randomNumber1>zone.potionsPercentage && randomNumber1<=zone.keysPercentage)
				{
					LootedItem = new InventorySystem.ItemKey();
				}
			}
			else
			{
				LootedItem = new InventorySystem.ItemKey();
			}

		}
		
		return LootedItem;
	}
	
	public static bool processLoot(InventorySystem.InventoryItem FinalLoot, out string errorMessage)
	{
		string error = "";
		
		bool success=false;
		if(FinalLoot.ItemType().Equals(InventorySystem.InventoryItem.Type.kHealthPotion))
		{
			error = "Upgrade your potion belt to carry more health potions!";
			success = GameManager._gameState.User._inventory.potionBelt.AddPotion((InventorySystem.ItemPotion)FinalLoot);
		}
		else if(FinalLoot.ItemType().Equals(InventorySystem.InventoryItem.Type.kKey))
		{
			error = "Upgrade your arcane keyring to carry more keys!";
			success = GameManager._gameState.User._inventory.keyRing.AddKey();
			GameManager.instance.scaleformCamera.levelScene.UpdateTotalKeys(GameManager._gameState.User._inventory.keyRing.keyCount);
		}
		else if(FinalLoot.ItemType().Equals(InventorySystem.InventoryItem.Type.kSoulBag))
		{
			error = "Upgrade your bottomless bag to carry more soul dust!";
			InventorySystem.ItemSoulBag tempSoulBag=(InventorySystem.ItemSoulBag)FinalLoot;
			GameManager._gameState.User._inventory.souls+=tempSoulBag.soulValue;
			GameManager.instance.SaveGameState(false);
			GameManager.instance.scaleformCamera.generalSwf.UpdateSoulDust(GameManager._gameState.User._inventory.souls);
			success=true;
		}
		else
		{	
			error = "Upgrade your bottomless bag to carry more items!";
			success = GameManager._gameState.User._inventory.bag.Add(FinalLoot);
		}
		
		errorMessage = error;
		return success;
		
		
	}
	
	public static bool CheckForKeys(int keys)
	{
		bool success=false;
		if(GameManager._gameState.User._inventory.keyRing.keyCount>=keys)
		{
			GameManager._gameState.User._inventory.keyRing.RemoveKeys(keys);
			success=true;
		}
		return success;
	}
}
