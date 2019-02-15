//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Gesture Recognition allows you to capture the movement of anything in 2D space (x,y) and compare
/// it to a predefined library of gestures, such as an alphabet, spell casting, dance movements, etc.
/// </summary>

public class GREngine
{
    /// ---------------------------------------------------------------------------------------------------
    /// Glossary:
    /// ---------------------------------------------------------------------------------------------------
    /// 
    /// Gesture Recognition	- The ability to analyze and match defined movement patterns
    /// Gesture 			- A collection of strokes (with variations on each stroke) that defines a pattern of movement (or changes in direction)
    /// Stroke				- A single movement capture of an object (such as the downward swipe in a cross pattern)
    /// Stroke Variation	- An instance of how a stroke can be performed within a gesture. 
    /// 						This allows us to have multiple ways of performing a single gesture,
    /// 						giving greater flexibility over an individual's drawing style.
    /// Gesture Library		- A collection of gestures that are used for recognition
    /// 
    /// ---------------------------------------------------------------------------------------------------
    /// Stroke Directions:
    /// ---------------------------------------------------------------------------------------------------
    /// 
    /// A stroke direction change can occur in one of the following four ways:
    /// 
    /// 1 - Down
    /// 2 - Up
    /// 3 - Right
    /// 4 - Left
    /// 
    /// Ex: Stroke 13212 means that the stroke moved down, right, up, down, up (in that order).
    /// 
    /// ---------------------------------------------------------------------------------------------------
    /// Gesture Format:
    /// ---------------------------------------------------------------------------------------------------
    /// 
    /// The format of the gesture is as follows:
    /// 
    /// GestureName : Stroke1_variation1, Stroke1_variation2, ... ; Stroke2_variation1, Stroke2_variation2 ... ; ...
    /// 
    /// Ex: TestGesture:12,32;33,34,32;12
    /// 
    /// This gesture is named "TestGesture". It has three strokes (separated by semicolons). The first stroke 
    /// has 2 variations (separated by a comma. The second stroke has 3 variations (separated by commas). The 
    /// third stroke only has one possible stroke. There are 6 possible combinations of these strokes that will
    /// yield a successful gesture match: 
    /// 
    /// 12,33,12
    /// 12,34,12
    /// 12,32,12 
    /// 32,33,12
    /// 32,34,12
    /// 32,32,12
    /// 
    /// Storing the gesture in this manner allows us to save space by enumerating variations instead of 
    /// explicitly listing every possible combination of strokes.
    /// 
    /// Note that a gesture must have the same stroke count for every variation. For instance, you cannot
    /// have a gesture that sometimes has 2 strokes and sometimes has 3 strokes. You would need to make
    /// a seperate gesture for this case.
    ///
    /// ---------------------------------------------------------------------------------------------------
    /// Usage:
    /// ---------------------------------------------------------------------------------------------------
    /// 
    /// The order of operations is:
    /// 
    /// 1) Reset the Gesture
    /// 2) Start a new stroke
    /// 3) Move stroke
    /// 4) ... repeat #3 as necessary
    /// 5) End Stroke
    /// 6) ... repeat #2 - #5 as necessary
    /// 7) Match gesture
    /// 8) Go back to #1
    /// 
    /// The recognition is based on direction changes, so the scale of the movement is irrelevant.
    /// This means you can match against large and small gesturess using the same library. Using this 
    /// technique allows better scalability, but requires precise starting and ending points for each stroke.
    /// For instance, if you are drawing an X and you want to be sure to capture all the possibilities of how to 
    /// draw the x, you will need to check for the eight cases:
    /// 
    /// 1) first stroke from top left to bottom right, second stroke from top right to bottom left
    /// 2) first stroke from top left to bottom right, second stroke from bottom left to top right
    /// 3) first stroke from bottom right to top left, second stroke from top right to bottom left
    /// 4) first stroke from bottom right to top left, second stroke from bottom left to top right
    /// 5) first stroke from top right to bottom left, second stroke from top left to bottom right
    /// 6) first stroke from bottom left to top right, second stroke from top left to bottom right
    /// 7) first stroke from top right to bottom left, second stroke from bottom right to top left
    /// 8) first stroke from bottom left to top right, second stroke from bottom right to top left
    /// 
    /// This may seem like more work than a raster based grid comparison, but it also allows you to reuse the same 
    /// look of a gesture in multiple ways. Using the X example above, you could have 8 seperate gestures that all
    /// look like an X, but are handled differently when the strokes are drawn in a different order and direction.
    /// 
    /// Circles and curves can be tricky since there are so many direction changes involved. It would be nearly
    /// impossible (at least on a large scale) to capture every possible stroke combination involved in drawing a
    /// circle. The best method here is to try to enforce a starting point, such as the top or bottom of the
    /// circle, then the possibility range is narrowed to just a few variations.
    /// 
    /// ---------------------------------------------------------------------------------------------------
    /// Performance:
    /// ---------------------------------------------------------------------------------------------------
    /// 
    /// Since this recognition is based on direction changes, it is best to have a high sample rate, meaning that
    /// if the frame rate drops too low, you will likely not get matches on your gestures. This is due to 
    /// the fact that your object being watched may have moved and changed direction in between captures, causing
    /// the engine to miss the fact that a direction change occurred. The optimum state for the engine is a
    /// high capture rate. If this cannot be achieved, then you may need to enforce the object to move slowly. If
    /// the object being watched is a finger on a touchpad or a mouse on a screen, then you will have to remind 
    /// your users to draw slower if they frequently experience unmatched gestures. A framerate of 30 fps should
    /// be sufficient for most gestures. The rule of thumb is: the faster the motion, the higher the framerate
    /// needs to be to capture the motion accurately.
		
    /// <summary>
    /// currently drawing a gesture 
    /// </summary>
    private bool _gestureBegan = false;
	
    /// <summary>
    /// currently drawing a stroke of the gesture
    /// </summary>
    private bool _strokeBegan = false;
	
    /// <summary>
    /// list of all the gestures in the library
    /// </summary>
    private string[] _libraryGestures;
	
    /// <summary>
    /// a string containing our gestures, each gesture separated by carriage returns, each collection of possible strokes separated by semicolons, each possible stroke separated by commas
    /// </summary>
    private string _gestureLibraryString;
	
    /// <summary>
    /// the array of all gestures in our library 
    /// </summary>
    public List<GREGesture> gestureLibrary;
	
	#region Sorcerer's ring specific
    public List<GREGesture> simpleGestureLibrary;
    public List<GREGesture> runeGestureLibrary;
	#endregion
	
    /// <summary>
    /// the array of strokes being captured into memory 
    /// </summary>	
    public List<GREStroke> currentStrokes;
	
    /// <summary>
    /// the current stroke being captured 
    /// </summary>
    public GREStroke currentStroke;
	
    /// <summary>
    /// True if the gesture is being recorded, else false 
    /// </summary>
    public bool GestureBegan
    {
        get
        {
            return _gestureBegan;
        }
        set
        {
            _gestureBegan = value;
        }
    }
	
    /// <summary>
    /// True if a stroke is being recorded, else false 
    /// </summary>
    public bool StrokeBegan
    {
        get
        {
            return _strokeBegan;
        }
        set
        {
            _strokeBegan = value;
        }
    }
	
    /// <summary>
    /// Public constructor 
    /// </summary>
    public GREngine()
    {
        currentStrokes = new List<GREStroke>();
        ResetLibrary();
    }
	
    /// <summary>
    /// Resets the current array of strokes 
    /// </summary>
    private void ClearCurrentStrokes()
    {
        foreach (GREStroke stroke in currentStrokes)
        {
            stroke.Dispose();
        }
        currentStrokes.Clear();
    }
	
    /// <summary>
    /// Clears the library of all strokes 
    /// </summary>
    public void ResetLibrary()
    {
        if (gestureLibrary == null)
        {
            gestureLibrary = new List<GREGesture>();
            simpleGestureLibrary = new List<GREGesture>();
            runeGestureLibrary = new List<GREGesture>();
        } else
        {
            foreach (GREGesture gs in gestureLibrary)
            {
                gs.Dispose();
            }
            gestureLibrary.Clear();
			
            foreach (GREGesture gs in simpleGestureLibrary)
            {
                gs.Dispose();
            }
            simpleGestureLibrary.Clear();
			
            foreach (GREGesture gs in runeGestureLibrary)
            {
                gs.Dispose();
            }
            runeGestureLibrary.Clear();
        }		
    }
	
    public void SetNormalLibrary(bool yesNo)
    {
        if (yesNo)
        {
            if (gestureLibrary != simpleGestureLibrary)
                gestureLibrary = simpleGestureLibrary;
        } else
        {
            if (gestureLibrary != runeGestureLibrary)
                gestureLibrary = runeGestureLibrary;
        }
    }
	
    /// <summary>
    /// Loads a library of gestures from a file path 
    /// </summary>
    /// <param name="filePath">
    /// The path to the file containing the gestures
    /// </param>
    /// <returns>
    /// Returns true if loaded, false if not
    /// </returns> 
    public bool LoadLibraryFromPath(string filePath)
    {
        try
        {
            // read the string of gestures from the file
            StreamReader sr = new StreamReader(filePath);
            string gestureLibraryString = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
			
            // parse the string into gestures
            LoadLibraryFromString(gestureLibraryString);
			
            return true;
        } catch
        {
            return false;
        }
    }
	
    /// <summary>
    /// Loads a library of strokes from a string (usually read from a text file). This version of LoadLibrary can be useful with TextAssets.
    /// </summary>
    /// <param name="_gestureLibraryString">
    /// String representing return delimited ( \n ) gestures, usually loaded from a text file
    /// </param>
    public void LoadLibraryFromString(string gestureLibraryString)
    {
        _gestureLibraryString = gestureLibraryString;
		
        ResetLibrary();

        if (_gestureLibraryString != "")
        {
            // split up the gestures, one per line
            _libraryGestures = _gestureLibraryString.Split("\n" [0]);
            for (int index=0; index<_libraryGestures.Length; index++)
            {
                // only add non-blank lines
                if (_libraryGestures [index].Trim() != "")
                {
                    // ignore comments
                    if (_libraryGestures [index].ToString().Substring(0, 1) != "'")
                    {
                        // add the gesture to our library
                        AddGesture(_libraryGestures [index]);
                    }
                }
            }
        }
    }
	
    /// <summary>
    /// Saves the library of gestures back to a file 
    /// </summary>
    /// <param name="filePath">
    /// The path to the file containing the gestures 
    /// </param>
    /// <returns>
    /// Returns true if saved, false if not
    /// </returns>
    public bool SaveLibraryToPath(string filePath)
    {
        try
        {
            // open a writer to the file path
            StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.ASCII);
			
            // add each gesture in the library
            foreach (GREGesture gesture in gestureLibrary)
            {
                sw.WriteLine(gesture.ToString());
            }
			
            // close the writer
            sw.Flush();
            sw.Close();
            sw.Dispose();	
			
            return true;
        } catch
        {
            return false;
        }
    }
	
    /// <summary>
    /// Adds a gesture to the library, based on a string of data
    /// </summary>
    /// <param name="gestureString">
    /// A formatted string containing the gesture name followed by stroke variations (i.e Test:12,34;21;4,1)
    /// </param>
    /// <returns>
    /// Returns the gesture
    /// </returns>
    public GREGesture AddGesture(string gestureString)
    {
        GREGesture gesture;
		
//		UnityEngine.Debug.Log("gestureString --->>> " + gestureString);
		
        gesture = new GREGesture(gestureLibrary.Count, gestureString);
        gestureLibrary.Add(gesture);
		
//		UnityEngine.Debug.Log("gesture NAME --->>> " + gesture.Name);
        if (gesture.Name.Equals("Ignite") || gesture.Name.Equals("Daze") || gesture.Name.Equals("Drain") || gesture.Name.Equals("LeechSeed"))
            runeGestureLibrary.Add(gesture);
        else
            simpleGestureLibrary.Add(gesture);
			
        gesture.Index = gestureLibrary.Count - 1;

        return gesture;
    }
	
	
	
    /// <summary>
    /// Replaces a gesture in the library with another one at the given index
    /// </summary>
    /// <param name="index">
    /// The index of the gesture to replace
    /// </param>
    /// <param name="newGestureString">
    /// The new gesture string to replace the old
    /// </param>
    /// <returns>
    /// Returns the index of the gesture
    /// </returns>
    public int ReplaceGesture(int index, string newGestureString)
    {
        foreach (GREGesture gesture in gestureLibrary)
        {
            if (gesture.Index == index)
            {
                gesture.Replace(newGestureString);
                return index;
            }
        }
		
        // Gesture not found, so we add a new one
        return AddGesture(newGestureString).Index;
    }
	
    /// <summary>
    /// Replaces a gesture the library with another one. If two gestures match this name, then the first will be overwritten
    /// </summary>
    /// <param name="oldName">
    /// The name of the gesture to replace
    /// </param>
    /// <param name="newGestureString">
    /// The new gesture string to replace the old
    /// </param>
    /// <returns>
    /// Returns the index of the gesture
    /// </returns>
    public int ReplaceGesture(string oldName, string newGestureString)
    {
        for (int index=0; index<gestureLibrary.Count; index++)
        {
            if (gestureLibrary [index].Name == oldName.Trim())
            {
                gestureLibrary [index].Replace(newGestureString);
                return index;
            }
        }
		
        // Gesture not found, so we add a new one
        return AddGesture(newGestureString).Index;
    }
	
    /// <summary>
    /// Check to see if the current strokes are a match for any gesture in our library 
    /// </summary>
    /// <returns>
    /// Returns the GREGesture if a match is found, null if not
    /// </returns>
    public GREGesture MatchCurrentStrokesToGesture()
    {
        // make sure we have at least one stroke
        if (currentStrokes.Count > 0)
        {
            // check each gesture in the library
            foreach (GREGesture gesture in gestureLibrary)
            {
                if (gesture.Match(currentStrokes))
                {
                    // match found
					
                    return gesture;
                }
            }
        }
//		if(currentStrokes.Count > 0 && currentStrokes[0] != null)
//		{
//			UnityEngine.Debug.Log("CURRRENT GESTURE NOT REC ="+currentStrokes[0].StrokeString);
//		}
        // no match found in the library
        return null;
    }
	
    /// <summary>
    /// Check to see if a test gesture matches any gesture in the library 
    /// </summary>
    /// <param name="gestureString">
    /// A formatted string containing the gesture name followed by stroke variations (i.e Test:12,34;1;4,2)
    /// </param>
    /// <returns>
    /// Returns GREGesture if a match is found, null if not
    /// </returns>
    public GREGesture MatchGesture(string gestureString)
    {
        if (gestureString.Trim() != "")
        {
            GREGesture gestureTest = new GREGesture(-1, gestureString);
			
            // check each gesture in the library
            foreach (GREGesture gesture in gestureLibrary)
            {
                if (gesture.Match(gestureTest))
                {
                    // match found
                    return gesture;
                }
            }
        }
	
        // no match found in the library
        return null;
    }
	
    /// <summary>
    /// Returns whether a gesture name already exists in the library 
    /// </summary>
    /// <param name="name">
    /// Name of the gesture to test (case sensitive)
    /// </param>
    /// <returns>
    /// Returns true if the gesture exists in library, false if not
    /// </returns>
    public bool GestureNameExists(string name)
    {
        name = name.Trim();
		
        foreach (GREGesture gs in gestureLibrary)
        {
            if (gs.Name == name)
            {
                return true;
            }
        }
		
        return false;
    }
	
    /// <summary>
    /// Resets the gesture recognition system when starting a new gesture
    /// </summary>
    public void ResetEngine()
    {
        GestureBegan = false;
        StrokeBegan = false;
		
        ClearCurrentStrokes();
    }
	
    /// <summary>
    /// Begins a new stroke in the current gesture 
    /// </summary>
    /// <param name="x">
    /// x location
    /// </param>
    /// <param name="y">
    /// y location
    /// </param>
    public void BeginStroke(float x, float y)
    {
        GestureBegan = true;
        StrokeBegan = true;
		
        // create a new stroke and move to the x,y location
        currentStroke = new GREStroke();
        currentStroke.MoveTo(x, y);
    }
	
    /// <summary>
    /// Move the current stroke to the x,y location 
    /// </summary>
    /// <param name="x">
    /// x location
    /// </param>
    /// <param name="y">
    /// y location
    /// </param>
    public void MoveStroke(float x, float y)
    {
        // only move if we have started a stroke
        if (StrokeBegan == true)
        {
            currentStroke.MoveTo(x, y);
        }
    }
	
    /// <summary>
    /// Finish the stroke 
    /// </summary>
    public void EndStroke()
    {
        if (StrokeBegan == true)
        {
            // add this stroke to the current list of strokes
            currentStrokes.Add(currentStroke);
        }
        StrokeBegan = false;
    }
	
    /// <summary>
    /// This is used to return the current strokes being created in string format
    /// </summary>
    public string GetCurrentStrokesString()
    {
        string strokeString = "";
        if (currentStrokes.Count > 0)
        {
            foreach (GREStroke stroke in currentStrokes)
            {
                strokeString += stroke.StrokeString + " ; ";
            }
            strokeString = strokeString.Substring(0, strokeString.Length - 3);
        }
        return strokeString;		
    }
	
    /// <summary>
    /// Formats the current stroke list as a list of semi-colon separated strokes 
    /// </summary>
    /// <returns>
    /// Returns a gesture string
    /// </returns>
    public string FormatCurrentStrokesAsGestureString()
    {
        string gestureString = "";
		
        foreach (GREStroke stroke in currentStrokes)
        {
            gestureString += stroke.StrokeString + ";";
        }
        gestureString = gestureString.Substring(0, gestureString.Length - 1);
		
        return gestureString;
    }
		
    /// <summary>
    /// Takes an array of stroke variations and formats it into a gesture string
    /// </summary>
    /// <param name="strokeVariations">
    /// An array of stroke variations, each element is a list of strokes (Ex: 123,412,12,2)
    /// </param>
    /// <returns>
    /// Returns the gesture string (without the gesture name and colon)
    /// </returns>
    public string FormatGestureString(List<string> strokeVariations)
    {
        string [] strokeStrings;										// array of strokes processed from each gesture stroke string
        List<List<string>> gestureStrokes = new List<List<string>>();	// temporary list of stroke variation strings
        List<string> strokes;											// temporary list of strokes
        int strokeCount = -1;											// the number of strokes for this gesture
        string gestureString;											// the output of this function, a formatted gesture
        bool foundStroke;												// tests whether a stroke was already in existance in the stroke variations
        int index;									
		
        // if the array passed is not empty
        if (strokeVariations.Count > 0)
        {
            // loop through each possible gesture stroke string in the array
            foreach (string strokeVariation in strokeVariations)
            {
                // get the individual strokes in this gesture stroke string
                if (strokeVariation.Contains(","))
                {
                    strokeStrings = strokeVariation.Trim().Split("," [0]);
                } else
                {
                    strokeStrings = new string[1];
                    strokeStrings [0] = strokeVariation.Trim();
                }
					
                // make sure the stroke count is the same as previous counts
                if (strokeCount == -1)
                {
                    strokeCount = strokeStrings.Length;
                } else if (strokeCount != strokeStrings.Length)
                {
                    return "Error: All the possible gestures must have the same stroke count";
                }
				
                // if this is the first gesture stroke string looked at,
                // then we just add its strokes to the temporary array
                if (gestureStrokes.Count == 0)
                {
                    // add all the strokes
                    foreach (string stroke in strokeStrings)
                    {
                        strokes = new List<string>();
                        strokes.Add(stroke.Trim());
                        gestureStrokes.Add(strokes);
                    }
                } else
                {
                    // loop through each stroke variation array
                    for (index=0; index<strokeCount; index++)
                    {
                        // seach for this stroke in the stroke variation array
                        foundStroke = false;
                        foreach (string stroke in gestureStrokes[index])
                        {
                            if (stroke.Trim() == strokeStrings [index].Trim())
                            {
                                // this stroke already exists in the stroke variation array
                                foundStroke = true;
                                break;
                            }
                        }
                        if (!foundStroke)
                        {
                            // we did not find this stroke in the stroke variation array
                            // so we add it to our current stroke variation array
                            gestureStrokes [index].Add(strokeStrings [index].Trim());
                        }
                    }
                }
            }
			
            // loop through each stroke variation array and then the strokes within
            // each array to build our formatted string
            gestureString = "";
            if (gestureStrokes.Count > 0)
            {
                foreach (List<string> strokeVariation in gestureStrokes)
                {
                    if (strokeVariation.Count > 0)
                    {
                        foreach (string strokeString in strokeVariation)
                        {
                            gestureString += strokeString.Trim() + ",";
                        }
                        gestureString = gestureString.Substring(0, gestureString.Length - 1) + ";";
                    }
                }
                gestureString = gestureString.Substring(0, gestureString.Length - 1);
            }
			
            return gestureString;
        } else
        {
            return "";
        }
    }
	
    /// <summary>
    /// Checks to see if two strings have the same number of strokes in them 
    /// </summary>
    /// <param name="strokes1">
    /// First string full of strokes
    /// </param>
    /// <param name="strokes2">
    /// Second string full of strokes
    /// </param>
    /// <returns>
    /// Returns true if the count is the same
    /// </returns>
    public bool StrokeCountsMatch(string strokes1, string strokes2)
    {
        int strokeCount1, strokeCount2;

        if (strokes1.Trim() == "")
        {
            strokeCount1 = 0;
        } else
        {
            if (strokes1.Contains(","))
            {
                strokeCount1 = (strokes1.Split("," [0])).Length;
            } else
            {
                strokeCount1 = 1;
            }
        }

        if (strokes2.Trim() == "")
        {
            strokeCount2 = 0;
        } else
        {
            if (strokes2.Contains(","))
            {
                strokeCount2 = (strokes2.Split("," [0])).Length;
            } else
            {
                strokeCount2 = 1;
            }
        }
		
        return strokeCount1 == strokeCount2;
    }
	
    /// <summary>
    /// Returns the formatted list of gestures for the current library 
    /// </summary>
    /// <returns>
    /// The formatted library string
    /// </returns>
    public string GetLibraryString()
    {
        string libraryString = "";
		
        for (int i=0; i < gestureLibrary.Count; i++)
        {
            libraryString += gestureLibrary [i].ToString();
            if (i < gestureLibrary.Count - 1)
                libraryString += "\n";
        }
		
        return libraryString;
    }
	
    /// <summary>
    /// Returns a comma delimited list of gesture names from the library
    /// </summary>
    public string GetGestureNames()
    {
        string gestureNamesString = "";
		
        for (int i=0; i < gestureLibrary.Count; i++)
        {
            gestureNamesString += gestureLibrary [i].Name;
            if (i < gestureLibrary.Count - 1)
                gestureNamesString += ",";
        }
		
        return gestureNamesString;
    }
	
    /// <summary>
    /// Returns the name of a gesture at a given index 
    /// </summary>
    /// <param name="index">
    /// Index of the gesture
    /// </param>
    public string GetGestureName(int index)
    {
        GREGesture gesture = GetGesture(index);
		
        if (gesture != null)
            return gesture.Name;
        else
            return "";
    }
	
    /// <summary>
    /// Gets a gesture by index 
    /// </summary>
    /// <param name="index">
    /// Index of the gesture to return
    /// </param>
    /// <returns>
    /// Returns a gesture
    /// </returns>
    public GREGesture GetGesture(int index)
    {
        foreach (GREGesture gesture in gestureLibrary)
        {
            if (gesture.Index == index)
            {
                return gesture;
            }
        }
		
        return null;
    }	

    /// <summary>
    /// Gets a gesture by name 
    /// </summary>
    /// <param name="index">
    /// Name of the gesture to return (case sensitive)
    /// </param>
    /// <returns>
    /// Returns a gesture
    /// </returns>	
    public GREGesture GetGesture(string name)
    {
        name = name.Trim();
		
        foreach (GREGesture gesture in gestureLibrary)
        {
            if (gesture.Name == name)
            {
                return gesture;
            }
        }
		
        return null;
    }
}