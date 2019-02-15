//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;

/// <summary>
/// This class tests drawn gestures against a chosen library. The user draws a gesture in the square, then clicks the 
/// "Test" button. If a match is found in the library, the appropriate gesture is returned.
/// </summary>
public class TestGestures : MonoBehaviour
{
	/// <summary>
	/// the name of library that is being tested against 
	/// </summary>
	private string _libraryName;
	
	/// <summary>
	/// gesture recognition engine 
	/// </summary>
	private GREngine _grEngine;
	
	/// <summary>
	/// the gesture returned if a match is found in the library 
	/// </summary>
	private GREGesture _testGesture;
	
	/// <summary>
	/// the message to display when testing a gesture (matched or not matched) 
	/// </summary>
	private string _infoMessage;
	
	/// <summary>
	/// the amount of time remaining for the displayed message 
	/// </summary>
	private float _infoTimeLeft;
	
	
	/// <summary>
	/// the touchpad class that handles user input 
	/// </summary>
	public TouchPad touchPad;
	
	/// <summary>
	/// the object to move when the user moves the mouse 
	/// </summary>
	public Transform drawTransform;
	
	/// <summary>
	/// the particle emitter that shows where the user has drawn 
	/// </summary>
	public ParticleEmitter emitter;
	
	/// <summary>
	/// the particle animator that sets the color of the particles 
	/// </summary>
	public ParticleAnimator particleAnimator;
	
	/// <summary>
	/// the color to draw the particles 
	/// </summary>
	public Color particleColor;
	
	/// <summary>
	/// the amount of time to display messages before they disappear 
	/// </summary>
	public float infoTime;
	
	#region "Gesture Recognition Functions"
	/// <summary>
	/// This sets up the gesture recognition engine and loads the library from a file 
	/// </summary>
	/// <param name="libraryPath">
	/// The path to the library text file
	/// </param>
	private void InitializeGestureRecognition(string libraryPath)
	{
		_grEngine = new GREngine();
		_grEngine.LoadLibraryFromPath(libraryPath);
	}
	
	/// <summary>
	/// This delegate handles input from the touchpad class 
	/// </summary>
	/// <param name="touchEvent">
	/// What event occured (TouchDown, TouchMove, etc)
	/// </param>
	/// <param name="touchPosition">
	/// Where the touch occured
	/// </param>
	private void TouchPadDelegate(TouchPad.TOUCH_EVENT touchEvent, Vector3 touchPosition)
	{
		Debug.Log(touchEvent + " " + touchPosition + " " + Time.frameCount);
		
		switch (touchEvent)
		{
		case TouchPad.TOUCH_EVENT.TouchDown:
			// begin a new stroke and turn on the particle emitter
			_infoTimeLeft = 0;
			_grEngine.BeginStroke(touchPosition.x, touchPosition.y);
			drawTransform.position = touchPosition;
			emitter.emit = true;
			break;
			
		case TouchPad.TOUCH_EVENT.TouchMove:
			// move the current stroke
			_grEngine.MoveStroke(touchPosition.x, touchPosition.y);
			drawTransform.position = touchPosition;
			break;
			
		case TouchPad.TOUCH_EVENT.TouchUp:
			// end the stroke and turn off the particle emitter
			_grEngine.EndStroke();
			emitter.emit = false;
			break;
		}
	}	
	
	/// <summary>
	/// Tests the current strokes against the gesture library 
	/// </summary>
	private void TestGesture()
	{
		// match the gesture to the library
		_testGesture = _grEngine.MatchCurrentStrokesToGesture();
		
		// display whether the gesture was matched or not
		if (_testGesture != null)
		{
			_infoTimeLeft = infoTime;
			_infoMessage = "Matched gesture [ " + _testGesture.Name + " ]";
		}
		else
		{
			_infoTimeLeft = infoTime;
			_infoMessage = "No gestures found in library that match this pattern";
		}
		
		// reset the gesture
		_grEngine.ResetEngine();
		emitter.emit = false;
		emitter.ClearParticles();
	}	
	#endregion
	
	#region "Unity Functions"
	/// <summary>
	/// Initializes the settings for the test 
	/// </summary>
	void Start()
	{
		// get the name of the library from the file
		_libraryName = System.IO.Path.GetFileName(PlayerPrefs.GetString("LibraryPath", "")).Replace(".txt", "");

		// set the color of the particles
		Color [] modifiedParticleColors = particleAnimator.colorAnimation;
		modifiedParticleColors[0] = particleColor;
		modifiedParticleColors[1] = particleColor;
		modifiedParticleColors[2] = particleColor;
		modifiedParticleColors[3] = particleColor;
		modifiedParticleColors[4] = particleColor;
		particleAnimator.colorAnimation = modifiedParticleColors;
		
		// set up the gesture recognition engine
		InitializeGestureRecognition(PlayerPrefs.GetString("LibraryPath", ""));
		
		// set the touchpad's input delegate
		touchPad.SetTouchPadDelegate(TouchPadDelegate);		
	}
	
	/// <summary>
	/// Decrements the information message timer 
	/// </summary>
	void Update()
	{
		if (_infoTimeLeft > 0)
		{
			_infoTimeLeft -= Time.deltaTime;
			if (_infoTimeLeft <= 0)
			{
				_infoTimeLeft = 0;
			}
		}
	}
	
	/// <summary>
	/// Formats the GUI elements 
	/// </summary>
	void OnGUI()
	{
		// button: go back to the library
		if (GUI.Button(new Rect(10, 5, 70, 30), "Library"))
		{
			Application.LoadLevel("LibraryBrowser");
			return;
		}
		
		// label: display the name of this library (centering it)
		GUIStyle style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(200, 7, 230, 20), _libraryName + " Library");
		style.alignment = TextAnchor.UpperLeft;

		// if we are drawing a gesture, then show the current strokes
		if (_grEngine.GestureBegan)
		{
			// box: strokes border
			GUI.Box(new Rect(400, 80, 230, 55), "");
			GUI.Box(new Rect(400, 80, 230, 20), "Current Strokes");
			
			// if we are drawing a stroke, then show the current movement vectors
			if (_grEngine.StrokeBegan)
			{
				string strokesString = _grEngine.GetCurrentStrokesString() + " ; " + _grEngine.currentStroke.ToString();
				if (strokesString.Substring(0, 3) == " ; ")
				{
					strokesString = strokesString.Substring(3, strokesString.Length-3);
				}

				// label: show the strokes currently being drawn
				GUI.Label(new Rect(410, 104, 210, 20), strokesString);
			}
			else
			{
				// label: show the current strokes that have been drawn
				GUI.Label(new Rect(410, 104, 210, 20), _grEngine.GetCurrentStrokesString());
			}
		}		
		
		// button: test the gesture
		if (GUI.Button(new Rect(400, 145, 230, 40), "Test"))
		{
			TestGesture();
		}

		// box: information border
		GUI.Box(new Rect(400, 195, 230, 255), "");
		if (_infoTimeLeft > 0)
		{
			// label: information on whether the gesture matched a library gesture or not
			GUI.Label(new Rect(410, 215, 210, 215), _infoMessage);
		}
	}
	#endregion
}