using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public abstract class CharacterModel
{
	private static Dictionary<string, TutorialManager.TutorialsAndCallback> buffIdToScaleformMethodDictionary;
	
	static CharacterModel ()
	{
		buffIdToScaleformMethodDictionary = new Dictionary<string, TutorialManager.TutorialsAndCallback> ();
		//buffIdToScaleformMethodDictionary["greed"] = TutorialManager.TutorialsAndCallback.HeroBuffGreed;
		buffIdToScaleformMethodDictionary ["leech seed"] = TutorialManager.TutorialsAndCallback.SpellLeechseed;
		buffIdToScaleformMethodDictionary ["drain"] = TutorialManager.TutorialsAndCallback.SpellDrain;
		buffIdToScaleformMethodDictionary ["daze"] = TutorialManager.TutorialsAndCallback.SpellDaze;
		buffIdToScaleformMethodDictionary ["amplify"] = TutorialManager.TutorialsAndCallback.SpellAmplify;
		buffIdToScaleformMethodDictionary ["haste"] = TutorialManager.TutorialsAndCallback.SpellHaste;
		buffIdToScaleformMethodDictionary ["lock"] = TutorialManager.TutorialsAndCallback.SpellLock;
		buffIdToScaleformMethodDictionary ["prison"] = TutorialManager.TutorialsAndCallback.SpellPrison;
		buffIdToScaleformMethodDictionary ["regen"] = TutorialManager.TutorialsAndCallback.SpellRegen;
		buffIdToScaleformMethodDictionary ["ignite"] = TutorialManager.TutorialsAndCallback.SpellIgnite;
		buffIdToScaleformMethodDictionary ["smash"] = TutorialManager.TutorialsAndCallback.SpellIgnite;
		buffIdToScaleformMethodDictionary ["dispel"] = TutorialManager.TutorialsAndCallback.SpellIgnite;
	}
	
	protected static TutorialManager.TutorialsAndCallback GetTutorialMethodForBuffName (string buffId)
	{
		buffId = buffId.ToLower ();
		return buffIdToScaleformMethodDictionary [buffId];
	}
	
	protected Dictionary<string, float> propertyDictionary;
		
	protected List<KeyValuePair<string, Buff>> propertyBuffList;
    
	public List<Buff> _buffList;
	
	public void ClearBuffs (BattleManager battleManager = null)
	{
		int count = _buffList.Count;
		Buff buff = null;
		for (int i = 0; i<count; i++) {
			buff = _buffList [i];
			buff.duration = 0;
		}
		
		TickBuffs ();
		_buffList.Clear ();
		propertyBuffList.Clear ();
	}
	
	private int _totalLife;
	public int totalLife {
		set {
			if (value <= 0)
				_totalLife = 0;
			else
				_totalLife = value;
		}
		get {
			return _totalLife;
		}
	}
	
	private int _life;
	public int life {
		set {
			if (value <= 0)
				_life = 0;
			else if (value >= this.totalLife)
				_life = this.totalLife;
			else
				_life = value;
		}
		get {
			float extra = 0;
			
			KeyValuePair<string, Buff> pair;
			int count = propertyBuffList.Count;
			for (int i = 0; i<count; i++) {
				pair = propertyBuffList [i];
				
				if (pair.Key.Equals ("life")) {
					if (!pair.Value.isPermanent)
						extra += pair.Value.modifierValue;
				}
			}
			
			if (this._life >= this.totalLife)
				_life = totalLife;
			
			return this._life + (int)extra;
		}
	}
	
	private int _damage;
	public int damage {
		set {
			if (value <= 0)
				_damage = 0;
			else
				_damage = value;
		}
		get {
			float extra = 0;
			KeyValuePair<string, Buff> pair;
			int count = propertyBuffList.Count;
			for (int i = 0; i<count; i++) {
				pair = propertyBuffList [i];
				if (pair.Key.Equals ("damage")) {
					if (!pair.Value.isPermanent) {
						extra += pair.Value.modifierValue;
					}
				}
			}
			return this._damage + (int)extra;
		}
	}
	
	protected abstract string GetName ();
	
	
	protected CharacterModel ()
	{
		propertyDictionary = new Dictionary<string, float> ();
		_buffList = new List<Buff> ();
		propertyBuffList = new List<KeyValuePair<string, Buff>> ();
	}
	
	protected abstract void PopulateProperties ();
	
	public bool HasApplied (Buff aBuff)
	{
		Buff buff = null;
		int count = _buffList.Count;
		for (int i = 0; i<count; i++) {
			buff = _buffList [i];
			if (buff.id.ToLower ().Equals (aBuff.id.ToLower ()))
				return true;
		}
		
		return false;
	}
	
	public bool HasApplied (string aBuffName)
	{
		Buff buff = null;
		int count = _buffList.Count;
		for (int i = 0; i< count; i++) {
			buff = _buffList [i];
			if (buff.id.ToLower ().Equals (aBuffName.ToLower ()))
				return true;
		}
		
		return false;
	}
	
	public float GetPropertyValue (string propertyName)
	{
		PopulateProperties ();
		if (propertyDictionary.ContainsKey (propertyName)) {
			return propertyDictionary [propertyName];
		}
		throw new System.Exception ("No Property : " + propertyName + " found!");
	}
	
	public void SetPropertyValue (string propertyName, float val)
	{
		PopulateProperties ();
		
		if (propertyDictionary.ContainsKey (propertyName)) {
			propertyDictionary [propertyName] = val;
			
			//Reflection... To set value in the object as well as in the object's dictionary
			PropertyInfo propInfo = this.GetType ().GetProperty (propertyName);
			propInfo.SetValue (this, Convert.ChangeType (propertyDictionary [propertyName], propInfo.PropertyType), null);
			return;
		}
		throw new System.Exception ("No Property : " + propertyName + " found!");
	}

	#region buff Handling
	
	public virtual void ApplyBuff (Buff buff, BattleManager battleManager, bool showSWFInfo = true, bool showTutorial = true)
	{
		Buff aBuff = null;
		int count = _buffList.Count;
		for (int i = 0; i<count; i++) {
			aBuff = _buffList [i];
			if (buff.id.ToLower ().Equals (aBuff.id.ToLower ())) {
				aBuff.duration = aBuff.totalDuration;
				return;
			}
		}
		
		this.AddToBuffList (buff, battleManager, showSWFInfo, showTutorial);
	}
	
	
	private void AddToBuffList (Buff buff, BattleManager battleManager, bool showSWFInfo, bool showTutorial = true)
	{
//		if(GameManager.PRINT_LOGS) Debug.Log(":::::: "+this.GetName() + " :::::: " + buff.id + " <<<--- APPLIED...");
		buff.Apply (this);
		_buffList.Add (buff);
		
		//GameManager.instance.scaleformCamera.generalSwf.AddPlayerDebuff(buff.id.ToLower());
		
		if (TutorialManager.instance.currentTutorial != TutorialManager.TutorialsAndCallback.None) {
			showSWFInfo = false;
			showTutorial = false;
		}
		
		if (buff.buffType == Buff.BuffType.kPositive) {
//			if(GameManager.PRINT_LOGS) Debug.Log("ON SELF ___________________ true");
			AddBuff (buff.id, true, showSWFInfo, showTutorial);
		} else {
//			if(GameManager.PRINT_LOGS) Debug.Log("ON SELF ___________________ false");
			AddBuff (buff.id, false, showSWFInfo, showTutorial);
		}
		
		if (buff.id.Equals ("SHIELD")) {
			battleManager.Shield ();
			return;
		} else if (buff.id.Equals ("HASTE")) {
			battleManager.HasteApplied ();
			return;
		} else if (buff.id.Equals ("LOCK")) {
			battleManager.SpellLock ();
			return;
		} else if (buff.id.Equals ("PRISON")) {
			battleManager.PrisonPet ();
			return;
		} else if (buff.id.Equals ("DISPEL")) {
			Debug.Log ("Calling Dispel.");
			battleManager.Dispel ();
			return;
		}
		
		propertyBuffList.Add (new KeyValuePair<string, Buff> (buff.targetProperty, buff));
	}
	
	public void TickBuffs (BattleManager battleManager = null)
	{
		List<Buff> buffDeleteList = new List<Buff> ();
		
		for (int i = 0; i < _buffList.Count; i++) {
			Buff buff = _buffList [i];
			CheckNegation (buff, battleManager);
			if (buff.duration <= 0) {
				buffDeleteList.Add (buff);	
			} else
				buff.Tick ();
			
		}
		RemoveBuffs (_buffList, buffDeleteList, battleManager);
	}
	
	public void RemoveBuffs (List<Buff> buffList, List<Buff> buffDeleteList, BattleManager battleManager)
	{
		List<KeyValuePair<string, Buff>> myBuffsToRemove = new List<KeyValuePair<string, Buff>> ();
		
		Buff buff = null;
		int count = buffDeleteList.Count;
		for (int i = 0; i<count; i++) {
			buff = buffDeleteList [i];
			
			buff.Expire ();

			if (battleManager != null && buff.id.Equals ("SHIELD"))
				battleManager.Unshield ();
			
			if (battleManager != null && buff.id.Equals ("LOCK"))
				battleManager.SpellUnlock ();
			
			if (battleManager != null && buff.id.Equals ("PRISON"))
				battleManager.UnprisonPet ();
			
			if (battleManager != null && buff.id.Equals ("HASTE"))
				battleManager.HasteRemoved ();
			
			KeyValuePair<string, Buff> pair;
			
			for (int j = 0; j< propertyBuffList.Count; j++) {
				pair = propertyBuffList [j];
				
				if (pair.Key.ToLower ().Equals (buff.id.ToLower ()))
					myBuffsToRemove.Add (pair);
			}
			
			
			RemoveBuff (buff.id);
			
			buffList.Remove (buff);

//			RemoveBuffsFromMyBuffsList(indexDeleteList);
			
		}
		
		KeyValuePair<string, Buff> aPair;
		for (int i = 0; i<myBuffsToRemove.Count; i++) {
			aPair = myBuffsToRemove [i];
			propertyBuffList.Remove (aPair);
		}
		
		buffDeleteList.Clear ();
		myBuffsToRemove.Clear ();
		
		//propertyBuffList.RemoveAll(myBuffsToRemove);
			
		//indicesToDelete.Clear();
	}
	
	protected abstract void RemoveBuff (string buffName);
	protected abstract void AddBuff (string buffName, bool onSelf, bool showSwfInfo = true, bool showTutorial = true);
	
	private void RemoveBuffsFromMyBuffsList (List<int> indicesToDelete)
	{
		for (int i = 0; i<indicesToDelete.Count; i++) {
			propertyBuffList.RemoveAt (indicesToDelete [i]);
		}
		indicesToDelete.Clear ();
	}
	
	#endregion
	
	private void CheckNegation (Buff buff, BattleManager battleManager)
	{
		if (battleManager != null) {
			if (buff.negatesBuffName != null) {
				List<Buff> removeBuffs = new List<Buff> ();
				
				Buff aBuff = null;
				int count = battleManager._battleState._enemy._buffList.Count;
				for (int i = 0; i<count; i++) {
					aBuff = battleManager._battleState._enemy._buffList [i];
					if (buff.negatesBuffName.ToLower ().Equals (aBuff.id.ToLower ())) {
						removeBuffs.Add (aBuff);
					}
				}
				
				battleManager._battleState._enemy.RemoveBuffs (battleManager._battleState._enemy._buffList, removeBuffs, battleManager);
			}
		}
	}
	
}
