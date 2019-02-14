using UnityEngine;
using System.Collections;

public class AIConstants
{
	public static string STREAMING_ASSET_PATH = "file://"+Application.streamingAssetsPath+"/Monsters.txt";
	
	public static string DATA_PATH = "jar:file://" + Application.dataPath + "!/assets/Monsters.txt";
	
	public const string MONSTER_TAG = "Monsters";
	
	public const string MODEL_NAME = "Name";
	public const string MODEL_SKULL_LEVEL = "SkullLevel";
	public const string MODEL_SPEED = "Speed";
	public const string MODEL_TYPE = "Type";
	public const string MODEL_CUES = "Cues";
	public const string MODEL_RACE = "Race";
	public const string MODEL_ELEMENT = "Element";
	public const string MODEL_SPECIAL = "Special";
	public const string MODEL_LIFE = "Life";
	public const string MODEL_DAMAGE = "Damage";
	public const string MODEL_HITS_TO_STUN = "Hits2Stun";
	public const string MODEL_LIFE_TO_STUN = "Life2Stun";
	public const string MODEL_CRIT_WEAKNESS = "enemyCritWeakness";
	public const string MODEL_FIRE = "Fire";
	public const string MODEL_WATER = "Water";
	public const string MODEL_EARTH = "Earth";
	public const string MODEL_LIGHTNING = "Lightning";
	public const string MODEL_SUBTLE = "Subtle";
	public const string MODEL_CHARGE = "Charge";
	public const string MODEL_NORMAL = "Normal";
	
	public const string FIELD_MODEL_SPEED_SLOW = "SLOW";
	public const string FIELD_MODEL_SPEED_FAST = "FAST";
	public const string FIELD_MODEL_SPEED_MEDIUM = "MEDIUM";
	public const string FIELD_MODEL_SPEED_HASTE = "HASTE";
	
	public const string FIELD_MODEL_TYPE_CASTER = "CASTER";
	
	public const string FIELD_MODEL_NAME_BATTLE_MAGE = "OGRE_BATTLE_MAGI";
	public const string FIELD_MODEL_NAME_BRAWLER = "OGRE_BRAWLER";
	public const string FIELD_MODEL_NAME_OGRE_SHAMAN = "OGRE_SHAMAN";
	
	public const string FIELD_MODEL_CUES_OBVIOUS = "OBVIOUS";
	public const string FIELD_MODEL_CUES_SUBTLE = "SUBTLE";
	
	public const string FIELD_MODEL_RACE_HUMAN = "HUMAN";
	public const string FIELD_MODEL_RACE_OGRE = "OGRE";
	public const string FIELD_MODEL_RACE_GOLEM = "GOLEM";
	
	public const string MODEL_SPELL_ONE = "Spell1";
	public const string MODEL_SPELL_TWO = "Spell2";
	public const string MODEL_SPELL_THREE = "Spell3";
	public const string MODEL_BUFF_TIME = "Buff Time";
	public const string FIELD_MODEL_SPECIAL_HASTE = "HUMAN";
	public const string FIELD_MODEL_SPECIAL_RAGE = "RAGE";
}


