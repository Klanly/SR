using UnityEngine;
using System.Collections;

public class GUICameraDepthController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Camera>().orthographicSize = (float)(0.44 * Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
