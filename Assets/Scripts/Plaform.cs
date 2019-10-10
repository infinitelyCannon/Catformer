using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plaform : MonoBehaviour
{
    public float speed;
    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = this.gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
   void Update()
    {
        transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
    }

    // Play particles
    private void OnMouseDown() 
    {
        if (!particles.isPlaying) 
        {
            particles.Play();
        }
    }

}



