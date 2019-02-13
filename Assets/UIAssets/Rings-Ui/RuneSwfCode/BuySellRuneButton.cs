using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuySellRuneButton : MonoBehaviour 
{
	
	public dfPanel MainUiPanelReferance;
	public bool buy;

	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(buy)
			MainUiPanelReferance.GetComponent<RuneUi>().onBuyRune();
		else
			MainUiPanelReferance.GetComponent<RuneUi>().onSellRune();
	}

	public void OnDoubleClick( dfControl control, dfMouseEventArgs mouseEvent )
	{
		OnClick(control,mouseEvent);
	}

}
