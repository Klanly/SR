using UnityEngine;
using System.Collections;

namespace GuildSystem
{
	public class Constants 
	{
		public const string NAME = "name";
		public const string LOGO_ID = "logo_id";
		public const string INVITE_ONLY = "invite_only";
		public const string DESCRIPTION = "description";
		public const string MAX_MEMBER_LIMIT = "max_member_limit";
		public const string MOTD = "motd";
		
		public const string GUILD_ID = "guildID";
		public const string INVITEE_ID = "inviteeID";
		public const string PROMOTEE_ID = "promoted_id";
		public const string DEMOTEE_ID = "demoted_id";
		public const string KICK_USER_ID = "affectee_id";
		
		public const string GUILD_OBJ_ID = "id";
		public const string GUILD_OBJ_NAME = "name";
		public const string GUILD_OBJ_LOGO_ID = "logoID";
		public const string GUILD_OBJ_INVITE_ONLY = "inviteOnly";
		public const string GUILD_OBJ_DESCRIPTION = "description";
		public const string GUILD_OBJ_MAX_MEMBERS = "maxMemberLimit";
		public const string GUILD_OBJ_MEMBERS = "members";
		public const string GUILD_OBJ_JOIN_COST = "joinCost";
		public const string GUILD_OBJ_MOTD = "motd";
		public const string GUILD_OBJ_CREATOR_ID = "creatorID";
		
		
		public const string GUILD_MEM_ID = "userID";
		public const string GUILD_MEM_USERNAME = "username";
		public const string GUILD_MEM_ROLE = "role";
		public const string GUILD_MEM_SKULL_LEVEL = "skullLevel";
		public const string GUILD_MEM_FACEBOOK_ID = "facebookID";
		
		public const string GUILD_MEM_COUNT = "currentMemberCount";
		
#region Chat Related
		public const string CHAT_MESSAGE_TYPE = "messageType";
		public const string CHAT_MESSAGE = "message";
		public const string CHAT_MESSAGE_TIMESTAMP = "timestamp";
		public const string CHAT_MESSAGE_GUILD_ID = "guildID";
		public const string CHAT_MESSAGE_FROM = "from";
#endregion
		
#region Guild Status (GS = Guild Status)
		public const string GS_ACL = "acl";
		public const string GS_ACL_RANK = "rank";
		public const string GS_ACL_INVITE = "invite";
		public const string GS_ACL_PROMOTE = "promote";
		public const string GS_ACL_DEMOTE = "demote";
		public const string GS_ACL_KICK = "kick";
		public const string GS_ACL_EDIT = "edit";
		public const string GS_ACL_READ = "read";
		public const string GS_ACL_WRITE = "write";
		public const string GS_ACL_ROLE_NAME = "roleName";
		public const string GS_CHAT_HISTORY = "chatHistory";
		public const string GS_ONLINE_MEMBERS = "onlineMembers";
#endregion		
	}
}
