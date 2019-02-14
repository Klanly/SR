
using System;
using System.Collections.Generic;

namespace Game
{
    public class User
    {
        public int donations { get; set; }
        public int groupID { get; set; }
        public int rank { get; set; }
		public int userID { get; set; }
		public int arcane { get; set; }
		public string username { get; set; }


        public string GetUserType()
        {
            return "Elder";
        }


		public enum TeamRank {
			Leader = 50,
			CoLeader = 51,
			Member = 52,
		}
    }

}
