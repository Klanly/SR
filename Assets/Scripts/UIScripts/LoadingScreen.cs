using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

//	public dfLabel _tipLabel;
//	public dfLabel _loadingLabel;
//	
//	public dfPanel _loadingBarPanel;
//	public dfSlicedSprite _loadingBarFill;
//	public dfLabel _loadingBarLabel;
//	
//	private System.Random rand = new System.Random();
//	
	private static string [] tips = {
		"Enemies too tough? Buy better Rings!", 
		"Transmutation can upgrade rings or runes",
		"Upgrade your Potion Belt to carry more Potions",
		"Defeat Primus Nex to open up new Zones",
		"Every Spirit has unique abilities and powers",
		"You can only have one Spirit active at a time",
		"Upgrade your Arcane Keyring to carry more Keys!",
		"Upgrade the Transmutation Cube for better rewards",
		"Elemental Rings let you use that element in battle",
		"Draw the spells correctly or they may not work!",
		"Only a Rune Spell can break through a Shield"
	};
	
//	public void Start()
//	{
//		SetDefaultValues();
//		
//		ShowLoadingScreenBarText(tips[rand.Next(0, tips.Length)]);
//	}
//	
//	public void SetDefaultValues()
//	{
//		_tipLabel.Text = string.Empty;
//		HideLoadingBar();
//		ShowLoadingScreenBarText(string.Empty);
//		gameObject.GetComponent<dfPanel>().PerformLayout();
//	}
//	
//	public void HideLoadingBar()
//	{
//		//_loadingBarLabel.IsVisible = false;
//		_loadingBarPanel.IsVisible = false;
//	}
//	
//	public void ShowLoadingScreenBarText(string text)
//	{
////		Debug.Log("ShowLoadingScreenBarText > " + text);
//		
//		if(string.IsNullOrEmpty(text))
//			return;
//		
//		_loadingBarLabel.IsVisible = true;
//		_loadingBarLabel.Text = text;
//	}
//	
//	public void SetLoadingPercentage(int percentage)
//	{
//		_loadingBarLabel.IsVisible = true;
//		_loadingBarFill.FillAmount = percentage/100f;
//	}
	
}
