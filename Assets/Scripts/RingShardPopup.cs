using UnityEngine;
using System;
using System.Collections;

public class RingShardPopup : MonoBehaviour {

	public const int MAX_SHARDS = 5;
	
	public dfTextureSprite _centerSprite;
	public dfLabel _shardNumberLabel;
	
	public dfPanel _touchPanel;
	
	private Action _closeCB;
	
	public void Show(int numberOfShards, Action closeCB)
	{
		_closeCB = closeCB;
		_centerSprite.zindex -= numberOfShards;
		 
		_shardNumberLabel.Text = string.Format("{0} of {1}", numberOfShards, MAX_SHARDS);
	}
	
	public void OnClick(dfControl control, dfMouseEventArgs mouseEvent )
	{
		if(mouseEvent.Source == _touchPanel)
		{
			if(_closeCB != null)
				_closeCB();
			
			Destroy(gameObject);
		}
	}
}
