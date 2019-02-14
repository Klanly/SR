using UnityEngine;
using System.Collections.Generic;

public class Triggerer
{
	protected List<Constraint> constraints;
	
	public Triggerer()
	{
		constraints = new List<Constraint>();
	}
	
	public void AddConstraint(Constraint contraint)
	{
		if (! constraints.Contains(contraint))
			constraints.Add(contraint);
	}
	
	public void RemoveConstraint(Constraint contraint)
	{
		if (constraints.Contains(contraint))
			constraints.Remove(contraint);
	}
	
	public List<Constraint> Constraints
	{
		get
		{
			return constraints;
		}
	}

	public void Supply(Dictionary<string, object> values)
	{
		foreach(KeyValuePair<string, object> entry in values)
			foreach (Constraint constraint in constraints)
				if (constraint.Parameter == entry.Key)
					constraint.Supply(entry.Value);
	}

	public bool Evaluate()
	{
		if (constraints.Count > 0)
		{
			foreach (Constraint constraint in Constraints)
				if (! constraint.Evaluate())
					return false;
			return true;
		}
		else
			return false;
	}
}
