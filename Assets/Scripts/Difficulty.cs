using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour {

    float secondsToMaxDifficulty = 300;
    public float gameHasStartedTime;

    void Update()
    {        
        if (FindObjectOfType<GameManage>().hasStarted) 
        {
            gameHasStartedTime += Time.deltaTime;
        }
    }

    public float GetDifficultyPercent() {
        return Mathf.Clamp01(gameHasStartedTime / secondsToMaxDifficulty);       
    }

}