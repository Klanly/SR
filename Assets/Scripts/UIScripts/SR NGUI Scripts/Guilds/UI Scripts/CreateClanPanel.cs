using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Game.UI
{
    public class CreateClanPanel : MonoBehaviour
    {
        public UIInput clanNameCreateClan;
        public UIInput clanDescriptionCreateClan;

		public UICenterOnChild teamFlagGrid;
		public UICenterOnChild trophiesGrid;

		private bool isWaiting = false;
        public void CreateClan()
        {
			Debug.LogError(isWaiting + " "+DatabankSystem.Databank.GUILD_CREATE_COST);
//			if(!isWaiting)
//				return;


			int shortOfGems = 0;
			if (PurchaseManager.Instance.CheckGuildPurchase(name, DatabankSystem.Databank.GUILD_CREATE_COST, false, out shortOfGems))
			{
				if (shortOfGems > 0)
				{
					PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.InsufficientSoulsForGuild;
					GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup2("Insufficient Souls", shortOfGems, "gems needed to create guild!", "Yes", "No"); //rocket
				} else
				{
					if (clanNameCreateClan.value == "")
					{
						Debug.LogError("clanNameCreateClan is null.");
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error", "Guild name is empty", () => {});
						
						return;
					}
					
					if (clanDescriptionCreateClan.value == "")
					{
						Debug.LogError("clanDescriptionCreateClan is null.");
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error", "Guild desciption is empty", () => {});
						return;
					}

					isWaiting = true;
					int requiredTrophies = int.Parse(trophiesGrid.mCenteredObject.name);
					string[] teamFlagName = teamFlagGrid.mCenteredObject.GetComponent<UISprite>().spriteName.Split(' ');
					int teamFlagID = int.Parse(teamFlagName[1]);
					
					// TODO: Using RAW Data for other parameter. Update it after adding proper UI and input Fields.
					ServerManager.Instance.CreateTeam(clanNameCreateClan.value, clanDescriptionCreateClan.value, teamFlagID, 1, requiredTrophies, 2, "Pakistan", (response) => {
						Debug.Log("Create Clan: " + response); 
						ClanData clanData = JsonConvert.DeserializeObject<ClanData>(response);
						
						if (!clanData.nameAvailable)
						{
//							Debug.LogError("Unable to Create a Team: " + response);
							UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error", "Guild name unavailable", () => {});
							return;
						}
						isWaiting = false;
						GameManager._gameState.User.team = clanData.team;
						ClanUIHandler.instance.ClanDetailPanel.PopulateMyClan();
						ClanUIHandler.instance.RefreshClanInfo();
						ClanUIHandler.instance._Listener.SetGuild();
						
					}, 
					() => {
						isWaiting = false;
						UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("Error", "Unable to create guild. Try again with a different guild name", () => {});
					});

				}
			} else {
				PurchaseManager.Instance.ShowGeneralPopup2(PurchaseManager.GeneralPopupType.InsufficientGems, shortOfGems, "Create guild", "Buy Gems");
			}
        }
    }
}
