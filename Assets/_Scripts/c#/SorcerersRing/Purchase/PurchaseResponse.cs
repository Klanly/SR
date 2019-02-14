using UnityEngine;
using System.Collections;

public class PurchaseResponse {

	private PurchaseRequest _purchaseRequest;
	private bool _isSuccess;
	private string _responseMessage;
	
	public PurchaseResponse(PurchaseRequest request, bool isSuccess, string responseMessage)
	{
		_purchaseRequest = request;
		_isSuccess = isSuccess;
		_responseMessage = responseMessage;
	}
	
	public PurchaseRequest PurchaseRequest
	{
		get
		{
			return _purchaseRequest;
		}
	}
	
	public bool IsSuccess
	{
		get
		{
			return _isSuccess;
		}
	}
	
	public string ResponseMessage
	{
		get
		{
			return _responseMessage;
		}
	}
}
