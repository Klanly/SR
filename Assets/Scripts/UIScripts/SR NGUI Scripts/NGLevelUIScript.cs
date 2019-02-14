using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System.Diagnostics;

public class NGLevelUIScript : MonoBehaviour {
	
	private LevelUIInterface _levelInterface;
	
	public UIPanel topStatsPanel;
	//public UISprite keysSprite;
	public UILabel keysAmountLabel;
	public UILabel arcAmountLabel;
	
	public UISprite statsAreaPanel;		//the following two panels are children of statsAreaPanel <---
	public UIWidget spellsPanel;
	public UIWidget runesPanel;
	public UIButton monsterIconButton;
	
	public UISprite earthIcon;
	public UISprite fireIcon;
	public UISprite lightningIcon;
	public UISprite waterIcon;
	public UISprite prisonIcon;
	public UISprite lockIcon;
	public UISprite regenIcon;
	public UISprite hasteIcon;
	public UISprite amplifyIcon;
	
	
	public UILabel daysLabel;
	//public dfLabel levelInfo;
	
	public UILabel monsterHealthLabel;
	public UILabel monsterDamageLabel;
	public UILabel skullLevelLabel;
	public UILabel bossNameLabel;
	
	
	private bool canSkip = false;
	
	// Use this for initialization
	void Start () {
		
		if(_levelInterface != null)
			_levelInterface.OnRegisterSWFCallback(this);
//		canSkip = true;
	}

	void OnEnable() {
//		canSkip = true;
	}

	void OnDisable() {
//		canSkip = false;
	}

	public void OnPress() {
//		UnityEngine.Debug.LogError("OnPressed "+canSkip);
//		Invoke("StopCanSkip", 3.0f);
		if(canSkip)
			Time.timeScale = 4.0f;
	}
	
	public void OnRelease() {
//		UnityEngine.Debug.LogError("OnRelease "+canSkip);
//		Invoke("StopCanSkip", 3.0f);
		if(canSkip)
			Time.timeScale = 1.0f;
	}
	
//	public void OnLevelLoaded() {
//		canSkip = false;
//		Time.timeScale = 1.0f;
//	}
	public void SetInterface(LevelUIInterface levelInterface)
	{
		_levelInterface = levelInterface;
	}
	
	public void HideKeyIcon()
	{
		topStatsPanel.gameObject.SetActive(false);
	}
	
	public void ShowKeyIcon()
	{
		topStatsPanel.gameObject.SetActive(true);
	}
	
	
	public void OnMonsterIcon()
	{
		//OnClick function of button Monster Icon
		//		ExternalInterface.call("onMonsterIcon");
		_levelInterface.onMonsterIcon();
		
	}
	public void HideMonsterIcon()
	{
		monsterIconButton.gameObject.SetActive(false);
//		monsterIconButton.transform.parent.GetComponent<dfSprite>().IsVisible = false;
//		monsterIconButton.IsVisible = false;
	}
	public void ShowMonsterIcon()
	{
		monsterIconButton.gameObject.SetActive(true);
//		monsterIconButton.transform.parent.GetComponent<dfSprite>().IsVisible = true;
//		monsterIconButton.IsVisible = true;
	}
	
	private bool _showEnemyStats;
	private float _lastTimeValue = float.MinValue;
	void Update()
	{
		if(_showEnemyStats)
		{
			if(_lastTimeValue == float.MinValue)
				_lastTimeValue = Time.time;
			
			float diff = Time.time - _lastTimeValue;
			//			UnityEngine.Debug.Log("diff > " + diff);
			if(diff < 1.0f)
			{
				spellsPanel.gameObject.SetActive(false);
				runesPanel.gameObject.SetActive(true);
			}
			else if(diff <= 3.5f)
			{
				spellsPanel.gameObject.SetActive(true);
				runesPanel.gameObject.SetActive(false);
			}
			else
			{
				_showEnemyStats = false;
				spellsPanel.gameObject.SetActive(false);
				spellsPanel.gameObject.SetActive(false);
				statsAreaPanel.gameObject.SetActive(false);
				_lastTimeValue = float.MinValue;
			}
		}
		else
		{
			_lastTimeValue = float.MinValue;
		}
	}
	
	
	public void MonsterStats(string MonsterName,int MonsterLevel,int MonsterHealth,int MonsterDamage,IDictionary MonsterSpell)
	{
		if(string.IsNullOrEmpty(MonsterName))
		{
			statsAreaPanel.gameObject.SetActive(false);
			return;
		}
		
		statsAreaPanel.gameObject.SetActive(!statsAreaPanel.gameObject.activeInHierarchy);//.IsVisible = !statsAreaPanel.IsVisible;
		_showEnemyStats = statsAreaPanel.gameObject.activeInHierarchy;//statsAreaPanel.IsVisible;
		
		IDictionary inventoryData = MonsterSpell["monsterspell"] as IDictionary;
		
		fireIcon.gameObject.SetActive(false);
		waterIcon.gameObject.SetActive(false);
		lightningIcon.gameObject.SetActive(false);
		earthIcon.gameObject.SetActive(false);
		prisonIcon.gameObject.SetActive(false);
		lockIcon.gameObject.SetActive(false);
		regenIcon.gameObject.SetActive(false);
		hasteIcon.gameObject.SetActive(false);
		amplifyIcon.gameObject.SetActive(false);
		
		bossNameLabel.text = MonsterName;
		skullLevelLabel.text = "Level "+ MonsterLevel.ToString();
		monsterHealthLabel.text = MonsterHealth.ToString();
		monsterDamageLabel.text = MonsterDamage.ToString();


		if(inventoryData.Contains("fire")){
			fireIcon.gameObject.SetActive(bool.Parse(inventoryData["fire"].ToString()));
		}
		if(inventoryData.Contains("water")){
			waterIcon.gameObject.SetActive(bool.Parse(inventoryData["water"].ToString()));
		}
		if(inventoryData.Contains("lightning")){
			lightningIcon.gameObject.SetActive(bool.Parse(inventoryData["lightning"].ToString()));
		}
		if(inventoryData.Contains("earth")){
			earthIcon.gameObject.SetActive(bool.Parse(inventoryData["earth"].ToString()));
		}
		if(inventoryData.Contains("prison")){
			prisonIcon.gameObject.SetActive(true);
		}
		if(inventoryData.Contains("lock")){
			lockIcon.gameObject.SetActive(true);
		}
		if(inventoryData.Contains("regen")){
			regenIcon.gameObject.SetActive(true);
		}
		if(inventoryData.Contains("haste")){
			hasteIcon.gameObject.SetActive(true);
		}
		if(inventoryData.Contains("amplify")){
			amplifyIcon.gameObject.SetActive(true);
		}
		//spellsPanel.IsVisible = true;
		//runesPanel.IsVisible = false;
		//statsAreaPanel.IsVisible = true;
		//PlayStatsAnimation();
	}
	
	private void PlayStatsAnimation()
	{
		//spellsPanel.SetAnchor(true);
		this.PerformActionWithDelay(2.5f, () => {
			spellsPanel.gameObject.SetActive(false);
			
			runesPanel.gameObject.SetActive(true);
			this.PerformActionWithDelay(2.5f, () => {
				runesPanel.gameObject.SetActive(false);
				OnPlayStatsAnimationComplete();
			});
		});
		
		/*
		HOTween.To(spellsPanel, 1.0f,new TweenParms().Prop("IsVisible", false));
		HOTween.To(runesPanel, 1.0f,new TweenParms().Prop("IsVisible", true).Delay(3.0f));
		Invoke("OnPlayStatsAnimationComplete",5.0f);
		*/
	}
	
	void OnPlayStatsAnimationComplete(){
		statsAreaPanel.gameObject.SetActive(false);
	}
	
	public void OnKeysIcon()
	{
		UnityEngine.Debug.Log("OnKeysIcon");
		//call this function in OnCLick call to KeyIcon
		
		//		ExternalInterface.call("onKeysIcon");
		_levelInterface.onKeysIcon();
		
	}
	
	
	public void UpdateKeys(int amount)
	{
		keysAmountLabel.text = amount.ToString();
	}
	
	public void UpdateArc(int amount)
	{
		arcAmountLabel.text = amount.ToString();
	}
	
	public void ShowDaysText(int days)
	{
		daysLabel.gameObject.SetActive(true);
		daysLabel.text = "Day "+ days.ToString();
		//levelInfo.Opacity = 0.0f;
		//HOTween.To(levelInfo, 1.0f,new TweenParms().Prop("Opacity", 1.0f));
		
	}
	
	public void HideDaysText()
	{
		//HOTween.To(levelInfo, 1.0f,new TweenParms().Prop("Opacity", 0.0f));		
		daysLabel.gameObject.SetActive(false);
	}
	
	//	public void UpdatePoints(dfLabel label, float current)
	//	{
	//		
	//	}
	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		//		if(mouseEvent.Source.name.Equals("SpiritsButton")){
//		//			spiritsButton.IsInteractive = false;
//		//			_battleInterface.SpiritCharged();
//		//		}
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

	public void SetVisible(bool isVisible)
	{
		UnityEngine.Debug.Log("[LevelUIScript] SetDisplayVisible = " + isVisible);
		//GetComponent<dfPanel>().IsVisible = isVisible;
		gameObject.SetActive(isVisible);
//		canSkip = isVisible;
//		Invoke("StopCanSkip", 3.0f);
	}

	public void SetCanSkip(bool status) {
		canSkip = status;
		if(canSkip) {
//			Invoke("StopCanSkip", 3.0f);
			int aaa = 0;
		} else
			Time.timeScale = 1.0f;
	}

	private void StopCanSkip() {
		canSkip = false;
		Time.timeScale = 1.0f;
	}
}
