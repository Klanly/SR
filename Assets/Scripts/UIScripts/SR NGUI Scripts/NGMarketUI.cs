using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using InAppsManager;

public class NGMarketUI : MonoBehaviour
{
	public MarketUIInterface _marketUIInterface;
	[SerializeField]
	private UIGrid scrollview;

	public GameObject inAppPrefab;

	private List<NGInAppNode> inAppNodes;

	public void setInterface (MarketUIInterface marketInterface)
	{
		_marketUIInterface = marketInterface;
	}

	void Start ()
	{
		if (_marketUIInterface != null)
			_marketUIInterface.OnRegisterSWFChildCallback (this);

		NGUITools.BringForward (gameObject);
		UIManager.instance.generalSwf.SetOnTop ();

		inAppNodes = new List<NGInAppNode>();	

		IDictionary dictionary = PurchaseManager.Instance.MarketProducts;
		IList currencyPackList = dictionary["inapps"] as IList;
		
		string productId = "";
		IDictionary aCurrencyPackDic = null;
		int count = currencyPackList.Count;
		NGInAppNode inAppNode  = null;
		ArrayList arr = new ArrayList(currencyPackList);

		for(int i = 0; i < count; i++)
		{
			aCurrencyPackDic = currencyPackList[i] as IDictionary;
			#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
			productId = aCurrencyPackDic["productIdentifierApple"].ToString();
			#elif UNITY_ANDROID
			productId = aCurrencyPackDic["productIdentifierGoogle"].ToString();
			#elif UNITY_IPHONE
			productId = aCurrencyPackDic["productIdentifierApple"].ToString();
			#endif
			inAppNode = (Instantiate(inAppPrefab) as GameObject).GetComponent<NGInAppNode>();
//			string price = aCurrencyPackDic["price"].ToString();
			string price = aCurrencyPackDic["price"].ToString();
			InApp.InAppClass inAppClass = (InApp.InAppClass)System.Convert.ToInt32(aCurrencyPackDic["class"]);
//			if(price.Contains("$")) {
			if(inAppClass == InApp.InAppClass.PREMIUM) {
				inAppNode.SetInAppPremium(true);
				inAppNode.SetInAppCost(price);
			} else{
				inAppNode.SetInAppPremium(false);
				inAppNode.SetInAppCost(int.Parse(price));
			}
//			Debug.LogError(aCurrencyPackDic["displayName"].ToString()+" "+productId+" "+inAppClass.ToString()+" "+aCurrencyPackDic["imageURL"].ToString());
			inAppNode.SetInAppID(productId);
			inAppNode.SetInAppName(aCurrencyPackDic["displayName"].ToString());
			inAppNode.SetInAppReward(System.Convert.ToInt32(aCurrencyPackDic["rewardQuantity"]));
			inAppNode.SetInAppSprite(aCurrencyPackDic["imageURL"].ToString());
			inAppNode.SetOnClickCallback(OnItemClick);
			inAppNode.transform.SetParent(scrollview.transform);

			inAppNode.transform.localScale = Vector3.one;
			scrollview.Reposition();
		}
		if (GameManager.instance.scaleformCamera.levelScene != null)
			GameManager.instance.scaleformCamera.levelScene.SetDisplayVisible(false);
	}

	private void OnItemClick(NGInAppNode obj) {
		foreach(NGInAppNode node in inAppNodes) {
			node.SetClickable(false);
		}
		Invoke("UnlockButtons", 5.0f);
	}

	private void UnlockButtons() {
		foreach(NGInAppNode node in inAppNodes) {
			node.SetClickable(true);
		}
	}
}
