using UnityEngine;
using System.Collections;

namespace Game
{
    public class ChatMessageResponse
    {
        //public string ACTION_MESSAGE;
		public long serverTimeStamp;
        public string action;
        public ChatMessage chat;
    }

    public class ChatMessage
    {
        public long updatedTime { get; set; }

        public int senderId { get; set; }

        public int function { get; set; }

        public long createdTime { get; set; }

        public string message { get; set; }

        public string senderName { get; set; }

		public User.TeamRank groupId {get; set; }

		public FunctionData functionData {get; set; }
    }

	public class FunctionData 
	{
		public User.TeamRank rank {get; set; }
		public string teamName {get; set; }
	}
}