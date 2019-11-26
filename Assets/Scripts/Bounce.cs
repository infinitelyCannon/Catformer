using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private Transform arrowPosition;
    
    void Start()
    {
        arrowPosition = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        arrowPosition.position += new Vector3(0, Mathf.Sin(Time.time * 10) * 0.8f, 0);

        if (Input.touchCount > 0) 
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                Destroy(gameObject);
            }
                
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
