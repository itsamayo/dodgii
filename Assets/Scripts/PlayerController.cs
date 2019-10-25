using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed = 0f;
    public float maxSpeed = 7f;    
    public float acceleration = 7f;
    public float deceleration = 7f;
    public float turnTilt = 15f;    

    public bool godMode;
    public bool slomo;

    public event System.Action OnPlayerDeath;

    float screenHalfWidthInWorldUnits;
    float godModeTimer;

    Color colorStart;
    Color colorGodMode = Color.cyan;

    public Text godModeDisplayerUI;

    public GameObject godModeDisplayer;
    public GameObject coinParticleEffect;
    public GameObject biggerCoinParticleEffect;
    public GameObject godModeParticleEffect;
    public GameObject obstacleParticleEffect;

    Renderer playerRenderer;

    // Start is called before the first frame update
    void Start()
    {        
        float halfPlayerWidth = transform.localScale.x / 2f;
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize + halfPlayerWidth;
        playerRenderer = GetComponent<Renderer> ();
        colorStart = playerRenderer.material.color;

    }

    // Update is called once per frame
    void Update()
    {

        if (!FindObjectOfType<GameManage>().isPaused && FindObjectOfType<GameManage>().hasStarted) {
            //MovePlayer();
            Cursor.visible = false;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;

            if (godMode)
            {
                godModeDisplayerUI.text = "you're invincible " + godModeTimer.ToString();
            }
            
            if (godModeTimer == 0)
            {
                StartCoroutine(FindObjectOfType<ScoreManager>().FadeTextToZeroAlpha(0.5f, godModeDisplayerUI));
                playerRenderer.material.color = colorStart;                
                godMode = false;
            }
        }       

    }

    IEnumerator EnableGodMode()
    {
        godMode = true;
        playerRenderer.material.color = colorGodMode;
        StartCoroutine(FindObjectOfType<ScoreManager>().FadeTextToFullAlpha(0.5f, godModeDisplayerUI));
        for (int i = 0; i < 8; i++) 
        {
            yield return new WaitForSeconds(1);
            godModeTimer -= 1;
        }        
    }

    IEnumerator DestroyParticleEffect(GameObject particleSystem)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(particleSystem);
    }
    
    void MovePlayer() {
       
        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX == -1 && speed < maxSpeed)
        {
            speed = speed - acceleration * Time.deltaTime;
        }
        else if (inputX == 1 && speed > -maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime;
        }
        else
        {
            if (speed > deceleration * Time.deltaTime)
            {
                speed = speed - deceleration * Time.deltaTime;
            }
            else if (speed < -deceleration * Time.deltaTime)
            {
                speed = speed + deceleration * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }

        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        transform.eulerAngles = new Vector2(0, inputX * 25);

        if (transform.position.x < -screenHalfWidthInWorldUnits)
        {
            transform.position = new Vector2(screenHalfWidthInWorldUnits, transform.position.y);
        }

        if (transform.position.x > screenHalfWidthInWorldUnits)
        {
            transform.position = new Vector2(-screenHalfWidthInWorldUnits, transform.position.y);
        }

    }

     void OnTriggerEnter2D(Collider2D triggerCollider) 
    {
        if(triggerCollider.tag == "Obstacle")
        {
            if (!godMode)
            {
                if (OnPlayerDeath != null)
                {
                    OnPlayerDeath();
                }
                Destroy(gameObject);
            }
            else 
            {
                Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
                GameObject newParticleEffect = (GameObject)Instantiate(obstacleParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
                FindObjectOfType<CameraShake>().Shake(0.08f, 0.13f);
                FindObjectOfType<ScoreManager>().AddToScore(10, "obstacle");
                FindObjectOfType<SoundManager>().PlayObstacleDestroyedSound();
                Destroy(triggerCollider.gameObject);
                StartCoroutine(DestroyParticleEffect(newParticleEffect));
                GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_destroyer_of_obstacles);
            }           
            
        }

        if (triggerCollider.tag == "Coin")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(coinParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.03f, 0.1f);
            FindObjectOfType<ScoreManager>().AddToScore(3, "coin");
            FindObjectOfType<SoundManager>().PlayCoinCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect));
        }

        if (triggerCollider.tag == "BiggerCoin")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(biggerCoinParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.06f, 0.1f);
            FindObjectOfType<ScoreManager>().AddToScore(5, "biggerCoin");
            FindObjectOfType<SoundManager>().PlayBiggerCoinCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect));
        }

        if (triggerCollider.tag == "GodMode")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(godModeParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.1f, 0.2f);
            FindObjectOfType<ScoreManager>().AddToScore(20, "godMode");
            godModeTimer = 7;
            StartCoroutine(EnableGodMode());
            FindObjectOfType<SoundManager>().PlayGodModeCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect));
        }

    }
}
