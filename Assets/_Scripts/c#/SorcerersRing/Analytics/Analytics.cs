using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class Analytics
{
	public static List<AnalyticsManager> managers = new List<AnalyticsManager>();
	
	public enum TransactionTransitionState
	{
		Purchasing,
		Purchased,
		Failed
	}
	
	public enum CurrencyTrackType
	{
		TrackLevel,
		TrackGems,
		TrackSouls,
		TrackCombats,
		TrackGameOver
	}
	
	public enum ParamName
	{
		ShinePoints,
		ShrineElement,
		ShrineLevelName,
		RingCost,
		RingName,
		RuneCost,
		RuneName,
		SkullLevel,
		IsPremium,
		GuildName,
		GuildDCost
	}
	
	public enum EventName
	{
		Shrine_Activate,
		Shrine_Open,
		Shrine_Connect,
		Shrine_Collect,
		Shrine_Charge,
		Shrine_Boost,
		Shrine_Invite,
		Shrine_Tutorial_Start,
		Shrine_Tutorial_End,
		RingUI_Tutorial_Start,
		RingUI_Tutorial_End,
		RuneUI_Tutorial_Start,
		RuneUI_Tutorial_End,
		UpgradesUI_Tutorial_Start,
		UpgradesUI_Tutorial_End,
		TransmuteUI_Tutorial_Start,
		TransmuteUI_Tutorial_End,
		BattleEnd_Tutorial_Start,
		BattleEnd_Tutorial_End,
		LowHealth_Tutorial_Start,
		LowHealth_Tutorial_End,
		
		BattleWater_Tutorial_Start,
		BattleWater_Tutorial_End,
		BattleFire_Tutorial_Start,
		BattleFire_Tutorial_End,
		BattleEarth_Tutorial_Start,
		BattleEarth_Tutorial_End,
		BattleLightning_Tutorial_Start,
		BattleLightning_Tutorial_End,
		
		BattleDaze_Tutorial_Start,
		BattleDrain_Tutorial_Start,
		BattleIgnite_Tutorial_Start,
		BattleLeechSeed_Tutorial_Start,
		
		BattleShield_Tutorial_Start,
		
		BattleAmplify_Tutorial_Start,
		BattleRegen_Tutorial_Start,
		BattleHaste_Tutorial_Start,
		BattleLock_Tutorial_Start,
		
		SpellBurst_Tutorial_Start,
		SpellBurst_Tutorial_End,
		
		RuneSpell_Tutorial_Start,
		RuneSpell_Tutorial_End,
		
		ArcaneKeys_Tutorial_Start,
		ArcaneKeys_Tutorial_End,
		
		Level_Tutorial_Start,
		
		Ring_Buy,
		Rune_Buy,
		
		Ring_Sell,
		Rune_Sell,
		
		PauseMenu,
		PauseMenu_Ring,
		PauseMenu_Rune,
		PauseMenu_Transmute,
		PauseMenu_Options,
		PauseMenu_Upgrade,
		PauseMenu_Market,
		
		Options_BattleTutorial,
		Options_Credits,
		Options_FacebookLogin,
		
		KeysPopUp_Buy,
		
		Market_BuyCard,
		
		Transmute_Start,
		Transmute_Boost,
		Transmute_Collect,
		
		Upgrade_Start,
		Upgrade_Boost,
		Upgrade_Collect,
		
		PotionPopUp_Buy,
		
		Guild_Buy
		/*
		#region Shrine
		Shrine_Activate,
		Shrine_Open,
		Shrine_Collect,
		Shrine_Charge,
		Shrine_Boost,
		Shrine_Tutorial,
		Shrine_Invite,
		Shrine_Connect,
		#endregion
		
		#region RingSwf
		Ring_Tutorial,
		Ring_Buy,
		Ring_Sell,
		#endregion
		
		#region RuneSwf
		Rune_Tutorial,
		Rune_Buy,
		Rune_Sell,
		#endregion
		
		#region TransmuteSwf
		Transmute_Start,
		Transmute_Boost,
		Transmute_Collect,
		Transmute_Tutorial,
		#endregion
		
		#region MarketSwf
		Market_BuyCard,
		#endregion
				
		#region Potion
		PotionPopUp_Use,
		PotionPopUp_Buy,
		PotionPopUp_Tutorial,
		#endregion
		
		#region UpgradeSwf
		Upgrade_Start,
		Upgrade_Boost,
		Upgrade_Collect,
		Upgrade_Tutorial,
		#endregion
		
		
		#region OptionSwf
		Options_FacebookLogin,
		Options_BattleTutorial,
		Options_Credits,
		#endregion
		
		#region Keys
		KeysPopUp_Buy
		#endregion
		*/
	}
	
	
	
	public static void registerAnalyticsManager(AnalyticsManager manager)
	{
		managers.Add(manager);
	}
	
	public static void unregisterAnalyticsManager(AnalyticsManager manager)
	{
		managers.Remove(manager);
	}
	
	public static void logEvent(EventName eventName)
	{
		Array.ForEach<AnalyticsManager>(managers.ToArray(),(obj) => { obj.logEvent(eventName.ToString()); });
	}
	
	public static void logEvent(EventName eventName, Dictionary<ParamName, string> parms)
	{
		Hashtable hashtable = new Hashtable();
		
		foreach(KeyValuePair<ParamName, string> pair in parms)
			hashtable.Add(pair.Key.ToString(), pair.Value);

		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach(KeyValuePair<ParamName, string> pair in parms)
			dictionary.Add(pair.Key.ToString(), pair.Value);

//		Array.ForEach<AnalyticsManager>(managers.ToArray(),(obj) => { obj.logEvent<Hashtable>(eventName.ToString(), hashtable); });
		Array.ForEach<AnalyticsManager>(managers.ToArray(),(obj) => { obj.logEvent(eventName.ToString(), dictionary); });
	}
	
	public static void logEvent(CurrencyTrackType trackWhat, int amount)
	{
		Array.ForEach<AnalyticsManager>(managers.ToArray(),(obj) => { obj.logEvent(trackWhat, amount); });
	}
	
	public static void logInappEvent(string productID, string transactionID, string transactionReceipt, string signature, TransactionTransitionState transactionState)
	{
		Array.ForEach<AnalyticsManager>(managers.ToArray(),(obj) => { obj.logInappEvent(productID, transactionID, transactionReceipt, signature, transactionState); });
	}
	
}
