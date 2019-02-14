using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ScaleformUtils : MonoBehaviour {


	public  static Byte[] getSwfInBytes(string swfName)
	{
		Byte[] swfBytes = null;
		
		//string filePath = SFManager.GetScaleformContentPath() + swfName;
		string filePath;
		if(GameManager.PRINT_LOGS) Debug.Log("***********SWFPATH===="+Application.persistentDataPath+"/PersistentSwfs/"+swfName);
		if(Application.isEditor)
		{
			filePath = Application.streamingAssetsPath+"/PersistentSwfs/"+swfName;
		}
		else
		{
			filePath =Application.persistentDataPath+"/PersistentSwfs/"+swfName;
		}
		//string filePath = System.IO.Path.Combine(Application.persistentDataPath, "/PersistentSwfs/"+swfName);
		
		if(GameManager.PRINT_LOGS) Debug.Log (filePath);
		
		using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader reader = new BinaryReader(fileStream))
			{
				swfBytes = reader.ReadBytes((int)fileStream.Length);
			}
		}
		
		return swfBytes;
	}

	public  static Byte[] getImageInBytes(string imageName)
	{
		Byte[] swfBytes = null;
		
		//string filePath = SFManager.GetScaleformContentPath() + swfName;
		string filePath;
		if(GameManager.PRINT_LOGS) Debug.Log("***********SWFPATH===="+Application.streamingAssetsPath+"/SwfImages/RingsSmall/"+imageName);
		if(Application.isEditor)
		{
			filePath = Application.streamingAssetsPath+"/SwfImages/RingsSmall/"+imageName;
		}
		else if(Application.platform==RuntimePlatform.Android)
		{
			filePath ="jar:file://" + Application.dataPath + "!/assets/SwfImages/RingsSmall/"+imageName;
		}
		else
		{
			filePath = Application.streamingAssetsPath+"/SwfImages/RingsSmall/"+imageName;
		}
		//string filePath = Helpers.formatLocalUrlToRead(imageName);
		
		if(GameManager.PRINT_LOGS) Debug.Log (filePath);
		
		using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		{
			using (BinaryReader reader = new BinaryReader(fileStream))
			{
				swfBytes = reader.ReadBytes((int)fileStream.Length);
			}
		}
		
		return swfBytes;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
