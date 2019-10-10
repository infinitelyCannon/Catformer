using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    private AudioSource musicAudio;
    private float musicVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        musicAudio.volume = musicVolume;
    }
    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
    }
}
