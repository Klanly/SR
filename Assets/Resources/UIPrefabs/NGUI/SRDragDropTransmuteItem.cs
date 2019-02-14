using UnityEngine;
using System.Collections;


public class SRDragDropTransmuteItem : UIDragDropItem {

	public NGTransmuteUi ringUI;
	public InventorySystem.InventoryItem _ringModel;
	public InventorySystem.InventoryItem ringModel
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

	public bool isRing;

	public bool isEquipped;
	public bool isDragable;

	protected override void OnCloneDragRelease(GameObject surface)
	{
		Debug.LogError("OnCloneDragRelease  - in  SRDDTI - "+surface);

		SRDragDropTransmuteContainer dropContainer = surface.GetComponent<SRDragDropTransmuteContainer>();
		if(dropContainer == null) 
		{
			Destroy(gameObject);
			return;
		}
		if(dropContainer.enabled) {
			if(dropContainer.ringModel != null && !string.IsNullOrEmpty(dropContainer.ringModel.uid))
				ringUI.SwapTransmute(dropContainer, this);
			else
				ringUI.SwapTransmute(null, this);
				
			dropContainer.ringModel = ringModel;
		}

		Destroy(gameObject);
	}

	protected void OnDragStart()
	{
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
