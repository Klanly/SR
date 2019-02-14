using UnityEngine;
using System.Collections;

public class SecretMenu : MonoBehaviour {
	
	public dfButton toggleFPS;
	public dfButton toggleUserID;
	public dfButton resetGame;
	public dfButton unlockGame;
	
	public dfLabel FPSStatus;
	public dfLabel UserIDStatus;
	public dfTextbox skullLevel;

	
	// Use this for initialization
	void Start () {
		
		skullLevel.TextSubmitted += textValueChanged;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setFPSStatus(bool enable){
		FPSStatus.Text = enable ? "On": "Off";
	}
	
	public void setUserIDStatus(bool enable){
		UserIDStatus.Text = enable ? "On": "Off";
	}
	
	public void textValueChanged(dfControl control, string text){
		Debug.Log(text+" value entered");
	}
	
	
}
