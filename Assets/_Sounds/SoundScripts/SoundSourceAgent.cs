using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundSourceAgent : MonoBehaviour 
{
	
	
	public string soundClipId;
	private bool _isMuted = false;
	private AudioClip audioClip;
	private float _volume;
	
	public bool isSFX = true;
	
	
	// Use this for initialization
	void Start () 
	{
		
		
		SoundManager.instance.SoundMuted += isSoundMuted;
		
		if(isSFX){
			SoundManager.instance.SetSFXVolume += SoundVolume;
		}
		else{
			SoundManager.instance.SetMusicVolume += SoundVolume;
		}
		if(soundClipId != string.Empty)
		{
			
			audioClip = SoundManager.instance.GetAudioClipForId(soundClipId);
			audio.playOnAwake = false;
			audio.clip = audioClip;
		}
		else
		{
			Debug.Log("No SoundClipId assinged for: " + gameObject.name);
		}
		_isMuted = SoundManager.instance.isSoundMuted;
		if(audio)
		{
			_volume = audio.volume;
		}
		else
		{
			Debug.Log("No AudioSource Attached for " + gameObject.name);
		}
	}
	
	
	public void RegisterListener()
	{
		SoundManager.instance.SoundMuted += isSoundMuted;
	}
	public void UnRegisterListener()
	{
		SoundManager.instance.SoundMuted -= isSoundMuted;
	}
	
	public void PlaySound() 
	{
		if(!SoundManager.instance.isSoundMuted)
		{
			SoundManager.instance.PlayAudioClipForId(soundClipId);
			//				audio.volume = _volume;
			//				audio.Play();			
			//				Debug.Log("Playing sound " +audioClip.name+ " for gameobject " + gameObject.name);
		}
		else
		{
			//				audio.volume = 0.0f;
			//				audio.Play();
			//				Debug.Log("couldn't play sound " + audioClip.name +", sound is muted");
		}
	}
	
	public void StopSound()
	{
		SoundManager.instance.PauseAudioClipForId (soundClipId);
//		audio.Stop();
	}
	
	public void SoundVolume(float volume)
	{
		audio.volume = volume;
		_volume = volume;
	}
	
	public void LoopSound(bool isLooped)
	{
		if(isLooped)
		{
			audio.loop = true;
		}
		else{
			audio.loop = false;
		}
	}
	
	void isSoundMuted(bool isMuted)
	{
		_isMuted = isMuted;
		Debug.Log( gameObject.name +" Agent Muted : " + isMuted);
		
		if(_isMuted)
		{
			audio.volume = 0.0f;
		}
		else
		{
			audio.volume = _volume;
		}
		
	}
	
	public void PlaySound(string soundId)
	{
		if(soundId == soundClipId)
		{
			
			SoundManager.instance.PlayAudioClipForId(soundClipId);
			//StartCoroutine(FadeAudio(6.0f, Fade.In, 0.0f, 1.0f));
		}
		else
		{
			SoundManager.instance.PlayAudioClipForId(soundId);
			//				Resources.UnloadAsset(audioClip);
			//				//Destroy(audioClip);
			//				audioClip = null;
			//				audio.clip = null;
			//				
			//				soundClipId = soundId;
			//				audioClip = SoundManager.instance.GetAudioClipForId(soundClipId);
			//				audio.clip = audioClip;
			//				PlaySound();
		}

	}
	
	public void ChangeClip(string soundId)
	{
		if(soundClipId != null)
		{
			if(soundId == soundClipId)
			{
				Debug.Log("same Clip running");
				//PlaySound();
				//StartCoroutine(FadeAudio(6.0f, Fade.In, 0.0f, 1.0f));
			}
			else
			{
				Resources.UnloadAsset(audioClip);
				//Destroy(audioClip);
				audioClip = null;
				audio.clip = null;
				
				soundClipId = soundId;
				audioClip = SoundManager.instance.GetAudioClipForId(soundClipId);
				audio.clip = audioClip;
				//PlaySound();
			}
		}
		else
		{
			soundClipId = soundId;
			audioClip = SoundManager.instance.GetAudioClipForId(soundClipId);
			audio.clip = audioClip;
			//PlaySound();
		}
	}
	
	
	public enum Fade {In, Out};

	float fadeTime = 4.0F;
	
	public IEnumerator FadeAudio (float timer, Fade fadeType, float startVolume, float endVolume) 
	{
		if(!_isMuted){
//			Debug.Log("fading audio");
		    float start = fadeType == Fade.In? 0.0F : _volume;
		
		    float end = fadeType == Fade.In? _volume : 0.0F;
		
		    float i = 0.0F;
		
		    float step = 1.0F/timer;
		
		 
		
		    while (i <= 1.0F) 
			{
		
		        i += step * Time.deltaTime;
		
		        audio.volume = Mathf.Lerp(start, end, i);
		
		        yield return new WaitForSeconds(step * Time.deltaTime);
		
		    }
			
			if(fadeType == Fade.Out)
			{
				audio.Stop();
			}
		}
		else{
//			Debug.Log("not fading, audio is muted!");
		}

	}
}
