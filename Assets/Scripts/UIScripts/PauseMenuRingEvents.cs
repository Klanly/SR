using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuRingEvents : MonoBehaviour 
{

	public void OnDragEnd( dfControl control, dfDragEventArgs dragEvent )
	{
		// Add event handler code here
		Debug.Log( "DragEnd" );
	}

	public void OnDragStart( dfControl control, dfDragEventArgs dragEvent )
	{
		// Add event handler code here
		Debug.Log( "DragStart" );
	}

}
