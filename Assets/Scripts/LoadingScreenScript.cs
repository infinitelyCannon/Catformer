using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
    public float speed;
    public float height;
    public GameObject image;
    public GameObject readyButton;

    private float tempVolume;
    private GameObject eventSystem;
    private int thisScene;

    // Start is called before the first frame update
    void Start()
    {
        //readyButton.SetActive(false);
        //eventSystem = GameObject.Find("EventSystem");
        //tempVolume = Catformer.SavedPreferences.instance.musicVolume;
        //Catformer.SavedPreferences.instance.musicVolume = 0f;
        //thisScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadNextScene());
    }

    // Update is called once per frame
    void Update()
    {
        image.transform.position += new Vector3(0f, Mathf.Sin(Time.time * speed) * height, 0f);
    }

    public void NextScene()
    {
        Catformer.SavedPreferences.instance.musicVolume = tempVolume;
        Catformer.SavedPreferences.instance.ChangeBGM();
        SceneManager.UnloadSceneAsync(thisScene);
    }

    private void OnDestroy()
    {
        Catformer.SavedPreferences.instance.ChangeBGM();
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            yield return null;
        }

        readyButton.SetActive(true);
    }
}
