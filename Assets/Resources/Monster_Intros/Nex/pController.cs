using UnityEngine;
using System.Collections;

public class pController : MonoBehaviour {
	
	public bool start = false;
	
	public GameObject verticalBeam;
	private NcCurveAnimation curAnim;
	
	public GameObject aura1;
	public GameObject aura2;
	public GameObject energySource;
	
	public GameObject explosion;
	

	
	
	// Use this for initialization
	void Start () {
		curAnim = verticalBeam.GetComponent<NcCurveAnimation>();
		//int cc = curAnim.GetCurveInfoCount
		
		
	}
	void Finish() {
		
		explosion.SetActive(true);
		explosion.particleEmitter.emit = true;
		//energySource.SetActive(false);
		
	}
	void removeAura() {
		aura1.SetActive(false);
		aura2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (start) {
			for (int i = 0; i<curAnim.GetCurveInfoCount();i++) {
				curAnim.GetCurveInfo(i).m_bEnabled=true;
			}
		}

	}

}
