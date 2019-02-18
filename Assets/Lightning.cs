using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour {
	
	public float minDecayTime = 0.24f;
	public float maxDecayTime = 0.75f;
	public float maxInterval = 10.0f;
	public float minInterval = 0.1f;
	
	private float brightness;
	
	// Use this for initialization
	void Start () 
	{
		brightness = light.intensity; // Set flash brightness
		light.intensity = 0; // Start in darkness
		Invoke ("Flash", Random.Range(minInterval, maxInterval));
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make shadows rotate slowly.
		transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime);
	}
	
	void Flash()
	{
		light.intensity = brightness;
		StartCoroutine("Decay");
	}
	
	IEnumerator Decay()
	{
		// Set random duration of flash
		float decayTime = Random.Range(minDecayTime, maxDecayTime);
		float t = 0;
		while (t < decayTime)
		{
			light.intensity = Mathf.Lerp(brightness, 0, t/decayTime);
			t += Time.deltaTime;
			yield return null;
		}
		light.intensity = 0;
		// Set up another flash interval
		Invoke ("Flash", Random.Range(minInterval, maxInterval));
	}
}
