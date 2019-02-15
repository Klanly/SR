using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class AssetBundleManager {
   // A dictionary to hold the AssetBundle references
   static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
   static AssetBundleManager (){
       dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
   }
   // Class with the AssetBundle reference, url and version
   private class AssetBundleRef {
       public AssetBundle assetBundle = null;
       public int version;
       public string url;
       public AssetBundleRef(string strUrlIn, int intVersionIn) {
           url = strUrlIn;
           version = intVersionIn;
       }
   };
   // Get an AssetBundle
   public static AssetBundle getAssetBundle (string url, int version){
       string keyName = url + version.ToString();
       AssetBundleRef abRef;
       if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
		{
           	return abRef.assetBundle;
//			Debug.Log("FOUND BUNDLE = "+ url);
		}
       else
		{
           return null;
//			Debug.Log("DID NOT FIND BUNDLE = "+ url);
		}
   }
	
		/// <summary>
	/// Checks the bundle cache status.
	/// </summary>
	/// <returns>
	/// The bundle cache status.
	/// </returns>
	/// <param name='url'>
	/// If set to <c>true</c> URL.
	/// </param>
	public static bool CheckBundleCacheStatus(string url)
	{		
		if(Caching.IsVersionCached(url,1))
			return true;
		else
			return false;
	}
	
	
	/// <summary>
	/// Checks the bundle load status.
	/// </summary>
	/// <returns>
	/// The bundle load status.
	/// </returns>
	/// <param name='url'>
	/// If set to <c>true</c> URL.
	/// </param>
	public static bool CheckBundleLoadStatus(string url)
	{
		if(getAssetBundle(url,1))
			return true;
		else
			return false;
	}
   // Download an AssetBundle
	public static WWW assetBudleWWW;
	public static bool isDisposed = false;
	public static string urlDownloading;
	public static int versionDownloading;
   	public static IEnumerator downloadAssetBundle (string url, int version){

//		Debug.LogError(string.Format("url = {0} version = {1}     79", url, version));

		while (!Caching.ready)
			yield return null;

//		Debug.LogError(string.Format("url = {0} version = {1}    84", url, version));

		if(GameManager.PRINT_LOGS) Debug.Log("url"+url+version.ToString());
		
       string keyName = url + version.ToString();
       if (dictAssetBundleRefs.ContainsKey(keyName))
		{
//			Debug.LogError(string.Format(" CONTAINS KEY {0}", keyName));
           yield return null;
		}
       else {
           using( assetBudleWWW = WWW.LoadFromCacheOrDownload(url,version)){  //using(WWW www = new WWW(url)){
				isDisposed = false;
//				Debug.LogError(string.Format(" RETURNING AB {0}", url));
               yield return assetBudleWWW;
               if (assetBudleWWW.error != null){
//					Debug.LogError(string.Format(" ERROR 96"));
					//GameManager.instance.scaleformCamera.generalSwf.UnLoadLoadingScreen();
					urlDownloading = url;
					versionDownloading = version;
					yield return new WaitForSeconds(2.0f);
//					Debug.LogError(string.Format(" ERROR 101"));
					GameManager.instance.scaleformCamera.generalSwf.ShowNoConnectivityPopup("No Internet Connectivity");
					isDisposed = true;
//					throw new Exception("WWW download:" + assetBudleWWW.error);
					Debug.Log("WWW download:" + assetBudleWWW.error);
				}
//				Debug.LogError(string.Format(" AB     107"));
               AssetBundleRef abRef = new AssetBundleRef (url, version);
               abRef.assetBundle = assetBudleWWW.assetBundle;
               dictAssetBundleRefs.Add (keyName, abRef);
//				Debug.LogError(string.Format(" FINISHING AB {0}", keyName));
				//LoadLanguageIfApplicable(keyName, abRef.assetBundle);
           }
       }
   }
	
   // Unload an AssetBundle
   public static void Unload (string url, int version, bool allObjects){
//		Debug.Log("Unload ........ url = " + url  + "        version = " + version);
       string keyName = url + version.ToString();
       AssetBundleRef abRef;
       if (dictAssetBundleRefs.TryGetValue(keyName, out abRef)){
           abRef.assetBundle.Unload (allObjects);
           abRef.assetBundle = null;
           dictAssetBundleRefs.Remove(keyName);
       }
   }
	
	public static float GetDownloadProgress(){
		if(!isDisposed){
//			Debug.Log("Download Progress: " +assetBudleWWW.progress*100+"%");
			return assetBudleWWW.progress*100;
		}
		else{
			return 0.0f;
		}
	}
}