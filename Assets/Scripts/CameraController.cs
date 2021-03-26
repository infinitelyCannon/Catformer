using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float elevationMultiplier = 1.0f;
    
    [Tooltip("How long it takes to reach the player while moving\n0 means snap to immediately.")]
    public float damping;

    [Range(0.0f, 1.0f), Tooltip("The level the camera uses to keep up with the player.")]
    public float followLine;

    [Header("Debug Variables")]
    public Color followLineColor = Color.green; // Default color if not set in editor inspector.

    private Transform playerRef;
    private Vector3 followTarget;
    private Vector3 lastTargetPosition;
    private Vector3 currentVelocity;
    private float lastPlayerPosition;

    private const float Z_POS = 10f;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerPosition = playerRef.position.y;
        followTarget = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, followLine, Z_POS));
        Catformer.PlayerScript script = playerRef.GetComponent<Catformer.PlayerScript>();
        if (script != null)
            script.AddDeathListener(OnPlayerDeathTrigger);
    }

    private void Update()
    {
        followTarget = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, followLine, Z_POS));

        if (playerRef != null && !playerRef.gameObject.GetComponent<Catformer.PlayerScript>().isDead && playerRef.position.y >= followTarget.y && (playerRef.position.y - lastPlayerPosition) >= 0)
        {
            Vector3 newPos = Vector3.SmoothDamp(new Vector3(0f, transform.position.y, -Z_POS), new Vector3(0f, playerRef.position.y, -Z_POS), ref currentVelocity, damping);
            transform.position = newPos;
        }

        if(playerRef != null)
            lastPlayerPosition = playerRef.position.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = followLineColor;
        Camera camera = GetComponent<Camera>();
        Gizmos.DrawLine(camera.ViewportToWorldPoint(new Vector3(0f, followLine, 10f)), camera.ViewportToWorldPoint(new Vector3(1f, followLine, 10f)));
    }

    public Vector3 getVelocity()
    {
        return currentVelocity;
    }

    public void OnPlayerDeathTrigger()
    {
        playerRef = null;
        currentVelocity = Vector3.zero;
    }
}
