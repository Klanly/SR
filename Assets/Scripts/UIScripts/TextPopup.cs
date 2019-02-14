using UnityEngine;
using System.Collections;

public class TextPopup : MonoBehaviour {
	
	public dfLabel _textLabel;
	
	public void Show(string popupText)
	{
		_textLabel.Text = popupText;
	}
}
