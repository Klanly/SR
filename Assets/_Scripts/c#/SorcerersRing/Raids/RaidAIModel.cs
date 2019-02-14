using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RaidAIModel : AIModel //,  IComparable
{
    /*
	int IComparable.CompareTo(object aModel)
	{
		RaidAIModel otherModel = (RaidAIModel) aModel;
		
		int otherIndex = otherModel.portalI
		
		if(this.level > otherBag.level)
			return 1;
		else if(this.level < otherBag.level)
			return -1;
		else
			return 0;
	}
	*/
	
    public float maxLife;
    public bool canLoot = false;
    public float userDamage;
	public bool isActive;
	public string portalID;
	
    public string arenaName;
	
    public long _startTimeTS;
    public long _endTimeTS;
    public long _nextEnableTS;
	
    public int numberOfRaiders;
    
    public int rewardGem;
	
    public IDictionary rawDictionary;
	
    /*
	public DateTime startTime
	{
		get
		{
			return Helpers.UnixTimeStampToDateTime(_startTimeTS);
		}
		private set	{	}
	}
	
	public DateTime endTime
	{
		get
		{
			return Helpers.UnixTimeStampToDateTime(_endTimeTS);
		}
		private set	{	}
	}
	*/
	
    public RaidAIModel(IDictionary monsterDictionary) : base()
    {
        rawDictionary = monsterDictionary;

//		Debug.LogError ("RaidAIModel => " + monsterDictionary.Keys.ToString ());

        this.name = monsterDictionary [RaidConstants.RM_NAME].ToString();

        IDictionary staticDataDictionary = monsterDictionary ["staticData"] as IDictionary;
        IDictionary dynamicDataDictionary = monsterDictionary ["dynamicData"] as IDictionary;

        this.skullLevel = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_SKULL_LEVEL].ToString());

        this.portalID = staticDataDictionary [RaidConstants.RM_PORTAL_ID].ToString();

        this.modelName = staticDataDictionary [RaidConstants.RM_MODEL_NAME].ToString();
        this.arenaName = staticDataDictionary [RaidConstants.RM_ARENA_NAME].ToString();
        this.totalLife = Convert.ToInt32(dynamicDataDictionary [RaidConstants.RM_CURRENT_LIFE].ToString());
//				this.totalLife = 1000;//Convert.ToInt32(dynamicDataDictionary[RaidConstants.RM_CURRENT_LIFE].ToString()); //farhan
        this.life = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_LIFE].ToString());//life
//				this.life = 150;//Convert.ToInt32(staticDataDictionary[RaidConstants.RM_LIFE].ToString());//life //farhan

        this.fire = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_FIRE].ToString());
        this.water = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_WATER].ToString());
        this.earth = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_EARTH].ToString());
        this.lightning = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_LIGHTNING].ToString());

        string speedVal = staticDataDictionary [RaidConstants.RM_SPEED].ToString();
        if (speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_SLOW))
            this.speed = Speed.SLOW;
        else if (speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_MEDIUM))
            this.speed = Speed.MEDIUM;
        else if (speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_FAST))
            this.speed = Speed.FAST;
        else if (speedVal.Equals(AIConstants.FIELD_MODEL_SPEED_HASTE))
            this.speed = Speed.HASTE;
        else
            this.speed = Speed.NOT_AVAILABLE;
		
        string raceVal = staticDataDictionary [RaidConstants.RM_RACE].ToString();
        if (raceVal.Equals(AIConstants.FIELD_MODEL_RACE_OGRE))
            this.race = AIModel.Race.OGRE;
        else if (raceVal.Equals(AIConstants.FIELD_MODEL_RACE_HUMAN))
            this.race = AIModel.Race.HUMAN;
        else if (raceVal.Equals(AIConstants.FIELD_MODEL_RACE_GOLEM))
            this.race = AIModel.Race.GOLEM;
		
        string firstSpell = staticDataDictionary [RaidConstants.RM_SPELL_1].ToString();
        string secondSpell = staticDataDictionary [RaidConstants.RM_SPELL_2].ToString();
        string thirdSpell = staticDataDictionary [RaidConstants.RM_SPELL_3].ToString();
        if (!firstSpell.Equals("NONE"))
            this.availableBuffs.Add(firstSpell);
        if (!secondSpell.Equals("NONE"))
            this.availableBuffs.Add(secondSpell);
        if (!thirdSpell.Equals("NONE"))
            this.availableBuffs.Add(thirdSpell);
		
        this.buffTime = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_BUFF_TIME].ToString());
		
        this.damage = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_DAMAGE].ToString());
        this.hitsToStun = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_HITS_TO_STUN].ToString());
        this.lifeToStun = Convert.ToInt32(staticDataDictionary [RaidConstants.RM_LIFE_TO_STUN].ToString());
		
        this.maxLife = Convert.ToSingle(staticDataDictionary [RaidConstants.RM_MAX_LIFE].ToString());
//		this.isEnabled = Convert.ToBoolean(dynamicDataDictionary [RaidConstants.RM_ENABLED].ToString());

		//				this.maxLife = 1000;//Convert.ToSingle(staticDataDictionary[RaidConstants.RM_MAX_LIFE].ToString());//farhan
        this._startTimeTS = long.Parse(dynamicDataDictionary [RaidConstants.RM_START_TIME_TS].ToString());
        if (Convert.ToBoolean(dynamicDataDictionary [RaidConstants.RM_IS_ACTIVE].ToString()))
        {
            this._endTimeTS = long.Parse(dynamicDataDictionary [RaidConstants.RM_END_TIME_TS].ToString());
        }
        this._nextEnableTS = long.Parse(dynamicDataDictionary [RaidConstants.RM_NEXT_ENABLE_TIME_TS].ToString());
        this.canLoot = Convert.ToBoolean(dynamicDataDictionary [RaidConstants.RM_CAN_LOOT].ToString());
        this.userDamage = Convert.ToSingle(dynamicDataDictionary [RaidConstants.RM_USER_DAMAGE].ToString());
		
        this.isActive = Convert.ToBoolean(dynamicDataDictionary [RaidConstants.RM_IS_ACTIVE].ToString());
//				this.isActive = true;//Convert.ToBoolean(dynamicDataDictionary[RaidConstants.RM_IS_ACTIVE].ToString());//farhan
        this.numberOfRaiders = Convert.ToInt32(dynamicDataDictionary [RaidConstants.RM_NUMBER_RAIDERS].ToString());
        
        IDictionary rewardDictionary = staticDataDictionary [RaidConstants.RM_REWARD] as IDictionary;
        this.rewardGem = int.Parse(rewardDictionary [RaidConstants.RM_GEMS].ToString());
		
//		Debug.Log(string.Format("name = {0} and life = {1} and totalLife = {2} and maxLife = {3}", name, life, totalLife, maxLife));
    }
	
    public void DecrementTimers(int byAmount)
    {
        _startTimeTS -= byAmount;
        _endTimeTS -= (long)byAmount;
    }
	
    public IDictionary ToDictionary()
    {
        IDictionary rawDictionary = this.rawDictionary;
        rawDictionary [RaidConstants.RM_CAN_LOOT] = false;
        rawDictionary [RaidConstants.RM_START_TIME_TS] = this._startTimeTS;
        rawDictionary [RaidConstants.RM_END_TIME_TS] = this._endTimeTS;
        return rawDictionary;
    }
	
    public int GetPositionOfUserInLeaderboard(float userid)
    {
        return 1;
        IList leaderboard = rawDictionary ["leaderBoard"] as IList;
        IDictionary lbObject = null;
        for (int i = 0; i<leaderboard.Count; i++)
        {
            lbObject = leaderboard [i] as IDictionary;
			
            if (Int32.Parse(lbObject ["userID"].ToString()) == userid)
            {
                if (Int32.Parse(lbObject ["damage"].ToString()) > 0)
                    return i + 1;
                return 0;
            }
        }
        return 0;
    }

    public String ToString()
    {
        string str = base.ToString();
        str += " ----- maxLife" + maxLife;
        return str;
    }
    //public string prefabName = "OGRE_WARLOCK";
}
