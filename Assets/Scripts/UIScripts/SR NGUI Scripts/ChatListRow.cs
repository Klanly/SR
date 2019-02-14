using UnityEngine;
using System.Collections;

public class ChatListRow : MonoBehaviour 
{
	public UILabel _memberNameLabel;
	public UILabel _messageLabel;

	public void Show(string memberName, string message)
	{
		_memberNameLabel.text = memberName;
		_messageLabel.text = message;
	}
}
