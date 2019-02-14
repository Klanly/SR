using UnityEngine;
using System.Collections.Generic;

public class Cutscene
{
	protected string name;
	protected string place;
	protected List<Triggerer> triggers;
	protected List<Dialogue> dialogues;
	protected long priority;

	public Cutscene()
	{
		triggers = new List<Triggerer>();
		dialogues = new List<Dialogue>();
	}

	public void AddTrigger(Triggerer trigger)
	{
		if (! triggers.Contains(trigger))
			triggers.Add(trigger);
	}

	public void RemoveTrigger(Triggerer trigger)
	{
		if (triggers.Contains(trigger))
			triggers.Remove(trigger);
	}

	public void AddDialogue(Dialogue dialogue)
	{
		if (! dialogues.Contains(dialogue))
			dialogues.Add(dialogue);
	}

	public void RemoveDialogue(Dialogue dialogue)
	{
		if (dialogues.Contains(dialogue))
			dialogues.Remove(dialogue);
	}

	public List<Triggerer> Triggers
	{
		get
		{
			return triggers;
		}
	}

	public List<Dialogue> Dialogues
	{
		get
		{
			return dialogues;
		}
	}

	public long Priority
	{
		get
		{
			return priority;
		}
		set
		{
			this.priority = value;
		}
	}

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			this.name = value;
		}
	}

	public string Place
	{
		get
		{
			return place;
		}
		set
		{
			this.place = value;
		}
	}

	public void Supply(Dictionary<string, object> values)
	{
		foreach (Triggerer trigger in triggers)
			trigger.Supply(values);
	}

	public bool Evaluate()
	{
		foreach (Triggerer trigger in triggers)
			if (trigger.Evaluate())
				return true;
		return false;
	}
}
