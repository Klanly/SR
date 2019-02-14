using UnityEngine;
using System.Collections;

public class ServerResponse  {

	private ServerRequest _request;
	private long _serverCurrentTime = 0;
	private bool _isSuccess = false;
	
	public ServerResponse(ServerRequest request, bool isSuccess, long serverCurrentTime)
	{
		_request = request;
		_isSuccess = isSuccess;
		_serverCurrentTime = serverCurrentTime;
	}
	
	public ServerRequest Request
	{
		get
		{
			return _request;
		}
	}
	
	public long ServerCurrentTime
	{
		get
		{
			return _serverCurrentTime;
		}
	}
	
	public bool IsSuccess
	{
		get
		{
			return _isSuccess;
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("[ServerResponse: Request={0}, ServerCurrentTime={1}, IsSuccess={2}]", Request, ServerCurrentTime, IsSuccess);
	}
}
