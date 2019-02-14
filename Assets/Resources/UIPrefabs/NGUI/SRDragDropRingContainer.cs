using UnityEngine;
using System.Collections;

public class SRDragDropRingContainer : UIDragDropContainer {

	public NGRingUi ringUI;
	public InventorySystem.ItemRing _ringModel = null;
	public InventorySystem.ItemRing ringModel
	{
		get
		{
			return _ringModel;
		}
		set
		{
			_ringModel = value;

			if(_ringModel == null)
			{
				icon.gameObject.SetActive(false);
				glow.gameObject.SetActive(false);
				return;
			}

			if(string.IsNullOrEmpty(_ringModel.id) || string.IsNullOrEmpty(_ringModel.uid))
				icon.gameObject.SetActive(false);
			else
			{
				icon.gameObject.SetActive(true);
				icon.spriteName = _ringModel.id;
			}
		}
	}

	public UISprite icon;
	public UISprite glow;

	public void OnClick()
	{
		if(ringModel != null && !string.IsNullOrEmpty(ringModel.uid))
		{
			ringUI.OnEquippedRingSelected(this);
		}
	}

	public void ShowGlow()
	{
		glow.gameObject.SetActive(true);
	}

	public void RemoveGlow()
	{
		glow.gameObject.SetActive(false);
	}
}
