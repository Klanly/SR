using UnityEngine;
using System.Collections;
using System;


public class RaidBossPanel : MonoBehaviour {
	

	//Unlocked Panel references
//	public dfPanel unlockedPanel;
//	public dfLabel name;
//	public dfLabel alias;
//	public dfLabel healthPercentage;
//	public dfSlicedSprite healthBarFill;
//	public dfLabel raidersValue;
//	public dfLabel rewardsValue;
//
//	//undefeated state
//	public dfLabel description;
//	
//	//defeated state
//	public dfButton collectButton;
//
//	
//	//Unlocked Panel references
//	public dfPanel lockedPanel;
//	
//	//present in both
//	public dfLabel unlockTime;
//	
//	
//	// Use this for initialization
//	void Start () {	
//	}
//	
//	public string convertTime(double timeInSeconds){
//
//		long sum = (long)timeInSeconds;
//		long sec = sum % 60;
//		long min = sum / (60) % 60;
//		long hou = sum / (60 * 60 )% 24;
//		long da = sum/(60*60*24)%30;
//		
//		return da+"d "+hou+"h "+sec+"s ";
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//
//	}
//	public void setPanel(RaidAIModel raidAIModel){
//
//		unlockedPanel.IsVisible = raidAIModel.isActive;
//		lockedPanel.IsVisible = !raidAIModel.isActive;
//		
//		if(raidAIModel.canLoot){
//			collectButton.IsVisible = true;
//		}else {
//			description.Text = "description goes here";
//		}
//		name.Text = raidAIModel.name;
//		alias.Text = "Alias goes here";
//		unlockTime.Text = convertTime(raidAIModel._endTimeTS-raidAIModel._startTimeTS);
//		rewardsValue.Text = "rewards #";
//		raidersValue.Text = raidAIModel.numberOfRaiders.ToString();
////		healthBarFill.FillAmount = 
////		healthPercentage = 	
//	}
//	
//	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(mouseEvent.Source == collectButton){
//			//collect loot
//		}	
//	}
//	
	
}
