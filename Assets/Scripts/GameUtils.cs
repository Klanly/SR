using UnityEngine;
using System;
using System.Collections;

public static class GameUtils
{

	public static T ParseEnum<T> (string value)
	{
		return (T)Enum.Parse (typeof(T), value, true);
	}

	public static void ClearTransform (Transform transform)
	{
		int childs = transform.childCount;
        
		for (int i = childs - 1; i >= 0; i--) {
			GameObject.Destroy (transform.GetChild (i).gameObject);
		}
	}
	#region UnixTime
	/// <summary>
	///  Convert froms the unix(Epoc) time to the DataDate.
	/// </summary>
	/// <returns>The unix DataTime.</returns>
	/// <param name="unixTime">Unix time.</param>
	public static DateTime FromUnixTime (long unixTime)
	{
		var epoch = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return epoch.AddMilliseconds (unixTime);
	}

	public static long GetTimeFromNow (long _timeFromPast)
	{
//		Debug.Log("GetTimeFromNow: " + _timeFromPast + "\nUnixTime: " + GameUtils.FromUnixTime(_timeFromPast)
//		          + "\nNow: " + DateTime.Now.ToUniversalTime() + "\nReturn: " + ((long) (DateTime.Now.ToUniversalTime() - FromUnixTime(_timeFromPast)).TotalSeconds) );
		return ((long)(DateTime.Now.ToUniversalTime () - FromUnixTime (_timeFromPast)).TotalSeconds);
	}
	
	public static long GetTimeDifferenceInSeconds (long _fromTime, long _toTime)
	{
		long _diff = _fromTime - _toTime;
		return (_diff / 1000);	// converting milliseconds to seconds
	}
	
	/// <summary>
	/// Gets the current time stamp with randomness.
	/// </summary>
	/// <returns>The current time stamp with randomness.</returns>
	/// <param name="flag">If set to <c>true</c> flag (Length=18), otherwise <c>false</c> (Length=14).</param>
	public static string GetCurrentTimeStampWithRandomness (bool flag = false)
	{
//		string timeStampNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
		string timeStampNow = DateTime.Now.ToString ("yyyyMMddHHmmss"); // makes the string of length 14
		if (flag) { // adds 4 random digits to the end and total length becomes 18
			timeStampNow += UnityEngine.Random.Range (1000, 10000).ToString ();
//			timeStampNow += SystemInfo.deviceUniqueIdentifier;
		}
		return timeStampNow;
	}
	#endregion UnixTime
	
	#region ConvertTime
	public static string ConvertTime (int Seconds)
	{
		string ToReturn = string.Empty;
		TimeSpan t = TimeSpan.FromSeconds (Seconds);
		
		bool Days = false;
		if (t.Days > 0) {
			if (t.Days < 10) {
				ToReturn = string.Format ("{0}d ", t.Days);
			} else {
				ToReturn = string.Format ("{0:D2}d ", t.Days);
			}
			Days = true;
		}
		
		if (t.Hours > 0) {
			if (t.Hours < 10) {
				ToReturn += string.Format ("{0}h ", t.Hours);
			} else {
				ToReturn += string.Format ("{0:D2}h ", t.Hours);
			}
		}
		
		
		if (t.Minutes > 0) {
			if (t.Minutes < 10) {
				ToReturn += string.Format ("{0}m ", t.Minutes);
			} else {
				ToReturn += string.Format ("{0:D2}m ", t.Minutes);
			}
		}
		
		if (t.Seconds > 0 && !Days) {
			if (t.Seconds < 10) {
				ToReturn += string.Format ("{0}s", t.Seconds);
			} else {
				ToReturn += string.Format ("{0:D2}s", t.Seconds);
			}
		}
		return ToReturn = ToReturn == string.Empty ? "0s" : ToReturn;
	}
	#endregion ConvertTime
	
	public static int ResourceToGemCost (int resource, int gemRatio)
	{
		return ((int)(resource / gemRatio)) > 0 ? ((int)(resource / gemRatio)) : 1;
	}
//	public static int RefundResourceAmount(int percentage, int totalCost) {
//		return (int)((percentage/100f) * totalCost);
//	}
	public static int RefundResourceAmount (float percentage, int totalCost)
	{
		return (int)(percentage * totalCost);
	}
	
	public static Mesh GetMeshOfObject (string _objectName, string _path = "")
	{
//		Debug.Log("GetMeshOfObject: " + _path + _objectName);
		return (Resources.Load (_path + _objectName) as GameObject).GetComponent<MeshFilter> ().mesh;
//		return ( Resources.Load(_path + _objectName) as Mesh);	// not works
//		return ( Resources.Load<GameObject>(_path + _objectName) ).GetComponent<MeshFilter>().mesh;	// it adds Instance word with mesh
	}
	
	public static GameObject GetGameObject (GameObject parentObject, string _objectName, int level, string _path = "", string _previousObjName = "", int _previousLevel = 0, bool showRotated = true)
	{
//		string fileNameOld = _previousObjName + "_lvl_" + _previousLevel.ToString() + "(Clone)";
//		Debug.Log("GetGameObject-0: " + parentObject.name + "   => " + _objectName + "   => " + level.ToString() + "\nPath: " + _path + _previousObjName + "_lvl_" + _previousLevel.ToString() + "(Clone)");
		if (_previousObjName != "" && _previousLevel != 0 && parentObject.transform.FindChild (_previousObjName + "_lvl_" + _previousLevel.ToString () + "(Clone)") != null) { // object already exists in hierarchy
//			Debug.Log("GetGameObject-1: " + _previousObjName + "_lvl_" + _previousLevel.ToString() + "(Clone)");
			MonoBehaviour.Destroy (parentObject.transform.FindChild (_previousObjName + "_lvl_" + _previousLevel.ToString () + "(Clone)").gameObject);
		} else
		if (parentObject.transform.FindChild (_objectName + "_lvl_" + (level - 1).ToString () + "(Clone)") != null) { // object already exists in hierarchy
//			Debug.Log("GetGameObject-2: " + _objectName + "_lvl_" + (level-1).ToString() + "(Clone)");
			MonoBehaviour.Destroy (parentObject.transform.FindChild (_objectName + "_lvl_" + (level - 1).ToString () + "(Clone)").gameObject);
		}
		string fileName = _path + _objectName + "_lvl_" + level.ToString ();
//		if (GameObject.Find( fileName + "(Clone)" ) == null)
		if (Resources.Load (fileName) == null) {
			Debug.Log ("--- GetGameObject: " + parentObject.name + "\nFile: " + fileName);
			return null;
		}
		if (parentObject.transform.FindChild (_objectName + "_lvl_" + level.ToString () + "(Clone)") == null) {
//			Debug.Log("=== GetGameObject: " + parentObject.name + "\nFile: " + fileName + "\nPrevious: " + _previousObjName + "_lvl_" + _previousLevel.ToString() );
//			Debug.Log("=== GetGameObject: " + parentObject.name + "\nFile: " + fileName + "\nPrevious: " + _previousObjName + "_lvl_" + _previousLevel.ToString() );
//			return GameObject.Instantiate( Resources.Load(_path + _objectName + "_lvl_" + level.ToString()), parentObject.transform.position, parentObject.transform.rotation ) as GameObject;
			GameObject go = GameObject.Instantiate (Resources.Load (fileName), parentObject.transform.position, parentObject.transform.rotation) as GameObject;
			go.transform.SetParent (parentObject.transform);
			go.transform.localPosition = new Vector3 (0, 0.008f, 0);
			if (showRotated)
				go.transform.localRotation = Quaternion.Euler (new Vector3 (270, 0, 0));
			return go;
		} else 
			return null;
	}

	public static string Caesar(string value, int shift)
	{
		char[] buffer = value.ToCharArray();
		for (int i = 0; i < buffer.Length; i++)
		{
			// Letter.
			char letter = buffer[i];
			// Add shift to all.
			letter = (char)(letter + shift);
			// Subtract 26 on overflow.
			// Add 26 on underflow.
			if (letter > 'z')
			{
				letter = (char)(letter - 26);
			}
			else if (letter < 'a')
			{
				letter = (char)(letter + 26);
			}
			// Store.
			buffer[i] = letter;
		}
		return new string(buffer);
	}
	public static string Base64Encode(string plainText) {
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}
	public static string Base64Decode(string base64EncodedData) {
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
	}
}
