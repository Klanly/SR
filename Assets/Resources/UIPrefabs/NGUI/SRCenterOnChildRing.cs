using UnityEngine;
using System;
using System.Collections;

public class SRCenterOnChildRing : UICenterOnChild {

	public void GoToIndexNumber(int index, bool instant = false)
	{
		Debug.LogError("GoToIndexNumber > " + index+" instant "+instant);
		float delay = 1.0f;
		if(instant) {
//			CenterOn(transform.GetChild(index));
			delay = 0.5f;
		}

		this.PerformActionWithDelay(delay, () => {
			CenterOn(transform.GetChild(index));
		});

	}

	public void GoToItem(SRDragDropRingItem item)
	{
		SRDragDropRingItem gotoItem = Array.Find<SRDragDropRingItem>(transform.GetComponentsInChildren<SRDragDropRingItem>(), (trans) => trans.ringModel.uid == item.ringModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void GoToItem(SRDragDropRingContainer item)
	{
		SRDragDropRingItem gotoItem = Array.Find<SRDragDropRingItem>(transform.GetComponentsInChildren<SRDragDropRingItem>(), (trans) => trans.ringModel.uid == item.ringModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void GoToNewRing()
	{
		this.PerformActionWithDelay(0.5f, () => {
			SRDragDropRingItem gotoItem = Array.Find<SRDragDropRingItem>(transform.GetComponentsInChildren<SRDragDropRingItem>(), trans => trans.ringModel.isNew);
			if(gotoItem == null) return;
			
			CenterOn(gotoItem.transform);
		});
	}

	public void HighlightRing(InventorySystem.ItemRing ring)
	{
		SRDragDropRingItem gotoItem = Array.Find<SRDragDropRingItem>(transform.GetComponentsInChildren<SRDragDropRingItem>(), (trans) => trans.ringModel.uid == ring.uid);
		if(gotoItem == null) return;

		gotoItem.Highlight();
	}

	public void UnhighlightRing(InventorySystem.ItemRing ring)
	{
		SRDragDropRingItem gotoItem = Array.Find<SRDragDropRingItem>(transform.GetComponentsInChildren<SRDragDropRingItem>(), (trans) => trans.ringModel.uid == ring.uid);
		if(gotoItem == null) return;

		gotoItem.Unhighlight();
	}

	public void OnGUI()
	{
//		if(GUI.Button(new Rect(10,10,100,20), "ADD"))
//		{
//			InventorySystem.ItemRing ring = DatabankSystem.Databank.GetRandomNonpremiumRingForSkullLevel(GameManager._dataBank, 5,"H");
//			ring.isNew = true;
//			GameManager._gameState.User._inventory.bag.Add(ring);
//		}
	}

	public void SetEnable(bool enable) {
		if(mScrollView != null) {
			mScrollView.enabled = enable;
		}
	}
}
