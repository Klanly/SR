using UnityEngine;
using System.Collections;

public class MarketUI : MonoBehaviour 
{
	public MarketUIInterface _marketUIInterface;
	
	public void setInterface(MarketUIInterface marketInterface)
	{
		_marketUIInterface = marketInterface;
	}

	void Start () 
	{
		if(_marketUIInterface != null)
			_marketUIInterface.OnRegisterSWFChildCallback(this);

//		gameObject.GetComponent<dfPanel>().PerformLayout();
	}

	void Update()
	{
		InputWrapper.disableTouch = true;
	}

//	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		switch(mouseEvent.Source.name)
//		{
//
//		case "1":
//			Debug.Log (mouseEvent.Source.name + "Fired");
//			mouseEvent.Use();
//			break;
//
//		case "2":
//			Debug.Log (mouseEvent.Source.name + "Fired");
//			mouseEvent.Use();
//			break;
//
//		case "3":
//			Debug.Log (mouseEvent.Source.name + "Fired");
//			mouseEvent.Use();
//			break;
//
//		case "4":
//			Debug.Log (mouseEvent.Source.name + "Fired");
//			mouseEvent.Use();
//			break;
//
//		case "5":
//			Debug.Log (mouseEvent.Source.name + "Fired");
//			mouseEvent.Use();
//			break;
//		
//		case "6":
//			BuyDust(20,5000.0f);
//			mouseEvent.Use();
//			break;
//			
//		case "7":
//			BuyDust(55,13750.0f);
//			mouseEvent.Use();
//			break;
//			
//		case "8":
//			BuyDust(120,30000.0f);
//			mouseEvent.Use();
//			break;
//			
//		case "9":
//			BuyDust(260,65000.0f);
//			mouseEvent.Use();
//			break;
//			
//		case "10":
//			BuyDust(700,175000.0f);
//			mouseEvent.Use();
//			break;
//		}
//	}

//	void BuyDust(int price, float rewardAmount)
//	{
//		if(GameManager._gameState.User._inventory.gems >= price)
//		{
//			GameManager._gameState.User._inventory.gems -= price;
//			GameManager._gameState.User._inventory.souls += rewardAmount;
//			GameManager.instance.SaveGameState(true);
//			UIManager.instance.generalSwf.Init();
//		}
//	}

}
