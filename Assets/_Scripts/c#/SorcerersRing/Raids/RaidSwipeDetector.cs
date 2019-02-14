using UnityEngine;
using System.Collections;

public class RaidSwipeDetector : MonoBehaviour {

	public enum SwipeDirection{Left, Right}
	SwipeDirection _swipeDirection;
	
	public delegate void SwipeDirectionListener(SwipeDirection direction);
	public SwipeDirectionListener _swipeDirectionListener;
	
	private float MIN_DISTANCE = Screen.width / 10;
	
	float startX;
	float endX;
	
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
			startX = Input.mousePosition.x;
		if(Input.GetMouseButtonUp(0))
			endX = Input.mousePosition.x;
		
		if(startX != 0 && endX != 0 && Mathf.Abs(endX - startX) > MIN_DISTANCE)
		{
			_swipeDirection = ((endX - startX) > 0) ? SwipeDirection.Right : SwipeDirection.Left;
			
			if(_swipeDirectionListener != null)
				_swipeDirectionListener(_swipeDirection);
			
			endX = 0;
			startX = 0;
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			endX = 0;
			startX = 0;
		}
	}
}
