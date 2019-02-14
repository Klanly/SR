using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Collections.Generic;
public class NGCredits: MonoBehaviour {


	public UIScrollView creditsView;
	//	public GameObject levelButtons;
	public UIGrid grid;
	public UILabel label;
	public UISprite logo;

	// Use this for initialization
	void Start () {
		StartCoroutine("Move");

//		Dictionary<string ,string> creditsDictionary = GameManager._dataBank.creditsDictionary;
//
//		foreach(string value in creditsDictionary.Values) {
//			UILabel lbl = (UILabel)Instantiate(label);
//			lbl.gameObject.SetActive(true);
//			grid.AddChild(lbl.transform);
//			lbl.text = value;
//			lbl.transform.localScale = Vector3.one;
//		}
//		UISprite sprite = (UISprite)Instantiate(logo);
//		sprite.gameObject.SetActive(true);
//		grid.AddChild(sprite.transform);
//		sprite.transform.localScale = Vector3.one;
//		grid.Reposition();

	}

	IEnumerator Move() {
		yield return new WaitForSeconds(2.0f);

		while(true) {
			yield return new WaitForEndOfFrame();
			creditsView.MoveRelative(new Vector3(0, 0.8f, 0));
		}
	}

	public void OnClick(UIButton button) {
		Destroy(gameObject);
	}

	public static Dictionary<string,string> LoadCredits(IList creditsList)
	{
		Dictionary<string, string> creditsDictionary = new Dictionary<string, string>();
		IDictionary creditsKeyValPair;
		for(int i = 0; i<creditsList.Count;i++)
		{
			creditsKeyValPair = creditsList[i] as IDictionary;
			creditsDictionary.Add(i.ToString(), creditsKeyValPair["text"].ToString());
		}
		return creditsDictionary;
	}
}
