using System.Collections;
using System.Reflection;
using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
	public float speed;
	public bool isGood;
	public float timeLeft = 3;
	public bool shouldFade;
	
	private float InitialTimeLeft;
	private bool playerPresent;

	SpriteRenderer platformRenderer;
	private float alpha = 1.0f;
	ParticleSystem particles;
	Animator animator;

	private Camera mCamera;
	float camHeight;
	private Vector3 bottomEdge;
	private bool isLeaf;
	private PlatformGenerator Parent;

	// Start is called before the first frame update
	void Awake()
	{
		particles = this.gameObject.GetComponentInChildren<ParticleSystem>();
		animator = gameObject.GetComponent<Animator>();
		playerPresent = false;
		platformRenderer = gameObject.GetComponent<SpriteRenderer>();
		InitialTimeLeft = timeLeft;
		mCamera = Camera.main;
		camHeight = Mathf.Abs(Vector3.Distance(
			mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10)),
			mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10))
		));

		Catformer.PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<Catformer.PlayerScript>();
		if(player.GetScore() >= 160f)
		{
			timeLeft = 2f;
			InitialTimeLeft = 2f;
		}
		else if(player.GetScore() >= 300f)
		{
			timeLeft = 1f;
			InitialTimeLeft = 1f;
		}

		isLeaf = platformRenderer.sprite.name.Contains("Leaf");
	}

	// Update is called once per frame
   void Update()
	{
		if(isLeaf)
			transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

		if (playerPresent == true && shouldFade)
		{
			timeLeft -= Time.deltaTime;
			alpha = ((timeLeft / InitialTimeLeft)) * (1f) / (InitialTimeLeft - timeLeft);
			platformRenderer.color = new Color(1, 1, 1, alpha);
			if (timeLeft <= 0)
			{
				Recycle();
			}
		}

		if (OutOfFrame()/*(mCamera.transform.position.y - transform.position.y) > 20f*/)
		{
			Recycle();
		}
	}

	private bool OutOfFrame()
	{
		Vector3 camBottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 10));
		Bounds box = platformRenderer.bounds;

		return box.max.y < camBottom.y;
	}

	private void Recycle()
	{
		Parent = transform.parent.GetComponent<PlatformGenerator>();
		particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		transform.position = Parent.StartLocation;
		gameObject.SetActive(false);
	}

	public void Restart(PlatformData data, Vector3 location)
	{
		gameObject.SetActive(true);
		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		platformRenderer.sprite = data.sprite;
		platformRenderer.color = Color.white;
		playerPresent = false;
		speed = data.speed;
		isGood = data.isGood;
		timeLeft = data.timeLeft;
		InitialTimeLeft = timeLeft;
		shouldFade = data.shouldFade;
		collider.size = data.Size;
		isLeaf = platformRenderer.sprite.name.Contains("Leaf");
		transform.localRotation = Quaternion.Euler(data.Rotation);
		transform.localScale = data.Scale;
		transform.position = location;

		// The property info for the ParticleSystem class type and the saved instance
		PropertyInfo[] particleProps = particles.GetType().GetProperties();
		ParticleSystem savedSystem = data.ParticlePrefab.GetComponent<ParticleSystem>();

		// For each property in this class,
		foreach (var property in particleProps)
		{
			// Grab each particle module (except subemitters which aren't used)
			if(property.PropertyType.Name.Contains("Module") && !property.PropertyType.Name.Contains("SubEmitters"))
			{
				// Grab the correspoinding module object on this GameObject, grab its properties,
				object module = property.GetValue(particles);
				PropertyInfo[] properties = module.GetType().GetProperties();
				object newModule = savedSystem.GetType().GetProperty(property.Name).GetValue(savedSystem);

				// Then loop through these properties and set them to the value from the prefab object.
				foreach (var prop in properties)
				{
					if(prop.GetIndexParameters().Length == 0 && prop.CanWrite)
					{
						prop.SetValue(module, prop.GetValue(newModule));
					}
				}
			}
		}

		// Repeat the process for the ParticleSystemRenderer.
		ParticleSystemRenderer renderer = GetComponentInChildren<ParticleSystemRenderer>(),
			savedRenderer = data.ParticlePrefab.GetComponent<ParticleSystemRenderer>();
		particleProps = renderer.GetType().GetProperties();

		foreach(var property in particleProps)
		{
			if(property.GetIndexParameters().Length == 0 && property.CanRead && property.CanWrite && !property.Name.Contains("materials", true))
			{
				if (property.Name.Contains("material", true) && !property.Name.Equals("sharedMaterial"))
					continue;
				property.SetValue(renderer, property.GetValue(savedRenderer));
			}
		}

		transform.GetChild(1).localPosition = data.ParticlePrefab.transform.localPosition;
	}

	// Play particles
	private void OnMouseDown() 
	{
		if (Time.timeScale > 0)
		{
			bottomEdge = mCamera.ViewportToWorldPoint(new Vector3(0, 0, 10));
			
			if (particles != null && !particles.isPlaying)
			{
				//particles.Play();
			}

			if (animator != null)
				animator.SetTrigger("Zapp");
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
		if (isGood == true && collision.gameObject.CompareTag("Player") && playerRef.GetComponent<Catformer.PlayerScript>().GetTarget() == gameObject)
		{
			playerPresent = true;
			particles.Play();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		
	}
	IEnumerator FadeOut()
	{
		Debug.Log("Fading out: " + name);

		while (platformRenderer.color.a > 0f)
		{
			if (Time.timeScale < 1f)
				yield return new WaitForEndOfFrame();
			alpha = ((timeLeft / InitialTimeLeft)) * (1f) / (InitialTimeLeft - timeLeft); 
			platformRenderer.color = new Color(1, 1, 1, alpha);
			yield return new WaitForEndOfFrame();
		}
	}
}



