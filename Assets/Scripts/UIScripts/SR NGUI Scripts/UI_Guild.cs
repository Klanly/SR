using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GuildSystem;
using Game.UI;
using Game;
using Newtonsoft.Json;
using MiniJSON;

public class UI_Guild : MonoBehaviour, ChatListener
{
    public const string GUILD_SWF_NAME = "guilds.swf";
	
    //protected Value guildUI = null;
    private GuildsManager guildManager;
    System.Action ack;
	
    public bool IsSearchJoinGuildOpen = false;

    public static UI_Guild Instance;

    public GameObject _noGuildPanel;
    public GameObject _connectedToGuildChatPanel;
    public GameObject _connectedToGlobalChatPanel;

    public GameObject chatListNode;

    [HideInInspector]
    public List<chatrow>
        GlobalChatList;
    [HideInInspector]
    public List<chatrow>
        ClanChatList;
    public GameObject globalChatScrollView;
    public GameObject clanChatScrollView;

    [HideInInspector]
    public ClanUIHandler
        _joinCreateSearchUI = null;

    void Awake()
    {
        Instance = this;
    }

    protected void Start()
    {
        guildManager = GuildsManager.Instance;
        guildManager._listener = this;
        SetGuild();
    }

    public void SetGuild()
    {
        _allowChatPanel = false;
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("UI_Guild:::::::::::::::::::::::::::::::SetGuild()");
        //guildManager.GetGuildForUserID(Int32.Parse(GameManager.instance.fragNetworkingNew.GetUserID()), this.OnGuildReceived);
		PopulateGlobalChat();
        ServerManager.Instance.GetMyTeam((result) => {
            var _response = JsonConvert.DeserializeObject<Dictionary<string,object>>(result);
			Debug.LogError(" GetMyTeam response "+result);
            bool success = (bool)_response ["success"];
            if (success)
            {
                //                  Team MyTeam = JsonConvert.DeserializeObject<Team> (Response ["team"].ToString ());
                GameManager._gameState.User.team = JsonConvert.DeserializeObject<Team>(_response ["team"].ToString());

                enableChat();
				PopulateChat();
                //SetGuildInfo(
            } else
            {
                GameManager._gameState.User.team = null;

                //SetGuildInfo(null);
                disableChat();
            }
            OnGuildChatButtonClick();
        });
        
        OnGuildChatButtonClick();
    }
	
	
    public void OnGuildReceived(Guild aGuild)
    {	
        GuildStatus currentStatus = null;
        if (GameManager._gameState.User.guild != null)
            currentStatus = GameManager._gameState.User.guild.status;
        GameManager._gameState.User.guild = aGuild;
        if (aGuild == null) 					//No response from server - set user guild null...
        {
            Debug.Log("No response from server - set user guild null...");
            SetGuildInfo(null);
            disableChat();
        } else
        {
            if (aGuild.Equals(Guild.EMPTY)) //User not in any guild
            {
                Debug.Log("User not in any guild");

                enableChat();

                SetGuildInfo(MiniJSON.Json.Serialize(aGuild.ToDictionary()));
				
            } else 							//User alredy in a guild
            {
                Debug.Log("User alredy in a guild");

                if (isChatLoggedIn)
                {
                    GameManager._gameState.User.guild.status = currentStatus;
                    SetGuildInfo(MiniJSON.Json.Serialize(aGuild.ToDictionary()));
                } else
                    JoinGuildChat();
            }
        }

        OnGuildChatButtonClick();
    }
	
    private bool isChatLoggedIn = false;
    private void JoinGuildChat()
    {
        if (!Guild.IsNullOrEmpty(GameManager._gameState.User.guild))
        {
            ClearChat();
            guildManager.JoinGuildChat((guildStatus) => {
                if (guildStatus != null)
                {
                    isChatLoggedIn = true;
                    GameManager._gameState.User.guild.status = guildStatus;
                    Debug.Log("::::::::::::::::::::::::: GUILD STATUS SET ::::::::::::::::::::)" + MiniJSON.Json.Serialize(GameManager._gameState.User.guild.status.ToDictionary()));
                    guildManager._guildACLUpdateListener = OnACLChanged;		//Set listener for ACL push-notification updates
                    HideUI();
                    enableChat();
                    SetGuildInfo(MiniJSON.Json.Serialize(GameManager._gameState.User.guild.ToDictionary()));
                    ShowUserAccessPanel(guildStatus.hasPermissionToEditAnything);	//This doesn't need to be there anymore!
					PopulateChat();
                }
            });
        }
    }
	
    public void SetGuildInfo(string guildString)
    {
        HideUI();
        /*
		Debug.Log("SetGuildInfo : : : " + guildString);
		Value val1 = new Value(guildString, MovieID);
		Value[] args={val1};
		guildUI.Invoke("setGuildInfo",args);
		*/
    }
	
#region GUILD 
	
    private void OnACLChanged(IDictionary aclDictionary)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SCENE _ LEVEL _ OnACLChanged");
        GameManager._gameState.User.guild.status.UpdateACL(aclDictionary);
    }
	
#region Guild chat	
    private bool _allowChatPanel = false;
    private bool _sliderOpen = false;
    public void enableChat() // Allows chat window to open on button tap
    {
        _allowChatPanel = true;
        /*
		Value[] args={};
		guildUI.Invoke("enableChat",args);
		*/
    }
	
    public void disableChat() // Disallows chat window to open
    {
        _allowChatPanel = false;
        /*
		Value[] args={};
		guildUI.Invoke("disableChat",args);
		*/
    }
	
    public void chatCallback()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("if(GameManager.PRINT_LOGS) Debug.Log(onChatIcon());");
		
        SetGuild();
		
        Guild userGuild = GameManager._gameState.User.guild;
        if (!Guild.IsNullOrEmpty(userGuild) && userGuild.status != null && !userGuild.status.isAclUpdated)
        {
            SetGuildInfo(MiniJSON.Json.Serialize(userGuild.ToDictionary()));
            userGuild.status.isAclUpdated = true;
        }
    }
	
    public void chatIsOpen()
    {
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("chatIsOpen");
        InputWrapper.disableTouch = true;
    }
	
    public void chatIsClose()
    {
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("chatIsClose");
        InputWrapper.disableTouch = false;
        SetGuild();
    }
	
    public void IsChatPanelOpen()
    {
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("IsChatPanelOpen()");
        /*
		Value[] args={};
		guildUI.Invoke("isChatPanelOpen",args);
		*/
    }
	
    public void chatPanelStatus(bool isOpen) // CB for "isChatPanelOpen" function
    {
        /*if(GameManager.PRINT_LOGS)*/ 
        Debug.Log("chatPanelStatus : : : " + isOpen);
        if (isOpen)
            guildManager.MarkMessageAsRead();
    }
	
    public void UpdateMessageCount(int newCount)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("UpdateMessageCount : : : " + newCount);
        /*
		Value val1 = new Value(newCount,MovieID);
		Value[] args={val1};
		guildUI.Invoke("updateMessageCount",args);
		*/
    }

	#region NGUI Callbacks

    public void OnGuildChatButtonClick()
    {
        SetGuildChatPanel();
        _connectedToGlobalChatPanel.SetActive(false);

        Debug.Log("OnGuildChatButtonClick");
    }

    public void OnGlobalChatButtonClick()
    {
        _noGuildPanel.SetActive(false);
        _connectedToGuildChatPanel.SetActive(false);
        _connectedToGlobalChatPanel.SetActive(true);

        Debug.Log("OnGlobalChatButtonClick");
    }

    private void SetGuildChatPanel()
    {
        _noGuildPanel.SetActive(!_allowChatPanel);
        _connectedToGuildChatPanel.SetActive(_allowChatPanel);
    }

    //Called when the guild icon (left side of the screen) is pressed (in either open or closed state)...
    public void ChatSliderClick()
    {
        //SetGuildChatPanel();
        Debug.Log("ChatSliderClick, _sliderOpen: " + _sliderOpen);

        if (_sliderOpen)
            CloseChatSlider();
        else
            OpenChatSlider();
    }

    public void JoinOnMainClick()
    {
        IsSearchJoinGuildOpen = true;
        CloseChatSlider();
        if (_joinCreateSearchUI == null)
        {
            UnityEngine.Object asset = Resources.Load(GuildJoinCreateSearchUI.PREFAB_PATH);
            _joinCreateSearchUI = NGUITools.AddChild(this.gameObject, asset as GameObject).GetComponent<ClanUIHandler>();
            _joinCreateSearchUI._Listener = this;
        } else
        {
            _joinCreateSearchUI.gameObject.SetActive(true);
        }
    }
	#endregion

    public void OpenAnimationFinished()
    {
//        _sliderOpen = true;
//        InputWrapper.disableTouch = true;
    }

    public void CloseAnimationFinished()
    {
//        _sliderOpen = false;
//        if (!IsSearchJoinGuildOpen)
//            InputWrapper.disableTouch = false;
    }

    private void OpenChatSlider()
    {
        Debug.Log("OpenChatSlider");
        if (!_sliderOpen)
        {
            var widget = GetComponentInChildren<UIWidget>();
            if (widget != null)
            {
                widget.leftAnchor.target = null;
                widget.rightAnchor.target = null;
                widget.topAnchor.target = null;
                widget.bottomAnchor.target = null;
                
                widget.ResetAndUpdateAnchors();
		    	InputWrapper.disableTouch = true;
				_sliderOpen = !_sliderOpen;
			}

            GetComponent<Animation>().Play("OpenChatPanel");
        }
    }

    public void CloseChatSlider()
    {
        Debug.Log("CloseChatSlider");
        if (_sliderOpen) {
	        if (!IsSearchJoinGuildOpen)
    	        InputWrapper.disableTouch = false;
			_sliderOpen = !_sliderOpen;
            GetComponent<Animation>().Play("CloseChatPanel");
		}
    }



#region	Send/receive messages
	
    public void userSendMessageFocus()
    {
        /*if(GameManager.PRINT_LOGS) */
        Debug.Log("onNameTextFocus");
        GameManager.instance.keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        GameManager.instance.keyBoardInputListener = getUserSendMessage;
    }
	
    public void SetUserSendMessage(string text)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetUserSendMessage string text" + text);
        /*
		Value val = new Value(text, MovieID);
		Value[] args = {val};
		guildUI.Invoke("setUserSendMessage", args);
		*/
    }

    public void getUserSendMessage(string messageString)
    {
        GuildSystem.GuildsManager.Instance.SendGuildMessage(messageString, (success, errorMessage) => {
            if (success)
            if (GameManager.PRINT_LOGS)
                Debug.Log("MESSAGE SENT SUCCESFULLY TO GUILD :D");
            else
			if (GameManager.PRINT_LOGS)
                Debug.Log("MESSAGE failed  :( ------ with error = " + errorMessage);
        });
    }
	
    public void SendUserMessage(string jsonStringForAllMessages)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SendUserMessage : : : " + jsonStringForAllMessages);
        /*
		Value val1 = new Value(jsonStringForAllMessages, MovieID);
		Value[] args={val1};
		guildUI.Invoke("sendUserMessage",args);
		*/
    }
	
#endregion	

#endregion	
	
	
	
    public void OnNewMessageReceived(ChatHistoryObject newMessage)
    {
//		GameManager.instance._levelManager.chatMessages = updatedMessageList;
        if (GameManager.PRINT_LOGS)
            Debug.Log("newMessage.ToDictionary() :::::::::::::::::::::::::::::::::: " + MiniJSON.Json.Serialize(newMessage.ToDictionary()));
        SendUserMessage(MiniJSON.Json.Serialize(newMessage.ToDictionary()));
    }


    public void joinCallBack() //CB when user has no guild & he taps the 'join guild' area
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("joinCallBack() : : : ");
        guildManager.GuildsList("", (guildsList) => {
            IList guildDictionaryList = new List<IDictionary>();
            for (int i = 0; i<guildsList.Count; i++)
                guildDictionaryList.Add(guildsList [i].ToDictionary());
            if (guildDictionaryList != null)
            {
                if (GameManager.PRINT_LOGS)
                    Debug.Log("MiniJSON.Json.Serialize(guildsDictionary) ::: " + MiniJSON.Json.Serialize(guildDictionaryList));
                PopulateJoinTab(MiniJSON.Json.Serialize(guildDictionaryList));
            }
        });
    }
	
    public void PopulateJoinTab(string jsonString)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("PopulateJoinTab() : : : " + jsonString);
        /*
		Value val1 = new Value(jsonString, MovieID);
		Value[] args={val1};
		guildUI.Invoke("populateJoinTab",args);
		*/
    }
	
	
    public static class TempJoinGuild
    {
        public static string joinGuildID;
        public static bool omitSoulCheck = false;
		
        public static void SetParams(string aJoinGuildID)
        {
            joinGuildID = aJoinGuildID;
        }
		
        public static void Clear()
        {
            joinGuildID = null;
            omitSoulCheck = false;
        }
    }
	
    public void JoinGuild(string guildIDStr)
    {
        int guildID = Int32.Parse(guildIDStr);
        if (GameManager.PRINT_LOGS)
            Debug.Log("JoinGuild(string guildID) : : : " + guildID);
				
        guildManager.GetGuildForGuildID(guildID, (aGuild) => {
            int gemsNeeded = 0;
            if (canJoinGuild(aGuild, false, out gemsNeeded))
            {
                if (gemsNeeded > 0 && !TempJoinGuild.omitSoulCheck)
                {
                    TempJoinGuild.SetParams(guildIDStr);
                    //Show popup for if a user wants to spend gems
                    PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.InsufficientSoulsForJoiningGuild;
                    GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup2("Insufficient Souls", gemsNeeded, "gems needed to join guild!", "Yes", "No"); //rocket
                } else
                {
                    guildManager.JoinGuild(guildID, (success, errorMessage) => {
                        TempJoinGuild.Clear();
                        if (success)
                        {
                            canJoinGuild(aGuild, true, out gemsNeeded);
                            SetGuild();
                        } else
                            GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Error", errorMessage);
				
                    });
                }
            } else
            {
                PurchaseManager.Instance.ShowGeneralPopup2(PurchaseManager.GeneralPopupType.InsufficientGems, gemsNeeded, "Join guild", "Buy Gems");
            }
        });
    }
	
    public void JoinGuildFromTemp()
    {
        JoinGuild(TempJoinGuild.joinGuildID);
    }
		
	
    private bool canJoinGuild(Guild aGuild, bool performPurchase, out int requiredGems)
    {
        bool canBuy = false;
        long soulsNeeded = 0;
        int gemsNeeded = 0;
		
        User currentUser = GameManager._gameState.User;
        if (currentUser._inventory.souls >= aGuild.joinCost)
        {
            soulsNeeded = aGuild.joinCost;
            canBuy = true;
        } else if (currentUser._inventory.souls < aGuild.joinCost && PurchaseManager.getGemsForSouls((int)(aGuild.joinCost - currentUser._inventory.souls)) <= currentUser._inventory.gems)
        {
            soulsNeeded = (long)currentUser._inventory.souls;
            gemsNeeded = PurchaseManager.getGemsForSouls((int)(aGuild.joinCost - currentUser._inventory.souls));
            canBuy = true;
        }
		
        if (canBuy && performPurchase)
        {
            currentUser._inventory.souls -= soulsNeeded;
            currentUser._inventory.gems -= gemsNeeded;
            GameManager.instance.SaveGameState(true);
        }
        requiredGems = gemsNeeded;
        return canBuy;
    }
			
#region Editable Name textfield on 'Create Guild UI' 
	
    public void onNameTextFocus()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onNameTextFocus");
        GameManager.instance.keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        GameManager.instance.keyBoardInputListener = onGetNameText;
    }
	
    public void SetNameText(string text)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetNameText string text" + text);
        /*
		Value val = new Value(text, MovieID);
		Value[] args = {val};
		guildUI.Invoke("setNameText", args);
		*/
    }
	
    public void GetNameText()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("GetNameText");
        /*
		Value[] args = {};
		guildUI.Invoke("getNameText", args);
		*/
    }
	
    public void onGetNameText(string nameText)
    {
        nameText = Nonce.GetUniqueID();
        if (GameManager.PRINT_LOGS)
            Debug.Log("onGetNameText string nameText" + nameText);
        if (!nameText.Equals(""))
            SetNameText(nameText);
    }
	
#endregion

#region Editable Description textfield on 'Create Guild UI'	
	
    public void onDescriptionTextFocus()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onDescriptionTextFocus");
        GameManager.instance.keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        GameManager.instance.keyBoardInputListener = onGetDescriptionText;
    }
	
    public void SetDescriptionText(string text)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetDescriptionText string text" + text);
        /*
		Value val = new Value(text, MovieID);
		Value[] args = {val};
		guildUI.Invoke("setDescriptionText", args);
		*/
    }
	
    public void GetDescriptionText()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("GetDescriptionText");
        /*
		Value[] args = {};
		guildUI.Invoke("getDescriptionText", args);
		*/
    }
	
    public void onGetDescriptionText(string descriptionText)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onGetDescriptionText string nameText" + descriptionText);
        if (!descriptionText.Equals(""))
            SetDescriptionText(descriptionText);
    }
	
#endregion	
	
    public static class TempGuildParams
    {
        public static string name;
        public static string description;
        public static string inviteOnly;
        public static string pointsToJoin;
        public static string logoID;
		
        public static void SetParams(string aName, string aDescription, string aInviteOnly, string aPointsToJoin, string aLogoID)
        {
            name = aName;
            description = aDescription;
            inviteOnly = aInviteOnly;
            pointsToJoin = aPointsToJoin;
            logoID = aLogoID;
        }
		
        public static void Clear()
        {
            name = "";
            description = "";
            inviteOnly = "";
            pointsToJoin = "";
            logoID = "";
            omitSoulCheck = false;
        }
		
        public static bool omitSoulCheck = false;
    }
	
    public void GuildCreate(string name, string description, string inviteOnly, string pointsToJoin, string logoID) //CB when guild create button is pressed
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("GuildCreate() : : : name " + name + "description" + description + "inviteOnly" + inviteOnly + "pointsToJoin" + pointsToJoin + "logoID" + logoID);
        //name = "tempGuild1";
        bool isInviteOnly = false;
        if (inviteOnly.Equals("invite"))
            isInviteOnly = true;
        int pToJoin = Int32.Parse(pointsToJoin);
        int logoIntID = Int32.Parse(logoID);
		
        int shortOfGems = 0;
        if (PurchaseManager.Instance.CheckGuildPurchase(name, DatabankSystem.Databank.GUILD_CREATE_COST, false, out shortOfGems))
        {
            if (shortOfGems > 0 && !TempGuildParams.omitSoulCheck)
            {
                TempGuildParams.SetParams(name, description, inviteOnly, pointsToJoin, logoID);
                PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.InsufficientSoulsForGuild;
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup2("Insufficient Souls", shortOfGems, "gems needed to create guild!", "Yes", "No"); //rocket
            } else
            {
                guildManager.CreateGuild(name, logoIntID, isInviteOnly, description, pToJoin, 50, "MOTD", (success, errorMessage) => {
                    TempGuildParams.Clear();
                    if (success)
                    {
                        PurchaseManager.Instance.CheckGuildPurchase(name, DatabankSystem.Databank.GUILD_CREATE_COST, true, out shortOfGems);
                        SetGuild();
                        GameManager.instance.scaleformCamera.generalSwf.Init();
                    } else
                    {
                        GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Error", errorMessage);
                    }
                });
            }
        } else
            PurchaseManager.Instance.ShowGeneralPopup2(PurchaseManager.GeneralPopupType.InsufficientGems, shortOfGems, "Create guild", "Buy Gems");
    }
	
    public void GuildCreateFromTemp()
    {
        TempGuildParams.omitSoulCheck = true;
        GuildCreate(TempGuildParams.name, TempGuildParams.description, TempGuildParams.inviteOnly, TempGuildParams.pointsToJoin, TempGuildParams.logoID);
    }
#region Editable Search textfield on 'Create Guild UI' 
	
    public void OnGuildSearchTextFocus()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("OnGuildSearchTextFocus");
        GameManager.instance.keyBoard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        GameManager.instance.keyBoardInputListener = onGetSearchText;
    }
	
    private void onGetSearchText(string searchText)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("onGetSearchText string searchText" + searchText);
        if (!searchText.Equals(""))
            SetSearchText(searchText);
    }
	
    public void SetSearchText(string searchText)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetSearchText string searchText" + searchText);
        /*
		Value val = new Value(searchText, MovieID);
		Value[] args = {val};
		guildUI.Invoke("setSearchText", args);
		*/
    }

#endregion
	
    public void searchGuild(string searchText) // CB on search button tap
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("searchGuild() : : : " + searchText);
		if(searchText.Equals("")) {
			Debug.LogError("guild name searched is empty");
			return;
		}
        guildManager.GuildsList(searchText, (guildsList) => {
            if (guildsList == null)
            {
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Error", "No guild matching your search criteria found!");
                return;
            }
            IList guildDictionaryList = new List<IDictionary>();
            for (int i = 0; i<guildsList.Count; i++)
                guildDictionaryList.Add(guildsList [i].ToDictionary());
            if (guildDictionaryList != null)
            {
                if (GameManager.PRINT_LOGS)
                    Debug.Log("MiniJSON.Json.Serialize(guildsDictionary) ::: " + MiniJSON.Json.Serialize(guildDictionaryList));
                PopulateSearchTab(MiniJSON.Json.Serialize(guildDictionaryList));
            }
        });
    }
	
    public void PopulateSearchTab(string jsonString)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("PopulateSearchTab() : : : " + jsonString);
        /*
		Value val1 = new Value(jsonString, MovieID);
		Value[] args={val1};
		guildUI.Invoke("populateSearchTab",args);
		*/
    }
	
    public void onLeaveGuild()
    {
        HideUI();
        guildManager.LeaveGuild((success, errorMessage) => {
            if (success)
            {
                isChatLoggedIn = false;
                GameManager._gameState.User.guild = null;
                SetGuild();
            } else
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Failure", errorMessage);
        });
    }
	
    public void HideUI()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("HideUI called!");
        /*
		Value[] args = {};
		guildUI.Invoke("hideUi", args);
		*/
    }
	
    public void ShowUserAccessPanel(bool yesNo)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("ShowUserAccessPanel - yesNO => " + yesNo);
        /*
		Value val1 = new Value(yesNo, MovieID);
		Value[] args={val1};
		guildUI.Invoke("UserAccess",args);
		*/
    }
	
	
    public void CreateGuild() //Callback for "create guild" tab
    {
        /*
		Value val1 = new Value(DatabankSystem.Databank.GUILD_CREATE_COST, MovieID);
		Value[] args={val1};
		guildUI.Invoke("setCreateButtonAmount",args);
		*/
    }
	
    public void ClearChat()
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("ClearChat ------ >>>>>>>>>>> ClearChat");
        /*
		Value[] args = {};
		guildUI.Invoke("ClearChat", args);
		*/
    }
	
    public void PromoteMember(string memberIDStr)
    {
        int memberID = Int32.Parse(memberIDStr);
        guildManager.PromoteMember(memberID, (success, errorMessage) => {
            if (success)
            {
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Success!", "Member promoted!");
                HideUI();
            } else
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Failure", errorMessage);
        });
    }
	
    public void DemoteMember(string memberIDStr)
    {
        int memberID = Int32.Parse(memberIDStr);
        guildManager.DemoteMember(memberID, (success, errorMessage) => {
            if (success)
            {
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Success!", "Member demoted!");
                HideUI();
            } else
                GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup("Failure", errorMessage);
        });
    }
	
    private void SetLanguage(string languageString, string charSetID)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("RAID UI --- private void SetLanguage(string languageString, string charSetID) :::::::::::::::::::::::::::::::::::::::::::::::: " + languageString);
        /*
		Value val=new Value(languageString,MovieID);
		Value val2=new Value(charSetID,MovieID);
		
		Value[] args1={val, val2};
		
		guildUI.Invoke("setLanguage",args1);
		*/
    }
	
    /*public void missingString(string missingThings)
	{
		if(Debug.isDebugBuild)
		{
			GameManager.instance.scaleformCamera.generalSwf.words +=missingThings+"\n";
			if(GameManager.PRINT_LOGS) Debug.Log("Word Recieved :::: "+missingThings+" Current Words :::"+GameManager.instance.scaleformCamera.generalSwf.words);
		}
	}*/
	
    public void SetCost()
    {
        /*
		Value val1 = new Value(DatabankSystem.Databank.GUILD_CREATE_COST, MovieID);
		Value[] args={val1};
		guildUI.Invoke("setCost",args);
		*/
    }

	#endregion


    #region ChatCode

    public void PopulateChatNode(bool isClan, ChatMessage chatMessage)
    {
        // TODO: Add additional parameters after update on the server side.
        PopulateChatNode(isClan, chatMessage.senderName.ToString(), chatMessage.functionData, chatMessage.message, chatMessage.createdTime, chatMessage.senderId);
    }
    
    public void PopulateChatNode(bool isClan, string username, Game.FunctionData data, string message, long time, int _senderID)
    {
        chatrow chatNode = (Instantiate(chatListNode) as GameObject).GetComponent<chatrow>();
//        chatNode.userNameButton.onClick.RemoveAllListeners();
//        chatNode.userNameButton.onClick.AddListener(() => {
//            //                Debug.Log ("PopulateChatNode-UsedId: " + _senderID + "   Username: " + username);
//            ShowBase(chatNode.transform, _senderID, username);
//        });
        chatNode.message.text = message;
        chatNode.userName.text = username;
		chatNode.TimeStamp = time;
		DateTime dateTime = GameUtils.FromUnixTime(time);
		chatNode.PushTime = dateTime;
		chatNode.memberType.text = isClan ? data.rank.ToString() : data.teamName;
        chatNode.transform.SetParent(isClan ? clanChatScrollView.transform : globalChatScrollView.transform);
        chatNode.transform.SetAsFirstSibling();
        chatNode.transform.name = (long.MaxValue - dateTime.Ticks).ToString();


        chatNode.transform.localPosition = new Vector3(0, chatNode.transform.localPosition.y, 0);
        chatNode.transform.localScale = Vector3.one;


//        chatNode._replay.SetActive(false);
    
        if (isClan)
        {
//            clanChatScrollView.GetComponent<ScrollContent>().RefreshContent();
            ClanChatList.Add(chatNode);
            clanChatScrollView.GetComponent<UIGrid>().Reposition();
            //                this.PerformActionWithDelay (0.2f, () => {
            //                    clanChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
            //                });
            //                Debug.Log ("NormalizedPosition-Clan: " + clanChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition);
        } else
        {
//            globalChatScrollView.GetComponent<ScrollContent>().RefreshContent();
            GlobalChatList.Add(chatNode);
            globalChatScrollView.GetComponent<UIGrid>().Reposition();
            //                this.PerformActionWithDelay (0.2f, () => {
            //                    globalChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
            //                });
            //                Debug.Log ("NormalizedPosition-Global: " + globalChatScrollView.transform.parent.GetComponent<ScrollRect> ().verticalNormalizedPosition);
        }

    }
    

	bool canSendMessage = true;
    public void SendClanMessage(UIInput message)
    {
		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to send messages", () => {});
			return;
		}
		if(!canSendMessage)
			return;
        if (message.text.Trim() == "")
            return;
		if(message.text.Contains("Type your message", true))
			return;

		canSendMessage = false;
		guildManager.SendTeamChat(message.text, (msg) => {
			message.text = "";
			Invoke("CanSendMessage", 1.0f);
            //PopulateChatNode(true, GameManager._gameState.User.name, "Elder", message.text, DateTime.Now, Int32.Parse(GameManager.instance.fragNetworkingNew.GetUserID()));
        }, () => {
            Debug.LogError("SendTeamChat Failed!");
			canSendMessage = true;
		});
    
    }
            
    public void SendGlobalMessage(UIInput message)
    {
		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to send messages", () => {});
			return;
		}
		if(!canSendMessage)
			return;
        if (message.text.Trim() == "")
            return;
		if(message.text.Contains("Type your message", true))
			return;

		canSendMessage = false;
		//            Debug.LogError ("SendGlobalMessage: " + message.text);
        guildManager.SendGlobalChatMessage(message.text, (msg) => {
            message.text = "";
			Invoke("CanSendMessage", 1.0f);
			//PopulateChatNode(false, GameManager._gameState.User.name, "Elder", message.text, DateTime.Now, Int32.Parse(GameManager.instance.fragNetworkingNew.GetUserID()));
        }, () => {
            Debug.LogError("SendGlobalMessage Failed!");
			canSendMessage = true;
		});
    }
    // To avoid double messages being sent
	private void CanSendMessage() {
		canSendMessage = true;
	}

    protected void PopulateChat()
    {
		ClearChat(true);
        guildManager.GetTeamChats(GameManager._gameState.User.id, (response) => {
			Debug.LogError("PopulateChat guild<<<<<<<< - "+response);
            TeamChat teamChat = JsonConvert.DeserializeObject<TeamChat>(response);
            if (teamChat.success)
            {
                foreach (var chatMessage in teamChat.chats)
                {
                    PopulateChatNode(true, chatMessage);
                }
            } else
                Debug.LogError("Unable to get TeamChat: " + response);
        }, () => {});
    
	}
	private bool isGlobalChatFetched = false;
	protected void PopulateGlobalChat() {
		if(!isGlobalChatFetched) {
			Debug.LogError("Populate GLobal Chat - -");
			guildManager.GetGlobalChats((response) => {
				Debug.LogError("PopulateChat Global <<<<<<<< - "+response);
				TeamChat teamChat = JsonConvert.DeserializeObject<TeamChat>(response);
				if (teamChat.success)
				{
//					isGlobalChatFetched = true;
					foreach (var chatMessage in teamChat.chats)
					{
						PopulateChatNode(false, chatMessage);
					}
				} else
					Debug.LogError("Unable to get Global Chats: " + response);
			}, () => {});
		}
	}

	private void ClearChat(bool isGuild) {
		Transform parentScrollview = isGuild ? clanChatScrollView.transform : globalChatScrollView.transform;
		foreach(Transform t in parentScrollview) {
			Destroy(t.gameObject);
		}
	}
		
		#endregion
}