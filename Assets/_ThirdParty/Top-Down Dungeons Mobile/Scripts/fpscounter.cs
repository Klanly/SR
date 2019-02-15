using UnityEngine;
using System.Collections;

public class fpscounter : MonoBehaviour 
{

// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
 
	public  float updateInterval = 1.0F;
	 
	private float accum   = 0; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private float fps = 0;
	private string fpsStr = "";

	private int qualityLevel = 0;
	
	public bool isCalculateFPS = true;
		
	void Awake() {
	   Application.targetFrameRate = 60;
	   this.useGUILayout = false;
		qualityLevel = QualitySettings.GetQualityLevel();
	}
	 
	void Start()
	{
	    timeleft = updateInterval; 
	}
	 
	void Update()
	{
		if(isCalculateFPS)
		{
		    timeleft -= Time.deltaTime;
		    accum += Time.timeScale/Time.deltaTime;
		    ++frames;
		    
		    // Interval ended - update GUI text and start new interval
		    if( timeleft <= 0.0 )
		    {
		        // display two fractional digits (f2 format)
			    fps = accum/frames;
			    fpsStr = System.String.Format("{0:F2} FPS", fps);
			   
				//GameManager.instance.scaleformCamera.generalSwf.ShowText(true, fpsStr);
//				if(fps < 40.0f) {
//					QualitySettings.SetQualityLevel(qualityLevel > 0 ? --qualityLevel : 0);
//				} else if(fps > 50.0f) {
//					QualitySettings.SetQualityLevel(qualityLevel < 6 ? ++qualityLevel : 6);
//				}
//				Debug.LogError("Quality settings = "+qualityLevel);
		        timeleft = updateInterval;
		        accum = 0.0F;
		        frames = 0;

		    }
		}
	}

	public float GetFPS() {
		return fps;
	}
	
	public void OnGUI()
	{
		if(Debug.isDebugBuild)
		{
//			isCalculateFPS = true;
			
//			GUI.Label(new Rect(10, Screen.height - 40, 300, 60), "<size=30> FPS : " + fpsStr + "</size>");
		}
	}
}