using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePositionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Vector3 rect, text, scaled;
        rect = renderer.sprite.bounds.size;
        text = new Vector3(renderer.sprite.texture.width, renderer.sprite.texture.height, 0);
        scaled = rect * transform.localScale.x;

        Debug.Log(name + " " + "Bounds: " + rect + " Texture: " + text + " Scaled: " + scaled);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
