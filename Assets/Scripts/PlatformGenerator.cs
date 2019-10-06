using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public float delay;
    public GameObject[] templates;
    public int spawnRate;
    public Transform[] spawnPoints;
    public AnimationCurve animCurve;

    public float height;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= delay)
        {
            timer -= timer;
            Instantiate(templates[Random.Range(0, templates.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
    }

    void Spawn()
    {
        
    }
}
