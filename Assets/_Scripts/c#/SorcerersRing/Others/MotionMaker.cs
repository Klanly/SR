using UnityEngine;
using System.Collections;

public class MotionMaker : MonoBehaviour {
	
	private float ShakeIntensity;

	private float ShakeDecay = 0.01f;
	
	private float shake;
	
	GameObject shakeObject;
	
	private bool Shaking = false;
	
	Quaternion OriginalRot;
	
	Vector3 OriginalPos;
	
	void Start()
	{
		Shaking = false;
	}
	
	public void Shake(GameObject gameObj, float shake)
	{
		shakeObject = gameObj;
		
		OriginalPos = gameObj.transform.position;
		
		OriginalRot = gameObj.transform.rotation;
		
		ShakeIntensity = shake;
		
		Shaking = true;
	}
	
	
	void Update () 
	{
		if(Shaking)
		{
			if(ShakeIntensity > 0)
			{
			    shakeObject.transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
			    shakeObject.transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                              OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                              OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
			                              OriginalRot.w + Random.Range(-ShakeIntensity,     ShakeIntensity)*.2f);
			
			   ShakeIntensity -= ShakeDecay;
			}
			else if (Shaking)
			{
				shakeObject.transform.position = OriginalPos;
				
				shakeObject.transform.rotation = OriginalRot;
				
			   Shaking = false;  
			}
		}
	}

}
