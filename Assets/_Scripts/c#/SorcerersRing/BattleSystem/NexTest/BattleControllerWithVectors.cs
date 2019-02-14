using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleControllerWithVectors: MonoBehaviour {
	
	/*
	 * POSITION MAP...
	 *
     *       POS1 **************** POS2 **************** POS3
	 *        **                                          **
	 *        **                                          **
	 *        **                                          **
	 *       POS4                                        POS5
	 * 
	 */
	
	public string enemyObjectName = "PREFAB_NEX";
	public string playerObjectName = "SORCERER_PREFAB";
	
	public GameObject enemyObject;
	
	public GameObject playerObject;
	public GameObject cameraObj;
	
	private Vector3 nexPos1; // = new Vector3(3.0f, 3.0f, 8.0f);
	private Vector3 nexPos2; // = new Vector3(3.0f, 0f, 8.0f);
	private Vector3 nexPos3; // = new Vector3(3.0f, 0f, 8.0f);
	private Vector3 nexPos4; // = new Vector3(3.0f, 0f, 8.0f);
	private Vector3 nexPos5; // = new Vector3(3.0f, 0f, 8.0f);
	private Vector3 nexPos6;

	public string pos1GameObjectName;
	public string pos2GameObjectName;
	public string pos3GameObjectName;
	public string pos4GameObjectName;
	public string pos5GameObjectName;
	public string pos6GameObjectName;
	
	const string POS1 = "nexPos1";
	const string POS2 = "nexPos2";
	const string POS3 = "nexPos3";
	const string POS4 = "nexPos4";
	const string POS5 = "nexPos5";
	const string POS6 = "nexPos6";

	public float camOffsetPos1;
	public float camOffsetPos2;
	public float camOffsetPos3;
	public float camOffsetPos4;
	public float camOffsetPos5;
	
	public GameObject camRotationPivotGameObject;
	public float camPivotRot1;
	public float camPivotRot2;
	public float camPivotRot3;
	public float camPivotRot4;
	public float camPivotRot5;
	
	private string currentPos;
	
	public float TOUCH_SENSITIVITY = 1f/6f;
	
	public float TILT_SENSITIVITY_X = .002F;
	public float TILT_SENSITIVITY_Y = .001F;
	
	public float mTiltX;
	public float mTiltY;
	
	public float MAX_TILT_X = 180.0f;
	public float MAX_TILT_Y = 180.0f;

	private float DIR_X = 1;
	private float DIR_Y = 1;

	public Dictionary<string, Vector3[]> nextPosDictionary = new Dictionary<string, Vector3[]>();
	
	public Dictionary<string, float> camOffsetDictionary = new Dictionary<string, float>();
	public Dictionary<string, float> camPivotRotationDictionary = new Dictionary<string, float>();
	
	public float TRANSITION_TIME = 0.1f;
	public GameObject explosionPrefab;
	
	public bool allowTilt;	
	
	void Start ()
	{
		TILT_SENSITIVITY_X = 1.8F;
		TILT_SENSITIVITY_Y = 1F;
		
		allowTilt = false;
		
		enemyObject = GameObject.Find(enemyObjectName);
		
		playerObject = GameObject.Find(playerObjectName);
		
		nexPos1 = GameObject.Find(pos1GameObjectName).transform.position;
		nexPos2 = GameObject.Find(pos2GameObjectName).transform.position;
		nexPos3 = GameObject.Find(pos3GameObjectName).transform.position;
		nexPos4 = GameObject.Find(pos4GameObjectName).transform.position;
		nexPos5 = GameObject.Find(pos5GameObjectName).transform.position;
		nexPos6 = GameObject.Find(pos6GameObjectName).transform.position;
		
		PopulateNextPosDictionary(Int32.Parse(GameManager._gameState.bossAttemptDictionary["PRIMUS_NEX"].ToString()));
		
		mTiltX = Mathf.Clamp(mTiltX, -24.0f, 24.0f);
		mTiltX = Mathf.Lerp(mTiltX, 0.0f, Time.deltaTime * 2.0f);
		
		mTiltY = Mathf.Clamp(mTiltY, -24.0f, 24.0f);
		mTiltY = Mathf.Lerp(mTiltY, 0.0f, Time.deltaTime * 2.0f);
	}
	
	private Vector3 GetNextRandomPos(string currentPosName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("currentPosName"+currentPosName);
		System.Random rand = new System.Random();
		return nextPosDictionary[currentPosName][rand.Next(0, nextPosDictionary[currentPosName].Length)];
	}
	
	private string startPos;
	public void MoveEnemyToStartPos()
	{
		currentPos = startPos;
		enemyObject.transform.position = GetPosForPosName(currentPos);
		enemyObject.GetComponent<SmoothLookAt>().target = playerObject.transform;
		enemyObject.GetComponent<SmoothLookAt>().enabled = true;
		
		//TutorialManager.instance.ShowTutorial(TutorialManager.TutorialsAndCallback.NexCameraEnd);
	}
	
	
	private void PopulateNextPosDictionary(int attemptNumber)
	{
		List<Vector3> allPos1 = new List<Vector3>{nexPos2, nexPos3};
		List<Vector3> allPos2 = new List<Vector3>{nexPos2, nexPos3, nexPos6};
		List<Vector3> allPos3 = new List<Vector3>{nexPos1, nexPos4, nexPos6};
		List<Vector3> allPos4 = new List<Vector3>{nexPos1, nexPos4, nexPos5, nexPos6};
		
		List<Vector3> pos1List = new List<Vector3>();
		List<Vector3> pos2List = new List<Vector3>();
		List<Vector3> pos3List = new List<Vector3>();
		List<Vector3> pos4List = new List<Vector3>();
		List<Vector3> pos5List = new List<Vector3>();
		List<Vector3> pos6List = new List<Vector3>();

		switch(attemptNumber)
		{
		case 0:
		case 1:
			pos2List = new List<Vector3>(allPos1);
			pos3List = new List<Vector3>(allPos1);
			startPos = POS2;
			break;
		case 2:
		case 3:
			pos2List = new List<Vector3>(allPos2);
			pos3List = new List<Vector3>(allPos2);
			pos6List = new List<Vector3>(allPos2);
			startPos = POS2;
			break;
		case 4:
			pos1List = new List<Vector3>(allPos3);
			pos4List = new List<Vector3>(allPos3);
			pos6List = new List<Vector3>(allPos3);
			startPos = POS1;
			break;
		case 5:
		default:
			pos1List = new List<Vector3>(allPos4);
			pos4List = new List<Vector3>(allPos4);
			pos5List = new List<Vector3>(allPos4);
			pos6List = new List<Vector3>(allPos4);
			startPos = POS1;
			break;	
		}
		
		pos1List.Remove(nexPos1);
		pos2List.Remove(nexPos2);
		pos3List.Remove(nexPos3);
		pos4List.Remove(nexPos4);
		pos5List.Remove(nexPos5);
		pos6List.Remove(nexPos6);
		
		nextPosDictionary[POS1] = pos1List.ToArray();
		nextPosDictionary[POS2] = pos2List.ToArray();
		nextPosDictionary[POS3] = pos3List.ToArray();
		nextPosDictionary[POS4] = pos4List.ToArray();
		nextPosDictionary[POS5] = pos5List.ToArray();
		nextPosDictionary[POS6] = pos6List.ToArray();
	}

	
	
	public void Move()
	{
		Vector3 nextNexPos = GetNextRandomPos(currentPos);
		string nextPosName = GetPosNameForPos(nextNexPos);
			
		Vector3 explosionPos = new Vector3(enemyObject.transform.position.x, enemyObject.transform.position.y + 3.5f, enemyObject.transform.position.z);
		DisappearNexAndAppear(explosionPos, nextNexPos);
	}
	
	private void DisappearNexAndAppear(Vector3 currentNexPos, Vector3 nextNexPos)
	{
		GameObject.Instantiate(explosionPrefab, currentNexPos, Quaternion.identity);
		explosionPrefab.transform.localScale = new Vector3(4f, 4f, 4f);
		StartCoroutine(MoveObject(enemyObject, nextNexPos, 0.5f, 0.0f));
	}
	
	private IEnumerator MoveObject(GameObject gObject, Vector3 newPos, float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		TweenPosition.Begin(gObject, duration, newPos);
		currentPos = GetPosNameForPos(newPos);
	}
	
	private IEnumerator RotateObject(GameObject gObject, Quaternion quaternionRot, float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		TweenRotation.Begin(gObject, duration, quaternionRot);
	}
	
	private Vector3 GetPosForPosName(string posName)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("posName" + posName);
		
		switch(posName)
		{
		case POS1:
			return nexPos1;
		case POS2:
			return nexPos2;
		case POS3:
			return nexPos3;
		case POS4:
			return nexPos4;
		case POS5:
			return nexPos5;
		case POS6:
			return nexPos6;
		}
		return nexPos1;
	}
	
	private string GetPosNameForPos(Vector3 pos)
	{
		if(pos == nexPos1)
			return POS1;
		else if(pos == nexPos2)
			return POS2;
		else if(pos == nexPos3)
			return POS3;
		else if(pos == nexPos4)
			return POS4;
		else if(pos == nexPos5)
			return POS5;
		else if(pos == nexPos6)
			return POS6;
		else
			return null;
	}
	
	public void DisableTilt()
	{
	}
	
	/*
	void Update()
	{
		mTiltX = cameraObj.transform.localEulerAngles.y * TILT_SENSITIVITY_X;
		mTiltY = cameraObj.transform.localEulerAngles.x * TILT_SENSITIVITY_Y;
		
		if(allowTilt)
		{
			if(Input.GetMouseButton(0) && Application.isEditor)
			{
				mTiltX += Input.GetAxis("Mouse X") * TILT_SENSITIVITY_X;
				mTiltY -= Input.GetAxis("Mouse Y") * TILT_SENSITIVITY_Y;
			}
			
			else if(Input.touchCount==1 &&  Input.GetTouch(0).phase != TouchPhase.Canceled)
			{
				mTiltX += Input.GetAxis("Mouse X") * TILT_SENSITIVITY_X;
				mTiltY -= Input.GetAxis("Mouse Y") * TILT_SENSITIVITY_Y;
			}
			cameraObj.transform.localEulerAngles = new Vector3(mTiltY, mTiltX, 0);
		}
		if(playerObject != null)
			playerObject.transform.localEulerAngles = new Vector3(playerObject.transform.localEulerAngles.x, cameraObj.transform.localEulerAngles.y - 180, playerObject.transform.localEulerAngles.z);
	}*/
	
	void Update()
	{
		mTiltX = cameraObj.transform.localEulerAngles.y;
		mTiltY = cameraObj.transform.localEulerAngles.x;
		
		if(allowTilt)
		{
			if(Input.GetMouseButton(0) && Application.isEditor/* && false*/)
			{
				mTiltX += Input.GetAxis("Mouse X") * TILT_SENSITIVITY_X;
				mTiltY -= Input.GetAxis("Mouse Y") * TILT_SENSITIVITY_Y;
			}
			
			else if(Input.touchCount==1 &&  Input.GetTouch(0).phase != TouchPhase.Canceled && Input.GetTouch(0).phase != TouchPhase.Began)
			{
				mTiltX += Input.GetTouch(0).deltaPosition.x * TILT_SENSITIVITY_X * TOUCH_SENSITIVITY;
				mTiltY -= Input.GetTouch(0).deltaPosition.y * TILT_SENSITIVITY_Y * TOUCH_SENSITIVITY;
			}
			cameraObj.transform.localEulerAngles = new Vector3(mTiltY, mTiltX, 0);
		}
		if(playerObject != null)
			playerObject.transform.localEulerAngles = new Vector3(playerObject.transform.localEulerAngles.x, cameraObj.transform.localEulerAngles.y - 180, playerObject.transform.localEulerAngles.z);
	}
}
