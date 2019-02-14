using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

namespace Game.UI
{
    public class ChatUIHandler : Singleton<ChatUIHandler>//, IDeselectHandler
    {
        // Use this for initialization
//        public GameObject chatListNode;
//        GameObject clone;
//        public GameObject ChatButton;
//        public Sprite ChatClose;
//        public Text ClanName;
//        public Image ClanFlag;
//        public Button CreateJoinClanButton;
//
//        [HideInInspector]
//        public List<chatrow>
//            GlobalChatList;
//        [HideInInspector]
//        public List<chatrow>
//            ClanChatList;
//        public GameObject globalChatScrollView;
//        public GameObject clanChatScrollView;
//        public GameObject inputPanel;
//        public GameObject infoPanel;
//        private int count = 0;
//        private int remove = 0;
//		
//        void Awake()
//        {
//            this.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
//            if (this.GetComponent<Canvas>().worldCamera == null)
//                Debug.LogError("GUICamera is NULL");
//        }
//        
//        void Start()
//        {
////            Debug.LogError("GameManager._gameState.User.team: "+GameManager._gameState.User.team);
//            if (GameManager._gameState.User.team != null)
//            {
//                PopulateChat();
//            } else
//            {
//                infoPanel.SetActive(false);
//                inputPanel.SetActive(false);
//                clanChatScrollView.SetActive(false);
//                CreateJoinClanButton.gameObject.SetActive(true);
//            }
//        }
//		
//        public void GoToClanPanel()
//        {
//            ShowChatWindow(false);
////			GoBackToHud ();
//            UIManager.instance.ShowUI(GameUI.ClanUI, hideAll: false);
//			
//        }
//		
////		public void GoBackToHud ()
////		{
//////			ChatButton.GetComponent<Image> ().sprite = ChatClose;
////			UIManager.instance.ShowUI (GameUI.HudUI);
////		}
//
//		#region SHOW CHAT window
//		public Animator _chatWindow;
//		public void ShowChatWindow (bool _show)
//		{
////			Debug.LogError ("ChatUIHandler-ShowChatWindow: " + _show);
//			if (_chatWindow == null)
//				_chatWindow = ChatUIHandler.instance.GetComponent<Animator> ();
//			_chatWindow.SetBool ("ShowChatWindow", _show);
//		}
//		#endregion SHOW CHAT window
//
//        public void PopulateChatNode(bool isClan, ChatMessage chatMessage)
//        {
//            // TODO: Add additional parameters after update on the server side.
//            PopulateChatNode(isClan, chatMessage.senderName.ToString(), "Member", chatMessage.message, GameUtils.FromUnixTime(chatMessage.createdTime), chatMessage.senderID);
//        }
//
//        public void PopulateChatNode(bool isClan, string username, string memberType, string message, DateTime time, int _senderID)
//        {
//            chatrow chatNode = (Instantiate(chatListNode) as GameObject).GetComponent<chatrow>();
//            chatNode.userNameButton.onClick.RemoveAllListeners();
//            chatNode.userNameButton.onClick.AddListener(() => {
////				Debug.Log ("PopulateChatNode-UsedId: " + _senderID + "   Username: " + username);
//                ShowBase(chatNode.transform, _senderID, username);
//            });
//            chatNode.message.text = message;
//            chatNode.userName.text = username;
//            chatNode.pushTime = time;
//            chatNode.transform.SetParent(isClan ? clanChatScrollView.transform : globalChatScrollView.transform);
//            chatNode.transform.localPosition = new Vector3(chatNode.transform.localPosition.x, chatNode.transform.localPosition.y, 0);
//            chatNode.transform.localScale = Vector3.one;
//            chatNode.transform.SetAsFirstSibling();
//            chatNode._replay.SetActive(false);
//
//            if (isClan)
//            {
//                clanChatScrollView.GetComponent<ScrollContent>().RefreshContent();
//                ClanChatList.Add(chatNode);
////				this.PerformActionWithDelay (0.2f, () => {
////					clanChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
////				});
////				Debug.Log ("NormalizedPosition-Clan: " + clanChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition);
//            } else
//            {
//                globalChatScrollView.GetComponent<ScrollContent>().RefreshContent();
//                GlobalChatList.Add(chatNode);
////				this.PerformActionWithDelay (0.2f, () => {
////					globalChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
////				});
////				Debug.Log ("NormalizedPosition-Global: " + globalChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition);
//            }
//        }
//
//        public void SendClanMessage(Text message)
//        {
//            if (message.text.Trim() == "")
//                return;
//            Networking.instance.SendTeamChat(message.text, (msg) => {
//                //PopulateChatNode(true, "UserName", "Elder", message.text, DateTime.Now);
//            }, () => {});
//
//        }
//        
//		public void SendGlobalMessage (InputField message)
//		{
//			if (message.text.Trim () == "")
//				return;
//
////			Debug.LogError ("SendGlobalMessage: " + message.text);
//			Networking.instance.SendGlobalChatMessage (message.text, (msg) => {
//				message.text = "";
//				//PopulateChatNode(false, "UserName", "Elder", message.text, DateTime.Now);
//			}, () => {});
//		}
//
//        protected void PopulateChat()
//        {
//            Networking.instance.GetTeamChats(GameDataHandler.instance.playerData.userID, (response) => {
//                TeamChat teamChat = JsonConvert.DeserializeObject<TeamChat>(response);
//                if (teamChat.success)
//                {
//                    foreach (var chatMessage in teamChat.chats)
//                    {
//                        PopulateChatNode(true, chatMessage);
//                    }
//                } else
//                    Debug.LogError("Unable to get TeamChat: " + response);
//            }, () => {});
//
//        }
//
//		#region VIEW BASE POP-UP
//        public GameObject _popUp;
//        public Button _viewBase;
//        public void ShowBase(Transform _caller, int _senderId, string username)
//        {
//            Debug.Log("ShowBase-UsedId: " + _senderId + "   Username: " + username);
//            _popUp.SetActive(true);
//            _popUp.transform.SetParent(_caller);
//            _popUp.transform.SetAsLastSibling();
//            _popUp.transform.localPosition = new Vector3(100f, 104f, 0f);
//            _viewBase.onClick.RemoveAllListeners();
//            _viewBase.onClick.AddListener(() => {
//                Debug.Log("Viewing-UsedId: " + _senderId + "   Username: " + username);
//                ShowChatWindow(false);
//                GameManager.instance.ViewBaseOfPlayerId(_senderId, username);
//            });
//        }
//
///*		void OnDisable ()
//		{
//			if (_popUp.activeInHierarchy)
//				_popUp.SetActive (false);
//		}
//
//		public void OnDeselect (BaseEventData _eventData)
//		{
//			Debug.Log ("OnDeselect: " + _eventData.selectedObject.name);
//			if (!_eventData.selectedObject.name.Equals ("UserName") && _popUp.activeInHierarchy) {
//				_popUp.transform.SetParent (transform);
//				_popUp.transform.SetAsLastSibling ();
//				_popUp.SetActive (false);
//			}
//		}*/
//		
//        public void ChatRowDeselect()
//        {
////			Debug.Log ("OnDeselect: " + _eventData.selectedObject.name);
//            if (_popUp.activeInHierarchy)
//            {
//                _popUp.transform.SetParent(transform);
//                _popUp.transform.SetAsLastSibling();
//                _popUp.SetActive(false);
//            }
//        }
//		#endregion VIEW BASE POP-UP
    }
}

