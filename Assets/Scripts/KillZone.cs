using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    Camera mCamera;
    EdgeCollider2D edgeCollider;

    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main;
        edgeCollider = GetComponent<EdgeCollider2D>();


        Vector3 bottom = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f));
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        Vector2[] points = { new Vector2(left.x, left.y), new Vector2(right.x, right.y) };

        transform.position = bottom + new Vector3(0f, -6f);
        edgeCollider.points = points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
