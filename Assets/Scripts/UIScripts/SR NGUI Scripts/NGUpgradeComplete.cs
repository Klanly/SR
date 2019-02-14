using UnityEngine;
using System.Collections;

public class NGSpiritComplete : MonoBehaviour
{

    public UISprite _itemSprite;
    public UILabel _itemNameLabel;
    public UILabel _itemLevelLabel;

    public void Show(string itemId, string itemName, string itemLevel)
    {

		string spriteName = "";
		if(itemName.Equals("DRAKE")) {
			spriteName = "dragon";
		} else if(itemName.Equals("GOLEM")) {
			spriteName = "Golem";
		} else if(itemName.Equals("IMP")) {
			spriteName = "IMP";
		} else {
			spriteName = "Chtulu";
		}
		_itemSprite.spriteName = spriteName;

		_itemNameLabel.text = itemName;
        _itemLevelLabel.text = "Level " + itemLevel;
    }

    public void OnClick()
    {
        Destroy(gameObject);
    }
}
