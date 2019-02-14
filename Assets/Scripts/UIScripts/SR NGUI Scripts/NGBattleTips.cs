using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
public class NGBattleTips : MonoBehaviour {


	public GameObject scrollView;
	[SerializeField]
	public GameObject debugMenu;
	[SerializeField]
	public GameObject debugButton;

	private bool isDebugMenu;

	// Use this for initialization
	void Start () {
		if(Debug.isDebugBuild) {
			Debug.LogError("IsDebugBuild");
			scrollView.SetActive(false);
			debugMenu.SetActive(true);
			isDebugMenu = true;
		} else {
			Debug.LogError("Not IsDebugBuild");
			scrollView.SetActive(true);
			debugMenu.SetActive(false);
			isDebugMenu = false;
			debugButton.gameObject.SetActive(false);
		}
	}
	// Update is called once per frame
	void Update () {
	}

	public void OnDebugButton() {
		if(isDebugMenu) {
			scrollView.SetActive(false);
			debugMenu.SetActive(true);
			isDebugMenu = false;
		} else {
			scrollView.SetActive(false);
			debugMenu.SetActive(true);
			isDebugMenu = true;
		}
	}
}