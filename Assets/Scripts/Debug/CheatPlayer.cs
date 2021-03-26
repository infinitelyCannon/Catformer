using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatPlayer : MonoBehaviour
{
    public float Speed = 10.0f;
    public bool autoRun = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = autoRun ? 1.0f : Input.GetAxisRaw("Vertical");

        transform.position += Vector3.up * Speed * vertical * Time.deltaTime;
    }
}
