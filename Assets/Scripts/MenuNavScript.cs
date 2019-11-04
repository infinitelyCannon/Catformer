using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavScript : MonoBehaviour
{
    public AudioSource musicAudio;
     public AudioClip gameMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        Destroy(Catformer.SavedPreferences.instance.gameObject);
        SceneManager.LoadScene(0);
        /*
        musicAudio.Stop();
        musicAudio.clip = gameMusic;
        musicAudio.Play();
        */
    }
}
