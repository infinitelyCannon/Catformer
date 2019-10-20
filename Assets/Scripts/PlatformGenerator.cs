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

    private const float PLATFORM_WIDTH = 17.17f * 0.2720631f;

    // Start is called before the first frame update
    void Start()
    {
        Camera cameraRef = Camera.main;
        Vector3 rightEdge = cameraRef.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
        Vector3 leftEdge = cameraRef.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
        Vector3 leftSpot = leftEdge + new Vector3(PLATFORM_WIDTH / 2f, 0f, 0f);
        Vector3 rightSpot = rightEdge - new Vector3(PLATFORM_WIDTH / 2f, 0f, 0f);

        foreach(Transform spot in spawnPoints)
        {
            if (spot.position.x < cameraRef.transform.position.x && spot.position.x < leftSpot.x)
                spot.position = new Vector3(leftSpot.x, spot.position.y, 0f);
            else if (spot.position.x > cameraRef.transform.position.x && spot.position.x > rightSpot.x)
                spot.position = new Vector3(rightSpot.x, spot.position.y, 0f);
        }
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
