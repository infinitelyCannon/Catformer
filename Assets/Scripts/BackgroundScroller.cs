using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public enum Stage
    {
        Ground,
        Forest,
        ForestToSky,
        Sky,
        SkyToSpace,
        Space
    };

    [System.Serializable]
    public class BackgroundStage
    {
        public Sprite sprite;
        public int times;
    };

    public float speed;
    public GameObject secondBackground;
    public BackgroundStage[] backgroundStages;

    private Vector3 size;
    private Camera mCamera;
    private SpriteRenderer mRenderer;
    private int stageIndex = 0;
    private SpriteRenderer secondRenderer;
    private Stage stage = Stage.Forest;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
        secondRenderer = secondBackground.GetComponent<SpriteRenderer>();

        Resize();
        transform.position = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f)) + Vector3.up * size.y / 2f;
        secondBackground.transform.position = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f)) + Vector3.up * (size.y + size.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        //Movement: Find out where the camera is and if it's moving, loop the background around the camera.
        float cameraSpeed = mCamera.GetComponent<CameraController>().getVelocity().magnitude;
        Vector3 cameraBottom = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f));
        transform.position += Vector3.up * cameraSpeed * Time.deltaTime * -speed;
        secondBackground.transform.position += Vector3.up * cameraSpeed * Time.deltaTime * -speed;

        if(Vector3.Distance(transform.position, cameraBottom) >= size.y / 2f && transform.position.y < mCamera.transform.position.y)
        {
            transform.position = secondBackground.transform.position + new Vector3(0f, size.y, 0f);
            CheckStage(true);
        }
        else if (Vector3.Distance(secondBackground.transform.position, cameraBottom) >= size.y / 2f && secondBackground.transform.position.y < mCamera.transform.position.y)
        {
            secondBackground.transform.position = transform.position + new Vector3(0f, size.y, 0f);
            CheckStage(false);
        }
    }

    private void CheckStage(bool checkFirstImage)
    {
        SpriteRenderer renderer;
        if (checkFirstImage)
            renderer = mRenderer;
        else
            renderer = secondRenderer;

        if (backgroundStages[stageIndex].times > 0)
        {
            if (backgroundStages[stageIndex].times - 1 <= 0)
            {
                stageIndex++;
                renderer.sprite = backgroundStages[stageIndex].sprite;
            }
            else
            {
                renderer.sprite = backgroundStages[stageIndex].sprite;
                backgroundStages[stageIndex].times -= 1;
            }
        }
        else
        {
            renderer.sprite = backgroundStages[stageIndex].sprite;
        }
        if (renderer.sprite.name.Contains("space"))
        {
            renderer.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public Stage GetStage()
    {
        return stage;
    }

    void Resize()
    {
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        float scale = Vector3.Distance(left, right) / size.x;

        transform.localScale = new Vector3(scale, scale, 1f);
        secondBackground.transform.localScale = new Vector3(scale, scale, 1f);
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
    }
}
