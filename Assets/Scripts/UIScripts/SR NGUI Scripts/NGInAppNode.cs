using UnityEngine;
using System;
using System.Collections;

public class NGInAppNode : MonoBehaviour {

	[SerializeField]
	private UILabel nameLabel;
	[SerializeField]
	private UILabel costLabel;
	[SerializeField]
	private UILabel rewardLabel;
	[SerializeField]
	private UISprite inAppSprite;
	[SerializeField]
	private UISprite typeSprite;

	private const string gemSprite = "image 29";
	private const string soulSprite = "image 25";

	private string inAppId;
	private bool isPremium;
	private int cost;
	private float reward;

	private bool canClick;
	private Action<NGInAppNode> OnClickCallback;


	// Use this for initialization
	void Start () {
		canClick = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInAppName(string name) {
		nameLabel.text = name;
	}

	public void SetInAppID(string id) {
		inAppId = id;
	}

	public void SetInAppCost(int cost) {
		this.cost = cost;
		costLabel.text = cost.ToString();
	}

	public void SetInAppCost(string cost) {
		costLabel.text = "$"+cost.ToString();
	}

	public void SetInAppReward(int reward) {
		this.reward = reward;
		rewardLabel.text = reward.ToString();
	}

	public void SetInAppSprite(string spriteName) {
		inAppSprite.spriteName = spriteName;
	}

	public void SetInAppPremium(bool isPremium) {
		this.isPremium = isPremium;
		typeSprite.spriteName = isPremium ? gemSprite : soulSprite;
	}

	public void SetOnClickCallback(Action<NGInAppNode> callback) {
		OnClickCallback = callback;
	}

	public void OnClick() {
		if(!GameManager.instance.fragNetworkingNew.isInternet) {
			PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
			UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup("No Internet Connectivity", "Restore internet connectivity to avail Market offers", () => {});
			return;
		}
		if(!canClick) {
			GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("", "Please Wait");
			return;
		}
		Debug.LogError("OnCLick - isPremium - "+isPremium+" id - "+inAppId);
		if(isPremium) {
			InAppsManager.InAppsManager.buyItem (inAppId);
		} else{
			BuyDust (cost, reward);
		}
		if(OnClickCallback != null) {
			OnClickCallback(this);
		}
	}

	void BuyDust (int price, float rewardAmount) {
		if (GameManager._gameState.User._inventory.gems >= price) {
			if (GameManager._gameState.User._inventory.souls == GameManager._gameState.User._inventory.bag.soulCapacity) {
				UIManager.instance.generalSwf.generalSwf.showUiGeneralPopup ("Error", "Upgrade Bag to add more souls", () => {});
				return;
			}
			GameManager._gameState.User._inventory.gems -= price;
			if(GameManager._gameState.User._inventory.souls + rewardAmount <= GameManager._gameState.User._inventory.bag.soulCapacity) {
				GameManager._gameState.User._inventory.souls += rewardAmount; 
			} else {
				GameManager._gameState.User._inventory.souls += GameManager._gameState.User._inventory.bag.soulCapacity;
			}

			GameManager.instance.SaveGameState (true);
			UIManager.instance.generalSwf.Init ();
		}
	}

	public void SetClickable(bool enable) {
		canClick = enable;
	}
}