using UnityEngine;
using System.Collections;

public class SpiritResponse : ServerResponse {

	private UpdateResponeParam _spirit = null;
	
	public SpiritResponse(ServerRequest request, bool isSuccess, long serverCurrentTime, UpdateResponeParam spirit) 
	: base(request, isSuccess, serverCurrentTime)
	{
		_spirit = spirit;
	}
	
	public UpdateResponeParam Spirit
	{
		get
		{
			return _spirit;
		}
	}
}
