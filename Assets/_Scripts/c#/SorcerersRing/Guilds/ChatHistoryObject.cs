using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

namespace GuildSystem
{
	public class ChatHistoryObject : IComparable
	{
		public int messageType			{	get;	private set;	}
		
		public string message			{	get;	private set;	}
		
		public long timestamp			{	get;	private set;	}
		
		public int guildID				{	get;	private set;	}
		
		public string fromSender		{	get;	private set;	}
		
		public IDictionary rawObject	{	get;	private set;	}
		
		private static Dictionary<int, string> MESSAGES;
		static ChatHistoryObject()
		{
			MESSAGES = new Dictionary<int, string>();
			MESSAGES[2] = "Joined chat";
			MESSAGES[3] = "Logged out";
			MESSAGES[4] = "Left guild";
			MESSAGES[5] = "Got kicked from guild";
			MESSAGES[6] = "Got demoted";
			MESSAGES[7] = "Got promoted";
		}
		
		public int CompareTo(object otherObject)
		{
			ChatHistoryObject otherChatObject = otherObject as ChatHistoryObject;
			if(this.timestamp > otherChatObject.timestamp)
				return -1;
			else if(this.timestamp < otherChatObject.timestamp)
				return 1;
			else
				return 0;
		}
		
		public ChatHistoryObject(IDictionary rawObject)
		{
			if(GameManager.PRINT_LOGS) Debug.Log(Json.Serialize(rawObject));
			messageType = Int32.Parse(rawObject[GuildSystem.Constants.CHAT_MESSAGE_TYPE].ToString());
			if(messageType != 1)
				message = MESSAGES[messageType];
			else
			{
				if(rawObject.Contains(GuildSystem.Constants.CHAT_MESSAGE))
					message = rawObject[GuildSystem.Constants.CHAT_MESSAGE].ToString();
			}
			if(rawObject.Contains(GuildSystem.Constants.CHAT_MESSAGE_TIMESTAMP) && rawObject[GuildSystem.Constants.CHAT_MESSAGE_TIMESTAMP] != null)
				timestamp = long.Parse(rawObject[GuildSystem.Constants.CHAT_MESSAGE_TIMESTAMP].ToString());
			guildID = Int32.Parse(rawObject[GuildSystem.Constants.CHAT_MESSAGE_GUILD_ID].ToString());
			if(rawObject.Contains(GuildSystem.Constants.CHAT_MESSAGE_FROM) && rawObject[GuildSystem.Constants.CHAT_MESSAGE_FROM] != null)
				fromSender = rawObject[GuildSystem.Constants.CHAT_MESSAGE_FROM].ToString();
			
			this.rawObject = rawObject;
		}
		
		public IDictionary ToDictionary()
		{
			IDictionary dictionary = new Dictionary<string, object>();
			
			dictionary[GuildSystem.Constants.CHAT_MESSAGE_TYPE] = this.messageType;
			dictionary[GuildSystem.Constants.CHAT_MESSAGE] = this.message;
			dictionary[GuildSystem.Constants.CHAT_MESSAGE_TIMESTAMP] = this.timestamp;
			dictionary[GuildSystem.Constants.CHAT_MESSAGE_GUILD_ID] = this.guildID;
			dictionary[GuildSystem.Constants.CHAT_MESSAGE_FROM] = this.fromSender;
			
			return dictionary;
		}
	
	}
}