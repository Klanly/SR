using UnityEngine;
using System.Collections;

public class KeysPopup : MonoBehaviour 
{
//	public dfLabel _RequiredKeys;
//	public dfLabel _price;
//	public dfLabel _AvailableKeys;
//
//	private int _availableKeys;
//
//	public void init(int availableKeys, float price, int requiredKeys)
//	{
//		_RequiredKeys.Text = "Requires " + requiredKeys.ToString();
//		_availableKeys = availableKeys;
//		_AvailableKeys.Text = "x" + _availableKeys.ToString();
//		_price.Text = price.ToString();
//		
//		this.PerformDFLayout();
//	}
//
//	public void init(int availableKeys, float price)
//	{
//		_RequiredKeys.IsVisible = false;
//		_availableKeys = availableKeys;
//		_AvailableKeys.Text = "x" + _availableKeys.ToString();
//		_price.Text = price.ToString();
//		
//		this.PerformDFLayout();
//	}
//
//	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(mouseEvent.Source.name.Equals("BuyButton"))
//		{
//			if(UIManager.instance.generalSwf.onKeyBuyButton())
//			{
//				////TODO Add inventory related code here
//				_availableKeys++;
//				_AvailableKeys.Text = "x" + _availableKeys.ToString();
//			}
//		}
//		else
//		{
//			GameObject.Destroy(gameObject);
//		}
//	}
//	
//	public void OnDestroy()
//	{
//		if(UIManager.instance.generalSwf != null)
//			UIManager.instance.generalSwf.onClosePopup();
//	}
}
