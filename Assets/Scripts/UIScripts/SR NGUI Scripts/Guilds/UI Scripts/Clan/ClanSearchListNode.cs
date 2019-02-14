using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Game.UI
{
    public class ClanSearchListNode : ItemIcon
    {
        public Team Team;

        public UISprite clanFlag;
        public UILabel clanName;
        public UILabel clanMembersCount;
        public UILabel trophiesCount;

        public override void Refresh()
        {
            clanName.text = Team.name;
            trophiesCount.text = Team.requiredTrophies.ToString();
			clanFlag.spriteName = "image "+Team.teamFlag;
			Debug.LogError("team required trophiess "+Team.requiredTrophies);
        }

        public void ShowTeamDetails()
        {
            Debug.Log(Team.name);
            ClanUIHandler.instance.JoinClanPanel.gameObject.SetActive(false);
            ClanUIHandler.instance.ClanDetailPanel.PopulateClan(Team);
            ClanUIHandler.instance.ClanDetailPanel.gameObject.SetActive(true);
        }
    }
}