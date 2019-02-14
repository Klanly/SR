using UnityEngine;
using System.Collections;

public class NGTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick(UIButton button)
	{
		Debug.Log(button.name);
	}
}
