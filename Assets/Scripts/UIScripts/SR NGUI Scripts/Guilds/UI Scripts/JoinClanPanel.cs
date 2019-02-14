using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Game.UI
{
    public class JoinClanPanel : MonoBehaviour
    {
        public GameObject ClanSearchResultScrollView;
        public GameObject ClanSearchResultListNode;

		private UIGrid grid;

		void Awake () {
			grid = ClanSearchResultScrollView.GetComponent<UIGrid>();
		}

        void OnEnable()
        {
            //ClearClanSearchList();
		}

        public void SearchClan(UIInput clanName)
        {
            ClearClanSearchList();
			if(clanName.text.Trim().Equals("")) {
				Debug.LogError("clan name empty");
				return;
			}
            ServerManager.Instance.SearchTeam(clanName.text, (response) => {
                ClanSearchResult clanSearchResult = JsonConvert.DeserializeObject<ClanSearchResult>(response);
                if (clanSearchResult.success)
                {
                    foreach (var team in clanSearchResult.teams)
                        PopulateClanSearchList(team);
                } else
                    Debug.LogError("Unable to get the Search Result: " + response.ToString());
            
            }, () => {});
        }

        public void PopulateClanSearchList(Team team)
        {
            ClanSearchListNode clanSearchListNode = (Instantiate(ClanSearchResultListNode) as GameObject).GetComponent<ClanSearchListNode>();
        
            if (team == null)
                Debug.LogError("Team is NULL");

            clanSearchListNode.Team = team;

            // TODO: Populated All the Fields after the Server Implementations.
            clanSearchListNode.clanName.text = team.name;
            clanSearchListNode.clanMembersCount.text = team.users.Count.ToString();
			clanSearchListNode.trophiesCount.text = team.requiredTrophies.ToString();

            clanSearchListNode.gameObject.transform.SetParent(ClanSearchResultScrollView.transform);
//            clanSearchListNode.gameObject.transform.localPosition = Vector3.zero;
//			clanSearchListNode.transform.localPosition = new Vector3(0, clanSearchListNode.transform.localPosition.y, 0);

			if(grid == null) {
				grid = ClanSearchResultScrollView.GetComponent<UIGrid>();
			}
            grid.Reposition();
            //ClanSearchResultScrollView.GetComponentInParent<UIScrollView>().ResetPosition();

            clanSearchListNode.gameObject.transform.localScale = Vector3.one;
            clanSearchListNode.transform.SetAsFirstSibling();
        }
        
		public void ClearClanSearchList()
        {
            foreach (Transform child in ClanSearchResultScrollView.transform)
                GameObject.Destroy(child.gameObject);
        }

    }
}
