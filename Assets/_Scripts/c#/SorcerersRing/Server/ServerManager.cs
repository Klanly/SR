using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FRAG;
using MiniJSON;

public class ServerManager
{
	
	private static ServerManager _instance = null;
	private Dictionary<string, object> updateResponseDictionary = null;
	
	private const string TRANSMUTATION = "transmutation";
	private const string UPGRADE = "upgrade";
	private const string SPIRIT = "spirit";
	
	private ServerManager ()
	{
		updateResponseDictionary = new Dictionary<string, object> ();
	}
	
	public static ServerManager Instance {
		get {
			if (_instance == null) {
				_instance = new ServerManager ();
			}
			return _instance;
		}
	}
	
	public void ProcessRequest (ServerRequest request)
	{

//		Debug.LogError("Internet Reachability status - "+Application.internetReachability.ToString() + " request type - "+request.RequestType.ToString());
//		Debug.Log("ProcessRequest called.... >> request >> " + request.RequestType);
		switch (request.RequestType) {
		case ServerRequest.ServerRequestType.Transmutation:
		case ServerRequest.ServerRequestType.Upgrade:
		case ServerRequest.ServerRequestType.Spirit:
//			Debug.LogError("Timer started - "+request.RequestType.ToString());
			GameManager.instance.fragNetworkingNew.StartCoroutine (StartTimer (request));
			break;
		case ServerRequest.ServerRequestType.All:
			GameManager.instance.fragNetworkingNew.StartCoroutine (FetchAllTimer (request));
			break;
		case ServerRequest.ServerRequestType.TransmutationCompleted:
		case ServerRequest.ServerRequestType.UpgradeCompleted:
		case ServerRequest.ServerRequestType.SpiritCompleted:
			GameManager.instance.fragNetworkingNew.StartCoroutine (VerifyTimer (request));
				//GameManager.instance.fragNetworking.StartCoroutine(StartTimer(request));
			break;
		case ServerRequest.ServerRequestType.GetInAppPurchases:
				GameManager.instance.fragNetworkingNew.StartCoroutine(GetInAppPurchases(request));
			break;
		case ServerRequest.ServerRequestType.VerifyInAppPurchase:
				GameManager.instance.fragNetworkingNew.StartCoroutine(VerifyInAppPurchase(request));
			break;
		case ServerRequest.ServerRequestType.LogTransactions:
			GameManager.instance.fragNetworkingNew.StartCoroutine (LogTransactions (request));
			break;
		case ServerRequest.ServerRequestType.ActivateShrine:
			GameManager.instance.fragNetworkingNew.StartCoroutine (CheckShrineActivation (request));
			break;
		case ServerRequest.ServerRequestType.ActivateFriendsShrine:
			GameManager.instance.fragNetworkingNew.StartCoroutine (CheckFriendShrineActivation (request));
			break;
		case ServerRequest.ServerRequestType.CollectShrineReward:
			GameManager.instance.fragNetworkingNew.StartCoroutine (CollectShrineReward (request));
			break;
		}

		GameManager.instance.SaveGameState (false);
	}
	
	public void LoadMetaData (System.Action<object, object, ServerRequest> handler, string version = "")
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.LoadMetaData (version, handler, handler);
	}


	public void GetShrineInformation (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.GetShrineInformation (handler, handler);
	}

//	public void ActivateShrine(string element, System.Action<object, object, ServerRequest> handler)
//	{
//		GameManager.instance.fragNetworkingNew.ActivateShrine (element, handler, handler);
//	}
	
	public void RegisterWithFacebook (System.Action<object, object, ServerRequest> handler, string fbAccessToken)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.registerWithFacebook (handler, handler, fbAccessToken);
	}
	
	IEnumerator CheckShrineActivation (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(CheckShrineActivationHandler, null,request);
		ShrineRequest shrineRequest = (ShrineRequest)request;
		GameManager.instance.fragNetworkingNew.CheckShrineActivation (CheckShrineActivationHandler, CheckShrineActivationHandler, shrineRequest.shrineType, shrineRequest);
		yield return null;
	}

	//This functionality will be taken care of by the server now...
	IEnumerator CheckFriendShrineActivation (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(CheckFriendShrineActivationHandler, null,request);
//		ShrineRequest shrineRequest = (ShrineRequest)request;
//		GameManager.instance.fragNetworking.CheckFriendShrineActivation(response,shrineRequest.shrineType);
		yield return null;
	}
	
	IEnumerator CollectShrineReward (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(CollectShrineRewardHandler, null,request);
		ShrineRequest shrineRequest = (ShrineRequest)request;
		GameManager.instance.fragNetworkingNew.CollectShrineReward (CollectShrineRewardHandler, CollectShrineRewardHandler, shrineRequest.shrineType, request);
		yield return null;
	}
	
	public void CheckShrineActivationHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("CheckShrineActivation");
		if (error == null) {
			IDictionary responseDictionary = responseParameters as IDictionary;
			bool isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			long remaingTime = 0;
//			if (responseDictionary ["remainingTime"] != null) {
//				remaingTime = System.Convert.ToInt64 (responseDictionary ["remainingTime"]);
//			} else {
//				remaingTime = -1;
//			}
////			bool lockedState = System.Convert.ToBoolean (responseDictionary ["locked"]);
			response = new ShrineResponse (request, isSuccess, remaingTime);
			request.Response (response);

		}
	}

	public void CheckFriendShrineActivationHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("CheckShrineActivation");
		if (error == null) {
			if (GameManager.PRINT_LOGS)
				Debug.Log ("CheckShrineActivation::: ");
			IDictionary responseDictionary = responseParameters as IDictionary;
			bool isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);

			if (!isSuccess) {
				int errorCode = System.Convert.ToInt16 (responseDictionary ["errorCode"]);

				if (errorCode == 2) {

//					if(FB.IsLoggedIn)
//					{
//						RegisterWithFacebook(SocialMediaManager.Instance.RegisterWithFacebookHandler,FB.AccessToken);
//					}


				}
			}
			response = new ShrineResponse (request, isSuccess);
			request.Response (response);
			
		}
	}
	
	public void CollectShrineRewardHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		if (GameManager.PRINT_LOGS)
			Debug.Log ("CollectShrineRewardHandler");
		if (error == null) {
			IDictionary responseDictionary = responseParameters as IDictionary;
			bool isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("CollectShrineRewardHandler:" + isSuccess);
//			bool isLocked = System.Convert.ToBoolean (responseDictionary ["locked"]);
			int points = System.Convert.ToInt32 (responseDictionary ["points"]);
//			long remainingTime = System.Convert.ToInt64 (responseDictionary ["remainingTime"]);
			response = new ShrineResponse (request, isSuccess, 0, points);
			request.Response (response);
		}
	}
	
//	public void StartTimer(ServerRequest request)
//	{
//		GameManager.instance.fragNetworking.StartCoroutine(StartTimer(request));
//	}
	
	IEnumerator StartTimer (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (StartTimerHandler, null, request);
		switch (request.RequestType) {
		case ServerRequest.ServerRequestType.Transmutation:
			TransmutationRequest transmuteRequest = (TransmutationRequest)request;
			GameManager.instance.fragNetworkingNew.StartTimer (transmuteRequest.Transmute.TransactionId, transmuteRequest.Transmute.SkullLevel, 
			                                                  TRANSMUTATION, transmuteRequest.Transmute.ItemId, StartTimerHandler, StartTimerHandler, transmuteRequest);
			break;
		case ServerRequest.ServerRequestType.Spirit:
			if (GameManager.PRINT_LOGS)
				Debug.Log ("SERVERMANAGER :::: ServerRequest.ServerRequestType.Spirit - start timer coroutine!!!");
			Debug.LogError("Spirit Start Timer - "+request.Response.ToString());
			SpiritRequest spiritRequest = (SpiritRequest)request;
			GameManager.instance.fragNetworkingNew.StartTimer (spiritRequest.Spirit.TransactionId, spiritRequest.Spirit.SkullLevel, 
			                                                  SPIRIT, spiritRequest.Spirit.ItemId, StartTimerHandler, StartTimerHandler, spiritRequest);
			break;
		case ServerRequest.ServerRequestType.Upgrade:
			UpgradeRequest upgradeRequest = (UpgradeRequest)request;
			Debug.LogError("Upgrades Start Timer - "+request.Response.ToString()+" "+upgradeRequest.Upgrade.ItemId+" "+upgradeRequest.Upgrade.SkullLevel);
			GameManager.instance.fragNetworkingNew.StartTimer (upgradeRequest.Upgrade.TransactionId, upgradeRequest.Upgrade.SkullLevel, 
			                                                  UPGRADE, upgradeRequest.Upgrade.ItemId, StartTimerHandler, StartTimerHandler, upgradeRequest);
			break;
		}
		//
		yield return null;
	}
	
	IEnumerator VerifyTimer (ServerRequest request)
	{
		
//		Networking.RpcResponse response = new Networking.RpcResponse (VerifyTimerHandler, null, request);
		switch (request.RequestType) {
		case ServerRequest.ServerRequestType.TransmutationCompleted:
			TransmutationRequest transmuteRequest = (TransmutationRequest)request;
			GameManager.instance.fragNetworkingNew.VerifyTimer (transmuteRequest.Transmute.TransactionId, transmuteRequest.Transmute.Boost, transmuteRequest.Transmute.BoostTransactionId, VerifyTimerHandler, VerifyTimerHandler, transmuteRequest);
			break;
		case ServerRequest.ServerRequestType.SpiritCompleted:
			SpiritRequest spiritRequest = (SpiritRequest)request;
			GameManager.instance.fragNetworkingNew.VerifyTimer (spiritRequest.Spirit.TransactionId, spiritRequest.Spirit.Boost, spiritRequest.Spirit.BoostTransactionId, VerifyTimerHandler, VerifyTimerHandler, spiritRequest);
			break;
		case ServerRequest.ServerRequestType.UpgradeCompleted:
			UpgradeRequest upgradeRequest = (UpgradeRequest)request;
			GameManager.instance.fragNetworkingNew.VerifyTimer (upgradeRequest.Upgrade.TransactionId, upgradeRequest.Upgrade.Boost, upgradeRequest.Upgrade.BoostTransactionId, VerifyTimerHandler, VerifyTimerHandler, upgradeRequest);
			break;
		}
		
		//
		yield return null;
	}
	
	IEnumerator FetchAllTimer (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(FetchAllTimerHandler, null, request);
		GameManager.instance.fragNetworkingNew.FetchAllTimer (FetchAllTimerHandler, FetchAllTimerHandler, request);		
		yield return null;
	}
		
	IEnumerator GetInAppPurchases(ServerRequest request)
	{
		GameManager.instance.fragNetworkingNew.GetInAppPurchases(GetInAppPurchasesHandler, GetInAppPurchasesHandler, request);		
		yield return null;
	}

	
	IEnumerator VerifyInAppPurchase(ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(VerifyInAppPurchaseHandler, null, request);
		InAppRequest inAppRequest = (InAppRequest)request;
		GameManager.instance.fragNetworkingNew.VerifyInAppPurchase(inAppRequest.ItemId, inAppRequest.Receipt, inAppRequest.Signature, VerifyInAppPurchaseHandler, VerifyInAppPurchaseHandler, inAppRequest);		
		yield return null;
	}
	
	IEnumerator LogTransactions (ServerRequest request)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(LogTransactionsHandler, null, request);
		LogTransactionsRequest logTransactionsRequest = (LogTransactionsRequest)request;
		GameManager.instance.fragNetworkingNew.LogTransactions (logTransactionsRequest.Transactions, LogTransactionsHandler, LogTransactionsHandler, logTransactionsRequest);		
		yield return null;
	}
	
	public void StartTimerHandler (object responseParameters, object error, ServerRequest request)
	{
		Debug.Log ("StartTimerHandler >> request.RequestType >> " + request.RequestType);
		ServerResponse response = null;
		UpdateResponeParam responseParam = null;
		bool isSuccess = false;
		long serverTime = 0;
		if (error == null) {
			IDictionary responseDictionary = responseParameters as IDictionary;
			
			isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			if (isSuccess) {
				serverTime = System.Convert.ToInt64 (responseDictionary ["serverTS"]);
				//if(GameManager.PRINT_LOGS) Debug.Log("Start Timer: Success: " + isSuccess + " ServerTime: " + serverTime);
				IDictionary timer = responseDictionary ["timer"] as IDictionary;
				Debug.Log ("TIMERRR >> " + MiniJSON.Json.Serialize (timer));
				if (timer != null) {
					string transactionId = timer ["transactionId"].ToString ();
					long startTime = System.Convert.ToInt64 (timer ["startTime"]);
					long endTime = System.Convert.ToInt64 (timer ["endTime"]);
					long remainingTime = System.Convert.ToInt64 (timer ["remainingTime"]);
					bool isCompleted = System.Convert.ToBoolean (timer ["isComplete"]);
					int boost = System.Convert.ToInt32 (timer ["boost"]);
					string boostId = timer ["boostId"].ToString ();
					//if(GameManager.PRINT_LOGS) Debug.Log("Start Transmutation: startTime: " + startTime);
					responseParam = new UpdateResponeParam (transactionId, startTime, endTime, remainingTime, isCompleted, boost, boostId);
				}
			}
			
		}
		switch (request.RequestType) {
		case ServerRequest.ServerRequestType.Transmutation:
			if (responseParam == null) {
				TransmutationRequest tRequest = (TransmutationRequest)request;
				responseParam = new UpdateResponeParam (tRequest.Transmute.TransactionId, -1, -1, -1, false, tRequest.Transmute.Boost, tRequest.Transmute.BoostTransactionId);
			}
			response = new TransmuteRespons (request, isSuccess, serverTime, responseParam);
			Debug.Log (response.ToString ());
			break;
		case ServerRequest.ServerRequestType.Upgrade:
			if (responseParam == null) {
				UpgradeRequest uRequest = (UpgradeRequest)request;
				responseParam = new UpdateResponeParam (uRequest.Upgrade.TransactionId, -1, -1, -1, false, uRequest.Upgrade.Boost, uRequest.Upgrade.BoostTransactionId);
			}
			response = new UpgradeResponse (request, isSuccess, serverTime, responseParam);
			break;
		case ServerRequest.ServerRequestType.Spirit:				
			if (responseParam == null) {
				SpiritRequest tRequest = (SpiritRequest)request;
				responseParam = new UpdateResponeParam (tRequest.Spirit.TransactionId, -1, -1, -1, false, tRequest.Spirit.Boost, tRequest.Spirit.BoostTransactionId);
			}
			response = new SpiritResponse (request, isSuccess, serverTime, responseParam);
			break;
		}
		request.Response (response);
	}
	
	public void FetchAllTimerHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		UpdateResponeParam transmutation = null;
		UpdateResponeParam upgrade = null;
		UpdateResponeParam spirit = null;
		if (error == null) {
			IDictionary responseDictionary = responseParameters as IDictionary;
			bool isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			if (isSuccess) {
				long serverTime = System.Convert.ToInt64 (responseDictionary ["serverTimeStamp"]);
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Fetch All Timer: ServerTime: " + serverTime);
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Fetch All Timer: Json: " + Json.Serialize (responseParameters));
				IList timers = responseDictionary ["timers"] as IList;
				if (timers != null && timers.Count > 0) {
					string transactionId = "";
					long startTime = -1;
					long endTime = -1;
					long remainingTime = -1;
					bool isCompleted = false;
					int boost = 0;
					string boostId = "";
					IDictionary timer = null;
					int count = timers.Count;
					for (int i = 0; i < count; i++) {
						timer = timers [i] as IDictionary;
						transactionId = timer ["transactionId"].ToString ();
						startTime = System.Convert.ToInt64 (timer ["startTime"]);
						endTime = System.Convert.ToInt64 (timer ["endTime"]);
						remainingTime = System.Convert.ToInt64 (timer ["remainingTime"]);
						isCompleted = System.Convert.ToBoolean (timer ["isComplete"]);
						boost = System.Convert.ToInt32 (timer ["boost"]);
						boostId = timer ["boostId"].ToString ();
						if (timer ["timerType"].ToString ().Equals ("transmutation")) {
							if (GameManager.PRINT_LOGS)
								Debug.Log ("Transmutation: startTime: " + startTime);
							transmutation = new UpdateResponeParam (transactionId, startTime, endTime, remainingTime, isCompleted, boost, boostId);
						} else if (timer ["timerType"].ToString ().Equals ("upgrade")) {
							if (GameManager.PRINT_LOGS)
								Debug.Log ("Upgrade: startTime: " + startTime);
							upgrade = new UpdateResponeParam (transactionId, startTime, endTime, remainingTime, isCompleted, boost, boostId);
						} else if (timer ["timerType"].ToString ().Equals ("spirit")) {
							if (GameManager.PRINT_LOGS)
								Debug.Log ("Spirit upgrade: startTime: " + startTime);
							spirit = new UpdateResponeParam (transactionId, startTime, endTime, remainingTime, isCompleted, boost, boostId);
						}
					}
				}
				response = new UpdateResponse (request, true, serverTime, transmutation, upgrade, spirit);
			} else {
				response = new UpdateResponse (request, false, 0, transmutation, upgrade, spirit);
			}
		} else {
			response = new UpdateResponse (request, false, 0, transmutation, upgrade, spirit);
		}
		request.Response (response);
	}
	
	public void VerifyTimerHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		UpdateResponeParam responseParam = null;
		bool isSuccess = false;
		long serverTime = 0;
		if (error == null) {
			IDictionary responseDictionary = responseParameters as IDictionary;
			isSuccess = System.Convert.ToBoolean (responseDictionary ["success"].ToString ());
			if (isSuccess) {
				serverTime = System.Convert.ToInt64 (responseDictionary ["serverTimeStamp"]);
				if (GameManager.PRINT_LOGS)
					Debug.Log ("Verify Timer: Success: " + isSuccess + " ServerTime: " + serverTime);
				Dictionary<string, object> timer = responseDictionary ["timer"] as Dictionary<string, object>;
				if (timer != null) {
					string transactionId = timer ["transactionId"].ToString ();
					long startTime = System.Convert.ToInt64 (timer ["startTime"]);
					long endTime = System.Convert.ToInt64 (timer ["endTime"]);
					long remainingTime = System.Convert.ToInt64 (timer ["remainingTime"]);
					bool isCompleted = System.Convert.ToBoolean (timer ["isComplete"]);
					int boost = System.Convert.ToInt32 (timer ["boost"]);
					string boostId = timer ["boostId"].ToString ();
					if (GameManager.PRINT_LOGS)
						Debug.Log ("Verify Transmutation: isComplete: " + isCompleted);
					responseParam = new UpdateResponeParam (transactionId, startTime, endTime, remainingTime, isCompleted, boost, boostId);
				}
			}
		}
		switch (request.RequestType) {
		case ServerRequest.ServerRequestType.TransmutationCompleted:
			response = new TransmuteRespons (request, isSuccess, serverTime, responseParam);
			break;
		case ServerRequest.ServerRequestType.UpgradeCompleted:
			response = new UpgradeResponse (request, isSuccess, serverTime, responseParam);
			break;
		case ServerRequest.ServerRequestType.SpiritCompleted:
			response = new SpiritResponse (request, isSuccess, serverTime, responseParam);
			break;
		}
		request.Response (response);
	}
	
	public void GetInAppPurchasesHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		bool isSuccess = false;
//		Debug.LogError("GetInAppPurchasesHandler "+responseParameters.ToString());
//		if(request == null) {
//			Debug.LogError("request is null - ");
//		}

		if (error == null) {
			Dictionary<string, object> responseDictionary = responseParameters as Dictionary<string, object>;
			if (responseDictionary != null && responseDictionary.ContainsKey ("inapps")) {
				Dictionary<string, object> inAppsDictionary = new Dictionary<string, object> ();
				inAppsDictionary ["inapps"] = responseDictionary ["inapps"];
				string inApps = Json.Serialize (inAppsDictionary);
				//				if(GameManager.PRINT_LOGS) 
				Debug.Log ("GetInAppPurchasesHandler: inapps: " + inApps);
				GameManager.instance._monoHelpers.WriteIntoPersistantDataPath (inApps, PurchaseManager.IN_APP_FILE_NAME);
				isSuccess = true;
			}
		}
		if(request != null) {
			response = new InAppResponse (request, isSuccess, 0, "", 0, "");
			request.Response (response);
		} else {
			request = new InAppRequest(ServerRequest.ServerRequestType.GetInAppPurchases, PurchaseManager.Instance.ProcessResponse);
			response = new InAppResponse (request, true, 0, "", 0, "");
			request.Response (response);
		}
	}
	
	public void VerifyInAppPurchaseHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		bool isSuccess = false;
		bool verified = false;
		string productId = "";
		string itemId = "";
		int itemQuantity = 0;
		InAppRequest inAppRequest = (InAppRequest)request;
		if (error == null) {
			Dictionary<string, object> responseDictionary = responseParameters as Dictionary<string, object>;
			isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			if(isSuccess) {
				verified = System.Convert.ToBoolean (responseDictionary ["verified"]);
				if (verified) {
					itemId = responseDictionary ["itemId"].ToString ();
					itemQuantity = System.Convert.ToInt32 (responseDictionary ["rewardQuantity"]);
					
					if (GameManager.PRINT_LOGS)
						Debug.Log ("VerifyInAppPurchaseHandler: itemId: " + itemId + " itemQuantity: " + itemQuantity);
				}
			} else {
				return;
			}
		}
		response = new InAppResponse (request, verified, 0, itemId, itemQuantity, inAppRequest.TransactionId);
		request.Response (response);
	}
	
	public void LogTransactionsHandler (object responseParameters, object error, ServerRequest request)
	{
		ServerResponse response = null;
		List<string> transactionIds = null;
		bool isSuccess = false;
		if (error == null && request != null) {
			Dictionary<string, object> responseDictionary = responseParameters as Dictionary<string, object>;
			isSuccess = System.Convert.ToBoolean (responseDictionary ["success"]);
			if (!isSuccess) {
				if (responseDictionary.ContainsKey ("transactionIDs")) {
					IList transactionIdsList = responseDictionary ["transactionIDs"] as IList;
					transactionIds = new List<string> ();
					int count = transactionIdsList.Count;
					for (int i = 0; i <  count; i++) {
						transactionIds.Add (transactionIdsList [i] as string);
					}
				}
			}
		}
		response = new LogTransactionsResponse (request, isSuccess, 0, transactionIds);
		request.Response (response);
	}
	
	public static bool IsInternetAvailable ()
	{
//		return (Application.internetReachability != NetworkReachability.NotReachable);
		//		return GameManager.instance.fragNetworkingNew.IsConnected;
		return GameManager.instance.fragNetworkingNew.isInternet;
	}
	
	public void Destroy ()
	{
		if (updateResponseDictionary != null) {
			updateResponseDictionary.Clear ();
		}
		_instance = null;
	}
	
	public void LoadAllRaidBosses (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.GetAllRaidBosses (handler, handler);
	}
	
	public void GetBossBattleResult (bool playerWon, int damage, string bossName, int bossSkullLevel, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.GetBossBattleResult (playerWon, damage, bossName, bossSkullLevel, handler, handler);
	}
	
	public void LootRaidBoss (string bossName, int bossSkullLevel, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.LootRaidBoss (bossName, bossSkullLevel, handler, handler);
	}
	
	public void TellPVPReady (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
//		GameManager.instance.fragNetworking.TellPVPReady (response);
	}
	
	public void QueueForPVP (System.Action<object, object, ServerRequest> handler, PVPServerManager.PVPOpponentFoundDelegate _PVPOpponentDelegate)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
//		GameManager.instance.fragNetworking.QueueForPVP (response, _PVPOpponentDelegate);
	}
	
	public void SubmitPVPTurn (string gesture, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
//		GameManager.instance.fragNetworking.SubmitPVPTurn (gesture, response);
	}
	
	public void CreateGuild (string name, int logoID, bool inviteOnly, string description, float pointsToJoin, int maxMemberLimit, string motd, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.CreateGuild (name, logoID, inviteOnly, description, pointsToJoin, maxMemberLimit, motd, handler, handler);
	}
	
	public void JoinGuild (int guildID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.JoinGuild (guildID, handler, handler);
	}
	
	public void InviteToGuild (int inviteeID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.InviteToGuild (inviteeID, handler, handler);
	}

	public void AcceptInviteToGuild (int guildID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.AcceptInviteToGuild (guildID, handler, handler);
	}
	public void PromoteMember (int promoteeID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.PromoteMember (promoteeID, handler, handler);
	}
	
	public void DemoteMember (int demoteeID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.DemoteMember (demoteeID, handler, handler);
	}

	public void KickMember (int kickUserID, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.KickMember (kickUserID, handler, handler);
	}
	
	public void EditGuildInfo (int guildID, string name, int logoID, bool inviteOnly, string description, int maxMemberLimit, string motd, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.EditGuildInfo (guildID, name, logoID, inviteOnly, description, maxMemberLimit, motd, handler, handler);
	}
	
	public void LeaveGuild (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.LeaveGuild (handler, handler);
	}

	//This call is not needed anymore! As soon as you join guild, you automatically get connected with guild chat.
	public void JoinGuildChat (System.Action<object, object, ServerRequest> handler, GuildSystem.GuildsManager.GuildRawMessagesListener _guildMessagesListener, GuildSystem.GuildsManager.GuildACLUpdateListener _guildACLUpdateListener)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
//		GameManager.instance.fragNetworking.JoinGuildChat (handler, handler, _guildMessagesListener, _guildACLUpdateListener);
	}


	//This is not allowed anymore!!!
	public void LeaveGuildChat (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
//		GameManager.instance.fragNetworking.LeaveGuildChat (response);
	}
	
	public void SendGuildMessage (string message, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.SendGuildMessage (message, handler, handler);
	}



	//This call is not there on server anymore!!!
	public void GetGuildForUserID (int userID, System.Action<object, object, ServerRequest> handler)
	{
		//Debug.LogError()l
		GameManager.instance.fragNetworkingNew.GetGuildForUserID (userID, handler, handler);
	}


	//This call is not there on server anymore!!!
	public void GetGuildForGuildID (int guildID, System.Action<object, object, ServerRequest> handler)
	{
		GameManager.instance.fragNetworkingNew.GetGuildForGuildID (guildID, handler, handler);
	}


	//This call is not there on server anymore!!!
	public void GetGuildMembersForGuildID (int guildID, System.Action<object, object, ServerRequest> handler)
	{
		GameManager.instance.fragNetworkingNew.GetGuildMembersForGuildID (guildID, handler, handler);
	}
	
	public void ListGuilds (string guildNameCriteria, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.ListGuilds (guildNameCriteria, handler, handler);
	}
	
	public void UpdateUserNameForUser (string username, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.UpdateUserNameForUser (username, handler, handler);
	}
	
	public void GetUserNameAndEmail (System.Action<object, object, ServerRequest> handler)
	{
		if (GameManager.PRINT_LOGS)
			Debug.Log ("GetUserNameAndEmail");
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.GetUserNameAndEmail (handler, handler);
	}
	
	public void UpdateUserEmailForUser (string emailAddress, System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.UpdateEmailForUser (emailAddress, handler, handler);
	}
	
	public void GetUserRating (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse (handler, null);
		GameManager.instance.fragNetworkingNew.GetUserRating (handler, handler);
	}
	
	public void GetArcanaLeaderboard (System.Action<object, object, ServerRequest> handler)
	{
//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.GetArcanaLeaderboard (handler, handler);
	}


	public void GetGuildLeaderboard (System.Action<object, object, ServerRequest> handler)
	{
		//		Networking.RpcResponse response = new Networking.RpcResponse(handler, null);
		GameManager.instance.fragNetworkingNew.GetGuildLeaderboard (handler, handler);
	}
	#region CLAN SYSTEM calls

	public void CreateTeam (string teamName, string teamDescription, int teamFlag, int teamType, int requiredTrophies, int warFrequency, string teamLocation, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.CreateTeam (teamName, teamDescription, teamFlag, teamType, requiredTrophies, warFrequency, teamLocation, OnSuccess, OnFailure);
	}
    
	public void ListTeams (Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.ListTeams (OnSuccess = null, OnFailure);
	}
    
	public void GetMyTeam (Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.GetMyTeam (OnSuccess, OnFailure);
	}
    
	public void SearchTeam (string teamName, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.SearchTeam (teamName, OnSuccess, OnFailure);
	}
    
	public void JoinTeam (long teamID, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.JoinTeam (teamID, OnSuccess, OnFailure);
	}
    
	public void LeaveTeam (Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.LeaveTeam (OnSuccess, OnFailure);
	}
    
	public void KickUser (long affecteeID, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.KickUser (affecteeID, OnSuccess, OnFailure);
	}
    
	public void SendGlobalChatMessage (string message, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.SendGlobalChatMessage (message, OnSuccess, OnFailure);
	}
    
	public void SendTeamChat (string message, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.SendTeamChat (message, OnSuccess, OnFailure);
	}
    
	public void GetTeamChats (long userID, Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.GetTeamChats (userID, OnSuccess, OnFailure);
	}

	public void GetGlobalChats (Action<string> OnSuccess = null, Action OnFailure = null)
	{
		GameManager.instance.fragNetworkingNew.GetGlobalChats (OnSuccess, OnFailure);
	}

	#endregion CLAN SYSTEM calls
}
