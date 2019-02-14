using UnityEngine;
using System.Collections;

public class SocialMediaManager : MonoBehaviour {
	
	private static SocialMediaManager _instance = null;

	public static SocialMediaManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new SocialMediaManager();
			}
			return _instance;
		}
	}
	
	private SocialMediaManager()
	{
	}
	
	public bool isFacebookLoggedIn
	{
		get;set;
	}
	
	public bool retrieveOldGameState
	{
		get;set;
	}
	
	public string fbAccessToken
	{
		get;set;
	}
	public string fbUserId
	{
		get;
		set;
	}
	
	public void RegisterWithFacebookHandler(object responseParameters, object error, ServerRequest request)
	{
		IDictionary responseDictionary = responseParameters as IDictionary;
		if(responseDictionary["error"] != null && System.Convert.ToInt32(responseDictionary["errorCode"]) == 3)
		{
			Debug.Log(responseDictionary["error"].ToString());
			retrieveOldGameState=true;
			GameManager.instance.scaleformCamera.generalSwf.ShowGeneralPopup3("Retrieve Player Profile ?","You already have a player profile with Sorcerer's Ring.Would you like to retrieve your existing data?","YES","NO");
			//isFacebookLoggedIn = System.Convert.ToBoolean(responseDictionary["success"]);
		}
		else
		{
			isFacebookLoggedIn = System.Convert.ToBoolean(responseDictionary["success"]);
			
			if(!isFacebookLoggedIn)
			{

			}
		}
		
		Debug.Log("~~~~~~~~~~~~~isFacebookLoggedIn~~~~~~~~~~~~~~~~ = "+ isFacebookLoggedIn);
	}
}
