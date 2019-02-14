using UnityEngine;
using System.Collections;

public class ShrineRequest : ServerRequest {
	
	private string _type;
	
	public ShrineRequest(ServerRequestType requestType, ProcessResponse response,string type)
	: base(requestType, response)
	{
		this._type=type;
	}
	
	public string shrineType
	{
		get
		{
			return _type;
		}
	}
	
}
