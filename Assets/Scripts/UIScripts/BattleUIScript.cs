using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Holoville.HOTween;


public class BattleUIScript : MonoBehaviour {
	
	private BattleUIInterface _battleInterface;
	//Ingame Battle UI elements
	private int skullLevel;
//	public dfPanel BattleUI;
//	public dfLabel enemyHealthLabel;
//	public dfLabel playerHealthLabel;
//	public dfSlicedSprite enemyHealthBar;
//	public dfSlicedSprite playerHealthBar;
//	
//	public dfButton potionsButton;
//	public dfLabel numberOfPotions;
//
//	
//	public dfPanel bottomPanel;		//off/On during/after staff sequence 
//	
//	public dfSprite activeFire;
//	public dfSprite activeWater;
//	public dfSprite activeLightening;
//	public dfSprite activeEarth;
//
//	public dfButton runeGesture1;
//	public dfButton runeGesture2;
//	public dfButton runeGesture3;
//	public dfButton runeGesture4;
//	
//	public dfLabel findNexLabel;
//	public dfLabel bossFightLabel;
//	public dfLabel bossFightNameLabel;
//	public dfLabel castNowLabel;
//	public dfLabel chainStunLabel;
//	
//	
//	public dfPanel playerWardsPanel;
//	public dfScrollPanel playerDebufsPanel;
//	public dfScrollPanel enemyDebufsPanel;
//	
//	public dfButton spiritsButton;
//	public List<dfSprite> spiritsList;
//	
//	public dfButton staffButton;
//	public List<dfSprite> staffList;
//
//	private List<dfControl> wardArray;
	public GameObject wardPrefab;
	
	public GameObject ignitePrefab;
	public GameObject hastePrefab;
	public GameObject prisonPrefab;
	public GameObject regenPrefab;
	public GameObject lockPrefab;
	public GameObject drainPrefab;
	public GameObject leechwoodPrefab;
	public GameObject amplifyPrefab;
	public GameObject shieldPrefab;
//	private List<dfControl> playerDebuffArray;
//	private List<dfControl> enemyDebuffArray;
	
	
	
//	public dfTweenGroup tg;
//	public dfSprite dot1;
	/*************************************************/

	
	// Use this for initialization
//	void Start () {
//		skullLevel = 0;
//		wardArray = new List<dfControl>();
//		playerDebuffArray = new List<dfControl>();
//		TurnOnWard(0);
//		enemyDebuffArray = new List<dfControl>();
//		
////		if(_battleInterface != null)
////			_battleInterface.OnRegisterSWFCallback(this);
//
//		gameObject.GetComponent<dfPanel>().PerformLayout();
//		
//		ResetRuneSpells();
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	public void SetInterface(BattleUIInterface battleInterface)
	{
		_battleInterface = battleInterface;
	}
	
//	public void SetBattleUI(bool visible){
//		BattleUI.IsVisible = visible;
//	}
	
//	private void ResetRuneSpells()  //farhan
//	{
//		runeGesture1.IsVisible = false;
//		runeGesture2.IsVisible = false;
//		runeGesture3.IsVisible = false;
//		runeGesture4.IsVisible = false;
//	}
	
//	public void setRuneGestures(string name)
//	{
//		string[] splits = name.Split(',');
//		
//		Array.ForEach<string>(splits, (obj) => Debug.Log(obj));
//		
//		ResetRuneSpells();
//		
//		for(int i = 0;i < splits.Length;i++){
//			if(splits[i].Equals("Ignite")){
//				runeGesture1.IsVisible = true;
//			}else if(splits[i].Equals("Daze")){
//				runeGesture2.IsVisible = true;
//			}else if(splits[i].Equals("Drain")){
//				runeGesture3.IsVisible = true;				
//			}else if(splits[i].Equals("Leechseed")){
//				runeGesture4.IsVisible = true;				
//			}
//		}
//	}

//	public void StaffModeEnable(bool isTrue)
//	{
//		Debug.Log("StaffModeEnable > " + isTrue);
//		bottomPanel.IsVisible = !isTrue;		
//		//spiritsButton.IsVisible = !isTrue;
//		
//		if(!isTrue)
//		{
//			ResetRuneSpells();
//			BattleStaff(0);
//		}
//	}
//	
//	public void SpiritEnable(bool isTrue)
//	{
//		Debug.Log("::: SpiritEnable " + isTrue);
//		
//		spiritsButton.IsVisible = isTrue;
//		if(isTrue)
//			BattleSpirit(0);
//	}
//	
//	public void StaffEnable(bool isTrue)
//	{
//		Debug.Log("StaffEnable > " + isTrue);
//		
//		staffButton.IsVisible = isTrue;
////		if(isTrue)
////			BattleStaff(0);
//		staffButton.IsInteractive = isTrue;
//	}
	
//	public void GamePotionLevel(int potionBeltLevel,int myPotions, bool mySprite = false)
//	{
//		skullLevel = potionBeltLevel;
//		if(potionBeltLevel == 1 || myPotions == 0 )
//		{
//			potionsButton.IsVisible = false;
//			return;
//		}
//		else
//		{
//			potionsButton.IsVisible = true;
//			numberOfPotions.Text = myPotions.ToString();
//		}
//		
////		Debug.Log("SPIRIT ENABLE > " + mySprite);
////		SpiritEnable(mySprite);
//	}
	public void UsePotion()
	{
		// on potions button click
		// can implement this is on click of function
	}
//
//	public void SetPlayerFill(float current, float total) 
//	{
//		SetBarFill(true, current, total);
//		playerHealthLabel.Text = current.ToString();
////		StartCoroutine(UpdateHealthPoints(playerHealthLabel, current));
//	}
//
//	public void SetEnemyFill(float current, float total) 
//	{
//		SetBarFill(false, current, total);
//		enemyHealthLabel.Text = current.ToString();
////		StartCoroutine(UpdateHealthPoints(enemyHealthLabel, current));
//	}
//
//	public void SetBarFill(bool player, float current, float total)
//	{
//		float Magnitude = current/total;
//		float time = Magnitude * 3.0f;
//		float val = Magnitude;
//		if(player)
//		{
//			HOTween.To(playerHealthBar, time,new TweenParms().Prop("FillAmount", val));
//		}else
//		{
//			HOTween.To(enemyHealthBar, time,new TweenParms().Prop("FillAmount", val));
//		}
//	}
//
//	public void WardGlow(GameObject wardObj)
//	{
//		dfSprite sprite;
//		for(int i = 0;i < wardArray.Count; i++)
//		{
//			sprite = wardArray[i] as dfSprite;
//			//HOTween.To(sprite, 0.5f, new TweenParms().Prop("Opacity", 0.5f).Loops(-1, LoopType.Yoyo).Delay(i*0.1f));
//		}
//
//	}
//	
//	
//	public void TurnOnWard(int wardNumber)
//	{
//		dfSprite sprite;
//		for(int i = wardArray.Count - 1 ;i >= 0;i--){
//			sprite = wardArray[i] as dfSprite;
//			DestroyImmediate(sprite);
//		}
//		
//		for(int i = 0;i< wardNumber;i++){
//			dfControl instance = playerWardsPanel.AddPrefab(wardPrefab);
//			instance.name += i.ToString();
//			wardArray.Add (instance);
//			
//		}
//		WardGlow(null);
//	}
//	
//	public void PlayerElementStats(bool fire, bool water, bool lightening, bool earth){
//		activeFire.IsVisible = fire;
//		activeWater.IsVisible = water;
//		activeLightening.IsVisible = lightening;
//		activeEarth.IsVisible = earth;
//	}
//	
//	public void ElementBlink(string element){
//		if(element.Equals("Fire")){
//			StartCoroutine(Blink(activeFire));			
//		}else if(element.Equals("Water")){
//				StartCoroutine(Blink(activeWater));
//		}else if(element.Equals("Lightning")){
//			StartCoroutine(Blink(activeLightening));		
//		}else if(element.Equals("Earth")){
//			StartCoroutine(Blink(activeEarth));			
//		}
//	}
//	
//	IEnumerator Blink(dfSprite sprite){
//		for(int i = 0;i < 10;i++){
//			sprite.IsVisible = !sprite.IsVisible;
//			if(i%2 == 0)
//				yield return new WaitForSeconds(0.1f);
//			else
//				yield return new WaitForSeconds(0.3f);
//		}
//	}
//	
//	public void TurnOffWard()
//	{
//		dfSprite sprite = wardArray[wardArray.Count-1] as dfSprite;
//		sprite.IsVisible = false;
//		wardArray.RemoveAt(wardArray.Count-1);
//	}
//
//	public void BossFights(string BossName)
//	{
//		bossFightLabel.IsVisible = true;
//		bossFightNameLabel.Text = BossName;
//	}
//	
//	public void TurnOffBossFights()
//	{
//		bossFightLabel.IsVisible = false;
//	}
//	
//	public void CastNow()
//	{
//		castNowLabel.Text = "Cast now!";
//		castNowLabel.IsVisible = true;
//		castNowLabel.Opacity = 1.0f;
//		//HOTween.To(castNowLabel, 0.2f ,new TweenParms().Prop("Opacity", 1.0f));
//	}
//	
//	public void ChainStun()
//	{			
//		chainStunLabel.Text = "Chain Stun!";
//		chainStunLabel.IsVisible = true;
//		//HOTween.To(castNowLabel, 2.0f ,new TweenParms().Prop("Opacity", 0.0f));
//	}
//	
//	public void FindNex()
//	{
//		findNexLabel.IsVisible = true;
//		findNexLabel.Opacity = 0.0f;
//		//HOTween.To(findNexLabel, 0.2f ,new TweenParms().Prop("Opacity", 1.0f));	
//	}
//
//	public void TurnOffCastNow()
//	{
//		castNowLabel.IsVisible = false;
//	}
//	
//	public void TurnOffFindNex()
//	{
//		findNexLabel.IsVisible = false;
//	}
//
//	public void AddPlayerDebuff(string debuffName,bool self = true, bool tutorialMode = false)
//	{	
//		Debug.Log("AddPlayerDebuff > " + debuffName + "self > " + self + "tutorialMode > " + tutorialMode);
//		dfControl db = null;		
//		switch (debuffName)
//		{
//		    case "lockwood":
//				db = playerDebufsPanel.AddPrefab(leechwoodPrefab);
//				break;
//		    case "ignite":
//				db = playerDebufsPanel.AddPrefab(ignitePrefab);
//				break;
//		    case "shield":
//				db = playerDebufsPanel.AddPrefab(shieldPrefab);
//				break;
//		    case "regen":
//				db = playerDebufsPanel.AddPrefab(regenPrefab);
//				break;
//		    case "prison":
//				db = playerDebufsPanel.AddPrefab(prisonPrefab);
//				break;
//		    case "amplify":
//				db = playerDebufsPanel.AddPrefab(amplifyPrefab);
//				break;
//		    case "drain":
//				db = playerDebufsPanel.AddPrefab(drainPrefab);
//				break;
//		    case "lock":
//				db = playerDebufsPanel.AddPrefab(lockPrefab);
//				break;
//		    default:
//				return;
//		}
//		playerDebuffArray.Add(db);
//
//	}
//	
//	public void AddEnemyDebuff(string debuffName,bool self = true, bool tutorialMode = false)
//	{
//		Debug.Log("AddEnemyDebuff > " + debuffName + "self > " + self + "tutorialMode > " + tutorialMode);
//		
//		dfControl db = null;		
//		switch (debuffName)
//		{
//		    case "leech seed":
//				db = enemyDebufsPanel.AddPrefab(leechwoodPrefab);
//				break;
//		    case "ignite":
//				db = enemyDebufsPanel.AddPrefab(ignitePrefab);
//				break;
//		    case "shield":
//				db = enemyDebufsPanel.AddPrefab(shieldPrefab);
//				break;
//		    case "regen":
//				db = enemyDebufsPanel.AddPrefab(regenPrefab);
//				break;
//		    case "prison":
//				db = enemyDebufsPanel.AddPrefab(prisonPrefab);
//				break;
//		    case "amplify":
//				db = enemyDebufsPanel.AddPrefab(amplifyPrefab);
//				break;
//		    case "drain":
//				db = enemyDebufsPanel.AddPrefab(drainPrefab);
//				break;
//		    case "lock":
//				db = enemyDebufsPanel.AddPrefab(lockPrefab);
//				break;
//		    default:
//				break;
//		}
//		enemyDebuffArray.Add(db);	
//	}
//
//	public void RemovePlayerDebuff(string debuffName)
//	{
//		Debug.Log("RemovePlayerDebuff > " + debuffName);
//		
//		for(int i = 0;i < playerDebuffArray.Count;i++){
//			if(playerDebuffArray[i].name.Equals(debuffName+"Prefab(Clone)")){
//				dfControl control = playerDebuffArray[i];
//				DestroyImmediate(control);
//				playerDebuffArray.RemoveAt(i);
//				break;
//			}
//		}
//	}
//	
//	public void RemoveEnemyDebuff(string debuffName)
//	{
//		Debug.Log("RemoveEnemyDebuff > " + debuffName);
//		
//		for(int i = 0;i < enemyDebuffArray.Count;i++){
//			if(enemyDebuffArray[i].name.ToLower().Equals((debuffName+"Prefab(Clone)").ToLower())){
//				dfControl control = enemyDebuffArray[i];
//				Destroy(control);
//				enemyDebuffArray.RemoveAt(i);
//				break;
//			}
//		}		
//	}
//
//	IEnumerator UpdateHealthPoints(dfLabel label, float value)
//	{
//		float time = 2f;
//		int current = System.Convert.ToInt32(label.Text);
//		float delay = time / Mathf.Abs(current - value); 
//		if (current < value)
//		{
//			for (int i = current; i <= value; i++)
//			{
//				playerHealthLabel.Text = i.ToString();
//				yield return new WaitForSeconds(delay);
//			}
//		}
//		else if (current > value)
//		{
//			for (int i = current; i >= value; i--)
//			{
//				playerHealthLabel.Text = i.ToString();
//				yield return new WaitForSeconds(delay);	
//			}
//		}
//	}
	
//	public void BattleSpirit(int charge)
//	{
//		Debug.Log("BattleSpirit - charge > " + charge);
//		SpiritCharge(charge, "spirit");
//	}
//
//	public void BattleStaff(int charge)
//	{
//		Debug.Log("BattleStaff - charge > " + charge);
//		SpiritCharge(charge, "staff");		
//	}
//
//	private void SpiritCharge(float charge, string spirit)
//	{
//		List<dfSprite> tempList = null;
//		if(spirit.Equals("spirit")){
//			tempList = spiritsList;
//			if(charge >= 6){
//				spiritsButton.IsInteractive = true;
//			}
//		}else if(spirit.Equals("staff")){
//			tempList = staffList;
//			if(charge >= 6){
//				staffButton.IsInteractive = true;
//			}
//		}
//		for(int i = 0;i < tempList.Count; i++){
//			if(i >= charge){
//				tempList[i].IsVisible = false;
//			}else{
//				//HOTween.To(tempList[i], 1.0f,new TweenParms().Prop("IsVisible", true).Delay(0.2f));
//				tempList[i].IsVisible = true;
//			}
//		}
//	}	
//		
	public void RingNArrow(GameObject Circle, float angle)
	{
		
	}
	
	public void RingNArrowOff(GameObject Circle)
	{
		
	}
	
	public void RingAnimation(GameObject obje)
	{
		
	}
	
	
	public void SpiritFightTutorialStart()
	{
		
	}
	
	public void SpiritFightTutorial2()
	{
		
	}
	
	public void SpiritFightTutorialEnd()
	{
		
	}

	public void SpiritFightTutorialEnding()
	{
		
	}
	
	public void SetLanguage(string strInput,string lan)
	{
		
	}
	
//	public void OnClick( dfControl control, dfMouseEventArgs mouseEvent ){
//		if(mouseEvent.Source.name.Equals("StaffButton"))
//		{
//			_battleInterface.StaffCharged();
//		}
//		else if(mouseEvent.Source.name.Equals("PotionsButton"))
//		{
//			_battleInterface.usePotionInBattle(true);
//		}
//		else if(mouseEvent.Source.name.Equals("DebuffRemove")){
//			RemovePlayerDebuff("Ignite");
//		}else if(mouseEvent.Source.name.Equals("DebuffLockwood")){
//			AddPlayerDebuff("lockwood",true,true);			
//		}
//	
//	}
	
	public void updateLoadingBar(float val)
	{
		
	}
	
	public void SetVisible(bool yesNo)
	{
//		GetComponent<dfPanel>().IsVisible = yesNo;
	}
}
