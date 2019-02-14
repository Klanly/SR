using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour
{
	public UITexture _mask;
	public Camera guiCamera;
	public void SetMask()
	{
//		_mask.GetComponent<dfFollowObject>().attach = GameObject.FindGameObjectWithTag("EnemyProjectile");
//		_mask.GetComponent<dfFollowObject>().enabled = true;
//		_mask.Show();
		Vector2 screenPos = Camera.main.WorldToViewportPoint(GameObject.FindGameObjectWithTag("EnemyProjectile").transform.position) ;
		
		// Move to node
		_mask.transform.position = guiCamera.ViewportToWorldPoint(screenPos);
		_mask.transform.position = new Vector3(_mask.transform.position.x, _mask.transform.position.y, 0);

		_mask.gameObject.SetActive(true);
	}

	public void RemoveMask()
	{
//		_mask.GetComponent<dfFollowObject>().enabled = false;
//		_mask.GetComponent<dfFollowObject>().attach = null;
//		_mask.Hide();
		_mask.gameObject.SetActive(false);
	}
}
