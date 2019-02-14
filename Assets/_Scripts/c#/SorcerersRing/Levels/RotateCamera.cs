using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	
	public Transform target;
	
	Quaternion tempTargetRot;
	
	public Quaternion originalRotation;
	
	public float camSpeed=0.5f;
	
	public float sensitivity = 0.5f;
	
	public float rotationX = 0F;
    public float rotationY = 0F;
	
	public float sensitivityX = .0002F;
    public float sensitivityY = .0001F;
 
    public float minimumX = -360F;
    public float maximumX = 360F;
 
    public float minimumY = -60F;
    public float maximumY = 60F;
	bool isMoving=false;
	
	public float waitTime=1.0f;
	
	bool isFirstClick = false;
	
	
	public bool invertX = false;
	public bool invertY = false;
	
	public bool startInvertX = false;
	public bool startInvertY = false; 
	
	// Use this for initialization
	void OnEnable () {
		camSpeed=1.0f;
		sensitivity = 0.75f;
		tempTargetRot=target.rotation;
		//tempTarget=target;
		waitTime = 2.0f;
		
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x-10.0f, transform.eulerAngles.y+10.0f, 0);
	}
	
	void OnDisable()
	{
		target.rotation=tempTargetRot;
	}
	
	// Update is called once per frame
	void Update () {
		
	if (target && !GameManager.instance.scaleformCamera.isPaused)
	{
   		rotateCamera();
	}
	}
	
//	float ClampAngle( float angle,  float limit) {
//	  if(angle > 180) {
//	    var angleB = 360 - angle;//360 - 355 = 5;
//	    if (angleB > limit) angle = 360 - limit;
//	    else return angle;
//	  }
//	 
//	  if(angle < 180){
//	    if (angle > limit && angle < 360-limit) angle = limit;
//	    else return angle;
//	  }
//	  return angle;
//	}
					
	float ClampAngle( float angle,  float min, float max) {
		if(angle > 180 && angle < (360 + min))
		{
 			angle= 360 + min;
			return angle;
		}

		else if(angle > max)
		{
 			angle = max;
			return angle;
		}
		else{
			return angle;
		}
		
	}
	void rotateCamera()
	{
		if(Application.isEditor)
		{	
			if(InputWrapper.GetMouseButtonDown(0))
			{
				if(!isFirstClick){
						if(!startInvertY)
						{
							if(transform.rotation.x >= 0){
								rotationY = -transform.localEulerAngles.x;
							}
							else
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
						}
						else
						{
							if(transform.rotation.x >= 0){
								rotationY = (360 -transform.localEulerAngles.x);
							}
							else
							{
								rotationY = -transform.localEulerAngles.x;
							}
						}
						
						
						
						
						if(!startInvertX)
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y;
							}
							else
							{
								rotationX = transform.localEulerAngles.y - 360;
							}
						}
						else
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y - 360;
							}
							else
							{
								rotationX = transform.localEulerAngles.y ;
							}
						}
				}
				
				isMoving=true;
//				Debug.Log("BUTTON DOWN");
			}
			
			if(InputWrapper.GetMouseButtonUp(0))
			{
				isMoving=false;
				isFirstClick = true;
//				Debug.Log("BUTTON UP");
			}
			
				//transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime*camSpeed);
				if(isMoving && InputWrapper.GetMouseButton(0))
				{
					waitTime=0.0f;
	
				//	Debug.Log("MOVING NOWW  !!!");
		            rotationX += InputWrapper.GetAxis("Mouse X") * sensitivityX * sensitivity;
		            rotationY += InputWrapper.GetAxis("Mouse Y") * sensitivityY * sensitivity;
		 			//Debug.Log("rotationX : "+ rotationX);
					//Debug.Log("rotationY : "+ rotationY);
				
					
			            rotationX = Helpers.ClampAngle (rotationX, minimumX, maximumX);
			            rotationY = Helpers.ClampAngle (rotationY, minimumY, maximumY);
				
					transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
				}
				else
				{
					if(isFirstClick)
				{
						waitTime+=Time.deltaTime;
						if(waitTime>=2.0f)
						{
						//Debug.Log("NOT MOVING !!");
						transform.rotation = Quaternion.Slerp(transform.rotation, tempTargetRot, Time.deltaTime*camSpeed);	
						
						if(!invertY)
						{
							if(transform.rotation.x >= 0)
							{
								rotationY = -transform.localEulerAngles.x;
							}
							else
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
						}
						else
						{
							if(transform.rotation.x >= 0)
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
							else
							{
								rotationY = -transform.localEulerAngles.x;
							}
						}
						if(!invertX)
						{
							if(transform.rotation.y >= 0)
							{
								rotationX = transform.localEulerAngles.y;
							}
							else
							{
								rotationX = transform.localEulerAngles.y - 360;
							}
						}
						else
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y - 360;
							}
							else
							{
								rotationX = transform.localEulerAngles.y ;
							}
						}
						
						}
					}
				}
		}
		else
		{
			
			if(InputWrapper.touchCount==1 && InputWrapper.GetTouch(0).phase == TouchPhase.Began)
			{
				
					if(!isFirstClick){
						if(!startInvertY)
						{
							if(transform.rotation.x >= 0){
								rotationY = -transform.localEulerAngles.x;
							}
							else
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
						}
						else
						{
							if(transform.rotation.x >= 0){
								rotationY = (360 -transform.localEulerAngles.x);
							}
							else
							{
								rotationY = -transform.localEulerAngles.x;
							}
						}
						
						
						
						
						if(!startInvertX)
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y;
							}
							else
							{
								rotationX = transform.localEulerAngles.y - 360;
							}
						}
						else
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y - 360;
							}
							else
							{
								rotationX = transform.localEulerAngles.y ;
							}
						}
				}
				
				isMoving=true;
	//			Debug.Log("BUTTON DOWN");
			}
			
			if(InputWrapper.touchCount==1 && InputWrapper.GetTouch(0).phase == TouchPhase.Ended)
	        {
				isMoving=false;
				isFirstClick=true;
	//			Debug.Log("BUTTON UP");
			}
			
				//transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime*camSpeed);
			if(InputWrapper.touchCount==1 && InputWrapper.GetTouch(0).phase == TouchPhase.Moved && isMoving)
	        {
				waitTime=0.0f;
	
				//	Debug.Log("MOVING NOWW  !!!");
		            rotationX += InputWrapper.GetAxis("Mouse X") * sensitivityX * sensitivity;
		            rotationY += InputWrapper.GetAxis("Mouse Y") * sensitivityY * sensitivity;
		 		//	Debug.Log("rotationX : "+ rotationX);
				//	Debug.Log("rotationY : "+ rotationY);
				
					
			            rotationX = Helpers.ClampAngle (rotationX, minimumX, maximumX);
			            rotationY = Helpers.ClampAngle (rotationY, minimumY, maximumY);
				
					transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else
			{
				if(isFirstClick)
				{
						waitTime+=Time.deltaTime;
						if(waitTime>=2.0f)
						{
						//Debug.Log("NOT MOVING !!");
						transform.rotation = Quaternion.Slerp(transform.rotation, tempTargetRot, Time.deltaTime*camSpeed);						
						
						if(!invertY)
						{
							if(transform.rotation.x >= 0)
							{
								rotationY = -transform.localEulerAngles.x;
							}
							else
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
						}
						else
						{
							if(transform.rotation.x >= 0)
							{
								rotationY = (360 -transform.localEulerAngles.x);
							}
							else
							{
								rotationY = -transform.localEulerAngles.x;
							}
						}
						if(!invertX)
						{
							if(transform.rotation.y >= 0)
							{
								rotationX = transform.localEulerAngles.y;
							}
							else
							{
								rotationX = transform.localEulerAngles.y - 360;
							}
						}
						else
						{
							if(transform.rotation.y >= 0){
								rotationX = transform.localEulerAngles.y - 360;
							}
							else
							{
								rotationX = transform.localEulerAngles.y ;
							}
						}
						
						}
					}
			}
			
		}

	}
	
}