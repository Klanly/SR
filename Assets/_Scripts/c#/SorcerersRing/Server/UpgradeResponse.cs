using UnityEngine;
using System.Collections;

public class UpgradeResponse : ServerResponse {

	private UpdateResponeParam _upgrade = null;
	
	public UpgradeResponse(ServerRequest request, bool isSuccess, long serverCurrentTime, UpdateResponeParam upgrade) 
	: base(request, isSuccess, serverCurrentTime)
	{
		_upgrade = upgrade;
	}
	
	public UpdateResponeParam Upgrade
	{
		get
		{
			return _upgrade;
		}
	}
	
	public override string ToString ()
	{
		
		return string.Format ("base.ToString() >> " + base.ToString() + " :::: [UpgradeResponse: Upgrade={0}]", Upgrade);
	}
}
