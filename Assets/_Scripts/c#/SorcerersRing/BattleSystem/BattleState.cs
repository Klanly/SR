using UnityEngine;
using System.Collections;

public class BattleState{
	
	
	public User _user
	{
		get;set;
	}
	
	public AIModel _enemy
	{
		get;set;
	}
	
	public BattleState()
	{
		_user=new User();
		_enemy=new AIModel();
		//_enemy.setHP(200);
	}

}
