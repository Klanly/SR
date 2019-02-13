using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using Holoville.HOTween;

[Serializable]
[RequireComponent( typeof( dfPanel ) )]
public class CarouselScrollEvents : MonoBehaviour 
{
	
	private bool isDraging;
	public List<dfSprite> TempRings;
	private dfSprite LastRing;
	private dfSprite CurrentRing;
	private dfSprite NextRing;
	public List<GameObject> RingsPositions;
	public bool isMouseDown;
	
	public bool CanClick = true;
	
	[SerializeField]
	public float time = 0.33f;
	
	public List<Vector2> tempPositions;
	private Vector2 MousePosition;
	
	public float MoveSpeed = 0;
	
	public dfControl host;

	
	public void Start()
	{
	
		LastRing = TempRings[TempRings.Count-1];
		CurrentRing = TempRings[0];
		NextRing = TempRings[1];
		CurrentRing.SpriteName = TempRings[0].SpriteName;
		NextRing.SpriteName = TempRings[1].SpriteName;
		LastRing.SpriteName = TempRings[TempRings.Count-1].SpriteName;
	}
	
	public void OnMouseDown( dfControl control, dfMouseEventArgs mouseEvent )
	{
		isDraging = true;
		isMouseDown = true;
		
		Debug.Log("IsClicked");
		MousePosition = Input.mousePosition;
		MoveSpeed = 0;
	}

	public void OnMouseUp( dfControl control, dfMouseEventArgs mouseEvent )
	{
		isDraging = false;
		Debug.Log("UnClicked");
	}
	
	public void OnDragStart( dfControl control, dfDragEventArgs args )
	{
		if( args.Used )
		{
			isMouseDown = false;
		}
	}

	public void OnMouseMove( dfControl control, dfMouseEventArgs mouseEvent )
	{

		if(isDraging)
		{
			
			MoveSpeed = Math.Abs(Input.mousePosition.y - MousePosition.y)/10;
			if(Input.mousePosition.y > MousePosition.y)
				MoveUpCarousel();
			if(Input.mousePosition.y < MousePosition.y)
				MoveDownCarousel();
				
			
	
			Debug.Log("Is_Moving");
			isDraging = false;
		}
	}
	
	public void MoveDownCarousel()
	{
		
		for(int i=0;i<RingsPositions.Count;i++)
		{
			if(i==TempRings.Count-1)
			{
				
				HOTween.To(RingsPositions[i].transform, MoveSpeed, new TweenParms().Prop("position",RingsPositions[0].transform.position).AutoKill(true));
				
				//HOTween.To(RingsPositions[i].transform,MoveSpeed,"position",RingsPositions[0].transform.position);
			//	RingsPositions[i].transform.DOMove(RingsPositions[0].transform.position,MoveSpeed,false).WaitForCompletion().OnComplete(CanClickFun());
			}
			else
			{
				HOTween.To(RingsPositions[i].transform, MoveSpeed, new TweenParms().Prop("position",RingsPositions[i+1].transform.position).AutoKill(true));
				
				//HOTween.To(RingsPositions[i].transform,MoveSpeed,"position",RingsPositions[i+1].transform.position);
			
			//	RingsPositions[i].transform.DOMove(RingsPositions[i+1].transform.position,MoveSpeed,false).WaitForCompletion();
			}
		}
		
		
	
	}
	public void MoveUpCarousel()
	{
		
		
		for(int i=0;i<TempRings.Count;i++)
		{
			if(i==0)
			{
				HOTween.To(RingsPositions[i].transform, MoveSpeed, new TweenParms().Prop("position",RingsPositions[TempRings.Count-1].transform.position).AutoKill(true));
				
			
				//RingsPositions[i].transform.DOMove(RingsPositions[TempRings.Count-1].transform.position,MoveSpeed,false).WaitForCompletion();
			}
			else
			{
				HOTween.To(RingsPositions[i].transform, MoveSpeed, new TweenParms().Prop("position",RingsPositions[i-1].transform.position).AutoKill(true));
				
				//RingsPositions[i].transform.DOMove(RingsPositions[i-1].transform.position,MoveSpeed,false).WaitForCompletion();
			}
		}
		
	}
	
	void CanClickFun()
	{
		CanClick = true;
	}
}
