using UnityEngine;
using System.Collections;

public class RaidBossDetailInfo : MonoBehaviour {

	/*
	public RaidBossDetailInfo _bossLockedPanel;
	public dfLabel _raidsDurationLabel;
	public dfLabel _raidsUnlockTimeLabel;
	public dfLabel _bossDetail_bossNameLabel;
	public dfLabel _bossDetail_bossTypeLabel;
	public dfLabel _locked_bossNameLabel;
	public dfLabel _locked_bossTypeLabel;
	public dfLabel _bossDescriptionLabel;
	public dfLabel _bossHealthPercentage;
	public dfLabel _raidersValueLabel;
	public dfLabel _rewardsValueLabel;
	public dfSlicedSprite _bossHealthFill;
	public dfButton _collectButton;
	
	[HideInInspector]
	public RaidPortalNavigator _portalNavigator;
	
	public void ShowMonsterUI(IDictionary dictionary, bool locked = false)
	{
		GetComponent<dfPanel>().IsVisible = false;
		if(_bossLockedPanel != null)
			_bossLockedPanel.GetComponent<dfPanel>().IsVisible = false;
		
		IDictionary monsterInfo = dictionary["currentMonsterInfo"] as IDictionary;
		
		if(monsterInfo == null)
		{
			GetComponent<dfPanel>().IsVisible = false;
			_bossLockedPanel.GetComponent<dfPanel>().IsVisible = false;
			return;
		}
		
		_currentBossDictionary = monsterInfo;
		if(!bool.Parse(monsterInfo["isActive"].ToString()) && !locked)
		{
			GetComponent<dfPanel>().IsVisible = false;
			_bossLockedPanel.GetComponent<dfPanel>().IsVisible = true;
			_bossLockedPanel.ShowMonsterUI(dictionary, true);
			return;
		}
		
		GetComponent<dfPanel>().IsVisible = true;
		
		if(locked) //when boss is unavailable...
		{
			_bossDetail_bossNameLabel.Text = monsterInfo["name"].ToString();
			_bossDescriptionLabel.Text = monsterInfo["title"].ToString();
			_raidsUnlockTimeLabel.Text = Upgradeui.ConvertSecToString(int.Parse(monsterInfo["endTimeTS"].ToString()));
		}
		else
		{
			
			_bossDetail_bossNameLabel.Text = monsterInfo["name"].ToString();
			_bossDetail_bossTypeLabel.Text = monsterInfo["title"].ToString();
			if(bool.Parse(monsterInfo["canLoot"].ToString()))
			{
				_bossDescriptionLabel.IsVisible = false;
				_collectButton.IsVisible = true;
			}
			else
			{
				_bossDescriptionLabel.IsVisible = true;
				_collectButton.IsVisible = false;
			}
			_bossDescriptionLabel.Text = monsterInfo["introduction"].ToString();
			string percentText = (float.Parse(monsterInfo["currentLife"].ToString()) / float.Parse(monsterInfo["maxLife"].ToString())) * 100 + "";
			percentText.Remove(4);
			_bossHealthPercentage.Text = percentText + " %";
				
			_bossHealthFill.FillAmount = (float.Parse(monsterInfo["currentLife"].ToString()) / float.Parse(monsterInfo["maxLife"].ToString()));
			_raidersValueLabel.Text = monsterInfo["raidersCount"].ToString();
			
			IDictionary rewardDictionary = monsterInfo["reward"] as IDictionary;
			_rewardsValueLabel.Text = int.Parse(rewardDictionary["gems"].ToString()) + "";
			_raidsDurationLabel.Text = Upgradeui.ConvertSecToString((int)(Helpers.UnixTimeStampToDateTime(double.Parse(monsterInfo["endTimeTS"].ToString())) - RaidsManager.Instance.ServerTime).TotalSeconds);
		}
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if(mouseEvent.Source == _collectButton)
		{
			_portalNavigator.OnCollectButton(_currentBossDictionary["portalID"].ToString());
		}
		mouseEvent.Use();
	}
	
	private IDictionary _currentBossDictionary;
	*/
}
