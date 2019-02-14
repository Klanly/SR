using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Game;

namespace Game.UI
{
    public class ClanDetailPanel : MonoBehaviour
    {
        public GameObject BackButton;
        public GameObject ClanMembersListNode;
		public GameObject ClanMembersScrollView;
		public UIScrollView ClanMembersScrollViewReference;

		public UILabel clanName;
        public UILabel clanDescription;
		public UISprite clanFlag;

        public GameObject JoinButton;
        public GameObject LeaveButton;

		
		public UIButton button1;
		public UIButton button2;
		public UISprite detailPanel;

        private int teamId;
		private int teamFlagId;

		private User myUser;
		private UIGrid grid;

		void Awake () {
			grid = ClanMembersScrollView.GetComponent<UIGrid>();
			grid.sorting = UIGrid.Sorting.Custom;
			grid.onCustomSort += customSort;
			Debug.LogError("grid init done - "+(grid == null ? "null": "not null"));
		}

        void OnEnable()
        {
            ServerManager.Instance.GetMyTeam((result) => {
				Debug.LogError(" GetMyTeam response "+result);
				var _response = JsonConvert.DeserializeObject<Dictionary<string,object>>(result);
                bool success = (bool)_response ["success"];
                if (success)
                {
//					Team MyTeam = JsonConvert.DeserializeObject<Team> (Response ["team"].ToString ());
                    GameManager._gameState.User.team = JsonConvert.DeserializeObject<Team>(_response ["team"].ToString());
					myUser = GameManager._gameState.User.team.users[int.Parse(GameManager.instance.fragNetworkingNew.GetUserID ())];
                } else
                {
                    GameManager._gameState.User.team = null;
                }
            });

		}

        public void JoinClan()
        {
            ServerManager.Instance.JoinTeam(teamId, (result) => {

                ClanData clanData = JsonConvert.DeserializeObject<ClanData>(result);
                
                if (clanData.success == false)
                {
                    Debug.LogError("Unable to Join a Team: " + result);
					UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error" , "Insufficent Arcane points", () => {});
					return;
                }
                
                GameManager._gameState.User.team = clanData.team;
                ClanUIHandler.instance.ClanDetailPanel.PopulateMyClan();
                ClanUIHandler.instance.RefreshClanInfo();
                ClanUIHandler.instance._Listener.SetGuild();
            }, () => {
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error" , "Please try again", () => {});
			});
        }
    
        public void LeaveClan()
        {
			if(!GameManager.instance.fragNetworkingNew.isInternet) {
				PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to leave guild", () => {});
				return;
			}

            ServerManager.Instance.LeaveTeam((result) => {
                ClanUIHandler.instance.ClanDetailPanel.gameObject.SetActive(false);
                ClanUIHandler.instance.CreateClanPanel.gameObject.SetActive(true);
            }, () => {});
            GameManager._gameState.User.team = null;

            ClanUIHandler.instance.RefreshClanInfo();
            ClanUIHandler.instance._Listener.SetGuild();
        }
    
        public void PopulateMyClan()
        {
            PopulateClan(GameManager._gameState.User.team, true);

            JoinButton.SetActive(false);
            LeaveButton.SetActive(true);
        }

        public void PopulateClan(Team team, bool myTeam = false)
        {
            if (team == null)
                return;

            ClearClanSearchList();

			// TODO: Populate Complete Data.
            teamId = team.teamID;
            clanName.text = team.name;
            clanDescription.text = team.description;
			clanFlag.spriteName = "image "+team.teamFlag;
			teamFlagId = team.teamFlag;

//			bool canEdit = false;
//			if(teamId == GameManager._gameState.User.team.teamID) {
//				canEdit = true;
//			}

            foreach (var user in team.users) {
				PopulateClanMembersList(user.Value, myTeam);
			}
			grid.Reposition();

			// TODO: Handle the Buttons according to the Team Data.
            JoinButton.SetActive(GameManager._gameState.User.team == null && team.type == 1);
            //BackButton.SetActive(GameManager._gameState.User.team == null);

            LeaveButton.SetActive(GameManager._gameState.User.team == team);

            BackButton.SetActive(GameManager._gameState.User.team != team);

        }
    
        public void PopulateClanMembersList(User user, bool myTeam)
        {
            ClanMembersListNode clanMembersListNode = (Instantiate(ClanMembersListNode) as GameObject).GetComponent<ClanMembersListNode>();
            
            // TODO: Populated All the Fields after the Server Implementations.
            clanMembersListNode.playerName.text = user.username;
//			Debug.LogError("group id = "+user.groupID+" "+user.arcane.ToString());
			clanMembersListNode.memberType.text = ((User.TeamRank)user.groupID).ToString();
			clanMembersListNode.troophiesCount.text = user.arcane.ToString() ;
			clanMembersListNode.leagueFlag.spriteName = "image "+teamFlagId;
			clanMembersListNode.user = user;
			clanMembersListNode.action = OnMemberClicked;
			clanMembersListNode.canEdit = myTeam;

            clanMembersListNode.gameObject.transform.SetParent(ClanMembersScrollView.transform);
            clanMembersListNode.gameObject.transform.localPosition = new Vector3(clanMembersListNode.gameObject.transform.localPosition.x, clanMembersListNode.gameObject.transform.localPosition.y, 0);
            clanMembersListNode.gameObject.transform.localScale = Vector3.one;
            clanMembersListNode.transform.SetAsFirstSibling();
			if(grid == null) {
				grid = ClanMembersScrollView.GetComponent<UIGrid>();
				grid.sorting = UIGrid.Sorting.Custom;
				grid.onCustomSort += customSort;
			}
//			Debug.LogError("Before - "+clanMembersListNode.GetComponent<UIWidget>().depth);
//			NGUITools.BringForward(clanMembersListNode.gameObject);
//			Debug.LogError("After - "+clanMembersListNode.GetComponent<UIWidget>().depth);

            grid.Reposition();
            //clanMembersListNode.GetComponentInParent<UIScrollView>().ResetPosition();
        }
//		public int offset = 100;
		private int customSort (Transform a, Transform b) {
			int arcaneA = int.Parse(a.GetComponent<ClanMembersListNode>().troophiesCount.text);
			int arcaneB = int.Parse(b.GetComponent<ClanMembersListNode>().troophiesCount.text);
			return arcaneB.CompareTo(arcaneA);
		}

//		private void OnMemberClicked(User user) {
		private void OnMemberClicked(ClanMembersListNode clanMember) {
			if(detailPanel.gameObject.activeSelf) {
				Debug.LogError("panel already visible - ");
				detailPanel.gameObject.SetActive(false);
				return;
			}
			User user = clanMember.user;
			if(user.userID.ToString().Equals( GameManager.instance.fragNetworkingNew.GetUserID())) {
				Debug.LogError("Clicked on MyUser ");
//				return;
			}

			Debug.LogError("User clicked = "+user.userID + " "+GameManager.instance.fragNetworkingNew.GetUserID ());

			Debug.LogError("myUser = "+myUser.groupID+" user = "+user.groupID);
			if(myUser == null || myUser.groupID >= user.groupID) {
				return;
			}

			detailPanel.gameObject.SetActive(true);
//			Vector3 position = clanMember.transform.position;
			Vector3 nodePosition = clanMember.transform.localPosition;
//			UITexture texture = GetComponent<UITexture>();
			int offset = 75;

			nodePosition.y += offset;
			detailPanel.transform.localPosition = nodePosition;
			Debug.LogError(detailPanel.gameObject.activeSelf+" " +detailPanel.transform.localPosition);

			button1.onClick.Clear();
			button2.onClick.Clear();

			if((User.TeamRank)user.groupID == User.TeamRank.Member) {
				button1.transform.GetChild(0).GetComponent<UILabel>().text = "PROMOTE";
				EventDelegate eventDelegate = new EventDelegate(this, "PromoteUser");
				eventDelegate.parameters[0].value = user;
				button1.onClick.Add(eventDelegate);
				button2.transform.GetChild(0).GetComponent<UILabel>().text = "KICK";
				eventDelegate = new EventDelegate(this, "KickUser");
				eventDelegate.parameters[0].value = user;
				button2.onClick.Add(eventDelegate);
			} else if((User.TeamRank)user.groupID == User.TeamRank.CoLeader) {
				button1.transform.GetChild(0).GetComponent<UILabel>().text = "PROMOTE";
				EventDelegate eventDelegate = new EventDelegate(this, "PromoteUser");
				eventDelegate.parameters[0].value = user;
				button1.onClick.Add(eventDelegate);
				button2.transform.GetChild(0).GetComponent<UILabel>().text = "DEMOTE";
				eventDelegate = new EventDelegate(this, "DemoteUser");
				eventDelegate.parameters[0].value = user;
				button2.onClick.Add(eventDelegate);
			} else if((User.TeamRank)user.groupID == User.TeamRank.Leader) {
//				detailPanel.gameObject.SetActive(false);
			}
			NGUITools.BringForward(detailPanel.gameObject);
			Debug.LogError("DetailPanel - "+detailPanel.gameObject.activeSelf);
		}

		public void OnMemberRankChanged(object responseParameters, object error, ServerRequest request) {
			if (error == null)
			{
				IDictionary response = responseParameters as IDictionary;
				Debug.Log("OnMemberRankChanged response = " + MiniJSON.Json.Serialize(response));
				bool responseSuccess = bool.Parse(response ["success"].ToString());
				Debug.LogError("responseSuccess for OnMemberRank Changed = "+responseSuccess);
			}
		}

		void DemoteUser(User user) {
			detailPanel.gameObject.SetActive(false);
			Debug.LogError("DEMOTE DEM USER: "+user.username);

			ServerManager.Instance.DemoteMember(user.userID, OnMemberRankChanged);
		}

		void PromoteUser(User user) {
			detailPanel.gameObject.SetActive(false);
			Debug.LogError("PROMOTE DEM USER: "+user.username);
			ServerManager.Instance.PromoteMember(user.userID, OnMemberRankChanged);
		}

		void KickUser(User user) {
			detailPanel.gameObject.SetActive(false);
			Debug.LogError("KICK DEM USER: "+user.username);
			ServerManager.Instance.KickMember(user.userID, OnMemberRankChanged);

		}


		public void OnUserStatusChangedReceived(string userInfo, bool joined) {
			Debug.LogError(userInfo +" - joined ");

			User user = JsonConvert.DeserializeObject<User>(userInfo);
			if((User.TeamRank)myUser.groupID == User.TeamRank.Leader && (User.TeamRank)user.groupID == User.TeamRank.Leader)  {
				myUser.groupID = (int)User.TeamRank.CoLeader;
				GameManager._gameState.User.team.users.Remove(myUser.userID);
				GameManager._gameState.User.team.users.Add(myUser.userID, myUser);
				ClearClanUser(myUser);
				PopulateClanMembersList(myUser, true);
			}

			if(joined) {
				GameManager._gameState.User.team.users.Remove(user.userID);
				GameManager._gameState.User.team.users.Add(user.userID, user);
			} else {
				GameManager._gameState.User.team.users.Remove(user.userID);
			}
			ClearClanUser(user);
			PopulateClanMembersList(user, true);
		}

        public void ClearClanSearchList()
        {
            foreach (Transform child in ClanMembersScrollView.transform)
                GameObject.Destroy(child.gameObject);
        }

		public void ClearClanUser(User user) {
			foreach (Transform child in ClanMembersScrollView.transform)
			{
				ClanMembersListNode node = child.GetComponent<ClanMembersListNode>();
				if(node.user.userID == user.userID) {
					Destroy(child.gameObject);
					Debug.LogError("deleting - "+user.userID+" username = "+user.username);
					ClanMembersScrollView.GetComponent<UIGrid>().Reposition();
				}
			}
		}
		
		void OnDisable()
        {
            ClearClanSearchList();
        }
    }
}
