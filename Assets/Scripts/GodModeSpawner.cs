﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeSpawner : MonoBehaviour
{

    public GameObject godModePrefab;

    public Vector2 secondsBetweenSpawnsMinMax;
    public Vector2 spawnSizeMinMax;

    float nextSpawnTime;
    
    public float spawnAngleMax;

    Vector2 screenHalfSizeWorldUnits;

    // Start is called before the first frame update
    void Start()
    {
        screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<GameManage>().isPaused && FindObjectOfType<GameManage>().hasStarted && FindObjectOfType<Difficulty>().GetDifficultyPercent() > 0.07f && !FindObjectOfType<PlayerController>().multiplying)
        {
            if (Time.time > nextSpawnTime)
            {
                float secondsBetweenSpawns = Mathf.Lerp(secondsBetweenSpawnsMinMax.y, secondsBetweenSpawnsMinMax.x, FindObjectOfType<Difficulty>().GetDifficultyPercent());
                nextSpawnTime = Time.time + secondsBetweenSpawns;

                float spawnAngle = Random.Range(-spawnAngleMax, spawnAngleMax);
                float spawnSize = Random.Range(spawnSizeMinMax.x, spawnSizeMinMax.y);
                Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x), screenHalfSizeWorldUnits.y + spawnSize);
                GameObject newGodMode = (GameObject)Instantiate(godModePrefab, spawnPosition, Quaternion.Euler(Vector3.forward * spawnAngle));
                newGodMode.transform.localScale = Vector2.one * spawnSize;
            }
        }         
        
    }
}
