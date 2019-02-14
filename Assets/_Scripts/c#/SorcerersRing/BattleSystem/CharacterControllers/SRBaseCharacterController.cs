using UnityEngine;
using System.Collections;

public abstract class SRBaseCharacterController : MonoBehaviour{
	
	#region Emitters and Aura setting Variables n Methods
	public ParticleEmitter leftHandEmitter;
	
	public ParticleEmitter rightHandEmitter;
	
	public Material generalAura;
	
	public Material fireAura;
	
	public Material lightningAura;
	
	public Material waterAura;
	
	public Material earthAura;
	
	protected void SetEmitterMaterial(GestureEmitter.Gesture gesture)
	{
		if(gesture==GestureEmitter.Gesture.kFire)
		{
//			Debug.Log("casting/kFire");
		
				leftHandEmitter.renderer.material=fireAura;
		
				rightHandEmitter.renderer.material=fireAura;
		}
		else if(gesture==GestureEmitter.Gesture.kLightning)
		{
//			Debug.Log("casting/kLightning");
		
				leftHandEmitter.renderer.material=lightningAura;
		
				rightHandEmitter.renderer.material=lightningAura;
		}
		else if(gesture==GestureEmitter.Gesture.kEarth)
		{
//			Debug.Log("casting/kEarth");
		
				leftHandEmitter.renderer.material=earthAura;
		
				rightHandEmitter.renderer.material=earthAura;
		}
		else if(gesture==GestureEmitter.Gesture.kWater)
		{
//			Debug.Log("casting/kWater");
		
				leftHandEmitter.renderer.material=waterAura;
		
				rightHandEmitter.renderer.material=waterAura;
		}
	}
	#endregion

	
	
	
	
	
}
