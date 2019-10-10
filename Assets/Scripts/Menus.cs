using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hints;

    public GameObject[] hintImages;
    public GameObject startMenu;
    private void Start()
    {
        hints = true;
    }
    public void StartGame()
    {
        if (hints == false)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    
}

