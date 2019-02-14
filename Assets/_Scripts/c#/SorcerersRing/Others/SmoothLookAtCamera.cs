using UnityEngine;
using System.Collections;

public class SmoothLookAtCamera : MonoBehaviour {
	public float damping=6.0f;
	public Transform target;
	private float zPos;
	// Use this for initialization
	void Start () {
		zPos = transform.localPosition.z;
		target=Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.timeScale==1.0f && Camera.main !=null)
		{
		target=Camera.main.transform;
			
		Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,zPos);
		}
		
		
	
	}
}
