using UnityEngine;
using System;
using System.Collections;

public enum Condition
{
	EqualTo,
	GreaterThan,
	LessThan,
	GreaterThanOrEqualTo,
	LessThanOrEqualTo
}

public class Constraint
{
	protected string parameter;
	protected Condition condition;
	protected object value;
	protected object actualValue;
	protected bool isSet;

	public Constraint()
	{
		isSet = false;
	}

	public Constraint(string parameter, Condition condition, object value) : this()
	{
		this.parameter = parameter;
		this.condition = condition;
		this.value = value;
	}

	public Condition Condition
	{
		get
		{
			return condition;
		}
		set
		{
			condition = value;
		}
	}

	public string Parameter
	{
		get
		{
			return parameter;
		}
		set
		{
			parameter = value;
		}
	}

	public object Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public object ActualValue
	{
		get
		{
			return actualValue;
		}
		set
		{
			Supply(value);
		}
	}

	public bool IsSet
	{
		get
		{
			return isSet;
		}
	}

	public void Supply(object actualValue)
	{
		isSet = true;
		this.actualValue = actualValue;
	}

	public bool Evaluate()
	{
		bool result = false;
		IComparable lhs = actualValue as IComparable;
		IComparable rhs = value as IComparable;
		if (isSet)
		{
			if (lhs != null && rhs != null)
			{
				switch (condition)
				{
				case Condition.EqualTo:
					result = (lhs.CompareTo(rhs) == 0);
					break;

				case Condition.GreaterThan:
					result = (lhs.CompareTo(rhs) > 0);
					break;

				case Condition.LessThan:
					result = (lhs.CompareTo(rhs) < 0);
					break;

				case Condition.GreaterThanOrEqualTo:
					result = (lhs.CompareTo(rhs) >= 0);
					break;

				case Condition.LessThanOrEqualTo:
					result = (lhs.CompareTo(rhs) <= 0);
					break;
				}
			}
		}
		return result;
	}
}