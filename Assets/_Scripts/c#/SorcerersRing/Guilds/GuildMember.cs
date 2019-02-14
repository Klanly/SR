using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GuildSystem
{
	public class GuildMember 
	{
		int userID;
		string username;
		string role;
		string facebookID;
		//int skullLevel;
		
		IDictionary rawDictionary;
		
		public GuildMember(IDictionary gMemberDictionary)
		{
			this.userID = System.Int32.Parse(gMemberDictionary[GuildSystem.Constants.GUILD_MEM_ID].ToString());
			if(gMemberDictionary[GuildSystem.Constants.GUILD_MEM_USERNAME] != null)
				this.username = gMemberDictionary[GuildSystem.Constants.GUILD_MEM_USERNAME].ToString();
			if(gMemberDictionary[GuildSystem.Constants.GUILD_MEM_FACEBOOK_ID] != null)
				this.facebookID = gMemberDictionary[GuildSystem.Constants.GUILD_MEM_FACEBOOK_ID].ToString();
			this.role = gMemberDictionary[GuildSystem.Constants.GUILD_MEM_ROLE].ToString();
			//this.skullLevel = System.Int32.Parse(gMemberDictionary[GuildSystem.Constants.GUILD_MEM_SKULL_LEVEL].ToString());
			this.rawDictionary = gMemberDictionary;
		}
		
		public IDictionary ToDictionary()
		{
			IDictionary dictionary = new Dictionary<string, object>();
			
			dictionary[GuildSystem.Constants.GUILD_MEM_ID] = this.userID;
			dictionary[GuildSystem.Constants.GUILD_MEM_USERNAME] = this.username;
			dictionary[GuildSystem.Constants.GUILD_MEM_FACEBOOK_ID] = this.facebookID;
			dictionary[GuildSystem.Constants.GUILD_MEM_ROLE] = this.role;
			//dictionary[GuildSystem.Constants.GUILD_MEM_SKULL_LEVEL] = this.skullLevel;
			
			return dictionary;
		}
	
	}
}