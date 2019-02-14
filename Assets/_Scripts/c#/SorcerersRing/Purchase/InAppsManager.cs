using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Global;
using SA.IOSNative.StoreKit;
namespace InAppsManager
{
	public class InAppsManager : Singleton<InAppsManager>
	{
		public List<string> IOSProductIds;


		// Use this for initialization
		void Start ()
		{
			#if UNITY_ANDROID
			//listening for Purchase and consume events
			AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
			AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
			
			//listening for store initialising finish
			AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
			
			//you may use loadStore function without parameter if you have filled base64EncodedPublicKey in plugin settings
			AndroidInAppPurchaseManager.Client.Connect ();
			#endif
			#if UNITY_IOS
				
				//You do not have to add products by code if you already did it in seetings guid
				//Windows -> IOS Native -> Edit Settings
				//Billing tab.
				// TODO: Add SKU Keys
				// Add SKU Keys in the Inspector.
//				Debug.LogError ("Adding IOSProductIds: " + IOSProductIds.Count);
/*				foreach (var productId in IOSProductIds)
					IOSInAppPurchaseManager.instance.AddProductId (productId);
*/				

			//Event Use Examples
//			IOSInAppPurchaseManager.OnVerificationComplete += HandleOnVerificationComplete;
//			IOSInAppPurchaseManager.OnStoreKitInitComplete += OnStoreKitInitComplete;
//				
//			IOSInAppPurchaseManager.OnTransactionComplete += OnTransactionComplete;
//			IOSInAppPurchaseManager.OnRestoreComplete += OnRestoreComplete;
//				
//
//			IOSInAppPurchaseManager.Instance.LoadStore ();
			#endif
		}
	
		public void Init() 
		{
			#if UNITY_IOS
			#endif
			IDictionary dictionary = PurchaseManager.Instance.MarketProducts;
			IList currencyPackList = dictionary["inapps"] as IList;
			string productId = "";
			IDictionary aCurrencyPackDic = null;
			int count = currencyPackList.Count;

			for(int i = 0; i < count; i++)
			{
				aCurrencyPackDic = currencyPackList[i] as IDictionary;
				productId = aCurrencyPackDic["productIdentifierApple"].ToString();

				PaymentManager.Instance.AddProductId (productId);

			}
//			PaymentManager.OnVerificationComplete += HandleOnVerificationComplete;
			PaymentManager.OnStoreKitInitComplete += OnStoreKitInitComplete;
							
			PaymentManager.OnTransactionComplete += OnTransactionComplete;
//			PaymentManager.OnRestoreComplete += OnRestoreComplete;
							
			
			PaymentManager.Instance.LoadStore ();
			Debug.LogError("LoadStore called - "+PaymentManager.Instance.Products.Count);
		}

		private static void OnBillingConnected (BillingResult result)
		{
			AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
			
			if (result.IsSuccess) {
				Debug.Log ("Connected");
			} else {
				Debug.Log ("Connection failed");
			}
		}

		private static void OnProductPurchased (BillingResult result)
		{
			if (result.IsSuccess) {
				AndroidMessage.Create ("Product Purchased", result.Purchase.SKU + "\n Full Response: " + result.Purchase.OriginalJson);
				OnProcessingPurchasedProduct (result.Purchase);
			} else {
				AndroidMessage.Create ("Product Purchase Failed", result.Response.ToString () + " " + result.Message);
			}
			
			Debug.Log ("Purchased Responce: " + result.Response.ToString () + " " + result.Message);
		}

		private static void OnProcessingPurchasedProduct (GooglePurchaseTemplate purchase)
		{
			
			switch (purchase.SKU) {
			case "":
//				AndroidInAppPurchaseManager.Client.Consume(CONIS_PACK);
				break;
//			case BONUS_TRACK:
//				GameData.UnlockBonusTrack();
//				break;
			}
		}
		
		private static void OnProductConsumed (BillingResult result)
		{


		}

		private static void OnProcessingConsumeProduct (GooglePurchaseTemplate purchase)
		{
			switch (purchase.SKU) {
			case "":
//				GameData.AddCoins (100);
				break;
			}
		}

		//--------------------------------------
		//  PUBLIC METHODS
		//--------------------------------------

		public List<MarketProduct> GetProductsList ()
		{
			List<MarketProduct> marketProducts = new List<MarketProduct> ();
			var products = PaymentManager.Instance.Products;
			foreach (var prod in products) {
				marketProducts.Add (new MarketProduct (prod.Id, prod.DisplayName, prod.Price, prod.LocalizedPrice));
			}
			return marketProducts;
		}

		public static void buyItem (string productId)
		{
			Debug.LogError ("buyItem " + productId);
			#if UNITY_ANDROID
			AndroidInAppPurchaseManager.Instance.Purchase (productId);
			#endif
			#if UNITY_IOS
			PaymentManager.Instance.BuyProduct (productId);
			#endif
		}
		
		//--------------------------------------
		//  GET/SET
		//--------------------------------------
		public static bool IsStoreLoaded {
			get {
				return PaymentManager.Instance.IsStoreLoaded;
			}
		}

		//--------------------------------------
		//  EVENTS
		//--------------------------------------
		
		
		private static void UnlockProducts (string productIdentifier)
		{
			IDictionary dictionary = PurchaseManager.Instance.MarketProducts;
			IList currencyPackList = dictionary["inapps"] as IList;
			
			string productId = "";
			IDictionary aCurrencyPackDic = null;
			int count = currencyPackList.Count;
			NGInAppNode inAppNode = null;
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
				if(productId.Equals(productIdentifier)) {
					GameManager._gameState.User._inventory.gems += System.Convert.ToInt32(aCurrencyPackDic["rewardQuantity"]);
					UIManager.instance.generalSwf.Init ();
					break;
				}
			}
			Debug.LogError (productIdentifier + " Unlocked!!!");
		}
		
		private static void OnTransactionComplete (PurchaseResult result)
		{
			
			Debug.Log ("OnTransactionComplete: " + result.ProductIdentifier);
			Debug.Log ("OnTransactionComplete: state: " + result.State);

			IDictionary dictionary = PurchaseManager.Instance.MarketProducts;
			IList currencyPackList = dictionary["inapps"] as IList;
			string productId = "";
			IDictionary aCurrencyPackDic = null;
			int count = currencyPackList.Count;
			
			ArrayList arr = new ArrayList(currencyPackList);
			string itemId = "";
			for(int i = 0; i < count; i++)
			{
				aCurrencyPackDic = currencyPackList[i] as IDictionary;
				productId = aCurrencyPackDic["productIdentifierApple"].ToString();
				
				if(productId.Equals(result.ProductIdentifier)) {
					itemId = aCurrencyPackDic["id"].ToString();
					break;
				}
			}

			switch (result.State) {
	
			case PurchaseState.Purchased:
#if  UNITY_EDITOR_OSX || UNITY_EDITOR
				Debug.LogError ("UNITY_EDITOR_OSX || UNITY_EDITOR");
//				Debug.LogError("receipt - editor "+result.Receipt);
				PurchaseManager.Instance.VerifyInAppPurchase(Nonce.GetUniqueID(), itemId, true, "debug");
//				UnlockProducts (result.ProductIdentifier);

#elif UNITY_IPHONE || UNITY_IOS
				Debug.LogError ("UNITY_IOS || UNITY_IPHONE");
				PurchaseManager.Instance.VerifyInAppPurchase(Nonce.GetUniqueID(), itemId, true, result.Receipt);
#endif			
				break;
			case PurchaseState.Restored:
				//Our product been succsesly purchased or restored
				//So we need to provide content to our user depends on productIdentifier
				UnlockProducts (result.ProductIdentifier);
				break;
			case PurchaseState.Deferred:
				//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
				//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
				//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
				break;
			case PurchaseState.Failed:
				//Our purchase flow is failed.
				//We can unlock intrefase and repor user that the purchase is failed. 
				Debug.Log ("Transaction failed with error, code: " + result.Error.Code);
				Debug.Log ("Transaction failed with error, description: " + result.Error.Message);
				break;
			}
			
//			if (result.State == InAppPurchaseState.Failed) {
//				IOSNativePopUpManager.showMessage ("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Message);
//			} else {
//				IOSNativePopUpManager.showMessage ("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString ());
//			}
		}
		
		
//		private static void OnRestoreComplete (IOSStoreKitRestoreResult res)
//		{
//			if (res.IsSucceeded) {
//				Debug.LogError ("OnRestoreComplete");
//				//IOSNativePopUpManager.showMessage ("Success", "Restore Compleated");
//			} else {
////				IOSNativePopUpManager.sh
////				IOSNativePopUpManager.showMessage ("Error: " + res.Error.Code, res.Error.Message);
//			}
//		}	
		
		
//		static void HandleOnVerificationComplete (IOSStoreKitVerificationResponse response)
//		{
////			IOSNativePopUpManager.showMessage ("Verification", "Transaction verification status: " + response.status.ToString ());
//			if (response.status == 0) {
//				Debug.Log ("Transaction is valid");
//				UnlockProducts (response.productIdentifier);
//			}
//
//			Debug.Log ("ORIGINAL JSON: " + response.originalJSON);
//		}
		
		
		private static void OnStoreKitInitComplete (SA.Common.Models.Result result)
		{


			if (result.IsSucceeded) {
				
				int avaliableProductsCount = 0;
				foreach (Product tpl in  SA.IOSNative.StoreKit.PaymentManager.Instance.Products) {
					if (tpl.IsAvailable) {
						avaliableProductsCount++;
					}
				}
				if(Debug.isDebugBuild)
					IOSNativePopUpManager.showMessage ("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount);
				Debug.LogError ("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);
			} else {
//				IOSNativePopUpManager.showMessage ("StoreKit Init Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Message);
			}
		}

	}
}