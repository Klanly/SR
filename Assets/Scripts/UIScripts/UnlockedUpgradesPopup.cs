using UnityEngine;
using System;
using System.Collections;

public class UnlockedUpgradesPopup : MonoBehaviour {
	

	public enum UnlockedUpgradesType
	{
		Upgrades,
		Runes,
		Transmutation,
		Spirits
	};
	
	
	public dfPanel upgradePanel;
	public dfPanel runesPanel;
	public dfPanel transmutationPanel;
	public dfPanel spiritsPanel;
	
	private Action _cb;
	
	public void showUnlockedUpgrade(UnlockedUpgradesType upgradeType, Action cb)
	{
		_cb = cb;
		spiritsPanel.IsVisible = false;
		upgradePanel.IsVisible = false;
		transmutationPanel.IsVisible = false;
		runesPanel.IsVisible = false;
		
		if(upgradeType == UnlockedUpgradesType.Upgrades)
		{
			upgradePanel.IsVisible = true;
		}else if(upgradeType == UnlockedUpgradesType.Runes)
		{
			runesPanel.IsVisible = true;
		}else if(upgradeType == UnlockedUpgradesType.Transmutation)
		{
			transmutationPanel.IsVisible = true;
		}else if(upgradeType == UnlockedUpgradesType.Spirits)
		{
			spiritsPanel.IsVisible = true;			
		}
	}
	
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(_cb != null)
			_cb();
		
		Destroy(gameObject);
	}
	
}
