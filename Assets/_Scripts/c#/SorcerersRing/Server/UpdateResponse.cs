using UnityEngine;
using System.Collections;

public class UpdateResponse : ServerResponse
{
	private UpdateResponeParam _transmutation = null;
	private UpdateResponeParam _upgrade = null;
	private UpdateResponeParam _spirit = null;
	
	public UpdateResponse(ServerRequest request, bool isSuccess, long serverCurrentTime, UpdateResponeParam transmutation, UpdateResponeParam upgrade, UpdateResponeParam spirit)
	: base(request, isSuccess, serverCurrentTime)
	{
		_transmutation = transmutation;
		_upgrade = upgrade;
		_spirit = spirit;
	}
	
	public UpdateResponeParam Transmutation
	{
		get
		{
			return _transmutation;
		}
	}
	
	public UpdateResponeParam Upgrade
	{
		get
		{
			return _upgrade;
		}
	}
	
	public UpdateResponeParam Spirit
	{
		get
		{
			return _spirit;
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("[UpdateResponse: Transmutation={0}, Upgrade={1}, Spirit={2}]", Transmutation, Upgrade, Spirit);
	}
}
