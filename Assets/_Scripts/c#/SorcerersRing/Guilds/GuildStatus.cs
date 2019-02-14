using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GuildSystem
{
	public class GuildStatus 
	{
		private IDictionary rawDictionary;
		
		public bool canInvite							{	get;	private set;	}
		public bool canPromote							{	get;	private set;	}
		public bool canDemote							{	get;	private set;	}
		public bool canKick								{	get;	private set;	}
		public bool canEdit								{	get;	private set;	}
		public bool canRead								{	get;	private set;	}
		public bool canWrite							{	get;	private set;	}
		public bool roleName							{	get;	private set;	}
		public int rank									{	get;	private set;	}
		public List<GuildMember> onlineMembers			{	get;	private set;	}
		public List<ChatHistoryObject> chatHistoryList	{	get;	private set;	}
		
		
		public GuildStatus(IDictionary rawDictionary)
		{
			UpdateACL(rawDictionary, false);
			
			onlineMembers = new List<GuildMember>();
			chatHistoryList = new List<ChatHistoryObject>();
			
			IList onlinePlayers = rawDictionary[GuildSystem.Constants.GS_ONLINE_MEMBERS] as IList;
			for(int i = 0; i< onlinePlayers.Count; i++)
				onlineMembers.Add(new GuildMember(onlinePlayers[i] as IDictionary));
		
			IList chatIList = rawDictionary[GuildSystem.Constants.GS_CHAT_HISTORY] as IList;
			for(int i = 0; i< chatIList.Count; i++)
				chatHistoryList.Add(new ChatHistoryObject(chatIList[i] as IDictionary));
			
			this.rawDictionary = rawDictionary;
		}
		
		public void UpdateACL(IDictionary aclDictionary, bool isUpdate = true)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("UpdateACL(IDictionary aclDictionary)" + MiniJSON.Json.Serialize(aclDictionary));
			
			IDictionary ACLDictionary = aclDictionary[GuildSystem.Constants.GS_ACL] as IDictionary;
			canInvite = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_INVITE].ToString());
			canPromote = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_PROMOTE].ToString());
			canDemote = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_DEMOTE].ToString());
			canKick = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_KICK].ToString());
			canEdit = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_EDIT].ToString());
			canRead = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_READ].ToString());
			canWrite = bool.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_WRITE].ToString());
			rank = System.Int32.Parse(ACLDictionary[GuildSystem.Constants.GS_ACL_RANK].ToString());
			if(isUpdate)
				isAclUpdated = false;
		}
		
		private bool _isAclUpdated = true;
		public bool isAclUpdated // defined only to allow for the default value of true
		{
			get
			{
				return _isAclUpdated;
			}
			set
			{
				_isAclUpdated = value;
			}
		}
		
		public IDictionary ToDictionary()
		{
			IDictionary dictionary = new Dictionary<string, object>();
			
			dictionary[GuildSystem.Constants.GS_ACL_INVITE] = canInvite;
			dictionary[GuildSystem.Constants.GS_ACL_PROMOTE] = canPromote;
			dictionary[GuildSystem.Constants.GS_ACL_DEMOTE] = canDemote;
			dictionary[GuildSystem.Constants.GS_ACL_KICK] = canKick;
			dictionary[GuildSystem.Constants.GS_ACL_EDIT] = canEdit;
			dictionary[GuildSystem.Constants.GS_ACL_READ] = canRead;
			dictionary[GuildSystem.Constants.GS_ACL_WRITE] = canWrite;
			dictionary[GuildSystem.Constants.GS_ACL_RANK] = rank;
			
			IList historyList = new List<IDictionary>();
			for(int i = 0; i<chatHistoryList.Count; i++)
				historyList.Add(chatHistoryList[i].ToDictionary());
			
			IList memberList = new List<IDictionary>();
			for(int i = 0; i<onlineMembers.Count; i++)
				memberList.Add(onlineMembers[i].ToDictionary());
			
			//dictionary["messages"] = historyList;
			//dictionary["members"] = memberList;
			return dictionary;
		}
		
		public bool hasPermissionToEditAnything
		{
			get
			{
				if(canInvite || canPromote || canDemote)
					return true;
				return false;
			}
			
			private set	{	}
		}
	}
}