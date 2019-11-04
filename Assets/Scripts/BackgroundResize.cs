using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour
{
    private float unitsPerPixelX;
    private float unitsPerPixelY;

    private Camera mCamera;
    private CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        cameraController = mCamera.gameObject.GetComponent<CameraController>();
        //distance = Vector3.Distance(mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0)), Vector3.zero);
        //screenTop = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10));
        unitsPerPixelX = renderer.sprite.texture.width / renderer.sprite.pixelsPerUnit;
        unitsPerPixelY = renderer.sprite.texture.height / renderer.sprite.pixelsPerUnit;
        //ASPECT_RATIO = (float)renderer.sprite.texture.width / renderer.sprite.texture.height;
        Resize();

        GameObject.Find("ScrollingForest").transform.position = new Vector3(0, transform.position.y + (unitsPerPixelY * transform.localScale.y), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Resize()
    {
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        float xSize = Vector3.Distance(left, right) / unitsPerPixelX;

        transform.localScale = new Vector3(xSize, xSize, 1f);
    }
}
