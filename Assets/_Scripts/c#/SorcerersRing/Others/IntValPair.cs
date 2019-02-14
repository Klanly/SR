using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntValPair <T> : IComparable
{
	int IComparable.CompareTo(object anObject)
	{
		IntValPair<T> otherPair = (IntValPair<T>) anObject;
		if(this.intKey > otherPair.intKey)
			return 1;
		else if(this.intKey < otherPair.intKey)
			return -1;
		else
			return 0;
	}
	
	public int intKey;
	public T val;
	
	public IntValPair(int key, T val)
	{
		this.intKey = key;
		this.val = val;
	}
}
