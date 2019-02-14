using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectController : MonoBehaviour
{
	
    public ModelType currentModel = ModelType.INVALID;
    private static Dictionary<ModelType, string> modelToPrefabDictionary;
    static CharacterSelectController()
    {
        modelToPrefabDictionary = new Dictionary<ModelType, string>();
        modelToPrefabDictionary [ModelType.MULTIPLAYER_MALE] = "PREFAB_MULTI_MALE";
        modelToPrefabDictionary [ModelType.MULTIPLAYER_FEMALE] = "PREFAB_MULTI_FEMALE";
        modelToPrefabDictionary [ModelType.SINGLEPLAYER_MALE] = "PREFAB_HERO";
    }
	
    AssetBundleLoader bundleLoader;
    SRCharacterController characterController;
    public enum ModelType
    {
        INVALID,
        MULTIPLAYER_MALE,
        MULTIPLAYER_FEMALE,
        SINGLEPLAYER_MALE
    }
	
    private bool isRequestProcessed = true;
	
    void Start()
    {
        characterController = gameObject.GetComponent<SRCharacterController>();
    }
	
    OnCharacterModelReceived characterModelLoadCB;
    public delegate void OnCharacterModelReceived(GameObject characterModelObject);
	
    public void SetModel(string modelName, OnCharacterModelReceived characterModelLoadCB = null)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("modelName " + modelName);
        if (modelName.Equals(ModelType.MULTIPLAYER_MALE.ToString()))
            SetModel(ModelType.MULTIPLAYER_MALE, characterModelLoadCB);
        else if (modelName.Equals(ModelType.SINGLEPLAYER_MALE.ToString()))
            SetModel(ModelType.SINGLEPLAYER_MALE, characterModelLoadCB);
        else if (modelName.Equals(ModelType.MULTIPLAYER_FEMALE.ToString()))
            SetModel(ModelType.MULTIPLAYER_FEMALE, characterModelLoadCB);
    }
	
    public void SetModel(ModelType modelType, OnCharacterModelReceived characterModelLoadCB = null)
    {
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetModel ---- 1 ::::::: modelType = " + modelType.ToString());
        if (modelType == ModelType.INVALID)
            return;
		
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetModel ---- 2");
		
        if (!isRequestProcessed)
            return;
		
        isRequestProcessed = false;
		
        this.characterModelLoadCB = characterModelLoadCB;
		
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetModel ---- 3");
		
        AssetBundleLoader heroAssetBundleLoader = this.gameObject.AddComponent<AssetBundleLoader>();
		
        //load asset bundle
        if (playerModelObject != null)
        {
            Destroy(playerModelObject);
            heroAssetBundleLoader.UnloadAssetBundle(modelToPrefabDictionary [modelType]);
            playerModelObject = null;
        }
		
        currentModel = modelType;
        if (GameManager.PRINT_LOGS)
            Debug.Log("SetModel ---- 4");
        heroAssetBundleLoader.SetDelegate(this.OnAssetBundleReceived);
        heroAssetBundleLoader.InvokeLoadPrefabAssetBundle(modelToPrefabDictionary [modelType]);
    }
	
    private GameObject playerModelObject;
    private void OnAssetBundleReceived(AssetBundle bundle)
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("OnAssetBundleReceived ---- 1");
		
        playerModelObject = Instantiate(bundle.mainAsset as GameObject) as GameObject;

        SetReferences();
		
        //GameManager.instance._levelManager.LevelsAssetBundles.UnloadAssetBundleWithoutObject(bundle.mainAsset.name);	
		
        if (characterModelLoadCB != null)
            characterModelLoadCB(playerModelObject);
		
        isRequestProcessed = true;
    }
	
    private void SetReferences()
    {
//		if(GameManager.PRINT_LOGS) Debug.Log("SetReferences ---- 1");
        characterController.projectileSpawnPoint = playerModelObject.transform.Find("Sorcerer/SpellPoint");
        characterController.rapidFireSpawnPoint = playerModelObject.transform.Find("Sorcerer/Sorcerer Prop1/SpellPoint_Staff");
        characterController.characterHitTransform = playerModelObject.transform.Find("Sorcerer/Sorcerer Pelvis");
        characterController.charStateController = playerModelObject.GetComponent<SRCharacterStateControllerYetAgain>();
        characterController.SorcererSoundController = playerModelObject.GetComponentInChildren<CharacterSoundController>();
//		if(GameManager.PRINT_LOGS) Debug.Log("SetReferences ---- 2");
    }
	

}
