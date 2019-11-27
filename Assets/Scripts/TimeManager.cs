using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    private Image background;
    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
        if (Time.timeScale < 1f)
        {
            background.color = Color.clear;
            for(int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (Time.timeScale > 0)
        {
            background.color = new Color(1, 1, 1, 0.8f);
            for (int j = 0; j < transform.childCount; j++)
                transform.GetChild(j).gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
