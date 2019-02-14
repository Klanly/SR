using UnityEngine;
using System;
using System.Collections;

public class VictoryPopup : MonoBehaviour {
	
	public dfPanel popupPanel;
	
	public dfLabel soulsLabel;
	public dfLabel healthLostLabel;
	public dfLabel battleTimeLabel;
	public dfLabel topDamageLabel;
	public dfLabel gradeLabel;
	
	public Action popupCB;
	
	public void showPopup(int soulsWon, int healthLost,int battleTime,int topDamage, string grade, Action popupCB)
	{
		this.soulsLabel.Text = soulsWon + "";
		this.healthLostLabel.Text = healthLost + "";
		this.battleTimeLabel.Text = battleTime + ""; //this needs to be converted from seconds to min:sec format
		this.topDamageLabel.Text = topDamage + "";
		this.gradeLabel.Text = grade + "";
		
		this.popupCB = popupCB;
	}
	
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == popupPanel)
		{
			if(popupCB != null)
				popupCB();
			
			GameObject.Destroy(gameObject);
		}
	}
}
