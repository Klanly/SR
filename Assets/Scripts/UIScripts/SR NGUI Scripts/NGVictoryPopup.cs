using UnityEngine;
using System;
using System.Collections;

public class NGVictoryPopup : MonoBehaviour
{

    public UILabel soulsLabel;
    public UILabel healthLostLabel;
    public UILabel battleTimeLabel;
    public UILabel topDamageLabel;
    public UILabel gradeLabel;
	
    public Action popupCB;
	
    public void showPopup(int soulsWon, int healthLost, int battleTime, int topDamage, string grade, Action popupCB)
    {
        //SoundManager.instance.PlayMenuVictorySound();

        this.soulsLabel.text = soulsWon.ToString();
        this.healthLostLabel.text = healthLost.ToString();
        this.battleTimeLabel.text = battleTime.ToString(); //this needs to be converted from seconds to min:sec format
        this.topDamageLabel.text = topDamage.ToString();
        this.gradeLabel.text = grade.ToString();
		
        this.popupCB = popupCB;
    }
	
	
    public void OnCloseClick()
    {
        if (popupCB != null)
            popupCB();
		
        GameObject.Destroy(gameObject);
    }
}
