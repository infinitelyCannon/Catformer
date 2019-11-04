using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catformer;

public class SpawnHazard : MonoBehaviour
{
    public GameObject[] Hazards;
    private string[] spawnSide = { "left", "right" };
    private string side;
    private int selectSide;
    public Vector3 spawnerPosition;
    public GameObject player;
    public GameObject hazardWarningLeft;
    public GameObject hazardWarningRight;
    public int selectHazard;
    public float timeLeft = 5;
    public float planeSpawnNow;
    float initialTimeLeft;
    void Start()
    {
        selectSide = 0;
        side = spawnSide[selectSide];
        selectHazard = 0;
        spawnerPosition = player.transform.GetChild(0).position;
        initialTimeLeft = timeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        spawnerPosition = player.transform.GetChild(0).position;

        if (timeLeft < 2 && timeLeft >= 0.5 && side == "left")
        {
            hazardWarningLeft.SetActive(true);
        }
        else
        {
            hazardWarningLeft.SetActive(false);
        }
        if (timeLeft < 2 && timeLeft >= 0.5 && side == "right")
        {
            hazardWarningRight.SetActive(true);
        }
        else
        {
            hazardWarningRight.SetActive(false);
        }

        if (timeLeft <= 0 && side == "left")
        {

            Instantiate(Hazards[selectHazard], spawnerPosition + new Vector3(0, Random.Range(0, 10), 0), Quaternion.identity);
            timeLeft = initialTimeLeft;
            side = spawnSide[Random.Range(0, spawnSide.Length)];
        }
        else if (timeLeft <= 0 && side == "right")
        {
            GameObject spawnRight = Instantiate(Hazards[selectHazard],spawnerPosition + new Vector3 (-spawnerPosition.x * 2,Random.Range(0,10),0), Quaternion.identity);
            spawnRight.GetComponent<Hazard>().speed *= -1;
            spawnRight.GetComponent<SpriteRenderer>().flipX = false;
            timeLeft = initialTimeLeft;
            side = spawnSide[Random.Range(0, spawnSide.Length)];
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (spawnerPosition.y >= planeSpawnNow)
            {
                selectHazard = 1;
                initialTimeLeft = 3;
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                if (spawnerPosition.y >= planeSpawnNow)
                {
                    selectHazard = 1;
                    initialTimeLeft = 3;
                }
            }
        }

    }
     
}
