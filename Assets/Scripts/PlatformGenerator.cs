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

	public enum Place
	{
		Forest,
		Sky,
		Space
	};

	public GameObject Template;
	public float spawnRate;
	public Transform[] spawnPoints;
	public PlatformData[] leafData;
	public PlatformData[] cloudData;
	public Sprite[] sunsetSprites;
	public PlatformData[] meteorData;

	public float cloudHeight;
	public float sunsetCloudHeight;
	public float spaceHeight;

	public int MaxPlatforms = 20;
	public Vector3 StartLocation;

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
	private float mTime = 0;
	private List<GameObject> Platforms;

	private void Awake()
	{
		playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
	}

	// Start is called before the first frame update
	void Start()
	{
		mCamera = Camera.main;
		platformInstances = new GameObject[spawnPoints.Length];
		Platforms = new List<GameObject>(MaxPlatforms);

		for (int i = 0; i < MaxPlatforms; ++i)
		{
			GameObject platform = Instantiate(Template, StartLocation, Quaternion.identity, transform);
			platform.SetActive(false);
			Platforms.Add(platform);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// The target the player will move to.
		GameObject startPoint;

		if (playerRef == null)
			return;
		
		elevation = playerRef.gameObject.GetComponent<Catformer.PlayerScript>().GetScore();

		// The position of the green line the camera uses to follow the player
		Vector3 followLine = mCamera.ViewportToWorldPoint(new Vector3(0.5f, mCamera.gameObject.GetComponent<CameraController>().followLine, 10f));

		// The top, right, and left edges of the camera.
		Vector3 topEdge = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10f));

		/*
		 * If we're in the forest stage,
		 * spawn a leaf after each spawn rate
		 */
		if(elevation <= cloudHeight && Time.timeScale > 0)
		{
			mTime += Time.deltaTime;
			if(mTime >= spawnRate)
			{
				SpawnPlatforms(topEdge.y + 5);
				mTime -= mTime;
			}
		}

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
				if (elevation >= cloudHeight && elevation <= spaceHeight)
					SpawnClouds(topEdge.y + 5);
				else if (elevation >= spaceHeight)
					SpawnMeteors(topEdge.y + 5);
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
				if (elevation > cloudHeight && elevation <= spaceHeight)
					SpawnClouds(topEdge.y + 5);
				else if (elevation >= spaceHeight)
					SpawnMeteors(topEdge.y + 5);
			}
		}
	}

	float FindSecondSide(Vector3 size1, Vector3 size2, float xSpot)
	{
		Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
		Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));

		if(xSpot > 0)
		{
			return Random.Range(leftEdge.x + (size2.x / 2f), xSpot - (size1.x / 2f));
		}
		else
		{
			return Random.Range(xSpot + (size1.x / 2f), rightEdge.x - (size2.x / 2f));
		}
	}

	private bool DoesOverlap(Vector3 location, Vector3 size)
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(location, size, 0f, LayerMask.GetMask("Platforms"));

		if (colliders.Length > 0)
			return true;

		return false;
	}

	private int GetPlatform(PlatformData[] choices, int choice, Vector3 position)
	{
		int index = 0;

		for (; index < Platforms.Count; ++index)
		{
			if (!Platforms[index].activeSelf)
			{
				Platforms[index].GetComponent<Platform>().Restart(choices[choice], position);
				return index;
			}
		}

		Platforms.Sort(delegate(GameObject x, GameObject y) 
		{
			if (x == null || y == null) return 0;
			return x.transform.position.y.CompareTo(y.transform.position.y);
		});

		Debug.Log("No free platform.\nGrabbing lowest one...");
		Platforms[0].GetComponent<Platform>().Restart(choices[choice], position);
		return 0;
	}

	void SpawnMeteors(float topEdgeY)
	{
		Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
		Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
		float xSpot;

		for (int i = 0; i < spawnPoints.Length; i++)
		{
			int selection = Random.Range(0, meteorData.Length);

			Vector3 size = meteorData[selection].sprite.bounds.size * meteorData[selection].Scale.x;
			xSpot = Random.Range(leftEdge.x + (size.x / 2f), rightEdge.x - (size.x / 2f));

			spawnVector = new Vector3(
						xSpot,//Random.Range(spawnPoints[i].position.x + 1f, spawnPoints[i].position.x - 1f),
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);

			if(!DoesOverlap(spawnVector, size))
			{
				if (platformInstances[i] == null || spawnVector.y - platformInstances[i].transform.position.y > spawnDist) //Matt
				{
					platformInstances[i] = Platforms[GetPlatform(meteorData, selection, spawnVector)];//Instantiate(meteorPrefabs[selection], spawnVector, Quaternion.identity, transform) as GameObject;
				}
			}
		}
	}

	void SpawnPlatforms(float topEdgeY)
	{
		int spot = Random.Range(1, 4);
		Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
		Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));

		if (spot == 1)
		{
			int selection = Random.Range(0, leafData.Length);
			Vector3 size = leafData[selection].sprite.bounds.size * leafData[selection].Scale.x;
			float xSpot = Random.Range(leftEdge.x + (size.x / 2f), rightEdge.x - (size.x / 2f));

			spawnVector = new Vector3(
						xSpot,
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);
			if (!DoesOverlap(spawnVector, size))
				GetPlatform(leafData, selection, spawnVector);
		}
		else if (spot == 2)
		{
			int selection = Random.Range(0, leafData.Length);
			Vector3 size = leafData[selection].sprite.bounds.size * leafData[selection].Scale.x;
			float xSpot = Random.Range(leftEdge.x + (size.x / 2f), rightEdge.x - (size.x / 2f));

			spawnVector = new Vector3(
						xSpot,
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);
			if (!DoesOverlap(spawnVector, size))
				GetPlatform(leafData, selection, spawnVector);//Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity, transform);
		}
		else // spot == 3
		{
			int selection1 = Random.Range(0, leafData.Length);
			Vector3 size = leafData[selection1].sprite.bounds.size * leafData[selection1].Scale.x;
			float xSpot1 = Random.Range(leftEdge.x + (size.x / 2f), rightEdge.x - (size.x / 2f));

			int selection2 = Random.Range(0, leafData.Length);
			Vector3 size2 = leafData[selection2].sprite.bounds.size * leafData[selection2].Scale.x;
			float xSpot2 = FindSecondSide(size, size2, xSpot1);

			spawnVector = new Vector3(
						xSpot1,
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);
			if(!DoesOverlap(spawnVector, size))
				GetPlatform(leafData, Random.Range(0, leafData.Length), spawnVector);//Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity, transform);

			spawnVector = new Vector3(
						xSpot2,
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);
			if(!DoesOverlap(spawnVector, size2))
				GetPlatform(leafData, Random.Range(0, leafData.Length), spawnVector);//Instantiate(templates[Random.Range(0, templates.Length)], spawnVector, Quaternion.identity, transform);
		}
	}

	void SpawnClouds(float topEdgeY)
	{
		Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
		Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
		float xSpot;

		for (int i = 0; i < spawnPoints.Length; i++)
		{
			int selection = Random.Range(0, cloudData.Length);

			Vector3 size = cloudData[selection].sprite.bounds.size * cloudData[selection].Scale.x;
			xSpot = Random.Range(leftEdge.x + (size.x / 2f), rightEdge.x - (size.x / 2f));

			spawnVector = new Vector3(
						xSpot,//Random.Range(spawnPoints[i].position.x + 1f, spawnPoints[i].position.x - 1f),
						Random.Range(topEdgeY + (PLATFORM_HEIGHT / 2f), (topEdgeY + SpawnHeight) - (PLATFORM_HEIGHT / 2f)),
						0f);

			if (!DoesOverlap(spawnVector, size))
			{
				if (platformInstances[i] == null || spawnVector.y - platformInstances[i].transform.position.y > spawnDist) //Matt
				{
					platformInstances[i] = Platforms[GetPlatform(cloudData, selection, spawnVector)];//Instantiate(templatesCloud[selection], spawnVector, Quaternion.identity, transform) as GameObject;
					if (elevation >= sunsetCloudHeight && elevation <= spaceHeight)
						platformInstances[i].GetComponent<SpriteRenderer>().sprite = sunsetSprites[Random.Range(0, sunsetSprites.Length)];
				}
			}
		}
	}
}
