using UnityEngine;
using System;

public class Dialogue
{
	protected string actor;
	protected string script;

	public Dialogue() {}

	public Dialogue(string character, string script)
	{
		this.actor = character;
		this.script = script;
	}

	public string Actor
	{
		get
		{
			return actor;
		}
		set
		{
			this.actor = value;
		}
	}

	public string Script
	{
		get
		{
			return script;
		}
		set
		{
			this.script = value;
		}
	}
}
