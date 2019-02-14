using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RaidsManager
{
    private static RaidsManager _instance = null;
    private RaidsManager()
    {
        monsters = new List<RaidAIModel>();
    }
	
    public static RaidsManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RaidsManager();
            return _instance;
        }
    }
	
    public List<RaidAIModel> monsters;
	
    public List<RaidAIModel> activeMonsters
    {
        get
        {
            List<RaidAIModel> actMonsters = new List<RaidAIModel>();
            int count = monsters.Count;
            for (int i = 0; i < count; i++)
            {
                if (monsters [i].isActive)
                    actMonsters.Add(monsters [i]);
            }
            return actMonsters;
        }
        private set	{	}
    }
	
	
	
//    public long _serverTime = -1;
///*		public DateTime ServerTime {
//				private set	{	}
//				get {
// if(_serverTime != -1)
////					return Helpers.UnixTimeStampToDateTime(_serverTime);
//					return Helpers.FromUnixTime(_serverTime);
//	            else
//						return DateTime.Now;
//			}
//		}*/
//	long offset = 0;
//
//    public long ServerTime
//    {
//        get
//        {
////			return _serverTime;
//			DateTime dtNow = DateTime.Now;
//			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
//			var time = posixTime.AddMilliseconds(offset);
//			var diff = dtNow - time;
//			Debug.LogError("serverTime - "+_serverTime+" total "+diff.TotalMilliseconds);
//			return _serverTime + (long)diff.TotalMilliseconds;
//			// return Helpers.GetTimeFromNow(_serverTime);
////          if(_serverTime != -1)
////			else
////                return DateTime.Now;
//        }
//		set{
//			DateTime dtNow = DateTime.Now;
////			DateTime dtServer = new DateTime(
//			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
//			var time = posixTime.AddMilliseconds(value);
//			var diff = dtNow - time;
//			offset = (long)diff.TotalMilliseconds;
////			Debug.LogError("ofset - "+ offset + " value - "+ value);
//		}
//    }
//
    #region Get Boss Battle Result
    public delegate void BattleResultListener(bool requestSucceeded);
	System.Action<object, object, ServerRequest> resultListener;
	public void GetBossBattleResult(bool playerWon, int damage, string bossName, int bossSkullLevel,  System.Action<object, object, ServerRequest> handle)
	{
        if (GameManager.PRINT_LOGS) Debug.LogError("GetBossBattleResult");
        this.resultListener = handle;
        ServerManager.Instance.GetBossBattleResult(playerWon, damage, bossName, bossSkullLevel, handle);
    }
	
    public void OnBattleResult(object responseParameters, object error, ServerRequest request)
    {
        IDictionary response = responseParameters as IDictionary;
        if (GameManager.PRINT_LOGS) Debug.LogError("OnBattleResult - RESPONSE SUCCESSFUL! >> " + MiniJSON.Json.Serialize(response));
		
//        if (error == null)
//        {
//            if (resultListener != null)
//                resultListener(responseParameters, error, );
//        } else
//        {
//            if (resultListener != null)
//                resultListener(false);
//            if (GameManager.PRINT_LOGS)
//                Debug.Log(" error " + error.ToString());
//        }
    }
	#endregion
	
	#region Get Boss Battle Result
    BattleResultListener lootResultListener;
    public void LootRaidBoss(string bossName, int bossSkullLevel, BattleResultListener lootResultListener)
    {
        this.lootResultListener = lootResultListener;
        ServerManager.Instance.LootRaidBoss(bossName, bossSkullLevel, this.OnLootRaidBoss);
    }
	
    private void OnLootRaidBoss(object responseParameters, object error, ServerRequest request)
    {
        if (error == null)
        {
            IDictionary response = responseParameters as IDictionary;
			
            lootResultListener(bool.Parse(response ["success"].ToString()));
        } else
        {
            lootResultListener(false);
            if (GameManager.PRINT_LOGS)
                Debug.Log(" error " + error.ToString());
        }
    }
	#endregion
	
	#region Get arcana rating
    BattleResultListener arcaneRatingListener;
    private void GetArcaneRating()
    {
        Debug.Log("GetArcaneRating");
        ServerManager.Instance.GetArcanaLeaderboard(this.OnArcaneRatingReceived);
    }
	
    public IList topRatings = null;
    public IList relativeRatings = null;
    public void OnArcaneRatingReceived(object responseParameters, object error, ServerRequest request)
    {
//        if (GameManager.PRINT_LOGS)
        Debug.LogError("RESPONSE FOR ARCANE RATING ::::::::: " + MiniJSON.Json.Serialize(responseParameters as IDictionary));
		
        ServerManager.Instance.LoadAllRaidBosses(LoadRaidBossHandler);
		
        if (error == null)
        {
            IDictionary response = responseParameters as IDictionary;
            if (MiniJSON.Json.Serialize(responseParameters as IDictionary) == null || (response == null))
            {
                this.topRatings = null;
                this.relativeRatings = null;
            } else
            {
                this.topRatings = (response.Contains("topRatings")) ? (response ["topRatings"] as IList) : null;
                this.relativeRatings = (response.Contains("relativeRatings")) ? (response ["relativeRatings"] as IList) : null;
            }
        }
    }

#endregion
	
	#region Get all Raid Bosses 
    private System.Action getRaidBossAck;
    public void GetAllRaidBosses(System.Action getRaidBossAck)
    {
//		Debug.Log("GetAllRaidBosses");
        this.getRaidBossAck = getRaidBossAck;
//        ServerManager.Instance.LoadAllRaidBosses(LoadRaidBossHandler);


		/***********************************************/
		GetArcaneRating();
		/***********************************************/
	}
	private DateTime _offsetDatetime;
	public long TimeOffset{
		get{
			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
			var diff = DateTime.UtcNow - _offsetDatetime;
			return (long)diff.TotalMilliseconds;
		}
		set{
			_offsetDatetime = DateTime.UtcNow;
		}

	}
    public void LoadRaidBossHandler(object responseParameters, object error, ServerRequest request)
    {
        if (error == null)
        {
			
            IDictionary response = responseParameters as IDictionary;
//            Debug.Log("LoadRaidBossHandler: " + (response == null) + "   NULL: " + string.IsNullOrEmpty(MiniJSON.Json.Serialize(response)));
            if ((response == null) || !bool.Parse(response ["success"].ToString()))
            {
                LoadRaidBossHandler(responseParameters, "Request Unsuccessful!", request);
                return;
            }
			
            if (GameManager.PRINT_LOGS)
                Debug.LogError("LoadRaidBossHandler ::: " + MiniJSON.Json.Serialize(response));
            IList monList = response ["raidBosses"] as IList;
//			_serverTime = Convert.ToDouble(response["serverTS"].ToString());
			long serverTime = -1;
            if (!long.TryParse(response ["serverTS"].ToString(), out serverTime))
				serverTime = -1;
			InitGameVersions.instance.ServerTime = serverTime;
			TimeOffset = 0;

			if (monsters == null)
                monsters = new List<RaidAIModel>();
            else
            {
                monsters.Clear();
                monsters.TrimExcess();
            }
			
			GameObject.Find("ArcanumRuhalis(Clone)").GetComponent<RaidPortalNavigator>().SetMonsterCount(monList.Count);
			IDictionary aMonsterDictionary;
            for (int i = 0; i<monList.Count; i++)
            {
                aMonsterDictionary = monList [i] as IDictionary;
                monsters.Add(new RaidAIModel(aMonsterDictionary));
            }
            this.getRaidBossAck();
        } else
        {
            monsters = null;
            this.getRaidBossAck();
            if (GameManager.PRINT_LOGS)
                Debug.Log(" error " + error.ToString());
        }
    }
	#endregion
	
}

