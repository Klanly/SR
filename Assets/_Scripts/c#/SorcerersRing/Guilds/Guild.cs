using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GuildSystem
{	
	public class Guild
	{
		public static Guild EMPTY; //User belongs to EMPTY guild if he isn't in any guild
		
		static Guild()
		{
			EMPTY = new Guild();
			EMPTY.guildId = -1;
		}
		
		int guildId;
		public string name;
		public int logoID;
		bool inviteOnly;
		public string description;
		public int maxMemberLimit;
		string motd;
		public int currentMemberCount;
		public long joinCost;
		public GuildStatus status;
		List<GuildMember> guildMembers;
		IDictionary rawDictionary;
		
		
		private Guild()	{	}
		
		
		public Guild(IDictionary rawDictionary)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("public Guild(IDictionary rawDictionary) >>> " + MiniJSON.Json.Serialize(rawDictionary));
			
			this.guildId = Int32.Parse(rawDictionary[GuildSystem.Constants.GUILD_OBJ_ID].ToString());
			this.name = rawDictionary[GuildSystem.Constants.NAME].ToString();
			this.logoID = Int32.Parse(rawDictionary[GuildSystem.Constants.GUILD_OBJ_LOGO_ID].ToString());
			this.inviteOnly = Boolean.Parse(rawDictionary[GuildSystem.Constants.GUILD_OBJ_INVITE_ONLY].ToString());
			this.description = rawDictionary[GuildSystem.Constants.DESCRIPTION].ToString();
			this.maxMemberLimit = Int32.Parse(rawDictionary[GuildSystem.Constants.GUILD_OBJ_MAX_MEMBERS].ToString());
			this.motd = rawDictionary[GuildSystem.Constants.MOTD].ToString();
			if(rawDictionary.Contains(GuildSystem.Constants.GUILD_OBJ_MEMBERS))
				this.guildMembers = GetMembers(rawDictionary[GuildSystem.Constants.GUILD_OBJ_MEMBERS] as IList);
			if(guildMembers != null)
				this.currentMemberCount = guildMembers.Count;
			if(rawDictionary.Contains(GuildSystem.Constants.GUILD_OBJ_JOIN_COST))
				this.joinCost = long.Parse(rawDictionary[GuildSystem.Constants.GUILD_OBJ_JOIN_COST].ToString());
			this.rawDictionary = rawDictionary;
		}
		
		
		private List<GuildMember> GetMembers(IList rawMembersList)
		{
			List<GuildMember> members = new List<GuildMember>();
			for(int i = 0; i<rawMembersList.Count;i++)
			{
				members.Add(new GuildMember(rawMembersList[i] as IDictionary));
			}
			return members;
		}
		
		
		public override string ToString()
		{
			return MiniJSON.Json.Serialize(this.rawDictionary);
		}
		
		
		public IDictionary ToDictionary()
		{
			IDictionary dictionary = new Dictionary<string, object>();
			
			dictionary[GuildSystem.Constants.GUILD_OBJ_ID] = this.guildId;
			dictionary[GuildSystem.Constants.NAME] = this.name;
			dictionary[GuildSystem.Constants.GUILD_OBJ_LOGO_ID] = this.logoID;
			dictionary[GuildSystem.Constants.GUILD_OBJ_INVITE_ONLY] = this.inviteOnly;
			dictionary[GuildSystem.Constants.DESCRIPTION] = this.description;
			dictionary[GuildSystem.Constants.GUILD_OBJ_MAX_MEMBERS] = this.maxMemberLimit;
			dictionary[GuildSystem.Constants.MOTD] = this.motd;
			if(rawDictionary != null && rawDictionary.Contains(GuildSystem.Constants.GUILD_OBJ_MEMBERS))
				dictionary[GuildSystem.Constants.GUILD_OBJ_MEMBERS] = this.rawDictionary[GuildSystem.Constants.GUILD_OBJ_MEMBERS];
			dictionary[GuildSystem.Constants.GUILD_MEM_COUNT] = this.currentMemberCount;
			if(status != null)
				dictionary["GuildStatus"] = status.ToDictionary();
			return dictionary;
		}
		
		
		public override bool Equals(object otherObject)
		{
			Guild otherGuild = otherObject as Guild;
			if(otherGuild == null)
				return false;
			if(this.guildId == otherGuild.guildId)
				return true;
			return false;
		}
		
		public static bool IsNullOrEmpty(Guild aGuild)
		{
			if(aGuild == null)
				return true;
			else if(aGuild.Equals(Guild.EMPTY))
				return true;
			return false;
		}
		

	}
}