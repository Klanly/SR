using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net;

public static class Helpers
{
	
    //FileLoadedIntoPersistant _fileLoaded;
    public static string AddQuotes(string aString)
    {
        return "\"" + aString + "\"";
    }
	
    //For Writing to IOS, it takes url without "file://"
    public static string formatLocalPersistentUrlToWrite(string filename)
    {
        string url;
        if (Application.platform == RuntimePlatform.Android)
        {
            url = Application.persistentDataPath + "/" + filename;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            url = Application.persistentDataPath + "/" + filename;
        } else
        {
            url = "file://" + Application.persistentDataPath + "/" + filename;
        }
        return url;
    }
	
    //For Reading from IOS,it takes url with "file://"
    public static string formatLocalPersistentUrlToRead(string filename)
    {
        string url;
        if (Application.platform == RuntimePlatform.Android)
        {
            url = "file://" + Application.persistentDataPath + "/" + filename;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            url = "file://" + Application.persistentDataPath + "/" + filename;
        } else
        {
            url = "file://" + Application.streamingAssetsPath + "/" + filename;
        }
        return url;
    }
	
    //For Reading from IOS,it takes url with "file://"
    public static string formatLocalPersistentUrlToReadWithoutFile(string filename)
    {
        string url;
        if (Application.platform == RuntimePlatform.Android)
        {
            url = "file://" + Application.persistentDataPath + "/" + filename;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            url = Application.persistentDataPath + "/" + filename;
        } else
        {
            url = Application.streamingAssetsPath + "/" + filename;
        }
        return url;
    }
	
    //For Testing in editor, For reading it takes url with "file://"
    public static string formatLocalUrlToRead(string filename)
    {
		
        string url;
		
        if (Application.platform == RuntimePlatform.Android)
        {
            url = "jar:file://" + Application.dataPath + "!/assets/" + filename;
        } else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            url = "file://" + Application.streamingAssetsPath + "/" + filename;
        } else
        {
            url = "file://" + Application.streamingAssetsPath + "/" + filename;
        }
        return url;
    }
    //For Testing in Editor, It takes url without the "file://"
    public static string formatLocalUrlToWrite(string filename)
    {
        string url;
		
        if (Application.platform == RuntimePlatform.Android)
        {
            url = "jar:file://" + Application.dataPath + "!/assets/" + filename;
        } else
        {
            url = Application.streamingAssetsPath + "/" + filename;
        }

        return url;
    }
	
    public static void formatLocalUrlForAssetBundle(string filename)
    {
		
    }
	
    public static int CheckForSkullLevelBounds(int skullMod)
    {
        int totalSkullLevel = skullMod + GameManager._gameState.skullLevel;
		
        if (totalSkullLevel <= DatabankSystem.Databank.NUMBER_OF_LEVELS)
        {
            return totalSkullLevel;
        } else
        {
            return DatabankSystem.Databank.NUMBER_OF_LEVELS;
        }
		
    }
	
    public static int CheckForSkullLevelValidatity(int skullLevel)
    {
        if (skullLevel > DatabankSystem.Databank.NUMBER_OF_LEVELS)
        {
            skullLevel = 35;
        }
        if (skullLevel < 1)
        {
            skullLevel = 1;
        }
		
        return skullLevel;
    }
	
    public static void Shuffle<T>(this IList<T> list)
    {  
        System.Random rng = new System.Random();  
        int n = list.Count;  
        while (n > 1)
        {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list [k];  
            list [k] = list [n];  
            list [n] = value;  
        }  
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
	
	
    public static string Encrypt(string toEncrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        // 256-AES key
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
 
    public static string Decrypt(string toDecrypt)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        // AES-256 key
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        // http://msdn.microsoft.com/en-us/library/system.security.cryptography.ciphermode.aspx
        rDel.Padding = PaddingMode.PKCS7;
        // better lang support
        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
	
	#region UnixTime
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
	
    /// <summary>
    ///  Convert froms the unix(Epoc) time to the DataDate.
    /// </summary>
    /// <returns>The unix DataTime.</returns>
    /// <param name="unixTime">Unix time.</param>
    public static DateTime FromUnixTime(long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddMilliseconds(unixTime);
    }
	
    public static long GetTimeFromNow(long _timeFromPast)
    {
        Debug.Log("GetTimeFromNow: " + _timeFromPast + "\nUnixTime: " + FromUnixTime(_timeFromPast)
            + "\nNow: " + DateTime.Now.ToUniversalTime() + "\nReturn: " + ((long)(DateTime.Now.ToUniversalTime() - FromUnixTime(_timeFromPast)).TotalSeconds));
        return ((long)(DateTime.Now.ToUniversalTime() - FromUnixTime(_timeFromPast)).TotalSeconds);
    }

    public static long GetTimeNowInMilliseconds
    {
        get
        {
//            return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            return (DateTime.Now.ToUniversalTime().Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
	
/*    public static long GetTimeFromServer(long _timeOfServer, long _timeFromPast)
    {
        Debug.Log("GetTimeFromServer: " + _timeFromPast + "\nUnixTime: " + FromUnixTime(_timeFromPast)
            + "\nNow: " + DateTime.Now.ToUniversalTime() + "\nReturn: " + ((long)(DateTime.Now.ToUniversalTime() - FromUnixTime(_timeFromPast)).TotalSeconds));
        return ((long)(FromUnixTime(_timeOfServer) - FromUnixTime(_timeFromPast)).TotalSeconds);
    }
	
    public static long GetTimeFromServer(DateTime _timeOfServer, long _timeFromPast)
    {
        Debug.Log("GetTimeFromServer: " + _timeFromPast + "\nUnixTime: " + FromUnixTime(_timeFromPast)
            + "\nNow: " + DateTime.Now.ToUniversalTime() + "\nReturn: " + ((long)(DateTime.Now.ToUniversalTime() - FromUnixTime(_timeFromPast)).TotalSeconds));
        return ((long)(_timeOfServer - FromUnixTime(_timeFromPast)).TotalSeconds);
    }*/
    
    public static long GetTimeDifferenceInSeconds(long _fromTime, long _toTime)
    {
        long _diff = Math.Abs( _fromTime - _toTime);
        return (_diff / 1000);	// converting milliseconds to seconds
    }
	#endregion UnixTime
	
    public static int Compute2DAngle(Vector3 toObj, Vector3 fromObj)
    {
        float rise = (toObj.y - fromObj.y);
        float run = (toObj.x - fromObj.x);
		
        float radians = Mathf.Atan(Mathf.Abs(rise / run));
		
        int angle = (int)(Mathf.Rad2Deg * radians);
		
        switch (toObj.y > fromObj.y)
        {
            case true:
                if (toObj.x > fromObj.x) 					//first Quadrant
                    angle = 90 - angle;
                else 										//fourth quadrant
                    angle = 270 + angle;
                break;
            case false:
                if (toObj.x > fromObj.x) 					//second Quadrant
                    angle = 90 + angle;
                else 										//third Quadrant
                    angle = 180 + (90 - angle);
                break;
        }

        return angle;
    }

	public static bool IsConnected(string hostedURL="http://www.google.com")
	{
		try{
			string HtmlText = GetHtmlFromUri(hostedURL);
			if(HtmlText == "" )
				return false;
			else
				return true;
		}
		catch(IOException ex)
		{
			return false;
		}
	} 
	
	public static string GetHtmlFromUri(string resource)
	{
		string html = string.Empty;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess)
				{
					using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
					{
						//We are limiting the array to 80 so we don't have
						//to parse the entire html document feel free to 
						//adjust (probably stay under 300)
						char[] cs = new char[80];
						reader.Read(cs, 0, cs.Length);
						foreach(char ch in cs)
						{
							html +=ch;
						}
					}
				}
			}
		}
		catch
		{
			return "";
		}
		return html;
	}

}
