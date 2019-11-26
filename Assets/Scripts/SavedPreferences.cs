using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

 namespace Catformer
{
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
        private float snd;
        private float vol;

        //Audio
        public AudioSource musicAudio;
        public AudioSource soundAudio;
        [HideInInspector] public float musicVolume = 1f;
        [HideInInspector] public float soundVolume = 1f;
        public AudioClip beep;
        public AudioClip gameMusic;
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
            //vol = PlayerPrefs.GetFloat("MusicVolume", vol);
           // snd = PlayerPrefs.GetFloat("SoundVolume", snd);
            //musicVolume = vol;
           // soundVolume = snd;
            characterY = characterX[characterAnimation];
            hints = true;
            characterCat.GetComponent<Image>().sprite = characterList[currentCharacter];
            customizeCat.GetComponent<Image>().sprite = characterList[currentCharacter];
            musicAudio = GetComponents<AudioSource>()[0];
            soundAudio = GetComponents<AudioSource>()[1];
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
                ChangeBGM();
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
            //ChangeBGM();
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
        public void SoundSettingSelected()
        {
            PlayerPrefs.SetFloat("MusicVolume", vol);
            PlayerPrefs.SetFloat("SoundVolume", snd);
        }
        public void ChangedCharacter()
        {
            customizeCat.GetComponent<Image>().sprite = characterList[currentCharacter];

        }
        public void SetMusicVolume(float vol)
        {
            musicVolume = vol;
            SoundSettingSelected();
        }

        public void SetSoundVolume(float snd)
        {
            soundVolume = snd;
            SoundSettingSelected();
        }
        public void PlaySoundEffect()
        {
            soundAudio.PlayOneShot(beep, soundVolume);

        }

        public void ChangeBGM()
        {
            musicAudio.Stop();
            musicAudio.clip = gameMusic;
            musicAudio.Play();
        }

    }
}



