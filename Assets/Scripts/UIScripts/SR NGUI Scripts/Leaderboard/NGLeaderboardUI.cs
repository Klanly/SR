using UnityEngine;
using System.Collections;

public class NGLeaderboardUI : MonoBehaviour {


	[SerializeField]
	private UIPanel topPlayerPanel;
	[SerializeField]
	private UIPanel topGuildPanel;
	

	[SerializeField]
	private UISprite playerHighlight;
	[SerializeField]
	private UISprite guildHighlight;
	
	[SerializeField]
	private UIButton topPlayerButton;
	[SerializeField]
	private UIButton topGuildButton;


	[SerializeField]
	private UIGrid topPlayerScrollview;
	[SerializeField]
	private UIGrid topGuildScrollview;

	[SerializeField]
	private UISprite playerGlowSprite;
	[SerializeField]
	private UISprite guildGlowSprite;

	private RaidPortalNavigator _raidPortalNavigator;

//	int topGuildInitialTime = 0;
//	int topUserInitialTime = 0;


	// Use this for initialization
	void Start () {
		InputWrapper.disableTouch = true;
		OnTopPlayerClicked();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetRaidPortalNavigator(RaidPortalNavigator raidPortalNavigator) 
	{
		this._raidPortalNavigator = raidPortalNavigator;
		_raidPortalNavigator.OnLeaderBoardClicked();
	}

//	void OnEnable() {
//		_raidPortalNavigator.OnLeaderBoardClicked();
//	}
//
//	void OnDisable() {
//		InputWrapper.disableTouch = false;
//	}
//
	void OnDestroy() 
	{
		_raidPortalNavigator.OnLeaderBoardClosed();
	}

	public void OnTopPlayerClicked() {
		NGUITools.SetActive(playerHighlight.gameObject, false);
		NGUITools.SetActive(guildHighlight.gameObject, true);
		NGUITools.SetActive(playerGlowSprite.gameObject, true);
		NGUITools.SetActive(guildGlowSprite.gameObject, false);
//		Debug.LogError(System.DateTime.Now.Millisecond - topUserInitialTime);
//		if(System.DateTime.Now.Second - topUserInitialTime < 10000) {
//			Debug.LogError("returning user");
//			return;
//		}
//		topUserInitialTime = System.DateTime.Now.Millisecond;
//
		foreach (Transform child in topPlayerScrollview.transform) {
			GameObject.Destroy(child.gameObject);
		}
		
		ServerManager.Instance.GetArcanaLeaderboard(OnArcaneRatingReceived);
	}

	public void OnTopGuildClicked() {
		NGUITools.SetActive(playerHighlight.gameObject, true);
		NGUITools.SetActive(guildHighlight.gameObject, false);
		NGUITools.SetActive(playerGlowSprite.gameObject, false);
		NGUITools.SetActive(guildGlowSprite.gameObject, true);
//		Debug.LogError(System.DateTime.Now.Millisecond - topGuildInitialTime);
//		if(System.DateTime.Now.Second - topGuildInitialTime < 10000) {
//			Debug.LogError("returning guild");
//			return;
//		}
//		topGuildInitialTime = System.DateTime.Now.Millisecond;
		foreach (Transform child in topGuildScrollview.transform) {
			GameObject.Destroy(child.gameObject);
		}
		ServerManager.Instance.GetGuildLeaderboard(OnGuildLeaderboardReceived);
	}

	public void OnCloseButton() {
		gameObject.SetActive(false);
		_raidPortalNavigator.OnLeaderBoardClosed();
	}

	public void OnArcaneRatingReceived(object responseParameters, object error, ServerRequest request) {
		Debug.LogError("RESPONSE FOR ARCANE RATING ::::::::: " + MiniJSON.Json.Serialize(responseParameters as IDictionary));
		
		if (error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			if (MiniJSON.Json.Serialize(responseParameters as IDictionary) == null || (response == null))
			{
			} else
			{
				IList topRatings = (response.Contains("topRatings")) ? (response ["topRatings"] as IList) : null;
				if(topRatings == null) {
					return;
				}
				topPlayerPanel.gameObject.SetActive(true);
				topGuildPanel.gameObject.SetActive(false);
				GameObject prefab = Resources.Load("UIPrefabs/NGUI/LeaderBoardNode") as GameObject;
				GameObject obj;
				NGLeaderboardPlayerNode node;
				int count = 1;
				foreach(var listItem in topRatings) {
					IDictionary dict = listItem as IDictionary;
					string name = dict["userName"].ToString();
					int points = int.Parse(dict["score"].ToString());
					int flagID = int.Parse(dict["teamFlag"].ToString());
					string teamName = dict["teamName"].ToString();
					obj = Instantiate(prefab) as GameObject;
					obj.SetActive(true);
					topPlayerScrollview.AddChild(obj.transform);
					obj.transform.localScale = Vector3.one;
					node = obj.GetComponent<NGLeaderboardPlayerNode>();
					node.SetRank(count++);
					node.SetName(name);
					node.SetArcanePoints(points);
					node.SetFlagID(flagID);
					node.SetGuildName(teamName.Equals("null") ? "" : teamName);
					Debug.LogError(name + " "+ points);
				}
				topPlayerScrollview.Reposition();
			}
		}
	}

	public void OnGuildLeaderboardReceived(object responseParameters, object error, ServerRequest request) {
		Debug.LogError("RESPONSE FOR OnGuildLeaderboardReceived ::::::::: " + MiniJSON.Json.Serialize(responseParameters as IDictionary));

		if (error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			if (MiniJSON.Json.Serialize(responseParameters as IDictionary) == null || (response == null))
			{
			} else
			{
				IList topRatings = (response.Contains("topRatings")) ? (response ["topRatings"] as IList) : null;
				if(topRatings == null) {
					return;
				}
				topPlayerPanel.gameObject.SetActive(false);
				topGuildPanel.gameObject.SetActive(true);

				GameObject prefab = Resources.Load("UIPrefabs/NGUI/LeaderBoardGuildNode") as GameObject;
				GameObject obj;
				NGLeaderboardGuildNode node;
				int count = 1;
				Debug.LogError("topratings count "+topRatings.Count);
				foreach(var listItem in topRatings) {
					IDictionary dict = listItem as IDictionary;
//					string name = dict["userName"].ToString();
					int points = int.Parse(dict["score"].ToString());
					int flagID = int.Parse(dict["teamFlag"].ToString());
					string teamName = dict["teamName"].ToString();
					obj = Instantiate(prefab) as GameObject;
					obj.SetActive(true);
					topGuildScrollview.AddChild(obj.transform);
					obj.transform.localScale = Vector3.one;
					node = obj.GetComponent<NGLeaderboardGuildNode>();
					node.SetGuildName(teamName);
					node.SetFlagID(flagID);
					node.SetArcanePoints(points);
					node.SetRank(count++);
					if(dict.Contains("memberCount")) {
						int memberCount = int.Parse(dict["memberCount"].ToString());
						node.SetMemberCount(memberCount);
					}
				}
				topGuildScrollview.Reposition();
			}

		}
	}

}
