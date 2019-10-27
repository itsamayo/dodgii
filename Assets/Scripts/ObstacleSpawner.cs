using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject obstaclePrefab;

    public Vector2 secondsBetweenSpawnsMinMax;
    public Vector2 spawnSizeMinMax;

    float nextSpawnTime;
    
    public float spawnAngleMax;

    Vector2 screenHalfSizeWorldUnits;

    Renderer obstacleRender;

    public Color obstacleStartColor;
    public Color godModeEnabled;
    public Color multiplierEnabled;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        obstacleRender = obstaclePrefab.GetComponent<Renderer>();
        obstacleRender.sharedMaterial.color = obstacleStartColor;
    }

    // Update is called once per frame
    void Update()
    {        

        if (!FindObjectOfType<GameManage>().isPaused && FindObjectOfType<GameManage>().hasStarted) 
        {
            if (FindObjectOfType<PlayerController>() != null && FindObjectOfType<PlayerController>().godMode)
            {
                obstacleRender.sharedMaterial.color = godModeEnabled;
            }
            else if (FindObjectOfType<PlayerController>() != null && FindObjectOfType<PlayerController>().multiplying) 
            {
                obstacleRender.sharedMaterial.color = multiplierEnabled;
            }
            else
            {
                obstacleRender.sharedMaterial.color = obstacleStartColor;
            }

            if (Time.time > nextSpawnTime)
            {
                float secondsBetweenSpawns = Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, FindObjectOfType<Difficulty>().GetDifficultyPercent());
                nextSpawnTime = Time.time + secondsBetweenSpawns;

                float spawnAngle = Random.Range(-spawnAngleMax, spawnAngleMax);
                float spawnSize = Random.Range(spawnSizeMinMax.x, spawnSizeMinMax.y);
                Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x), screenHalfSizeWorldUnits.y + spawnSize);
                GameObject newObstacle = (GameObject)Instantiate(obstaclePrefab, spawnPosition, Quaternion.Euler(Vector3.forward * spawnAngle));
                newObstacle.transform.localScale = Vector2.one * spawnSize;
            }
        }         
        
    }
}
