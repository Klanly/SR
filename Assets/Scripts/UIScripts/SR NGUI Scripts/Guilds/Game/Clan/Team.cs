
using System;
using System.Collections.Generic;

namespace Game
{
    public class Team
    {
        public long updatedTime { get; set; }

        public object joinRequests { get; set; }

        public int creatorID { get; set; }

        public string description { get; set; }

        // TODO: Implement After Implementaion on the Server side.
        public object inviteRequests { get; set; }

        public int type { get; set; }

        public Dictionary<long,User> users { get; set; }

        public Stats stats { get; set; }

        public int appID { get; set; }

        public int requiredTrophies { get; set; }

        public int teamID { get; set; }

        public string name { get; set; }

        public long createdTime { get; set; }

		public int teamFlag { get; set; }
    }

}
