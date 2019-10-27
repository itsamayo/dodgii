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
    public bool multiplying;

    public event System.Action OnPlayerDeath;

    float screenHalfWidthInWorldUnits;
    float godModeTimer;
    float multiplierTimer;
    float multiplier = 1.0f;

    Color colorStart;
    Color colorGodMode = Color.cyan;
    Color colorMultiplying = Color.magenta;


    public Text godModeDisplayerUI;
    public Text multiplierDisplayerUI;

    public GameObject godModeDisplayer;
    public GameObject multiplierDisplayer;
    public GameObject coinParticleEffect;
    public GameObject biggerCoinParticleEffect;
    public GameObject godModeParticleEffect;
    public GameObject jewelParticleEffect;
    public GameObject godModeObstacleParticleEffect;
    public GameObject multiplyingObstacleParticleEffect;

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

            if (multiplying)
            {
                multiplierDisplayerUI.text = "x" + multiplier.ToString() + " point multiplyer " + multiplierTimer.ToString();
            }

            if (godModeTimer == 0)
            {
                StartCoroutine(FindObjectOfType<ScoreManager>().FadeTextToZeroAlpha(0.5f, godModeDisplayerUI));
                playerRenderer.material.color = colorStart;                
                godMode = false;
            }

            if (multiplierTimer == 0)
            {
                StartCoroutine(FindObjectOfType<ScoreManager>().FadeTextToZeroAlpha(0.5f, multiplierDisplayerUI));
                playerRenderer.material.color = colorStart;
                multiplying = false;
                multiplier = 1.0f;
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

    IEnumerator EnableMultiplier()
    {
        multiplying = true;
        multiplier = Random.Range(2, 5);
        playerRenderer.material.color = colorMultiplying;
        StartCoroutine(FindObjectOfType<ScoreManager>().FadeTextToFullAlpha(0.5f, multiplierDisplayerUI));
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(1);
            multiplierTimer -= 1;
        }
    }

    IEnumerator DestroyParticleEffect(GameObject particleSystem, float time)
    {
        yield return new WaitForSeconds(time);
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
            if (!godMode && !multiplying)
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
                GameObject newParticleEffect;
                if (godMode)
                {
                    newParticleEffect = (GameObject)Instantiate(godModeObstacleParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
                }
                else 
                {
                    newParticleEffect = (GameObject)Instantiate(multiplyingObstacleParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
                }                
                FindObjectOfType<CameraShake>().Shake(0.08f, 0.13f);
                FindObjectOfType<ScoreManager>().AddToScore(10 * multiplier, "obstacle");
                FindObjectOfType<SoundManager>().PlayObstacleDestroyedSound();
                Destroy(triggerCollider.gameObject);
                StartCoroutine(DestroyParticleEffect(newParticleEffect,  0.5f));
                GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_destroyer_of_obstacles);
            }           
            
        }

        if (triggerCollider.tag == "Coin")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(coinParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.03f, 0.1f);
            FindObjectOfType<ScoreManager>().AddToScore(3 * multiplier, "coin");
            FindObjectOfType<SoundManager>().PlayCoinCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect, 0.5f));
        }

        if (triggerCollider.tag == "BiggerCoin")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(biggerCoinParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.06f, 0.1f);
            FindObjectOfType<ScoreManager>().AddToScore(5 * multiplier, "biggerCoin");
            FindObjectOfType<SoundManager>().PlayBiggerCoinCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect, 0.5f));
        }

        if (triggerCollider.tag == "GodMode")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(godModeParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.1f, 0.2f);
            FindObjectOfType<ScoreManager>().AddToScore(20 * multiplier, "godMode");
            godModeTimer = 7;
            StartCoroutine(EnableGodMode());
            FindObjectOfType<SoundManager>().PlayGodModeCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect, 0.5f));
        }

        if (triggerCollider.tag == "Jewel")
        {
            Vector2 spawnPosition = new Vector2(triggerCollider.transform.position.x, triggerCollider.transform.position.y);
            GameObject newParticleEffect = (GameObject)Instantiate(jewelParticleEffect, spawnPosition, Quaternion.Euler(Vector3.forward));
            FindObjectOfType<CameraShake>().Shake(0.1f, 0.2f);
            FindObjectOfType<ScoreManager>().AddToScore(250, "jewel");
            multiplierTimer = 10;
            StartCoroutine(EnableMultiplier());
            FindObjectOfType<SoundManager>().PlayBiggerCoinCollectSound();
            Destroy(triggerCollider.gameObject);
            StartCoroutine(DestroyParticleEffect(newParticleEffect, 0.5f));
        }

    }
}
