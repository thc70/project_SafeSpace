﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public AudioClip audioClip;

    public string name;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 5f)]
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;

    public bool loop;



	

		
	

}

