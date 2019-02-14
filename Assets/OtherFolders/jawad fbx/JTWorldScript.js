#pragma strict


	var targetCamera : Camera;
	


function Update () {
	camera.transform.rotation = targetCamera.transform.rotation;
}