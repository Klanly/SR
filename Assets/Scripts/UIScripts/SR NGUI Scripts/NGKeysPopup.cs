using UnityEngine;
using System.Collections;

public class NGKeysPopup : MonoBehaviour
{
	public UILabel _RequiredKeys;
	public UILabel _price;
	public UILabel _AvailableKeys;
	public UIButton _buyButton;
	
	private int _availableKeys;

	public void init (int availableKeys, float price, int requiredKeys)
	{
		_RequiredKeys.text = "Requires " + requiredKeys.ToString ();
		_availableKeys = availableKeys;
		_AvailableKeys.text = "x" + _availableKeys.ToString ();
		_price.text = price.ToString ();
	}
	
	public void init (int availableKeys, float price)
	{
		_RequiredKeys.gameObject.SetActive (false);
		_availableKeys = availableKeys;
		_AvailableKeys.text = "x" + _availableKeys.ToString ();
		_price.text = price.ToString ();
		if(GameManager._gameState.User._inventory.keyRing.level >= GameManager._dataBank.GetMaxLevelKeyRing ().level
		   && availableKeys >= GameManager._dataBank.GetMaxLevelKeyRing ().capacity) {
			NGUITools.SetActive(_buyButton.gameObject, false);
		}
	}
	


	#region UI clicks interface...
	public void OnBuyButton ()
	{
		if (UIManager.instance.generalSwf != null && UIManager.instance.generalSwf.onKeyBuyButton ()) {
			////TODO Add inventory related code here
			//_availableKeys++;
			_AvailableKeys.text = "x" + GameManager._gameState.User._inventory.keyRing.keyCount;//_availableKeys.ToString();
			if(GameManager._gameState.User._inventory.keyRing.level >= GameManager._dataBank.GetMaxLevelKeyRing ().level
			   && GameManager._gameState.User._inventory.keyRing.keyCount >= GameManager._dataBank.GetMaxLevelKeyRing ().capacity) {
				NGUITools.SetActive(_buyButton.gameObject, false);
			}
		}
	}

	public void OnBackgroundButton ()
	{
		GameObject.Destroy (gameObject);
	}

	#endregion

	public void OnDestroy ()
	{
		if (UIManager.instance.generalSwf != null)
			UIManager.instance.generalSwf.onClosePopup ();
	}
}
