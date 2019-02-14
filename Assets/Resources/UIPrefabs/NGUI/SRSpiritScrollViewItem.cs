using UnityEngine;
using System.Collections;


public class SRSpiritScrollViewItem: UIDragScrollView
{
	public NGSpiritsUI spiritUI;
	public PetModel _petModel;
	public PetModel petModel
	{
		get
		{
			return _petModel;
		}
		set
		{
			_petModel = value;

			//Debug.LogError(_petModel.id+" assigned to the Model.");

			if(_petModel != null)
			{
				icon.gameObject.SetActive(true);
				// TODO: Uncemment the below line after adding the Sprites for Other Pets.
				//icon.spriteName = _petModel.id;
			}
			else
				icon.gameObject.SetActive(false);
		}
	}

	public UISprite icon;
	public UISprite glow;

	public bool isEquipped;
	
	public void Highlight()
	{
		glow.gameObject.SetActive(true);
	}

	public void Unhighlight()
	{
		glow.gameObject.SetActive(false);
	}
}
