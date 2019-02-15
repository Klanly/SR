using UnityEngine;

public class GestureResults : MonoBehaviour
{
	private Transform _thisTransform;
	private float _showTimeLeft;
	
	public TextMesh resultMesh;
	
	public float spinSpeed;
	public float showTime;
	
	void Awake()
	{
		_thisTransform = this.transform;
		ShowResult("");
	}
	
	void Update()
	{
		if (_showTimeLeft > 0)
		{
			_thisTransform.Rotate(0, spinSpeed * Time.deltaTime, 0);
		
			_showTimeLeft -= Time.deltaTime;
			if (_showTimeLeft <= 0)
			{
				_showTimeLeft = 0;
				ShowResult("");
			}
		}
	}
	
	public void ShowResult(string result)
	{
		resultMesh.text = result;
		
		_showTimeLeft = showTime;
	}
}
