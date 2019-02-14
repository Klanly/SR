using UnityEngine;
using System.Collections;


public class SRDragDropRingItem : UIDragDropItem {

	public NGRingUi ringUI;
	public InventorySystem.ItemRing _ringModel;
	public InventorySystem.ItemRing ringModel
	{
		get
		{
			return _ringModel;
		}
		set
		{
			_ringModel = value;

			if(_ringModel != null)
			{
				icon.gameObject.SetActive(true);
				icon.spriteName = _ringModel.id;
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
		SRDragDropRingContainer dropContainer = surface.GetComponent<SRDragDropRingContainer>();
		if(dropContainer == null) 
		{
			Destroy(gameObject);
			return;
		}

		if(dropContainer.ringModel != null && !string.IsNullOrEmpty(dropContainer.ringModel.uid))
			ringUI.OnRingsSwapped(dropContainer.ringModel, ringModel);
		else
			ringUI.OnRingsSwapped(null, ringModel);

		dropContainer.ringModel = ringModel;

		Destroy(gameObject);
	}

	protected void OnDragStart()
	{
		if(ringUI != null)
			ringUI.OnDragStart();
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
