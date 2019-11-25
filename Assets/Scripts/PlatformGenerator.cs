using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catformer;

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
    public GameObject[] templatesCloud;

    public float cloudHeight;
    public float spaceHeight;

    [Header("Debug Variables")]
    public Color SafetyZoneColor = Color.cyan;

    private float timer = 0f;
    private int[] weights = { 3, 3, 3};

    private float SpawnHeight;
    private PlayerScript playerRef;
    private Camera mCamera;

    private const float PLATFORM_WIDTH = 17.17f * 0.2720631f;
    private const float PLATFORM_HEIGHT = 8.29f * 0.2720631f;

    private GameObject platformInstance; //Matt: To be called in the platform script
    private GameObject[] platformInstances;
    private Vector3 spawnVector;
    public float spawnDist = 0f;

    private Vector3 spawnVectorHazard;
    public GameObject[] hazardPrefabs;
    private GameObject hazardInstance;

    private float elevation = 0f;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main;
        platformInstances = new GameObject[spawnPoints.Length];
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
                //Holds random spawn vector
                elevation = playerRef.gameObject.GetComponent<Catformer.PlayerScript>().GetScore();
                if (elevation >= cloudHeight)
                    SpawnClouds(topEdge.y);
                else
                    SpawnPlatforms(topEdge.y);
                //SpawnHazard(topEdge.y);
                /*spawnVector = new Vector3(
                        spawnPoints[Random.Range(0, spawnPoints.Length)].position.x Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f)),
                        Random.Range(topEdge.y + (PLATFORM_HEIGHT / 2f), (topEdge.y + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);

                if (platformInstance == null || spawnVector.y - platformInstance.transform.position.y> spawnDist) //Matt
                {
                    platformInstance = Instantiate(templates[0], spawnVector, Quaternion.identity) as GameObject;
                }*/
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
                //Finds elevation to reference transitions
                elevation = playerRef.gameObject.GetComponent<Catformer.PlayerScript>().GetScore();
                if (elevation > cloudHeight)
                    SpawnClouds(topEdge.y);
                else
                    SpawnPlatforms(topEdge.y);
                //SpawnHazard(topEdge.y);
            }
        }
    }

    void SpawnPlatforms(float topEdgeY)
    {
        int spot = Random.Range(1, 4);

        if(spot == 1)
        {
            spawnVector = new Vector3(
                        spawnPoints[0].position.x /*Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f))*/,
                        Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);
            Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity);
        }
        else if (spot == 2)
        {
            spawnVector = new Vector3(
                        spawnPoints[1].position.x /*Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f))*/,
                        Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);
            Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity);
        }
        else // spot == 3
        {
            spawnVector = new Vector3(
                        spawnPoints[0].position.x /*Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f))*/,
                        Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);
            Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity);

            spawnVector = new Vector3(
                        spawnPoints[1].position.x /*Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f))*/,
                        Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);
            Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity);
        }
    }

    void SpawnClouds(float topEdgeY)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnVector = new Vector3(
                        Random.Range(spawnPoints[i].position.x + 1f, spawnPoints[i].position.x - 1f) /*Random.Range(leftEdge.x + (PLATFORM_WIDTH / 2f), rightEdge.x - (PLATFORM_WIDTH / 2f))*/,
                        Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
                        0f);

            if (platformInstances[i] == null || spawnVector.y - platformInstances[i].transform.position.y > spawnDist) //Matt
            {
                platformInstances[i] = Instantiate(templatesCloud[Random.Range(0, templatesCloud.Length)], spawnVector, Quaternion.identity) as GameObject;
            }
        }
    }

    void SpawnHazard(float topEdgeY)
    {
        spawnVectorHazard = new Vector3(
            0f,
            Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
            0f);
        if(hazardInstance == null || spawnVector.y - hazardInstance.transform.position.y > spawnDist)
        {
            hazardInstance = Instantiate(hazardPrefabs[0], spawnVectorHazard, Quaternion.identity) as GameObject;
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
