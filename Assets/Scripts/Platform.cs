using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public bool isGood;
    float timeLeft = 3;
    float InitialTimeLeft;
    public bool playerPresent;
    SpriteRenderer platformRenderer;
    private float alpha = 1.0f;
    ParticleSystem particles;
    Animator animator;

    private Camera mCamera;
    float camHeight;
    private Vector3 bottomEdge;

    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
   void Update()
    {
        //transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

        if (playerPresent == true)
        {
            timeLeft -= Time.deltaTime;
            alpha = ((timeLeft / InitialTimeLeft)) * (1f) / (InitialTimeLeft - timeLeft);
            platformRenderer.color = new Color(1, 1, 1, alpha);
            if (timeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }

        if ((mCamera.transform.position.y - transform.position.y) > 20f)
        {
            Destroy(gameObject);
        }
    }

    // Play particles
    private void OnMouseDown() 
    {
        if (Time.timeScale > 0)
        {
            bottomEdge = mCamera.ViewportToWorldPoint(new Vector3(0, 0, 10));
            
            if (particles != null && !particles.isPlaying)
            {
                particles.Play();
            }

            if (animator != null)
                animator.SetTrigger("Zapp");

            if (isGood == true)
            {
                playerPresent = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerPresent == true)
        {
            //Instantiate(gameObject, new Vector3(SpawnGrid[12].x, SpawnGrid[12].y + mCamera.transform.position.y + camHeight / 3f, 0), Quaternion.identity);
            //StartCoroutine(FadeOut());
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



