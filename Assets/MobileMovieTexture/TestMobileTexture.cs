using UnityEngine;
using System.Collections;


public class TestMobileTexture : Singleton<TestMobileTexture>
{
	//    private MobileMovieTexture m_movieTexture;
	public MediaPlayerCtrl scrMedia;
    
	public bool skipMovieButton = false;
    
	public Camera movieCamera;
	public InitGameVersions _initializer;
	void Awake ()
	{
		Debug.Log ("Before SkipMovie");

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Debug.Log ("After sleepTimeout set");
		//        m_movieTexture = GetComponent<MobileMovieTexture>();
		//
		//      Invoke("OnFinished",25f);

#if UNITY_ANDROID
//       scrMedia.Load("Cinematics_vo_9.mp4");
		OnFinished();
		return;
#endif

		scrMedia.OnReady = PlayCinematic;
		scrMedia.OnEnd = OnFinished;

		//m_movieTexture.renderer.material.SetColor("_TintColor",new Color(1,1,1,0.0f));
		//CheckDeviceRatio();
		//Debug.Log("After Checking Device Ratio.");
		//skipMovie = true;

        
		//      Invoke("StartMovieWithDelay",9.0f);

		//        Invoke("FadeInAgain", 5.0f);
		//        Invoke("OnFinished", 6f);
	}
    
	float widthDenom = 14f;
	float heightDenom = 10f;

	void PlayCinematic ()
	{
		Debug.LogError ("OnReady: PlayCinematic");
		if (PlayerPrefs.HasKey ("CinematicPlayed") || skipMovieButton) {
			OnFinished ();
		} else {
			Debug.Log ("Playing Cinematic");
			PlayerPrefs.SetInt ("CinematicPlayed", 1);
			BundleDownloadManager.instance.isVideoPlaying = true;


			MonoExtensions.PerformActionWithDelay (this, 2.0f, () => {
				scrMedia.Play ();
				skipMovieButton = true;
			});

		}
	}

	void Start ()
	{
//		PlayMakerFSM.BroadcastEvent ("FadeOutEvent");
//		Debug.LogWarning("FadeOutEvent - TestMobile Start");
		#if UNITY_EDITOR
		OnFinished ();
		#endif   
        
	}
    
	private void FadeInAgain ()
	{
		PlayMakerFSM.BroadcastEvent ("FadeInEvent");
		Debug.LogWarning("FadeInAgain - TestMobile Start");
	}
    
	private void FadeOutAgain ()
	{
		PlayMakerFSM.BroadcastEvent ("FadeOutEvent");
		Debug.LogWarning("FadeOutEvent - TestMobile Start");
	}
    
	void StartMovieWithDelay ()
	{
		if (PlayerPrefs.HasKey ("SRingMovieSkipAllowed")) {
			skipMovieButton = true;
			//Application.LoadLevel("MainMenuScene");
			BundleDownloadManager.instance.isVideoPlaying = false;
			//            m_movieTexture.renderer.material.SetColor("_TintColor",new Color(0,0,0,0.0f));
			BundleDownloadManager.instance.InvokeInit (0);
			BundleDownloadManager.instance.ContinueLoadingGame ();
		} else {
			PlayerPrefs.SetInt ("SRingMovieSkipAllowed", 1);
			skipMovieButton = false;
			//m_movieTexture.renderer.material.SetColor("_TintColor",new Color(0,0,0,0.0f));
			BundleDownloadManager.instance.isVideoPlaying = true;
			//            m_movieTexture.Play();
			//audio.Play();

		}
	}
    
	void OnFinished (/*MobileMovieTexture sender*/)
	{
		scrMedia.UnLoad ();
//		Debug.Log ("OnFinished");
		//Debug.Log(sender.Path + " has finished ");
		//      m_movieTexture.renderer.material.SetColor("_TintColor",new Color(1,1,1,0.0f));
		//Application.LoadLevel("MainMenuScene");

		skipMovieButton = false;
		/////////////////////////// FT //////////////////////////
		/// 
		/// 
		this.PerformActionWithDelay(1.0f,()=>{
			_initializer.gameObject.SetActive (true);
			gameObject.SetActive(false);
//			Debug.LogError("UImanager is calling hide");
			if(UIManager.instance != null && UIManager.instance.generalSwf != null)
				UIManager.instance.generalSwf.HideCutsceneDialog();
			Destroy(gameObject);
		});

		return;
		///////////////////////////////////////////////////////// 
//		BundleDownloadManager.instance.isVideoPlaying = false;
//		BundleDownloadManager.instance.InvokeInit (0);
//		BundleDownloadManager.instance.ContinueLoadingGame ();


//        BundleDownloadManager.instance.DelayedMovieStart();
//        BundleDownloadManager.instance.LoadMainMenu();
        
//        BundleDownloadManager.instance.InvokeInit(0.5f);
	}
	/*
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 50, 50), m_movieTexture.isPlaying ? "Skip" : "Play" ))
        {
            if (m_movieTexture.isPlaying)
            {
                m_movieTexture.pause = true;
                Application.LoadLevel("MainMenuScene");
            }
            else
            {
                if (m_movieTexture.pause)
                {
                    m_movieTexture.pause = false;
                }
                else
                {
                    m_movieTexture.Play();
                }
            }
        }

    }
    */
    
	public void OnClick ()
	{
        
		print ("button clicked");
        
		if (skipMovieButton) {
			print ("button clicked");
			//m_movieTexture.
			//m_movieTexture.renderer.material.SetColor("_TintColor",new Color(1,1,1,0.0f));
			//m_movieTexture.renderer.material.color = new Color(255,255,255,.1f);
			//Application.LoadLevel("MainMenuScene");
		}
	}
    
	private void SkipMovieNContinue ()
	{
		//      if(m_movieTexture.isPlaying)
		//      {
		//          stopMovie();
		//          OnFinished(null);
		//      }
		scrMedia.Stop ();
		skipMovieButton = false;
		OnFinished ();
	}
    
	public Texture buttonImage;
	public void OnGUI ()
	{
		float skipButtonWidth = Screen.width / widthDenom;
		float skipButtonHeight = Screen.height / heightDenom;
        
		GUIStyle labelStyle = GUI.skin.label;
        
		if (skipMovieButton) {
			if (GUI.Button (new Rect (Screen.width - skipButtonWidth - 1, Screen.height - skipButtonHeight - 1, skipButtonWidth, skipButtonHeight), buttonImage, labelStyle))
				SkipMovieNContinue ();
		}
	}
    
//    private void stopMovie()
//    {
//        //      Debug.Log("stopMovie");
//        //      m_movieTexture.renderer.material.SetColor("_TintColor",new Color(1,1,1,0.0f));
//        //      movieCamera.depth = 1;
//        //      audio.Stop();
//        //      m_movieTexture.Stop();
//    }
    
	private void CheckDeviceRatio ()
	{
        
		float resRatio = (float)Screen.width / (float)Screen.height;
		if (resRatio < 1.4) {
			Camera.main.orthographicSize = 240;
		} else if (resRatio > 1.4 && resRatio < 1.55) {
			Camera.main.orthographicSize = 210;
		} else if (resRatio > 1.55 && resRatio < 1.8) {
			Camera.main.orthographicSize = 180;
		}
	}
    

	void OnDestroy() {
		Debug.LogError("OnDestroy called - TestMobileTexture "+StackTraceUtility.ExtractStackTrace());
	}
}
