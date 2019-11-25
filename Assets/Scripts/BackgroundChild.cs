using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChild : MonoBehaviour
{
    public delegate void InvisibilityTrigger();
    public InvisibilityTrigger invisibilityTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible()
    {
        invisibilityTrigger();
    }
}
