using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioController : MonoBehaviour
{
	private IPlayAudio _audioRef;

	private AudioSource _audioSource;

	// Start is called before the first frame update
	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioRef = GetComponent<IPlayAudio>();
	}

	private void OnEnable()
	{
		_audioRef.PlayAudioEffect += PlayAudioEffect;
		_audioRef.PlayLoopedAudio += PlayLoopedAudio;
		_audioRef.StopLoopedAudio += StopLoopedAudio;
	}

	private void OnDisable()
	{
		_audioRef.PlayAudioEffect -= PlayAudioEffect;
		_audioRef.PlayLoopedAudio -= PlayLoopedAudio;
		_audioRef.StopLoopedAudio -= StopLoopedAudio;
	}

	void PlayLoopedAudio(object sender, AudioClip audioClip)
	{
		_audioSource.clip = audioClip;
		_audioSource.loop = true;
		_audioSource.playOnAwake = false;
		_audioSource.Play();
	}

	void StopLoopedAudio(object sender, EventArgs eventArgs)
	{
		_audioSource.Stop(); 
	}

	void PlayAudioEffect(object sender, AudioClip audioClip)
	{
		AudioSource.PlayClipAtPoint(audioClip, _audioSource.transform.position); 
	}
	
}
