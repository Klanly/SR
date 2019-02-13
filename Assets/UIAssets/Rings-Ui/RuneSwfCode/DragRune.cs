using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using AnimationOrTween;
using UnityEngine;
using System.Collections;
using InventorySystem;
 
[AddComponentMenu( "Drag Rune" )]
public class DragRune : MonoBehaviour 
{
	public InventorySystem.ItemRune _runeModel;
	
	public static Action<DragRune> _tapListener;
	public static Action<DragRune> _runeSwapOutListener;
	
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
	private dfAnimatedFloat RuneDropAnimation;
	private static dfPanel _container;
	private static dfSprite _sprite;
	
	public bool CanDistroy;
	public bool isEquipped = false;
	public bool isTransmute;
	

	public void OnEnable()
	{
		Refresh();
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
		GoToCarouselRune();
		
		if( string.IsNullOrEmpty( ItemName ) )
			return;
	
		Refresh();
	}
	
	private void GoToCarouselRune()
	{
		if(transform.parent.name == "Rune-Carousel")
			return;

		if(_runeModel == null || string.IsNullOrEmpty(_runeModel.uid))
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
		if(transform.parent.name == "Rune-Panel")
			return;
		
		if(isDragAble && !isEquipped && !gameObject.GetComponent<DragRune>()._runeModel.market)
		{
			args.Data = this;
			args.State = dfDragDropState.Dragging;
			args.Use();

			DragRuneCursor.Show( this, args.Position );
			
			Debug.Log("Drag start");
		}
	}

	public void OnDragEnd( dfControl source, dfDragEventArgs args )
	{	
		Debug.Log("Drag end");
		
		DragRuneCursor.Hide();
		
		if( args.State == dfDragDropState.Dropped )
		{
			isDragAble = false;
			Refresh();
		}
	}

	public void OnDragDrop( dfControl source, dfDragEventArgs args )
	{
		if(transform.parent.name == "Rune-Carousel")
			return;
		
		if(args.Data is DragRune )
		{
			DragRune draggedSelectedRune = (DragRune)args.Data;
			ItemRune selectedRune = draggedSelectedRune.gameObject.GetComponent<DragRune>()._runeModel;
			(draggedSelectedRune.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = true;
			
			ItemRune thisRune = gameObject.GetComponent<DragRune>()._runeModel;
//			
//			if(_runeModel == null || string.IsNullOrEmpty(_runeModel.uid))
//			{
//				UIManager.instance.generalSwf.runeUI.swapTheRune("empty", draggedSelectedRune._runeModel.ItemName() + "|" + draggedSelectedRune._runeModel.skullLevel);
//			}
//			else
//			{
//				UIManager.instance.generalSwf.runeUI.swapTheRune(_runeModel.ItemName() + "|" + _runeModel.skullLevel, draggedSelectedRune._runeModel.ItemName() + "|" + draggedSelectedRune._runeModel.skullLevel);
//				if(_runeSwapOutListener != null)
//					_runeSwapOutListener(gameObject.GetComponent<DragRune>());
//			}
//			
			draggedSelectedRune.isEquipped = true;
		
			this.ItemName = draggedSelectedRune.ItemName;
			this.Icon = draggedSelectedRune.Icon;
			this._runeModel = draggedSelectedRune._runeModel;
			
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
