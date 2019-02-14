using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {
	
	/*
	 * POSITION MAP...
	 *
     *       TRANSFORM1 **************** TRANSFORM2 **************** TRANSFORM3
	 *        **                                          **
	 *        **                                          **
	 *        **                                          **
	 *       TRANSFORM4                                        TRANSFORM5
	 * 
	 */
	
	public string enemyName;
	
	private GameObject nexObject;
	public GameObject playerObject;
	
	public Transform nexTrans1;
	public Transform nexTrans2;
	public Transform nexTrans3;
	public Transform nexTrans4;
	public Transform nexTrans5;
	
	public Vector3 playerPos = new Vector3(-1f, 0f, -2f);
	
	const string TRANSFORM1 = "nexTransform1";
	const string TRANSFORM2 = "nexTransform2";
	const string TRANSFORM3 = "nexTransform3";
	const string TRANSFORM4 = "nexTransform4";
	const string TRANSFORM5 = "nexTransform5";

	public Dictionary<string, Transform[]> nextTransformDictionary = new Dictionary<string, Transform[]>();

	public float TRANSITION_TIME = 1.0f;
	
	public GameObject explosionPrefab;
	
	void Start ()
	{
		PopulateNextPosDictionary();
		Transform initialTransform = GetTransformForPosName(TRANSFORM1);
		nexObject.transform.position = initialTransform.position;
		nexObject.transform.rotation = initialTransform.rotation;
		nexObject.transform.localScale = initialTransform.localScale;
	}
	
	private Transform GetNextRandomTransform(string currentPosName)
	{
		System.Random rand = new System.Random();
		return nextTransformDictionary[currentPosName][rand.Next(0, nextTransformDictionary[currentPosName].Length)];
	}
	
	private void PopulateNextPosDictionary()
	{
		Transform[] trans1List = {nexTrans2, nexTrans4};
		nextTransformDictionary[TRANSFORM1] = trans1List;
		
		Transform[] trans2List = {nexTrans1, nexTrans4, nexTrans3, nexTrans5};
		nextTransformDictionary[TRANSFORM2] = trans2List;
		
		Transform[] trans3List = {nexTrans2, nexTrans5};
		nextTransformDictionary[TRANSFORM3] = trans3List;
		
		Transform[] trans4List = {nexTrans1, nexTrans2};
		nextTransformDictionary[TRANSFORM4] = trans4List;
		
		Transform[] trans5List = {nexTrans2, nexTrans3};
		nextTransformDictionary[TRANSFORM5] = trans5List;
	}
	
	public void Move()
	{
		string currentPosName = GetTransformNameForTransform(nexObject.transform);
		if(GameManager.PRINT_LOGS) Debug.Log("currentPosName" + currentPosName);
		Transform nextNexTransform = GetNextRandomTransform(currentPosName);
		
		TweenPosition.Begin(nexObject, 0.1f, nextNexTransform.position);
	}

	
	private IEnumerator MoveObject(GameObject gObject, Vector3 newPos, float delay, float duration)
	{
		yield return new WaitForSeconds(delay);
		TweenPosition.Begin(gObject, duration, newPos);
	}
	
	private Transform GetTransformForPosName(string posName)
	{
		switch(posName)
		{
		case TRANSFORM1:
			return nexTrans1;
		case TRANSFORM2:
			return nexTrans2;
		case TRANSFORM3:
			return nexTrans3;
		case TRANSFORM4:
			return nexTrans4;
		case TRANSFORM5:
			return nexTrans5;
		}
		return nexTrans1;
	}
	
	private string GetTransformNameForTransform(Transform transform)
	{
		if(transform == nexTrans1)
			return TRANSFORM1;
		else if(transform == nexTrans2)
			return TRANSFORM2;
		else if(transform == nexTrans3)
			return TRANSFORM3;
		else if(transform == nexTrans4)
			return TRANSFORM4;
		else if(transform == nexTrans5)
			return TRANSFORM5;
		else
			return null;
	}
	
	void OnGUI()
	{
//		 if(GUI.Button(new Rect(10, 70, 100, 100), "Move Nex!"))
//            Move();
	}
}
