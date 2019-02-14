using UnityEngine;
using System.Collections;

public class TransmuteRespons : ServerResponse {

	private UpdateResponeParam _transmutation = null;
	
	public TransmuteRespons(ServerRequest request, bool isSuccess, long serverCurrentTime, UpdateResponeParam transmutation) 
	: base(request, isSuccess, serverCurrentTime)
	{
		_transmutation = transmutation;
	}
	
	public UpdateResponeParam Transmutation
	{
		get
		{
			return _transmutation;
		}
	}
	
	public override string ToString ()
	{
		return string.Format ("BASE STR >> {0} [TransmuteRespons: Transmutation={1}]", base.ToString(), Transmutation);
	}
}
