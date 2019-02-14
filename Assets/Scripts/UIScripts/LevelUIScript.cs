using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System.Diagnostics;

public class LevelUIScript : MonoBehaviour {
	
	private LevelUIInterface _levelInterface;
	
//	public dfPanel topStatsPanel;
//	public dfSprite keysSprite;
//	public dfLabel keysAmountLabel;
//	public dfLabel arcAmountLabel;
//	
//	public dfSprite statsAreaPanel;		//the following two panels are children of statsAreaPanel <---
//	public dfPanel spellsPanel;
//	public dfPanel runesPanel;
//	public dfButton monsterIconButton;
//	
//	public dfSprite earthIcon;
//	public dfSprite fireIcon;
//	public dfSprite lightningIcon;
//	public dfSprite waterIcon;
//	public dfSprite prisonIcon;
//	public dfSprite lockIcon;
//	public dfSprite regenIcon;
//	public dfSprite hasteIcon;
//	public dfSprite amplifyIcon;
//	
//	
//	public dfLabel daysLabel;
//	//public dfLabel levelInfo;
//	
//	public dfLabel monsterHealthLabel;
//	public dfLabel monsterDamageLabel;
//	public dfLabel skullLevelLabel;
//	public dfLabel bossNameLabel;
	

	
	
	// Use this for initialization
	void Start () {
	
//		if(_levelInterface != null)
//			_levelInterface.OnRegisterSWFCallback(this);

//		gameObject.GetComponent<dfPanel>().PerformLayout();
	}
	
	
	public void SetInterface(LevelUIInterface levelInterface)
	{
		_levelInterface = levelInterface;
	}
//	
//	public void HideKeyIcon()
//	{
////		topStatsPanel.IsVisible = false;	
//	}
//	
//	public void ShowKeyIcon()
//	{
//		topStatsPanel.IsVisible = true;
//	}
	
	
	private void OnMonsterIcon()
	{
		//OnClick function of button Monster Icon
//		ExternalInterface.call("onMonsterIcon");
		_levelInterface.onMonsterIcon();

	}
//	public void HideMonsterIcon()
//	{
//		monsterIconButton.transform.parent.GetComponent<dfSprite>().IsVisible = false;
//		monsterIconButton.IsVisible = false;
//	}
//	public void ShowMonsterIcon()
//	{
//		monsterIconButton.transform.parent.GetComponent<dfSprite>().IsVisible = true;
//		monsterIconButton.IsVisible = true;
//	}
	
	private bool _showEnemyStats;
	private float _lastTimeValue = float.MinValue;
//	void Update()
//	{
//		if(_showEnemyStats)
//		{
//			if(_lastTimeValue == float.MinValue)
//				_lastTimeValue = Time.time;
//			
//			float diff = Time.time - _lastTimeValue;
////			UnityEngine.Debug.Log("diff > " + diff);
//			if(diff < 1.0f)
//			{
//				spellsPanel.IsVisible = false;
//				runesPanel.IsVisible = true;
//			}
//			else if(diff <= 3.5f)
//			{
//				spellsPanel.IsVisible = true;
//				runesPanel.IsVisible = false;
//			}
//			else
//			{
//				_showEnemyStats = false;
//				spellsPanel.IsVisible = false;
//				runesPanel.IsVisible = false;
//				statsAreaPanel.IsVisible = false;
//				_lastTimeValue = float.MinValue;
//			}
//		}
//		else
//		{
//			_lastTimeValue = float.MinValue;
//		}
//	}
	

//	public void MonsterStats(string MonsterName,int MonsterLevel,int MonsterHealth,int MonsterDamage,IDictionary MonsterSpell)
//	{
//		if(string.IsNullOrEmpty(MonsterName))
//		{
//			statsAreaPanel.IsVisible = false;
//			return;
//		}
//		
//		statsAreaPanel.IsVisible = !statsAreaPanel.IsVisible;
//		_showEnemyStats = statsAreaPanel.IsVisible;
//		
//		IDictionary inventoryData = MonsterSpell["monsterspell"] as IDictionary;
//		
//		fireIcon.IsVisible = false;
//		waterIcon.IsVisible = false;
//		lightningIcon.IsVisible = false;
//		earthIcon.IsVisible = false;
//		prisonIcon.IsVisible = false;
//		lockIcon.IsVisible = false;
//		regenIcon.IsVisible = false;
//		hasteIcon.IsVisible = false;
//		amplifyIcon.IsVisible = false;
//		
//		bossNameLabel.Text = MonsterName;
//		skullLevelLabel.Text = "Level "+ MonsterLevel.ToString();
//		monsterHealthLabel.Text = MonsterHealth.ToString();
//		monsterDamageLabel.Text = MonsterDamage.ToString();
//		
//		if(inventoryData.Contains("fire")){
//			fireIcon.IsVisible = bool.Parse(inventoryData["fire"].ToString());
//		}
//		if(inventoryData.Contains("water")){
//			waterIcon.IsVisible = bool.Parse(inventoryData["water"].ToString());
//		}
//		if(inventoryData.Contains("lightning")){
//			lightningIcon.IsVisible = bool.Parse(inventoryData["lightning"].ToString());
//		}
//		if(inventoryData.Contains("earth")){
//			earthIcon.IsVisible = bool.Parse(inventoryData["earth"].ToString());
//		}
//		if(inventoryData.Contains("prison")){
//			prisonIcon.IsVisible = true;
//		}
//		if(inventoryData.Contains("lock")){
//			lockIcon.IsVisible = true;
//		}
//		if(inventoryData.Contains("regen")){
//			regenIcon.IsVisible = true;
//		}
//		if(inventoryData.Contains("haste")){
//			hasteIcon.IsVisible = true;
//		}
//		if(inventoryData.Contains("amplify")){
//			amplifyIcon.IsVisible = true;
//		}
//		//spellsPanel.IsVisible = true;
//		//runesPanel.IsVisible = false;
//		//statsAreaPanel.IsVisible = true;
//		//PlayStatsAnimation();
//	}

//	private void PlayStatsAnimation()
//	{
//		spellsPanel.IsVisible = true;
//		this.PerformActionWithDelay(2.5f, () => {
//			spellsPanel.IsVisible = false;
//			
//			runesPanel.IsVisible = true;
//			this.PerformActionWithDelay(2.5f, () => {
//				runesPanel.IsVisible = false;
//				OnPlayStatsAnimationComplete();
//			});
//		});
//		
//		/*
//		HOTween.To(spellsPanel, 1.0f,new TweenParms().Prop("IsVisible", false));
//		HOTween.To(runesPanel, 1.0f,new TweenParms().Prop("IsVisible", true).Delay(3.0f));
//		Invoke("OnPlayStatsAnimationComplete",5.0f);
//		*/
//	}
	
//	void OnPlayStatsAnimationComplete(){
//		statsAreaPanel.IsVisible = false;
//	}
	
	private void OnKeysIcon()
	{
		UnityEngine.Debug.Log("OnKeysIcon");
		//call this function in OnCLick call to KeyIcon

//		ExternalInterface.call("onKeysIcon");
		_levelInterface.onKeysIcon();

	}
	
	
//	public void UpdateKeys(int amount)
//	{
//		keysAmountLabel.Text = amount.ToString();
//	}
	
//	public void UpdateArc(int amount)
//	{
//		arcAmountLabel.Text = amount.ToString();
//	}

//	public void ShowDaysText(int days)
//	{
//		daysLabel.IsVisible = true;
//		daysLabel.Text = "Day "+ days.ToString();
//		//levelInfo.Opacity = 0.0f;
//		//HOTween.To(levelInfo, 1.0f,new TweenParms().Prop("Opacity", 1.0f));
//		
//	}

//	public void HideDaysText()
//	{
//		//HOTween.To(levelInfo, 1.0f,new TweenParms().Prop("Opacity", 0.0f));		
//		daysLabel.IsVisible = false;
//	}
	
//	public void UpdatePoints(dfLabel label, float current)
//	{
//		
//	}
	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
////		if(mouseEvent.Source.name.Equals("SpiritsButton")){
////			spiritsButton.IsInteractive = false;
////			_battleInterface.SpiritCharged();
////		}
//		
//		if(mouseEvent.Source == keysSprite || mouseEvent.Source == keysAmountLabel)
//		{
//			OnKeysIcon();
//		}
//		else if(mouseEvent.Source == monsterIconButton)
//		{
//			OnMonsterIcon();
//		}
//	}
//	
//	public void SetVisible(bool isVisible)
//	{
//		UnityEngine.Debug.Log("[LevelUIScript] SetDisplayVisible = " + isVisible);
//		GetComponent<dfPanel>().IsVisible = isVisible;
//		
//	}

}
