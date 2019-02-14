using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Shrine  {
	
	public Shrine(GestureEmitter.Gesture kType)
	{
		this.Type = kType;
		_friendsList = new List<string>();
	}

	public Shrine()
	{

	}

	public enum shrineType {kFire,kWater,kEarth,kLightening};
	
	public GestureEmitter.Gesture Type;
	
	public int shrineLevel;

	public int currentShrinePoints;

	public int guildShrinePoints;

	public int maxShrinePoints;
	
	public bool isCharged;
	
	public long remainingTime;
	
	public string Tag;

	public bool isActivated;
	
	public bool isLocked;
	
	public long resetTime;
	
	private List<string> _friendsList;
	public List<string> FriendsList
	{
		get
		{
			return _friendsList;
		}
		set
		{
			_friendsList = value;
		}
	}

	public Dictionary<string,object> ToDictionary()
	{
		Dictionary<string,object> shrine = new Dictionary<string,object>();
		switch(this.Type)
		{
			case GestureEmitter.Gesture.kFire:
			shrine.Add("element","fire");
			break;
			case GestureEmitter.Gesture.kWater:
			shrine.Add("element","water");
			break;
			case GestureEmitter.Gesture.kEarth:
			shrine.Add("element","earth");
			break;
			case GestureEmitter.Gesture.kLightning:
			shrine.Add("element","lightning");
			break;
		}
		shrine.Add("isCharged",this.isCharged);
		shrine.Add("isLocked",this.isLocked);
		shrine.Add("isActivated",this.isActivated);
		shrine.Add("resetTime",this.resetTime);

		return shrine;
	}
		
	
}
