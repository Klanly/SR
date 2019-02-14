using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Game;

namespace GuildSystem
{
    public class GuildsManager : MonoBehaviour , ChatListener
    {
        private static GuildsManager _instance = null;
        private ChatManager chatManager;
        private GuildsManager()
        {
        }
		
        public static GuildsManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GuildsManager();
                return _instance;
            }
        }
		
        public UI_Guild _listener;
		
        public void OnGuildMessageReceived(ChatMessage message)
        {
			Debug.LogError("OnGuildMessageReceived - "+message.message+" time - "+message.createdTime);
            if (UI_Guild.Instance != null)
                UI_Guild.Instance.PopulateChatNode(true, message);
        }
		
        public void OnGlobalMessageReceived(ChatMessage message)
        {
            if (UI_Guild.Instance != null)
                UI_Guild.Instance.PopulateChatNode(false, message);
            else
                Debug.LogError("chatListener is null.");
        }

        public delegate void SuccessListener(bool success,string errorMessage);
		
#region Get all guilds
		
        public delegate void GuildsListListener(List<Guild> guildsList);
        private GuildsListListener guildsListListener;
		
        public void GuildsList(string guildNameCriteria, GuildsListListener guildsListListener)
        {
            this.guildsListListener = guildsListListener;
            ServerManager.Instance.ListGuilds(guildNameCriteria, this.OnGuildsListResult);
        }
		
        public void OnGuildsListResult(object responseParameters, object error, ServerRequest request)
        {
            if (error == null)
            {
                IDictionary response = responseParameters as IDictionary;
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                Debug.Log("OnGuildsListResult - MiniJSON.Json.Serialize(response)" + MiniJSON.Json.Serialize(response));
                if (responseSuccess)
                {
                    List<Guild> guildsList = new List<Guild>();
                    IList guildIList = response ["guilds"] as IList;
                    for (int i = 0; i < guildIList.Count; i++)
                    {
                        IDictionary aGuild = guildIList [i] as IDictionary;
                        guildsList.Add(new Guild(aGuild));
                    }
					
                    guildsListListener(guildsList);
                } else
                {
                    guildsListListener(null);
                    if (response.Contains("error_code"))
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())).ToString());
                    else
						if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException().ToString());
                }
					
            } else
                guildsListListener(null);
        }
		
#endregion
		
#region Create Guild
		
        private SuccessListener guildCreateListener;
        public void CreateGuild(string name, int logoID, bool inviteOnly, string description, float pointsToJoin, int maxMemberLimit, string motd, SuccessListener guildCreateListener)
        {
            this.guildCreateListener = guildCreateListener;
            ServerManager.Instance.CreateGuild(name, logoID, inviteOnly, description, pointsToJoin, maxMemberLimit, motd, this.OnGuildCreateResult);
        }
		
        public void OnGuildCreateResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildCreateListener);
        }
	
#endregion
		
#region Join Guild		
		
        private SuccessListener guildJoinListener;
        public void JoinGuild(int guildID, SuccessListener guildJoinListener)
        {
            this.guildJoinListener = guildJoinListener;
            ServerManager.Instance.JoinGuild(guildID, this.OnGuildJoinResult);
        }
		
        public void OnGuildJoinResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildJoinListener);
        }
	
#endregion
		
#region Invite To Guild		
		
        private SuccessListener guildInviteListener;
        public void InviteToGuild(int inviteeID, SuccessListener guildInviteListener)
        {
            this.guildInviteListener = guildInviteListener;
            ServerManager.Instance.InviteToGuild(inviteeID, this.OnGuildInviteResult);
        }
		
        public void OnGuildInviteResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildInviteListener);
        }
	
#endregion
		
#region Accept Invite To Guild		
		
        private SuccessListener guildInviteAcceptListener;
        public void AcceptInviteToGuild(int guildID, SuccessListener guildInviteAcceptListener)
        {
            this.guildInviteAcceptListener = guildInviteAcceptListener;
            ServerManager.Instance.AcceptInviteToGuild(guildID, this.OnGuildInviteAcceptResult);
        }
		
        public void OnGuildInviteAcceptResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildInviteAcceptListener);
        }
	
#endregion		
		
#region Promote member in Guild		
		
        private SuccessListener promoteMemberListener;
        public void PromoteMember(int promoteeID, SuccessListener promoteMemberListener)
        {
            this.promoteMemberListener = promoteMemberListener;
            ServerManager.Instance.PromoteMember(promoteeID, this.OnMemberPromoted);
        }
		
        public void OnMemberPromoted(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, promoteMemberListener);
        }
	
#endregion	
		
#region Demote member in Guild	
		
        private SuccessListener demoteMemberListener;
        public void DemoteMember(int demoteeID, SuccessListener demoteMemberListener)
        {
            this.demoteMemberListener = demoteMemberListener;
            ServerManager.Instance.DemoteMember(demoteeID, this.OnMemberDemoted);
        }
		
        public void OnMemberDemoted(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, demoteMemberListener);
        }
	
#endregion			
		
#region Kick member from Guild	
		
        private SuccessListener kickMemberListener;
        public void KickMember(int kickUserID, SuccessListener kickMemberListener)
        {
            this.kickMemberListener = kickMemberListener;
            ServerManager.Instance.KickMember(kickUserID, this.OnMemberKicked);
        }
		
        public void OnMemberKicked(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, kickMemberListener);
        }
	
#endregion					
		
#region Edit guild info		
		
        private SuccessListener guildEditListener;
        public void EditGuildInfo(int guildID, string name, int logoID, bool inviteOnly, string description, int maxMemberLimit, string motd, SuccessListener guildEditListener)
        {
            this.guildEditListener = guildEditListener;
            ServerManager.Instance.EditGuildInfo(guildID, name, logoID, inviteOnly, description, maxMemberLimit, motd, this.OnGuildEditResult);
        }

        public void OnGuildEditResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildEditListener);
        }
		
#endregion		
		
#region Leave guild 
		
        private SuccessListener guildLeaveListener;
        public void LeaveGuild(SuccessListener guildLeaveListener)
        {
            this.guildLeaveListener = guildLeaveListener;
            ServerManager.Instance.LeaveGuild(this.OnGuildLeaveResult);
        }
		
        public void OnGuildLeaveResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildLeaveListener);
        }
		
#endregion			
		
#region Guild chat login

        public delegate void GuildMessagesListener(string guildMessage);
        public delegate void GuildRawMessagesListener(object guildMessageResponse);
		
        public delegate void GuildJoinStatusListener(GuildStatus guildStatus);
        public delegate void GuildACLUpdateListener(IDictionary aclDictionary);
        public GuildACLUpdateListener _guildACLUpdateListener;
		
        private GuildJoinStatusListener guildJoinStatusListener;
        public void JoinGuildChat(GuildJoinStatusListener guildJoinStatusListener)
        {
            this.guildJoinStatusListener = guildJoinStatusListener;
            ServerManager.Instance.JoinGuildChat(this.OnGuildChatJoinResult, this.OnGuildMessageReceived, this.OnAclUpdated);
        }
		
        public void OnAclUpdated(IDictionary aclUpdateDictionary) //ACL = Access List "i think"
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("Guilds manager --- OnAclUpdated(IDictionary aclUpdateDictionary))");
            if (_guildACLUpdateListener != null)
                _guildACLUpdateListener(aclUpdateDictionary);
        }
		
        public void OnGuildChatJoinResult(object responseParameters, object error, ServerRequest request)
        {
            if (error == null)
            {
                IDictionary response = responseParameters as IDictionary;
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                if (GameManager.PRINT_LOGS)
                    Debug.Log("OnGuildsListResult - MiniJSON.Json.Serialize(response)" + MiniJSON.Json.Serialize(response));
                if (responseSuccess)
                {
                    GuildStatus guildStatus = new GuildStatus(response);
                    chatManager = new ChatManager(this, guildStatus.chatHistoryList);
                    guildJoinStatusListener(guildStatus);
                } else
                {
                    guildJoinStatusListener(null);
                    if (response.Contains("error_code"))
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())).ToString());
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException().ToString());
                }
					
            } else
                guildJoinStatusListener(null);
        }
		
        private void OnGuildMessageReceived(object rawGuildMessage)
        {
            IDictionary response = rawGuildMessage as Dictionary<string, object>;
            if (GameManager.PRINT_LOGS)
                Debug.Log("response" + MiniJSON.Json.Serialize(response));
            if (response != null && chatManager != null)
                chatManager.OnMessageReceived(new ChatHistoryObject(response));
        }
	
#endregion
		
#region Guild chat logout

		
        private SuccessListener guildChatLeaveListener;
        public void LeaveGuildChat(SuccessListener guildChatLeaveListener)
        {
            this.guildChatLeaveListener = guildChatLeaveListener;
            ServerManager.Instance.LeaveGuildChat(this.OnGuildChatLeaveResult);
        }
		
        public void OnGuildChatLeaveResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildChatLeaveListener);
        }
	
#endregion
		
#region Guild chat message

		
        private SuccessListener guildChatMessageListener;
        public void SendGuildMessage(string message, SuccessListener guildChatMessageListener)
        {
            this.guildChatMessageListener = guildChatMessageListener;
            ServerManager.Instance.SendGuildMessage(message, this.OnGuildChatMessageResult);
        }
		
        public void OnGuildChatMessageResult(object responseParameters, object error, ServerRequest request)
        {
            DispatchResult(responseParameters, error, request, guildChatMessageListener);
        }
	
#endregion		
		
#region Get User Guild
		
        public delegate void GuildRequestForUserDelegate(Guild aGuild);
        GuildRequestForUserDelegate _guildRequestForUserDelegate;
		
        public delegate void RawGuildRequestForUserDelegate(object guildObject);
		
        public void GetGuildForUserID(int userID, GuildRequestForUserDelegate _guildRequestForUserDelegate)
        {
            Debug.Log("GetGuildForUserID > " + userID);
            this._guildRequestForUserDelegate = _guildRequestForUserDelegate;
            if (userID.ToString() == GameManager.instance.fragNetworkingNew.GetUserID())
                ServerManager.Instance.GetGuildForUserID(userID, this.OnRawGuildObjectReceived);
            else
            {
                Debug.Log("Separate call needed for getting guild of a given userid...");
                ServerManager.Instance.GetGuildForUserID(userID, this.OnRawGuildObjectReceived);
            }
        }
		
        public void OnRawGuildObjectReceived(object responseParameters, object error, ServerRequest request)
        {
            IDictionary response = responseParameters as IDictionary;
            Debug.Log("response = " + MiniJSON.Json.Serialize(response));
            if (error == null)
            {
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                if (responseSuccess)
                {	
                    _guildRequestForUserDelegate(new Guild(response ["guild"] as IDictionary));
                } else
                {
                    _guildRequestForUserDelegate(Guild.EMPTY);
                    if (response.Contains("error_code"))
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())));
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException());
                }
            } else
            {
                if (response == null)
                {
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException().ToString());
                    _guildRequestForUserDelegate(null);
                } else
                {
                    if (response.Contains("error_code"))
                    {
                        if (GameManager.PRINT_LOGS)
                            Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())));
                        //TODO Show popup for error, if you want!
                        _guildRequestForUserDelegate(Guild.EMPTY);
                    } else
                    { //Case when request timed out
                        if (GameManager.PRINT_LOGS)
                            Debug.Log(new GuildActionException().ToString());
                        _guildRequestForUserDelegate(null);
                    }
                }
            }
        }	
		
#endregion
		
#region Get Guild for Guild ID
		
        public delegate void GuildRequestForIDDelegate(Guild aGuild);
        GuildRequestForIDDelegate _guildRequestForIDDelegate;
		
        public delegate void RawGuildRequestForIDDelegate(object guildObject);
		
        public void GetGuildForGuildID(int guildID, GuildRequestForIDDelegate _guildRequestForIDDelegate)
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("GetGuildForGuildID");
            this._guildRequestForIDDelegate = _guildRequestForIDDelegate;
            ServerManager.Instance.GetGuildForGuildID(guildID, this.OnRawGuildObjectForIDReceived);
        }
		
		
        //Returns 
        //'null' when could not connect to server OR
        //'Guild.EMPTY' when no guild found OR
        //'Guild' object when a guild exists
        public void OnRawGuildObjectForIDReceived(object responseParameters, object error, ServerRequest request)
        {
            if (error == null)
            {
                IDictionary response = responseParameters as IDictionary;
                if (GameManager.PRINT_LOGS)
                    Debug.Log("response = " + MiniJSON.Json.Serialize(response));
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                if (responseSuccess)
                    _guildRequestForIDDelegate(new Guild(response ["guild"] as IDictionary));
                else
                {
                    _guildRequestForIDDelegate(Guild.EMPTY);
                    if (response.Contains("error_code"))
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())));
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException());
                }
            } else
            {
                _guildRequestForIDDelegate(null);
                if (GameManager.PRINT_LOGS)
                    Debug.Log(new GuildActionException().ToString());
            }
        }	
		
#endregion		
		
#region Get GuildMembers in a guild
		
        public delegate void GuildMembersRequestForGuildDelegate(List<GuildMember>  guildMembers);
        GuildMembersRequestForGuildDelegate _guildMembersRequestForGuildDelegate;
		
        public delegate void RawGuildMembersRequestForGuildDelegate(object guildObject);
		
        public void GetGuildMembersForGuildID(int guildID, GuildMembersRequestForGuildDelegate _guildMembersRequestForGuildDelegate)
        {
            this._guildMembersRequestForGuildDelegate = _guildMembersRequestForGuildDelegate;
            ServerManager.Instance.GetGuildMembersForGuildID(guildID, this.OnRawGuildMembersReceived);
        }
		
        public void OnRawGuildMembersReceived(object responseParameters, object error, ServerRequest request)
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("OnRawGuildMembersReceived");
            if (error == null)
            {
                IDictionary response = responseParameters as IDictionary;
                if (GameManager.PRINT_LOGS)
                    Debug.Log("response = " + MiniJSON.Json.Serialize(response));
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                if (responseSuccess)
                {
                    List<GuildMember> members = new List<GuildMember>();
					
                    IList membersList = response ["guildMemberships"] as IList;
                    for (int i = 0; i<membersList.Count; i++)
                        members.Add(new GuildMember(membersList [i] as IDictionary));
					
                    _guildMembersRequestForGuildDelegate(members);
                } else
                {
                    _guildMembersRequestForGuildDelegate(null);
                    if (response.Contains("error_code"))
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())).ToString());
                    if (GameManager.PRINT_LOGS)
                        Debug.Log(new GuildActionException().ToString());
                }
            } else
            {
                _guildMembersRequestForGuildDelegate(null);
                if (GameManager.PRINT_LOGS)
                    Debug.Log(new GuildActionException().ToString());
            }
        }
		
        public void MarkMessageAsRead()
        {
            chatManager.MarkRead();
        }

        public void PopulateChatNode(bool isClan, ChatMessage chatMessage)
        {

        }
#endregion
		
		
        private void DispatchResult(object responseParameters, object error, ServerRequest request, SuccessListener dispatchDelegate)
        {
            if (GameManager.PRINT_LOGS)
                Debug.Log("DispatchResult");
            if (error == null)
            {
                IDictionary response = responseParameters as IDictionary;
                if (GameManager.PRINT_LOGS)
                    Debug.Log("response = " + MiniJSON.Json.Serialize(response));
                bool responseSuccess = bool.Parse(response ["success"].ToString());
				
                if (responseSuccess)
                    dispatchDelegate(true, null);
                else
                {
                    if (response.Contains("error_code"))
                        dispatchDelegate(false, new GuildActionException(System.Int32.Parse(response ["error_code"].ToString())).errorMessage);
                    else
                        dispatchDelegate(false, new GuildActionException().errorMessage);
                }
            } else
                dispatchDelegate(false, new GuildActionException().errorMessage);
        }



        #region CLAN SYSTEM calls
        public void CreateTeam(string teamName, string teamDescription, int teamFlag, int teamType, int requiredTrophies, int warFrequency, string teamLocation, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.CreateTeam(teamName, teamDescription, teamFlag, teamType, requiredTrophies, warFrequency, teamLocation, OnSuccess, OnFailure);
        }
        
        public void ListTeams(Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.ListTeams(OnSuccess = null, OnFailure);
        }
        
        public void GetMyTeam(Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.GetMyTeam(OnSuccess, OnFailure);
        }
        
        public void SearchTeam(string teamName, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.SearchTeam(teamName, OnSuccess, OnFailure);
        }
        
        public void JoinTeam(long teamID, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.JoinTeam(teamID, OnSuccess, OnFailure);
        }
        
        public void LeaveTeam(Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.LeaveTeam(OnSuccess, OnFailure);
        }
        
        public void KickUser(long affecteeID, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.KickUser(affecteeID, OnSuccess, OnFailure);
        }
        
        public void SendGlobalChatMessage(string message, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.SendGlobalChatMessage(message, OnSuccess, OnFailure);
        }
        
        public void SendTeamChat(string message, Action<string> OnSuccess = null, Action OnFailure = null)
        {
            ServerManager.Instance.SendTeamChat(message, OnSuccess, OnFailure);
        }
        
		public void GetTeamChats(long userID, Action<string> OnSuccess = null, Action OnFailure = null)
		{
			ServerManager.Instance.GetTeamChats(userID, OnSuccess, OnFailure);
		}
		public void GetGlobalChats(Action<string> OnSuccess = null, Action OnFailure = null)
		{
			ServerManager.Instance.GetGlobalChats(OnSuccess, OnFailure);
		}

		#endregion CLAN SYSTEM calls
	
    }
}
