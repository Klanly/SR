using UnityEngine;
using System.Collections;

public class UpgradeRequest : ServerRequest {

	private ServerRequestParam _upgrade = null;
	
	public UpgradeRequest(ServerRequestType requestType, ProcessResponse response, ServerRequestParam upgrade) 
	: base(requestType, response)
	{
		_upgrade = upgrade;
	}
	
	public ServerRequestParam Upgrade
	{
		get
		{
			return _upgrade;
		}
	}
}
