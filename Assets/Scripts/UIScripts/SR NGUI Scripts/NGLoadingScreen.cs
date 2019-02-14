using UnityEngine;
using System.Collections;

public class NGLoadingScreen : MonoBehaviour 
{
	public UISlider _loadingBarSlider;
	public UILabel _loadingBarLabel;
	public GameObject _container;
	
	private System.Random rand = new System.Random();
	private Transform _containerTransform;
	private Vector3 _containerPosition;
	public float tiltFactor = 10.0f;

	private static string [] tips = {
		"Enemies too tough? Buy better Rings!", 
		"Transmutation can upgrade rings or runes",
		"Upgrade your Potion Belt to carry more Potions",
		"Defeat Primus Nex to open up new Zones",
		"Every Spirit has unique abilities and powers",
		"You can only have one Spirit active at a time",
		"Upgrade your Arcane Keyring to carry more Keys!",
		"Upgrade the Transmutation Cube for better rewards",
		"Elemental Rings let you use that element in battle",
		"Draw the spells correctly or they may not work!",
		"Only a Rune Spell can break through a Shield"
	};
	
	public void Start()
	{
		SetDefaultValues();
		
		ShowLoadingScreenBarText(tips[rand.Next(0, tips.Length)]);
		_containerTransform = _container.transform;
		_containerPosition = _containerTransform.position;
	}

	void Update() {
//		Vector3 newPos = new Vector3(_containerPosition.x + Input.acceleration.x * tiltFactor, _containerPosition.y + Input.acceleration.y * tiltFactor, _containerPosition.z);
//		_containerTransform.position = Vector3.Slerp(_containerTransform.position, newPos, Time.deltaTime * 5.0f);
	}

	public void SetDefaultValues()
	{
		HideLoadingBar();
		ShowLoadingScreenBarText(string.Empty);
	}
	
	public void HideLoadingBar()
	{
		//_loadingBarLabel.IsVisible = false;
		_loadingBarSlider.gameObject.SetActive(false);
	}
	
	public void ShowLoadingScreenBarText(string text)
	{
		if(string.IsNullOrEmpty(text))
			return;
		
		_loadingBarLabel.gameObject.SetActive(true);
		_loadingBarLabel.text = text;
	}
	
	public void SetLoadingPercentage(int percentage)
	{
		_loadingBarLabel.gameObject.SetActive(true);

		_loadingBarSlider.gameObject.SetActive(true);
		_loadingBarSlider.value = percentage/100f;
	}
	
}
