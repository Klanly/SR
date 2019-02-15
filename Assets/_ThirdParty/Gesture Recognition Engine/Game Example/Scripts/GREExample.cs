using UnityEngine;

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
/// like in the Gesture Editor scenes). A countdown timer is started when the user lifts their finger / 
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
public class GREExample : MonoBehaviour
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
	public GestureResults gestureResults;
	
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
	
	public bool attakAllowed = false;
	
	public BattleManager _battleManager;
	
	void Awake()
	{
		// Instantiate the GRE and load in the gesture library from a text asset
		// Note: this example loads from a text asset, whereas the gesture
		// editor loads from a file path
		_grEngine = new GREngine();
		_grEngine.LoadLibraryFromString(gestureData.text);
		
		// set the camera's orthographic size based on half the screen height
		guiCamera.orthographicSize = Screen.height / 2.0f;
		
		// move the touchpad to the lower left position of the screen
		touchPad.transform.localPosition = new Vector3((Screen.width / 2.0f) - (touchPad.touchPadWidth / 2.0f), (touchPad.touchPadHeight / 2.0f) - (Screen.height / 2.0f), 0);
		// set the touchpad delegate to a function in this class
		touchPad.SetTouchPadDelegate(TouchPadDelegate);
		
		// set the sprite's scale to match the touchpad's boundary
		spriteTransform.localScale = new Vector3(touchPad.touchPadWidth, touchPad.touchPadHeight, 1.0f);

		// reset the countdown timer
		_countdownTimeLeft = 0;
		
		// reset the feedback timer
		_feedbackTimeLeft = 0;
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
				CheckGesture();
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
		if(attakAllowed){
		switch (touchEvent)
		{
		case TouchPad.TOUCH_EVENT.TouchDown:
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
			// end the stroke and turn off the particle emitter
			_grEngine.EndStroke();
			emitter.emit = false;
			
			// turn on the countdown timer when the mouse / finger has stopped drawing
			_countdownTimeLeft = autoCheckGestureCountdown;
			break;
		}	
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
			gestureResults.ShowResult(testGesture.Name);
			//_battleManager.GestureRecieved(testGesture.Name);
			
			// hide the feedback
			feedbackText.text = "";
		}
		else
		{
			// no match, so hide the spinning 3d text object
			gestureResults.ShowResult("");
			
			// show that there was no match
			feedbackText.text = "No gestures found in library that match this pattern";
			
			// set the feedback timer so that the text doesn't constantly show
			_feedbackTimeLeft = 3.0f;
		}
		
		// reset the gesture and particle emitter
		_grEngine.ResetEngine();
		emitter.emit = false;
		emitter.ClearParticles();
	}
	
}
