//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using System.Collections.Generic;

/// <summary>
/// Class that stores a collection of stroke variation lists
/// </summary>
public class GREGesture
{
	/// <summary>
	/// The index of this gesture 
	/// </summary>
	private int _index;			
	
	/// <summary>
	/// The name of this gesture 
	/// </summary>
	private string _name;		

	/// <summary>
	/// /// The list of strokes (which hold the list of variations of each stroke) 
	/// </summary>
	private List<List<GREStroke>> _strokes;
	
	/// <summary>
	/// List of the current set of strokes 
	/// </summary>
	private List<GREStroke> _currentStrokeArray;
	
	/// <summary>
	/// Public accessor to the index 
	/// </summary>
	public int Index { get { return _index; } set { _index = value; } }
	
	/// <summary>
	/// Public accessor to the name with trimming
	/// </summary>
	public string Name { get { return _name.Trim(); } set { _name = value.Trim(); } }
	
	/// <summary>
	/// Public accessor to the list of strokes 
	/// </summary>
	public List<List<GREStroke>> Strokes { get { return _strokes; } }
	
	/// <summary>
	/// Overrides the ToString() function to return the formatted gesture string
	/// </summary>
	public override string ToString ()
	{
		string gestureString = _name.Trim() + ":";
		
		foreach (List<GREStroke> strokes in _strokes)
		{
			foreach (GREStroke stroke in strokes)
			{
				gestureString += stroke.StrokeString.Trim() + ",";
			}
			gestureString = gestureString.Substring(0, gestureString.Length-1) + ";";
		}
		gestureString = gestureString.Substring(0, gestureString.Length-1);
		
		return gestureString;
	}
	
	/// <summary>
	/// Creates a new gesture 
	/// </summary>
	public GREGesture()
	{
		_index = -1;
		_name = "";
		
		_strokes = new List<List<GREStroke>>();
	}

	/// <summary>
	/// Creates a new gesture 
	/// </summary>
	/// <param name="name">
	/// Gesture's name
	/// </param>
	public GREGesture(string name)
	{
		_index = -1;
		_name = name;
		
		_strokes = new List<List<GREStroke>>();
	}	
	
	/// <summary>
	/// Creates a new gesture with an index and string 
	/// </summary>
	/// <param name="_index">
	/// Index is the number of this gesture
	/// </param>
	/// <param name="gestureString">
	/// The string of stroke variations that comprise this gesture.
	/// </param>
	public GREGesture(int index, string gestureString)
	{
		_index = index;
		
		_strokes = new List<List<GREStroke>>();
		
		Replace(gestureString);
	}
	
	/// <summary>
	/// Adds a list of strokes to the variations
	/// </summary>
	/// <param name="strokesString">
	/// list of strokes, separated by commas (i.e. 12,32,42 -- stroke count must match the gesture's current number of strokes)
	/// </param>
	/// <param name="error">
	/// Returns an error string if there was something wrong with the stroke list
	/// </param>
	/// <returns>
	/// returns true if successful, false if the stroke count was wrong
	/// </returns>
	public bool AddVariation(string strokesString, ref string error)
	{
		List<GREStroke> tempStrokes = new List<GREStroke>();
		GREStroke tempStroke;
		
		// split the string into a list of strokes and pass to the overloaded version of this function
		string [] strokes = strokesString.Trim().Split(","[0]);
		foreach (string stroke in strokes)
		{
			tempStroke = new GREStroke(stroke);
			tempStrokes.Add(tempStroke);
		}
		
		return AddVariation(tempStrokes, ref error);
	}
	
	/// <summary>
	/// Adds a list of strokes to the variations 
	/// </summary>
	/// <param name="strokesString">
	/// List of strokes
	/// </param>
	/// <param name="error">
	/// Returns an error string if there was something wrong with the stroke list
	/// </param>
	/// <returns>
	/// Returns true if successful, false if the stroke count was wrong
	/// </returns>
	public bool AddVariation(List<GREStroke> strokeList, ref string error)
	{
		GREStroke newVariation;
		bool addNew = false;
		int i;
		int strokeMatches = 0;
		
		if (strokeList == null)
		{
			// no strokes to add
			error = "There were no strokes to add.";
			return false;
		}
		
		if (strokeList.Count == 0)
		{
			// no strokes to add
			error = "There were no strokes to add.";
			return false;
		}
		
		// check to see if this gesture has strokes first
		if (_strokes.Count > 0)
		{
			if (_strokes.Count != strokeList.Count)
			{
				// wrong number of strokes for this gesture
				if (strokeList.Count < _strokes.Count)
					error = "There weren't enough strokes. Expecting " + _strokes.Count.ToString();
				else 
					error = "There were too many strokes. Expecting " + _strokes.Count.ToString();
				
				return false;
			}

			for (i=0; i<strokeList.Count; i++)
			{
				// search the variations to make sure this stroke hasn't been added already
				addNew = true;
				foreach (GREStroke variation in _strokes[i])
				{
					if (variation.StrokeString == strokeList[i].StrokeString)
					{
						// this stroke variation already exists
						addNew = false;
						strokeMatches++;
						break;
					}
				}
				
				if (addNew)
				{
					newVariation = new GREStroke(strokeList[i].StrokeString);
					_strokes[i].Add(newVariation);
				}
			}
			
			if (strokeMatches == _strokes.Count)
			{
				error = "All of the strokes already have a match.";
				return false;
			}
		}
		else
		{
			// add all the strokes since this is a new gesture
			for (i=0; i < strokeList.Count; i++)
			{
				newVariation = new GREStroke(strokeList[i].StrokeString);

				_strokes.Add(new List<GREStroke>());					
				_strokes[i].Add(newVariation);
			}
		}

		return true;
	}	
	
	/// <summary>
	/// Puts a stroke collection of variations in at the specified index 
	/// </summary>
	/// <param name="strokeIndex">
	/// The index to insert the stroke
	/// </param>
	public void InsertStroke(int strokeIndex)
	{
		_strokes.Insert(strokeIndex, new List<GREStroke>());					
	}
	
	/// <summary>
	/// Removes a list of stroke variations from the gesture 
	/// </summary>
	/// <param name="strokeIndex">
	/// The index of the stroke to remove
	/// </param>
	public void RemoveStroke(int strokeIndex)
	{
		// dispose each of the variations of this stroke
		foreach (GREStroke variation in _strokes[strokeIndex])
		{
			variation.Dispose();
		}
		
		// clear the list of variations
		_strokes[strokeIndex].Clear();
		
		// remove the stroke from the list of strokes
		_strokes.RemoveAt(strokeIndex);
	}
	
	/// <summary>
	/// Removes a variation of a stroke based on a stroke string 
	/// </summary>
	/// <param name="strokeIndex">
	/// The index of the stroke from which to remove the variation
	/// </param>
	/// <param name="strokeString">
	/// The stroke string to match (i.e. 1234)
	/// </param>
	public void RemoveVariation(int strokeIndex, string strokeString)
	{
		strokeString = strokeString.Trim();
		
		// loop through the variations of this stroke
		for (int i=0; i<_strokes[strokeIndex].Count; i++)
		{
			// check the variation to see if it matches the string
			if (_strokes[strokeIndex][i].StrokeString == strokeString)
			{
				// remove the matched string
				RemoveVariation(strokeIndex, i);
				break;
			}
		}
	}
	
	/// <summary>
	/// Removes a variation of a stroke based on a stroke string 
	/// </summary>
	/// <param name="strokeIndex">
	/// The index of the stroke from which to remove the variation
	/// </param>
	/// <param name="strokeString">
	/// The index of the variation to remove
	/// </param>	
	public void RemoveVariation(int strokeIndex, int variationIndex)
	{
		// dispose the variation
		_strokes[strokeIndex][variationIndex].Dispose();
		
		// remove the variation from the list
		_strokes[strokeIndex].RemoveAt(variationIndex);
	}
	
	
	/// <summary>
	/// Replaces a gesture with the contents of another gesture 
	/// </summary>
	/// <param name="newGesture">
	/// The gesture to get the new data from
	/// </param>
	public void Replace(GREGesture newGesture)
	{
		if (newGesture != null)
			Replace(newGesture.ToString());
	}
	
	/// <summary>
	/// Replaces a gesture's data with new data
	/// </summary>
	/// <param name="gestureString">
	/// The string of stroke variations that comprise this gesture. (i.e. Test:12,34;21;1,3,4)
	/// </param>		
	public void Replace(string gestureString)
	{
		gestureString = gestureString.Trim();
		
		// clear out old data
		Dispose();
		
		// ignore blank strings
		if (gestureString == "")
			return;
		
		// if the string isn't properly formated, just set the string to the name
		if (!gestureString.Contains(":"))
		{
			_name = gestureString;
			return;
		}
		
		// parse the name out of this gesture
		string [] fields = gestureString.Split(":"[0]);
		int strokeIndex;
		
		// there must be just a name, then the list of possible strokes
		if (fields.Length == 2)
		{
			_name = fields[0].Trim();

			// split the string into its strokes, each stroke seperated by semicolons
			string [] strokes = fields[1].Trim().Split(";"[0]);
			
			// create an array of strokes
			for (strokeIndex=0; strokeIndex<strokes.Length; strokeIndex++)
			{
				_strokes.Add(new List<GREStroke>());
			}

			// loop through each stroke, parsing out the variations
			for (strokeIndex=0; strokeIndex<strokes.Length; strokeIndex++)
			{
				// parse the variations delimited by commas for this stroke
				string [] variations = ((string)strokes[strokeIndex]).Trim().Split(","[0]);
				
				GREStroke newVariation;
				
				// for each variation, add it to the stroke array
				foreach (string variation in variations)
				{
					newVariation = new GREStroke(variation.Trim());
					
					_strokes[strokeIndex].Add(newVariation);
				}
			}
		}			
	}
	
	/// <summary>
	/// Cleans up the gesture 
	/// </summary>
	public void Dispose()
	{
		// clear each variation list
		foreach (List<GREStroke> variations in _strokes)
		{
			// dispose each stroke in the variations list
			foreach (GREStroke stroke in variations)
			{
				stroke.Dispose();
			}
			variations.Clear();
		}
		_strokes.Clear();
	}
	
	/// <summary>
	/// Check to see if an array of strokes matches this gesture 
	/// </summary>
	/// <param name="strokes">
	/// Array of stroke class
	/// </param>
	/// <returns>
	/// Returns true if the gesture was matched, false if not
	/// </returns>
	public bool Match(List<GREStroke> testStrokes)
	{
		GREStroke testStroke;
		bool gestureFound;
		bool strokeSetFound;
		int strokeIndex;
		
		gestureFound = false;
		
		// only check gestures that have the same number of strokes
		if (_strokes.Count == testStrokes.Count)
		{
			// check each stroke
			gestureFound = true;
			for (strokeIndex=0; strokeIndex<testStrokes.Count; strokeIndex++)
			{
				testStroke = testStrokes[strokeIndex];

				// check each variation of each stroke
				strokeSetFound = false;
				foreach (GREStroke stroke in _strokes[strokeIndex])
				{
					if (testStroke.StrokeString == stroke.StrokeString)
					{
						//UnityEngine.Debug.Log("CURRRENT GESTURE ="+testStroke.StrokeString);
						strokeSetFound = true;
						break;
					}
				}
				
				if (strokeSetFound == false)
				{
					//UnityEngine.Debug.Log("CURRRENT GESTURE NOT REC ="+testStroke.StrokeString);
					gestureFound = false;
					break;
				}
			}

			// if all the strokes in the variation were found, then this gesture is what we captured
			if (gestureFound == true)
			{
				return true;
			}
//			else
//			{
//				UnityEngine.Debug.Log("CURRRENT GESTURE NOT REC ="+testStroke.StrokeString);
//			}
		}
		
		// could not find a match
		return false;
	}
	
	/// <summary>
	/// This version of match compares two gesture classes against each other to find a match 
	/// </summary>
	/// <param name="testGesture">
	/// The gesture to test
	/// </param>
	/// <returns>
	/// Returns true if a match was found, false if not
	/// </returns>
	public bool Match(GREGesture testGesture)
	{
		bool matched = false;

		if (testGesture.Strokes.Count != _strokes.Count)
			return false;
		
		for (int i=0; i<_strokes.Count; i++)
		{
			// we have to find a match for each stroke,
			// so we reset the matched flag to false for each stroke
			matched = false;
			foreach (GREStroke stroke in _strokes[i])
			{
				foreach (GREStroke testStroke in testGesture.Strokes[i])
				{
					if (stroke.StrokeString == testStroke.StrokeString)
					{
						// we found a match for this stroke variation
						matched = true;
						break;
					}
				}
				if (matched)
					break;
			}
			if (!matched)
			{
				// none of these stroke variations match for this stroke
				return false;
			}
		}
		
		// all of the strokes found a match
		return true;
	}
}
