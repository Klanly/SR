//-----------------------------------------------------------------
//  Copyright 2011 Layne Bryant   Midnight Ware
//
//  All rights reserved
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// This class allows editing of a gesture, modifying the strokes and stroke count 
/// </summary>
public class GestureEditor : MonoBehaviour
{
	/// <summary>
	/// the mode determines what elements are drawn in OnGUI 
	/// </summary>
	private enum MODE
	{
		Normal,
		EditName,
		ConfirmDeleteStroke,
		ConfirmDeleteStrokeVariation,
		ConfirmClearGesture,
		ConfirmReloadGesture
	}
	
	/// <summary>
	/// GUI Style used for aligned label text 
	/// </summary>
	private GUIStyle _labelStyle;
	
	/// <summary>
	/// GUI Style used for centering text input boxes 
	/// </summary>
	private GUIStyle _centeredTextAreaStyle;
	
	/// <summary>
	/// the scroll list of strokes and variations 
	/// </summary>
	private Vector2 _scrollSavedStrokesVector = Vector2.zero;
	
	/// <summary>
	/// the window around the strokes and variations 
	/// </summary>
	private Rect _savedStrokesWindowRect = new Rect(400, 115, 230, 340);
	
	/// <summary>
	/// the popup box that confirms deletions and reloads 
	/// </summary>
	private Rect _popupQuestionRect = new Rect(20, 100, 350, 100);
	
	/// <summary>
	/// the gesture recognition engine 
	/// </summary>
	private GREngine _grEngine;
	
	/// <summary>
	/// the mode that the editor is in, determining what is drawn in OnGUI 
	/// </summary>
	private MODE _mode;
	
	/// <summary>
	/// the amount of time left to display a message 
	/// </summary>
	private float _infoTimeLeft = 0;
	
	/// <summary>
	/// the message to display for feedback 
	/// </summary>
	private string _information;
	
	/// <summary>
	/// the path of the library file 
	/// </summary>
	private string _libraryPath;
	
	/// <summary>
	/// the name of the gesture being edited 
	/// </summary>
	private string _gestureName;
	
	/// <summary>
	/// index of the gesture in the library 
	/// </summary>
	private int _gestureIndex;
	
	/// <summary>
	/// the new name of the gesture (used when changing the name) 
	/// </summary>
	private string _newGestureName;
	
	/// <summary>
	/// the current gesture being edited 
	/// </summary>
	private GREGesture _currentGesture;
	
	/// <summary>
	/// the transform of the object holding the image to be traced 
	/// </summary>
	private Transform _traceImageTransform;
	
	/// <summary>
	/// the original scale of the image being traced (used to reset the scale if desired) 
	/// </summary>
	private Vector3 _originalTraceImageScale;
	
	/// <summary>
	/// if the gesture needs saving, we will flash the save button 
	/// </summary>
	private bool _needsSaved = false;

	/// <summary>
	/// if flashing the save button, then flash the color on 
	/// </summary>
	private bool _flashSaveOn;
	
	/// <summary>
	/// the amount of time remaining until toggling the save flash state 
	/// </summary>
	private float _flashSaveTimeLeft;
	
	/// <summary>
	/// the stroke number used for selecting and inserting strokes
	/// </summary>
	private int _strokeNumber;
	
	/// <summary>
	/// the variation number of the stroke used for selecting 
	/// </summary>
	private int _variationNumber;
	
	/// <summary>
	/// the string representation of the stroke variation that is selected 
	/// </summary>
	private string _variationString;
	
	/// <summary>
	/// private accessor to _mode that changes the editor layout when editing the gesture name 
	/// </summary>	
	private MODE Mode
	{
		set
		{
			_mode = value;
			
			switch (_mode)
			{
			case MODE.Normal:
				break;
				
			case MODE.EditName:
				_newGestureName = _gestureName;
				ResetGesture();				
				break;
			}
		}
	}
	
	/// <summary>
	/// the touchpad used to capture input 
	/// </summary>
	public TouchPad touchPad;
	
	/// <summary>
	/// the transform of the object that shows where we are drawing 
	/// </summary>
	public Transform drawTransform;
	
	/// <summary>
	/// the particle emitter that shows where we've drawn 
	/// </summary>
	public ParticleEmitter emitter;
	
	/// <summary>
	/// the particle animator that sets the color of the particles 
	/// </summary>
	public ParticleAnimator particleAnimator;
	
	/// <summary>
	/// the color of the particles being drawn 
	/// </summary>
	public Color particleColor;
	
	/// <summary>
	/// the optional image to trace 
	/// </summary>
	public GameObject traceImageObject;
	
	/// <summary>
	/// the amount to change the scale when zooming 
	/// </summary>
	public Vector3 scaleDelta;
	
	/// <summary>
	/// the amount to move the trace image when moving
	/// </summary>
	public Vector3 moveDelta;
	
	/// <summary>
	/// the amount to rotate the trace image when rotating 
	/// </summary>
	public float rotateDelta;
	
	/// <summary>
	/// the amount of time being flashes on the save button when needing to save 
	/// </summary>
	public float flashSaveTime;
	
	/// <summary>
	/// Initializes the editor 
	/// </summary>
	void Start()
	{
		// get the library and gesture
		_libraryPath = PlayerPrefs.GetString("LibraryPath", "");
		_gestureName = PlayerPrefs.GetString("GestureName", "");
		_gestureIndex = PlayerPrefs.GetInt("GestureIndex", 0);

		// set up the particle colors
		Color [] modifiedParticleColors = particleAnimator.colorAnimation;
		modifiedParticleColors[0] = particleColor;
		modifiedParticleColors[1] = particleColor;
		modifiedParticleColors[2] = particleColor;
		modifiedParticleColors[3] = particleColor;
		modifiedParticleColors[4] = particleColor;
		particleAnimator.colorAnimation = modifiedParticleColors;
		
		// initialize the image to trace
		traceImageObject.SetActiveRecursively(false);
		_traceImageTransform = traceImageObject.transform;
		Texture texture = ((MeshRenderer)(traceImageObject.GetComponent(typeof(MeshRenderer)))).material.mainTexture;
		_originalTraceImageScale = new Vector3(texture.width, texture.height, 1);
		_traceImageTransform.localScale = _originalTraceImageScale;
		
		// initialize the gesture recognition
		_grEngine = new GREngine();
		if (!_grEngine.LoadLibraryFromPath(_libraryPath))
		{
			_information = "Failed to load library at path: " + _libraryPath;
			_infoTimeLeft = 5.0f;
		}
		
		// initialize the current gesture being recorded
		_currentGesture = new GREGesture(_gestureName);
		_currentGesture.Replace(_grEngine.GetGesture(_gestureIndex));
		
		// initialize the input system
		touchPad.SetTouchPadDelegate(TouchPadDelegate);		
		
		// set the editor to normal state
		Mode = MODE.Normal;
	}
	
	/// <summary>
	/// decrements the information time left and flashes the save button 
	/// </summary>
	void Update()
	{
		if (_infoTimeLeft > 0)
		{
			_infoTimeLeft -= Time.deltaTime;
			if (_infoTimeLeft <= 0)
			{
				_infoTimeLeft = 0;
				_information = "";
			}
		}
		
		if (_needsSaved)
		{
			_flashSaveTimeLeft -= Time.deltaTime;
			if (_flashSaveTimeLeft <= 0)
			{
				_flashSaveOn = !_flashSaveOn;
				_flashSaveTimeLeft = flashSaveTime;
			}
		}
	}
	
	/// <summary>
	/// draws the editor's layout 
	/// </summary>
	void OnGUI()
	{
		// set the left label style
		_labelStyle = GUI.skin.GetStyle("Label");
		_labelStyle.alignment = TextAnchor.UpperLeft;

		// set the centered text input box style
		_centeredTextAreaStyle = GUI.skin.GetStyle("TextArea");
		_centeredTextAreaStyle.alignment = TextAnchor.UpperCenter;		
		
		// button: go back to the library
		if (GUI.Button(new Rect(10, 5, 70, 30), "Library"))
		{
			Application.LoadLevel("LibraryBrowser");
			return;
		}
		
		// button: toggle the trace image on or off
		if (GUI.Button(new Rect(85, 5, 70, 30), "Image"))
		{
			traceImageObject.SetActiveRecursively(!traceImageObject.active);
		}
		
		// button: clear out the strokes for the gesture
		if (GUI.Button(new Rect(160, 5, 70, 30), "Clear"))
		{
			Mode = MODE.ConfirmClearGesture;
		}
		
		// button: reload the gesture from the library
		if (GUI.Button(new Rect(235, 5, 70, 30), "Reload"))
		{
			if (_needsSaved)
			{
				// if we have made changes, make sure the user actually wants to overwrite the changes
				Mode = MODE.ConfirmReloadGesture;
			}
			else
			{
				// reload the gesture from the library and turn off the needs saving flag
				_grEngine.LoadLibraryFromPath(PlayerPrefs.GetString("LibraryPath", "").Trim());
				_currentGesture.Replace(_grEngine.GetGesture(_gestureName));
				FlagSave(false);
			}
		}
		
		// if the gesture needs saving, then we will flash the save button
		if (_needsSaved)
		{
			// if the flash is on, then we will draw two extra buttons to darken the save button
			if (_flashSaveOn)
			{
				if (GUI.Button(new Rect(310, 5, 70, 30), "Save"))
					Save();
				if (GUI.Button(new Rect(310, 5, 70, 30), "Save"))
					Save();
			}
			
			// button: save the gesture
			if (GUI.Button(new Rect(310, 5, 70, 30), "Save"))
			{
				Save();
			}
		}
		
		// label: show the name field for the gesture
		GUI.Label(new Rect(400, 5, 40, 20), "Name:");
		
		// format the editor depending on the current _mode
		switch (_mode)
		{
		case MODE.Normal:
			// we are not changing the name of the gesture, so show the name on a button
			
			// button: change the gesture name
			if (GUI.Button(new Rect(445, 5, 185, 23), _gestureName))
			{
				Mode = MODE.EditName;
			}
			break;
			
		case MODE.EditName:
			// we are changing the name of the gesture, so display a text field
			
			// textfield: get the new gesture name
			_newGestureName = GUI.TextField(new Rect(445, 5, 185, 23), _newGestureName, _centeredTextAreaStyle);
			
			// button: change the name
			if (GUI.Button(new Rect(505, 30, 60, 20), "OK"))
			{
				_gestureName = _newGestureName;
				_newGestureName = "";
				
				_currentGesture.Name = _gestureName;
				
				// the gesture needs saved since the name was changed
				FlagSave(true);
				
				// switch editor back to normal mode
				Mode = MODE.Normal;
			}
			
			// button: cancel the name change
			if (GUI.Button(new Rect(570, 30, 60, 20), "Cancel"))
			{
				Mode = MODE.Normal;
			}
			break;
		}
		
		// if the gesture has been started, the show the current strokes
		if (_grEngine.GestureBegan)
		{
			// box: border for the current gestures
			GUI.Box(new Rect(400, 44, 230, 65), "");
			GUI.Box(new Rect(400, 44, 230, 20), "Current Strokes");
			
			// if the stroke has been started, show the current move vectors
			if (_grEngine.StrokeBegan)
			{
				string strokesString = _grEngine.GetCurrentStrokesString() + " ; " + _grEngine.currentStroke.ToString();
				if (strokesString.Substring(0, 3) == " ; ")
				{
					strokesString = strokesString.Substring(3, strokesString.Length-3);
				}
				
				// label: the current stokes
				GUI.Label(new Rect(410, 64, 210, 20), strokesString);
			}
			else
			{
				// label: the drawn strokes
				GUI.Label(new Rect(410, 64, 210, 20), _grEngine.GetCurrentStrokesString());
			}
			
			// if the current stroke count matches the gesture's stroke count, then we show the add button
			if (_grEngine.currentStrokes.Count == _currentGesture.Strokes.Count || _currentGesture.Strokes.Count == 0)
			{
				// button: add these strokes to the gesture's variations
				if (GUI.Button(new Rect(480, 85, 70, 20), "Add"))
				{
					string error = "";
					
					// add the variation
					if (!_currentGesture.AddVariation(_grEngine.currentStrokes, ref error))
					{
						// there was a problem, most likely the strokes already matched
						_information = error;
						_infoTimeLeft = 5.0f;
					}
					else
					{
						// the strokes were added to the variations, so now we need inform the user the gesture needs saving
						_flashSaveOn = true;
						_needsSaved = true;
						_flashSaveTimeLeft = flashSaveTime;
						
						_information = "Added " + _grEngine.GetCurrentStrokesString() + " to gesture";
						_infoTimeLeft = 5.0f;
					}
	
					// reset the gesture to start a new drawing sequence
					ResetGesture();
				}
			}
			else if (_grEngine.currentStrokes.Count > _currentGesture.Strokes.Count)
			{
				// label: we drew too many strokes, so we can't add these strokes to our gesture's variations
				GUI.Label(new Rect(440, 85, 110, 20), "Too many strokes!");
			}
			
			// button: cancel the current strokes and reset the gesture
			if (GUI.Button(new Rect(555, 85, 70, 20), "Cancel"))
			{
				ResetGesture();
			}
		}
		
		// if there is time left, show the message
		if (_infoTimeLeft > 0)
		{
			// label: information giving feedback to the user
			GUI.Label(new Rect(15, 458, 610, 20), _information);
		}
		
		// if the trace image is active, we will show controls to manipulate the image
		if (traceImageObject.active)
		{
			int x, y;
			int width, height;

			y = 47;
			height = 20;
			
			x = 15;
			width = 50;
			
			// button: reset will set the trace image back to its original parameters
			if (GUI.Button(new Rect(x, y, width, height), "Reset"))
			{
				_traceImageTransform.localPosition = Vector3.zero;
				_traceImageTransform.localRotation = Quaternion.identity;
				_traceImageTransform.localScale = _originalTraceImageScale;
			}
			
			x += width + 3;
			width = 50;
			
			// button: fit will change the width and height (proportionally) to fit entirely in the drawing window
			if (GUI.Button(new Rect(x, y, width, height), "Fit"))
			{
				_traceImageTransform.localPosition = Vector3.zero;
				
				if (_originalTraceImageScale.x > _originalTraceImageScale.y)
				{
					_traceImageTransform.localScale = new Vector3(380.0f, _originalTraceImageScale.y * (380.0f / _originalTraceImageScale.x), 1.0f);
				}
				else
				{
					_traceImageTransform.localScale = new Vector3(_originalTraceImageScale.x * (380.0f / _originalTraceImageScale.y), 380.0f, 1.0f);
				}
			}

			x += width + 3;
			width = 20;
			
			// button: zoom in on the trace image
			if (GUI.Button(new Rect(x, y, width, height), "+"))
			{
				_traceImageTransform.localScale += scaleDelta;
			}
			
			x += width + 3;
			
			// button: zoom out on the trace image
			if (GUI.Button(new Rect(x, y, width, height), "-"))
			{
				_traceImageTransform.localScale -= scaleDelta;
			}			
			
			

			
			x = 180;
			width = 35;
			
			// button: rotate the trace image in the positive direction
			if (GUI.Button(new Rect(x, y, width, height), "R+"))
			{
				_traceImageTransform.localEulerAngles += new Vector3(0, 0, rotateDelta);
			}
			
			x += width + 3;
			
			// button: rotate the trace image in the negative direction
			if (GUI.Button(new Rect(x, y, width, height), "R-"))
			{
				_traceImageTransform.localEulerAngles += new Vector3(0, 0, -rotateDelta);
			}

			
			
			
			x = 270;
			width = 25;
			
			// button: move the trace image to the left
			if (GUI.Button(new Rect(x, y, width, height), "<"))
			{
				_traceImageTransform.localPosition += new Vector3(moveDelta.x, 0, 0);
			}
			
			x += width + 3;
			
			// button: move the trace image to the right
			if (GUI.Button(new Rect(x, y, width, height), ">"))
			{
				_traceImageTransform.localPosition += new Vector3(-moveDelta.x, 0, 0);
			}

			x += width + 3;
			
			// button: move the trace image up
			if (GUI.Button(new Rect(x, y, width, height), @"/\"))
			{
				_traceImageTransform.localPosition += new Vector3(0, -moveDelta.y, 0);
			}

			x += width + 3;
			
			// button: move the trace image down
			if (GUI.Button(new Rect(x, y, width, height), @"\/"))
			{
				_traceImageTransform.localPosition += new Vector3(0, moveDelta.y, 0);
			}
		}
		
		// if we are trying to confirm the user's action, show a popup window, otherwise show the saved strokes
		switch (_mode)
		{
		case MODE.ConfirmDeleteStroke:
		case MODE.ConfirmDeleteStrokeVariation:
			// make sure the user meant to delete the stroke or stroke variation
			_popupQuestionRect = GUI.Window(0, _popupQuestionRect, PopupQuestionWindow, "Confirm Delete");
			break;
			
		case MODE.ConfirmClearGesture:
			// make sure the user meant to clear out the gesture's strokes
			_popupQuestionRect = GUI.Window(0, _popupQuestionRect, PopupQuestionWindow, "Confirm Clear");
			break;
			
		case MODE.ConfirmReloadGesture:
			// make sure the user meant to reload the gesture from the library
			_popupQuestionRect = GUI.Window(0, _popupQuestionRect, PopupQuestionWindow, "Confirm Reload");
			break;

		case MODE.Normal:
			// show the saved strokes for this gesture
			_savedStrokesWindowRect = GUI.Window(0, _savedStrokesWindowRect, SavedStrokesWindow, "Gesture Strokes (" + _currentGesture.Strokes.Count.ToString() + ")");			
			break;
		}
	}
	
	/// <summary>
	/// The window to show the list of strokes and their variations for the gesture. This is an extension of OnGUI that is specific to the normal mode
	/// </summary>
	private void SavedStrokesWindow(int windowID)
	{
		// only show this window if there are strokes in the gesture
		if (_currentGesture.Strokes.Count > 0)
		{
			// scrollview: contain all the strokes and variations in the scroll 
			_scrollSavedStrokesVector = GUI.BeginScrollView(new Rect (10,20,_savedStrokesWindowRect.width-20,_savedStrokesWindowRect.height-30), _scrollSavedStrokesVector, new Rect(0, 0, _savedStrokesWindowRect.width-40, 3000));
			
			int y = 5;
			_strokeNumber = 0;
			_variationNumber = 0;
			
			// if the current gesture is active
			if (_currentGesture != null)
			{
				// if the current gesture has strokes
				if (_currentGesture.Strokes.Count > 0)
				{
					// button: insert a stroke before the first stroke that is saved.
					// insert stroke should only be used if you are needing to add a new set of variations to a gesture,
					// for instance, if you wanted to turn a minus sign into a plus sign
					if (GUI.Button(new Rect(40, y, _savedStrokesWindowRect.width-110, 20), "Insert Stroke"))
					{
						// insert an empty stroke
						_currentGesture.InsertStroke(_strokeNumber);
						
						// gesture needs saving
						FlagSave(true);
					}				
					
					y += 30;			
				}
				
				// loop through the strokes and display them
				foreach (List<GREStroke> variations in _currentGesture.Strokes)
				{
					// box: box containing the stroke variations for this stroke
					GUI.Box(new Rect(2, y, _savedStrokesWindowRect.width-44, (variations.Count+2) * 20), "");
					
					// box: box surrounding the stroke header
					GUI.Box(new Rect(2, y, _savedStrokesWindowRect.width-44, 27), "");
	
					y += 5;
					
					// label: stroke header
					GUI.Label(new Rect(5, y, _savedStrokesWindowRect.width-10, 20), "Stroke " + (_strokeNumber + 1).ToString());
					// button: the button to delete the stroke (and all of its variations)
					if (GUI.Button(new Rect(_savedStrokesWindowRect.width-75, y, 30, 20), "X"))
					{
						// confirm the deletion of the stroke
						Mode = MODE.ConfirmDeleteStroke;
						
						// we break here because the collection was modified and will crash the foreach loop.
						// the OnGUI call next frame will redraw the elements.
						return;
					}
					
					y += 25;
					
					_variationNumber = 0;
					
					// loop through each variation of the stroke, displaying it
					foreach (GREStroke variation in variations)
					{
						// if the variation is too long to fit, add elipses (...)
						if (variation.StrokeString.Length > 18)
						{
							// label: shortened variation with elipses
							GUI.Label(new Rect(20, y, _savedStrokesWindowRect.width-85, 20), variation.StrokeString.Substring(0, 15) + "...");
						}
						else
						{
							// label: the stroke variation
							GUI.Label(new Rect(20, y, _savedStrokesWindowRect.width-85, 20), variation.StrokeString);
						}
						
						// button: delete the stroke variation
						if (GUI.Button(new Rect(_savedStrokesWindowRect.width-75, y, 30, 20), "X"))
						{
							// set the variation string so we can use it in the popup confirmation window
							_variationString = variation.StrokeString;
							
							// confirm deleting the stroke variation
							Mode = MODE.ConfirmDeleteStrokeVariation;
							
							// we break here because the collection was modified and will crash the foreach loop.
							// the OnGUI call next frame will redraw the elements.
							return;
						}
						
						y += 20;
						
						_variationNumber++;
					}
					
					y += 20;
					
					_strokeNumber++;
					
					// button: insert a stroke before the next stroke (if this is the last stroke, then insert a stroke at the end)
					if (GUI.Button(new Rect(40, y, _savedStrokesWindowRect.width-110, 20), "Insert Stroke"))
					{
						// insert an empty stroke
						_currentGesture.InsertStroke(_strokeNumber);
						
						// gesture needs saving
						FlagSave(true);
						
						// we break here because the collection was modified and will crash the foreach loop.
						// the OnGUI call next frame will redraw the elements.
						break;
					}				
					
					y += 30;
				}
			}
			
			// scrollview: wrap the scroll view here
			GUI.EndScrollView();	
		}
	}	
	
	/// <summary>
	/// Popup window that asks the user to confirm an action. This is an extension of OnGUI.
	/// </summary>
	private void PopupQuestionWindow(int windowID)
	{	
		string message = "";
		
		// set the confirmation message depending on what action the user performed
		switch (_mode)
		{
		case MODE.ConfirmDeleteStroke:
			message = "If you delete stroke [ " + (_strokeNumber+1).ToString() + " ], all of its variations will be deleted also. Are you sure you want to do this?";
			break;
			
		case MODE.ConfirmDeleteStrokeVariation:
			message = "Are you sure you want to delete variation [ " + _variationString + " ] from stroke [ " + (_strokeNumber+1).ToString() + " ]?";
			break;
			
		case MODE.ConfirmClearGesture:
			message = "Are you sure you want to remove all of this gestures strokes?";
			break;
			
		case MODE.ConfirmReloadGesture:
			message = "If you reload the gesture, any new strokes will be lost. Are you sure you want to do this?";
			break;
		}
		
		// label: confirmation message
		_labelStyle.alignment = TextAnchor.UpperCenter;
		GUI.Label(new Rect(10, 20, _popupQuestionRect.width - 20, _popupQuestionRect.height - 35), message, _labelStyle);
		_labelStyle.alignment = TextAnchor.UpperLeft;
		
		// button: confirm the action
		if (GUI.Button(new Rect(_popupQuestionRect.width-180,_popupQuestionRect.height-30,80,20), "Yes"))
		{
			// switch what happens depending on the action the user performed
			switch (_mode)
			{
			case MODE.ConfirmDeleteStroke:
				// remove the stroke and its variations from the gesture and set the gesture to needing saved
				_currentGesture.RemoveStroke(_strokeNumber);
				FlagSave(true);
				break;
				
			case MODE.ConfirmDeleteStrokeVariation:
				// remove the stroke variation and flag saving
				_currentGesture.RemoveVariation(_strokeNumber, _variationNumber);
				FlagSave(true);
				break;
				
			case MODE.ConfirmClearGesture:
				// clear all the strokes and variations from the gesture
				_currentGesture.Dispose();
				FlagSave(true);
				break;
				
			case MODE.ConfirmReloadGesture:
				// reload the gesture from the library and turn off needs saving
				_grEngine.LoadLibraryFromPath(PlayerPrefs.GetString("LibraryPath", "").Trim());
				_currentGesture.Replace(_grEngine.GetGesture(_gestureName));
				FlagSave(false);
				break;	
			}
			
			Mode = MODE.Normal;
		}
		
		// button: cancel the action that prompted this confirmation
		if (GUI.Button(new Rect(_popupQuestionRect.width-90,_popupQuestionRect.height-30,80,20), "No"))
		{
			Mode = MODE.Normal;
		}
	}	
	
	/// <summary>
	/// The input delegate that handles the mouse movements 
	/// </summary>
	/// <param name="touchEvent">
	/// The event (touchdown, touchmove, etc.) captured from the touchpad
	/// </param>
	/// <param name="touchPosition">
	/// The position of the touch
	/// </param>
	private void TouchPadDelegate(TouchPad.TOUCH_EVENT touchEvent, Vector3 touchPosition)
	{
		// only capture events if we are in the normal editor mode.
		// this prevents accidentally drawing when answering popup confirmations, for instance
		if (_mode == MODE.Normal)
		{
			switch (touchEvent)
			{
			case TouchPad.TOUCH_EVENT.TouchDown:
				// start a new stroke and turn on the particle emitter
				_infoTimeLeft = 0;
				_grEngine.BeginStroke(touchPosition.x, touchPosition.y);
				drawTransform.position = touchPosition;
				emitter.emit = true;
				break;
				
			case TouchPad.TOUCH_EVENT.TouchMove:
				// move the stroke
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
	}	
	
	/// <summary>
	/// Resets the currently drawn strokes and turns off the particles 
	/// </summary>
	public void ResetGesture()
	{
		_grEngine.ResetEngine();
		emitter.emit = false;
		emitter.ClearParticles();
	}	
	
	/// <summary>
	/// Sets the interface to flash the save button or not
	/// </summary>
	/// <param name="save">
	/// Whether the flashing should occur
	/// </param>
	private void FlagSave(bool save)
	{
		if (save)
		{
			_needsSaved = true;
			_flashSaveOn = true;
			_flashSaveTimeLeft = flashSaveTime;
		}
		else
		{
			_needsSaved = false;
			_flashSaveOn = false;
		}
	}
	
	/// <summary>
	/// Saves the gesture into the library 
	/// </summary>
	private void Save()
	{
		// WARNING! Comments will be overwritten and lost when saving!
		
		// replace the library gesture with the current gesture
		_gestureIndex = _grEngine.ReplaceGesture(PlayerPrefs.GetInt("GestureIndex", -1), _currentGesture.ToString());
		PlayerPrefs.SetString("GestureName", _gestureName);
		PlayerPrefs.SetInt("GestureIndex", _gestureIndex);
		
		// write the gestures back into the library file
		if (!_grEngine.SaveLibraryToPath(_libraryPath))
		{
			Debug.Log(_libraryPath);
			_information = "Failed to save library to path: " + _libraryPath;
			_infoTimeLeft = 5.0f;
		}
		
		// set needs saving back off
		FlagSave(false);
	}
}