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
    public float totalObstaclesDestroyed;

    // Start is called before the first frame update
    void Start()
    {        
        highScore = PlayerPrefs.GetFloat("highScore", highScore);
        coinsCollected = PlayerPrefs.GetFloat("coinsCollected", coinsCollected);
        totalCoinsCollected = PlayerPrefs.GetFloat("totalCoinsCollected", totalCoinsCollected);
        biggerCoinsCollected = PlayerPrefs.GetFloat("biggerCoinsCollected", biggerCoinsCollected);
        totalBiggerCoinsCollected = PlayerPrefs.GetFloat("totalBiggerCoinsCollected", totalBiggerCoinsCollected);
        godModesCollected = PlayerPrefs.GetFloat("godModesCollected", godModesCollected);
        totalGodModesCollected = PlayerPrefs.GetFloat("totalGodModesCollected", totalGodModesCollected);
        totalObstaclesDestroyed = PlayerPrefs.GetFloat("totalObstaclesDestroyed", totalObstaclesDestroyed);
        // For resetting highscore
        //PlayerPrefs.SetFloat("highScore", 0);
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

        // Check for achievements
        AchievementCheck();

        highScoreUI.text = "Best: " + highScore.ToString();
    }

    void AchievementCheck()
    {
        // Score achievements
        if (highScore >= 100)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_noob);
        }
        else if (highScore >= 500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_amateur);
        }
        else if (highScore >= 900)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_semipro);
        }
        else if (highScore >= 1500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_professional);
        }
        else if (highScore >= 2000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_master);
        }
        else if (highScore >= 3500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_grand_master);
        }

        // Coin collector/horder achievements
        if (coinsCollected >= 40)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_collector);
        }
        if (coinsCollected >= 100)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_horder);
        }
        if (totalCoinsCollected >= 3000)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_coin_connoisseur);
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
        if (totalBiggerCoinsCollected >= 1500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_gem_connoisseur);
        }

        // Godmode collector/horder achievements
        if (godModesCollected >= 15)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_buff_collector);
        }
        if (godModesCollected >= 30)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_buff_horder);
        }
        if (totalGodModesCollected >= 500)
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_buff_connoisseur);
        }

        // Obstacle destroyer achievements
        if (totalObstaclesDestroyed >= 1000 )
        {
            GooglePlayServices.instance.UnlockAchievement(GPGSIds.achievement_obstacles_worst_nightmare);
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
            PlayerPrefs.SetFloat("coinsCollected", coinsCollected);
            PlayerPrefs.SetFloat("totalCoinsCollected", totalCoinsCollected);
        }
        else if (collectable == "biggerCoin") 
        {
            biggerCoinsCollected += 1;
            totalBiggerCoinsCollected += 1;
            PlayerPrefs.SetFloat("biggerCoinsCollected", biggerCoinsCollected);
            PlayerPrefs.SetFloat("totalBiggerCoinsCollected", totalBiggerCoinsCollected);
        }
        else if (collectable == "godMode")
        {
            godModesCollected += 1;
            totalGodModesCollected += 1;
            PlayerPrefs.SetFloat("godModesCollected", godModesCollected);
            PlayerPrefs.SetFloat("totalGodModesCollected", totalGodModesCollected);
        }
        else if (collectable == "obstacle")
        {
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
