using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Camera cameraRef;
    SpriteRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cameraRef = Camera.main;
        mRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.BoxCast(cameraRef.ScreenToWorldPoint(Input.mousePosition), new Vector2(0.01f, 0.01f), 0, Vector2.zero);
            if (hit.collider != null)
            {
                Color boxColor = hit.collider.gameObject.GetComponent<SpriteRenderer>().color;
                if(boxColor == Color.white)
                {
                    mRenderer.color = new Color(96, 183, 19);
                }
                else if(boxColor == Color.red)
                {
                    
                }
            }
        }
    }
}
