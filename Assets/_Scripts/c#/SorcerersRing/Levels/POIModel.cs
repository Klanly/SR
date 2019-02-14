using UnityEngine;
using System.Collections;

public class POIModel {

	public string poiIndex
	{
		get;set;
	}
	
	public string poiId
	{
		get;set;
	}
	
	public string poiType
	{
		get;set;
	}
	
	public string poiProtectedBy
	{
		get;set;
	}
	
	public bool isCleared
	{
		get;set;
	}
	
	public string poiNextLevel
	{
		get;set;
	}
	
	public Transform poiPoint
	{
		get;set;
	}
	
	public Transform poiMoveTo
	{
		get;set;
	}
	
	public Camera poiCameraObject
	{
		get;set;
	}
	
	public GameObject poiObject
	{
		get;set;
	}
	
	public GameObject relatedObject
	{
		get;set;
	}
	
	public POIModel(string poiID, bool cleared)
	{
		this.poiId=poiID;
		this.isCleared=cleared;
	}
	public POIModel(string pointOfIntrestIndex,string poiID,string POIType,string protectedBy,string nextLevel,Transform POIPoint,Transform MoveTo,Camera POICamera,GameObject POIObject,bool cleared)
	{
		this.poiIndex=pointOfIntrestIndex;
		this.poiId=poiID;
		this.poiType=POIType;
		this.poiProtectedBy=protectedBy;
		this.poiNextLevel=nextLevel;
		this.poiPoint=POIPoint;
		this.poiCameraObject=POICamera;
		this.poiCameraObject.gameObject.SetActive(false);
		this.poiObject=POIObject;
		this.poiObject.SetActive(true);
		this.poiMoveTo=MoveTo;
		this.isCleared=cleared;
		//this.relatedObject=objectAgainstPoi;
	}
	
	
	
	
}
