using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public enum Stage
    {
        Forest,
        Sky,
        Space
    };

    public float speed;

    private Vector3 size;
    private Camera mCamera;
    private SpriteRenderer mRenderer;
    private SpriteRenderer childRenderer;
    private Vector3 startPosition;
    [Range(-37.97551872f, 37.97551872f)]
    public float mTime = 0f;
    private Stage stage = Stage.Forest;
    private bool isParentAlone = true;
    private bool isChildAlone = false;
    private GameObject child;

    private const float REPEAT_OFFSET = 0.01f;

    float newSpot;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<SpriteRenderer>();
        mCamera = Camera.main;
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
        child = transform.GetChild(0).gameObject;
        childRenderer = child.GetComponent<SpriteRenderer>();
        child.GetComponent<BackgroundChild>().invisibilityTrigger = new BackgroundChild.InvisibilityTrigger(this.OnChildBecameInvisible);

        Resize();
        transform.position = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f)) + Vector3.up * size.y;
        mTime = size.y - REPEAT_OFFSET;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement: Find out where the camera is and if it's moving, loop the background around the camera.
        startPosition = mCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 10f));//.transform.position + new Vector3(0,0,10f);
        float cameraSpeed = mCamera.GetComponent<CameraController>().getVelocity().magnitude;
        //mTime += Time.deltaTime * cameraSpeed * -speed;
        newSpot = mTime % size.y; //Mathf.Repeat(mTime, size.y);
        transform.position = startPosition + Vector3.up * newSpot;

        isParentAlone = mRenderer.isVisible && !(mRenderer.isVisible && childRenderer.isVisible);
        isChildAlone = childRenderer.isVisible && !(mRenderer.isVisible && childRenderer.isVisible);
    }

    public void OnChildBecameInvisible()
    {
        Debug.Log("CHILD");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("PARENT");
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("Label"));
        style.fontSize = 62;
        GUI.Label(new Rect(500, 500, 700, 500), mTime.ToString(), style);
        GUI.Label(new Rect(500, 800, 700, 500), newSpot.ToString(), style);
    }

    void Resize()
    {
        Vector3 left = mCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 10f));
        Vector3 right = mCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f));
        float scale = Vector3.Distance(left, right) / size.x;

        transform.localScale = new Vector3(scale, scale, 1f);
        size = mRenderer.sprite.bounds.size * transform.localScale.x;
    }
}
