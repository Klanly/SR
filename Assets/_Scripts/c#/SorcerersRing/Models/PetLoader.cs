using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetLoader : MonoBehaviour
{
	
	public GameObject IMP1Prefab;
	public GameObject IMP2Prefab;
	public GameObject IMP3Prefab;
	public GameObject IMP4Prefab;
	public GameObject IMP5Prefab;

	public GameObject AQUA1Prefab;
	public GameObject AQUA2Prefab;
	public GameObject AQUA3Prefab;
	public GameObject AQUA4Prefab;
	public GameObject AQUA5Prefab;
    
	public GameObject GOLEM1Prefab;
	public GameObject GOLEM2Prefab;
	public GameObject GOLEM3Prefab;
	public GameObject GOLEM4Prefab;
	public GameObject GOLEM5Prefab;

	public GameObject DRAKE1Prefab;
	public GameObject DRAKE2Prefab;
	public GameObject DRAKE3Prefab;
	public GameObject DRAKE4Prefab;
	public GameObject DRAKE5Prefab;
	
	private Dictionary<string, GameObject> petIdToGameObjectDictionary;
	
	void Awake ()
	{
		petIdToGameObjectDictionary = new Dictionary<string, GameObject> ();
		
		petIdToGameObjectDictionary ["IMP1"] = IMP1Prefab;
		petIdToGameObjectDictionary ["IMP2"] = IMP2Prefab;
		petIdToGameObjectDictionary ["IMP3"] = IMP3Prefab;
		petIdToGameObjectDictionary ["IMP4"] = IMP4Prefab;
		petIdToGameObjectDictionary ["IMP5"] = IMP5Prefab;

		petIdToGameObjectDictionary ["AQUA1"] = AQUA1Prefab;
		petIdToGameObjectDictionary ["AQUA2"] = AQUA2Prefab;
		petIdToGameObjectDictionary ["AQUA3"] = AQUA3Prefab;
		petIdToGameObjectDictionary ["AQUA4"] = AQUA4Prefab;
		petIdToGameObjectDictionary ["AQUA5"] = AQUA5Prefab;
        
		petIdToGameObjectDictionary ["GOLEM1"] = GOLEM1Prefab;
		petIdToGameObjectDictionary ["GOLEM2"] = GOLEM2Prefab;
		petIdToGameObjectDictionary ["GOLEM3"] = GOLEM3Prefab;
		petIdToGameObjectDictionary ["GOLEM4"] = GOLEM4Prefab;
		petIdToGameObjectDictionary ["GOLEM5"] = GOLEM5Prefab;

		petIdToGameObjectDictionary ["DRAKE1"] = DRAKE1Prefab;
		petIdToGameObjectDictionary ["DRAKE2"] = DRAKE2Prefab;
		petIdToGameObjectDictionary ["DRAKE3"] = DRAKE3Prefab;
		petIdToGameObjectDictionary ["DRAKE4"] = DRAKE4Prefab;
		petIdToGameObjectDictionary ["DRAKE5"] = DRAKE5Prefab;
	}
	
	public GameObject GetSpiritObjectForId (string spiritId)
	{
//		if(GameManager.PRINT_LOGS) Debug.Log("GetSpiritObjectForId  ---   spiritId" + spiritId);
		if (spiritId == null || spiritId.Equals (""))
			return null;
		GameObject spiritObject = GameObject.Instantiate (petIdToGameObjectDictionary [spiritId]) as GameObject;
		spiritObject.SetActive (false);
		spiritObject.GetComponent<PetController> ()._petModel = new PetModel (spiritId);
		return spiritObject;
	}
}
