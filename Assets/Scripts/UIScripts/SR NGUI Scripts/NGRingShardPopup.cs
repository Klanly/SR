using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NGRingShardPopup : MonoBehaviour {
	
	public const int MAX_SHARDS = 5;
	public int currentShards = 0;	
	public UISprite _centerSprite;
	public UILabel _shardNumberLabel;
	public List<UISprite> _shards;
	
	private Action _closeCB;
	
	public void Show(int numberOfShards, Action closeCB)
	{
		_closeCB = closeCB;
//		_centerSprite.depth -= numberOfShards;

		for(int i = 0; i < numberOfShards; i++) {
			_shards[i].gameObject.SetActive(true);
		}
		currentShards = numberOfShards;
		_shardNumberLabel.text = string.Format("{0} of {1}", numberOfShards, MAX_SHARDS);
	}
	
	public void OnClick()
	{
		if(currentShards >= MAX_SHARDS)
		{
			InventorySystem.ItemRing sorcererRing = DatabankSystem.Databank.GetSorcererRingForSkullLevel(GameManager._dataBank, GameManager._gameState.skullLevel);     
			if(sorcererRing != null) 
			{
				GameManager.instance.scaleformCamera.generalSwf.LoadLoot(sorcererRing);
				Destroy(gameObject);

				return;
			}
		}

		if(_closeCB != null)
			_closeCB();
		
		Destroy(gameObject);

	}
}
