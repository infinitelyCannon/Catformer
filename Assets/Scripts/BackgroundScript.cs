using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public enum BackgroundType
    {
        Foreground,
        Background
    };

    public float relativeCameraSpeed;
    public BackgroundType type;
    public GameObject background;

    [Header("Background Stage Data")]
    public List<int> heightLimits;
    public List<Sprite> sprites;
    //public int[] heightTriggers;
    //public Sprite[] sprites;

    protected static int foregroundSpawns = 0;
    protected static int backgroundSpawns = 0;

    //private float distance;
    private Vector3 screenTop;
    private Vector3 screenBottom;
    private bool hasSpawned = false;
    private Camera mCamera;
    private CameraController cameraController;
    private float speed = 0f;

    //private float ASPECT_RATIO;
    private float unitsPerPixelX;
    private float unitsPerPixelY;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        cameraController = mCamera.gameObject.GetComponent<CameraController>();
        //distance = Vector3.Distance(mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0)), Vector3.zero);
        screenTop = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10));
        unitsPerPixelX = renderer.sprite.texture.width / renderer.sprite.pixelsPerUnit;
        unitsPerPixelY = renderer.sprite.texture.height / renderer.sprite.pixelsPerUnit;
        //ASPECT_RATIO = (float)renderer.sprite.texture.width / renderer.sprite.texture.height;
        Resize();

        if (type == BackgroundType.Foreground)
            foregroundSpawns++;
        else
            backgroundSpawns++;

        if (heightLimits[0] > 0)
            heightLimits[0]--;
    }

    // Update is called once per frame
    void Update()
    {
        speed = cameraController.getVelocity().magnitude * relativeCameraSpeed;
        screenTop = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10));
        transform.position += new Vector3(0, -1f) * speed * Time.deltaTime;

        //if (Mathf.Abs(screenTop.y - (transform.position.y + (unitsPerPixelY / 2f) * transform.localScale.y)) <= 1f && !hasSpawned)
        if(!hasSpawned && heightLimits[0] >= 0)
        {
            if (type == BackgroundType.Foreground)
            {
                if (heightLimits[0] == 0 && heightLimits.Count > 1 && sprites.Count > 1)
                {
                    GameObject newBG = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    sprites.RemoveAt(0);
                    heightLimits.RemoveAt(0);
                    newBG.name = "Foreground " + foregroundSpawns;
                    newBG.GetComponent<BackgroundScript>().SetSprite(1);
                    newBG.GetComponent<BackgroundScript>().sprites = new List<Sprite>(sprites);
                    newBG.GetComponent<BackgroundScript>().heightLimits = new List<int>(heightLimits);
                }
                else if (heightLimits[0] > 0)
                {
                    GameObject newBG = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    newBG.name = "Foreground " + foregroundSpawns;
                } 
            }
            else
            {
                if (heightLimits[0] == 0 && heightLimits.Count > 1 && sprites.Count > 1)
                {
                    GameObject newBG = Instantiate(background, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    sprites.RemoveAt(0);
                    heightLimits.RemoveAt(0);
                    newBG.name = "Background " + backgroundSpawns;
                    newBG.GetComponent<BackgroundScript>().SetSprite(1);
                    newBG.GetComponent<BackgroundScript>().sprites = new List<Sprite>(sprites);
                    newBG.GetComponent<BackgroundScript>().heightLimits = new List<int>(heightLimits);
                }
                else if (heightLimits[0] > 0)
                {
                    GameObject newBG = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    newBG.name = "Background " + backgroundSpawns;
                }
            }

            hasSpawned = true;
        }
        else if(Mathf.Abs(screenTop.y - (transform.position.y + (unitsPerPixelY / 2f) * transform.localScale.y)) <= 1f && !hasSpawned)
        {
            GameObject newBG = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
            newBG.name = (type == BackgroundType.Background ? "Background " + backgroundSpawns % 2 : "Foreground " + foregroundSpawns % 2);
            hasSpawned = true;
        }

        screenBottom = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 10));
        if (screenBottom.y > (transform.position.y + (unitsPerPixelY / 2f) * transform.localScale.y))
            Destroy(gameObject);
    }

    void Resize()
    {
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        float xSize = Vector3.Distance(left, right) / unitsPerPixelX;

        transform.localScale = new Vector3(xSize, xSize, 1f);
    }

    public void SetSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
