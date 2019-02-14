using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public dfSlider musicSlider;
	public dfSlider sfxSlider;
	private dfControl temp;
	private float currentMusicValue;
	private float currentSFXValue;
	public dfPanel mainPanel;
	public dfPanel battleTipsPanelPrefab;
	
	public dfLabel loginInfoLabel;
	
	public OptionsUIInterface _optionsUIInterface;

	public void setInterface(OptionsUIInterface optionsInterface)
	{
		_optionsUIInterface = optionsInterface;
	}

	// Use this for initialization
	void Start () 
	{
		currentMusicValue = 0.0f;
		currentSFXValue = 0.0f;


		if(_optionsUIInterface != null)
			_optionsUIInterface.OnRegisterSWFChildCallback(this);
		
//		UIManager.instance.generalSwf.ToggleTopStats(false);
//		musicSlider.Value = GameManager._gameState.musicVolume * 100;
//		sfxSlider.Value = GameManager._gameState.gfxVolume * 100;
		gameObject.GetComponent<dfPanel>().PerformLayout();
		
		loginInfoLabel.Text = string.Format("Logged in with user id = {0}, user name = {1}", GameManager.instance.fragNetworkingNew.GetUserID(), GameManager._gameState.User.username);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void onMusicValueChanged()
	{
		currentMusicValue = musicSlider.Value/100f;
		SoundManager.instance.SetMVolume(currentMusicValue);
		Debug.Log("value changed to "+currentMusicValue);
	}

	public void onSfxValueChanged()
	{
		currentSFXValue = sfxSlider.Value/100f;
		SoundManager.instance.SetSfxVolume(currentSFXValue);
		Debug.Log("value changed to "+currentSFXValue);
	}

	public void OnDestroy()
	{
		UnityEngine.Debug.Log("generalSwf.ToggleTopStats > T");
		UIManager.instance.generalSwf.ToggleTopStats(true);
	}

	public void onBattleTipsButton()
	{
		mainPanel.IsVisible = false;
		Debug.Log("Main Options Panel ===> " + mainPanel.IsVisible);
		temp = gameObject.GetComponent<dfPanel>().AddPrefab(battleTipsPanelPrefab.gameObject);
		temp.GetComponent<dfPanel>().PerformLayout();
		Debug.Log("Temp Panel : " + temp.name);
		Analytics.logEvent(Analytics.EventName.Options_BattleTutorial);
	}

	public void onSocialButton()
	{

	}

	public void onEquipmentsButton()
	{

	}

	public void onSupportButton()
	{

	}

	public void onCreditsButton()
	{

	}

	public void onCinematicsButton()
	{

	}

	public void onBackButton( dfControl control, dfMouseEventArgs mouseEvent )
	{
		mainPanel.IsVisible = true;
		Destroy(mouseEvent.Source.Parent.gameObject);
	}


}
