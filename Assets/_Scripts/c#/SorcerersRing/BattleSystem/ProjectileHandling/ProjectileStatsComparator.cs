using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileStatsComparator : MonoBehaviour {
	
	public static Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture> neutralDictionary = GetNeutralDictionary();
	
	public static Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture> counterDictionary = GetCounterDictionary();
	
	public GestureEmitter.Gesture _projectileType
	{
		get;
		set;
	}
	
	public float Strength
	{
		get;
		set;
	}
	
	public bool isCharged
	{
		get;set;
	}
	
	public bool IsNeutral(ProjectileStatsComparator otherProjectile)
	{
		if(neutralDictionary.ContainsKey(this._projectileType) && neutralDictionary[this._projectileType] == otherProjectile._projectileType)
		{
			return true;
		}
		
		return false;
	}
	
	public bool IsCounter(ProjectileStatsComparator otherProjectile)
	{
		if(counterDictionary.ContainsKey(this._projectileType) && counterDictionary[this._projectileType] == otherProjectile._projectileType)
		{
			return true;
		}
		
		return false;
	}
	
	
	public bool IsSame(ProjectileStatsComparator otherProjectile)
	{
		if(this._projectileType == otherProjectile._projectileType)
		{
			return true;
		}
		
		return false;
	}
	
	
	private static Dictionary<GestureEmitter.Gesture,GestureEmitter.Gesture> GetNeutralDictionary()
	{
		Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture> neutralDictionary = new Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture>();
		neutralDictionary.Add(GestureEmitter.Gesture.kFire, GestureEmitter.Gesture.kLightning);
		neutralDictionary.Add(GestureEmitter.Gesture.kEarth, GestureEmitter.Gesture.kWater);
		neutralDictionary.Add(GestureEmitter.Gesture.kLightning, GestureEmitter.Gesture.kFire);
		neutralDictionary.Add(GestureEmitter.Gesture.kWater, GestureEmitter.Gesture.kEarth);
		return neutralDictionary;
	}
	
	
	
	private static Dictionary<GestureEmitter.Gesture,GestureEmitter.Gesture> GetCounterDictionary()
	{
		Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture> neutralDictionary = new Dictionary<GestureEmitter.Gesture, GestureEmitter.Gesture>();
		neutralDictionary.Add(GestureEmitter.Gesture.kFire, GestureEmitter.Gesture.kWater);
		neutralDictionary.Add(GestureEmitter.Gesture.kLightning, GestureEmitter.Gesture.kEarth);
		neutralDictionary.Add(GestureEmitter.Gesture.kWater, GestureEmitter.Gesture.kLightning);
		neutralDictionary.Add(GestureEmitter.Gesture.kEarth, GestureEmitter.Gesture.kFire);
		return neutralDictionary;
		
	}
	
}
