using UnityEngine;
using System.Collections;

public class SRDragDropRuneContainer : UIDragDropContainer {

	public NGRuneUi runeUI;
	public InventorySystem.ItemRune _runeModel = null;
	public InventorySystem.ItemRune runeModel
	{
		get
		{
			return _runeModel;
		}
		set
		{
			_runeModel = value;

			if(_runeModel == null)
			{
				icon.gameObject.SetActive(false);
				glow.gameObject.SetActive(false);
				return;
			}

			if(string.IsNullOrEmpty(_runeModel.id) || string.IsNullOrEmpty(_runeModel.uid))
				icon.gameObject.SetActive(false);
			else
			{
				icon.gameObject.SetActive(true);
				icon.spriteName = _runeModel.id;
			}
		}
	}

	public UISprite icon;
	public UISprite glow;

	public void OnClick()
	{
		if(runeModel != null && !string.IsNullOrEmpty(runeModel.uid))
		{
			runeUI.OnEquippedRuneSelected(this);
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
