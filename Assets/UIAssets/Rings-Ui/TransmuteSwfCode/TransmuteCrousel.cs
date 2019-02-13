using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;

public class TransmuteCrousel : MonoBehaviour {
	
	public dfLabel TransmuteName;
	public dfLabel TransmuteLevel;
	public dfLabel TransmuteDescription;
	
	 
	public dfPanel TransmuteCarousel;
	
	private Vector2 MousePosition;

	public float MoveSpeed = 0;
	public float time = 1;
	
	private int selectedIndex;
	
	public void LateUpdate ()
	{
		Refresh();
	}
	
	
	public void Refresh()
	{
		updateTransmuteStatsArea(TransmuteCarousel.GetComponent<CarouselTransmuteCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragTransmute>()._model);
		gameObject.transform.parent.GetComponent<TransmuteUi>().CurrentTransmute = TransmuteCarousel.GetComponent<CarouselTransmuteCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragTransmute>();
	}
	
	
	public void Unhighlight(DragTransmute drag)
	{
		DragTransmute[] children = TransmuteCarousel.transform.GetComponentsInChildren<DragTransmute>();
		DragTransmute targetDrag = Array.Find<DragTransmute>(children, (child) => child._model.uid == drag._model.uid);
		targetDrag.isEquipped = false;
		(targetDrag.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = false;
	}
	
	
	public void updateTransmuteStatsArea(InventoryItem r)
	{
		TransmuteName.Text = r.ItemName();
		//RuneDescription.Text = r.description;
	}
}
