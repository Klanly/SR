using UnityEngine;
using System;
using System.Collections;

using GuildSystem;

public class GuildListRow : MonoBehaviour 
{
	public UISprite _guildIcon;
	public UILabel _guildNameLabel;
	public UILabel _guildPointsLabel;
	public UILabel _guildMembersLabel;

	private Guild _guild;

	public void Show(Guild guild)
	{
		_guild = guild;

		_guildIcon.spriteName = _guild.logoID.ToString();
		_guildNameLabel.text = _guild.name;
		_guildPointsLabel.text = _guild.joinCost.ToString();
		_guildMembersLabel.text = string.Format("{0}/{1}", _guild.currentMemberCount, _guild.maxMemberLimit);
	}

	public static Action<Guild> _guildTapListener;

	public void OnClick()
	{
		if(_guildTapListener != null)
			_guildTapListener(_guild);
	}
}