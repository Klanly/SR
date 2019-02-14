using UnityEngine;
using System.Collections;
using System;

public class NGRaidsPopup : MonoBehaviour
{

    public UILabel positionValueLabel;
    public UILabel arcaneValueLabel;
    public UILabel damageValueLabel;

    public UILabel raidSizeLabel;
    public UILabel raidTimeLabel;

    public UILabel bossHPBarLabel;
    public UISlider bossHPBarFill;

    public Action popupCB;

	public void Show(int position, int arcana, int deltaArcane, int topDamage, int bossHp, int raidsSize, long timeLeft, Action popupCB)
    {
        positionValueLabel.text = position.ToString();
		int currentArcanePoints = (int)GameManager._gameState.User.arcanePoints;
		if(deltaArcane < 0) {
			arcaneValueLabel.text = (arcana - deltaArcane).ToString() +" [FF8C00]-"+Mathf.Abs(deltaArcane)+"[-]";
		} else {
			arcaneValueLabel.text = (arcana - deltaArcane).ToString() +" [FF8C00]+"+Mathf.Abs(deltaArcane)+"[-]";
		}
        damageValueLabel.text = topDamage.ToString();
        raidSizeLabel.text = raidsSize.ToString();
		long _time = Helpers.GetTimeDifferenceInSeconds(timeLeft, 0);
		StartCoroutine("StartCountdown", (int)_time);
		bossHPBarLabel.text = "BOSS HP " + bossHp.ToString() + "%";
        bossHPBarFill.value = bossHp / (float)100;

        this.popupCB = popupCB;
	}

    public void OnClick()          // this method is bound to RaidsPopup/PopupArt/OuterRing
    {
        if (popupCB != null)
            popupCB();
        GameObject.Destroy(gameObject);
    }
	
	IEnumerator StartCountdown(int time) {
		float deltaTime = 0;
		while(time >= 0) {
			if(raidTimeLabel != null) {
				while(deltaTime <= 1.0f) {
					deltaTime += Time.unscaledDeltaTime;
					yield return new WaitForEndOfFrame();
				}
				raidTimeLabel.text = Upgradeui.ConvertTime (time--);
				deltaTime = 0.0f;
			}
			yield return null;
		}
	}

}
