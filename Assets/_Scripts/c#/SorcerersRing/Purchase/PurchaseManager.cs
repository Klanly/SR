using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;

using System.Linq;
using MiniJSON;

public class PurchaseManager {

	private static PurchaseManager _instance = null;

	//public SmartIAPAgent IAPAgent; // Neatplug stuff

	private List<Dictionary<string, object>> transactionList = null;
	private List<Dictionary<string, object>> receiptList = null;
	public const string FILE_NAME = "TransactionHistory.txt";
	public const string RECEIPTS_FILE_NAME = "PurchaseReceipt.txt";
	private const string DEFAULT_IN_APP_FILE_NAME = "DefaultInAppPurchase.txt";
	public const string IN_APP_FILE_NAME = "InAppPurchase.txt";
	
	private const string ID = "transactionID";
	private const string PARENT_ID = "parentID";
	private const string REQUEST_TYPE = "requestType";
	private const string GEMS = "gems";
	private const string SOULS = "souls";
	private const string CURRENT_GEMS = "currentGems";
	private const string CURRENT_SOULS = "currentSouls";
	private const string SKULL_LEVLE = "skullLevel";
	private const string ITEM_ID = "itemID";
	private const string TIME = "time";
	private const string COMMITTED = "committed";
	private const string SENDING_REQUEST = "sending";
	private const string SENDING_REQUEST_TIME = "sendingTime";
	private const string COMPLETED = "completed";
	private const string LOGGED_TO_SERVER = "loggedToServer";
	
	private const string RECEIPT_DATA = "receipt";
	private const string VERIFIED = "verified";
	private const string SIGNATURE = "signature";

	//MarketPlace _marketPlace = null;
	//MarketEventHandler _handler = null;
	private IDictionary _marketProducts = null;
	private IDictionary _products = new Dictionary<string, object>();
	string productsIds = "";
	private bool _isMarketInit = false;
	
	private PurchaseRequest _lastPurcahseRequest = null;
	
	public delegate void PurchaseResponse(PurchaseResponse response);
	
	public enum GeneralPopupType
	{
		InsufficientGems,
		BagUpgrade,
		KeyRingUpgrade,
		PotionBeltUpgrade,
		BagUpgradeOrSell,
		InsufficientSouls,
		InsufficientSoulsForGuild,
		InsufficientSoulsForJoiningGuild,
		None
	}
	
	public GeneralPopupType currentType = GeneralPopupType.None;
	
	private PurchaseManager()
	{
		//_marketPlace = new MarketPlace();
		LoadTransactions();
		LoadReceipts();
	}
	
	public static PurchaseManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new PurchaseManager();
			}
			return _instance;
		}
	}
	
	public PurchaseRequest LastPurcahseRequest
	{
		get
		{
			return _lastPurcahseRequest;
		}
		set
		{
			_lastPurcahseRequest = value;
		}
	}
	
	public IDictionary MarketProducts
	{
		get
		{
			lock(_products)
			{
				if(_products.Count > 0)
				{
					string productId = "";
					IList marketList = _marketProducts["inapps"] as IList;
//					IDictionary product = null;
//					int count = marketList.Count;
//					for(int i = 0; i < count; i++)
//					{
//						product = marketList[i] as IDictionary;
//						#if (UNITY_STANDALONE_WIN || UNITY_EDITOR || UNITY_STANDALONE_OSX)
//							productId = product["productIdentifierGoogle"].ToString();
//						#elif UNITY_ANDROID
//							productId = product["productIdentifierGoogle"].ToString();
//						#elif UNITY_IPHONE
//							productId = product["productIdentifierApple"].ToString();
//						#endif
//						if(!productId.Equals("") && _products.Contains(productId))
//						{
//							product["price"] = _products[productId];
//						}
//					}
				}
			}
			return _marketProducts;
		}
	}
	
	public List<Dictionary<string, object>> TransactionList
	{
		get
		{
			return transactionList;
		}
	}
	
	public void PerformPurchase(PurchaseRequest request)
	{
		GameManager gameManager = GameManager.instance;
		User user = GameManager._gameState.User;
		InventoryItem item = GetItem(request);

		string response = "success";
		if(request.IsAutoBuy)
		{
			int remainingSouls = (int)(item.dCost - user._inventory.souls);
			int gems = getGemsForSouls(remainingSouls);
			if(!PerformPurchase(gameManager, user, item, request, gems))
			{
				response = "gems_error";
			}
		}
		else
		{
			
			bool isPremium = (item.dCost == 0);
			bool isAddRing = true;
			
			
			if(isPremium)
			{
				if(user._inventory.gems < item.gCost)
				{
					isAddRing = false;
					response = "gems_error";
					string activity = "";
					switch(request.RequestType)
					{
						case PurchaseRequest.PurchaseRequestType.Ring:
							activity = "Buy Ring";
							break;
						case PurchaseRequest.PurchaseRequestType.Rune:
							activity = "Buy Rune";
							break;
					}
					ShowGeneralPopup2(GeneralPopupType.InsufficientGems, item.gCost - user._inventory.gems, activity, "Buy Gems");
				}
			}
			else
			{
				if(user._inventory.souls < item.dCost)
				{
					isAddRing = false;
					response = "soul_error";
					if(CheckBagCapcityForSouls(item.dCost))
					{
						ShowGeneralPopup2(GeneralPopupType.BagUpgrade, 0, "", "");
					}
					else
					{
						int remainingSouls = (int)(item.dCost - user._inventory.souls);
						int gems = getGemsForSouls(remainingSouls);
						_lastPurcahseRequest = request;
						string activity = "";
						switch(request.RequestType)
						{
							case PurchaseRequest.PurchaseRequestType.Ring:
								activity = "Buy Ring";
								break;
							case PurchaseRequest.PurchaseRequestType.Rune:
								activity = "Buy Rune";
								break;
						}
						ShowGeneralPopup2(GeneralPopupType.InsufficientSouls, gems, activity, "Buy");
					}
				}
			}
			if(isAddRing)
			{	
				bool isAdded = AddItem(item);
				if(isAdded)
				{
					if(isPremium)
					{
						user._inventory.gems -= item.gCost;
						SaveTransaction(request.RequestType, request.Uid, -item.gCost, 0, true, "", item.skullLevel, item.id);
					}
					else
					{
						user._inventory.souls -= item.dCost;
						SaveTransaction(request.RequestType, request.Uid, 0, -item.dCost, true, "", item.skullLevel, item.id);
					}
					gameManager.scaleformCamera.generalSwf.Init();
					gameManager.SaveGameState(false);
				}
				else
				{
					ShowGeneralPopup2(GeneralPopupType.BagUpgradeOrSell, 0, "", "");
					response = "slot_error";
				}
			}
		}
		switch(request.RequestType)
		{
			case PurchaseRequest.PurchaseRequestType.Ring:
				Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
				analParams.Add (Analytics.ParamName.RingName, item.ItemName());
				analParams.Add (Analytics.ParamName.RingCost, item.dCost.ToString());
				analParams.Add (Analytics.ParamName.SkullLevel, item.skullLevel.ToString());
				analParams.Add (Analytics.ParamName.IsPremium,(item.dCost == 0).ToString());
				Analytics.logEvent(Analytics.EventName.Ring_Buy, analParams);
				gameManager.scaleformCamera.generalSwf.OnBuyRingComplete(response);
			break;
				
			case PurchaseRequest.PurchaseRequestType.Rune:
				Dictionary<Analytics.ParamName, string> analParams2 = new Dictionary<Analytics.ParamName, string>();
				analParams2.Add (Analytics.ParamName.RuneName, item.ItemName());
				analParams2.Add (Analytics.ParamName.RuneCost, item.dCost.ToString());
				analParams2.Add (Analytics.ParamName.SkullLevel, item.skullLevel.ToString());
				analParams2.Add (Analytics.ParamName.IsPremium, (item.dCost == 0).ToString());
				Analytics.logEvent(Analytics.EventName.Rune_Buy, analParams2);
				gameManager.scaleformCamera.generalSwf.OnBuyRuneComplete(response);
			break;
		}
		if(request.IsAutoBuy)
		{
			_lastPurcahseRequest = null;
		}
	}
	
	private bool PerformPurchase(GameManager gameManager, User user, InventoryItem item, PurchaseRequest request, int gems)
	{
		bool isAdded = false;
		if(user._inventory.gems < gems)
		{
			string activity = "";
			switch(request.RequestType)
			{
				case PurchaseRequest.PurchaseRequestType.Ring:
					activity = "Buy Ring";
					break;
				case PurchaseRequest.PurchaseRequestType.Rune:
					activity = "Buy Rune";
					break;
			}
			ShowGeneralPopup2(GeneralPopupType.InsufficientGems, gems - user._inventory.gems, activity, "Buy Gems");
		}
		else
		{
			isAdded = AddItem(item);
			if(isAdded)
			{
				
				user._inventory.gems -= gems;
				user._inventory.souls = 0;
				SaveTransaction(request.RequestType, request.Uid, -gems, 0, true, "", item.skullLevel, item.id);
				
				gameManager.scaleformCamera.generalSwf.Init();
				gameManager.SaveGameState(false);
			}
			else
			{
				ShowGeneralPopup2(GeneralPopupType.BagUpgradeOrSell, 0, "", "");
				//response = "slot_error";
			}
		}
		return isAdded;
	}
	
	public void PerformSell(PurchaseRequest request)
	{
		GameManager gameManager = GameManager.instance;
		User user = GameManager._gameState.User;
		InventoryItem item = GetItem(request);
		user._inventory.souls += item.sellCost;
		SaveTransaction(request.RequestType, request.Uid, 0, item.sellCost, true, "", item.skullLevel, item.id);
//		bool isPremium = (item.dCost == 0);
//		// Selling a premium ring should give you souls = 1/SELL_FACTOR of gems. MAKE SURE TO GET IT SYNCED AT THE SCALEFORM END TOO!!!
//		if(isPremium)
//		{
//			int gCost = (int)Mathf.Floor(item.gCost/Inventory.SELL_FACTOR);
//			user._inventory.gems += gCost;
//			SaveTransaction(request.RequestType, request.Uid, gCost, 0, true, "", item.skullLevel, item.id);
//		}
//		else
//		{
//			int dCost = (int)Mathf.Floor(item.dCost/Inventory.SELL_FACTOR);
//			user._inventory.souls += dCost;
//			SaveTransaction(request.RequestType, request.Uid, 0, dCost, true, "", item.skullLevel, item.id);
//		}
		
		RemoveItem(item);
			
		gameManager.SaveGameState(false);

		
		switch(request.RequestType)
		{
			case PurchaseRequest.PurchaseRequestType.Ring:
				Dictionary<Analytics.ParamName, string> analParams = new Dictionary<Analytics.ParamName, string>();
				analParams.Add (Analytics.ParamName.RingName, item.ItemName());
				analParams.Add (Analytics.ParamName.RingCost, item.dCost.ToString());
				analParams.Add (Analytics.ParamName.SkullLevel, item.skullLevel.ToString());
				analParams.Add (Analytics.ParamName.IsPremium,(item.dCost == 0).ToString());
				Analytics.logEvent(Analytics.EventName.Ring_Sell, analParams);
				GameManager.instance.scaleformCamera.generalSwf.OnSellRingComplete();
			break;
				
			case PurchaseRequest.PurchaseRequestType.Rune:
				Dictionary<Analytics.ParamName, string> analParams2 = new Dictionary<Analytics.ParamName, string>();
				analParams2.Add (Analytics.ParamName.RuneName, item.ItemName());
				analParams2.Add (Analytics.ParamName.RuneCost, item.dCost.ToString());
				analParams2.Add (Analytics.ParamName.SkullLevel, item.skullLevel.ToString());
				analParams2.Add (Analytics.ParamName.IsPremium, (item.dCost == 0).ToString());
				Analytics.logEvent(Analytics.EventName.Rune_Sell, analParams2);
				GameManager.instance.scaleformCamera.generalSwf.OnSellRuneComplete();
			break;
		}	
		
		GameManager.instance.scaleformCamera.generalSwf.Init();
	}
	
	private InventoryItem GetItem(PurchaseRequest request)
	{
		InventoryItem item = null;
		switch(request.RequestType)
		{
			case PurchaseRequest.PurchaseRequestType.Ring:
				item = GetItemRing(request.Uid);
			break;
			case PurchaseRequest.PurchaseRequestType.Rune:
				item = GetItemRune(request.Uid);
			break;
			
		}
		return item;
	}
	
	private ItemRing GetItemRing(string uid)
	{
		ItemRing ring = null;
		GameManager gameManager = GameManager.instance;
		User user = GameManager._gameState.User;
		
		ring = gameManager.suggestedUIRingList.Find(x => (x.uid.Equals(uid)));
		if(ring == null)
		{
			ring = user._inventory.equippedRings.Find(x => (x.uid.Equals(uid)));
		}
		if(ring == null)
		{
			ring = user._inventory.bag.GetAllRings().Find(x => (x.uid.Equals(uid)));
		}
		
		return ring;
	}
	
	private ItemRune GetItemRune(string uid)
	{
		ItemRune rune = null;
		GameManager gameManager = GameManager.instance;
		User user = GameManager._gameState.User;
		
		rune = gameManager.suggestedUIRuneList.Find(x => (x.uid == uid));
		if(rune == null)
		{
			rune = user._inventory.staffRunes.Find(x => (x.uid == uid));
		}
		if(rune == null)
		{
			rune = user._inventory.bag.GetAllRunes().Find(x => (x.uid == uid));
		}
		
		return rune;
	}
	
	private bool RemoveItem(InventoryItem item)
	{
		bool isRemoved = false;
		User user = GameManager._gameState.User;
		
		switch(item.ItemType())
		{
			case InventoryItem.Type.kRing:
				ItemRing ring = (ItemRing)item;
				isRemoved = user._inventory.equippedRings.Remove(ring);
				if(!isRemoved)
				{
					isRemoved = user._inventory.bag.Remove(item);
				}
			break;
			
			case InventoryItem.Type.kRune:
				ItemRune rune = (ItemRune)item;
				isRemoved = user._inventory.staffRunes.Remove(rune);
				if(!isRemoved)
				{
					isRemoved = user._inventory.bag.Remove(item);
				}
			break;
		}
		return isRemoved;
	}
	
	private bool AddItem(InventoryItem item)
	{
		bool isAdded = false;
		switch(item.ItemType())
		{
			case InventoryItem.Type.kRing:
				ItemRing ring = (ItemRing)item;
				isAdded = AddItemRing(ring);
			break;
			
			case InventoryItem.Type.kRune:
				ItemRune rune = (ItemRune)item;
				isAdded = AddItemRune(rune);
			break;
			
			
		}
		return isAdded;
	}
	
	private bool AddItemRing(ItemRing ring)
	{
		bool isAdded = false;
		User user = GameManager._gameState.User;
			
		isAdded = user._inventory.bag.Add(ring);
		
		return isAdded;
	}
	
	private bool AddItemRune(ItemRune rune)
	{
		bool isAdded = false;
		User user = GameManager._gameState.User;
			
		isAdded = user._inventory.bag.Add(rune);
		
		return isAdded;
	}
	
	public PurchaseRequest.PurchaseRequestType PerformTransaction(TransactionRequest request)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		User user = GameManager._gameState.User;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(request.Uid);
			if(transaction == null)
			{
				int gems = request.TransactionGem;
				int souls = request.TransactionSouls;
				if(_lastPurcahseRequest != null)
				{
					_lastPurcahseRequest = null;
					int remainingSouls = (int)(souls - user._inventory.souls);
					gems = getGemsForSouls(remainingSouls);
					if(user._inventory.gems >= gems)
					{
						user._inventory.gems -= gems;
						user._inventory.souls = 0;
						gems = -gems;
						response = PurchaseRequest.PurchaseRequestType.Success;
					}
					else
					{
						string activity = "";
						switch(request.RequestType)
						{
							case PurchaseRequest.PurchaseRequestType.Transmutation:
								activity = "Start Transmutation";
								break;
							case PurchaseRequest.PurchaseRequestType.TransmutationBoost:
								activity = "Boost Transmutation";
								break;
							case PurchaseRequest.PurchaseRequestType.BagUpgrade:
								activity = "Upgrade Bag";
								break;
							case PurchaseRequest.PurchaseRequestType.BagUpgradeBoost:
								activity = "Boost Bag Upgrade";
								break;
							case PurchaseRequest.PurchaseRequestType.KeyRingUpgrade:
								activity = "Upgrade Key Ring";
								break;
							case PurchaseRequest.PurchaseRequestType.KeyRingUpgradeBoost:
								activity = "Boost Key Ring Upgrade";
								break;
							case PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade:
								activity = "Upgrade Transmutation Cube";
								break;
							case PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgradeBoost:
								activity = "Boost Transmutation Cube Upgrade";
								break;
							case PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade:
								activity = "Upgrade Potion Belt";
								break;
							case PurchaseRequest.PurchaseRequestType.PotionBeltUpgradeBoost:
								activity = "Boost Potion Belt Upgrade";
								break;
							case PurchaseRequest.PurchaseRequestType.PetUpgrade:
								activity = "Pet Upgrade";
								break;
							case PurchaseRequest.PurchaseRequestType.PetUpgradeBoost:
								activity = "Boost Pet Upgrade";
								break;
						}
						ShowGeneralPopup2(GeneralPopupType.InsufficientGems, gems - user._inventory.gems, activity, "Buy Gems");
					}
				}
				else
				{
					if(gems == 0 && souls == 0)
					{
						response = PurchaseRequest.PurchaseRequestType.Success;
					}
					else if(gems > 0 && user._inventory.gems >= gems)
					{
						user._inventory.gems -= gems;
						gems = -gems;
						response = PurchaseRequest.PurchaseRequestType.Success;
					}
					else if(souls > 0 && user._inventory.souls >= souls)
					{
						user._inventory.souls -= souls;
						souls = -souls;
						response = PurchaseRequest.PurchaseRequestType.Success;
					}
					else if(souls > 0 && gems == 0)
					{
						if(CheckBagCapcityForSouls(souls))
						{
							ShowGeneralPopup2(GeneralPopupType.BagUpgrade, 0, "", "");
						}
						else
						{
							int remainingSouls = (int)(souls - user._inventory.souls);
							gems = getGemsForSouls(remainingSouls);
							_lastPurcahseRequest = request;
							string activity = "";
							string action = "";
							switch(request.RequestType)
							{
								case PurchaseRequest.PurchaseRequestType.BagUpgrade:
									activity = "Upgrade Bag";
									action = "Upgrade";
									break;
								case PurchaseRequest.PurchaseRequestType.KeyRingUpgrade:
									activity = "Upgrade Key Ring";
									action = "Upgrade";
									break;
								case PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade:
									activity = "Upgrade Transmutation Cube";
									action = "Upgrade";
									break;
								case PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade:
									activity = "Upgrade Potion Belt";
									action = "Upgrade";
									break;
								case PurchaseRequest.PurchaseRequestType.PetUpgrade:
									activity = "Upgrade Pet";
									action = "Upgrade";
									break;
							}
							ShowGeneralPopup2(GeneralPopupType.InsufficientSouls, gems, activity, action);
						}
					}
				}
				if(response ==  PurchaseRequest.PurchaseRequestType.Success)
				{
					SaveTransaction(request.RequestType, request.Uid, gems, souls, request.IsCommitted, request.ParentUid, request.SkullLevel, request.ItemId);
				}
			}
		}
		return response;
	}
	
	/*public PurchaseRequest.PurchaseRequestType RollbackTransaction(string transactionId)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		User user = GameManager._gameState.User;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(transactionId);
			if(transaction != null)
			{
				int rollbackGems = (int)transaction[GEMS];
				int rollbackSouls = (int)transaction[SOULS];
				if(rollbackGems > 0)
				{
					user._inventory.gems += rollbackGems;
				}
				else if(rollbackSouls > 0)
				{
					user._inventory.souls += rollbackSouls;
				}
				SaveTransaction(transactionId, rollbackGems, rollbackSouls, true, ((transaction[PARENT_ID] != null) ? transaction[PARENT_ID].ToString() : ""));
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}*/
	
	public PurchaseRequest.PurchaseRequestType VerifyReceipt(string transactionId, bool isVerified)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(receiptList)
		{
			Dictionary<string, object> receipt = GetReceipt(transactionId);
			if(receipt != null)
			{
				receipt[VERIFIED] = isVerified;
				receipt[SENDING_REQUEST] = false;
				SaveReceipts();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType CommitTransaction(string transactionId, bool isCommited)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(transactionId);
			if(transaction != null)
			{
				transaction[COMMITTED] = isCommited;
				transaction[SENDING_REQUEST] = false;
				SaveTransactions();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType MarkCommittedAndCompleteTrue(string transactionId)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(transactionId);
			if(transaction != null)
			{
				transaction[COMMITTED] = true;
				transaction[COMPLETED] = true;
				transaction[LOGGED_TO_SERVER] = true;
				transaction[SENDING_REQUEST] = false;
				SaveTransactions();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType CompleteTransaction(string transactionId, string parentTransactionId)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(transactionId);
			if(transaction != null)
			{
				transaction[COMPLETED] = true;
				SaveTransactions();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
			if(!parentTransactionId.Equals(""))
			{
				transaction = GetTransaction(parentTransactionId);
				if(transaction != null)
				{
					transaction[COMPLETED] = true;
					SaveTransactions();
					response = PurchaseRequest.PurchaseRequestType.Success;
				}
			}
			
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType UpdateTransactionSending(string transactionId)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(transactionList)
		{
			Dictionary<string, object> transaction = GetTransaction(transactionId);
			if(transaction != null)
			{
				transaction[SENDING_REQUEST] = true;
				transaction[SENDING_REQUEST_TIME] = TimeStamp.CurrentTimeInSeconds();
				SaveTransactions();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType UpdateReceiptSending(string transactionId)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(receiptList)
		{
			Dictionary<string, object> receipt = GetReceipt(transactionId);
			if(receipt != null)
			{
				receipt[SENDING_REQUEST] = true;
				receipt[SENDING_REQUEST_TIME] = TimeStamp.CurrentTimeInSeconds();
				SaveReceipts();
				response = PurchaseRequest.PurchaseRequestType.Success;
			}
		}
		return response;
	}
	
	public PurchaseRequest.PurchaseRequestType TransactionsLoggedToServer(List<String> transactionIds)
	{
		PurchaseRequest.PurchaseRequestType response = PurchaseRequest.PurchaseRequestType.Error;
		lock(transactionList)
		{			
			List<Dictionary<string, object>> tempTransactionList = transactionList.FindAll(t => !System.Convert.ToBoolean(t[LOGGED_TO_SERVER]));
			Dictionary<string, object> transaction = null;
			int count = tempTransactionList.Count;
			for(int i = 0; i < count; i++)
			{
				transaction = tempTransactionList[i];
				if(transactionIds == null)
				{
					transaction[LOGGED_TO_SERVER] = true;
				}
				else
				{
					string transactionId = transactionIds.Find(t => t.Equals(transaction[ID].ToString()));
					if(transactionId == null)
					{
						transaction[LOGGED_TO_SERVER] = true;
					}
				}
			}
			SaveTransactions();
			response = PurchaseRequest.PurchaseRequestType.Success;	
		}
		return response;
	}
	
	private Dictionary<string, object> GetReceipt(string transactionId)
	{
		return receiptList.Find(receipt => receipt[ID].ToString().Equals(transactionId));
	}

	private Dictionary<string, object> GetTransaction(string transactionId)
	{
		return transactionList.Find(transaction => transaction[ID].ToString().Equals(transactionId));
	}
	
	private List<Dictionary<string, object>> GetTransactionsToLogged()
	{		
		Dictionary<string, object> transactionToLogged = null;
		List<Dictionary<string, object>> transactionsToLogged = new List<Dictionary<string, object>>();
		Dictionary<string, object> transaction = null;
		int count = transactionList.Count;
		for(int i = 0; i < count; i++)
		{
			transaction = transactionList[i];
			transactionToLogged = new Dictionary<string, object>();
			transactionToLogged.Add(ID, transaction[ID]);
			transactionToLogged.Add(PARENT_ID, transaction[PARENT_ID]);
			transactionToLogged.Add(REQUEST_TYPE, transaction[REQUEST_TYPE]);
			transactionToLogged.Add(GEMS, transaction[GEMS]);
			transactionToLogged.Add(SOULS, transaction[SOULS]);
			transactionToLogged.Add(CURRENT_GEMS, transaction[CURRENT_GEMS]);
			transactionToLogged.Add(CURRENT_SOULS, transaction[CURRENT_SOULS]);
			transactionToLogged.Add(SKULL_LEVLE, transaction[SKULL_LEVLE]);
			transactionToLogged.Add(ITEM_ID, transaction[ITEM_ID]);
			transactionToLogged.Add(TIME, transaction[TIME]);
			transactionToLogged.Add(COMMITTED, transaction[COMMITTED]);
			transactionToLogged.Add(COMPLETED, transaction[COMPLETED]);
			transactionsToLogged.Add(transactionToLogged);
		}
		return transactionsToLogged;
	}
	
	public bool IsParentCommited(string transactionId)
	{
		bool isCommited = false;
		Dictionary<string, object> transaction = GetTransaction(transactionId);
		if(transaction != null)
		{
			isCommited = System.Convert.ToBoolean(transaction[COMMITTED]);
		}
		return isCommited;
	}
	
	private void SaveTransaction(PurchaseRequest.PurchaseRequestType requestType, string transactionId, int gems, int souls, bool isCommitted, string parentId, int skullLevel, string itemId)
	{
		Dictionary<string, object> transaction = new Dictionary<string, object>();
		InventorySystem.Inventory inventory = GameManager._gameState.User._inventory;
		transaction.Add(ID, transactionId);
		transaction.Add(PARENT_ID, parentId);
		transaction.Add(REQUEST_TYPE, requestType.ToString());
		transaction.Add(GEMS, gems);
		transaction.Add(SOULS, souls);
		transaction.Add(CURRENT_GEMS, inventory.gems);
		transaction.Add(CURRENT_SOULS, inventory.souls);
		transaction.Add(SKULL_LEVLE, skullLevel);
		transaction.Add(ITEM_ID, itemId);
		transaction.Add(COMMITTED, isCommitted);
		transaction.Add(TIME, TimeStamp.CurrentTimeInSeconds());
		transaction.Add(SENDING_REQUEST, !isCommitted);
		transaction.Add(COMPLETED, isCommitted);
		transaction.Add(SENDING_REQUEST_TIME, TimeStamp.CurrentTimeInSeconds());
		transaction.Add(LOGGED_TO_SERVER, false);
		
		lock(transactionList)
		{
			transactionList.Add(transaction);
			SaveTransactions();
		}
	}
	
	static byte[] GetBytes(string str)
	{
	    byte[] bytes = new byte[str.Length * sizeof(char)];
	    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
	    return bytes;
	}
	
	private void SaveReceipt(string transactionId, string itemId, string receiptData, string signature)
	{
		Dictionary<string, object> receipt = new Dictionary<string, object>();
		receipt.Add(ID, transactionId);
		receipt.Add(ITEM_ID, itemId);
		receipt.Add(RECEIPT_DATA, receiptData);
		receipt.Add(SIGNATURE, signature);
		receipt.Add(VERIFIED, false);
		receipt.Add(SENDING_REQUEST, true);
		receipt.Add(SENDING_REQUEST_TIME, TimeStamp.CurrentTimeInSeconds());
		lock(receiptList)
		{
			receiptList.Add(receipt);
			SaveReceipts();
		}
	}
	
	private void LoadTransactions()
	{
		transactionList = new List<Dictionary<string, object>>();
		MonoHelpers monoHelper = GameManager.instance._monoHelpers;
		monoHelper.StartCoroutine(monoHelper.LoadFromFile(FILE_NAME, false, OnFileContentReceived));
	}
	
	private void LoadReceipts()
	{
		receiptList = new List<Dictionary<string, object>>();
		MonoHelpers monoHelper = GameManager.instance._monoHelpers;
		monoHelper.StartCoroutine(monoHelper.LoadFromFile(RECEIPTS_FILE_NAME, false, OnReceiptFileContentReceived));
	}
	
	private void SaveTransactions()
	{
		string transactions = Json.Serialize(transactionList);		
		GameManager.instance._monoHelpers.WriteIntoPersistantDataPath(transactions, FILE_NAME);
	}
	
	private void SaveReceipts()
	{
		string receipts = Json.Serialize(receiptList);		
		GameManager.instance._monoHelpers.WriteIntoPersistantDataPath(receipts, RECEIPTS_FILE_NAME);
	}
	
	private void OnFileContentReceived(bool isError, string fileContent)
	{
		if(isError)
		{
			if(GameManager.PRINT_LOGS) Debug.Log("Error reading file : " + FILE_NAME); //bug 1.0.2
			return;
		}
		
		if(fileContent != null && !fileContent.Equals(""))
		{
			IList transactions = Json.Deserialize(fileContent) as IList;
			
			if(transactionList == null)
			{
				transactionList = new List<Dictionary<string, object>>();
			}
			transactionList.Clear();
			int count = transactions.Count;
			for(int i = 0; i < count; i++)
			{
				transactionList.Add(transactions[i] as Dictionary<string, object>);
			}
		}
	}
	
	private void OnReceiptFileContentReceived(bool isError, string fileContent)
	{
		if(fileContent != null && !fileContent.Equals(""))
		{
			IList receipts = Json.Deserialize(fileContent) as IList;
			
			if(receiptList == null)
			{
				receiptList = new List<Dictionary<string, object>>();
			}
			receiptList.Clear();
			int count = receipts.Count;
			for(int i = 0; i < count; i++)
			{
				receiptList.Add(receipts[i] as Dictionary<string, object>);
			}
		}
	}
	
	public void LoadMarketData()
	{
		ServerRequest request = new InAppRequest(ServerRequest.ServerRequestType.GetInAppPurchases, ProcessResponse);
		ServerManager.Instance.ProcessRequest(request);
		GameManager.instance.CheckPendingTransactions();
	}
	
	public void LogTranactions()
	{
		if(transactionList != null && transactionList.Count > 0)
		{
			ServerRequest request = new LogTransactionsRequest(ServerRequest.ServerRequestType.LogTransactions, ProcessResponse, GetTransactionsToLogged());
			ServerManager.Instance.ProcessRequest(request);
		}
	}
	
	public void ProcessResponse(ServerResponse response)
	{
//		Debug.Log("PurchaseManager ::: ProcessResponse ::>>> " + response.ToString());
		
		switch(response.Request.RequestType)
		{
			case ServerRequest.ServerRequestType.GetInAppPurchases:
				if(response.IsSuccess)
				{
					GameManager.instance.StartCoroutine(GameManager.instance._monoHelpers.LoadFromFile(IN_APP_FILE_NAME, false, MarketJsonLoaded));
				}
				else
				{
					GameManager.instance.StartCoroutine(GameManager.instance._monoHelpers.LoadFromFile(DEFAULT_IN_APP_FILE_NAME, true, MarketJsonLoaded));
				}
			break;
			case ServerRequest.ServerRequestType.VerifyInAppPurchase:
				InAppResponse inAppResponse = (InAppResponse)response;
//				Debug.LogError("verify inapp success - "+response.IsSuccess);
				if(response.IsSuccess)
				{
					InAppRequest request = (InAppRequest)inAppResponse.Request;
					
					if(request != null)
						Analytics.logInappEvent(inAppResponse.ItemIdentifier, inAppResponse.TransactionId, request.Receipt, request.Signature, Analytics.TransactionTransitionState.Purchased);
					
					GameManager._gameState.User._inventory.gems += inAppResponse.ItemQuantity;
					SaveTransaction(PurchaseRequest.PurchaseRequestType.INAPP, inAppResponse.TransactionId, inAppResponse.ItemQuantity, 0, true, "", 0, inAppResponse.ItemIdentifier);
					GameManager.instance.scaleformCamera.generalSwf.Init();
					GameManager.instance.SaveGameState(true);

				}
				else
				{
					PurchaseManager.Instance.currentType = PurchaseManager.GeneralPopupType.None;
					GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("Transaction Error", "Unable to connect. Please check your internet connectivity.");
				}
				// TODO Remove
				InAppRequest request1 = (InAppRequest)inAppResponse.Request;
				if(request1 != null)
					Analytics.logInappEvent(inAppResponse.ItemIdentifier, inAppResponse.TransactionId, request1.Receipt, request1.Signature, Analytics.TransactionTransitionState.Purchased);
				// 
				VerifyReceipt(inAppResponse.TransactionId, response.IsSuccess);
				ShowOrHideLoadingScreen(false);
			break;
			case ServerRequest.ServerRequestType.LogTransactions:
				if(response.IsSuccess)
				{
					TransactionsLoggedToServer(null);	
				}
				else
				{
					LogTransactionsResponse logTransactionsResponse = (LogTransactionsResponse)response;
					if(logTransactionsResponse.TransactionIds != null)
					{
						TransactionsLoggedToServer(logTransactionsResponse.TransactionIds);
					}
				}
			break;
		}
	}
		
	public void MarketJsonLoaded(bool isSuccess, string text)
	{
//		SmartIAPAgent IAPAgent = MonoBehaviour.FindObjectOfType(typeof(SmartIAPAgent)) as SmartIAPAgent;
//		Debug.LogError("MarketJsonLoaded Data called - - - - - - - - "+text);

		if(GameManager.PRINT_LOGS) Debug.Log("~~~~~~~~~~~~~~~~~IN APP JSON LOADED~~~~~~~~~~~~~~~~~~~~");
		_marketProducts = Json.Deserialize(text) as IDictionary;

		//List<VirtualCurrencyPack> marketList = new List<VirtualCurrencyPack>();
		
		//IDictionary tempMarketDic = Json.Deserialize(text) as IDictionary;
		
		IList currencyPackList = _marketProducts["inapps"] as IList;
		
		string productId = "";
		IDictionary aCurrencyPackDic = null;
		int count = currencyPackList.Count;
		List<string> ProductIDs =new List<string>();
		//IAPAgent.nonconsumableProductSkus = new string[0];
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
			if(!string.IsNullOrEmpty(productId))
			{
//				Debug.LogError("productId > " + productId);
				UIManager.instance.Log("productId > " + productId);
				//productsIds += productId + ",";
				//IAPAgent.nonconsumableProductSkus[i] = productId;
				ProductIDs.Add(productId);

			}
		}
		InAppsManager.InAppsManager.instance.Init();

	}
	
	public void OnMarketProducts(string products)
	{
		lock(_products)
		{
			_products.Clear();
			if(products != null)
			{
				string[] productArr = null;
				string[] productsArr = products.Split('-');
				int length = productsArr.Length;
				for(int i = 0; i < length; i++)
				{
					productArr = productsArr[i].Split(new char[]{','});
					_products.Add(productArr[0], productArr[1]); 
				}
			}
		}
	}
	
	public void InitAndOpenStore()
	{
		CloseStore();
		//StoreController.Initialize(_marketPlace);
		//_handler = new MarketEventHandler();
		//StoreController.StoreOpening();
		
		_isMarketInit = true;
	}
	
	public void LoadProducts()
	{
		if(_isMarketInit)
		{
//			if(GameManager.PRINT_LOGS) Debug.Log("Unity RequestProducts: " + productsIds);
			//StoreController.RequestProducts(productsIds);
		}
	}
	public void BuyItem(string itemId)
	{
		//VirtualCurrencyPack currencyPack = _marketPlace.MarketList.Find(item => item.ItemId.Equals(itemId));
		//if(currencyPack != null)
		{
			//PurchaseWithMarket product = (PurchaseWithMarket)currencyPack.PurchaseType;
			//if(!currencyPack.id.Equals("") && product.MarketItem.Price <= 0)
			{
				if(ServerManager.IsInternetAvailable() && !string.IsNullOrEmpty(itemId))
				{
					lock(_products)
					{
						//if(_products.Count > 0 && _products.Contains(currencyPack.id))
						{
							Debug.Log("itemId to buy > " + itemId);
							ShowOrHideLoadingScreen(true);
//							SmartIAP.Instance().Purchase(itemId);
							//StoreInventory.BuyItem(itemId);
							//log event for purchasing that itemid!!!
						}
//						else
//						{
//							//ShowOrHideLoadingScreen(false);
//							GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("Connection Error", "Could not connect to server. Please check your internet connectivity.");
//						}
					}
				}
				else
				{
					//ShowOrHideLoadingScreen(false);
					GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("Connection Error", "Please check your internet connectivity.");
				}
			}
//			else if(currencyPack.id.Equals("") && product.MarketItem.Price > 0)
//			{
//				if(CheckBagCapcityForSouls(currencyPack.CurrencyAmount + GameManager._gameState.User._inventory.souls))
//				{
//					//ShowOrHideLoadingScreen(false);
//					ShowGeneralPopup2(GeneralPopupType.BagUpgrade, 0, "", "");
//				}
//				else
//				{
//					int gems = (int)product.MarketItem.Price;
//					TransactionRequest request = new TransactionRequest(PurchaseRequest.PurchaseRequestType.DUST, Nonce.GetUniqueID(), "", gems, 0, 0, itemId, true);
//					PurchaseRequest.PurchaseRequestType response = PerformTransaction(request);
//					if(response == PurchaseRequest.PurchaseRequestType.Success)
//					{
//						GameManager._gameState.User._inventory.souls += currencyPack.CurrencyAmount;
//						//ShowOrHideLoadingScreen(false);
//						GameManager.instance.scaleformCamera.generalSwf.Init();
//					}
//					else if(response == PurchaseRequest.PurchaseRequestType.Error)
//					{
//						//ShowOrHideLoadingScreen(false);
//						ShowGeneralPopup2(GeneralPopupType.InsufficientGems, gems - GameManager._gameState.User._inventory.gems, "Buy Souls Dust", "Buy Gems");
//					}
//				}
			}
//			else
//			{
//				//ShowOrHideLoadingScreen(false);
//				GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("Invalid IAP Product", "Requested IAP Product not found.");
//			}
//		}
//		else
//		{
//			//ShowOrHideLoadingScreen(false);
//			GameManager.instance.scaleformCamera.generalSwf.ShowUiGeneralPopup("Invalid IAP Product", "Requested IAP Product not found.");
//		}
	}
	
	public void ShowGeneralPopup2(GeneralPopupType type, int gems, string activity, string action)
	{
		string title = "";
		int numberVal = 0;
		string message = "";
		string btn1Text = "";
		string btn2Text = "";
		currentType = type;
		switch(type)
		{
			case GeneralPopupType.InsufficientGems:
				title = "Insufficient Gems";
				numberVal = gems;
				message = "gems to " + activity + ".";
				btn1Text = "Buy Gems";
				btn2Text = "Not Now";
				break;
			case GeneralPopupType.BagUpgradeOrSell:
				title = "Bag Capacity Maxed Out";
				message = "Please upgrade your Bag or sell an item.";
				btn1Text = "Upgrade Now";
				btn2Text = "Not Now";
				break;
			case GeneralPopupType.BagUpgrade:
				title = "Bag Capacity Maxed Out";
				message = "Please upgrade your Bag to hold more soul dust.";
				btn1Text = "Upgrade Now";
				btn2Text = "Not Now";
				break;
			case GeneralPopupType.KeyRingUpgrade:
				title = "Key Ring Capacity Maxed Out";
				message = "Please upgrade your Key Ring to hold more keys.";
				btn1Text = "Upgrade Now";
				btn2Text = "Not Now";
				break;
			case GeneralPopupType.PotionBeltUpgrade:
				title = "Potion Belt Capacity Maxed Out";
				message = "Please upgrade your Potion Belt to hold more potions.";
				btn1Text = "Upgrade Now";
				btn2Text = "Not Now";
				break;
			case GeneralPopupType.InsufficientSouls:
				title = "Insufficient Souls Dust";
				numberVal = gems;
				message = "gems to " + activity + ".";
				btn1Text = action;
				btn2Text = "Not Now";
				break;
		}
		Debug.Log("title >> " + title + "   message >>" + message);
		if(numberVal == 0)
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup2(title, message, btn1Text, btn2Text);
		else
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup2(title, numberVal, message, btn1Text, btn2Text);
	}
	
	public void VerifyInAppPurchase(string transactionId, string itemId, bool isSaveReceipt, string transactionReceipt, string signature = null)
	{
		string productId = itemId;
		if(isSaveReceipt)
		{
			//productId = _marketPlace.MarketList.Find(item => item.ItemId.Equals(itemId)).id;
			SaveReceipt(transactionId, productId, transactionReceipt, signature);
		}
		else
		{
			UpdateReceiptSending(transactionId);
		}
		ServerRequest request = new InAppRequest(ServerRequest.ServerRequestType.VerifyInAppPurchase, ProcessResponse, itemId, transactionId, transactionReceipt, signature);
		ServerManager.Instance.ProcessRequest(request);
	}
	
	public IEnumerator CheckPendingTransactions()
	{		
		lock(_products)
		{
			if(_products.Count == 0)
			{
				LoadProducts();
			}
		}
		lock(transactionList)
		{
			if(transactionList.Capacity > 0)
			{
				Dictionary<string, object> transaction = null;
				int count  = transactionList.Count;
				for(int i = 0; i <  count; i++)
				{
					transaction = transactionList[i];
					if(!System.Convert.ToBoolean(transaction[COMMITTED]))
					{
						if(!System.Convert.ToBoolean(transaction[SENDING_REQUEST]))
						{
							if(transaction[PARENT_ID] == null || transaction[PARENT_ID].ToString().Equals(""))
							{
								// Send Request to Server
								Debug.Log("1 - send Request to server... request = " + MiniJSON.Json.Serialize(transaction));
								SendRequest(transaction, transaction[ID].ToString(), "", 0);
							}
							else
							{
								//Dictionary<string, object> parentTransaction = GetTransaction(transaction[PARENT_ID].ToString()); 
								//if(transaction[PARENT_ID].ToString()  && System.Convert.ToBoolean(parentTransaction[COMMITTED])  )
								if(transaction[PARENT_ID] != null && (! string.IsNullOrEmpty(transaction[PARENT_ID].ToString()))) 
								{
									// Send Request to Server
									Debug.Log("2 - send Request to server... request = " + MiniJSON.Json.Serialize(transaction));
									//SendRequest(transaction, parentTransaction[ID].ToString(), transaction[ID].ToString(), 1); 
									
										
									SendRequest(transaction, transaction[PARENT_ID].ToString(), transaction[ID].ToString(), 1); 
								}
							}
						}
						else
						{
							long sendingRequestTime = System.Convert.ToInt64(transaction[SENDING_REQUEST_TIME]);
							long currentTime = TimeStamp.CurrentTimeInSeconds();
							if((currentTime - sendingRequestTime) >= 15)
							{
								if(transaction[PARENT_ID] == null || transaction[PARENT_ID].ToString().Equals(""))
								{
									// Send Request to Server
//									Debug.Log("3 - send Request to server... request = " + MiniJSON.Json.Serialize(transaction));
									SendRequest(transaction, transaction[ID].ToString(), "", 0);
								}
								else
								{
									Dictionary<string, object> parentTransaction = GetTransaction(transaction[PARENT_ID].ToString());
									if(parentTransaction != null && System.Convert.ToBoolean(parentTransaction[COMMITTED]))
									{
										Debug.Log("4 - send Request to server... request = " + MiniJSON.Json.Serialize(transaction));
										SendRequest(transaction, parentTransaction[ID].ToString(), transaction[ID].ToString(), 1);
									}
								}
							}
						}
					}
				}
				transactionList.RemoveAll(tempTransaction => (System.Convert.ToBoolean(tempTransaction[COMPLETED]) && System.Convert.ToBoolean(tempTransaction[LOGGED_TO_SERVER])));
				SaveTransactions();
			}
		}
		lock(receiptList)
		{
			if(receiptList.Count > 0)
			{
				Dictionary<string, object> receipt = null;
				int count = receiptList.Count;
				for(int i = 0; i < count; i++)
				{
					receipt = receiptList[i];
					if(!System.Convert.ToBoolean(receipt[VERIFIED]))
					{
						if(!System.Convert.ToBoolean(receipt[SENDING_REQUEST]))
						{
							VerifyInAppPurchase(receipt[ID].ToString(), receipt[ITEM_ID].ToString(), false, receipt[RECEIPT_DATA].ToString());
						}
						else
						{
							long sendingRequestTime = System.Convert.ToInt64(receipt[SENDING_REQUEST_TIME]);
							long currentTime = TimeStamp.CurrentTimeInSeconds();
							if((currentTime - sendingRequestTime) >= 15)
							{
								VerifyInAppPurchase(receipt[ID].ToString(), receipt[ITEM_ID].ToString(), false, receipt[RECEIPT_DATA].ToString());
							}
						}
					}
				}
				receiptList.RemoveAll(tempReceipt => (System.Convert.ToBoolean(tempReceipt[VERIFIED])));
				SaveReceipts();
			}
		}
		yield return new WaitForSeconds(60.0f);
	}
	
	private void SendRequest(Dictionary<string, object> transaction, string uid, string boostUid, int boost)
	{
		ServerRequest request = null;
		ServerRequestParam requestParam = new ServerRequestParam(uid, boostUid, System.Convert.ToInt32(transaction[SKULL_LEVLE]), transaction[ITEM_ID].ToString(), boost);
		string requestType = transaction[REQUEST_TYPE].ToString();
		
//		Debug.Log("requestType >> " + requestType + "  REQUEST = " + request);
		
		if(boost == 0)
		{
			if(IsTransmutation(requestType))
			{
				request = new TransmutationRequest(ServerRequest.ServerRequestType.Transmutation, 
							GameManager.instance.scaleformCamera.generalSwf.ProcessResponse, requestParam);
			}
			else if(IsUpgrade(requestType))
			{
				request = new UpgradeRequest(ServerRequest.ServerRequestType.Upgrade, 
							GameManager._gameState.User._inventory.ProcessResponse, requestParam);
			} else if(IsPet(requestType))
			{
				request = new SpiritRequest(ServerRequest.ServerRequestType.Spirit, 
				            GameManager._gameState.User._inventory.ProcessResponse, requestParam);
			}

		}
		else if(boost == 1)
		{
			if(IsTransmutationBoost(requestType))
			{
				request = new TransmutationRequest(ServerRequest.ServerRequestType.TransmutationCompleted, 
							GameManager.instance.scaleformCamera.generalSwf.ProcessResponse, requestParam);
			}
			else if(IsUpgradeBoost(requestType))
			{
				request = new UpgradeRequest(ServerRequest.ServerRequestType.UpgradeCompleted, 
							GameManager.instance.scaleformCamera.generalSwf.ProcessResponse, requestParam);
			} else if(IsPetBoost(requestType))
			{
				request = new SpiritRequest(ServerRequest.ServerRequestType.Spirit, 
				            GameManager._gameState.User._inventory.ProcessResponse, requestParam);
			}
		}
		if(request != null)
		{
			if(boost == 0)
			{
				UpdateTransactionSending(uid);
			}
			else if(boost == 1)
			{
				UpdateTransactionSending(boostUid);
			}
			Debug.LogError("SendRequest - Purchase Manager - "+requestType+" created - "+request.RequestType);
			ServerManager.Instance.ProcessRequest(request);
		}
	}
	
	public void ShowOrHideLoadingScreen(bool isShowLoading)
	{
		Debug.Log("ShowOrHideLoadingScreen > " + isShowLoading);
		if(isShowLoading)
		{
			GameManager.instance.scaleformCamera.generalSwf.ShowUILoadingScreen(true);
		}
		else
		{
			GameManager.instance.scaleformCamera.generalSwf.HideUILoadingScreen(true);
		}
	}
	
	private bool IsTransmutation(string requestType)
	{
		return requestType.Equals(PurchaseRequest.PurchaseRequestType.Transmutation.ToString());
	}
	
	private bool IsTransmutationBoost(string requestType)
	{
		return requestType.Equals(PurchaseRequest.PurchaseRequestType.TransmutationBoost.ToString());
	}
	private bool IsPet(string requestType)
	{
		return requestType.Equals(PurchaseRequest.PurchaseRequestType.PetUpgrade.ToString());
	}

	private bool IsPetBoost(string requestType)
	{
		return requestType.Equals(PurchaseRequest.PurchaseRequestType.PetUpgradeBoost.ToString());
	}

	private bool IsUpgrade(string requestType)
	{
		return (requestType.Equals(PurchaseRequest.PurchaseRequestType.BagUpgrade.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.KeyRingUpgrade.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.PotionBeltUpgrade.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgrade.ToString()));
	}
	
	private bool IsUpgradeBoost(string requestType)
	{
		return (requestType.Equals(PurchaseRequest.PurchaseRequestType.BagUpgradeBoost.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.KeyRingUpgradeBoost.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.PotionBeltUpgradeBoost.ToString())
				|| requestType.Equals(PurchaseRequest.PurchaseRequestType.TransmutationCubeUpgradeBoost.ToString()));
	}
	
	public static int getGemsForSouls(int souls)
	{
		double dGems = souls / (double)InventoryItem.SOULS_PER_GEM;
		return (int)Math.Ceiling(dGems);
	}
	
	public bool CheckBagCapcityForSouls(float souls)
	{		
		return souls > GameManager._gameState.User._inventory.bag.soulCapacity;
	}
	
	private void CloseStore()
	{
		if(_isMarketInit)
		{
			_isMarketInit = false;
			if(_products.Count > 0)
			{
				lock(_products)
				{
					_products.Clear();
				}
			}
			//StoreController.StoreClosing();
		}
	}
	
	public bool CheckGuildPurchase(string guildName, float dCost, bool performPurchase, out int requiredGems)
	{
		User currentUser = GameManager._gameState.User;
		
		bool success = false;
		float deductSouls = 0;
		float deductGems = 0;
		
		if(currentUser._inventory.souls >= dCost)
		{
			deductSouls = dCost;
			success = true;
		}
		else
		{
			float remainingSouls = dCost - currentUser._inventory.souls;
			float gemsNeeded = getGemsForSouls((int)remainingSouls);
			if(currentUser._inventory.gems >= gemsNeeded)
			{	
				deductSouls = currentUser._inventory.souls;
				deductGems = gemsNeeded;
				success = true;
			}
			else
			{
				//ShowGeneralPopup2(GeneralPopupType.InsufficientGems, (int)deductGems, "Create guild", "Buy Gems");
				//Take user to market screen...
			}
		}
		
		if(success)
		{
			if(performPurchase)
			{
				Dictionary<Analytics.ParamName, string> analyticsParams = new Dictionary<Analytics.ParamName, string>();
				analyticsParams.Add (Analytics.ParamName.GuildName, guildName);
				analyticsParams.Add (Analytics.ParamName.GuildDCost, dCost.ToString());
				Analytics.logEvent(Analytics.EventName.Guild_Buy, analyticsParams);
				
				currentUser._inventory.souls -= deductSouls;
				currentUser._inventory.gems -= (int)deductGems;
				
				GameManager.instance.SaveGameState(true);
			}
		}
		requiredGems = Mathf.CeilToInt(deductGems);
		return success;
	}
	
	public void Destroy()
	{
		CloseStore();
		_marketProducts = null;
		if(_products.Count > 0)
		{
			lock(_products)
			{
				_products.Clear();
				_products = null;
			}
		}
		//_handler = null;
		/*
		if(_marketPlace != null)
		{
			_marketPlace.Destroy();
		}
		_marketPlace = null;
		*/
		_instance = null;
	}
}
