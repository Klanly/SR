using UnityEngine;
using System.Collections;

public class TouchDetector : MonoBehaviour {
	
	Camera guiCamera;
	public GameObject playerGameObject;
	public GameObject enemyGameObject;
	
	public bool getUpdates;
	
	public delegate void PositionListener();
	public PositionListener _positionListener;
	
	PlayerScreenPos _playerScreenPos;
	private enum PlayerScreenPos{kLeft, kRight}
	Vector3 playerPos3;
	Vector3 enemyPos3;
	
	public enum SwipeDirection{kLeftRight, kUp}
	public SwipeDirection _swipeDirection;
	
	public const float PLAYER_BOUND_EXTRA = 20;
	public const float ENEMY_BOUND_EXTRA = 20;
	
	private float MIN_DISTANCE = Screen.width / 10;
	
	public int degreeDifference;
	public delegate void AngleDelegate(int angle);
	
	void Start () 
	{
		playerPos3 = Vector3.zero;
		enemyPos3 = Vector3.zero;
		degreeDifference = 0;
	}
	
	public void UpdatePositions(SwipeDirection direction = SwipeDirection.kLeftRight, AngleDelegate angleDelegate = null)
	{
		guiCamera = Camera.main;
		
		_swipeDirection = direction;
		
		if(_swipeDirection == SwipeDirection.kLeftRight)
		{
			playerPos3 = guiCamera.WorldToScreenPoint(playerGameObject.transform.position);
			enemyPos3 = guiCamera.WorldToScreenPoint(enemyGameObject.transform.position);
			
			if(playerPos3.x < enemyPos3.x)
				_playerScreenPos = PlayerScreenPos.kLeft;
			else
				_playerScreenPos = PlayerScreenPos.kRight;
			
			if(angleDelegate != null)
				angleDelegate(Helpers.Compute2DAngle(enemyPos3, playerPos3));
			
			if(GameManager.PRINT_LOGS) Debug.Log("angleDelegate(degreeDifference);" + degreeDifference);
		}
		
		getUpdates = true;
	}
	
	public void StopRapidFireUpdates()
	{
		getUpdates = false;
	}

	float startX;
	float endX;
	
	float startY;
	float endY;
	
	void Update()
	{
		if(getUpdates)
		{
			if(Input.GetMouseButtonDown(0))
			{
				switch(_swipeDirection)
				{
				case SwipeDirection.kUp:
					startY = Input.mousePosition.y;
					break;
				default:
					startX = Input.mousePosition.x;
					break;
				}
				
			}
			if(Input.GetMouseButtonUp(0))
			{
				switch(_swipeDirection)
				{
				case SwipeDirection.kUp:
					endY = Input.mousePosition.y;
					
					if((endY - startY) >= MIN_DISTANCE)
					{
						if(_positionListener != null)
							_positionListener();
						startY = 0;
						endY = 0;
					}
					break;
				default:
					endX = Input.mousePosition.x;
				
					switch(_playerScreenPos)
					{
					case PlayerScreenPos.kLeft:
						if((endX - startX) >= MIN_DISTANCE)
						{
							if(_positionListener != null)
								_positionListener();
							startX = 0;
							endX = 0;
						}
						break;
					case PlayerScreenPos.kRight:
						if((startX - endX) >= MIN_DISTANCE)
						{
							if(_positionListener != null)
								_positionListener();
							startX = 0;
							endX = 0;
						}
						break;
					}
					break;
				}
				
			}
		}
	}
}
