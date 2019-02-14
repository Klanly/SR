using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

public class Buff {
	
	private static Dictionary<string, string> runeDescriptionDictionary;
		
	static Buff()
	{
		runeDescriptionDictionary = new Dictionary<string, string>();
		
		runeDescriptionDictionary["DRAIN"] = "Steals PLACEHOLDER damage from the enemy for PLACEHOLDER rounds.";
		runeDescriptionDictionary["DAZE"] = "Steals PLACEHOLDER damage from the enemy for PLACEHOLDER rounds.";
		runeDescriptionDictionary["AMPLIFY"] = "Adds PLACEHOLDER damage for PLACEHOLDER rounds.";
		runeDescriptionDictionary["LEECH SEED"] = "Steals PLACEHOLDER health for PLACEHOLDER rounds.";
		runeDescriptionDictionary["IGNITE"] = "Burns PLACEHOLDER health for PLACEHOLDER rounds. Negates PLACEHOLDER.";
		runeDescriptionDictionary["REGEN"] = "Heals PLACEHOLDER stats for PLACEHOLDER rounds.";
		runeDescriptionDictionary["GREED"] = "Gives PLACEHOLDER souls every round.";
	}
	
	public string description
	{
		get
		{
			string descriptionString = runeDescriptionDictionary[this.id];
			if(string.IsNullOrEmpty(descriptionString))
				return string.Empty;
			
			int placeholderCount = descriptionString.Split(new string[] { "PLACEHOLDER" }, System.StringSplitOptions.None).Length - 1;
			int placeholderDealt = 0;
			
			Regex regex = new Regex("PLACEHOLDER");
			
			while(placeholderCount != placeholderDealt)
			{
				switch(placeholderDealt)
				{
				case 0:
					descriptionString = regex.Replace(descriptionString, this.modifierValue + "", 1);
					placeholderDealt++;
					break;
				case 1:
					descriptionString = regex.Replace(descriptionString, this.totalDuration + "", 1);
					placeholderDealt++;
					break;
				case 2:
					descriptionString = regex.Replace(descriptionString, this.negatesBuffName, 1);
					placeholderDealt++;
					break;
				default:
					placeholderDealt = placeholderCount;
					break;
				}
			}
			
			return descriptionString;
		}
	}
	
	
	public enum BuffType {kPositive, kNegative}
	
	public enum BuffTarget {kSelf, kEnemy}
	
	private BuffTarget buffTarget
	{
		set;get;
	}
	
	public int skullLevel
	{
		set;get;
	}
	
	public int totalDuration
	{
		private set;
		get;
	}	
	
	private string targetString;
		
	public Buff()
	{

	}

	public Buff(string buffId,int skullLevel)
	{
		IDictionary buffDictionary = GameManager._dataBank.GetBuffForBuffID(buffId, skullLevel);
		
		this.id = buffId;
		
		this.skullLevel = skullLevel;
		
		this.modifierValue = float.Parse(buffDictionary["DeltaStat"].ToString());
		this.totalDuration = System.Convert.ToInt32(buffDictionary["Rounds"].ToString());
		this.duration = this.totalDuration;
		this.isPermanent = (System.Convert.ToInt32(buffDictionary["IsTick"].ToString()) == 1) ? true : false;
		this.targetProperty = buffDictionary["Stat"].ToString();
		this.targetString = buffDictionary["Target"].ToString();
		this.buffType = this.targetString.Equals("ENEMY") ? BuffType.kNegative : BuffType.kPositive;
		if(buffDictionary.Contains("Negates"))
			this.negatesBuffName = buffDictionary["Negates"].ToString();
		this.targetProperty = this.targetProperty.Equals("hits2Stun") ? "hitsToStun" : this.targetProperty;
		this.targetProperty = this.targetProperty.Equals("Life") ? "life" : this.targetProperty;
		this.targetProperty = this.targetProperty.Equals("Damage") ? "damage" : this.targetProperty;
	}
	
	public string id;
	
	public GameObject targetObject;
	
	public CharacterModel sourceModel;
	
	public CharacterModel targetModel;
	
	public BuffType buffType;
	
	public bool isPermanent;
	
	public string negatesBuffName;
	
	public string targetProperty;
	
	public float modifierValue;
	
	public int duration;
	
	public void Apply(CharacterModel targetModel)
	{	
		this.targetModel = targetModel;
	}
	
	private float endValue;
	
	private void calculateValue()
	{	}
	
	public void Tick()
	{
		if(isPermanent)
		{
			targetModel.SetPropertyValue(this.targetProperty, targetModel.GetPropertyValue(this.targetProperty) + this.modifierValue);
		}
		
		this.duration--;
	}
	
	public void Expire()
	{
		this.duration = this.totalDuration;
	}
	
	
	public void OnEnd()
	{
		
	}
	
	public IDictionary ToDictionary()
	{
		IDictionary buffDictionary = new Dictionary<string, object>();
		buffDictionary["Spell Name"] = this.id;
		buffDictionary["SkullLevel"] = this.skullLevel;
		buffDictionary["DeltaStat"] = this.modifierValue;
		buffDictionary["Rounds"] = this.totalDuration;
		buffDictionary["Stat"] = this.targetProperty;
		buffDictionary["Target"] = this.targetString;
		buffDictionary["IsTick"] = this.isPermanent;
		if(this.negatesBuffName != null)
			buffDictionary["Negates"] = this.negatesBuffName;
		return buffDictionary;
	}
	
	public override string ToString()
	{
		string buffString = "";
		buffString += "BUFF ID --->>> "+this.id;
		buffString += "BUFF DURATION --->>> "+this.duration;
		buffString += "BUFF TYPE --->>> "+this.buffType;
		
		return buffString;
	}
	
	
	public bool Equals(Buff aBuff)
	{
		if(!aBuff.id.Equals(this.id))
		{
			return false;
		}
		if(aBuff.buffType != this.buffType)
		{
			return false;
		}
		if(!aBuff.negatesBuffName.Equals(this.negatesBuffName))
		{
			return false;
		}
		if(aBuff.targetProperty != this.targetProperty)
		{
			return false;
		}
		if(aBuff.duration != this.duration)
		{
			return false;
		}
		
		return true;
	}
	
	
}
