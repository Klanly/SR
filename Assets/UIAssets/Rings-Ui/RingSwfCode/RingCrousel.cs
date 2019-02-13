using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System;
using InventorySystem;
using System.Collections.Generic;

public class RingCrousel : MonoBehaviour {
	
	public dfLabel RingName;
	public dfLabel RingLevel;
	
	public dfSprite damage_wards_health;
	public dfLabel Damage_wards_amount;
	
	public dfSprite water;
	public dfSprite fire;
	public dfSprite lightning;
	public dfSprite earth;
	 
	public dfSprite newRing;
	public dfSprite suggestedRing;
	
	public dfPanel BuyButton;
	public dfPanel SellButton;
	
	public dfPanel RingCarousel;
	
	private Vector2 MousePosition;

	public float MoveSpeed = 0;
	public float time = 1;
	
	public dfButton BuySellButtonReferance;
	
	
	private int selectedIndex;
	
	public void OnEnable()
	{
		RingCarousel.GetComponent<CarouselCoverFlow>().SelectedIndexChanged += IndexChange;
			
	}
	
	private void IndexChange(object sender, int newInd) 
	{
		Debug.Log("Index changed!");
		if(_carouselChange != null)
			_carouselChange();
	}
		
	public void OnDisable()
	{
		RingCarousel.GetComponent<CarouselCoverFlow>().SelectedIndexChanged -= IndexChange;
			
	}
	
	private bool _gotoNew;
	private int _gotoIndex = int.MaxValue;
	
	float gotoTimer = float.MinValue;
	
	public void LateUpdate ()
	{
		Refresh();
		if(_gotoNew)
		{
			gotoTimer = Time.time;
			_gotoNew = false;
			Debug.Log("_gotoNew = false");
			
		}
		if(_gotoIndex != int.MaxValue)
		{
			ExecuteGoToRingNumber(_gotoIndex);
		}
		
		if(gotoTimer != float.MinValue)
		{
			if(Time.time - gotoTimer > 1)
			{
				ExecuteGoToNew();
				gotoTimer = float.MinValue;
			}
		}
	}
	
	public static event Action _carouselChange;
	
	public void Refresh()
	{
		updateRingStatsArea(RingCarousel.GetComponent<CarouselCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragRing>()._ringModel);
		gameObject.transform.parent.GetComponent<RingUi>().CurrentRing = RingCarousel.GetComponent<CarouselCoverFlow>().ObjectAtSelectedIndex().GetComponent<DragRing>();
	}
	
	public void GoToNewRing()
	{
		_gotoNew = true;
		Debug.Log("_gotoNew = true");
	}
	
	private void ExecuteGoToNew()
	{
		Debug.Log("ExecuteGoToNew");

		Refresh();
		Debug.Log("RingCarousel.transform.childCount > " + RingCarousel.transform.childCount);
		for(int i = 0; i < RingCarousel.transform.childCount;i++)
		{
			DragRing currentRing = RingCarousel.transform.GetChild(i).GetComponent<DragRing>();
			if(currentRing._ringModel.isNew)
			{
				RingCarousel.GetComponent<CarouselCoverFlow>().selectedIndex = int.Parse(currentRing.gameObject.name);
				Debug.Log("NEW RING INDEX = " + currentRing.gameObject.name);
				return;
			}
		}
		Refresh();

	}
	
	public void GoToRingNumber(int number)
	{
		_gotoIndex = number;
	}
	
	private void ExecuteGoToRingNumber(int number)
	{
		Debug.Log("ExecuteGoToRingNumber > " + number);
		_gotoIndex = int.MaxValue;
		
		this.PerformActionWithDelay(0.5f, () => {
			CarouselCoverFlow coverFlow = RingCarousel.GetComponent<CarouselCoverFlow>();
			coverFlow.selectedIndex = number;	
			Refresh();
		});
	}
	
	public void GoToRing(DragRing ring)
	{
		for(int i = 0; i < RingCarousel.transform.childCount;i++)
		{
			DragRing currentRing = RingCarousel.transform.GetChild(i).GetComponent<DragRing>();
			if(currentRing._ringModel.uid == ring._ringModel.uid)
			{
				if(_carouselChange != null)
					_carouselChange();
				RingCarousel.GetComponent<CarouselCoverFlow>().selectedIndex = int.Parse(currentRing.gameObject.name);
				Debug.Log("SELECTED INDEX = " + currentRing.gameObject.name);
				
				return;
			}
		}
	}
		
	public void UnhighlightRing(DragRing ring)
	{
		for(int i = 0; i < RingCarousel.transform.childCount;i++)
		{
			DragRing currentRing = RingCarousel.transform.GetChild(i).GetComponent<DragRing>();
			if(currentRing._ringModel.uid == ring._ringModel.uid)
			{
				currentRing.isEquipped = false;
				(currentRing.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = false;
				Debug.Log("SELECTED INDEX = " + currentRing.gameObject.name);
				
				return;
			}
		}
	}
	
	
	public void ButtonPanel(float cost,bool dCost,bool buy)
	{
		if(buy)
		{
			BuyButton.IsVisible = true;
			SellButton.IsVisible = false;
			BuySellButtonReferance.GetComponent<BuySellButton>().buy = true;
			
			BuyButton.gameObject.transform.FindChild("Cost").GetComponent<dfLabel>().Text = cost.ToString();
			if(!dCost)
				BuyButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 115";
			else
				BuyButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 112";
			
		}
		else
		{
			BuyButton.IsVisible = false;
			SellButton.IsVisible = true;
			
			BuySellButtonReferance.GetComponent<BuySellButton>().buy = false;
			
			SellButton.gameObject.transform.FindChild("Cost").GetComponent<dfLabel>().Text = cost.ToString();
			if(!dCost)
				SellButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 115";
			else if(dCost)
				SellButton.gameObject.transform.FindChild("Dust").GetComponent<dfSprite>().SpriteName = "image 112";
		}
	}
	
	
	
	public void updateRingStatsArea(ItemRing r)
	{
		RingName.Text = r.ItemName();

		water.IsVisible = false;
		fire.IsVisible = false;
		earth.IsVisible = false;
		lightning.IsVisible = false;
		
		Damage_wards_amount.Text = "x 0";
	
		if(r.damage>0)
		{
			damage_wards_health.IsVisible = true;
			damage_wards_health.SpriteName = "image 80";
			Damage_wards_amount.Text = "x" + r.damage;
		}
		else if (r.wards>0)
		{	
			damage_wards_health.IsVisible = true;
			damage_wards_health.SpriteName = "image 75";
			Damage_wards_amount.Text = "x" + r.wards;
		}
		else if (r.life>0)
		{	
			damage_wards_health.IsVisible = true;
			damage_wards_health.SpriteName = "image 71";
			Damage_wards_amount.Text = "x" + r.life;
		}
		
		if(r.market && r.dCost>0)
			ButtonPanel(r.dCost,true,true);
		else if(r.market && r.gCost>0)
			ButtonPanel(r.gCost,false,true);
		
		else if(!r.market && r.sellCost>0)
			ButtonPanel(r.sellCost,true,false);
		
		suggestedRing.IsVisible = r.isSuggested;
		newRing.IsVisible = r.isNew;
		
		water.IsVisible = r.water>0;
		earth.IsVisible = r.earth>0;
		fire.IsVisible = r.fire>0;
		lightning.IsVisible = r.lightning>0;
		
	}
}
