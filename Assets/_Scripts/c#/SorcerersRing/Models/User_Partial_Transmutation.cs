using UnityEngine;
using System.Collections;
using InventorySystem;
using System.Collections.Generic;

public partial class User : CharacterModel
{
	private const int MIN_TRANSMUTE_STAT = 10;

	[Newtonsoft.Json.JsonIgnore]
	private IDictionary _emptyTransmutationDictionary = new Dictionary<string, object>();
	[Newtonsoft.Json.JsonIgnore]
	public IDictionary emptyTransmutationDictionary
	{
		get
		{	
			_emptyTransmutationDictionary["BaseItems"] = new List<IDictionary>();
			_emptyTransmutationDictionary["slots"] = this._inventory.transmutationCube.capacity;
			return _emptyTransmutationDictionary;
		}
		private set
		{
			_emptyTransmutationDictionary = value;
		}
	}
	
	public void RollBackTransmutation()
	{
		if(transmutationDictionary["Type"].ToString().Equals(InventoryItem.Type.kRing.ToString()))
		{
			IList itemsList = transmutationDictionary["BaseItems"] as IList;
			for(int i = 0;i< itemsList.Count;i++)
			{
				IDictionary ringDictionary = itemsList[i] as IDictionary;
				this._inventory.bag.Add(InventoryLoader.GetRingObjectFromJsonData(ringDictionary));
			}
		}
		else
		{
			IList itemsList = transmutationDictionary["BaseItems"] as IList;
			for(int i = 0;i< itemsList.Count;i++)
			{
				IDictionary runeDictionary = itemsList[i] as IDictionary;
				this._inventory.bag.Add(InventoryLoader.GetRuneObjectFromJsonData(runeDictionary));
			}
		}
		
		this.transmutationDictionary = emptyTransmutationDictionary;
	}
	
	public IDictionary _transmutationDictionary = new Dictionary<string, object>();
	[Newtonsoft.Json.JsonIgnore]
	public IDictionary transmutationDictionary
	{
		get
		{
			
			if((_transmutationDictionary["BaseItems"] as IList).Count == 0)
				return emptyTransmutationDictionary;
			return _transmutationDictionary;
		}
		set
		{
			_transmutationDictionary = value;
		}
	}
	
	public long GetTimeForTransmutation(List<InventoryItem> items)
	{
		List<ItemRing> ringsList = new List<ItemRing>();
		List<ItemRune> runesList = new List<ItemRune>();
		
		bool isRing = false;
		bool isRune = false;
		
		InventoryItem anItem = null;
		int  count = items.Count;
		for(int i = 0;i<items.Count;i++)
		{
			anItem = items[i] as InventoryItem;
			
			if(anItem as ItemRing != null)
			{
				isRing = true;
				ringsList.Add(anItem as ItemRing);
			}
			
			if(anItem as ItemRune != null)
			{
				isRune = true;
				runesList.Add(anItem as ItemRune);
			}
			
			if(isRing && isRune)
				throw new System.Exception("Can't transmute rings and runes together!");
		}
		
		InventorySystem.ItemRing transmutedRing = null;
		InventorySystem.ItemRune transmutedRune = null;
		
		long uTime;
		
		if(isRing)
		{
			transmutedRing = TransmuteRings(ringsList);
			uTime = (long)GameManager._dataBank.GetUTimeForSkullLevel(transmutedRing.skullLevel);
			return uTime;

		}
		else
		{
			transmutedRune = TransmuteRunes(runesList);
			uTime = (long)GameManager._dataBank.GetUTimeForSkullLevel(transmutedRune.skullLevel);
	
			return uTime;
		}
	}
	
	public IDictionary Transmute(List<InventoryItem> items, long endTime = -1, long currentTime = -1, long timeRemaining = -1)
	{
		Debug.Log("TRANSMUTE CALLED !!!");
		
		int equipmentSlots = GameManager._gameState.User._inventory.transmutationCube.capacity;
		
		List<ItemRing> ringsList = new List<ItemRing>();
		List<ItemRune> runesList = new List<ItemRune>();
		
		bool isRing = false;
		bool isRune = false;
		
		InventoryItem anItem = null;
		for(int i = 0; i < items.Count; i++)
		{
			anItem = items[i] as InventoryItem;
			
			if(anItem as ItemRing != null)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("IS RING");
				isRing = true;
				ringsList.Add(anItem as ItemRing);
			}
			
			if(anItem as ItemRune != null)
			{
				if(GameManager.PRINT_LOGS) Debug.Log("IS RUNE:::::::::");
				isRune = true;
				runesList.Add(anItem as ItemRune);
			}
			
			if(isRing && isRune)
				throw new System.Exception("Can't transmute rings and runes together!");
		}
		
		InventorySystem.ItemRing transmutedRing = null;
		InventorySystem.ItemRune transmutedRune = null;
		
		float uTime;
		string uid = null;
		if(transmutationDictionary.Contains("uid"))
			uid = transmutationDictionary["uid"].ToString();
		string boostuid = null;
		if(transmutationDictionary.Contains("Boostuid"))
			boostuid = transmutationDictionary["Boostuid"].ToString();
		if(isRing)
		{
			transmutedRing = TransmuteRings(ringsList);
			if(GameManager.PRINT_LOGS) Debug.Log(" FINAL RING >>>>>>>> " + transmutedRing.ToString());
			uTime = GameManager._dataBank.GetUTimeForSkullLevel(transmutedRing.skullLevel);
			
			return new TransmutationProgress<ItemRing>(ringsList, endTime, currentTime, (long)uTime, transmutedRing, GameManager._dataBank.GettCostForSkullLevel(transmutedRing.skullLevel), equipmentSlots, uid, boostuid, timeRemaining).ToDictionary();
		}
		else
		{
			transmutedRune = TransmuteRunes(runesList);
			uTime = GameManager._dataBank.GetUTimeForSkullLevel(transmutedRune.skullLevel);
			
			return new TransmutationProgress<ItemRune>(runesList, endTime, currentTime, (long)uTime, transmutedRune, GameManager._dataBank.GettCostForSkullLevel(transmutedRune.skullLevel), equipmentSlots, uid, boostuid, timeRemaining).ToDictionary();
		}	
	}
	
	
	#region Ring Transmutation
	public ItemRing TransmuteRings(List<ItemRing> ringsList)
	{
		ItemRing finalRing = null;
		List<string> markers = new List<string>();
		ItemRing baseRing = GetBaseRingForTransmutation(ringsList, out markers);
		
		int statPool = GetStatPointsForRings(ringsList, baseRing);
		int newSkullLevel = GetSkullAgainstStatPoints(statPool + (baseRing.stats + MIN_TRANSMUTE_STAT));
		
		if(GameManager.PRINT_LOGS) Debug.Log("baseRing.stats" + baseRing.stats + "        statPool::::   "+statPool);
		
		for(int i = 0;i<markers.Count;i++)
			if(GameManager.PRINT_LOGS) Debug.Log("*************************************                  MARKER...:::::::::::::::   "+markers[i]);
		
		if(baseRing.gCost>0)
			finalRing = DatabankSystem.Databank.GetRandomRingForSkullLevel(GameManager._dataBank, newSkullLevel,markers.ToArray(), true);
		else
			finalRing = DatabankSystem.Databank.GetRandomRingForSkullLevel(GameManager._dataBank, newSkullLevel,markers.ToArray(), false);
		
		return finalRing;
	}
	
	
	private ItemRing GetBaseRingForTransmutation(List<ItemRing> ringsList, out List<string> finalMarkers)
	{
		List<ItemRing> premiumRings = new List<ItemRing>();
		List<ItemRing> nonpremiumRings = new List<ItemRing>();
		
		ItemRing baseRing = null;
		
		List<string> markers = new List<string>();
		
		ItemRing aRing = null;
		for(int i = 0;i<ringsList.Count;i++)
		{
			aRing = ringsList[i] as ItemRing;
			if(GameManager.PRINT_LOGS) Debug.Log("A RING --->>> "+aRing.ToString());
			if(aRing.gCost > 0)
				premiumRings.Add(aRing);
			else
				nonpremiumRings.Add(aRing);
		}
		
		baseRing = GetHighestLevelRingFromList(premiumRings, out markers);
		if(baseRing == null)
			baseRing = GetHighestLevelRingFromList(nonpremiumRings, out markers);
		
		finalMarkers = markers;
		return baseRing;
	}
	
	
	private ItemRing GetHighestLevelRingFromList(List<ItemRing> ringsList, out List<string> finalMarkers)
	{
		ItemRing[] ringsArray = ringsList.ToArray();
		
		List<string> markers = new List<string>();
		finalMarkers = markers;
		if(ringsArray.Length<= 0)
			return null;
		
		ItemRing highLevelRing = ringsArray[0];
		
		int count = ringsList.Count;
		ItemRing aRing = null;
		for(int i = 0;i<count;i++)
		{
			aRing = ringsList[i] as ItemRing;
			if(!markers.Contains(aRing.tag))
				markers.Add(aRing.tag);
			
			if(aRing.skullLevel > highLevelRing.skullLevel)
				highLevelRing = aRing;
		}
		
		finalMarkers = markers;
		return highLevelRing;
	}
	
	
	private int GetStatPointsForRings(List<ItemRing> ringsList, ItemRing exceptionRing)
	{
		int statPool = 0;
		
		int count = ringsList.Count;
		ItemRing aRing = null;
		for(int i = 0;i<count;i++)
		{
			aRing = ringsList[i] as ItemRing;
			if(aRing == exceptionRing) //Reference checking here...
				continue;
			
			statPool += ((int)(aRing.stats * _inventory.transmutationCube.transmutationLoss/100));
		}
		
		if(GameManager.PRINT_LOGS) Debug.Log("STAT POOL ---" + statPool + " after transmutation loss = " + _inventory.transmutationCube.transmutationLoss);
		return statPool;
	}
	#endregion
	

	#region Runes transmutation
	private ItemRune TransmuteRunes(List<ItemRune> runesList)
	{
		ItemRune finalRune = null;
		List<string> markers = new List<string>();
		ItemRune baseRune = GetBaseRuneForTransmutation(runesList, out markers);
		int statPool = GetStatPointsForRunes(runesList, baseRune);
		int newSkullLevel = GetSkullAgainstStatPoints(statPool + (baseRune.stats + MIN_TRANSMUTE_STAT));
		
		for(int i = 0;i<markers.Count;i++)
			if(GameManager.PRINT_LOGS) Debug.Log("*************************************                  MARKER...:::::::::::::::   "+markers[i]);
		
		if(baseRune.gCost > 0)
			finalRune = DatabankSystem.Databank.GetRandomRuneForSkullLevel(GameManager._dataBank, newSkullLevel, markers.ToArray(), true);
		else
			finalRune = DatabankSystem.Databank.GetRandomRuneForSkullLevel(GameManager._dataBank, newSkullLevel, markers.ToArray(), false);
		
		
		return finalRune;
	}
	
	private ItemRune GetBaseRuneForTransmutation(List<ItemRune> runesList, out List<string> finalMarkers)
	{
		List<ItemRune> premiumRunes = new List<ItemRune>();
		List<ItemRune> nonpremiumRunes = new List<ItemRune>();
		
		ItemRune baseRune = null;
		
		List<string> markers = new List<string>();
		int count = runesList.Count;
		ItemRune aRune = null;
		for(int i = 0;i<count; i++)
		{
			aRune = runesList[i] as ItemRune;
			if(aRune.gCost > 0)
				premiumRunes.Add(aRune);
			else
				nonpremiumRunes.Add(aRune);
		}
		baseRune = GetHighestLevelRuneFromList(premiumRunes, out markers);
		if(baseRune == null)
			baseRune = GetHighestLevelRuneFromList(nonpremiumRunes, out markers);
		
		if(GameManager.PRINT_LOGS) Debug.Log("BASE RUNE ::::::::::::::: "+baseRune.ToString());
		finalMarkers = markers;
		return baseRune;
	}
	
	private ItemRune GetHighestLevelRuneFromList(List<ItemRune> runesList, out List<string> finalMarkers)
	{
		ItemRune[] runesArray = runesList.ToArray();
		List<string> markers = new List<string>();
		finalMarkers = markers;
		if(runesArray.Length <= 0)
			return null;
		
		ItemRune highLevelRune = runesArray[0];
		ItemRune aRune = null;
		int count = runesList.Count;
		for(int i = 0;i<count;i++)
		{
			aRune = runesList[i] as ItemRune;
			
			if(!markers.Contains(aRune.tag))
				markers.Add(aRune.tag);
			
			if(aRune.skullLevel > highLevelRune.skullLevel)
				highLevelRune = aRune;
		}
		
		finalMarkers = markers;
		return highLevelRune;
	}
	
	private int GetStatPointsForRunes(List<ItemRune> runesList, ItemRune exceptionRune)
	{
		int statPool = 0;
		int count = runesList.Count;
		ItemRune aRune = null;
		for(int i = 0;i<count;i++)
		{
			aRune = runesList[i] as ItemRune;
			if(aRune == exceptionRune) //Reference checking here...
				continue;
			
			statPool += ((int)(aRune.stats * _inventory.transmutationCube.transmutationLoss/100));
		}
		
		if(GameManager.PRINT_LOGS) Debug.Log("STAT POOL FOR RUNES ---" + statPool + " after transmutation loss = " + _inventory.transmutationCube.transmutationLoss);
		return statPool;
	}
	#endregion
	
	
	public class TransmutationProgress <T> where T : InventoryItem
	{
		private List<T> itemsList; 
		private long duration;
		private long endTime;
		private long currentTime;
		private T outputItem;
		private int gCost;
		private int equipmentSlots;
		private string uid;
		private string boostuid;
		
		public TransmutationProgress(List<T> itemsList,long endTime, long currentTime, long duration, T outputItem, int gCost, int equipmentSlots, string uid = null, string boostuid = null, long timeRemaining = -1)
		{
			this.itemsList = itemsList;
			this.outputItem = outputItem;
			this.endTime = endTime;
			this.currentTime = currentTime;
			this.duration = duration;
			this.gCost = gCost;
			this.equipmentSlots = equipmentSlots;
			this.uid = uid;
			this.boostuid = boostuid;
			this.timeRemaining = timeRemaining;
		}
		
		private double _timeRemaining = -1;
		public double timeRemaining
		{
			get
			{
				//long timeRem = -1;
//				if(GameManager.PRINT_LOGS) Debug.Log("System.TimeSpan(matureTime - Helpers.GetSecondsFromEpoch(System.DateTime.UtcNow)).TotalSeconds" + timeRem);
				return this._timeRemaining;
			}
			private set
			{
				_timeRemaining = value;
			}
		}
		
		
		public IDictionary ToDictionary()
		{
			IDictionary transmutationDictionary = new Dictionary<string, object>();
			
			IList itemsIList = new List<IDictionary>();
			
			T item = null;
			int count = this.itemsList.Count;
			for(int i = 0;i<count;i++)
			{
				item = this.itemsList[i] as T;
				itemsIList.Add(item.ToDictionary());
			}
			
			transmutationDictionary["BaseItems"] = itemsIList;
			transmutationDictionary["Type"] = outputItem.ItemType();
			transmutationDictionary["FinalItem"] = outputItem.ToDictionary();
			transmutationDictionary["TimeRemaining"] = timeRemaining;
			transmutationDictionary["gCost"] = this.gCost;
			transmutationDictionary["CurrentTime"] = this.currentTime;
			transmutationDictionary["EndTime"] = this.endTime;
			transmutationDictionary["BoostCost"] = GameManager._dataBank.GetbCostForSkullLevel(outputItem.skullLevel);
			transmutationDictionary["Duration"] = this.duration;
			transmutationDictionary["slots"] = this.equipmentSlots;
			if(this.uid != null)
				transmutationDictionary["uid"] = this.uid;
			else
				transmutationDictionary["uid"] = Nonce.GetUniqueID();
			if(this.boostuid != null)
				transmutationDictionary["Boostuid"] = this.boostuid;
			else
				transmutationDictionary["Boostuid"] = Nonce.GetUniqueID();
			transmutationDictionary["slots"] = this.equipmentSlots;
			
			if(GameManager.PRINT_LOGS) Debug.Log("MiniJSON.Json.Serialize(transmutationDictionary)" + MiniJSON.Json.Serialize(transmutationDictionary));
			
			return transmutationDictionary;
		}
	}
	
	
	
	public IDictionary LoadTransmutationProgress(IDictionary transmutationDictionary)
	{
		if(transmutationDictionary == null || transmutationDictionary["Type"] == null)
			return emptyTransmutationDictionary;
		
		string uid = null;
			
		if(transmutationDictionary.Contains("uid"))
			uid = transmutationDictionary["uid"].ToString();
		
		string boostuid = null;
			
		if(transmutationDictionary.Contains("Boostuid"))
			boostuid = transmutationDictionary["Boostuid"].ToString();
//		else
//			uid = transmutationDictionary["uid"].ToString();
		
		//transmutationDictionary["uid"] = uid;
		
		if(transmutationDictionary["Type"].ToString().Equals(InventoryItem.Type.kRing+""))
		{
			IList ringsJsonList = transmutationDictionary["BaseItems"] as IList;
			List<InventorySystem.ItemRing> ringsList = new List<InventorySystem.ItemRing>();
			for(int i = 0;i<ringsJsonList.Count;i++)
			{
				ringsList.Add(InventoryLoader.GetRingObjectFromJsonData(ringsJsonList[i] as IDictionary));
			}
			InventorySystem.ItemRing transmutedRing = InventoryLoader.GetRingObjectFromJsonData(transmutationDictionary["FinalItem"] as IDictionary);
			
			//long matureTime = long.Parse(transmutationDictionary["MatureTime"].ToString());
			long endTime = long.Parse(transmutationDictionary["EndTime"].ToString());
			long currentTime = long.Parse(transmutationDictionary["CurrentTime"].ToString());
			long timeRemaining = -1;//long.Parse(transmutationDictionary["TimeRemaining"].ToString());
			long duration = long.Parse(transmutationDictionary["Duration"].ToString());
			return new TransmutationProgress<ItemRing>(ringsList, endTime, currentTime, duration, transmutedRing, GameManager._dataBank.GettCostForSkullLevel(transmutedRing.skullLevel), GameManager._gameState.User._inventory.transmutationCube.capacity, uid, boostuid, timeRemaining).ToDictionary();
		}
		else
		{
			IList runesJsonList = transmutationDictionary["BaseItems"] as IList;
			List<InventorySystem.ItemRune> runesList = new List<InventorySystem.ItemRune>();
			for(int i = 0;i<runesJsonList.Count;i++)
			{
				runesList.Add(InventoryLoader.GetRuneObjectFromJsonData(runesJsonList[i] as IDictionary)); 
			}
			InventorySystem.ItemRune transmutedRune = InventoryLoader.GetRuneObjectFromJsonData(transmutationDictionary["FinalItem"] as IDictionary); 
			
			//long matureTime = long.Parse(transmutationDictionary["MatureTime"].ToString());
			long startTime = long.Parse(transmutationDictionary["EndTime"].ToString());
			long duration = long.Parse(transmutationDictionary["Duration"].ToString());
			long currentTime = long.Parse(transmutationDictionary["CurrentTime"].ToString());
			long timeRemaining = -1;//long.Parse(transmutationDictionary["TimeRemaining"].ToString());
			return new TransmutationProgress<ItemRune>(runesList, startTime, currentTime, duration, transmutedRune, GameManager._dataBank.GettCostForSkullLevel(transmutedRune.skullLevel), GameManager._gameState.User._inventory.transmutationCube.capacity, uid, boostuid, timeRemaining).ToDictionary();	
		}
	}
	
	private int GetSkullAgainstStatPoints(int statPoints)
	{
		if(GameManager._dataBank.HasStatToSkullValue(statPoints))
			return GameManager._dataBank.statToSkullDictionary[statPoints];
		else
		{
			if(statPoints - 1 > 0)
				return GetSkullAgainstStatPoints(statPoints - 1);
			else
				throw new System.Exception("Invalid stat points to lookup against!");
		}
	}
}