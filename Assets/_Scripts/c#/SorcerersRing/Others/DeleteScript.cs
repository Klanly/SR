using UnityEngine;
using System.Collections;

public class DeleteScript : MonoBehaviour {

	void Start () {
	
		StartCoroutine(DeleteRoutine(6.0f/Time.timeScale));
	}
	
	public void DeleteObjectIn(float time)
	{
		StartCoroutine(DeleteRoutine(time));
	}
	
	IEnumerator DeleteRoutine(float time)
	{
		yield return new WaitForSeconds(time);
		
		GameObject.Destroy(gameObject);
	}
}