using System.Collections;

namespace UnityEngine
{
	public class InputWrapper 
	{
		public static bool _disableTouch = false; //dont use this!
			
		public static bool disableTouch
		{
			get
			{
				return _disableTouch;
			}
			set
			{
				if(value)
					_disableTouch = value;
				else
					GameManager.instance.EnableInput();
				//Debug.Log("_disableTouch > " + _disableTouch);
			}
		}
		
		public static int touchCount
		{
			get
			{
				if(disableTouch)
					return 0;
				return Input.touchCount;
			}
			private set	{	}
		}
		
		public static Touch[] touches
		{
			get
			{
				if(disableTouch)
					return new Touch[0];
				return Input.touches;
			}
			private set	{	}
		}
		
		public static float GetAxis(string axisName)
		{
			if(disableTouch)
				return 0;
			return Input.GetAxis(axisName);
		}
		
		public static bool GetMouseButton(int button)
		{
			if(disableTouch)
				return false;
			return Input.GetMouseButton(button);
		}
		
		public static bool GetMouseButtonUp(int button)
		{
			if(disableTouch)
				return false;
			return Input.GetMouseButtonUp(button);
		}
		
		public static bool GetMouseButtonDown(int button)
		{
			if(disableTouch)
				return false;
			return Input.GetMouseButtonDown(button);
		}
		
		public static Touch GetTouch(int index)
		{
			if(disableTouch)
				return Input.GetTouch(int.MaxValue);
			return Input.GetTouch(index);
		}
	}
}