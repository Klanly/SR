using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PetModel{

	public string id;
	public string name;
	public int uLevel;
	public int skullLevel;
	public float dCost;
	public float uTime;
	public int buffTime;
	private IDictionary abilities;
	
	const string TAG_ABILITY = "Ability";
	const string TAG_ABILITY_TYPE = "aType";
	const string TAG_ABILITY_ACTIVE = "active";
	const string TAG_ABILITY_STAT = "stat";
	const string TAG_ABILITY_ELEMENT = "element";
	
	
	//private Dictionary<string, string> abilityTypeToAbilityDictionary;
	
	public PetModel(string petId)
	{
		IDictionary petDictionary = GameManager._dataBank.GetSpiritDictionaryForID(petId);
		
		this.id = petDictionary["id"].ToString();
		this.uLevel = Int32.Parse(petDictionary["uLevel"].ToString());
		this.skullLevel = Int32.Parse(petDictionary["SkullLevel"].ToString());
		this.dCost = Single.Parse(petDictionary["dCost"].ToString());
		this.uTime = Single.Parse(petDictionary["uTime"].ToString());
		this.buffTime = Int32.Parse(petDictionary["BuffTime"].ToString());
		
		this.abilities = petDictionary["Abilities"] as IDictionary;

		//abilityTypeToAbilityDictionary = new Dictionary<string, string>();
		//populateAbilities(petDictionary);
	}

	public string getPetName()
	{
		if( id == "" )
			return "";
		return id.Substring(0,id.Length-1);
	}

	public string activeSpell
	{
		private set{	}
		get 
		{
			return getSpellForType(TAG_ABILITY_ACTIVE);
		}
	}

	public string abilityStat
	{
		private set{	}
		get 
		{
			return getSpellForType(TAG_ABILITY_STAT);
		}
	}
	
	public string abilityElement
	{
		private set{	}
		get 
		{
			return getSpellForType(TAG_ABILITY_ELEMENT);
		}
	}
	
	private string getSpellForType(string abilityType)
	{
		if(abilities == null)
			return null;

		if(string.IsNullOrEmpty(abilityType) || !this.abilities.Contains(abilityType) )
			return null;
		
		if(string.IsNullOrEmpty(abilities[abilityType].ToString().Trim()))
			return null;

		return abilities[abilityType].ToString();
	}
}