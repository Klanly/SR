using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AIModel: CharacterModel
{

    private const int MIN_HITS_TO_STUN = 3;
	
    public enum NameTypes
    {
        HUMAN_ACOLYTE,
        OGRE_WARLOCK,
        OGRE_SHAMAN,
        OGRE_WARLORD,
        HUMAN_MAGICKER,
        OGRE_BATTLEMAGE,
        PRIMUS_NEX,
        PIT_FIEND,
        Jaghdarr,
        VALKYRIE}
    ;
	
    public enum Speed
    {
        SLOW,
        MEDIUM,
        FAST,
        HASTE,
        NOT_AVAILABLE}
    ;
	
    public enum Type
    {
        CASTER}
    ;
	
    public enum Cue
    {
        OBVIOUS,
        SUBTLE}
    ;
	
    public enum Race
    {
        OGRE,
        HUMAN,
        GOLEM}
    ;
	
    public enum Special
    {
        HASTE,
        RAGE,
        NOT_AVAILABLE}
    ;
	
    public int encounter = -1;
	
    public AIModel()
    {
        availableBuffs = new List<string>();
    }
	
    public List<string> availableBuffs;
	
    public Buff GetUnusedBuff()
    {
        Buff finalBuff = null;
        string finalBuffName = null;
		
        int count = availableBuffs.Count;
        string availableBuff = null;
		
        Buff currentBuff = null;
        for (int i = 0; i< count; i++)
        {
            availableBuff = availableBuffs [i];
            finalBuffName = availableBuff;
			
            int innerCount = _buffList.Count;
            for (int j = 0; j<innerCount; j++)
            {
                currentBuff = _buffList [j];
				
                if (availableBuff.ToLower().Equals(currentBuff.id.ToLower()))
                    finalBuffName = null;
            }
        }
		
        if (finalBuffName != null)
            finalBuff = new Buff(finalBuffName, this.skullLevel);
		
        return finalBuff;
    }
			
			
    public override void ApplyBuff(Buff buff, BattleManager battleManager, bool showSWFInfo = true, bool showTutorial = true)
    {
        if (buff.negatesBuffName != null)
        {
            List<Buff> removeBuffs = new List<Buff>();
            int count = battleManager._battleState._enemy._buffList.Count;
            Buff aBuff = null;

            for (int i = 0; i<count; i++)
            {
                aBuff = battleManager._battleState._enemy._buffList [i];
				
                if (buff.negatesBuffName.ToLower().Equals(aBuff.id.ToLower()))
                {
                    removeBuffs.Add(aBuff);
                }
            }

            battleManager._battleState._enemy.RemoveBuffs(battleManager._battleState._enemy._buffList, removeBuffs, battleManager);
        }
        base.ApplyBuff(buff, battleManager, showSWFInfo, showTutorial);
    }
	
	
    protected override void RemoveBuff(string buffName)
    {
        buffName = buffName.ToLower();
        GameManager.instance.scaleformCamera.hud.RemoveEnemyDebuff(buffName);
//		if(GameManager.PRINT_LOGS) Debug.Log("***************************************************BUFF to remove >>>>>>" + buffName);
    }
	
    protected override void AddBuff(string buffName, bool onSelf, bool showSwfInfo = true, bool showTutorial = true)
    {
        buffName = buffName.ToLower();
		
//		/*if(GameManager.PRINT_LOGS) */ Debug.Log("***************************************************BUFF ADDED >>>>>>" + buffName +
//			"SHOW SWF INFO   ::::: " + showSwfInfo + " :::::::::: showTutorial :::" + showTutorial + " ::: onSelf ::::: " + onSelf);

        if (buffName.Equals("shield") || buffName.Equals("lock"))
        {
            GameManager.instance.scaleformCamera.hud.AddEnemyDebuff(buffName, onSelf, showSwfInfo);
            return;
        }		
		
        if (TutorialManager.instance.IsTutorialCompleted(GetTutorialMethodForBuffName(buffName)))
            GameManager.instance.scaleformCamera.hud.AddEnemyDebuff(buffName, onSelf);
        else
        {
            if (!GameManager.instance._levelManager.battleManager.IsBattleEnded)
            {
                GameManager.instance.scaleformCamera.hud.AddEnemyDebuff(buffName, onSelf, showSwfInfo);
                TutorialManager.instance.ShowTutorial(GetTutorialMethodForBuffName(buffName));
            }
        }
    }
	
    public int buffTime;
	
    public string name;
	
    public int skullLevel;

    protected override string GetName()
    {
        return "ENEMY";
    }
	
    public Speed speed;
    public Type type;
    public Race race;
    public Cue cues;
    public int element;
    public int fire;
    public int charge;
    public int subtle;
    public int water;
    public int lightning;
    public int earth;
    public string modelName;
	
    private int _hitsToStun;
    public int hitsToStun
    {
        set
        {
            if (value <= MIN_HITS_TO_STUN)
                _hitsToStun = MIN_HITS_TO_STUN;
            _hitsToStun = value;
        }
        get
        {
            float extra = 0;
			
            int count = propertyBuffList.Count;
            KeyValuePair<string, Buff> pair;
            for (int i = 0; i<count; i++)
            {
                pair = propertyBuffList [i];
                if (pair.Key.Equals("hitsToStun"))
                {
                    if (!pair.Value.isPermanent)
                        extra += pair.Value.modifierValue;
                }
            }
			
            if (_hitsToStun + (int)extra <= 3)
                return 3;
            return _hitsToStun + (int)extra;
        }
    }
	
    private float _enemyCritWeakness = 0;
    public float enemyCritWeakness
    {
        private set
        {
        }
        get
        {
            float extra = 0;
			
            int count = propertyBuffList.Count;
            KeyValuePair<string, Buff> pair;
            for (int i = 0; i<count; i++)
            {
                pair = propertyBuffList [i];
                if (pair.Key.Equals("enemyCritWeakness"))
                {
                    if (!pair.Value.isPermanent)
                        extra += pair.Value.modifierValue;
                }
            }
						
            return this._enemyCritWeakness + extra;
        }
    }
	
	
    public int lifeToStun;
	
    public AIModel Clone()
    {
        AIModel cloneModel = new AIModel();
		
        cloneModel.name = this.name;
        cloneModel.skullLevel = this.skullLevel;
        cloneModel.speed = this.speed;
        cloneModel.totalLife = this.totalLife;
        cloneModel.type = this.type;
        cloneModel.race = this.race;
        cloneModel.cues = this.cues;
        cloneModel.element = this.element;
        cloneModel.fire = this.fire;
        cloneModel.water = this.water;
        cloneModel.earth = this.earth;
        cloneModel.lightning = this.lightning;
        cloneModel.charge = this.charge;
        cloneModel.subtle = this.subtle;
        cloneModel.buffTime = this.buffTime;
        cloneModel.availableBuffs = this.availableBuffs;
        cloneModel.life = this.life;
        cloneModel.damage = this.damage;
        cloneModel.hitsToStun = this.hitsToStun;
        cloneModel.lifeToStun = this.lifeToStun;
		
        return cloneModel;
    }
	
    protected override void PopulateProperties()
    {
        if (propertyDictionary.ContainsKey("life"))
            propertyDictionary ["life"] = this.life;
        else
            propertyDictionary.Add("life", this.life);
		
        if (propertyDictionary.ContainsKey("damage"))
            propertyDictionary ["damage"] = this.damage;
        else
            propertyDictionary.Add("damage", this.damage);
		
        if (propertyDictionary.ContainsKey("hitsToStun"))
            propertyDictionary ["hitsToStun"] = this.hitsToStun;
        else
            propertyDictionary.Add("hitsToStun", this.hitsToStun);
		
        if (propertyDictionary.ContainsKey("lifeToStun"))
            propertyDictionary ["lifeToStun"] = this.lifeToStun;
        else
            propertyDictionary.Add("lifeToStun", this.lifeToStun);
		
        if (propertyDictionary.ContainsKey("enemyCritWeakness"))
            propertyDictionary ["enemyCritWeakness"] = this.enemyCritWeakness;
        else
            propertyDictionary.Add("enemyCritWeakness", this.enemyCritWeakness);
    }
	
    public int GetHealIfLeechApplied()
    {
        Buff leechBuff = null;
		
        int count = _buffList.Count;
        Buff aBuff = null;
        for (int i = 0; i<count; i++)
        {
            aBuff = _buffList [i];
			
            if (aBuff.id.ToLower().Equals("leech seed"))
                leechBuff = aBuff;
        }
		
        if (leechBuff == null)
            return 0;
		
        return Mathf.CeilToInt(leechBuff.modifierValue);
    }
	
    public bool ProcDaze()
    {
        Buff dazeBuff = null;
		
        int count = _buffList.Count;
        Buff aBuff = null;
        for (int i = 0; i<count; i++)
        {
            aBuff = _buffList [i];
			
            if (aBuff.id.ToLower().Equals("daze"))
                dazeBuff = aBuff;
        }
		
        if (dazeBuff == null)
            return false;	
		
        if (new System.Random().Next(1, 101) <= dazeBuff.modifierValue)
            return true;
		
        return false;
    }
	
    public string ToString()
    {
        string str = "AIMODEL INFO : ";
        str += "name = " + name + "***";
        str += "skullLevel = " + skullLevel + "***";
        str += "speed = " + speed + "***";
        str += "life = " + life + "***";
        str += "totalLife = " + totalLife + "***";
        str += "damage = " + damage + "***";
        return str;
    }
	
    public int availableSpellCount
    {
        get
        {
            int count = 0;
            if (fire > 0)
                count++;
            if (water > 0)
                count++;
            if (earth > 0)
                count++;
            if (lightning > 0)
                count++;
            return count;
        }
        private set	{	}
    }
}
