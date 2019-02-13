using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIScripts : MonoBehaviour {
	
	/*************************************************/
	//Ingame Battle UI elements
	public dfPanel BattleUI;
	public dfLabel enemyHealthLabel;
	public dfLabel playerHealthLabel;
	public dfSlider enemyHealthBar;
	public dfSlider playerHealthBar;
	
	public dfSprite activeFire;
	public dfSprite activeWater;
	public dfSprite activeLightening;
	public dfSprite activeEarth;

	public dfSprite rune1;
	public dfSprite rune2;
	public dfSprite rune3;
	
	public dfTweenGroup tg;
	public dfSprite dot1;
	/*************************************************/

	
	
	
	// Use this for initialization
	void Start () {
//		DOTween.Init(autoKillMode, useSafeMode, logBehaviour);
		UpdateBar(30);
		setBarFill(true,32,40);
		
//		rune1.SpriteName = "image 93";
//		DOTween.To ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBattleUI(bool visible){
		BattleUI.IsVisible = visible;
	}
	
	public void PlayerElementStats(bool fire, bool water, bool lightening, bool earth){
		activeFire.IsVisible = fire;
		activeWater.IsVisible = water;
		activeLightening.IsVisible = lightening;
		activeEarth.IsVisible = earth;
	}
	
	public void setPlayerFill(float current, float total) {
		setBarFill(true, current, total);
		StartCoroutine(UpdateHealthPoints(playerHealthLabel, current));
	}

	public void setEnemyFill(float current, float total) {
		setBarFill(false, current, total);
		StartCoroutine(UpdateHealthPoints(enemyHealthLabel, current));
	}

	public void setBarFill(bool player, float current, float total){
		dfSlider temp;
		float Magnitude = current/total;
		float time = Magnitude * 4.0f;
		float val = Magnitude * 100.0f;
		if(player){
			temp = playerHealthBar;
		}else{
			temp = enemyHealthBar;
		}
		
		DOTween.To(()=> playerHealthBar.Value, x=> playerHealthBar.Value = x, val, time);
//		temp.Value = val;

		}
	
	
	void UpdateBar(float value){
		ElementBlink("Fire");
		StartCoroutine(UpdateHealthPoints(playerHealthLabel, value));
		
		//		HOTween.To(playerHealthBar,3.0f,new TweenParms().Prop("Value",value).Ease(EaseType.EaseOutCubic));
//		HOTween.To(playerHealthLabel,3.0f,new TweenParms().Prop("Text",value).Ease(EaseType.EaseOutCubic));
//		playerHealthLabel.te
	}
	
	IEnumerator UpdateHealthPoints(dfLabel label, float value)
	{
		float time = 4.0f;
		int current = System.Convert.ToInt32(label.Text);
		float delay = time / Mathf.Abs(current - value); 
		if (current < value)
		{
			for (int i = current; i <= value; i++)
			{
				playerHealthLabel.Text = i.ToString();
				yield return new WaitForSeconds(delay);
			}
		}
		else if (current > value)
		{
			for (int i = current; i >= value; i--)
			{
				playerHealthLabel.Text = i.ToString();
				yield return new WaitForSeconds(delay);	
			}
		}
	}

	void ElementBlink(string element){
		if(element.Equals("Fire")){
			StartCoroutine(Blink(activeFire));			
		}else if(element.Equals("Water")){
				StartCoroutine(Blink(activeWater));
		}else if(element.Equals("Lightning")){
			StartCoroutine(Blink(activeLightening));		
		}else if(element.Equals("Earth")){
			StartCoroutine(Blink(activeEarth));			
		}
	}
	
	IEnumerator Blink(dfSprite sprite){
		for(int i = 0;i < 10;i++){
			sprite.IsVisible = !sprite.IsVisible;
			if(i%2 == 0)
				yield return new WaitForSeconds(0.1f);
			else
				yield return new WaitForSeconds(0.3f);
		}
	}
}
