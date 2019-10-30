using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public enum SpawnUnits
    {
        World,
        Camera
    }

    public GameObject[] templates;
    public int spawnRate;
    public Transform[] spawnPoints;

    public float height;

    [Header("Debug Variables")]
    public Color SafetyZoneColor = Color.cyan;

    private float timer = 0f;
    private int[] weights = { 3, 3, 3};

    private float SpawnHeight;
    private PlayerScript playerRef;
    private Camera mCamera;

    private const float PLATFORM_WIDTH = 17.17f * 0.2720631f;
    private const float PLATFORM_HEIGHT = 8.29f * 0.2720631f;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main;


        /*Vector3 rightEdge = cameraRef.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
        Vector3 leftEdge = cameraRef.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
        Vector3 leftSpot = leftEdge + new Vector3(PLATFORM_WIDTH / 2f, 0f, 0f);
        Vector3 rightSpot = rightEdge - new Vector3(PLATFORM_WIDTH / 2f, 0f, 0f);

        foreach(Transform spot in spawnPoints)
        {
            if (spot.position.x < cameraRef.transform.position.x && spot.position.x < leftSpot.x)
                spot.position = new Vector3(leftSpot.x, spot.position.y, 0f);
            else if (spot.position.x > cameraRef.transform.position.x && spot.position.x > rightSpot.x)
                spot.position = new Vector3(rightSpot.x, spot.position.y, 0f);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // The target the player will move to.
        GameObject startPoint;

        // The position of the green line the camera uses to follow the player
        Vector3 followLine = mCamera.ViewportToWorldPoint(new Vector3(0.5f, mCamera.gameObject.GetComponent<CameraController>().followLine, 10f));

        // The top, right, and left edges of the camera.
        Vector3 topEdge = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10f));
        Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
        Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));

        // On click . . .
        if (Time.timeScale > 0 && Input.GetMouseButtonDown(0))
        {
            // Grab the player's target platform.
            startPoint = playerRef.GetTarget();

            // If that platform exists, it's not the player, and it's higher than the camera's follow line,
            if(startPoint != null 
                && startPoint != playerRef.gameObject 
                && startPoint.transform.position.y > followLine.y)
            {
                /*
                    Get the vertical distance between the platform and the follow line,
                    and spawn a new platform somewhere inside that distance above the top of the camera.
                    (Also picks a random X coordinate).
                 */
                SpawnHeight = Mathf.Abs(startPoint.transform.position.y - followLine.y);
                Instantiate(templates[0], 
                    new Vector3(
                        Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f)),
                        Random.Range(topEdge.y + (PLATFORM_HEIGHT / 2f), (topEdge.y + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f
                    ), 
                    Quaternion.identity);
            }
        }
        // Or on touch . . .
        else if(Time.timeScale > 0 && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            // Grab the player's target platform.
            startPoint = playerRef.GetTarget();

            // If that platform exists, it's not the player, and it's higher than the camera's follow line,
            if (startPoint != null
                && startPoint != playerRef.gameObject
                && startPoint.transform.position.y > followLine.y)
            {
                /*
                    Get the vertical distance between the platform and the follow line,
                    and spawn a new platform somewhere inside that distance above the top of the camera.
                    (Also picks a random X coordinate).
                 */
                SpawnHeight = Mathf.Abs(startPoint.transform.position.y - followLine.y);
                Instantiate(templates[0],
                    new Vector3(
                        Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f)),
                        Random.Range(topEdge.y + (PLATFORM_HEIGHT / 2f), (topEdge.y + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f
                    ),
                    Quaternion.identity);
            }
        }
        /*
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
            Instantiate(templates[IndexToPlatform(0)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            ShiftWeights(index);

        }
        */
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
