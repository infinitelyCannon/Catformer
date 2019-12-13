using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePositionTest : MonoBehaviour
{
    Camera mCamera;
    SpriteRenderer mRenderer;
    Vector3 size;

    public GameObject test;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
        Resize();
    }

    // Update is called once per frame
    void Update()
    {
        test.transform.position = transform.TransformPoint(new Vector3(x, y, 0));
    }

    void Resize()
    {
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        float scale = Vector3.Distance(left, right) / size.x;

        transform.localScale = new Vector3(scale, scale, 1f);
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
    }
}
