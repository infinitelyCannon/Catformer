using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePositionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Vector3 rect, text;
        rect = renderer.sprite.bounds.size;
        text = new Vector3(renderer.sprite.texture.width, renderer.sprite.texture.height, 0);

        Debug.Log(name + " " + "Bounds: " + rect + " Texture: " + text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
