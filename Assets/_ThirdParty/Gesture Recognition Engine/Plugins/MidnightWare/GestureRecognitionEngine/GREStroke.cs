//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class that handles all movement change vectors for gesture recognition
/// </summary>
public class GREStroke
{
	/// <summary>
	/// the number of x elements captured before we start looking for direction changes 
	/// </summary>
	private const int HISTORY_X = 2;
	
	/// <summary>
	/// the number of y elements captured before we start looking for direction changes 
	/// </summary>
	private const int HISTORY_Y = 2;
	
	/// <summary>
	/// the minimum distance that needs to be traveled before we can look for a direction change 
	/// </summary>
	private const float RANGE = 1.0f;
	
	/// <summary>
	/// The various direction changes that the stroke can go 
	/// </summary>
	private enum DIRECTION
	{
		/// <summary>
		/// no direction change 
		/// </summary>
		None = 0,	
		
		/// <summary>
		/// moved down 
		/// </summary>
		Down = 1,	
		
		/// <summary>
		/// moved up
		/// </summary>
		Up = 2,		
		
		/// <summary>
		/// moved right 
		/// </summary>
		Right = 3,	
		
		/// <summary>
		/// moved left 
		/// </summary>
		Left = 4	
	}
	
	/// <summary>
	/// the string that stores the direction changes for this stroke
	/// </summary>
	private string _strokeString;
	
	/// <summary>
	/// array of the x locations visited by this stroke 
	/// </summary>
	private List<float> _xLocations;
	
	/// <summary>
	/// array of the y locations visited by this stroke 
	/// </summary>
	private List<float> _yLocations;
	
	/// <summary>
	/// current x direction 
	/// </summary>
	private DIRECTION _xDirection;
	
	/// <summary>
	/// current y direction
	/// </summary>
	private DIRECTION _yDirection;

	/// <summary>
	/// public override of the ToString() method
	/// </summary>
	public override string ToString ()
	{
		return _strokeString.Trim();
	}
	
	/// <summary>
	/// public accessor to the stroke string with trimming
	/// </summary>
	public string StrokeString
	{
		get
		{
			return _strokeString.Trim();
		}
		set
		{
			_strokeString = value.Trim();
		}
	}
	
	/// <summary>
	/// Public constructor of this class 
	/// </summary>
	public GREStroke()
	{
		// Initialize the members
		_xLocations = new List<float>();
		_yLocations = new List<float>();
		_xDirection = DIRECTION.None;
		_yDirection = DIRECTION.None;
		_strokeString = "";
	}
	
	/// <summary>
	/// Public constructor class
	/// </summary>
	/// <param name="strokeString">
	/// An initializing string of strokes separated by commas
	/// </param>
	public GREStroke(string strokeString)
	{
		StrokeString = strokeString;
		
		// Initialize the members
		_xLocations = new List<float>();
		_yLocations = new List<float>();
		_xDirection = DIRECTION.None;
		_yDirection = DIRECTION.None;
	}
	
	/// <summary>
	/// Public destructor of this class 
	/// </summary>
	public void Dispose()
	{
		_xLocations.Clear();
		_yLocations.Clear();
	}
	
	/// <summary>
	/// Moves the stroke to an x and y location 
	/// </summary>
	/// <param name="x">
	/// x value to move to
	/// </param>
	/// <param name="y">
	/// y value to move to
	/// </param>
	public void MoveTo(float x, float y)
	{
		bool xDirectionChanged;		// whether or not the x direction has changed from the last x direction
		bool yDirectionChanged;		// whether or not the y direction has changed from the last y direction
		float historyXLocation=0.0f;		// the value of the array of x locations at the index of the history
		float historyYLocation=0.0f;		// the value of the array of y locations at the index of the history
		
		// initialize to no change
		xDirectionChanged = false;
		yDirectionChanged = false;
		
		Vector2 vectorOld=Vector2.zero;
		Vector2 vectorNew=Vector2.zero;
		
		// add this x,y location to the arrays
		_xLocations.Add(x);
		_yLocations.Add(y);
		
		
		if(_xLocations.Count > HISTORY_X && _yLocations.Count > HISTORY_Y)
		{
			vectorOld=new Vector2(_xLocations[_xLocations.Count - HISTORY_X],_yLocations[_yLocations.Count - HISTORY_Y]);
			vectorNew=new Vector2(x,y);
			
			// store the history x location
			historyXLocation = _xLocations[_xLocations.Count - HISTORY_X];
			
			// store the history y location
			historyYLocation = _yLocations[_yLocations.Count - HISTORY_Y];
			
		}
		// check the X direction
		
		// if the number of samples is greater than the history counter
		if (_xLocations.Count > HISTORY_X)
		{		
			
		if(Mathf.Abs(x - historyXLocation) >= Mathf.Abs(y - historyYLocation))
			{
//				Debug.Log("XGreater x=="+Mathf.Abs(x - historyXLocation)+"y=="+Mathf.Abs(y - historyYLocation)+"_xDirection"+_xDirection);
				// if the last direction was none
				if (_xDirection == DIRECTION.None)
				{
					// if the offset of this x is greater than the minimum range and going to the right
					if ((x - historyXLocation) > RANGE)
					{
						// moving to the right
						_xDirection = DIRECTION.Right;
						//_yDirection=DIRECTION.None;
						xDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
//						Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
					else if ((historyXLocation - x) > RANGE) // going to the left
					{
						// moving to the left
						_xDirection = DIRECTION.Left;
						//_yDirection=DIRECTION.None;
						xDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
//						Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
				}
				else
				{
					// if we are going to the right and the last direction was left
					if ((x - historyXLocation) > RANGE && _xDirection == DIRECTION.Left)
					{
						// moving to the right
						_xDirection = DIRECTION.Right;
						xDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
					//	Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
					else if ((historyXLocation - x) > RANGE && _xDirection == DIRECTION.Right) // going to the left and the last direction was right
					{
						// moving to the left
						_xDirection = DIRECTION.Left;
						xDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
//						Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
				}
			}
		}			
		
		// check the Y direction (similar to the x checking above)
		if (_yLocations.Count > HISTORY_Y)
		{
			
			if(Mathf.Abs(x - historyXLocation) <= Mathf.Abs(y - historyYLocation))
			{
			//	Debug.Log("YGreater x=="+Mathf.Abs(x - historyXLocation)+"y=="+Mathf.Abs(y - historyYLocation)+"_yDirection="+_yDirection);
				if (_yDirection == DIRECTION.None)
				{
					if ((y - historyYLocation) > RANGE)
					{
						_yDirection = DIRECTION.Up;
						//_xDirection=DIRECTION.None;
						yDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
						//Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
					else if ((historyYLocation - y) > RANGE)
					{
						_yDirection = DIRECTION.Down;
						//_xDirection=DIRECTION.None;
						yDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
						//Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
				}
				else
				{
					if ((y - historyYLocation) > RANGE && _yDirection == DIRECTION.Down)
					{
						_yDirection = DIRECTION.Up;
						yDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
						//Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
					else if ((historyYLocation - y) > RANGE && _yDirection == DIRECTION.Up)
					{
						_yDirection = DIRECTION.Down;
						yDirectionChanged = true;
						float angle=Vector3.Angle(vectorOld,vectorNew);
						//Debug.Log("angle==="+angle+"_xLocations.Count=="+_xLocations.Count+"_yLocations.Count=="+_yLocations.Count);
					}
				}
			}
		}
		
		// if the x direction changed and it wasn't none
		if (xDirectionChanged && _xDirection != DIRECTION.None)
		{
			// add this direction to the stroke direction change list
			_strokeString += ((int)_xDirection).ToString();
		}
		
		// if the y direction changed and it wasn't none
		if (yDirectionChanged && _yDirection != DIRECTION.None)
		{
			// add this direction to the stroke direction change list
			_strokeString += ((int)_yDirection).ToString();
		}
//		Debug.Log("storkeString===="+_strokeString);
	}
}

