using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Frankfort.Threading;

//using Game.UI;

namespace AMQP
{
		public class Client
		{
				public static float Timeout = 50.0f;
				public static float GetTime = 0f;
				public static bool SendMSG = true;
				public static bool GetReplies = true;
				public static string LoginExchange = "sr.usercontrol";
				public static string CommunicationExchange;

				public static Dictionary<string, OnReceivedCallback> Handlers  { get; protected set; }

				public static Queue<OutboundMessage> Unsent { get; protected set; }

				public static Dictionary<string, Request> Unreplied  { get; protected set; }
				public delegate void OnReceivedCallback (InboundMessage message);
    
				public static ConnectionFactory ConnectionFactory { get; protected set; }

				public static IConnection Connection { get; protected set; }

				public static IModel Channel { get; protected set; }

				public static string ReceiveQueue { get; protected set; }

				protected static Action onConnected;
				public static Action onUnableToConnect;

				public static Thread Thread { get; protected set; }

				public static bool IsRunning { get; protected set; }

				public static void On (string action, OnReceivedCallback callback)
				{
						lock (Handlers) {
								Handlers [action] = callback;
						}
				}

				public static void Connect (Settings settings, Action onConnected)
				{
						if (IsConnected)
								Disconnect ();
           
						if (! IsConnectedToInternet) {
								Log (new AMQP.Exception (AMQP.ExceptionType.NoInternetAccess, settings.ToString ()));
								return;
						}
           
						Client.onConnected = onConnected;
						Thread = Loom.StartSingleThread (Execute, settings, System.Threading.ThreadPriority.Normal, true);

//            ConnectInternal(settings);
				}

				public static void Connect (Settings settings, Action onConnected, Action onNoInternetConnection)
				{
						if (IsConnected)
								Disconnect ();
			
						if (! IsConnectedToInternet) {
								onNoInternetConnection ();
								Log (new AMQP.Exception (AMQP.ExceptionType.NoInternetAccess, settings.ToString ()));
								return;
						}
			
						Client.onConnected = onConnected;
						Thread = Loom.StartSingleThread (Execute, settings, System.Threading.ThreadPriority.Normal, true);
			
						//            ConnectInternal(settings);
				}
        
				protected static void Execute (object settings)
				{
           
						IsRunning = true;
						ConnectInternal (settings as Settings);
						while (IsRunning) {
								List<string> toRemove = new List<string> ();

								lock (Unreplied) {
										foreach (KeyValuePair<string, Request> pair in Unreplied) {
												if (GetTime - pair.Value.SentTime > Timeout) {
														if (pair.Value.onFailed != null) {
																Loom.DispatchToMainThread (() => {
																		pair.Value.onFailed (pair.Value); 
																});
														}
														Log ("Removed: " + pair.Key + " ,Unreplied for:" + (GetTime - pair.Value.SentTime));
														Loom.DispatchToMainThread (() => {
//                                Handlers.Clear();
//                                Game.GameUtils.RelaunchGame();
														});

														toRemove.Add (pair.Key);
												}
										}
								}

								foreach (string ID in toRemove) {
										Unreplied.Remove (ID);
								}
                
								int tempCount = Unsent.Count;
								while (tempCount > 0) {
										tempCount --;
										OutboundMessage toSend = Unsent.Dequeue ();
										if (GetTime - toSend.QueueTime < Timeout) {
												if (SendMSG) {
														if (toSend.IsRetry > 0) {
																if (toSend.onRetry != null) {
																		Loom.DispatchToMainThread (() => {
																				toSend.onRetry (toSend); });
																}
														}
														toSend.SendInternal ();
														Log (" unreplied: " + Unreplied.Count);
												} else {
														if (toSend.IsRetry > 0) {
																if (toSend.onRetry != null) {
																		Loom.DispatchToMainThread (() => {
																				toSend.onRetry (toSend); });
																}
														}
														if (toSend.IsRetry > 3) {

																Handlers.Clear ();
//                                Game.GameUtils.RelaunchGame();
														}
														Unsent.Enqueue (toSend);
												}
										} else {
												Log ("Removed: " + (toSend as Request).CorrelationID + " ,Unsent for:" + (GetTime - toSend.QueueTime));
												if (toSend.onFailed != null)
														Loom.DispatchToMainThread (() => {
																toSend.onFailed (toSend); 
																Handlers.Clear ();
//                                Game.GameUtils.RelaunchGame();

														});
										}
								}
						}
				}

				protected static void ConnectInternal (Settings settings)
				{
						try {
								ConnectionFactory = new ConnectionFactory ();
//				ConnectionFactory.auto
//				ConnectionFactory.n
								ConnectionFactory.SocketFactory = new ConnectionFactory.ObtainSocket (Ipv4SocketFactory.GetSocket);
								ConnectionFactory.HostName = settings.hostName;
								if (settings.protocol != null)
										ConnectionFactory.Protocol = settings.protocol;
								if (settings.virtualHost != null)
										ConnectionFactory.VirtualHost = settings.virtualHost;
								if (settings.port != null)
										ConnectionFactory.Port = settings.port.Value;
								if (settings.userName != null)
										ConnectionFactory.UserName = settings.userName;
								if (settings.password != null)
										ConnectionFactory.Password = settings.password;
								if (settings.heartbeat != null)
										ConnectionFactory.RequestedHeartbeat = settings.heartbeat.Value;


								Connection = ConnectionFactory.CreateConnection ();

								Channel = Connection.CreateModel ();

								ReceiveQueue = Channel.QueueDeclare ();

								EventingBasicConsumer consumer = new EventingBasicConsumer ();
								consumer.Received += OnReceive;
								Channel.BasicConsume (ReceiveQueue, true, consumer);
				Connection.AutoClose = true;


				Unsent = new Queue<OutboundMessage> ();
								Unreplied = new Dictionary<string, Request> ();
								Handlers = new Dictionary<string, OnReceivedCallback> ();
								Log ("Connected", settings.ToString ());
								if (onConnected != null)
									Loom.DispatchToMainThread (() => {
//										Connection.AutoClose = true;
										onConnected ();
									});
						} catch (System.Exception ex) {

				IsRunning = false;
//              onUnableToConnect();
								if (onUnableToConnect != null)
										Loom.DispatchToMainThread (() => {
												onUnableToConnect (); });
								Log (new AMQP.Exception (AMQP.ExceptionType.CouldNotConnect, settings.ToString (), ex));
						}
				}

				protected static void OnReceive (IBasicConsumer sender, BasicDeliverEventArgs args)
				{
						IBasicProperties properties = args.BasicProperties;
						InboundMessage message = null;
            
						string action = null;
						if (properties.IsHeadersPresent ()) {
								object actionObj = null;
								if (properties.Headers.TryGetValue ("action", out actionObj))
								{
										action = Message.GetString (actionObj as byte[]);
										Log("Header found. Action Found");
								}
								Log("Header found. Action not Found");
			}
            
						if (properties.IsCorrelationIdPresent ()) {
								string correlationId = properties.CorrelationId;
								message = new Reply (correlationId, action, properties.Headers as Dictionary<string, object>, args.Body);
						} else {
								message = new InboundMessage (action, properties.Headers as Dictionary<string, object>, args.Body);
				Log ("Inbound Message with action: "+action);
						}
						Log ("Received", message.ToString ());
						message.Dispatch ();
				}

				public static void Disconnect ()
				{

					Debug.LogError("Disconnect called - Client "+IsConnected);
						if (IsConnected) {
								try {
										IsRunning = false;
										Connection.Close (0);
										Channel.Close ();
//										Channel.WaitForConfirmsOrDie(TimeSpan.Zero);
										Thread.Abort ();
										Channel = null;
										Connection = null;
										ConnectionFactory = null;
										Unsent = null;
										Unreplied = null;
										Handlers = null;
										Log ("Disconnected");
//										Messenger.StartMessengerHelper();
								} catch (System.Exception ex) {
										Log (new AMQP.Exception (AMQP.ExceptionType.CouldNotDisconnect, "", ex));
								}
						}
				}

		public static bool IsConnected {
			get {
				return Channel != null && Channel.IsOpen;
			}
		}

		public static bool IsConnection {
			get {

				return Connection != null && Connection.IsOpen;
			}
		}

				public static bool IsConnectedToInternet {
						get {
								#if UNITY_EDITOR || UNITY_STANDALONE
//								try {
//										using (var client = new WebClient()) {
//												using (var stream = client.OpenRead("http://www.google.com")) {
//														return true;
//												}
//										}
//								} catch {
//										return false;
//								}
				return Helpers.IsConnected();


								#else
				return Application.internetReachability != NetworkReachability.NotReachable;
								#endif
						}
				}

				public static void Log (AMQP.Exception exception)
				{
						Log (exception.ToString ());
				}
        
				public static void Log (string tag, string data)
				{
						Log (tag + ":\n" + data);
				}
        
				public static void Log (string message)
				{
						Loom.DispatchToMainThread (() => {
//								Debug.LogWarning ("[AMQP] " + message + "\n"); 
						});
				}
		}
}