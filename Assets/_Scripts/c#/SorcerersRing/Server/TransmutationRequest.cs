using UnityEngine;
using System.Collections;

public class TransmutationRequest : ServerRequest {

	private ServerRequestParam _transmute = null;
	
	public TransmutationRequest(ServerRequestType requestType, ProcessResponse response, ServerRequestParam transmute) 
	: base(requestType, response)
	{
		_transmute = transmute;
	}
	
	public ServerRequestParam Transmute
	{
		get
		{
			return _transmute;
		}
	}
}
