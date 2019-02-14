using UnityEngine;
using System;
using System.Collections;

public class SRCenterOnChildTransmute : UICenterOnChild {

	public void GoToIndexNumber(int index)
	{
		Debug.Log("GoToIndexNumber > " + index);
		this.PerformActionWithDelay(1f, () => {
			CenterOn(transform.GetChild(index));
		});

	}

	public void GoToItem(SRDragDropTransmuteItem item)
	{
		SRDragDropTransmuteItem gotoItem = Array.Find<SRDragDropTransmuteItem>(transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), (trans) => trans.ringModel.uid == item.ringModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void GoToItem(SRDragDropRingContainer item)
	{
		SRDragDropTransmuteItem gotoItem = Array.Find<SRDragDropTransmuteItem>(transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), (trans) => trans.ringModel.uid == item.ringModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void HighlightRing(InventorySystem.InventoryItem ring)
	{
		SRDragDropTransmuteItem gotoItem = Array.Find<SRDragDropTransmuteItem>(transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), (trans) => trans.ringModel.uid == ring.uid);
		if(gotoItem == null) return;

		gotoItem.Highlight();
	}

	public void UnhighlightRing(InventorySystem.InventoryItem ring)
	{
		SRDragDropTransmuteItem gotoItem = Array.Find<SRDragDropTransmuteItem>(transform.GetComponentsInChildren<SRDragDropTransmuteItem>(), (trans) => trans.ringModel.uid == ring.uid);
		if(gotoItem == null) return;

		gotoItem.Unhighlight();
	}
}
