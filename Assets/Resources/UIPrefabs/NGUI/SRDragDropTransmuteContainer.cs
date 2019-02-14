using UnityEngine;
using System.Collections;

public class SRDragDropTransmuteContainer : UIDragDropContainer {

	public NGTransmuteUi transmuteUI;
	public InventorySystem.InventoryItem _ringModel = null;
	public InventorySystem.InventoryItem ringModel
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
			transmuteUI.OnEquippedItemSelected(this);
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
