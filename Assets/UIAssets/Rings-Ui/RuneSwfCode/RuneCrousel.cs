using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;

public class RuneCrousel : MonoBehaviour {
	
	public dfLabel RuneName;
	public dfLabel RuneLevel;
	public dfLabel RuneDescription;
	public dfSprite BuffIcon;
	
	 
	public dfSprite newRune;
	public dfSprite suggestedRune;
	
	public dfPanel BuyButton;
	public dfPanel SellButton;
	
	public dfPanel RuneCarousel;
	
	private Vector2 MousePosition;

	public float MoveSpeed = 0;
	public float time = 1;
	
	public dfButton BuySellButtonReferance;
	
	private int selectedIndex;
	
	public void LateUpdate ()
	{
		Refresh();
	}
	
	
	public void Refresh()
	{
		updateRuneStatsArea(RuneCarousel.GetComponent<CarouselRuneCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragRune>()._runeModel);
		gameObject.transform.parent.GetComponent<RuneUi>().CurrentRune = RuneCarousel.GetComponent<CarouselRuneCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragRune>();
	}
	
	public void ButtonPanel(float cost,bool dCost,bool buy)
	{
		if(buy)
		{
			BuyButton.IsVisible = true;
			SellButton.IsVisible = false;
			BuySellButtonReferance.GetComponent<BuySellRuneButton>().buy = true;
			
			BuyButton.gameObject.transform.FindChild("Cost").GetComponent<dfLabel>().Text = cost.ToString();
			if(!dCost)
				BuyButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 115";
			else if(dCost)
				BuyButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 112";
			
		}
		else
		{
			BuyButton.IsVisible = false;
			SellButton.IsVisible = true;
			
			BuySellButtonReferance.GetComponent<BuySellRuneButton>().buy = false;
			
			SellButton.gameObject.transform.FindChild("Cost").GetComponent<dfLabel>().Text = cost.ToString();
			if(!dCost)
				SellButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 115";
			else if(dCost)
				SellButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 112";
		}
	}
	
	
	public void GoToRune(DragRune rune)
	{
		if(rune._runeModel == null || string.IsNullOrEmpty(rune._runeModel.uid))
			return;
		
		for(int i = 0; i < RuneCarousel.transform.childCount;i++)
		{
			DragRune currentRune = RuneCarousel.transform.GetChild(i).GetComponent<DragRune>();
			if(currentRune._runeModel.uid == rune._runeModel.uid)
			{
				RuneCarousel.GetComponent<CarouselRuneCoverFlow>().selectedIndex = int.Parse(currentRune.gameObject.name);
				Debug.Log("SELECTED INDEX = " + currentRune.gameObject.name);
				
				return;
			}
		}
	}
	
	public void Unhighlight(DragRune rune)
	{
		for(int i = 0; i < RuneCarousel.transform.childCount;i++)
		{
			DragRune currentRune = RuneCarousel.transform.GetChild(i).GetComponent<DragRune>();
			if(currentRune._runeModel.uid == rune._runeModel.uid)
			{
				currentRune.isEquipped = false;
				(currentRune.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = false;
				Debug.Log("SELECTED INDEX = " + currentRune.gameObject.name);
				return;
			}
		}
	}
	
	public void updateRuneStatsArea(ItemRune r)
	{
		RuneName.Text = r.ItemName();
		
		if(r.market && r.dCost>0)
			ButtonPanel(r.dCost,true,true);
		else if(r.market && r.gCost>0)
			ButtonPanel(r.gCost,false,true);
		
		else if(!r.market && r.sellCost>0)
			ButtonPanel(r.sellCost,true,false);
		
		suggestedRune.IsVisible = r.isSuggested;
		newRune.IsVisible = r.isNew;
//		RuneDescription.Text = r.description;
		
		
		RuneDescription.Text = new Buff(r.buff.id, r.skullLevel).description;
		/*
		string description = "";
		if(buff.id.ToLower().Equals("greed"))
		{
			description = string.Format("Gives {0} extra gold every round", buff.modifierValue);
		}
		else if(buff.id.ToLower().Equals("ignite"))
		{
			description = string.Format("Inflicts {0} damage per round for {1} rounds ", buff.modifierValue, buff.totalDuration);
		}
		else if(buff.id.ToLower().Equals("drain"))
		{
			description = string.Format("Inflicts {0} damage per round for {1} rounds ", buff.modifierValue, buff.totalDuration);
		}
		*/

		BuffIcon.SpriteName = r.buff.id;
				
	}
}
