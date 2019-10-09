using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Camera cameraRef;
    SpriteRenderer mRenderer;
    public Vector3 targetMove;
    public Vector3 prevMove;
    public float speed;
    public GameObject player;

    GameObject targetPlat;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        targetMove = transform.position;
        targetPlat = null;
        cameraRef = Camera.main;
        mRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlat != null)
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

            if(touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.BoxCast(cameraRef.ScreenToWorldPoint(touch.position), new Vector2(0.01f, 0.01f), 0, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("Color: " + hit.collider.gameObject.GetComponent<SpriteRenderer>().color);

                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                        targetMove = hit.collider.transform.GetChild(0).position;
                        targetPlat = hit.collider.gameObject;
                        //transform.position = targetMove;
                        speed = hit.collider.gameObject.GetComponent<Plaform>().speed;

                    }
                    else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.BoxCast(cameraRef.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.01f, 0.01f), 0, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("Color: " + hit.collider.gameObject.GetComponent<SpriteRenderer>().color);

                if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                    targetMove = hit.collider.transform.GetChild(0).position;
                    targetPlat = hit.collider.gameObject;
                    //transform.position = targetMove;
                    speed = hit.collider.gameObject.GetComponent<Plaform>().speed;
                    
                }
                else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                {
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                }
            }
        }
    }
 
}
