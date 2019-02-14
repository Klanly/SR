using UnityEngine;
using System.Collections;

public class TreasureObjectLoader : MonoBehaviour {
	
	public GameObject treasurePrefab1;
	public GameObject treasurePrefab2;
	public GameObject treasurePrefab3;
	
	public delegate void treasureModelDelegate (GameObject charGameObject, Transform point,int index);
	
	public void LoadTreasure(treasureModelDelegate del, Transform point,int index,string chestType)
	{	
		//GameObject charGameObject = GameObject.Instantiate(charMagePrefab, atPosition,atAngle) as GameObject;
		switch(chestType)
		{
		case "1 KEY":
		del(treasurePrefab1,point,index);
		break;
		case "3 KEYS":
		del(treasurePrefab2,point,index);
		break;
		case "5 KEYS":
		del(treasurePrefab3,point,index);
		break;
		}
	}
}
