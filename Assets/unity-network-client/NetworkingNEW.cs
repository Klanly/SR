using UnityEngine;
using System;
using System.Collections;

using AMQP;
using System.Collections.Generic;

using Newtonsoft.Json;
using MiniJSON;
using RabbitMQ.Client;

using Game;
using GuildSystem;
using Game.UI;
using WebSocket_Client;

namespace FRAG
{
	 
	public class NetworkingNEW : Singleton<NetworkingNEW>
	{
		bool isDebugging = false;
//		string userID = "752";
//		string user = "NewTest5";
//		string password = "bb4bfcf4-3b65-4543-a07e-e047a00444d9";
		
//		string userID = "766";
//		string user = "NewTest2";
//		string password = "febbc4d9-a5ff-4596-837c-1b19dfb5c05f";
		
//		string userID = "766";
//		string user = "NewTest3";
//		string password = "f9399805-9f08-497f-8fa6-3045dad674f3";
		
//		string userID = "771";
//		string user = "NewTest6";
//		string password = "95cfa7cd-6205-44ae-8515-4bbf45337948";

		string userID = "779";
		string user = "NewTest9";
		string password = "91d10b6f-c973-4c3b-9570-33c9693b95d3";


		private const int GAME_ID = 1; // not exist
		private const int LOAD_META_DATA = 1100;
		private const int START_TIMER = 1150;
		private const int FETCH_ALL_TIMER = 1151;
		private const int VERIFY_TIMER = 1152; // check it
		private const int UPDATE_USERNAME = 1004;
		private const int LINK_ACCOUNT = 1005;
		private const int UNLINK_ACCOUNT = 1006;
		private const int FIND_ACCOUNT = 1007;
		private const int DEVICE_IDENTIFIER = 1008;

		private const int GET_IN_APP_PURCHASES = 1350; // not exist
		private const int VERIFY_IN_APP_PURCHASE_ITUNES = 1351; // not exist
		private const int VERIFY_IN_APP_PURCHASE_PLAY_STORE = 1352; // not exist
		private const int LOG_TRANSACTIONS = 1200;
		private const int GET_ALL_RAIDBOSSES = 1250;
		private const int BOSS_BATTLE_RESULT = 1251;
		private const int LOOT_RAID_BOSS = 1252;
		private const int PVP_BATTLE_READY = 1018; // not using it, now
		private const int PVP_TURN = 1019; // not using it, now
		private const int PVP_QUEUE = 1017; // not using it, now
		private const int CREATE_GUILD = 2000;
		private const int EDIT_GUILD_DATA = 2013;
		private const int JOIN_GUILD = 2003;
		private const int INVITE_TO_GUILD = 2010;
		private const int ACCEPT_GUILD_INVITE = 2016;
		private const int PROMOTE_GUILD_MEMBER = 2011;
		private const int DEMOTE_GUILD_MEMBER = 2012;
		private const int KICK_GUILD_MEMBER = 2005;
		private const int EDIT_GUILD_INFO = 2013;
		private const int LEAVE_GUILD = 2004;
		private const int INVITE_USER_TO_GUILD = 2010;
		private const int ACCEPT_INVITE_TO_GUILD = 2016;
		private const int SEND_GUILD_MESSAGE = 2006;
		private const int SEND_GLOBAL_MESSAGE = 2008;
		private const int GET_USER_GUILD = 2009;
		private const int SEARCH_GUILD = 2002;
		private const int LIST_GUILDS = 2001;

		private const int GET_GUILD_MESSAGES = 2007;
		private const int GET_GLOBAL_MESSAGES = 2018;
		private const int GET_GUILD_MEMBERS = 2017; 
		private const int GET_ARCANA_LEADERBOARD = 1253;
		private const int GET_GUILD_LEADERBOARD = 1254;
		private const int USER_OFFLINE_RATING = 22; // not implemented on server
		public const string USERID_PREF = "userID";
		public const string USERNAME_PREF = "userName";
		private const string PASSWORD_PREF = "password";


		private const string PRODUCTION_IP = "sr.frag-games.com";
		private const string STAGING_IP = "fbomb.cloudapp.net";

		private const string PRODUCTION_USER = "omar";
		private const string STAGING_USER = "sr";

	


		public Action _noConnectivityListener;
		public Action<object, object, ServerRequest> _loginResponse;

//				public static AMQP.Settings stagingServerNew = new AMQP.Settings("192.168.9.56")	// Talal Laptop
//				public static AMQP.Settings stagingServerNew = new AMQP.Settings ("dev.fbomb.frag-games.com")	// Raheel Laptop
		/*public static AMQP.Settings stagingServerNew = new AMQP.Settings (PRODUCTION_IP)	// online server
					.VirtualHost ("/")
					.Protocol (Protocols.AMQP_0_9_1)
					.Username (PRODUCTION_USER)
					.Password (PRODUCTION_USER);*/

		//TODO Remove later
		// Temporarily using for calculating network and scene loading times
		public static DateTime dt;

		public void Start ()
		{
			//PlayerPrefs.DeleteAll();
			dt = DateTime.Now;
			Connect ();
		}

		public void UpdateLocalUsername (string userName)
		{
			SaveToPref (USERNAME_PREF, userName);
		}


		DateTime lastDateTime = DateTime.MaxValue;

		public void PushToServer(string action, Dictionary<string,object> args)
		{
			WebSocket_Client.Request req = new WebSocket_Client.Request (action,args);
			WebSocket_Client.Client.Instance.OutBoundMessage(req);
		}

		public void ConnectionActionHook(){
			isInternet = true;
			//StartCoroutine("CheckNetworkInactivity");
//			_Register ();
			WebSocket_Client.Client.ResponseProcessRequests.Enqueue (() => {
//				StartCoroutine("CheckNetworkInactivity");
				if (string.IsNullOrEmpty (PlayerPrefs.GetString (USERID_PREF, string.Empty))) {
					Debug.LogError("register called in Networking");
					_Register ();
				} else {
					Debug.LogError("login called in Networking");
					_Login ();
				}
			});
			
			//register for server push messages here, below is just an example
			
			//AMQP.Client.On ("1003", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On("1003", (WebSocket_Client.InBoundMessage message) => {
				lastDateTime = DateTime.Now;
				//					Debug.LogError("Ping received - "+(DateTime.Now - lastDateTime).TotalMilliseconds);
				//*****
				var args = new Dictionary<string, object>();
				//new AMQP.OutboundMessage ("1003", null, "{ }").Send ();

				PushToServer("1003",args);
			});


			
			//AMQP.Client.On ("100001", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100001", (WebSocket_Client.InBoundMessage message) => {
				
				// Patch: To extract the Json of the Reply. This will be deleted after server update.
				Debug.LogError("100001 - "+message.DataAsString);
				//					#region PATCH
				//					string msg = "";
				//					string [] tokens = message.DataAsString.Replace ("\\", "").Split (new char[]
				//                                                                                    {
				//                        '{',
				//                        '}'
				//                    }, StringSplitOptions.RemoveEmptyEntries);
				//					if (tokens.Length < 3)
				//						return;
				//					msg = "{" + tokens [1] + "}";
				//					#endregion PATCH
				//                    
				//					Debug.Log ("Networking.on(100001): " + msg);
				
				
				
				ChatMessageResponse Msg = JsonConvert.DeserializeObject<ChatMessageResponse> (message.DataAsString);
				GuildsManager.Instance.OnGlobalMessageReceived (Msg.chat);
			});
			
			
			//AMQP.Client.On ("100002", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100002", (WebSocket_Client.InBoundMessage message) => {
				
				// Patch: To extract the Json of the Reply. This will be deleted after server update.
				Debug.LogError("100002 - "+message.DataAsString);
				//					#region PATCH
				//					string msg = "";
				//					string [] tokens = message.DataAsString.Replace ("\\", "").Split (new char[]
				//                                                                                    {
				//                        '{',
				//                        '}'
				//                    }, StringSplitOptions.RemoveEmptyEntries);
				//					if (tokens.Length < 3)
				//						return;
				//					msg = "{" + tokens [1] + "}";
				//					#endregion PATCH
				//                    
				//					Debug.Log ("Networking.on(100002): " + msg);
				ChatMessageResponse Msg = JsonConvert.DeserializeObject<ChatMessageResponse> (message.DataAsString);
				GuildsManager.Instance.OnGuildMessageReceived (Msg.chat);
			});
			
			//AMQP.Client.On ("100005", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100005", (WebSocket_Client.InBoundMessage message) => {
				//User Kicked
				Debug.LogError ("Networking.on(100005): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string,object>;
				var dict = dictionary["team_user"] as Dictionary<string, object>;
				string userString = JsonConvert.SerializeObject(dict);
				ClanUIHandler.instance.OnUserStatusChangedReceived(userString, true);
			});
			
			//AMQP.Client.On ("100006", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100006", (WebSocket_Client.InBoundMessage message) => { 
				//User Left
				Debug.LogError ("Networking.on(100006): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string,object>;
				var dict = dictionary["team_user"] as Dictionary<string, object>;
				string userString = JsonConvert.SerializeObject(dict);
				ClanUIHandler.instance.OnUserStatusChangedReceived(userString, true);
			});
			
			//AMQP.Client.On ("100007", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100007", (WebSocket_Client.InBoundMessage message) => { 
				//User Join
				Debug.LogError ("Networking.on(100007): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string,object>;
				var dict = dictionary["team_user"] as Dictionary<string, object>;
				string userString = JsonConvert.SerializeObject(dict);
				ClanUIHandler.instance.OnUserStatusChangedReceived(userString, true);
			});
			
			//AMQP.Client.On ("100009", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100009", (WebSocket_Client.InBoundMessage message) => {
				//On Promote
				
				Debug.LogError ("Networking.on(100009): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string, object>;
				var dict = dictionary["team_user"] as Dictionary<string, object>;
				string userString = JsonConvert.SerializeObject(dict);
				ClanUIHandler.instance.OnUserStatusChangedReceived(userString, true);
				
			});
			
			//AMQP.Client.On ("100010", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100010", (WebSocket_Client.InBoundMessage message) => {
				//On Demote
				Debug.LogError ("Networking.on(100010): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string, object>;
				var dict = dictionary["team_user"] as Dictionary<string, object>;
				string userString = JsonConvert.SerializeObject(dict);
				ClanUIHandler.instance.OnUserStatusChangedReceived(userString, true);
				
			});
			//AMQP.Client.On ("100011", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("100011", (WebSocket_Client.InBoundMessage message) => {
				//On Demote
				Debug.LogError ("Networking.on(100011): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string, object>;
				var points = dictionary["points"].ToString();
				var key = dictionary["key"].ToString();
				Debug.LogError("points - "+points+" keys = "+key);
				Shrine shrineToUpdate = ShrineManager.Instance.GetShrineForLevel(key);
				shrineToUpdate.guildShrinePoints = int.Parse(points);;
				ShrineManager.Instance.SetShrineLevel(shrineToUpdate.Type);
				GameManager.instance.scaleformCamera.generalSwf.SetShrineLevel(shrineToUpdate.shrineLevel);
				GameManager.instance.scaleformCamera.generalSwf.SetShrineBar(shrineToUpdate.guildShrinePoints, shrineToUpdate.maxShrinePoints);
				
			});
			//AMQP.Client.On ("2201", (AMQP.InboundMessage message) => {
			WebSocket_Client.Client.Instance.On ("2201", (WebSocket_Client.InBoundMessage message) => {
				//On Demote
				Debug.LogError ("Networking.on(2201): " + message.DataAsString);
				var dictionary = Json.Deserialize (message.DataAsString) as Dictionary<string, object>;
				var msg = dictionary["message"].ToString();
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Attention!", msg, () => {});
			});
		
		}
		public void Connect ()
		{
			Debug.LogError("Connect called");
			StopCoroutine("CheckNetworkInactivity");
			lastDateTime = DateTime.MaxValue;
			// connect to AMQP server
			WebSocket_Client.Client.OnConnected += ConnectionActionHook;
			WebSocket_Client.Client.Instance.Connect(OnInternetConnectionFailed);

		}
	
		private void OnInternetConnectionFailed (object sender, EventArgs e)
		{
			WebSocket_Client.Client.ResponseProcessRequests.Enqueue (() => {
				_noConnectivityListener();
			});
		}

		public bool isInternet = false;
		IEnumerator CheckNetworkInactivity() {
//			yield break;
			while(true) {
				var diff = DateTime.Now - lastDateTime;
				Debug.LogWarning("diff - "+diff.TotalMilliseconds +" "+WebSocket_Client.Client.Instance.IsConnected());
				if(diff.TotalMilliseconds > 12000.0f) {	//10 sec with a buffer of 2 sec
					isInternet = false;
					// Try logging again
					Debug.LogWarning("calling disconnect after 1");
//					yield return new WaitForSeconds(1.0f);

					WebSocket_Client.Client.Instance.Disconnect();
					//					Debug.LogWarning("calling connect after 1 sec");
					yield return new WaitForSeconds(10.0f);
					lastDateTime = DateTime.MaxValue;
					Connect();
//					yield break;
				} else {
					isInternet = true;
				}
				yield return new WaitForSeconds(5.0f);
//				Debug.LogWarning("connection status - AMQP "+IsConnected);
			}
		}



		public string GetUserID ()
		{
			if(isDebugging) {
				return userID;
			} else
				return PlayerPrefs.GetString (USERID_PREF, string.Empty);
		}

		private void _Register ()
		{
			//string json = Json.Serialize (new Dictionary<string, object> ()); //empty params dictionary
			var args = new Dictionary<string, object> ();

			/*new AMQP.Login ("1050", null, json)
	            .OnReply ((request, reply) => {*/

			new WebSocket_Client.Request("1050",args)
				.OnReply((reply)=>{

				var response = Json.Deserialize (reply.DataAsString) as Dictionary<string,object>;
				bool success = (bool)response ["success"];
				Debug.Log ("RegisterResponse >> " + success);
				if (success) {
					//AMQP.Client.CommunicationExchange = response ["exchange"].ToString ();
					string userID = response [USERID_PREF].ToString ();
					string username = response [USERNAME_PREF].ToString ();
					string password = response [PASSWORD_PREF].ToString ();

					if (string.IsNullOrEmpty (GetPrefs (USERID_PREF))) {
						SaveToPref (USERID_PREF, userID);
						SaveToPref (PASSWORD_PREF, password);
						SaveToPref (USERNAME_PREF, username);
					}

					_Login ();
				}
			})
	                .Send ();
		}

		public bool IsConnected {
			get {
				//return Client.IsConnected;
				return WebSocket_Client.Client.Instance.IsConnected();
			}
		}

		private string GetPrefs (string key)
		{
			return PlayerPrefs.GetString (key, string.Empty);
		}

		private void SaveToPref (string key, string val)
		{
			PlayerPrefs.SetString (key, val);
		}

		private void _Login ()
		{
			var args = new Dictionary<string, object> ();


			if(isDebugging) {
				args.Add(USERNAME_PREF, user);
				args.Add(PASSWORD_PREF, password);
			} else {
				args.Add (USERNAME_PREF, GetPrefs (USERNAME_PREF));
				args.Add (PASSWORD_PREF, GetPrefs (PASSWORD_PREF));
			}


			//string json = Json.Serialize (args);
			/*new AMQP.Login ("1001", null, json)
				.OnReply ((request, reply) => {*/

			new WebSocket_Client.Request("1001",  args)
				.OnReply(( reply) => {
				var response = Json.Deserialize (reply.DataAsString) as Dictionary<string,object>;
				bool success = (bool)response ["success"];
				Debug.Log ("Login Success => " + success);
					Debug.LogError("login response - "+reply.DataAsString);
				if (success) {
					//AMQP.Client.CommunicationExchange = response ["exchange"].ToString ();
					StartCoroutine("CheckNetworkInactivity");

					string userID = response [USERID_PREF].ToString ();
					_loginResponse (response, null, null);
					///write your code here

						Debug.LogError("login response success - "+userID +" and starting coroutine for network inactivity");
					} else {
					_loginResponse (response, response, null);
				}
			})
	                .Send ();
		}
		
		private void GeneralQuery (string debugString, string actionID, Dictionary<string, object> args, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			/*new AMQP.Request (actionID, null, json, null)
                .OnReply ((request, reply) => {*/
			new WebSocket_Client.Request(actionID, args)
				.OnReply((reply) => {
				try {
					Debug.Log ("~~~ Successfully " + debugString + " on Server: " + reply.DataAsString);
					var response = JsonConvert.DeserializeObject<Dictionary<string,object>> (reply.DataAsString);
					bool success = (bool)response ["success"];
					if (success && OnSuccess != null) {
						OnSuccess (reply.DataAsString);
					} else if (OnFailure != null) {
						OnFailure ();
						//GameManager.instance.ShowSyncPopUp();
					}
				} catch (System.Exception _ex) {
					Debug.LogError ("GeneralQuery " + debugString + "-Exception:  " + _ex.Message);
				}
			})
                /*    .OnFailed ((AMQP.OutboundMessage message) => {
				Debug.Log ("... Failed " + debugString + " on Server: " + message.DataAsString);
				if (OnFailure != null) {
					OnFailure ();
					//GameManager.instance.ShowSyncPopUp();
				}
			})*/
                    .Send ();
		}


		//*** commenting this and using new SendRequestfunction **//
		private void SendRequest (string debugString, string actionID, Dictionary<string, object> args, Action<object, object, ServerRequest> OnSuccess = null, Action<object, object, ServerRequest> OnFailure = null, ServerRequest serverRequest = null)
		{
			//Debug.Log("Request Sent : "+json);
			initTime = System.DateTime.Now.Millisecond;
			new WebSocket_Client.Request(actionID, args)
				.OnReply((reply) => {
			/*new AMQP.Request (actionID.ToString (), null, json, serverRequest)
				.OnReply ((request, reply) => {*/
//					Debug.LogError("Response receieved : "+reply.DataAsString);

				try {
					Debug.Log ("~~~ Successfully " + debugString + " on Server: " + reply.DataAsString);
					var response = Json.Deserialize (reply.DataAsString) as IDictionary;
					bool success = bool.Parse (response ["success"].ToString ());
					//                    bool success = (response.Contains("success")) ? bool.Parse(response ["success"].ToString()) : false;
					if (success && OnSuccess != null) {
							OnSuccess (response,null,serverRequest);
						//OnSuccess (response, null, request.serverRequest);
					} else if (OnFailure != null) {
						var error = new Dictionary<string,object> ();
						error.Add ("error", reply.DataAsString);
							OnFailure(response, null, serverRequest);
						//OnFailure (response, null, request.serverRequest);
					}
				} catch (System.Exception _ex) {
					Debug.LogError ("SendRequest " + debugString + "-Exception:  " + _ex.Message + "\nReply: " + reply.DataAsString + "\nStackTrace: " + _ex.StackTrace + "   <====");
				}
				}).OnFailure((failedResponse)=>{
//					Debug.LogError("Failed Response receieved : "+failedResponse);
//					OnFailure(failedResponse, null, serverRequest);
					if(OnFailure == null) {
						Debug.LogError("OnFailure is null");
					}
					if(serverRequest == null) {
						Debug.LogError("serverRequest is null");
					}
					if(OnFailure != null)
						OnFailure (failedResponse, new object(), serverRequest);

				})
					.Send ();
		}

		/***private void SendRequest (string debugString, int actionID, string json, Action<object, object, ServerRequest> OnSuccess = null, Action<object, object, ServerRequest> OnFailure = null, ServerRequest serverRequest = null)
		{
			Debug.Log("Request Sent : "+json);
			initTime = System.DateTime.Now.Millisecond;
			new AMQP.Request (actionID.ToString (), null, json, serverRequest)
					.OnReply ((request, reply) => {
				try {
//					Debug.LogError("time taken = "+(System.DateTime.Now.Millisecond - initTime) + " action iD = "+actionID);
					
					Debug.Log ("~~~ Successfully " + debugString + " on Server: " + reply.DataAsString);
					var response = Json.Deserialize (reply.DataAsString) as IDictionary;
					bool success = bool.Parse (response ["success"].ToString ());
//                    bool success = (response.Contains("success")) ? bool.Parse(response ["success"].ToString()) : false;
					if (success && OnSuccess != null) {
						OnSuccess (response, null, request.serverRequest);
					} else if (OnFailure != null) {
						var error = new Dictionary<string,object> ();
						error.Add ("error", reply.DataAsString);
						OnFailure (response, null, request.serverRequest);
					}
				} catch (System.Exception _ex) {
					Debug.LogError ("SendRequest " + debugString + "-Exception:  " + _ex.Message + "\nReply: " + reply.DataAsString + "\nStackTrace: " + _ex.StackTrace + "   <====");
				}
			})***/
                   /* .OnFailed ((AMQP.OutboundMessage message) => {
				Debug.Log ("... Failed " + debugString + " on Server: " + message.DataAsString);
				if (OnFailure != null) {
					var error = new Dictionary<string,object> ();
					error.Add ("error", "Connection dropped.");
					OnFailure (Json.Serialize (error), null, null);
				}
			})*/

/*                Debug.Log("~~~ Successfully " + debugString + " on Server: " + reply.DataAsString);
                var response = Json.Deserialize(reply.DataAsString)  as IDictionary;
//				if (OnSuccess != null) {
//						OnSuccess (response, null, null);
//				}

                if (request.Action == "2009")
                {
//						//Case for null response
//						OnSuccess(null, new object(), request.serverRequest);

                    //Case for empty guild
                    response.Add("error_code", 10);
                    OnSuccess(response, new object(), request.serverRequest);
                }


                bool success = (bool)response ["success"];
                if (success && OnSuccess != null)
                {
                    OnSuccess(response, null, request.serverRequest);
                } else if (OnFailure != null)
                {
                    var error = new Dictionary<string,object>();
                    error.Add("error", reply.DataAsString);
                    OnFailure(Json.Serialize(error), null, request.serverRequest);
                }
            })
						.OnFailed((AMQP.OutboundMessage message) => {
                Debug.Log("... Failed " + debugString + " on Server: " + message.DataAsString);

                if (OnFailure != null)
                {
                    var error = new Dictionary<string,object>();
                    error.Add("error", "Connection dropped.");
                    OnFailure(Json.Serialize(error), null, null);
                }
            })*/
	                   /***** .Send ();

		}****/

		//
		// PUBLIC METHODS
		//
//		public bool GameLogicPing ()
//		{
//			var args = new Dictionary<string, object> ();
//			var payload = new Dictionary<string, object> ();
//			payload.Add ("atif", "creed 4");
//			payload.Add ("hmm", 123);
//			
//			args.Add ("payload", payload);
//			string json = Json.Serialize (args);
//			//RpcResponse response = new RpcResponse(GameLogicPingHandler, null);
//			return SendRequest ("GameLogicPing", 1001, json, GameLogicPingHandler, GameLogicPingHandler);
//		}
		
//		public void GameLogicPingHandler (object response, object error, ServerRequest request)
//		{
//			if (error != null) {
//				if (GameManager.PRINT_LOGS)
//					Debug.Log ("AMQP: got game logic ping reply error: " + error);
//			}
//		}
		
//		public bool ServicesPing ()
//		{
//			var args = new Dictionary<string, object> ();
//			var payload = new Dictionary<string, object> ();
//			payload.Add ("message", "services, yo 4");
//			payload.Add ("pi", 3.14);
//			
//			args.Add ("payload", payload);
//			string json = Json.Serialize (args);
//			RpcResponse response = new RpcResponse(GameLogicPingHandler, null);
//			return SendRequest ("ServicesPing", 1, json, ServicesPingHandler, ServicesPingHandler);
//		}
//		
//		public void ServicesPingHandler (object response, object error, ServerRequest request)
//		{
//			if (error != null) {
//				if (GameManager.PRINT_LOGS)
//					Debug.Log ("AMQP: got services ping reply error: " + error);
//			}
//		}

		public void CheckPendingTransactions ()
		{
			StartCoroutine (PurchaseManager.Instance.CheckPendingTransactions ());
		}

		int initTime = 0;

		public void LoadMetaData (string version, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
//			args.Add ("version", version);
			//string json = Json.Serialize (args);
			//SendRequest ("LoadMetaData", LOAD_META_DATA, json, OnSuccess, OnFailure);
			SendRequest ("LoadMetaData", LOAD_META_DATA.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("LoadMetaData", LOAD_META_DATA.ToString (), args, OnSuccess, OnFailure);
			initTime = System.DateTime.Now.Second;
		}

	
		
		public void LoadGameState (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();

			//string json = Json.Serialize (args);
			int actionNo = 1051;
			//SendRequest ("LoadGameState", 1051, json, OnSuccess, OnFailure);
			SendRequest ("LoadGameState", actionNo.ToString(),args, OnSuccess, OnFailure);
			//GeneralQuery ("LoadGameState", actionNo.ToString(), args, OnSuccess, OnFailure);
		}

//		IEnumerator SaveGameStateRoutine (IDictionary state, Action<string> OnSuccess = null)
//		{
//			var args = new Dictionary<string, object> ();
//			args.Add ("state", state);
//			string json = Json.Serialize (args);
//			yield return SendRequest ("SaveGameStateRoutine", 1003, json, OnSuccess);
//		}
		
		public void SaveGameState (IDictionary stateDictionary, Action<object, object, ServerRequest> OnSuccess = null, Action<object, object, ServerRequest> OnFailure = null)
		{
//			StartCoroutine (SaveGameStateRoutine (state, OnSuccess));

			var args = new Dictionary<string, object> ();
			args.Add ("state", stateDictionary);
			//string json = Json.Serialize (args);
			//SendRequest ("SaveGameState", 1053, json, OnSuccess, OnFailure);
			int actionNo = 1053;
			SendRequest ("SaveGameState", actionNo.ToString(),args, OnSuccess, OnFailure);
			//GeneralQuery ("SaveGameState", actionNo.ToString(), args, OnSuccess, OnFailure);
			initTime = System.DateTime.Now.Second;
		}

		public void SetDeviceIdentifier (string deviceId,int deviceType, Action<object, object, ServerRequest> OnSuccess = null, Action<object, object, ServerRequest> OnFailure = null)
		{
			//			StartCoroutine (SaveGameStateRoutine (state, OnSuccess));
			
			var args = new Dictionary<string, object> ();
			args.Add ("deviceIdentifier", deviceId);
			args.Add ("deviceType", deviceType);
			//string json = Json.Serialize (args);
			//SendRequest ("SetDeviceIdentifier", DEVICE_IDENTIFIER, json, OnSuccess, OnFailure);
			SendRequest ("SetDeviceIdentifier", DEVICE_IDENTIFIER.ToString(), args, OnSuccess, OnFailure);
			// ("SetDeviceIdentifier", DEVICE_IDENTIFIER.ToString(), args, OnSuccess, OnFailure);
		}

		public void LinkAccount (string playerId, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			#if  UNITY_EDITOR_OSX || UNITY_EDITOR
			args.Add ("accessToken", playerId);
			args.Add ("accountType", "itune");
			args.Add ("iTuneId", playerId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			args.Add ("accessToken", playerId);
			args.Add ("accountType", "google");
			args.Add ("googleId", playerId);
			#elif UNITY_IPHONE && !UNITY_EDITOR
			args.Add ("accessToken", playerId);
			args.Add ("accountType", "itune");
			args.Add ("iTuneId", playerId);
			
			#endif	
			
			//string json = Json.Serialize (args);
			//Debug.LogError("Link account "+json);
			//SendRequest ("LinkAccount", LINK_ACCOUNT, json, OnSuccess, OnFailure);
			SendRequest ("LinkAccount", LINK_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery("LinkAccount", LINK_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
		}

		public void UnlinkAccount (string playerId, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			#if  UNITY_EDITOR_OSX || UNITY_EDITOR
			args.Add ("accountType", "itune");
			args.Add ("ituneid", playerId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			args.Add ("accountType", "google");
			args.Add ("googleId", playerId);
			#elif UNITY_IPHONE && !UNITY_EDITOR
			args.Add ("accountType", "itune");
			args.Add ("ituneid", playerId);
			#endif	
			
			//string json = Json.Serialize (args);
			//SendRequest ("UnlinkAccount", UNLINK_ACCOUNT, json, OnSuccess, OnFailure);
			SendRequest ("UnlinkAccount", UNLINK_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("UnlinkAccount", UNLINK_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
		}

		public void FindAccount (string playerId, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			#if  UNITY_EDITOR_OSX || UNITY_EDITOR
			args.Add ("accountType", "itune");
			args.Add ("iTuneId", playerId);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			args.Add ("accountType", "google");
			args.Add ("googleId", playerId);
			#elif UNITY_IPHONE && !UNITY_EDITOR
			args.Add ("accountType", "itune");
			args.Add ("iTuneId", playerId);
			#endif	
			
			//string json = Json.Serialize (args);
			//SendRequest ("FindAccount", FIND_ACCOUNT, json, OnSuccess, OnFailure);
			SendRequest ("FindAccount", FIND_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("FindAccount", FIND_ACCOUNT.ToString(), args, OnSuccess, OnFailure);
		}

		public void StartTimer (string transactionId, int skullLevel, string type, string itemId, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			Debug.LogError ("StartTimer CALLED with params : transactionId" + transactionId + " skullLevel " + skullLevel + " type " + type+" itemId "+itemId);
			var args = new Dictionary<string, object> ();
			args.Add ("transactionId", transactionId);
			args.Add ("skullLevel", skullLevel);
			args.Add ("timerType", type);
			args.Add ("itemId", itemId);
			//string json = Json.Serialize (args);
			//SendRequest ("StartTimer", START_TIMER, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("StartTimer", START_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("StartTimer", START_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}

		public void FetchAllTimer (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);
			//SendRequest ("FetchAllTimer", FETCH_ALL_TIMER, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("FetchAllTimer", FETCH_ALL_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("FetchAllTimer", FETCH_ALL_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}

		public void VerifyTimer (string transactionId, int boost, string boostId, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("transactionId", transactionId);
			args.Add ("boost", boost);
			args.Add ("boostId", boostId);
			
			//string json = Json.Serialize (args);
			//SendRequest ("VerifyTimer", VERIFY_TIMER, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("VerifyTimer", VERIFY_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("VerifyTimer", VERIFY_TIMER.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}

		public void LogTransactions (List<Dictionary<string, object>> transactions, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			if (GameManager.PRINT_LOGS)
				Debug.Log ("Transactions:  " + transactions.Count);
			args.Add ("transactions", transactions);
			if (GameManager.PRINT_LOGS)
				Debug.Log ("TransactionsJson==" + Json.Serialize (transactions));
			//string json = Json.Serialize (args);
			//SendRequest ("LogTransactions", LOG_TRANSACTIONS, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("LogTransactions", LOG_TRANSACTIONS.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("LogTransactions", LOG_TRANSACTIONS.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}

		public void GetShrineInformation (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);
			//SendRequest ("GetShrineInformation", 1300, json, OnSuccess, OnFailure);
			int actionNo = 1300;
			SendRequest ("GetShrineInformation", actionNo.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GetShrineInformation", actionNo.ToString(), args, OnSuccess, OnFailure);
		}


//		public void ActivateShrine (string element, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
//		{
//			var args = new Dictionary<string, object> ();
//			args.Add ("element", element);
//			string json = Json.Serialize (args);
//			SendRequest ("Charge Shrine", 1301, json, OnSuccess, OnFailure);
//		}

		public void CheckShrineActivation (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, string element, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("element", element);
			//string json = Json.Serialize (args);
			//SendRequest ("CheckShrineActivation", 1301, json, OnSuccess, OnFailure, serverRequest);
			int actionNo = 1301;
			SendRequest ("CheckShrineActivation", actionNo.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("CheckShrineActivation", actionNo.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}


		public void UpdateUserNameForUser (string username, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["userName"] = username;
			
			//string json = Json.Serialize (args);
			//SendRequest ("UpdateUserName", UPDATE_USERNAME, json, OnSuccess, OnFailure);
			SendRequest ("UpdateUserName", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("UpdateUserName", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
		}


		public void GetAllRaidBosses (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);
			//SendRequest ("GetAllRaidBosses", GET_ALL_RAIDBOSSES, json, OnSuccess, OnFailure);
			SendRequest ("GetAllRaidBosses", GET_ALL_RAIDBOSSES.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GetAllRaidBosses", GET_ALL_RAIDBOSSES.ToString(), args, OnSuccess, OnFailure);
		}


		public void GetBossBattleResult (bool bossDefeated, int damageDoneToBoss, string bossName, int bossSkullLevel, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["bossDefeated"] = bossDefeated;
			args ["damage"] = damageDoneToBoss;
			args ["name"] = bossName;
			args ["skullLevel"] = bossSkullLevel;
			//string json = Json.Serialize (args);
			//SendRequest ("RaidBossBattleResult", BOSS_BATTLE_RESULT, json, OnSuccess, OnFailure);
			SendRequest ("RaidBossBattleResult", BOSS_BATTLE_RESULT.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("RaidBossBattleResult", BOSS_BATTLE_RESULT.ToString(), args, OnSuccess, OnFailure);
		}

		public void GetInAppPurchases (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("gameID", GAME_ID);
			//string json = Json.Serialize (args);
			//SendRequest ("GetInAppPurchases", GET_IN_APP_PURCHASES, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("GetInAppPurchases", GET_IN_APP_PURCHASES.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery("GetInAppPurchases", GET_IN_APP_PURCHASES.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}


		public void VerifyInAppPurchase (string itemId, string receipt, string signature, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
//			args.Add ("gameID", GAME_ID);
			args.Add ("itemId", itemId);
			int actionCode = 0;
			#if  UNITY_EDITOR_OSX || UNITY_EDITOR
			actionCode = VERIFY_IN_APP_PURCHASE_ITUNES;
			args.Add ("receipt", receipt);
			#elif UNITY_ANDROID && !UNITY_EDITOR
//			args.Add("storeType", StoreType.google_play.ToString());
			actionCode = VERIFY_IN_APP_PURCHASE_PLAY_STORE;
			args.Add ("signedData", receipt);
			args.Add ("signature", signature);
			#elif UNITY_IPHONE && !UNITY_EDITOR
			actionCode = VERIFY_IN_APP_PURCHASE_ITUNES;
			args.Add ("receipt", receipt);
//			args.Add("storeType", StoreType.itunes.ToString());
			#endif
			//string json = Json.Serialize (args);
//			Debug.LogError("New Verify Inapp Purchase - receipt "+receipt);
			//SendRequest ("VerifyInAppPurchase", actionCode, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("VerifyInAppPurchase", actionCode.ToString(), args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("VerifyInAppPurchase", actionCode.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}


		public void CollectShrineReward (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, string type, ServerRequest serverRequest)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("element", type);
			//string json = Json.Serialize (args);
			int actionNo = 1302;
			//SendRequest ("CollectShrineReward", 1302, json, OnSuccess, OnFailure, serverRequest);
			SendRequest ("CollectShrineReward", actionNo.ToString(),args, OnSuccess, OnFailure, serverRequest);
			//GeneralQuery ("CollectShrineReward", actionNo.ToString(), args, OnSuccess, OnFailure, serverRequest);
		}


		public void registerWithFacebook (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure, string accessToken)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("fbAccessToken", accessToken);
			
			//string json = Json.Serialize (args);
			//SendRequest ("registerWithFacebook", 5, json, OnSuccess, OnFailure);
			int actionNo = 5;
			SendRequest ("registerWithFacebook", actionNo.ToString(),args, OnSuccess, OnFailure);
			//GeneralQuery ("registerWithFacebook", actionNo.ToString(), args, OnSuccess, OnFailure);
		}


		public void GetArcanaLeaderboard (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			//string json = Json.Serialize (args);
			//SendRequest ("GET_ARCANA_LEADERBOARD", GET_ARCANA_LEADERBOARD, json, OnSuccess, OnFailure);
			SendRequest ("GET_ARCANA_LEADERBOARD", GET_ARCANA_LEADERBOARD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GET_ARCANA_LEADERBOARD", GET_ARCANA_LEADERBOARD.ToString(), args, OnSuccess, OnFailure);
		}

		public void GetGuildLeaderboard (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			//string json = Json.Serialize (args);
			//SendRequest ("GET_GUILD_LEADERBOARD", GET_GUILD_LEADERBOARD, json, OnSuccess, OnFailure);
			SendRequest ("GET_GUILD_LEADERBOARD", GET_GUILD_LEADERBOARD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GET_GUILD_LEADERBOARD", GET_GUILD_LEADERBOARD.ToString(), args, OnSuccess, OnFailure);
		}

		public void GetMyGuild (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);

			//SendRequest ("GET_MY_GUILD", 2009, json, OnSuccess, OnFailure);
			int actionNo = 2009;
			SendRequest ("GET_MY_GUILD", actionNo.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery("GET_MY_GUILD", actionNo.ToString(), args, OnSuccess, OnFailure);
		}


		public void GetGuildForUserID (int userID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["targetUserID"] = userID;
			
			//string json = Json.Serialize (args);
			//SendRequest ("GET_USER_GUILD", GET_USER_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("GET_USER_GUILD", GET_USER_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GET_USER_GUILD", GET_USER_GUILD.ToString(), args, OnSuccess, OnFailure);
		}



		public void JoinGuild (int guildID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;

			//string json = Json.Serialize (args);

			//SendRequest ("JOIN_GUILD", JOIN_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("JOIN_GUILD", JOIN_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery("JOIN_GUILD", JOIN_GUILD.ToString(), args, OnSuccess, OnFailure);
		}


		public void ListGuilds (string guildNameCriteria, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["guildNameCriteria"] = guildNameCriteria;
			
			//string json = Json.Serialize (args);
			//SendRequest ("LIST_GUILDS", LIST_GUILDS, json, OnSuccess, OnFailure);
			SendRequest ("LIST_GUILDS", LIST_GUILDS.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("LIST_GUILDS", LIST_GUILDS.ToString(), args, OnSuccess, OnFailure);
		}

		public void LootRaidBoss (string name, int skullLevel, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["name"] = name;
			args ["skullLevel"] = skullLevel;
			//string json = Json.Serialize (args);
			//SendRequest ("LOOT_RAID_BOSS", LOOT_RAID_BOSS, json, OnSuccess, OnFailure);
			SendRequest ("LOOT_RAID_BOSS", LOOT_RAID_BOSS.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("LOOT_RAID_BOSS", LOOT_RAID_BOSS.ToString(), args, OnSuccess, OnFailure);
		}


		public void CreateGuild (string name, int logoID, bool inviteOnly, string description, float pointsToJoin, int maxMemberLimit, string motd, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.NAME] = name;
			args [GuildSystem.Constants.GUILD_OBJ_LOGO_ID] = logoID;
			args [GuildSystem.Constants.GUILD_OBJ_INVITE_ONLY] = inviteOnly;
			args [GuildSystem.Constants.DESCRIPTION] = description;
			args [GuildSystem.Constants.GUILD_OBJ_MAX_MEMBERS] = maxMemberLimit;
			args [GuildSystem.Constants.GUILD_OBJ_JOIN_COST] = pointsToJoin;
			args [GuildSystem.Constants.MOTD] = motd;
			//string json = Json.Serialize (args);
			//SendRequest ("CREATE_GUILD", CREATE_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("CREATE_GUILD", CREATE_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery("CREATE_GUILD", CREATE_GUILD.ToString(), args, OnSuccess, OnFailure);
			
		}


		public void InviteToGuild (int inviteeID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.INVITEE_ID] = inviteeID;
			//string json = Json.Serialize (args);
			//SendRequest ("INVITE_TO_GUILD", INVITE_TO_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("INVITE_TO_GUILD", INVITE_TO_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("INVITE_TO_GUILD", INVITE_TO_GUILD.ToString(), args, OnSuccess, OnFailure);
		}

		public void AcceptInviteToGuild (int inviteeID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.INVITEE_ID] = inviteeID;
			//string json = Json.Serialize (args);
			//SendRequest ("ACCEPT_GUILD_INVITE", ACCEPT_GUILD_INVITE, json, OnSuccess, OnFailure);
			SendRequest ("ACCEPT_GUILD_INVITE", ACCEPT_GUILD_INVITE.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("ACCEPT_GUILD_INVITE", ACCEPT_GUILD_INVITE.ToString(), args, OnSuccess, OnFailure);
		}


		public void PromoteMember (int promoteeID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.PROMOTEE_ID] = promoteeID;
			//string json = Json.Serialize (args);
			//SendRequest ("PROMOTE_GUILD_MEMBER", PROMOTE_GUILD_MEMBER, json, OnSuccess, OnFailure);
			SendRequest ("PROMOTE_GUILD_MEMBER", PROMOTE_GUILD_MEMBER.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("PROMOTE_GUILD_MEMBER", PROMOTE_GUILD_MEMBER.ToString(),args, OnSuccess, OnFailure);
		}



		public void DemoteMember (int demoteeID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.DEMOTEE_ID] = demoteeID;
			//string json = Json.Serialize (args);
			//SendRequest ("DEMOTE_GUILD_MEMBER", DEMOTE_GUILD_MEMBER, json, OnSuccess, OnFailure);
			SendRequest ("DEMOTE_GUILD_MEMBER", DEMOTE_GUILD_MEMBER.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("DEMOTE_GUILD_MEMBER", DEMOTE_GUILD_MEMBER.ToString(), args, OnSuccess, OnFailure);
		}


		public void KickMember (int kickUserID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.KICK_USER_ID] = kickUserID;
			//string json = Json.Serialize (args);
			//SendRequest ("KICK_GUILD_MEMBER", KICK_GUILD_MEMBER, json, OnSuccess, OnFailure);
			SendRequest ("KICK_GUILD_MEMBER", KICK_GUILD_MEMBER.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("KICK_GUILD_MEMBER", KICK_GUILD_MEMBER.ToString(), args, OnSuccess, OnFailure);
		}


		public void EditGuildInfo (int guildID, string name, int logoID, bool inviteOnly, string description, int maxMemberLimit, string motd, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;
			args [GuildSystem.Constants.NAME] = name;
			args [GuildSystem.Constants.LOGO_ID] = logoID;
			args [GuildSystem.Constants.INVITE_ONLY] = inviteOnly;
			args [GuildSystem.Constants.DESCRIPTION] = description;
			args [GuildSystem.Constants.MAX_MEMBER_LIMIT] = maxMemberLimit;
			args [GuildSystem.Constants.MOTD] = motd;
			
			//string json = Json.Serialize (args);
			//SendRequest ("EDIT_GUILD_INFO", EDIT_GUILD_INFO, json, OnSuccess, OnFailure);
			SendRequest ("EDIT_GUILD_INFO", EDIT_GUILD_INFO.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("EDIT_GUILD_INFO", EDIT_GUILD_INFO.ToString(), args, OnSuccess, OnFailure);
		}


		public void LeaveGuild (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);
			//SendRequest ("LEAVE_GUILD", LEAVE_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("LEAVE_GUILD", LEAVE_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("LEAVE_GUILD", LEAVE_GUILD.ToString(), args, OnSuccess, OnFailure);
		}


		public void SendGuildMessage (string message, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["message"] = message;
			
			//string json = Json.Serialize (args);
			//SendRequest ("SEND_GUILD_MESSAGE", SEND_GUILD_MESSAGE, json, OnSuccess, OnFailure);
			SendRequest ("SEND_GUILD_MESSAGE", SEND_GUILD_MESSAGE.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("SEND_GUILD_MESSAGE", SEND_GUILD_MESSAGE.ToString(), args, OnSuccess, OnFailure);
		}


		//This call is not there on server anymore!!!
		public void GetGuildForGuildID (int guildID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;
			
			//string json = Json.Serialize (args);
			//SendRequest ("GET_USER_GUILD", GET_USER_GUILD, json, OnSuccess, OnFailure);
			SendRequest ("GET_USER_GUILD", GET_USER_GUILD.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GET_USER_GUILD", GET_USER_GUILD.ToString(), args, OnSuccess, OnFailure);
		}


		//This call is not there on server anymore!!!
		public void GetGuildMembersForGuildID (int guildID, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;
			
			//string json = Json.Serialize (args);
			//SendRequest ("GET_GUILD_MEMBERS", GET_GUILD_MEMBERS, json, OnSuccess, OnFailure);
			SendRequest ("GET_GUILD_MEMBERS", GET_GUILD_MEMBERS.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("GET_GUILD_MEMBERS", GET_GUILD_MEMBERS.ToString(), args, OnSuccess, OnFailure);
		}


		//Confirm with Raheel if this call even exists!!!
		public void GetUserNameAndEmail (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			//string json = Json.Serialize (args);
			//SendRequest ("USER_UPDATE", UPDATE_USERNAME, json, OnSuccess, OnFailure);
			SendRequest ("USER_UPDATE", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("USER_UPDATE", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
		}


		//Confirm with Raheel if this call even exists!!!
		public void UpdateEmailForUser (string email, Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["email"] = email;
			
			//string json = Json.Serialize (args);
			//SendRequest ("USER_UPDATE", UPDATE_USERNAME, json, OnSuccess, OnFailure);
			SendRequest ("USER_UPDATE", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("USER_UPDATE", UPDATE_USERNAME.ToString(), args, OnSuccess, OnFailure);
		}


		//Confirm with Raheel if this call even exists!!!
		public void GetUserRating (Action<object, object, ServerRequest> OnSuccess, Action<object, object, ServerRequest> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			
			//string json = Json.Serialize (args);
			//SendRequest ("USER_OFFLINE_RATING", USER_OFFLINE_RATING, json, OnSuccess, OnFailure);
			SendRequest ("USER_OFFLINE_RATING", USER_OFFLINE_RATING.ToString(), args, OnSuccess, OnFailure);
			//GeneralQuery ("USER_OFFLINE_RATING", USER_OFFLINE_RATING.ToString(), args, OnSuccess, OnFailure);
		}


		/*


		
		public bool CheckShrineActivation (Action<string> OnSuccess, Action<string> OnFailure, string type)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("element", type);
			string json = Json.Serialize (args);
			return SendRequest ("CheckShrineActivation", 1015, json, OnSuccess, OnFailure);
		}
		
		public bool CheckFriendShrineActivation (Action<string> OnSuccess, Action<string> OnFailure, string type)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("element", type);
			string json = Json.Serialize (args);
			return SendRequest ("CheckFriendShrineActivation", 23, json, OnSuccess, OnFailure);
		}
		


		

		

		
		#region Raids

		
		public bool LootRaidBoss (string name, int skullLevel, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["name"] = name;
			args ["skullLevel"] = skullLevel;
			string json = Json.Serialize (args);
			return SendRequest (LOOT_RAID_BOSS, json, OnSuccess, OnFailure);
		}
		#endregion
		
		#region PVP
		public bool TellPVPReady (Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			string json = Json.Serialize (args);
			return SendRequest (PVP_BATTLE_READY, json, OnSuccess, OnFailure);
		}
		
		private PVPServerManager.PVPOpponentFoundDelegate _PVPOpponentFoundDelegate;

		public bool QueueForPVP (Action<string> OnSuccess, Action<string> OnFailure, PVPServerManager.PVPOpponentFoundDelegate _PVPOpponentDelegate)
		{
			this._PVPOpponentFoundDelegate = _PVPOpponentDelegate;
			var args = new Dictionary<string, object> ();
			string json = Json.Serialize (args);
			return SendRequest (PVP_QUEUE, json, OnSuccess, OnFailure);
		}
		
		public bool SubmitPVPTurn (string name, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["name"] = name;
			string json = Json.Serialize (args);
			return SendRequest (PVP_TURN, json, OnSuccess, OnFailure);
		}
		#endregion
		
		#region GUILDS
		public bool CreateGuild (string name, int logoID, bool inviteOnly, string description, float pointsToJoin, int maxMemberLimit, string motd, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.NAME] = name;
			args [GuildSystem.Constants.GUILD_OBJ_LOGO_ID] = logoID;
			args [GuildSystem.Constants.GUILD_OBJ_INVITE_ONLY] = inviteOnly;
			args [GuildSystem.Constants.DESCRIPTION] = description;
			args [GuildSystem.Constants.GUILD_OBJ_MAX_MEMBERS] = maxMemberLimit;
			args [GuildSystem.Constants.GUILD_OBJ_JOIN_COST] = pointsToJoin;
			args [GuildSystem.Constants.MOTD] = motd;
			string json = Json.Serialize (args);
			return SendRequest (CREATE_GUILD, json, OnSuccess, OnFailure);
		}
		
		public bool JoinGuild (int guildID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;
			return SendRequest (JOIN_GUILD, json, OnSuccess, OnFailure);
		}
		
		public bool InviteToGuild (int inviteeID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.INVITEE_ID] = inviteeID;
			string json = Json.Serialize (args);
			return SendRequest (INVITE_TO_GUILD, json, OnSuccess, OnFailure);
		}
		
		public bool AcceptInviteToGuild (int inviteeID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.INVITEE_ID] = inviteeID;
			string json = Json.Serialize (args);
			return SendRequest (ACCEPT_GUILD_INVITE, json, OnSuccess, OnFailure);
		}
		
		public bool PromoteMember (int promoteeID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.PROMOTEE_ID] = promoteeID;
			string json = Json.Serialize (args);
			return SendRequest (PROMOTE_GUILD_MEMBER, json, OnSuccess, OnFailure);
		}
		
		public bool DemoteMember (int demoteeID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.DEMOTEE_ID] = demoteeID;
			string json = Json.Serialize (args);
			return SendRequest (DEMOTE_GUILD_MEMBER, json, OnSuccess, OnFailure);
		}
		
		public bool KickMember (int kickUserID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.KICK_USER_ID] = kickUserID;
			string json = Json.Serialize (args);
			return SendRequest (KICK_GUILD_MEMBER, json, OnSuccess, OnFailure);
		}
		
		public bool EditGuildInfo (int guildID, string name, int logoID, bool inviteOnly, string description, int maxMemberLimit, string motd, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;
			args [GuildSystem.Constants.NAME] = name;
			args [GuildSystem.Constants.LOGO_ID] = logoID;
			args [GuildSystem.Constants.INVITE_ONLY] = inviteOnly;
			args [GuildSystem.Constants.DESCRIPTION] = description;
			args [GuildSystem.Constants.MAX_MEMBER_LIMIT] = maxMemberLimit;
			args [GuildSystem.Constants.MOTD] = motd;

			string json = Json.Serialize (args);
			return SendRequest (EDIT_GUILD_INFO, json, OnSuccess, OnFailure);
		}
		
		public bool LeaveGuild (Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			string json = Json.Serialize (args);
			return SendRequest (LEAVE_GUILD, json, OnSuccess, OnFailure);
		}
		
		private GuildSystem.GuildsManager.GuildRawMessagesListener _guildMessagesListener;
		private GuildSystem.GuildsManager.GuildACLUpdateListener _guildACLUpdateListener;

		public bool JoinGuildChat (Action<string> OnSuccess, Action<string> OnFailure, GuildSystem.GuildsManager.GuildRawMessagesListener _guildMessagesListener, GuildSystem.GuildsManager.GuildACLUpdateListener _guildACLUpdateListener)
		{
			this._guildMessagesListener = _guildMessagesListener;
			this._guildACLUpdateListener = _guildACLUpdateListener;
			var args = new Dictionary<string, object> ();

			string json = Json.Serialize (args);
			return SendRequest (JOIN_GUILD_CHAT, json, OnSuccess, OnFailure);
		}
		
		public bool LeaveGuildChat (Action<string> OnSuccess, Action<string> OnFailure)
		{
			_guildMessagesListener = null;
			var args = new Dictionary<string, object> ();

			string json = Json.Serialize (args);
			return SendRequest (LEAVE_GUILD_CHAT, json, OnSuccess, OnFailure);
		}
		
		public bool SendGuildMessage (string message, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["message"] = message;

			string json = Json.Serialize (args);
			return SendRequest (SEND_GUILD_MESSAGE, json, OnSuccess, OnFailure);
		}
		
		public bool GetGuildForUserID (int userID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["targetUserID"] = userID;

			string json = Json.Serialize (args);
			return SendRequest (GET_USER_GUILD, json, OnSuccess, OnFailure);
		}
		
		public bool GetGuildForGuildID (int guildID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;

			string json = Json.Serialize (args);
			return SendRequest (GET_USER_GUILD, json, OnSuccess, OnFailure);
		}
		
		public bool GetGuildMembersForGuildID (int guildID, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args [GuildSystem.Constants.GUILD_ID] = guildID;

			string json = Json.Serialize (args);
			return SendRequest (GET_GUILD_MEMBERS, json, OnSuccess, OnFailure);
		}
		
		public bool ListGuilds (string guildNameCriteria, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["guildNameCriteria"] = guildNameCriteria;

			string json = Json.Serialize (args);
			return SendRequest (LIST_GUILDS, json, OnSuccess, OnFailure);
		}
		
		#endregion
		

		
		public bool GetUserNameAndEmail (Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			string json = Json.Serialize (args);
			return SendRequest (USER_UPDATE, json, OnSuccess, OnFailure);
		}
	    
		public bool UpdateEmailForUser (string email, Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();
			args ["email"] = email;
	        
			string json = Json.Serialize (args);
			return SendRequest (USER_UPDATE, json, OnSuccess, OnFailure);
		}
	    
		public bool GetUserRating (Action<string> OnSuccess, Action<string> OnFailure)
		{
			var args = new Dictionary<string, object> ();

			string json = Json.Serialize (args);
			return SendRequest (USER_OFFLINE_RATING, json, OnSuccess, OnFailure);
		}
	    

		*/

        #region CLAN SYSTEM calls
		public void CreateTeam (string teamName, string teamDescription, int teamFlag, int teamType, int requiredTrophies, int warFrequency, string teamLocation, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("teamName", teamName);
			args.Add ("teamDescription", teamDescription);
			args.Add ("teamFlag", teamFlag);
			args.Add ("teamType", teamType);
			args.Add ("teamRequiredTrophies", requiredTrophies);
			args.Add ("teamWarFrequency", warFrequency);
			args.Add ("teamLocation", teamLocation);
            
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("CreateTeam-JSON: " + json);
            
			GeneralQuery ("CreateTeam", CREATE_GUILD.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void ListTeams (Action<string> OnSuccess = null, Action OnFailure = null)
		{ var args = new Dictionary<string, object>();
			GeneralQuery ("ListTeams", LIST_GUILDS.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void GetMyTeam (Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object>();
			GeneralQuery ("GetMyTeam", GET_USER_GUILD.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void SearchTeam (string teamName, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("teamName", teamName);
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("SearchTeam-JSON: " + json);
            
			GeneralQuery ("SearchTeam", SEARCH_GUILD.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void JoinTeam (long teamID, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("teamId", teamID);
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("JoinTeam-JSON: " + json);
            
			GeneralQuery ("JoinTeam", JOIN_GUILD.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void LeaveTeam (Action<string> OnSuccess = null, Action OnFailure = null)
		{ 
			var args = new Dictionary<string, object>();
			GeneralQuery ("LeaveTeam", LEAVE_GUILD.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void KickUser (long affecteeID, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("affecteeID", affecteeID);
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("KickUser-JSON: " + json);
            
			GeneralQuery ("KickUser", KICK_GUILD_MEMBER.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void SendGlobalChatMessage (string message, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("message", message);
			//string json = JsonConvert.SerializeObject (args);
			//Debug.Log ("SendGlobalChatMessage-JSON: " + json);
            
			GeneralQuery ("SendGlobalChatMessage", SEND_GLOBAL_MESSAGE.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void SendTeamChat (string message, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("message", message);
			//string json = JsonConvert.SerializeObject (args);
			//Debug.Log ("SendTeamChat-JSON: " + json);
            
			GeneralQuery ("SendTeamChat", SEND_GUILD_MESSAGE.ToString (), args, OnSuccess, OnFailure);
		}
        
		public void GetTeamChats (long userID, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			args.Add ("userID", userID);
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("GetTeamChats-JSON: " + json);
            
			GeneralQuery ("GetTeamChats", GET_GUILD_MESSAGES.ToString (), args, OnSuccess, OnFailure);
		}

		public void GetGlobalChats (Action<string> OnSuccess = null, Action OnFailure = null)
		{
			var args = new Dictionary<string, object> ();
			//string json = JsonConvert.SerializeObject (args);
			//      Debug.Log("GetTeamChats-JSON: " + json);
			
			GeneralQuery ("GetGlobalChats", GET_GLOBAL_MESSAGES.ToString (), args, OnSuccess, OnFailure);
		}

		#endregion CLAN SYSTEM calls

	}
}
