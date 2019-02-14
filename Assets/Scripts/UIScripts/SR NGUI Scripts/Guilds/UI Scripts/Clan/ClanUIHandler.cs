using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Game.UI
{
    public class ClanUIHandler : Singleton<ClanUIHandler>
    {   
        public UIButton GuildButton;
        public UIButton CreateButton;
        public UIButton SearchButton;

        public UI_Guild _Listener;

        public CreateClanPanel CreateClanPanel;
        public JoinClanPanel JoinClanPanel;
        public ClanDetailPanel ClanDetailPanel;
//
//        void Awake()
//        {
//            this.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
//            if (this.GetComponent<Canvas>().worldCamera == null)
//                Debug.LogError("GUICamera is NULL");
//        }
//        
        void OnEnable()
        {
//			Debug.LogError ("ClanUIHandler-OnEnable-Team is NULL: " + (GameManager._gameState.User.team == null));
            InputWrapper.disableTouch = true;

            EnableMyClanUI(GameManager._gameState.User.team != null);
            if (GameManager._gameState.User.team == null)
            {
                ServerManager.Instance.GetMyTeam((result) => {
					Debug.LogError(" GetMyTeam response "+result);
					var _response = JsonConvert.DeserializeObject<Dictionary<string,object>>(result);
                    bool success = (bool)_response ["success"];
                    if (success)
                    {
//						Team MyTeam = JsonConvert.DeserializeObject<Team> (Response ["team"].ToString ());
                        GameManager._gameState.User.team = JsonConvert.DeserializeObject<Team>(_response ["team"].ToString());
                        EnableMyClanUI(true);
                    } else
                    {
                        GameManager._gameState.User.team = null;
//                        Debug.Log("GetMyTeam returned null.");
                    }
                });
            }
        }
//

		void Update() {
			InputWrapper.disableTouch = true;
		}

        public void GoBackToHud()
        {
            //UIManager.instance.ShowUI(GameUI.HudUI);
            this.gameObject.SetActive(false);

            _Listener.IsSearchJoinGuildOpen = false;

            InputWrapper.disableTouch = false;
        }

        public void RefreshClanInfo()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        public void BackToSearchClan()
        {
            JoinClanPanel.gameObject.SetActive(true);
            ClanDetailPanel.gameObject.SetActive(false);
        }

        void EnableMyClanUI(bool _enable)
        {
//			Debug.LogError ("ClanUIHandler-EnableMyClanUI: " + _enable + "   Clan: " + createClan.gameObject.activeInHierarchy);
            GuildButton.enabled = (_enable);
            CreateButton.enabled = (!_enable);

            CreateClanPanel.gameObject.SetActive((!_enable));
            JoinClanPanel.gameObject.SetActive(false);
            ClanDetailPanel.gameObject.SetActive(_enable);

            if (_enable)
                ClanDetailPanel.PopulateMyClan();
        }


        public void onCreateClick()
        {
            CreateClanPanel.gameObject.SetActive(true);
            JoinClanPanel.gameObject.SetActive(false);
            ClanDetailPanel.gameObject.SetActive(false);
        }

        public void onSearchClick()
        {
            CreateClanPanel.gameObject.SetActive(false);
            JoinClanPanel.gameObject.SetActive(true);
            ClanDetailPanel.gameObject.SetActive(false);
        }

        public void onMyClanClick()
        {
            CreateClanPanel.gameObject.SetActive(false);
            JoinClanPanel.gameObject.SetActive(false);
            ClanDetailPanel.gameObject.SetActive(true);

            ClanDetailPanel.PopulateMyClan();
        }

		public void OnUserStatusChangedReceived(string userInfo, bool joined) {
			ClanDetailPanel.OnUserStatusChangedReceived(userInfo, joined);
		}

		public void OnCreateButtonClicked() {
			int gems = PurchaseManager.getGemsForSouls((int)DatabankSystem.Databank.GUILD_CREATE_COST);
			if(GameManager._gameState.User._inventory.gems >= gems) {
				GameManager._gameState.User._inventory.gems -= gems;
				GameManager._gameState.User._inventory.souls += (int)DatabankSystem.Databank.GUILD_CREATE_COST;
			}
			CreateClanPanel.CreateClan();
		}
    }
}

