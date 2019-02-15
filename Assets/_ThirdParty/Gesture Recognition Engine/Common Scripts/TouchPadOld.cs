////-----------------------------------------------------------------
////  Copyright 2011 Layne Bryant   Midnight Ware
////
////  All rights reserved
////-----------------------------------------------------------------
//
//using UnityEngine;
//
///// <summary>
///// Handles input to be used in gesture recognition. This class captures mouse movement and events, but you
///// could create a class that handles finger events just as easily. You could also use third party GUI tools
///// to capture the input as well, but for legal issues, this simplified class is all that is included.
///// </summary>
//public class TouchPad : MonoBehaviour
//{
//	/// <summary>
//	/// delegate to be used in other classes when a touch event occurs
//	/// </summary>
//	public delegate void TouchPadDelegate(TOUCH_EVENT touchEvent, Vector3 _touchPosition);
//	
//	/// <summary>
//	/// cached transform for quicker access later
//	/// </summary>
//	private Transform _thisTransform;
//	
//	/// <summary>
//	/// the delegate to call when a touch event occurs 
//	/// </summary>
//	private TouchPadDelegate _touchPadDelegate;
//	
//	/// <summary>
//	/// position of the touch 
//	/// </summary>
//	private Vector3 _touchPosition;
//	
//	/// <summary>
//	/// cached value of half the touch pad width 
//	/// </summary>
//	private float _halfTouchPadWidth;
//	
//	/// <summary>
//	/// cached value of half the touch pad height 
//	/// </summary>
//	private float _halfTouchPadHeight;
//	
//	/// <summary>
//	/// event that occured 
//	/// </summary>
//	public enum TOUCH_EVENT
//	{
//		TouchDown,
//		TouchMove,
//		TouchUp,
//		TouchStationary
//		
//	}
//	
//	float stationaryTouchCount=0.0f;
//	public bool isGestureFinished=false;
//	public bool didGoStationary=false;
//	/// <summary>
//	/// the camera used to translate points
//	/// </summary>
//	public Camera guiCamera;
//	
//	/// <summary>
//	/// width of the touch pad 
//	/// </summary>
//	public float touchPadWidth;
//	
//	/// <summary>
//	/// height of the touch pad 
//	/// </summary>
//	public float touchPadHeight;
//	
//	Vector3 previousTouchPos=Vector3.zero;
//	Vector3 currentTouchPos=Vector3.zero;
//	
//	public void setTouchPadSize(float width,float height)
//	{
//		this.touchPadWidth=width;
//		this.touchPadHeight=height;
//	}
//	
//	
//	/// <summary>
//	/// Initialize 
//	/// </summary>
//	void Awake()
//	{
//	//	touchPadWidth=Screen.width;
//	//	touchPadHeight=Screen.height;
//		// cache the transform
//		_thisTransform = this.transform;
//		
//		_halfTouchPadWidth = touchPadWidth / 2.0f;
//		_halfTouchPadHeight = touchPadHeight / 2.0f;
//	}
//	
//	
//	/// <summary>
//	/// Look for input 
//	/// </summary>
//	void Update()
//	{
//		// Determine whether we are using a mouse or touchpad
//		
//#if UNITY_IPHONE || UNITY_ANDRIOD
//		if (!Application.isEditor)
//		{
//			CheckTouch();
//		}
//		else
//		{
//			CheckMouse();
//		}
//#else
//		
//		CheckMouse();		
//#endif
//	}
//	
//	public void CheckMouse()
//	{
//		// if left mouse button is down
//		if (Input.GetMouseButtonDown(0))
//		{
//			didGoStationary=false;
//			isGestureFinished=false;
//			// if there is a delegate assigned to handle the input
//			if (_touchPadDelegate != null)
//			{
//				// if position is in bounds
//				if (GetTouchPosition(ref _touchPosition))
//				{
//					// call the delegate with the touch down event and position
//					_touchPadDelegate(TOUCH_EVENT.TouchDown, _touchPosition);
//				}
//			}
//		}
//		else if (Input.GetMouseButtonUp(0))
//		{
//			//isGestureFinished=false;
////			if(isGestureFinished)
////			{
////				if (_touchPadDelegate != null)
////				_touchPadDelegate(TOUCH_EVENT.TouchStationary, Vector3.zero);
////			}
////			else
////			{
//			isGestureFinished=true;
//					if (_touchPadDelegate != null)
//				_touchPadDelegate(TOUCH_EVENT.TouchUp, Vector3.zero);
//		//	}
//			
//		}
//		else if (Input.GetMouseButton(0) && !isGestureFinished)
//		{
//			if (_touchPadDelegate != null)
//			{
//				if (GetTouchPosition(ref _touchPosition))
//				{
//					currentTouchPos=guiCamera.ScreenToWorldPoint(Input.mousePosition);
//					//	Debug.Log("Vector3.Distance(previousTouchPos,currentTouchPos)=="+Vector3.Distance(previousTouchPos,currentTouchPos));
//					if(previousTouchPos!=Vector3.zero)
//					{
//						if(Vector3.Distance(previousTouchPos,currentTouchPos)==0.0f)
//						{
//							
//							stationaryTouchCount+=Time.deltaTime;
//							if(stationaryTouchCount>=0.15f)
//							{
//								didGoStationary=true;
////								Debug.Log("stationaryTouchCount="+stationaryTouchCount);
//								stationaryTouchCount=0.0f;
//								isGestureFinished=true;
//								_touchPadDelegate(TOUCH_EVENT.TouchStationary, Vector3.zero);	
//							}
//
//						}
//						else
//						{
//							stationaryTouchCount=0.0f;	
//						}
//					}
//							previousTouchPos=currentTouchPos;
//							_touchPadDelegate(TOUCH_EVENT.TouchMove, _touchPosition);
//					
//				}
//			}
//		}		
//	}
//	
//	public void CheckTouch()
//	{
//		// if a touch began
//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
//		{
//		//	stationaryTouchTime=Time.deltaTime;
//			// if there is a delegate assigned to handle the input
//			isGestureFinished=false;
//			if (_touchPadDelegate != null)
//			{
//				// if position is in bounds
//				if (GetTouchPosition(ref _touchPosition))
//				{
//					// call the delegate with the touch down event and position
//					_touchPadDelegate(TOUCH_EVENT.TouchDown, _touchPosition);
//				}
//			}				
//		}			
//		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
//		{
//			isGestureFinished=true;
//			// else if the touch ended
//			if (_touchPadDelegate != null)
//				_touchPadDelegate(TOUCH_EVENT.TouchUp, Vector3.zero);
//			// if delegate exists, call the touch up event
//		}
//		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) 
//		{
//			// else the touch moved
//			
//			// if delegate exists and the touch is in bounds, then call the touch move event
//			if (_touchPadDelegate != null)
//				if (GetTouchPosition(ref _touchPosition))
//				{
//					_touchPadDelegate(TOUCH_EVENT.TouchMove, _touchPosition);
//				}
//		}
//		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary) 
//		{
//			isGestureFinished=true;
//			if (_touchPadDelegate != null)
//				_touchPadDelegate(TOUCH_EVENT.TouchStationary, Vector3.zero);
//		}
//	}
//	
//	/// <summary>
//	/// Determines if a touch is in the rectangle's bounds
//	/// </summary>
//	/// <param name="position">
//	/// Camera relative position
//	/// </param>
//	/// <returns>
//	/// Returns true if in bounds
//	/// </returns>	
//	private bool GetTouchPosition(ref Vector3 position)
//	{
//		// translate the position to world space
//		position = guiCamera.ScreenToWorldPoint(Input.mousePosition);
//		position.z = 0;
//		
//		if (position.x >= (_thisTransform.position.x - _halfTouchPadWidth) && position.x <= (_thisTransform.position.x + _halfTouchPadWidth)
//		    	&& position.y >= (_thisTransform.position.y - _halfTouchPadHeight) && position.y <= (_thisTransform.position.y + _halfTouchPadHeight))
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
//	
//	/// <summary>
//	/// Sets the touch delegate from another class 
//	/// </summary>
//	/// <param name="_touchPadDelegate">
//	/// A <see cref="TouchPadDelegate"/>
//	/// </param>
//	public void SetTouchPadDelegate(TouchPadDelegate touchPadDelegate)
//	{
//		_touchPadDelegate = touchPadDelegate;
//	}
//	
//	/// <summary>
//	/// Draws a wireframe cube of the touchpad in the scene editor 
//	/// </summary>
//	void OnDrawGizmosSelected() 
//	{
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawWireCube(transform.position, new Vector3(touchPadWidth, touchPadHeight, 0.1f));
//	}
//}