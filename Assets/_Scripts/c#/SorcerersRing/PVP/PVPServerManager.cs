using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameStateModule;
using MiniJSON;

public class PVPServerManager
{
	private static string TAG = "PVPServerManager";
	private static PVPServerManager _instance = null;
	private PVPServerManager()
	{	}
	
	public static PVPServerManager Instance
	{
		get
		{
			if(_instance == null)
				_instance = new PVPServerManager();
			return _instance;
		}
	}
	
#region Opponent's gamestate push message receiver
	public delegate void PVPOpponentFoundDelegate(object opponent);

	public delegate void PVPOpponentParsedDelegate(string opponentID, GameState opponent);
	PVPOpponentParsedDelegate _PVPOpponentParsedDelegate;
	
	public void OnOpponentFound(object opponent)
	{
		IDictionary<string, object> response = opponent as IDictionary<string, object>;

		string opponentID = "";
		//string opponentID = (response["opponentID"]).ToString();
		string gameState = MiniJSON.Json.Serialize(response["gameState"]);

		if(GameManager.PRINT_LOGS) Debug.Log("Opponent state :" + gameState);
		if (opponentID != null)
			if(GameManager.PRINT_LOGS) Debug.Log("Opponent id: " + opponentID);

		if (gameState != null)
			GameStateParser.ProcessGameState(OnOpponentParsed, gameState, new GameState(), opponentID);

		if(GameManager.PRINT_LOGS) Debug.Log(TAG + "OnOpponentFound");
	}

	private void OnOpponentParsed(GameState gState, string filename, bool success)
	{
		if (success)
			if(_PVPOpponentParsedDelegate != null)
				_PVPOpponentParsedDelegate(filename, gState);
		if(GameManager.PRINT_LOGS) Debug.Log(TAG + "PVPOponentParsed = " + success.ToString());
	}

#endregion
#region Queue for pvp battle
	public delegate void PVPQueueRequestDelegate(bool requestSucceeded);
	PVPQueueRequestDelegate _PVPQueueRequestDelegate;
	
	public void QueueForPVP(PVPOpponentParsedDelegate _PVPOpponentDelegate, PVPQueueRequestDelegate _PVPQueueRequestDelegate = null)
	{
		this._PVPQueueRequestDelegate = _PVPQueueRequestDelegate;
		this._PVPOpponentParsedDelegate = _PVPOpponentDelegate;
		ServerManager.Instance.QueueForPVP(this.OnPVPQueueResponse, this.OnOpponentFound);
	}
	
	public void OnPVPQueueResponse(object responseParameters, object error, ServerRequest request)
	{
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			
			if(_PVPQueueRequestDelegate != null)
				_PVPQueueRequestDelegate(true);
			
			if(GameManager.PRINT_LOGS) Debug.Log("OnPVPReady - RESPONSE SUCCESSFUL! >> " + MiniJSON.Json.Serialize(response));
		}
		else
		{
			if(_PVPQueueRequestDelegate != null)
				_PVPQueueRequestDelegate(false);
			if(GameManager.PRINT_LOGS) Debug.Log( " error " + error.ToString());
		}
	}
#endregion
	
	
#region Get PVPReadyListener Result
	public delegate void PVPReadyListener(bool requestSucceeded);
	PVPReadyListener _pvpReadyListener;
	
	public void TellPVPReady(PVPReadyListener _pvpReadyListener = null)
	{
		this._pvpReadyListener = _pvpReadyListener;
		ServerManager.Instance.TellPVPReady(this.OnPVPReady);
	}
	
	public void OnPVPReady(object responseParameters, object error, ServerRequest request)
	{
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			
			if(_pvpReadyListener != null)
				_pvpReadyListener(true);
			
			if(GameManager.PRINT_LOGS) Debug.Log("OnPVPReady - RESPONSE SUCCESSFUL! >> " + MiniJSON.Json.Serialize(response));
		}
		else
		{
			if(_pvpReadyListener != null)
				_pvpReadyListener(false);
			if(GameManager.PRINT_LOGS) Debug.Log( " error " + error.ToString());
		}
	}
#endregion
	
	
#region Get PVPReadyListener Result
	public delegate void TurnResultListener(GestureEmitter.Gesture gesture);
	TurnResultListener _turnResultListener;
	
	public void SubmitPVPTurn(string gesture, TurnResultListener _turnResultListener = null)
	{
		this._turnResultListener = _turnResultListener;
		ServerManager.Instance.SubmitPVPTurn(gesture, this.OnPVPTurnResult);
	}
	
	public void OnPVPTurnResult(object responseParameters, object error, ServerRequest request)
	{
		if(error == null)
		{
			IDictionary response = responseParameters as IDictionary;
			if(GameManager.PRINT_LOGS) Debug.Log("OnPVPTurnResult" + MiniJSON.Json.Serialize(response));
			string opponentCastElement = response["opponentCastElement"] as String;
			GestureEmitter.Gesture gesture = SRCharacterController.GetGestureForSpellName(opponentCastElement);
			//int playerLife = response["playerLife"] as Int32;
			//int opponentLife = response["opponentLife"] as Int32; 
			//bool winner = response["winner"] as Boolean;

			if(_turnResultListener != null)
				_turnResultListener(gesture);
			
			if(GameManager.PRINT_LOGS) Debug.Log("OnPVPReady - RESPONSE SUCCESSFUL! >> " + MiniJSON.Json.Serialize(response));
		}
		else
		{
			if(_turnResultListener != null)
				_turnResultListener(GestureEmitter.Gesture.kInvalid);
			if(GameManager.PRINT_LOGS) Debug.Log( " error " + error.ToString());
		}
	}
#endregion
	
}

