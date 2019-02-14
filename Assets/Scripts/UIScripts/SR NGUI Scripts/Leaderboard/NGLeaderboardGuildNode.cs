using UnityEngine;
using System.Collections;

public class NGLeaderboardGuildNode : MonoBehaviour {


	[SerializeField]
	private UILabel guildNameLabel;
	[SerializeField]
	private UILabel memberCountLabel;
	[SerializeField]
	private UILabel rankLabel;
	[SerializeField]
	private UILabel guildArcanePointsLabel;
	[SerializeField]
	private UISprite flagSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetRank(int rank) {
		rankLabel.text = rank+".";
	}
	
	public void SetGuildName(string name) {
		guildNameLabel.text = name;
	}
	
	public void SetArcanePoints(int points) {
		guildArcanePointsLabel.text = points.ToString();
	}

	public void SetMemberCount(int count) {
		memberCountLabel.text = count.ToString();
	}
	
	public void SetFlagID(int flagID) {
		if(flagID == -1) {
			NGUITools.SetActive(flagSprite.gameObject, false);
			return;
		}
		NGUITools.SetActive(flagSprite.gameObject, true);
		flagSprite.spriteName = "image "+flagID;

	}
	
}
