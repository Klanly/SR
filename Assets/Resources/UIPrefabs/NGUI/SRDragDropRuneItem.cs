using UnityEngine;
using System.Collections;


public class SRDragDropRuneItem : UIDragDropItem {

	public NGRuneUi runeUI;
	public InventorySystem.ItemRune _runeModel;
	public InventorySystem.ItemRune runeModel
	{
		get
		{
			return _runeModel;
		}
		set
		{
			_runeModel = value;

			if(_runeModel != null)
			{
				icon.gameObject.SetActive(true);
				icon.spriteName = _runeModel.id;
			}
			else
				icon.gameObject.SetActive(false);
		}
	}

	public UISprite icon;
	public UISprite glow;


	public bool isEquipped;
	public bool isDragable;
	protected override void OnCloneDragRelease(GameObject surface)
	{
		SRDragDropRuneContainer dropContainer = surface.GetComponent<SRDragDropRuneContainer>();
		if(dropContainer == null) 
		{
			Destroy(gameObject);
			return;
		}

		if(dropContainer.runeModel != null && !string.IsNullOrEmpty(dropContainer.runeModel.uid))
			runeUI.OnRunesSwapped(dropContainer.runeModel, runeModel);
		else
			runeUI.OnRunesSwapped(null, runeModel);

		dropContainer.runeModel = runeModel;

		Destroy(gameObject);
	}

	protected void OnDragStart()
	{
		if(runeUI != null)
			runeUI.OnDragStart();
		if(isDragable)
		{
			base.OnDragStart();
		}
	}

	public void Highlight()
	{
		glow.gameObject.SetActive(true);
	}

	public void Unhighlight()
	{
		glow.gameObject.SetActive(false);
	}
}
