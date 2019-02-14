using UnityEngine;
using System.Collections;

public class RotatePlayer : MonoBehaviour {
	
	public float speed;
	public float MinYAngle;
	public float MaxYAngle;
	
	public float MinXAngle;
	public float MaxXAngle;
	
	float waitTime;
	
	public Transform target;
	public Vector3 angles=Vector3.zero;
	Quaternion rotation=Quaternion.identity;
	// Use this for initialization
	void OnEnable () {
		angles=transform.eulerAngles;
		
//		if (rigidbody)
//		rigidbody.freezeRotation = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		
	if (target)
	{
   		rotatePlayer();
	
	}
}
					
	public float getPlayerAngle()
	{
		return angles.x;
	}
	
	void rotatePlayer()
	{
		if(Application.isEditor)
		{
			 if(Input.GetMouseButton(0))
	   		 {
				
				waitTime+=Time.deltaTime;
				//Debug.Log(waitTime+"editor");
				if(waitTime>0.1f)
				{
					angles.y += Input.GetAxis("Mouse X");
					angles.x +=-Input.GetAxis("Mouse Y");
					if(!(angles.y<=MaxYAngle && angles.y>=MinYAngle))
						{
							if((MaxYAngle-angles.y)<=0.1f)
							{
								angles.y=MaxYAngle-0.1f;
							}
							else
							{
								angles.y=MinYAngle-0.1f;
								//Debug.Log("Editor===ClampAngle(angles.y,MinYAngle,MaxYAngle)"+ClampAngle(angles.y,MinYAngle,MaxYAngle)+"angles.y"+angles.y);
							}
						}
					if(!(angles.x<=MaxXAngle && angles.x>=MinXAngle))
						{
							if((MaxXAngle-angles.x)<=0.1f)
							{
								angles.x=MaxXAngle-0.1f;
							}
							else
							{
								angles.x=MinXAngle-0.1f;
								//Debug.Log("Editor===ClampAngle(angles.x,MinYAngle,MaxYAngle)"+ClampAngle(angles.x,MinXAngle,MaxXAngle)+"angles.x"+angles.x);
							}
						}
		        	rotation = Quaternion.Euler(ClampAngle(angles.x,MinYAngle,MaxYAngle), ClampAngle(angles.y,MinYAngle,MaxYAngle), transform.eulerAngles.z);
					if(angles.y<=MaxYAngle+4.0f && angles.y>=MinYAngle-4.0f)
						{
		       				 transform.rotation = rotation;
						}
				}
	        }
			else if(Input.GetMouseButtonUp(0))
			{
				waitTime=0.0f;
			}
		}
		else 
		{
	        if(Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Moved)
	        {
				waitTime+=Time.deltaTime;
				//Debug.Log(waitTime+"pod");
				if(waitTime>0.1f)
				{
					angles.y +=Input.GetTouch(0).deltaPosition.x * 0.3f ;
					angles.x+=-Input.GetTouch(0).deltaPosition.y * 0.3f ;
					if(!(angles.y<=MaxYAngle && angles.y>=MinYAngle))
						{
							if((MaxYAngle-angles.y)<=0.1f)
							{
								angles.y=MaxYAngle-0.1f;
							}
							else
							{
								angles.y=MinYAngle-0.1f;
							//	Debug.Log("Editor===ClampAngle(angles.y,MinYAngle,MaxYAngle)"+ClampAngle(angles.y,MinYAngle,MaxYAngle)+"angles.y"+angles.y);
							}
						}
					if(!(angles.x<=MaxXAngle && angles.x>=MinXAngle))
						{
							if((MaxXAngle-angles.x)<=0.1f)
							{
								angles.x=MaxXAngle-0.1f;
							}
							else
							{
								angles.x=MinXAngle-0.1f;
							//	Debug.Log("Editor===ClampAngle(angles.x,MinYAngle,MaxYAngle)"+ClampAngle(angles.x,MinXAngle,MaxXAngle)+"angles.x"+angles.x);
							}
						}
			      	rotation = Quaternion.Euler(ClampAngle(angles.x,MinYAngle,MaxYAngle), ClampAngle(angles.y,MinYAngle,MaxYAngle), transform.eulerAngles.z);
		        	//Debug.Log("Touchh===ClampAngle(angles.x,MinYAngle,MaxYAngle)"+ClampAngle(angles.x,MinYAngle,MaxYAngle)+"angles.x"+angles.x);
					if(angles.y<=MaxYAngle+4.0f && angles.y>=MinYAngle-4.0f)
						{
		       				 transform.rotation = rotation;
						}
				}
	        }
			else if(Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				waitTime=0.0f;
			}
		}

	}
	
	static float ClampAngle (float angle, float min , float max) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
		
	if(angle>180)
		angle -=360;
		if(angle<0)
		angle -=360;
	return Mathf.Clamp (angle, min, max);
}
}
