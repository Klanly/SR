using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AnimateTransparentShader : MonoBehaviour {
	
	void Start()
	{
		//AnimateShader();
		//disAppearEnemyParticles=Resources.Load(disAppearParticlePath) as GameObject;
	}
	public void AnimateShader()
	{
//		if(this.gameObject.transform.parent.FindChild("DisappearEnemyParticles(Clone)")==null)
//		{
//			GameObject disAppearParticlesGameObject=Instantiate(GameManager.instance.LoadPrefab("disappearEnemyParticles"),Vector3.zero,Quaternion.identity) as GameObject;
			this.renderer.material.shader=Shader.Find("Transparent/Cutout/Diffuse");
			//disAppearParticlesGameObject.transform.parent=this.gameObject.transform.parent;
			//this.gameObject.transform.parent.FindChild("DisappearEnemyParticles(Clone)").transform.localPosition=new Vector3(0.0f,4.0f,-2.0f);
			//disAppearParticlesGameObject.particleEmitter.emit=true;
			
			StartCoroutine(disAppearEnemy());
		//}
	}
	
	IEnumerator disAppearEnemy()
	{
		float temp=0.0f;
		while(temp<=1.0f)
		{
			this.renderer.material.SetFloat("_Cutoff",temp);
			temp+=0.005f;
			yield return new WaitForSeconds(0.015f);
		}
		Debug.Log(temp+"<<<Animatingggg");
		//Destroy(poi[POINumber].relatedObject);
		yield return null;
	}
	
}