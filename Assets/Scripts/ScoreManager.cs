using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text currentScoreUI;
    public Text highScoreUI;
    public Text pointDisplayerUI;
    public Text pointMessageUI;

    bool hasDisplayedNewHighscore;

    public GameObject scoreUI;
    public GameObject pointDisplayer;

    public float score;
    public float scoreMultiplier = 0.5f;
    public float highScore;
    public float coinsCollected;
    public float totalCoinsCollected;
    public float biggerCoinsCollected;
    public float totalBiggerCoinsCollected;
    public float godModesCollected;
    public float totalGodModesCollected;
    public float jewelsCollected;
    public float totalJewelsCollected;
    public float obstaclesDestroyed;
    public float totalObstaclesDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("highScore", highScore);
        totalCoinsCollected = PlayerPrefs.GetFloat("totalCoinsCollected", totalCoinsCollected);        
        totalBiggerCoinsCollected = PlayerPrefs.GetFloat("totalBiggerCoinsCollected", totalBiggerCoinsCollected);        
        totalGodModesCollected = PlayerPrefs.GetFloat("totalGodModesCollected", totalGodModesCollected);
        totalObstaclesDestroyed = PlayerPrefs.GetFloat("totalObstaclesDestroyed", totalObstaclesDestroyed);
        totalJewelsCollected = PlayerPrefs.GetFloat("totalJewelsCollected", totalJewelsCollected);

        ScoreAchievementCheck(highScore);
        TotalCollectorAchievementCheck();
    }

    // Update is called once per frame
    void Update()
    {
        currentScoreUI.text = "Score: " + score.ToString();
        PlayerPrefs.SetFloat("score", score);        
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetFloat("highScore", highScore);            
            if (!hasDisplayedNewHighscore) 
            {
                StartCoroutine(ShowMessage("new highscore!"));
                hasDisplayedNewHighscore = true;
            }           
        }       

        highScoreUI.text = "Best: " + highScore.ToString();
    }

    public void CurrentGameAchievementsCheck()
    {
        // Coin collector/horder achievements
        if (coinsCollected >= 40)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_collector);
        }
        if (coinsCollected >= 100)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_horder);
        }

        // Bigger Coin collector/horder achievements
        if (biggerCoinsCollected >= 20)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_gem_collector);
        }
        if (biggerCoinsCollected >= 60)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_gem_horder);
        }

        // Invincibility collector/horder achievements
        if (godModesCollected >= 15)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_invincibility_collector);
        }
        if (godModesCollected >= 30)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_invincibility_horder);
        }

        // Multiplier collector/horder achievements
        if (jewelsCollected >= 15)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_invincibility_collector);
        }
        if (jewelsCollected >= 30)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_invincibility_horder);
        }

    }

    public void TotalCollectorAchievementCheck()
    {
        // Coin connoisseur
        if (totalCoinsCollected >= 3000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_connoisseur);
        }

        // Gem connoisseur
        if (totalBiggerCoinsCollected >= 1500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_gem_connoisseur);
        }

        // Invincibility connoiseur
        if (totalGodModesCollected >= 500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_invincibility_connoisseur);
        }

        // Multiplier connoiseur
        if (totalJewelsCollected >= 500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_multiplier_connoisseur);
        }

        // Obstacle's worst nightmare
        if (totalObstaclesDestroyed >= 1000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_obstacles_worst_nightmare);
        }
    }

    public void ScoreAchievementCheck(float score)
    {
        // Score achievements
        if (score >= 100)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_noob);
        }
        else if (score >= 500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_amateur);
        }
        else if (score >= 900)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_semipro);
        }
        else if (score >= 1500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_professional);
        }
        else if (score >= 2000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_master);
        }
        else if (score >= 5000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_grand_master);
        }        
    }

    // Add to score and set collect counts
    public void AddToScore(float points, string collectable) 
    {
        score += points;
        StartCoroutine(ShowPoints(points));
        if (collectable == "coin")
        {
            coinsCollected += 1;
            totalCoinsCollected += 1;
            PlayerPrefs.SetFloat("totalCoinsCollected", totalCoinsCollected);
        }
        else if (collectable == "biggerCoin") 
        {
            biggerCoinsCollected += 1;
            totalBiggerCoinsCollected += 1;
            PlayerPrefs.SetFloat("totalBiggerCoinsCollected", totalBiggerCoinsCollected);
        }
        else if (collectable == "godMode")
        {
            godModesCollected += 1;
            totalGodModesCollected += 1;
            PlayerPrefs.SetFloat("totalGodModesCollected", totalGodModesCollected);
        }
        else if (collectable == "jewel")
        {
            jewelsCollected += 1;
            totalJewelsCollected += 1;
            PlayerPrefs.SetFloat("totalJewelsCollected", totalJewelsCollected);
        }
        else if (collectable == "obstacle")
        {
            obstaclesDestroyed += 1;
            totalObstaclesDestroyed += 1;
            PlayerPrefs.SetFloat("totalObstaclesDestroyed", totalObstaclesDestroyed);
        }
    }

    public IEnumerator ShowMessage(string message) 
    {
        StartCoroutine(FadeTextToFullAlpha(0.5f, pointMessageUI));
        pointMessageUI.text = message;
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeTextToZeroAlpha(0.5f, pointMessageUI));
    }

    public IEnumerator ShowPoints(float points) 
    {
        StartCoroutine(FadeTextToFullAlpha(0.5f, pointDisplayerUI));
        pointDisplayerUI.text = "+" + points.ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeTextToZeroAlpha(0.5f, pointDisplayerUI));
    }   

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
