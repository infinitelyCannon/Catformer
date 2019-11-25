using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour
{
    private Camera mCamera;
    private CameraController cameraController;
    private Vector3 size;
    private SpriteRenderer mRenderer;

    private float dist;

    void Start()
    {
        mCamera = Camera.main;
        mRenderer = GetComponent<SpriteRenderer>();
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
        cameraController = mCamera.gameObject.GetComponent<CameraController>();
        Resize();

        transform.position = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f));
    }

    // Update is called once per frame
    void Update()
    {
        
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
