using System;


public class WorkQueue : AsyncQueue<WorkQueue, WorkQueue.Request, WorkQueue.Response> {
	
	public class Request : BaseRequest {
		public Func<Object> backgrundCallback;
		public Action backgrundNoReturnCallback;
	}
	
	public class Response: BaseResponse {
		public object retVal;
		public string error;
	}
	
	public static void Do(Func<Object> backgroundCallback, Callback foregroundCallback) {
		WorkQueue.instance.Enqueue(new WorkQueue.Request(){
			backgrundCallback = backgroundCallback,
			callback = foregroundCallback});
	}
	
	public static void Do(Action backgroundNoReturnCallback) {
		WorkQueue.instance.Enqueue(new WorkQueue.Request(){
			backgrundNoReturnCallback = backgroundNoReturnCallback});
	}
	
	protected override void ProcessRequest(Request request, Response response) {
		if (request.backgrundNoReturnCallback != null)
			try {
				request.backgrundNoReturnCallback();
			} catch (System.Exception e) {
				//Log.Error("Error executing non returning callback. Error: " + e.ToString());
			UnityEngine.Debug.Log("Error executing non returning callback. Error: " + e.ToString());
			}
		else
			try {response.retVal = request.backgrundCallback();} catch (System.Exception e) {response.error = e.ToString();}
	}
}
