using UnityEngine;

using System.Collections;

 

public class PanCamera : MonoBehaviour 

{

   // public Camera mainCamera;

    public float speed = 0.1F;

    // Use this for initialization

    void Start () 

    {

    

    }

    

    // Update is called once per frame

    void Update () 

    {
		if (Input.GetMouseButton(0)) { 
		//transform.rotation = this.transform.rotation; 
		this.transform.Translate(-Input.GetAxis("Mouse X")*Time.deltaTime,-Input.GetAxis("Mouse Y")*Time.deltaTime,0.0f,Space.World); 
		//transform.Translate(transform.up * -Input.GetAxis("Mouse Y") * Time.deltaTime, Space.World); 
		}
    

        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) 

        {

        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

        this.transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, transform.position.z);

        }

        

    }

}