using UnityEngine;
using System.Collections;

public class SpiritRequest : ServerRequest {

	private ServerRequestParam _spirit = null;
	
	public SpiritRequest(ServerRequestType requestType, ProcessResponse response, ServerRequestParam spirit) 
	: base(requestType, response)
	{
		_spirit = spirit;
	}
	
	public ServerRequestParam Spirit
	{
		get
		{
			return _spirit;
		}
	}
}
