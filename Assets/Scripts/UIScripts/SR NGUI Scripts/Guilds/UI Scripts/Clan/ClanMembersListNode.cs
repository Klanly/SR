using UnityEngine;
using System;
using System.Collections;


namespace Game.UI
{
    public class ClanMembersListNode : ItemIcon
    {
        public UISprite leagueFlag;
        public UILabel playerName;
        public UILabel memberType;
        public UILabel troophiesCount;

		public User user;
		public bool canEdit;		//change member roles

		public Action<ClanMembersListNode> action;

        public override void Refresh()
        {

        }

		public void ShowUserAction() {
			Debug.LogError("Show user action");
			if(canEdit) {
				action(this);
			}
		}

    }
}