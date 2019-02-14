using UnityEngine;
using System.Collections;
using WebSocket4Net;
using System;
using SuperSocket.ClientEngine;
using System.Collections.Generic;

using Newtonsoft.Json;
using LogMessages;

//using Game.Managers;

namespace WebSocket_Client
{
    public class Client : MonoBehaviour
    {

        public static Client Instance;
        public static bool PrintLogs = true;

        public static event Action OnConnected;
        public static event Action OnDisconnect;

        private float TimeOut = 2f;
       
        [HideInInspector]
        private string
            URI;
        [SerializeField]
        bool
            ConnectLocally;

        const string LocalIP = "ws://192.168.9.47:37121";
//        const string ServerIP = "ws://35.160.146.193:10094/websocket";
//		const string ServerIP = "ws://fbomb.cloudapp.net:37121";
		const string ServerIP = "ws://ec2-35-160-90-137.us-west-2.compute.amazonaws.com:37131";

        private WebSocket
            websocket;

        private const string CORELATIONID = "correlationId";
        Dictionary<string,Request> UnReplied = new Dictionary<string, Request>();
        Dictionary<string,Action<InBoundMessage>> Handlers = new Dictionary<string, Action<InBoundMessage>>();
        List<string> Ids = new List<string>();


        void Awake()
        {
            if (ConnectLocally)
                URI = LocalIP;
            else
                URI = ServerIP;

            Instance = this;
            ResponseProcessRequests = new Queue<Action>();
            StartCoroutine("ProcessRequests");
//            StartCoroutine("Testing");
        }

        public static Queue<Action> ResponseProcessRequests;
        IEnumerator ProcessRequests()
        {
            while (true)
            {
               // Debug.Log("ProcessRequests");
                if (ResponseProcessRequests.Count >= 1)
                {
                    ResponseProcessRequests.Dequeue().Invoke();
                }
                yield return new WaitForSeconds(0.025f);
            }
        }

        IEnumerator CheckRequestTimeout()
        {
            while (true)
            {  
                var Values = UnReplied.Values;
//				Debug.LogError("Unreplied - "+Values.Count);
                foreach (var item in Values)
                {
//                    Debug.LogError("CheckRequestTimeout " + item.ElaspedTime + " " + Time.time);
                    if (TimeOut <= item.ElaspedTime)
                    {
                        Ids.Add(item.CorrelationID);
                        if (item.onFailure != null)
                        {
							item.onFailure("{\"success\": false}");
//							Debug.LogError("onFailure called inside " + item.ElaspedTime + " " + Time.time);

                        }
                    } else
                    {
                        item.ElaspedTime++;
                    }
                }

                for (int i = 0; i < Ids.Count; i++)
                {
                    UnReplied.Remove(Ids [i]);
                }
                Ids.Clear();
                yield return new WaitForSeconds(1f);
            }
        }

		public void Connect(Action<object, EventArgs> noConnectivityListener)
        {
            this.Log("[WS] Trying to connect");
			StopCoroutine("CheckRequestTimeout");
			StartCoroutine("CheckRequestTimeout");
			websocket = new WebSocket(URI);
            websocket.Opened += new EventHandler(websocket_Opened);
			websocket.Error += new EventHandler<ErrorEventArgs>(websocket_Error);
			websocket.Error += new EventHandler<ErrorEventArgs>(noConnectivityListener);
			websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();

//            StopCoroutine("CheckRequestTimeout");
//            StartCoroutine("CheckRequestTimeout");
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
//            Networking.ResponseProcessRequests.Enqueue(() =>
//                {
            if (OnConnected != null)
            {
                Debug.LogError("[WS] Connected");
                OnConnected();
            } else
                Debug.LogError("OnConnected not Registered!!!");
//                });
        }

        void websocket_Error(object sender, EventArgs e)
        {
            Hashtable Ex = (e as ErrorEventArgs).Exception.Data as Hashtable;
            Debug.Log(Ex.Count);
            foreach (string key in Ex.Keys)
            {
                this.Log(String.Format("{0}: {1}", key, Ex [key]));
            }
            this.Log("Error: " + (e as ErrorEventArgs).Exception);

        }

        void websocket_Closed(object sender, EventArgs e)
        {
            this.Log("Socket Closed: " + (e as ClosedEventArgs).Reason);
            UnReplied.Clear();
            Handlers.Clear();
        }

        void websocket_MessageReceived(object sender, EventArgs e)
        {
            string Response = (e as MessageReceivedEventArgs).Message;
            this.Log("[Received] " + Response);
            var ResponseDict = JsonConvert.DeserializeObject<Dictionary<string,object>>(Response);
            string CorelationID = null;
            if (ResponseDict.ContainsKey(CORELATIONID))
            {
                CorelationID = ResponseDict [CORELATIONID].ToString();
                Request Req = null;
                if (UnReplied.TryGetValue(CorelationID, out Req))
                {
                    if (Req.onReply != null)
                    {
                        ResponseProcessRequests.Enqueue(() =>
                        {
                            Req.onReply(new Reply(CorelationID, null, ResponseDict ["message"].ToString()));
                        });
                        UnReplied.Remove(CorelationID);
                    }
                }
            } else
            {
                string action = null;
                if (ResponseDict.ContainsKey("action"))
                {
                    Action<InBoundMessage> CallBack = null;
                    action = ResponseDict ["action"].ToString();

                    if (Handlers.TryGetValue(action, out CallBack))
                    {
                        ResponseProcessRequests.Enqueue(() =>
                        {
                            CallBack(new InBoundMessage(action, ResponseDict ["message"].ToString()));
                        });
                        
                    }
                }
            }
        }

        public void Disconnect()
        {
            if (OnDisconnect != null)
                OnDisconnect();

            Debug.Log("socket Disconnected");
            UnReplied.Clear();
            Handlers.Clear();

            websocket.Close();
        }

        public void On(string ActionNo, Action<InBoundMessage> CallBack)
        {
            if (!Handlers.ContainsKey(ActionNo))
                Handlers.Add(ActionNo, CallBack);
            else
                Debug.LogError(ActionNo + " already exists!");
        }

        public void Send(Request req)
        {

            if (websocket.State == WebSocketState.Open)
            {
//                UnReplied.Add(req.CorrelationID, req);
                websocket.Send(req.DataAsString);
            }
			UnReplied.Add(req.CorrelationID, req);
			this.Log("[WS] Sent: " + req.DataAsString);
            
        }

        public void OutBoundMessage(Request req)
        {     
            if (websocket.State == WebSocketState.Open)
            {
//                UnReplied.Add(req.CorrelationID, req);
                websocket.Send(req.DataAsString);
            }
        }

        void OnDisable()
        {
            Debug.Log("OnDisable");
        }

        void OnDestroy()
        {
            Disconnect();
        }

		public bool IsConnected() {

				if(websocket.State == WebSocketState.Open)
				 return true;
				else 
					return false;

		}
    }
}
