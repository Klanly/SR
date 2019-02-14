using UnityEngine;
using System;
using System.Collections;

public class RaidsPopup : MonoBehaviour {
	
//	public dfLabel positionValueLabel;
//	public dfLabel arcaneValueLabel;
//	public dfLabel damageValueLabel;
//	
//	public dfLabel raidSizeLabel;
//	public dfLabel raidTimeLabel;
//	
//	public dfSlicedSprite bossHPBarFill;
//	
//	public Action popupCB;
//	
//	public void Show(int position, int arcana, int topDamage, int bossHp, int raidsSize, long timeLeft, Action popupCB)
//	{
//		positionValueLabel.Text = position + "";
//		arcaneValueLabel.Text = arcana + "";
//		damageValueLabel.Text = topDamage + "";
//		raidSizeLabel.Text = raidsSize + "";
//		raidTimeLabel.Text = Upgradeui.ConvertSecToString((int) timeLeft) + "";
//		bossHPBarFill.FillAmount = bossHp/(float)100;
//		
//		this.popupCB = popupCB;
//	}
//	
//	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
//	{
//		if(popupCB != null)
//			popupCB();
//			
//		GameObject.Destroy(gameObject);
//		
//		mouseEvent.Use();
//	}
}
