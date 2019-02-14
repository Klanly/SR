using UnityEngine;
using System.Collections;

public class NexGodRaysSequence : MonoBehaviour {


	public GameObject particle;
	// Use this for initialization
	void Start () {
//		GameObject obj = Instantiate(particle, Vector3.zero, Quaternion.identity);
		Transform t = particle.transform;
		t.gameObject.SetActive(true);
		t.GetComponent<ParticleSystem>().Play();
		ParticleSystem ps = null;
		foreach(Transform ti in t) {
			ps = ti.GetComponent<ParticleSystem>();
			if(ps != null) {
				ps.Play();
			}
			t.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
