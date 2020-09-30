using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventArgs : EventArgs
{
	public AudioClip _audioClip;

	AudioEventArgs()
	{

	}
}


public interface IPlayAudio
{
	event EventHandler<AudioClip> PlayLoopedAudio;

	event EventHandler StopLoopedAudio;

	event EventHandler<AudioClip> PlayAudioEffect;
}
