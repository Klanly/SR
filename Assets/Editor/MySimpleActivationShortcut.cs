using UnityEngine;
using System.Collections;
using UnityEditor;

public class MySimpleActivationShortcut : Editor {

	[MenuItem("GameObject/ActiveToggle _a")]
  	static void ToggleActivationSelection()
  	{
    	var go = Selection.activeGameObject;
		go.SetActive(!go.activeSelf);
	}

//	[MenuItem("GameObject/ActiveToggle _v")]
//	static void ToggleVisibilitySelection()
//	{
//		var go = Selection.activeGameObject;
//
//		go.GetComponent<dfControl>().IsVisible = !go.GetComponent<dfControl>().IsVisible;
//
//		//dfControl comp = go.GetComponent<dfLabel>() ?? go.GetComponent<dfSprite>();
//
//	}
}
