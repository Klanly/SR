using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GuildSystem;

public class GuildJoinCreateSearchUI : MonoBehaviour {

	public const string PREFAB_PATH = "UIPrefabs/NGUI/GuildJoinCreateSearch";

	public GameObject _joinTabPanel;
	public GameObject _createTabPanel;
	public GameObject _searchTabPanel;

	#region CreateGuild specific properties
	public int _guildMaxMembers = 20;
	public UIInput _guildNameInput;
	public UIInput _guildDescriptionInput;
	public int _guildTeamType = 1; //1 = open for all, 2 = invite only // only option "1" for now...
	public int _guildCreateCost = 10000; //this is a constant value, hardcoded in UI. Ideally, UI should show the value input here, but meh! 
	public UIInput _guildJoinCost;
	public int _guildWarFrequency = 1; //WTF is this bulsheetttt??? 
	public string _guildLocation = "Mango"; //WTF is this bulsheetttt??? 
	public int _guildFlag = 4; //Constant at the moment, don't forget to change it later!
	public string _guildMOTD = "I thought this was removed !!!";

	#endregion

	public UI_Guild _listener;

	private bool isCreatePressed;

	protected void Start()
	{
		OnJoinTabClick ();
	}

	protected void PopulateJoinTab(List<Guild> guildsList)
	{
		
	}


	#region NGUI button cb

	public void OnJoinTabClick()
	{
		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to join guild", () => {});
			return;
		}

		_joinTabPanel.SetActive (true);
		_createTabPanel.SetActive (false);
		_searchTabPanel.SetActive (false);

		GuildSystem.GuildsManager.Instance.GuildsList("", (guildsList) => {
			Debug.Log("guildslist count >> " + guildsList.Count);
			PopulateJoinTab(guildsList);
		});
	}


	public void OnCreateButtonClick()
	{
		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to create guild", () => {});
			return;
		}

		if(isCreatePressed)
			return;

		string guildName = _guildNameInput.label.text;
		string guildDescription = _guildDescriptionInput.label.text;
		int teamFlag = _guildFlag;
		int teamType = _guildTeamType;
		int requiredTrophies = _guildCreateCost;
		int warFrequency = _guildWarFrequency;
		string teamLocation = _guildLocation;
		int memberLimit = _guildMaxMembers;
		string messageOfTheDay = _guildMOTD;
		string joinCost = _guildJoinCost.label.text;

		isCreatePressed = true;

		_listener.GuildCreate (guildName, guildDescription, teamType.ToString(), joinCost, teamFlag.ToString());
	}


	public void OnCreateTabClick()
	{
		isCreatePressed = false;
		_joinTabPanel.SetActive (false);
		_createTabPanel.SetActive (true);
		_searchTabPanel.SetActive (false);
	}


	public void OnSearchTabClick()
	{
		_joinTabPanel.SetActive (false);
		_createTabPanel.SetActive (false);
		_searchTabPanel.SetActive (true);
	}

	#endregion
}
