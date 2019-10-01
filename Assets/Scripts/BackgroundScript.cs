using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float speed;
    public GameObject background;

    Camera mCamera;
    private float distance;
    private Vector3 screenBottom;
    bool hasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        mCamera = Camera.main;
        //distance = Vector3.Distance(mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0)), Vector3.zero);
        screenBottom = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 10));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -1f) * speed * Time.deltaTime;
        if (Vector3.Distance(screenBottom, transform.position) <= 3f && !hasSpawned)
        {
            Instantiate(background, new Vector3(transform.position.x, transform.position.y + 19.2f, 0), Quaternion.identity);
            hasSpawned = true;
        }

        if (transform.position.y <= -15f)
            Destroy(gameObject);
    }
}
