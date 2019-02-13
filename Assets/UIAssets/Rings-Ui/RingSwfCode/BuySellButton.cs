using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuySellButton : MonoBehaviour 
{
	
	public dfPanel MainUiPanelReferance;
	public bool buy;

	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(buy)
			MainUiPanelReferance.GetComponent<RingUi>().onBuyRing();
		else
			MainUiPanelReferance.GetComponent<RingUi>().onSellRing();
	}

	public void OnDoubleClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		OnClick(control,mouseEvent);
	}

}
