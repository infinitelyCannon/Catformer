using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catformer;
public class Hazard : MonoBehaviour
{
	public float speed;
	public GameObject caughtSpace;
	public AudioSource mSound;

	private Camera mCamera;

	void Start()
	{
		AudioSource[] sources = GetComponents<AudioSource>();
		foreach (AudioSource source in sources)
		{
			if (Catformer.SavedPreferences.instance != null)
				source.volume = Catformer.SavedPreferences.instance.soundVolume;
		}

		mCamera = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 rightEdge = mCamera.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
		Vector3 leftEdge = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));

		if (gameObject.tag == "PPHazard")
			transform.position = new Vector3(Mathf.PingPong(speed, rightEdge.x), transform.position.y, 0f);
		if (gameObject.tag == "FlyingHazard")
		{
			transform.Translate(speed * Time.deltaTime, -1 * Time.deltaTime, 0);
			Destroy(gameObject, 5);
		}
		if (gameObject.tag == "MeteorHazard")
		{
			transform.Translate(speed * Time.deltaTime, -1 * Time.deltaTime, 0);
			gameObject.transform.Rotate(0, 0, -30);
			Destroy(gameObject, 5);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	   if (collision.gameObject.CompareTag("Player"))
		{
			//Debug.Log("HitPlayer");
			collision.transform.parent = caughtSpace.transform;
			collision.gameObject.GetComponent<Catformer.PlayerScript>().isDead = true;
			mSound.Play();

			GameObject catdude = collision.gameObject;
		   // catdude.transform.Rotate(0, 0, -60);
			
		}
	}

}
