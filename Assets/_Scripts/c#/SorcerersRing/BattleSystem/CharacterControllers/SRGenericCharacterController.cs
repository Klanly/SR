using UnityEngine;
using System.Collections;

public abstract class SRGenericCharacterController : MonoBehaviour {

	
	//public SRGenericCharacterStateController genCharStateControl;
	
	public void RotateTo(Vector3 point) {	}
	
	public abstract void OnSetStart();
	
	public void MoveTo(Vector3 point)
	{
		
	}
}
