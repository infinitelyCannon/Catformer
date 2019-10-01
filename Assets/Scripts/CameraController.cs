using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDelta;

        moveDelta.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        moveDelta.y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        moveDelta.z = 0f;

        transform.position += moveDelta;
    }
}
