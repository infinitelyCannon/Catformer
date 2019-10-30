using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SavedPreferences : MonoBehaviour
{
    
    private bool hints;

    public GameObject[] hintImages;
    public string[] characterX;
    public string characterY;
    public int characterAnimation;
    public Sprite[] characterList;
    public GameObject startMenu;
    public GameObject characterCat;
    public GameObject customizeCat;
    public int currentCharacter;
    public GameObject savedPreferences;

    //Audio
    private AudioSource musicAudio;
    private float musicVolume = 1f;
    private float soundVolume = 1f;
    public AudioClip beep;

    public static SavedPreferences instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        currentCharacter = PlayerPrefs.GetInt("CharacterSelected");
        characterAnimation = PlayerPrefs.GetInt("AnimationInt");
        characterY = characterX[characterAnimation];
        hints = true;
        characterCat.GetComponent<Image>().sprite = characterList[currentCharacter];
        customizeCat.GetComponent<Image>().sprite = characterList[currentCharacter];
        musicAudio = GetComponent<AudioSource>();
    }
    void Update()
    {
        musicAudio.volume = musicVolume;
    }
    public void StartGame()
    {
        if (hints == false)
        {
            //stop overlapping music playing from start menu to game scene
            musicAudio.Stop();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
        else if (hints == true)
        {
            startMenu.SetActive(false);
            for (int i = 0; i < hintImages.Length; i++)
            {
                hintImages[i].SetActive(true);

            }

        }

    }
    public void StarGameAfterHints()
    {
        //stop overlapping music playing from start menu to game scene
        musicAudio.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void NoHints()
    {
        for (int i = 0; i < hintImages.Length; i++)
        {
            hintImages[i].SetActive(false);
        }
        hints = false;
        
    }
    public void YesHints()
    {
        hints = true;
    }
    public void NextCharacter()
    {
        if (++currentCharacter == characterList.Length)
        {
            currentCharacter = 0;
        }
        if (++characterAnimation == characterX.Length)
        {
            characterAnimation = 0;
        }

        Sprite current = characterList[currentCharacter];
        string currentstring = characterX[characterAnimation];
        characterCat.GetComponent<Image>().sprite = current;
        characterY = currentstring;
        CharacterSelected();
    }
    public void PreviousCharacter()
    {
        if (--currentCharacter < 0)
        {
            currentCharacter = characterList.Length - 1;
        }
        if (--characterAnimation < 0)
        {
            characterAnimation = characterX.Length - 1;
        }
        Sprite current = characterList[currentCharacter];
        string currentstring = characterX[characterAnimation];
        characterCat.GetComponent<Image>().sprite = current;
        characterY = currentstring;
        CharacterSelected();
    }
    public void CharacterSelected()
    {
        PlayerPrefs.SetInt("CharacterSelected", currentCharacter);
        PlayerPrefs.SetInt("AnimationInt", characterAnimation);
        PlayerPrefs.SetString("AnimationSelected", characterY);
    }
    public void ChangedCharacter()
    {
        customizeCat.GetComponent<Image>().sprite = characterList[currentCharacter];
        
    }
    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
    }
    
    public void SetSoundVolume(float snd)
    {
        soundVolume = snd;
    }
    public void PlaySoundEffect()
    {
        musicAudio.PlayOneShot(beep, soundVolume);
        
    }

}


