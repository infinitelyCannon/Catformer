using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Camera cameraRef;
    SpriteRenderer mRenderer;
    public Vector3 targetMove;
    public Vector3 prevMove;
    public float speed;
    public GameObject player;
    


    GameObject targetPlat;
    bool isDead = false;
    float playTime = 0;

    public float touchRadius;

    [Header("Debug Variables")]
    public int deathTextSize;
    public int deathTextWidth;
    public int deathTextHeight;
    public Color deatTextColor;

    private int platformMask;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        targetPlat = null;
        cameraRef = Camera.main;
        mRenderer = GetComponent<SpriteRenderer>();
        platformMask = LayerMask.GetMask("Platforms");
      

        float halfWidth = mRenderer.sprite.bounds.extents.x;
        Vector3 rightEdge = cameraRef.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
        Vector3 leftEdge = cameraRef.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
        Vector3 leftSpot = leftEdge + new Vector3(halfWidth, 0f, 0f);
        Vector3 rightSpot = rightEdge - new Vector3(halfWidth, 0f, 0f);

        if (transform.position.x < leftSpot.x && transform.position.x < cameraRef.transform.position.x)
            transform.position = new Vector3(leftSpot.x, transform.position.y, 0f);
        else if (transform.position.x > rightSpot.x && transform.position.x > cameraRef.transform.position.x)
            transform.position = new Vector3(rightSpot.x, transform.position.y, 0f);

        targetMove = transform.position;

        if (deathTextSize == 0)
            deathTextSize = 84;
        if (deathTextWidth == 0)
            deathTextWidth = 1100;
        if (deathTextHeight == 0)
            deathTextHeight = 420;
        if (deatTextColor == Color.clear)
            deatTextColor = Color.magenta;
    }

    private void OnBecameInvisible()
    {
        isDead = true;
    }

    private void OnGUI()
    {
        if (isDead)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(playTime);

            GUI.color = deatTextColor;
            GUIStyle skin = new GUIStyle(GUI.skin.label);
            skin.fontSize = deathTextSize;
            GUI.Label(new Rect((cameraRef.pixelWidth / 2) - (deathTextWidth/2), cameraRef.pixelHeight / 2, deathTextWidth, deathTextHeight), "You Died ฅ(＾x ω x＾ฅ)∫\nTotal Time: " + t.ToString(@"hh\:mm\:ss\:fff"), skin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (targetPlat != null && targetPlat.transform.childCount > 0)
                targetMove = targetPlat.transform.GetChild(0).position;
            transform.position = Vector3.Lerp(transform.position, targetMove, 0.1f);
            // transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

            if (transform.parent != null)
            {
                transform.localPosition = Vector3.zero;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];

                if (touch.phase == TouchPhase.Began)
                {
                    //RaycastHit2D hit = Physics2D.BoxCast(cameraRef.ScreenToWorldPoint(touch.position), new Vector2(0.01f, 0.01f), 0, Vector2.zero);
                    Collider2D hit = Physics2D.OverlapCircle(cameraRef.ScreenToWorldPoint(touch.position), touchRadius, platformMask);
                    if (hit != null)
                    {
                        if (hit.gameObject.GetComponent<Platform>() != null && hit.gameObject.GetComponent<Platform>().isGood)
                        {
                            //hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                            targetMove = hit.transform.GetChild(0).position;
                            targetPlat = hit.gameObject;
                            //transform.position = targetMove;
                            speed = hit.gameObject.GetComponent<Platform>().speed;
                            
                            if (hit.gameObject.GetComponent<Platform>().transform.position.x < 0.0f)
                            {
                                mRenderer.flipX = true;
                                Debug.Log("should flip");
                            }
                            else if(hit.gameObject.GetComponent<Platform>().transform.position.x >-0.0f)
                            {
                                mRenderer.flipX = false;
                                Debug.Log("should flip");
                            }
                        }
                        /*else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                        {
                            hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                        }*/
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Collider2D hit = Physics2D.OverlapCircle(cameraRef.ScreenToWorldPoint(Input.mousePosition), touchRadius, platformMask);
                if (hit != null)
                {
                    if (hit.gameObject.GetComponent<Platform>() != null && hit.gameObject.GetComponent<Platform>().isGood)
                    {
                        //hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                        targetMove = hit.transform.GetChild(0).position;
                        targetPlat = hit.gameObject;
                        //transform.position = targetMove;
                        speed = hit.gameObject.GetComponent<Platform>().speed;

                        if (hit.gameObject.GetComponent<Platform>().transform.position.x < 0.0f)
                        {
                            mRenderer.flipX = true;
                            Debug.Log("should flip");
                        }
                        else if (hit.gameObject.GetComponent<Platform>().transform.position.x >= 0.0f)
                        {
                            mRenderer.flipX = false;
                            Debug.Log("should flip");
                        }
                    }
                    /*else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                    }*/
                }
            }

            playTime += Time.deltaTime;
        }
    }
 
}
