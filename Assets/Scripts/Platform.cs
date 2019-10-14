using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public bool isGood;

    ParticleSystem particles;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        particles = this.gameObject.GetComponentInChildren<ParticleSystem>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
   void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
    }

    // Play particles
    private void OnMouseDown() 
    {
        if (particles != null && !particles.isPlaying) 
        {
            particles.Play();
        }

        if (animator != null)
            animator.SetTrigger("Zapp");
    }

}



