using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GuildSystem
{
    public class ChatManager
    {
        private const int MSG_HISTORY_COUNT = 10;
		
        private List<ChatHistoryObject> historyMessages;
        private List<ChatHistoryObject> unreadMessages;
        private ChatListener listener;
		
        public ChatManager(ChatListener listener, List<ChatHistoryObject> historyMessages)
        {
            this.listener = listener;
			
            if (historyMessages == null)
                this.historyMessages = new List<ChatHistoryObject>();
            else
                this.historyMessages = historyMessages;
			
            unreadMessages = new List<ChatHistoryObject>();
			
            historyMessages.Sort();
            historyMessages.Reverse();
//			for(int i = 0; i<historyMessages.Count; i++)
//				this.listener.OnNewMessageReceived(historyMessages[i]);
        }
		
        public void MarkRead()
        {
            for (int i = 0; i<unreadMessages.Count; i++)
            {
                historyMessages.Add(unreadMessages [i]);
            }
            unreadMessages.Clear();
            unreadMessages.TrimExcess();
			
            if (historyMessages.Count > MSG_HISTORY_COUNT)
            {
                historyMessages.Reverse();
				
                int countToDelete = historyMessages.Count - MSG_HISTORY_COUNT;
                if (GameManager.PRINT_LOGS)
                    Debug.Log("countToDelete" + countToDelete);
                historyMessages.RemoveRange(MSG_HISTORY_COUNT, countToDelete);
                historyMessages.TrimExcess();
            }
					
            //populateMessages();
        }
		
        public void OnMessageReceived(ChatHistoryObject newMessage)
        {
            //if(GameManager.PRINT_LOGS) Debug.Log("OnMessageReceived" + newMessage.message);
            unreadMessages.Add(newMessage);
//            listener.OnNewMessageReceived(newMessage);
            //populateMessages();
        }
		
        /*private void populateMessages()
		{
			List<ChatHistoryObject> messages = new List<ChatHistoryObject>();
			unreadMessages.Sort();
			historyMessages.Sort();
			messages.AddRange(unreadMessages);
			messages.AddRange(historyMessages);
			listener.OnNewMessageReceived(messages);
			foreach(ChatHistoryObject histObj in messages)
				if(GameManager.PRINT_LOGS) Debug.Log(MiniJSON.Json.Serialize(histObj.ToDictionary()));
		}*/
    }
}