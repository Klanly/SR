using UnityEngine;
using System.Collections;

public class NGUpgradeComplete : MonoBehaviour
{

    public UISprite _itemSprite;
    public UILabel _itemNameLabel;
    public UILabel _itemLevelLabel;

    public void Show(string itemId, string itemName, string itemLevel)
    {
        Debug.Log("upgraded ItemID > " + itemId);
        _itemSprite.spriteName = itemId;

        _itemNameLabel.text = itemName.Replace('_', ' ').Substring(0, itemName.IndexOfAny(new char[]
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9'
        }));
        _itemLevelLabel.text = "Level " + itemLevel;
    }

    public void OnClick()
    {
        Destroy(gameObject);
    }
}
