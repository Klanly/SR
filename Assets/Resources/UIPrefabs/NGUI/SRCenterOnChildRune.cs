using UnityEngine;
using System;
using System.Collections;

public class SRCenterOnChildRune : UICenterOnChild {

	public void GoToIndexNumber(int index)
	{
		Debug.Log("GoToIndexNumber > " + index);
		this.PerformActionWithDelay(1f, () => {
			CenterOn(transform.GetChild(index));
		});

	}

	public void GoToItem(SRDragDropRuneItem item)
	{
		SRDragDropRuneItem gotoItem = Array.Find<SRDragDropRuneItem>(transform.GetComponentsInChildren<SRDragDropRuneItem>(), (trans) => trans.runeModel.uid == item.runeModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void GoToItem(SRDragDropRuneContainer item)
	{
		SRDragDropRuneItem gotoItem = Array.Find<SRDragDropRuneItem>(transform.GetComponentsInChildren<SRDragDropRuneItem>(), (trans) => trans.runeModel.uid == item.runeModel.uid);
		if(gotoItem == null) return;

		CenterOn(gotoItem.transform);
	}

	public void GoToNewRune()
	{
		this.PerformActionWithDelay(1f, () => {
			SRDragDropRuneItem gotoItem = Array.Find<SRDragDropRuneItem>(transform.GetComponentsInChildren<SRDragDropRuneItem>(), trans => trans.runeModel.isNew);
			if(gotoItem == null) return;
			
			CenterOn(gotoItem.transform);
		});
	}

	public void HighlightRune(InventorySystem.ItemRune rune)
	{
		SRDragDropRuneItem gotoItem = Array.Find<SRDragDropRuneItem>(transform.GetComponentsInChildren<SRDragDropRuneItem>(), (trans) => trans.runeModel.uid == rune.uid);
		if(gotoItem == null) return;

		gotoItem.Highlight();
	}

	public void UnhighlightRune(InventorySystem.ItemRune rune)
	{
		SRDragDropRuneItem gotoItem = Array.Find<SRDragDropRuneItem>(transform.GetComponentsInChildren<SRDragDropRuneItem>(), (trans) => trans.runeModel.uid == rune.uid);
		if(gotoItem == null) return;

		gotoItem.Unhighlight();
	}
}
