using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimationOrTween;
using UnityEngine;
using System.Collections;
using InventorySystem;
 
[AddComponentMenu( "Drag Ring" )]
public class DragRing : MonoBehaviour 
{
	
	public static Action<DragRing> _tapListener;
	public static Action<DragRing> _ringSwapOutListener;
	
	public InventorySystem.ItemRing _ringModel;
	
	public string ItemName;
	private string _Icon;
	public string Icon
	{
		set
		{
			if(_Icon != value)
			{
				_Icon = value;
				Refresh();
			}
			else
			_Icon = value;
		}
		get
		{
			return _Icon;
		}
	}
	
	public bool isDragAble;
	private dfAnimatedFloat RingDropAnimation;
	private static dfPanel _container;
	private static dfSprite _sprite;
	
	public bool CanDistroy;
	public bool isEquipped = false;
	public bool isTransmute;
	

	public void OnEnable()
	{
		Refresh();
		
		RingCrousel._carouselChange += OnIndexChange;
	}
	
	public void OnDisable()
	{
		RingCrousel._carouselChange -= OnIndexChange;
	}
	
	private void OnIndexChange()
	{
		dfSlicedSprite spr = GetComponent<dfPanel>().Find("glow") as dfSlicedSprite;
		if(spr != null)
			spr.IsVisible = false;
	}
	
	public void Start()
	{
		Refresh();
	}

	public void OnDoubleClick( dfControl source, dfMouseEventArgs args )
	{
		OnClick( source, args );
	}

	public void OnClick( dfControl source, dfMouseEventArgs args )
	{
		GoToCarouselRing();
		
		if( string.IsNullOrEmpty( ItemName ) )
			return;
	
		Refresh();
	}
	
	private void GoToCarouselRing()
	{
		if(transform.parent.name == "Ring-Carousel")
			return;
		
		if(_ringModel == null || string.IsNullOrEmpty(_ringModel.uid))
			return;
		
		if(_tapListener != null)
			_tapListener(this);
		
		(GetComponent<dfPanel>().Find("glow") as dfSprite).IsVisible = true;
		
		Refresh();
	}
	
	public void OnMouseDown( dfControl control, dfMouseEventArgs args )
	{
		StartCoroutine(WaitAndDo());
	}

	public void OnDragStart( dfControl source, dfDragEventArgs args )
	{
		if(transform.parent.name == "Left-Panel")
			return;
		if(isDragAble && !isEquipped && !gameObject.GetComponent<DragRing>()._ringModel.market)
		{
			args.Data = this;
			args.State = dfDragDropState.Dragging;
			args.Use();

			DragCursor.Show( this, args.Position );
			
			Debug.Log("Drag start");
		}
	}

	public void OnDragEnd( dfControl source, dfDragEventArgs args )
	{	
		Debug.Log("Drag end");
		
		DragCursor.Hide();
		
		if( args.State == dfDragDropState.Dropped )
		{
			isDragAble = false;
			Refresh();
		}
	}

	public void OnDragDrop( dfControl source, dfDragEventArgs args )
	{
		if(transform.parent.name == "Ring-Carousel")
			return;
		
		if(args.Data is DragRing )
		{
			DragRing draggedSelectedRing = (DragRing)args.Data;
			
			ItemRing selectedRing = draggedSelectedRing._ringModel;
			(draggedSelectedRing.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = true;
			
			ItemRing thisRing = gameObject.GetComponent<DragRing>()._ringModel;
			
			if(_ringModel == null || string.IsNullOrEmpty(_ringModel.uid))
			{
				UIManager.instance.generalSwf.ringUI.swapTheRings("empty", draggedSelectedRing._ringModel.ItemName() + "|" + draggedSelectedRing._ringModel.skullLevel);
				
			}
			else
			{
				UIManager.instance.generalSwf.ringUI.swapTheRings(_ringModel.ItemName() + "|" + _ringModel.skullLevel, draggedSelectedRing._ringModel.ItemName() + "|" + draggedSelectedRing._ringModel.skullLevel);
				if(_ringSwapOutListener != null)
					_ringSwapOutListener(gameObject.GetComponent<DragRing>());
			}
		
			draggedSelectedRing.isEquipped = true;
			
			this.ItemName = draggedSelectedRing.ItemName;
			this.Icon = draggedSelectedRing.Icon;
			this._ringModel = draggedSelectedRing._ringModel;
			
			draggedSelectedRing.isDragAble = true;
			isDragAble = false;
			
			args.State = dfDragDropState.Dropped;
			args.Use();
		}
		
		Refresh();
	}

	private void Refresh()
	{
		_container = GetComponent<dfPanel>();
		_sprite = _container.Find("Icon") as dfSprite;
		_sprite.SpriteName = this.Icon;
	}
	
	IEnumerator WaitAndDo()
	{
		yield return new WaitForSeconds(1f);
		isDragAble= true;
	}
}
