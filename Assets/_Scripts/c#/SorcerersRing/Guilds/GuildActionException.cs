using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GuildSystem
{
	public class GuildActionException : Exception {
	
		private const string TAG = "GuildActionException :::: ";
		
		public int errorCode = 0;
		public string errorMessage = "Missing parameters or No Internet Connection";
		public static Dictionary<int, string> errorExceptionMap;
		static GuildActionException()
		{
			errorExceptionMap = new Dictionary<int, string>();
			errorExceptionMap[4] = "Guild already exists";
			errorExceptionMap[5] = "TargetID already part of a guild";
			errorExceptionMap[6] = "Guild not found";
			errorExceptionMap[7] = "No invite found for TargetID";
			errorExceptionMap[8] = "Guild reached its max member limit";
			errorExceptionMap[9] = "TargetID lacks role permissions to perform action";
			errorExceptionMap[10] = "Invite not possible as non invite only guild";
			errorExceptionMap[11] = "TargetID already invited to a guild";
			errorExceptionMap[12] = "Can't do action as both userIDs belong to different guilds";
			errorExceptionMap[13] = "UserID's rank <= TargetID's rank";
			errorExceptionMap[14] = "TargetID already at max rank";
			errorExceptionMap[15] = "TargetID already at min rank";
			errorExceptionMap[16] = "TargetID not part of a guild";
			errorExceptionMap[17] = "Invalid param found";
		}
		
		public GuildActionException() 
			: base(TAG) {	}
		
		public GuildActionException(string message) 
			: base(TAG + message) {	this.errorMessage = message;	}
		
		public GuildActionException(string message, Exception innerException)
        : base(TAG + message, innerException) {	this.errorMessage = message;	}
 
    	protected GuildActionException(SerializationInfo info, StreamingContext context)
        : base(info, context) {	}
				
		public GuildActionException(int errorCode) : base(TAG + errorExceptionMap[errorCode]) 
		{
			this.errorCode = errorCode;
			if(errorExceptionMap.ContainsKey(errorCode))
				this.errorMessage = errorExceptionMap[errorCode];
		}
	}
}