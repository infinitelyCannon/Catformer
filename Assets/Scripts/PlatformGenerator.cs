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
    private int[] weights = { 3, 3, 3};

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
            int rand = Random.Range(1, Sum(weights) + 1);
            int index;
            for(index = 0; index < templates.Length; index++)
            {
                rand -= weights[index];
                if (rand <= 0)
                    break;
            }
            Instantiate(templates[IndexToPlatform(index)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            ShiftWeights(index);
        }
    }

    int Sum(int[] arr)
    {
        int result = 0;

        for (int i = 0; i < arr.Length; i++)
            result += arr[i];

        return result;
    }

    void ShiftWeights(int index)
    {
        switch (index)
        {
            case 1:
            case 2:
                weights[1] -= 1;
                weights[2] -= 1;
                weights[0] += 1;
                break;
            case 0:
                weights[1] += 1;
                weights[2] += 1;
                weights[0] -= 1;
                break;
            default:
                break;
        }
    }

    int IndexToPlatform(int idx)
    {
        return (idx > 0) ? 1 : 0;
    }
}
