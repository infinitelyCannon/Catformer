using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    private AudioSource musicAudio;
    private float musicVolume = 1f;
   
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        musicAudio.volume = musicVolume;
    }
    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
    }
}
