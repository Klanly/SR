using UnityEngine;
using System.Collections;

public class ProjectileParticleResolver : MonoBehaviour
{
	private const string TAG = "*** ProjectileParticleResolver ...";
	
	public const float PROJECTILE_MAX_HEIGHT = 3.0f;
	
	public GameObject fireParticleGameObject;
	
	public GameObject earthParticleGameObject;
	
	public GameObject waterParticleGameObject;
	
	public GameObject lightningParticleGameObject;
	
	public GameObject burstParticleGameObject;
	
	public GameObject igniteParticleGameObject;
	
	public GameObject entangleParticleGameObject;
	
	public GameObject GetParticleObjectForGesture(GestureEmitter.Gesture gesture)
	{
		return this.GetParticleObjectForGestureWithPositionAndRotation(gesture, Vector3.zero, Quaternion.identity);
	}
	
	public GameObject GetParticleObjectForGestureWithPositionAndRotation(GestureEmitter.Gesture gesture, Vector3 pos, Quaternion rot)
	{
		GameObject particleObject = null;
		
		switch(gesture)
		{
		case GestureEmitter.Gesture.kEarth:
			particleObject =  Instantiate(earthParticleGameObject, pos, rot) as GameObject;
			break;
		
		case GestureEmitter.Gesture.kFire:
			particleObject =  Instantiate(fireParticleGameObject, pos, rot) as GameObject;
			break;
		
		case GestureEmitter.Gesture.kWater:
			particleObject =  Instantiate(waterParticleGameObject, pos, rot) as GameObject;
			break;
		
		case GestureEmitter.Gesture.kLightning:
			particleObject =  Instantiate(lightningParticleGameObject, pos, rot) as GameObject;
			break;
		case GestureEmitter.Gesture.Ignite:
			particleObject =  Instantiate(igniteParticleGameObject, pos, rot) as GameObject;
			break;
		case GestureEmitter.Gesture.Daze:
			particleObject =  Instantiate(igniteParticleGameObject, pos, rot) as GameObject;
			break;
		case GestureEmitter.Gesture.Drain:
			particleObject =  Instantiate(igniteParticleGameObject, pos, rot) as GameObject;
			break;
		case GestureEmitter.Gesture.LeechSeed:
			particleObject =  Instantiate(igniteParticleGameObject, pos, rot) as GameObject;
			break;
		case GestureEmitter.Gesture.Burst:
			particleObject =  Instantiate(burstParticleGameObject, pos, rot) as GameObject;
			break;
		}
		
		particleObject.AddComponent<ProjectileStatsComparator>();
			
		particleObject.GetComponent<ProjectileStatsComparator>()._projectileType = gesture;
		
		return particleObject;
	}

}
