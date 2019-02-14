using UnityEngine;
using System.Collections;

public class NGLeaderboardPlayerNode : MonoBehaviour {


	[SerializeField]
	private UILabel nameLabel;
	[SerializeField]
	private UILabel arcanePointsLabel;
	[SerializeField]
	private UILabel rankLabel;
	[SerializeField]
	private UILabel guildNameLabel;
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
	
	public void SetName(string name) {
		nameLabel.text = name;
	}
	
	public void SetArcanePoints(int points) {
		arcanePointsLabel.text = points.ToString();
	}

	public void SetGuildName(string guildName) {
		guildNameLabel.text = guildName;
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
