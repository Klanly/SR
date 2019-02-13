using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingDeagEvents : MonoBehaviour 
{
	public dfSprite TempObject;
	public dfAtlas ObjectAtlus;
	
	
	
	public void OnDragDrop( dfControl control, dfDragEventArgs dragEvent )
	{
			dragEvent.State = dfDragDropState.Dropped;
			dragEvent.Use();
	}

}
