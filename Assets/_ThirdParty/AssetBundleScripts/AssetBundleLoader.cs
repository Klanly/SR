//using UnityEditor;
using UnityEngine;
using System;
using System.Collections;

public class AssetBundleLoader : MonoBehaviour {
   public string url=null;
   public int version;
   AssetBundle bundle;
	
	private string levelName;
	//GameObject level1;
	public delegate void AssetBundleLoadedDelegate(AssetBundle levelbundle);
	public bool isDownloading = false;
	
	private AssetBundleLoadedDelegate myAssetBundleDel;
	
	public void SetDelegate(AssetBundleLoadedDelegate myBundle)
	{
		this.myAssetBundleDel=myBundle;
	}
	
	void Start()
	{
		//version=354;
		version = BundleDownloadManager.instance.version;
	}
	
	public void InvokeLoadAssetBundle(string filename)
	{
//		Debug.Log("InvokeLoadAssetBundle = " + filename);
		Console.WriteLine("InvokeLoadAssetBundle = " + filename);

		levelName = filename;
		formatUrl(filename);
		
//		if(GameManager.PRINT_LOGS) Debug.Log(" BUNDLE load request for >>>>!!!>>" + filename);
		
		if(IsInvoking("LoadAssetBundle"))
		{
			Console.WriteLine("41 IS INVOKING = " + filename);
			if(GameManager.PRINT_LOGS) Debug.Log("LOADED BUNDLE ALREADY!!!");
			
			CancelInvoke("LoadAssetBundle");	
		}
		

		Console.WriteLine("48 ASset bundle INVOKE = " + filename);
		Invoke("LoadAssetBundle",0.0f);
		
	}
	
	public void InvokeLoadPrefabAssetBundle(string filename)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("InvokeLoadPrefabAssetBundle ::: " + filename);
		
		levelName = filename;
		formatPrefabUrl(filename);
		
//		if(GameManager.PRINT_LOGS) Debug.Log(" Prefab BUNDLE load request for >>>>!!!>>" + filename);
		
		if(IsInvoking("LoadAssetBundle"))
		{
			if(GameManager.PRINT_LOGS) Debug.Log("LOADED Prefab BUNDLE ALREADY!!!");
			
			CancelInvoke("LoadAssetBundle");	
		}
		
		Invoke("LoadAssetBundle",0.0f);
		
	}
	
	void formatUrl(string filename)
	{
		/*
		if(Application.platform==RuntimePlatform.Android)
		{
   			//url = "jar:file://" + Application.dataPath + "!/assets/"+filename+"_bundle_android.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Android/Levels/"+filename+".unity3d";
		}
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			//url= "file://"+Application.streamingAssetsPath+"/"+filename+"_bundle_iphone.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Iphone/Levels/"+filename+".unity3d";
		}
		else
		{
			//url= "file://"+Application.streamingAssetsPath+"/"+filename+"_bundle_iphone.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Iphone/Levels/"+filename+".unity3d";
		}
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("formatUrl = " + filename);
		url = BundleDownloadManager.instance.GetFormatedURL(filename);
	}
	
	void formatPrefabUrl(string filename)
	{/*
		if(Application.platform==RuntimePlatform.Android)
		{
   			//url = "jar:file://" + Application.dataPath + "!/assets/"+filename+"_bundle_android.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Android/Prefabs/"+filename+".unity3d";
		}
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
		{
			//url= "file://"+Application.streamingAssetsPath+"/"+filename+"_bundle_iphone.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Iphone/Prefabs/"+filename+".unity3d";
		}
		else
		{
			//url= "file://"+Application.streamingAssetsPath+"/"+filename+"_bundle_iphone.unity3d";
			url= "https://s3-us-west-2.amazonaws.com/staging-streaming-assets/Iphone/Prefabs/"+filename+".unity3d";
		}
		*/
		if(GameManager.PRINT_LOGS) Debug.Log("formatPREFABUrl = " + filename);
		url = BundleDownloadManager.instance.GetFormatedURL(filename);
	}
	
	void LoadAssetBundle()
	{
		Console.WriteLine("LoadAssetBundle >> levelName > + 120");
		if(GameManager.PRINT_LOGS) Debug.Log(url);
		version = BundleDownloadManager.instance.GetVersionForLevelName(levelName);
		Console.WriteLine("BundleDownloadManager.instance.GetVersionForLevelName    123   " + version);
//		Debug.Log("loading Level--- " +url + " VERSION: "+ version);

		Console.WriteLine("url = " + url);

		bundle = AssetBundleManager.getAssetBundle (url, version);
       
		if(!bundle)
		{
			Console.WriteLine("NOT bundle = 132");

			BundleDownloadManager.instance.PauseDownloader();
			Console.WriteLine("NOT bundle = 135");
           StartCoroutine(DownloadAB());
			if(!Caching.IsVersionCached(url,version)){
				Console.WriteLine("FALSE CACHING  = 138");
				if(GameManager.instance == null || GameManager.instance.scaleformCamera == null || GameManager.instance.scaleformCamera.generalSwf == null) return;
//				GameManager.instance.scaleformCamera.generalSwf.LoadingScreenShowBar(levelName);
				//GameManager.instance.scaleformCamera.generalSwf.LoadingPercentage(0);
			}
		}
		else
		{
			if(GameManager.PRINT_LOGS) Debug.Log("ASSET AlREADTY EXISTS!!!!");	
			Console.WriteLine("ELSE  = 146");
			this.myAssetBundleDel(bundle);
		}
	}
	
	

//   void OnGUI (){
//       if(GUILayout.Button("Download bundle")){
//          
//       }
//   }
   public IEnumerator DownloadAB(){
		InvokeRepeating("LoadingPercentage",1,0.5f);
		isDownloading = true;
//		Debug.LogError("Download ab 160");
       yield return StartCoroutine(AssetBundleManager.downloadAssetBundle (url, version));
		if(GameManager.instance != null && GameManager.instance.scaleformCamera != null && GameManager.instance.scaleformCamera.generalSwf != null)
		{
//			Console.WriteLine("Download ab 166");
//			Debug.LogError("Download ab 166");
			//			GameManager.instance.scaleformCamera.generalSwf.LoadingPercentage(100);
//			GameManager.instance.scaleformCamera.generalSwf.LoadingScreenHideBar();
		}
//		Debug.LogError("Download ab      167");
		CancelInvoke("LoadingPercentage");
		
		isDownloading = false;
		bundle = AssetBundleManager.getAssetBundle (url, version);
//		Debug.LogError("Download ab      172");
		this.myAssetBundleDel(bundle);	
		BundleDownloadManager.instance.ResumeDownloader();
   }
	
	public void getLoadedAssetBundle(string filename)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("getLoadedAssetBundle =" + filename);
		formatUrl(filename);
		bundle = AssetBundleManager.getAssetBundle (url, version);
		this.myAssetBundleDel(bundle);
	}
	
	
   public void UnloadAssetBundle (string filename)
	{	
		if(GameManager.PRINT_LOGS) Debug.Log("UnloadAssetBundle:::filename = " + filename);
		 
		version = BundleDownloadManager.instance.GetVersionForLevelName(filename);
		if(GameManager.PRINT_LOGS) Debug.Log("UnloadAssetBundle:::version = " + version);
		formatUrl(filename);
       AssetBundleManager.Unload (url, version,true);
    }
	
	public void UnloadAssetBundleWithoutObject (string filename)
	{
		if(GameManager.PRINT_LOGS) Debug.Log("UnloadAssetBundleWithoutObject =" + filename);
		version = BundleDownloadManager.instance.GetVersionForLevelName(filename);
		formatUrl(filename);
		if(GameManager.PRINT_LOGS) Debug.Log("Unload URl=  " + url + "   verSion = " + version);
       AssetBundleManager.Unload (url, version,false);
    }
	
	public void LoadingPercentage(){
		if(!AssetBundleManager.isDisposed){
			GameManager.instance.scaleformCamera.generalSwf.LoadingPercentage((int)AssetBundleManager.GetDownloadProgress());
		}
		else{
			CancelInvoke("LoadingPercentage");
		}
	}
}