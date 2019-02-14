using UnityEngine;
using UnityEditor;
using System.Collections;

public class ReApplyShadersEditor : MonoBehaviour {

//
//	static void DoSomething () {
//		Debug.Log ("Doing Something...");
//	}

	// Use this for initialization
	[MenuItem ("Custom/ReApply Shaders/ReApply All")]
	static void ReApplyAll () {
		if(!Application.isPlaying) {
			return;
		}
		object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			g.AddComponent<ReApplyShaders>();
		}
//		ReApplyShaders ras = null;
//		foreach (object o in obj)
//		{
//			GameObject g = (GameObject) o;
//			ras = g.GetComponent<ReApplyShaders>();
//			Destroy(ras);
//		}

	}
	[MenuItem ("Custom/ReApply Shaders/ReApply Selected")]
	static void ReApplySelected () {
		if(!Application.isPlaying) {
			return;
		}
		object[] obj = Selection.objects;
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			g.AddComponent<ReApplyShaders>();
		}
		ReApplyShaders ras = null;
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			ras = g.GetComponent<ReApplyShaders>();
			Destroy(ras);
		}
	}



	[MenuItem ("Custom/ReApply Shaders/Find obj")]
	static void FindObj () {

		object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
//			g.AddComponent<ReApplyShaders>();
			if(g.tag.Equals("Cinematic_Outro")) {
				Debug.LogWarning("Found - "+g.name);


			}
		}
		//		ReApplyShaders ras = null;
		//		foreach (object o in obj)
		//		{
		//			GameObject g = (GameObject) o;
		//			ras = g.GetComponent<ReApplyShaders>();
		//			Destroy(ras);
		//		}
		
	}
}
