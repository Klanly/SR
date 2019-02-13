using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using AnimationOrTween;
using UnityEngine;
using System.Collections;
using InventorySystem;
 
[AddComponentMenu( "Drag Transmute" )]
public class DragTransmute : MonoBehaviour 
{
	public static Action<DragTransmute> _itemSwapOutListener;
	
	public InventorySystem.ItemRune _transmuteModel;
	public InventoryItem _model;
	
//	if(_model as ItemRing == null)
//	{
//		// its a rune!
//	}
	
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
	private dfAnimatedFloat TransmuteDropAnimation;
	private static dfPanel _container;
	private static dfSprite _sprite;
	
	public bool CanDistroy;
	public bool isEquipped = false;
	public bool isRing;
	

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
		if( string.IsNullOrEmpty( ItemName ) )
			return;
	
		Refresh();
	}
	
	public void OnMouseDown( dfControl control, dfMouseEventArgs args )
	{
		StartCoroutine(WaitAndDo());
	}

	public void OnDragStart( dfControl source, dfDragEventArgs args )
	{
		if(transform.parent.name == "Frame")
			return;
		
		if(TransmuteUi.isTransmuting)
		{
			Debug.Log("Transmutation in progress... Drag not allowed");
			return;
		}
		
//		if(isDragAble && !isEquipped && _model.market)
		if(isDragAble && !isEquipped)
		{
			args.Data = this;
			args.State = dfDragDropState.Dragging;
			args.Use();

			DragTransmuteCursor.Show( this, args.Position );
			
			Debug.Log("Drag start");
		}
	}

	public void OnDragEnd( dfControl source, dfDragEventArgs args )
	{	
		Debug.Log("Drag end");
		
		DragTransmuteCursor.Hide();
		
		if( args.State == dfDragDropState.Dropped )
		{
			isDragAble = false;
			Refresh();
		}
	}

	public void OnDragDrop( dfControl source, dfDragEventArgs args )
	{
//		if(transform.parent.name == "Ring-Carousel")
//			return;
//		if(TransmuteUi.isTransmuting)
//		{
//			Debug.Log("Transmutation in progress... Drag not allowed");
//			return;
//		}
//		
//		if(args.Data is DragTransmute )
//		{
//			DragTransmute draggedSelectedTransmute = (DragTransmute)args.Data;
//			InventoryItem selectedTransmute = draggedSelectedTransmute._model;
//			
//			UIManager.instance.generalSwf.transmuteUI.removeTransmuteItems(draggedSelectedTransmute.isRing);
//			
//			(draggedSelectedTransmute.GetComponent<dfControl>().Find("Equipped") as dfSprite).IsVisible = true;
//			
//			InventoryItem thisTransmute = _model;
//			
//			if(_model == null || string.IsNullOrEmpty(_model.ItemName()))
//			{
//				UIManager.instance.generalSwf.transmuteUI.swapTheTransmute(null, draggedSelectedTransmute);
////				UIManager.instance.generalSwf.runeUI.swapTheRune("empty", draggedSelectedRune._runeModel.ItemName() + "|" + draggedSelectedRune._runeModel.skullLevel);
//				
//			}
//			else
//			{
//				//UIManager.instance.generalSwf.transmuteUI.swapTheTransmute(_model.ItemName() + "|" + _model.skullLevel, draggedSelectedTransmute._model.ItemName() + "|" + draggedSelectedTransmute._model.skullLevel);
//				UIManager.instance.generalSwf.transmuteUI.swapTheTransmute(this, draggedSelectedTransmute);
//				if(_itemSwapOutListener != null)
//					_itemSwapOutListener(gameObject.GetComponent<DragTransmute>());
//			}
//		
//			this.ItemName = draggedSelectedTransmute.ItemName;
//			this.Icon = draggedSelectedTransmute.Icon;
//			this._model = draggedSelectedTransmute._model;
//			
//			draggedSelectedTransmute.isEquipped = true;
//			isDragAble = false;
//			
//			args.State = dfDragDropState.Dropped;
//			args.Use();
//		}
//		
//		Refresh();
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
