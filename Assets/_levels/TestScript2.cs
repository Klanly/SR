using UnityEngine;
using System.Collections;

public class TestScript2 : MonoBehaviour {

	public Transform[] points;
	
	LineRenderer lineRenderer;
	
	private float minX;
	private float minY;
	
	private float maxX;
	private float maxY;
	
	void Start()
	{
		PopulateBounds();
		
		lineRenderer = GetComponent<LineRenderer>();
		
		SetRendererLines();
	}
	
	public void PopulateBounds()
	{
		for(int i = 0;i<points.Length;i++)
		{
			maxX = (points[i].position.x > maxX) ? points[i].position.x : maxX;
			minX = (points[i].position.x < minX) ? points[i].position.x : minX;
			
			maxY = (points[i].position.y > maxY) ? points[i].position.y : maxY;
			minY = (points[i].position.y < minY) ? points[i].position.y : minY;
		}
	}
	
	private void SetRendererLines()
	{
		for(int i = 0;i<points.Length;i++)
			lineRenderer.SetPosition(i, points[i].position);
		
		lineRenderer.enabled = true;
		
		Camera.main.fieldOfView = (maxY - minY) * 20;
	}
	
	void Update () {
		for(int i = 0;i<points.Length -1;i++)
			Debug.DrawLine(points[i].position, points[i+1].position, Color.green);

	}
}
