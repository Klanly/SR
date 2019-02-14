using UnityEngine;
using System.Collections;

public class LevelCameraHandler{

//	 public Camera entryCamera
//	{
//		set;get;
//	}
	public Camera defaultCamera
	{
		set;get;
	}
	public Camera ActiveCamera
	{
		set;get;
	}
	public UIManager sfCamera
	{
		set;get;
	}
	public bool cameraLoaded
	{
		get;set;
	}
	
	public delegate void CameraLoadedForNextPOI(bool isLoaded);
	
	public CameraLoadedForNextPOI _poiCam
	{
		get;set;
	}
	
	public IEnumerator LoadCameraAfterWaitForTime(float time,Camera CameraType)
	{

		yield return new WaitForSeconds(time);

		this.ActiveCamera.gameObject.SetActive(false);
		
		this.ActiveCamera.tag=null;
		
		CameraType.gameObject.SetActive(true);
		
		this.ActiveCamera=CameraType;
		
		this.ActiveCamera.tag="MainCamera";
		
		this.cameraLoaded=true;
		
		_poiCam(true);
	}
	
	public void ResetCameras()
	{
		this.defaultCamera=null;
		this.ActiveCamera=null;
		//this.sfCamera=null;
		this.cameraLoaded=false;
		
		
	}
}
