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

    public float speed;
    public BackgroundType type;
    public GameObject background;

    [Header("Background Stage Data")]
    public List<int> heightLimits;
    public List<Sprite> sprites;
    //public int[] heightTriggers;
    //public Sprite[] sprites;

    Camera mCamera;
    protected static int foregroundSpawns = 0;
    protected static int backgroundSpawns = 0;

    //private float distance;
    private Vector3 screenBottom;
    bool hasSpawned = false;

    //private float ASPECT_RATIO;
    private float unitsPerPixelX;
    private float unitsPerPixelY;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        //distance = Vector3.Distance(mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0)), Vector3.zero);
        screenBottom = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, 10));
        unitsPerPixelX = renderer.sprite.texture.width / renderer.sprite.pixelsPerUnit;
        unitsPerPixelY = renderer.sprite.texture.height / renderer.sprite.pixelsPerUnit;
        //ASPECT_RATIO = (float)renderer.sprite.texture.width / renderer.sprite.texture.height;
        Resize();

        if (type == BackgroundType.Foreground)
            foregroundSpawns++;
        else
            backgroundSpawns++;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -1f) * speed * Time.deltaTime;
        if (Vector3.Distance(screenBottom, transform.position) <= 3f && !hasSpawned)
        {
            if(type == BackgroundType.Foreground)
            {
                if(foregroundSpawns >= heightLimits[0] && heightLimits.Count > 1 && sprites.Count > 1)
                {
                    GameObject newBG = Instantiate(background, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    sprites.RemoveAt(0);
                    heightLimits.RemoveAt(0);
                    newBG.GetComponent<BackgroundScript>().SetSprite(1);
                    newBG.GetComponent<BackgroundScript>().sprites = new List<Sprite>(sprites);
                    newBG.GetComponent<BackgroundScript>().heightLimits = new List<int>(heightLimits);
                }
                else if(foregroundSpawns < heightLimits[0])
                    Instantiate(background, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
            }
            else
            {
                if (backgroundSpawns >= heightLimits[0] && heightLimits.Count > 1 && sprites.Count > 1)
                {
                    GameObject newBG = Instantiate(background, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
                    sprites.RemoveAt(0);
                    heightLimits.RemoveAt(0);
                    newBG.GetComponent<BackgroundScript>().SetSprite(1);
                    newBG.GetComponent<BackgroundScript>().sprites = new List<Sprite>(sprites);
                    newBG.GetComponent<BackgroundScript>().heightLimits = new List<int>(heightLimits);
                }
                else if(backgroundSpawns < heightLimits[0])
                    Instantiate(background, new Vector3(transform.position.x, transform.position.y + /*38f*/(unitsPerPixelY * transform.localScale.y), 0), Quaternion.identity);
            }
            
            hasSpawned = true;
        }

        if (transform.position.y <= unitsPerPixelY * -1.02f)
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
