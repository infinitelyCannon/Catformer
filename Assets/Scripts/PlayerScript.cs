using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Catformer
{
    public class PlayerScript : MonoBehaviour
    {
        Camera cameraRef;
        SpriteRenderer mRenderer;
        public Vector3 targetMove;
        public Vector3 prevMove;
        public float speed;
        public GameObject player;
        public Collider2D hitPlatform;
        public AudioSource jumpAudio;
        public AudioSource hurtAudio;
        public Animator catAnimator;
        public bool canJump;
        public int pointsPerPlatform;

        GameObject targetPlat;
        [HideInInspector]
        public bool isDead = false;
        float playTime = 0;

        public float touchRadius;

        private int platformMask;
        private float score = 0;

        public GameObject deathScreen;
        public Text scoreText;

        // Start is called before the first frame update
        void Start()
        {
            targetPlat = gameObject;
            cameraRef = Camera.main;
            mRenderer = GetComponent<SpriteRenderer>();
            platformMask = LayerMask.GetMask("Platforms");
            canJump = true;
            float halfWidth = mRenderer.sprite.bounds.extents.x;
            Vector3 rightEdge = cameraRef.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, 10f));
            Vector3 leftEdge = cameraRef.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f));
            Vector3 leftSpot = leftEdge + new Vector3(halfWidth, 0f, 0f);
            Vector3 rightSpot = rightEdge - new Vector3(halfWidth, 0f, 0f);
            if (transform.position.x < leftSpot.x && transform.position.x < cameraRef.transform.position.x)
                transform.position = new Vector3(leftSpot.x, transform.position.y, 0f);
            else if (transform.position.x > rightSpot.x && transform.position.x > cameraRef.transform.position.x)
                transform.position = new Vector3(rightSpot.x, transform.position.y, 0f);

            targetMove = transform.position;

            // Insert the pause and death screens if they don't exist.
            if (deathScreen == null && scoreText == null)
            {
                GameObject screen = (GameObject) Instantiate(Resources.Load("GameHUD", typeof(GameObject)), Vector3.zero, Quaternion.identity);
                GameObject text =  (GameObject) Instantiate(Resources.Load("EventSystem", typeof(GameObject)), Vector3.zero, Quaternion.identity);

                deathScreen = screen.transform.GetChild(2).gameObject; //GameObject.Find("DeathOverlay");
                scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
            }

            AudioSource[] sources = GetComponents<AudioSource>();
            foreach(AudioSource source in sources)
            {
                if(Catformer.SavedPreferences.instance != null)
                    source.volume = Catformer.SavedPreferences.instance.soundVolume;
            }

            if (pointsPerPlatform == 0)
                pointsPerPlatform = 20;
        }

        private void OnBecameInvisible()
        {
            System.TimeSpan secs = System.TimeSpan.FromSeconds(playTime);
            if (deathScreen == null)
                return;
            Text deathText = deathScreen.GetComponentInChildren<Text>();

            isDead = true;
            hurtAudio.Play();
            deathScreen.SetActive(true);
            deathText.text = "Game Over\nฅ(＾x ω x＾ฅ)∫\nElevation: " + string.Format("{0:n2}", score) + "ft" + "\nTotal Time: " + secs.ToString(@"hh\:mm\:ss\:fff");
        }

        private void OnGUI()
        {

        }



        // Update is called once per frame
        void Update()
        {
            if (!isDead && Time.timeScale > 0)
            {
                if (targetPlat != null && targetPlat.transform.childCount > 1)
                    targetMove = targetPlat.transform.GetChild(0).position;
                if (targetPlat == null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, -50f, 10f), speed * Time.deltaTime);
                    canJump = false;
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, targetMove, speed * Time.deltaTime);

                // transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;

                if (transform.parent != null)
                {
                    transform.localPosition = Vector3.zero;
                }

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.touches[0];

                    if (touch.phase == TouchPhase.Began)
                    {

                        //RaycastHit2D hit = Physics2D.BoxCast(cameraRef.ScreenToWorldPoint(touch.position), new Vector2(0.01f, 0.01f), 0, Vector2.zero);
                        Collider2D hit = Physics2D.OverlapCircle(cameraRef.ScreenToWorldPoint(touch.position), touchRadius, platformMask);
                        if (canJump && hit.gameObject == targetPlat)
                            goto MoveOn;
                        if (hit != null && canJump == true)
                        {
                            if (hit.gameObject.GetComponent<Platform>() != null && hit.gameObject.GetComponent<Platform>().isGood)
                            {
                                //hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                                targetMove = hit.transform.GetChild(0).position;
                                targetPlat = hit.gameObject;
                                //transform.position = targetMove;
                                //speed = hit.gameObject.GetComponent<Platform>().speed;
                                //hit = hitPlatform;
                                jumpAudio.Play();
                                catAnimator.SetBool("isJumping", true);

                                /*if (hit.gameObject.GetComponent<Platform>().transform.position.x < 0.0f)
                                {
                                    mRenderer.flipX = true;
                                    //Debug.Log("should flip");
                                }
                                else if (hit.gameObject.GetComponent<Platform>().transform.position.x > -0.0f)
                                {
                                    mRenderer.flipX = false;
                                    //Debug.Log("should flip");
                                }*/

                                canJump = false;
                            }
                            /*else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                            {
                                hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                            }*/
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {

                    Collider2D hit = Physics2D.OverlapCircle(cameraRef.ScreenToWorldPoint(Input.mousePosition), touchRadius, platformMask);
                    if (canJump && hit != null && hit.gameObject == targetPlat)
                        goto MoveOn;
                    if (hit != null && canJump == true)
                    {
                        if (hit.gameObject.GetComponent<Platform>() != null && hit.gameObject.GetComponent<Platform>().isGood)
                        {
                            //hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
                            targetMove = hit.transform.GetChild(0).position;
                            targetPlat = hit.gameObject;
                            //transform.position = targetMove;
                            jumpAudio.Play();
                            catAnimator.SetBool("isJumping", true);
                            //speed = hit.gameObject.GetComponent<Platform>().speed;
                        }
                        /*else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                        {
                            hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
                        }*/

                        canJump = false;
                    }
                }
            MoveOn:
                playTime += Time.deltaTime;
                scoreText.text = "Elevation: " + string.Format("{0:n2}", score) + "ft";
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == targetPlat)
            {
                catAnimator.SetBool("isJumping", false);
                mRenderer.flipX = (targetMove.x < 0.0f);

                if (targetMove.x < 0.0f)
                {
                    mRenderer.flipX = true;
                    Debug.Log("should flip");
                }
                if (targetMove.x >= 0.0f)
                {
                    mRenderer.flipX = false;
                    Debug.Log("should flip");
                }

                canJump = true;
            }

        }

        public GameObject GetTarget()
        {
            return targetPlat;
        }

        public float GetScore()
        {
            return score;
        }

        public void AddToScore(float value)
        {
            score += value;
        }

        private string TrimFloat(float value, int decimalPlaces)
        {
            string result = value.ToString();
            int dot = result.IndexOf('.');
            if ( dot == -1 || dot + decimalPlaces + 1 >= result.Length)
                return result;

            return result.Remove(dot + decimalPlaces + 1);//5.555
        }
    }
}
