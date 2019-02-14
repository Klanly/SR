using UnityEngine;
using System;
using System.Collections;

public class NGUnlockedUpgradesPopup : MonoBehaviour {
	
	
	public enum UnlockedUpgradesType
	{
		Upgrades,
		Runes,
		Transmutation,
		Spirits
	};
	
	
	public UITexture upgradePanel;
	public UITexture runesPanel;
	public UITexture transmutationPanel;
	public UISprite spiritsPanel;
	
	private Action _cb;
	
	public void showUnlockedUpgrade(UnlockedUpgradesType upgradeType, Action cb)
	{
		_cb = cb;

		spiritsPanel.gameObject.SetActive(false);
		upgradePanel.gameObject.SetActive(false);
		transmutationPanel.gameObject.SetActive(false);
		runesPanel.gameObject.SetActive(false);
		
		if(upgradeType == UnlockedUpgradesType.Upgrades)
		{
			upgradePanel.gameObject.SetActive(true);
		}
		else if(upgradeType == UnlockedUpgradesType.Runes)
		{
			runesPanel.gameObject.SetActive(true);
		}
		else if(upgradeType == UnlockedUpgradesType.Transmutation)
		{
			transmutationPanel.gameObject.SetActive(true);
		}
		else if(upgradeType == UnlockedUpgradesType.Spirits)
		{
			spiritsPanel.gameObject.SetActive(true);
		}
		SoundManager.instance.PlayTreasureLootSound();
	}
	
	
	public void OnClick()
	{
		if(_cb != null)
			_cb();
		
		Destroy(gameObject);
	}
	
}
