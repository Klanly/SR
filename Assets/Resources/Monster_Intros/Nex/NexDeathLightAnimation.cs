using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NexDeathLightAnimation : MonoBehaviour {

	public List<float> seconds;
	public List<float> lightIntensity;
	public Light light;

	// Use this for initialization
	void Start () {
		StartCoroutine("PlayLightCinematics");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayLightCinematics() {
		float timeStart = 0.0f;
		float timeEnd = seconds[seconds.Count-1];
		while(timeStart < timeEnd) {
			timeStart += Time.deltaTime;
			int previousIndex = seconds.Count-1;
			for(int i = 0;i < seconds.Count;i++) {
				if(timeStart < seconds[i]) {
					previousIndex = i;
					break;
				}
			}
			int index = previousIndex < seconds.Count ? previousIndex : previousIndex - 1;
//			Debug.LogError("index = "+index+" value = "+lightIntensity[index]+" time = "+timeStart+" "+light.intensity);
			light.intensity = Mathf.Lerp(light.intensity, lightIntensity[index], Time.deltaTime * 3.0f);

			yield return new WaitForEndOfFrame();
		}
	}
}
