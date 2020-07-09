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

        public Text catText;
        public GameObject[] hintImages;
        public string[] characterX;
        public string characterY;
        public int characterAnimation;
        public Sprite[] characterList;
        public GameObject startMenu;
        public GameObject characterCat;
        public GameObject customizeCat;
        public GameObject volumeSlider;
        public GameObject soundSlider;
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
            //CharacterSelect
            currentCharacter = PlayerPrefs.GetInt("CharacterSelected");
            characterAnimation = PlayerPrefs.GetInt("AnimationInt");
            characterY = characterX[characterAnimation];
            characterCat.GetComponent<Image>().sprite = characterList[currentCharacter];
            customizeCat.GetComponent<Image>().sprite = characterList[currentCharacter];
            //VolumeControls
            vol = PlayerPrefs.GetFloat("MusicVolume", vol);
            snd = PlayerPrefs.GetFloat("SoundVolume", snd);
            musicVolume = vol;
            soundVolume = snd;
            musicAudio = GetComponents<AudioSource>()[0];
            soundAudio = GetComponents<AudioSource>()[1];
            volumeSlider.GetComponent<Slider>().value = vol;
            soundSlider.GetComponent<Slider>().value = snd;
            if (PlayerPrefs.GetFloat("Hint") == 0)
            {
                hints = false;
            }
            else
            {
                hints = true;
            }
            
            
            Debug.Log(vol);
        }
        void Update()
        {
            musicAudio.volume = musicVolume;
            switch (currentCharacter)
            {
                case 0:
                    catText.text = "Cat Dude";
                    break;
                case 1:
                    catText.text = "Tiger";
                    break;
                case 2:
                    catText.text = "Bluey";
                    break;
                case 3:
                    catText.text = "Cocoa";
                    break;
                case 4:
                    catText.text = "DakaraiCat";
                    break;
                case 5:
                    catText.text = "Simon";
                    break;
                case 6:
                    catText.text = "KyleCat";
                    break;
                case 7:
                    catText.text = "MateoCat";
                    break;
                case 8:
                    catText.text = "CottonCatty";
                    break;
                case 9:
                    catText.text = "VolcaniCat";
                    break;
                case 10:
                    catText.text = "AstroCat";
                    break;
                case 11:
                    catText.text = "WatersCat";
                    break;
                case 12:
                    catText.text = "Frenchy";
                    break;
                case 13:
                    catText.text = "NoraCat";
                    break;
                case 14:
                    catText.text = "TacoCat";
                    break;
                case 15:
                    catText.text = "Trippy";
                    break;
                case 16:
                    catText.text = "Catsper";
                    break;
            }
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
            PlayerPrefs.SetFloat("Hints", 0);
        }
        public void YesHints()
        {
            hints = true;
            PlayerPrefs.SetFloat("Hints", 1);
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
            PlayerPrefs.SetFloat("MusicVolume", vol);
        }

        public void SetSoundVolume(float snd)
        {
            soundVolume = snd;
            PlayerPrefs.SetFloat("SoundVolume", snd);
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



