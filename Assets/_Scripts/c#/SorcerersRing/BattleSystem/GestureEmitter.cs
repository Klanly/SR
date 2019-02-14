using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class shows how to use the Gesture Recognition Engine. It uses a flat orthographic drawing 
/// and feedback plane that sits on top of a perspective 3d view. To use, just draw gestures in the
/// box at the bottom right of the screen. If a gesture is matched, the name of the gesture will
/// be displayed in a rotating 3d text object. If no match is found, a string at the top of the screen
/// will give the user feedback.
/// 
/// The example uses the touchpad class found in the Common Scripts folder. This touchpad class is
/// a simple way to gather mouse and finger data, but can be replaced by more sophisticated means.
/// For example, you could use a button or other object from your favorite 3rd party 2d input toolkit.
/// 
/// This example tries to match a gesture half a second after the user lifts their finger / mouse to 
/// show you that you can match gestures at any time during drawing (not just after clicking a button
/// like in the Gesture Editor scenes). A countdown timer is started when t                                 he user lifts their finger / 
/// mouse. If the timer expires before the users starts a new stroke, the engine looks for a match. If
/// the user starts a new stroke before the timer expires, the timer is reset and the matching is
/// postponed. Care must be taken to balance this speed with the user's preferences. Having a low 
/// countdown time will result in matching occuring too often, before the user is done. Having a high
/// countdown time will cause the user to have to wait longer before drawing a new gesture. If the
/// timing is a significant issue, it may be wise to have a drawing complete button like in the
/// TestGestures example scene from the Gesture Editor. This way it is explicit when the user is
/// done drawing and the engine is ready to match.
/// 
/// A background sprite is provided to display the drawing bounds, but this is not necessary. Particles
/// are also used to show where the user has drawn, but they are not necessary either. These
/// mechanisms provide visual feedback to the user, but some games may be better suited to having 
/// an invisible boundary and/or drawing lines.
/// 
/// This example uses some alphabet gestures: A, B, C, D, E. You can provide your own gesture library
/// by setting the gestureData member in the Unity editor.
/// </summary>
public class GestureEmitter : MonoBehaviour
{
	/// <summary>
	/// Gesture Recognition Engine instance
	/// </summary>
	private GREngine _grEngine;
	
	/// <summary>
	/// Time left before we check the gesture
	/// </summary>
	private float _countdownTimeLeft;
	
	/// <summary>
	/// Time left to show the feedback to the user
	/// </summary>
	private float _feedbackTimeLeft;
	
	/// <summary>
	/// Ortho camera used to place the touchpad in the lower right corner
	/// </summary>
	public Camera guiCamera;
	
	/// <summary>
	/// Touchpad object used to capture input. You can use buttons or other 
	/// input objects from 3rd party tools instead the touchpad.
	/// </summary>
	public TouchPad touchPad;
	
	/// <summary>
	/// The transform to move with the touches / mouse
	/// </summary>
	public Transform drawTransform;
	
	/// <summary>
	/// The particle emitter to show where we have moved
	/// </summary>
	public ParticleEmitter emitter;
	
	/// <summary>
	/// Transform of the sprite so that we can scale it to fit the touchpad
	/// </summary>
	public Transform spriteTransform;
	
	/// <summary>
	/// Text to show you that a gesture was not matched
	/// </summary>
	public GUIText feedbackText;
	
	/// <summary>
	/// A reference to the GestureResults object (all it does is spin the results)
	/// </summary>
	//public GestureResults gestureResults;
	
	/// <summary>
	/// The gesture data to match against
	/// </summary>
	public TextAsset gestureData;
	
	/// <summary>
	/// Offset of the particles from the touchpad (usually only along the z axis)
	/// </summary>
	public Vector3 particlesOffset;
	
	/// <summary>
	/// The amount of time (in seconds) to wait after the user lifts the mouse / finger
	/// before checking for a match
	/// </summary>
	public float autoCheckGestureCountdown;
	
	public bool shouldEnable
	{
		set;get;
	}
	
	public enum Gesture
	{
		kFire,
		kWater,
		kLightning,
		kEarth,
		Ignite, //element attack 1
		Daze, //element attack 2
		Drain, //element attack 3
		LeechSeed, //element attack 4
		Burst,
		kInvalid,
		kSmash
	}
	
	static GestureEmitter()
	{
		Gesture[] nGestures = {Gesture.kEarth, Gesture.kFire,  Gesture.kWater, Gesture.kLightning};
		normalGestures = new List<Gesture>(nGestures);
		
		Gesture[] sGestures = {Gesture.Ignite, Gesture.Daze, Gesture.Drain, Gesture.LeechSeed};
		specialGestures = new List<Gesture>(sGestures);
	}
	public static List<Gesture> normalGestures;
	
	public static List<Gesture> specialGestures;
	
	string guiOut;
//	float stationaryTouchBegan=0.0f;
//	float stationaryTouchTime;
	
	public delegate void GestureDelegate(Gesture gestureName);
	
	public delegate void FingerDownDelegate(bool yesNo);
	public delegate void FingerUpDelegate(float time);
	
	private FingerDownDelegate _fingerDownDelegate;
	
	private GestureDelegate myDelegate;
	
	public void SetDelegate(GestureDelegate del)
	{
		this.myDelegate=del;
	}
	
	private void testSetDelegate(){
		this.myDelegate = delegate(Gesture aGesture) {
			//Debug.Log(aGesture.ToString());
		};
	}//for testing purposes
	
	public void setTouchParameters(float originX, float originY, float width, float height)
	{
//		touchPad.setTouchPadSize(width, height);
//		spriteTransform.localScale = new Vector3(width, height, 1.0f);
//		touchPad.transform.localPosition = new Vector3(originX, originY, 0);
	}
	
	
	public void SetFingerDownDelegate(FingerDownDelegate del)
	{
		_fingerDownDelegate = del;
	}
	
	public void SetNormalLibrary(bool yesNo)
	{
		_grEngine.SetNormalLibrary(yesNo);
	}
	
	void Awake()
	{
		// Instantiate the GRE and load in the gesture library from a text asset
		// Note: this example loads from a text asset, whereas the gesture
		// editor loads from a file path
		_grEngine = new GREngine();
		_grEngine.LoadLibraryFromString(gestureData.text);
		
		// set the camera's orthographic size based on half the screen height
		//guiCamera.orthographicSize = Screen.height / 2.0f;
		
		// move the touchpad to the lower left position of the screen
		setTouchParameters(Screen.width*0.1f,-Screen.height*0.09f,Screen.width* 0.75f,Screen.height* 0.75f/* *0.4f */);
		
		//setGesturePosition(0,0);
		// set the touchpad delegate to a function in this class
		touchPad.SetTouchPadDelegate(TouchPadDelegate);
		
		// set the sprite's scale to match the touchpad's boundary
		
		//setGestureSize(Screen.width,Screen.height);
		// reset the countdown timer
		_countdownTimeLeft = 0;
		
		// reset the feedback timer
		_feedbackTimeLeft = 0;
		
		//this.testSetDelegate(); //for testing purpose
	}
	
	void Update()
	{
		// if we are counting down
		if (_countdownTimeLeft > 0)
		{
			// reduce the countdown timer by the time between frames
			_countdownTimeLeft -= Time.deltaTime;
			
			// if we have exceeded the countdown timer
			if (_countdownTimeLeft <= 0)
			{
				// reset the timer
				_countdownTimeLeft = 0;
				
				// check for a match
				//CheckGesture();
			}
		}
		
		// if we are showing feedback
		if (_feedbackTimeLeft > 0)
		{
			// reduce countdown
			_feedbackTimeLeft -= Time.deltaTime;
			if (_feedbackTimeLeft <= 0)
			{
				// hide feedback
				_feedbackTimeLeft = 0;
				feedbackText.text = "";
			}
		}
	}
		                             
	public void TouchPadDelegate(TouchPad.TOUCH_EVENT touchEvent, Vector3 touchPosition)
	{
//		if(enabled)
//		{
//			Debug.Log("TOUCH DEL!!!");
			
			switch (touchEvent)
			{
			case TouchPad.TOUCH_EVENT.TouchDown:
				if(_fingerDownDelegate != null)
				{
					_fingerDownDelegate(true);
				}
				// begin a new stroke and turn on the particle emitter
				_grEngine.BeginStroke(touchPosition.x, touchPosition.y);
				drawTransform.position = touchPosition + particlesOffset;
				emitter.emit = true;
				
				// turn off the countdown timer since we don't want to match while we are drawing
				_countdownTimeLeft = 0;
				break;
				
			case TouchPad.TOUCH_EVENT.TouchMove:
				// move the current stroke
				_grEngine.MoveStroke(touchPosition.x, touchPosition.y);
				drawTransform.position = touchPosition + particlesOffset;
				break;
				
			case TouchPad.TOUCH_EVENT.TouchUp:
			//	Debug.Log("--------------TouchUPCalleddddd---------");
				
				if(!touchPad.didGoStationary)
				{
					_grEngine.EndStroke();
					CheckGesture();

				}
				_grEngine.ResetEngine();
					emitter.emit = false;
					emitter.ClearParticles();
				// turn on the countdown timer when the mouse / finger has stopped drawing
				//_countdownTimeLeft = autoCheckGestureCountdown;
				if(_fingerDownDelegate != null)
				{
					_fingerDownDelegate(false);
				}
				
				break;
				
			case TouchPad.TOUCH_EVENT.TouchStationary:
//				Debug.Log("--------------TouchSTATIONARYCalleddddd---------");
				_grEngine.EndStroke();
				CheckGesture();
				
				break;
			}		
		//}
	}
	
	void OnEnable()
	{
		if(touchPad != null)
		{
			touchPad.gameObject.SetActive(true);
			touchPad.enabled = true;
		}
	}
	
	void OnDisable()
	{
		if(touchPad != null)
		{
			touchPad.gameObject.SetActive(false);
			touchPad.enabled = false;
		}
	}

	private void generateGesture(string gestureName)
	{
		if(gestureName.Equals("Fire"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.kFire);
		}
		else if(gestureName.Equals("Water"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.kWater);
		}
		else if(gestureName.Equals("Earth"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.kEarth);
		}
		else if(gestureName.Equals("Lightning"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.kLightning);
		}
		else if(gestureName.Equals("Ignite"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.Ignite);
		}
		else if(gestureName.Equals("Daze"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.Daze);
		}
		else if(gestureName.Equals("Drain"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.Drain);
		}
		else if(gestureName.Equals("LeechSeed"))
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.LeechSeed);
		}
		else
		{
			//gestureResults.ShowResult(gestureName);
			this.myDelegate(Gesture.kInvalid);
		}
	}
	private void CheckGesture()
	{
		// match the gesture to the library
		GREGesture testGesture = _grEngine.MatchCurrentStrokesToGesture();
		
		// display whether the gesture was matched or not
		if (testGesture != null)
		{
			// Match! show the results in a spinning 3d text object
			generateGesture(testGesture.Name);
			
		//	Debug.Log("~~~~~~~GESTURE NAME================="+testGesture.Name);
			// hide the feedback
			feedbackText.text = "";
		}
		else
		{
			// no match, so hide the spinning 3d text object
			//gestureResults.ShowResult("");
			
			// show that there was no match
			feedbackText.text = "No gestures found in library that match this pattern";
			generateGesture("Gesture.kInvalid");
			// set the feedback timer so that the text doesn't constantly show
			_feedbackTimeLeft = 3.0f;
		}
		
		
	}
	
	void OnGUI()
	{
//		GUI.Label(new Rect(100,200,200,200),guiOut);
	}
}
